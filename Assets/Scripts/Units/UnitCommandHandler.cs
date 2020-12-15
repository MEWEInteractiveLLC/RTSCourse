using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitCommandHandler : MonoBehaviour
{
    [SerializeField] private UnitSelectionHandler unitSelectionHandler;
    [SerializeField] private LayerMask layerMask = new LayerMask();
    private Camera mainCamera;
    
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // CHECK TO SEE IF THE PLAYER HAS PRESSED THE MOUSE BUTTON THIS FRAME
        if (!Mouse.current.rightButton.wasPressedThisFrame)
        {
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        // IF WE DONT HIT A VALID OBJECT EXIT THE CODE
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            return;
        }


        if (hit.collider.TryGetComponent(out Targetable newTarget))
        {
            Debug.Log("Entering the check for Targetable Items");
            if (newTarget.hasAuthority)
            {
                TryMove(hit.point);
                Debug.Log("Trying to move");
                return;
            }

            TryToTarget(newTarget);
            Debug.Log("Trying to target the TARGET");
            return;
        }
        
        TryMove(hit.point);
        Debug.Log(" All failed, Trying to move");
       
        


    }

    private void TryToTarget(Targetable inTarget)
    {
        
        foreach (Unit unit in unitSelectionHandler.selectedUnits)
        {
            unit.GetTargeter.CmdSetTarget(inTarget.gameObject);
        }
    }


    private void TryMove(Vector3 hitInfoPoint)
    {
        
        foreach (Unit unit in unitSelectionHandler.selectedUnits)
        {
            unit.GetUnitMovement().CmdMove(hitInfoPoint);
        }
    }
}
