using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionHandler : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask = new LayerMask();
    [SerializeField] private RectTransform unitSelectionArea = null;

    private Vector2 startPosition;
    private RTSPlayer player;
    private Camera mainCamera;

    public List<Unit> selectedUnits { get;  } = new List<Unit>();

    private void Start()
    {
        mainCamera = Camera.main;
        Invoke(nameof(GetPlayer), 0.1f);
    }


    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
           StartSelectionArea();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ClearSelectionArea();
        }
        else if (Mouse.current.leftButton.isPressed)
        {
            UpdateSelectionArea();
            
        }
    }

    private void ClearSelectionArea()
    {

        unitSelectionArea.gameObject.SetActive(false);

        if (unitSelectionArea.sizeDelta.magnitude == 0)
        {
           
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            // CHECK TO SEE IF WE HIT ANYTHING 
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                return; 
            }
       

            // CHECK TO SEE IF THE THING WE HIT HAS A UNIT COMPONENT todo Change this to compare tags 
            if (!hit.collider.TryGetComponent<Unit>(out Unit unit))
            {
                return;
            }

            // CHECK IF WE OWN THE UNIT WE SELECTED
            if (!unit.hasAuthority)
            {
                return;
            }
        
            selectedUnits.Add(unit);

            foreach (var units in selectedUnits)
            {
                units.Select();
            }
            
            return;
        }

        Vector2 min = unitSelectionArea.anchoredPosition - (unitSelectionArea.sizeDelta / 2);
        Vector2 max = unitSelectionArea.anchoredPosition + (unitSelectionArea.sizeDelta / 2);

        foreach (var unit in player.GetMyUnits())
        {
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(unit.transform.position);

            if (screenPosition.x > min.x && screenPosition.x < max.x && screenPosition.y > min.y && screenPosition.y < max.y)
            {
                selectedUnits.Add(unit);
                unit.Select();
            }
        }


    }


    private void StartSelectionArea()
    {
        foreach (var selectedUnit in selectedUnits)
        {
            selectedUnit.Deselect();
        }
        selectedUnits.Clear();

        unitSelectionArea.gameObject.SetActive(true);

        startPosition = Mouse.current.position.ReadValue();
        
        UpdateSelectionArea();
    }
    private void UpdateSelectionArea()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        float areaWidth = mousePosition.x - startPosition.x;
        float areaHeight = mousePosition.y - startPosition.y;

        unitSelectionArea.sizeDelta = new Vector2(Mathf.Abs(areaWidth), Mathf.Abs(areaHeight));
        unitSelectionArea.anchoredPosition = startPosition + new Vector2(areaWidth / 2, areaHeight / 2);
    }

    private void GetPlayer()
    {
        player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
    }
}
