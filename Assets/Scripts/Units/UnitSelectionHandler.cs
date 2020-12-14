using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionHandler : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask = new LayerMask();
    private Camera mainCamera;

    private List<Unit> selectedUnits = new List<Unit>();

    private void Start()
    {
        mainCamera = Camera.main;
    }


    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            foreach (var selectedUnit in selectedUnits)
            {
                selectedUnit.Deselect();
                selectedUnits.Clear();
            }
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ClearSelectionArea();
        }
    }

    private void ClearSelectionArea()
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

       
    }
}
