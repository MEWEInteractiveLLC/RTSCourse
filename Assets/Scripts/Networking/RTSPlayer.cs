using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class RTSPlayer : NetworkBehaviour
{
    private List<Unit> myUnits = new List<Unit>();

    public List<Unit> GetMyUnits()
    {
        return myUnits;
    }
    
    

    #region SERVER

    
    public override void OnStartServer()
    {
        Unit.ServerOnUnitSpawned += ServerHandleUnitSpawned;
        Unit.ServerOnUnitDespawned += ServerHandleUnitDespawned;
        base.OnStartServer();
    }

    public override void OnStopServer()
    {
        Unit.ServerOnUnitSpawned -= ServerHandleUnitSpawned;
        Unit.ServerOnUnitDespawned -= ServerHandleUnitDespawned;
        base.OnStopServer();
    }


    private void ServerHandleUnitSpawned(Unit unit)
    {
        // CHECK IF THE UNIT SPAWNED BELONGS TO THE CLIENT THAT SPAWNED IT
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId)
        {
            return;
        }

        myUnits.Add(unit);
    }

    private void ServerHandleUnitDespawned(Unit unit)
    {
        // CHECK IF THE UNIT SPAWNED BELONGS TO THE CLIENT THAT SPAWNED IT
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId)
        {
            return;
        }

        myUnits.Remove(unit);
    }

    #endregion


    #region CLIENT

    public override void OnStartClient()
    {
        if (!isClientOnly)
        {
            return;
        }

        Unit.AuthorityOnUnitSpawned += AuthorityHandleUnitSpawned;
        Unit.AuthorityOnUnitDespawned += AuthorityHandleUnitDespawned;
    }

    public override void OnStopClient()
    {
        if (!isClientOnly)
        {
            return;
        }
        
        Unit.AuthorityOnUnitSpawned -= AuthorityHandleUnitSpawned;
        Unit.AuthorityOnUnitDespawned -= AuthorityHandleUnitDespawned;
    }


    private void AuthorityHandleUnitSpawned(Unit unit)
    {
        if (!hasAuthority)
        {
            return;
        }
        myUnits.Add(unit);
    }

    private void AuthorityHandleUnitDespawned(Unit unit)
    {
        if (!hasAuthority)
        {
            return;
        }
        myUnits.Remove(unit);
    }

    #endregion
}
