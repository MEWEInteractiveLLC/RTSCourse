using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class Unit : NetworkBehaviour
{
      [Header("PROPERTIES")]
      [SerializeField] private UnitMovement unitMovement = null;
      [SerializeField] private Targeter targeter = null;

      [Header("EVENTS")]
      [SerializeField] private UnityEvent onSelected = null;
      [SerializeField] private UnityEvent onDeselected = null;
     
      public Targeter GetTargeter => targeter;

      #region EVENTS

      public static event Action<Unit> ServerOnUnitSpawned; 
      public static event Action<Unit> ServerOnUnitDespawned;

      public static event Action<Unit> AuthorityOnUnitSpawned; 
      public static event Action<Unit> AuthorityOnUnitDespawned;


      #endregion
    
      #region SERVER

      public override void OnStartServer()
      {
            ServerOnUnitSpawned?.Invoke(this);
            base.OnStartServer();
      }

      public override void OnStopServer()
      {
            ServerOnUnitDespawned?.Invoke(this);
            base.OnStopServer();
      }

      #endregion

      public UnitMovement GetUnitMovement()
      {
            return unitMovement;
      }

      #region CLIENT

      [Client]
      public void Select()
      {
            if (!hasAuthority)
            {
                  return;
            }
            onSelected?.Invoke();
      }

      [Client]
      public void Deselect()
      {
            if (!hasAuthority)
            {
                  return;
            }
            
            onDeselected?.Invoke();
      }


      public override void OnStartClient()
      {
            if (!isClientOnly || !hasAuthority)
            {
                  return;
            }
            
            AuthorityOnUnitSpawned?.Invoke(this);
            
      }

      public override void OnStopClient()
      {
            if (!isClientOnly || !hasAuthority)
            {
                  return;
            }
            
            AuthorityOnUnitDespawned?.Invoke(this);
            
      }

      #endregion
}
