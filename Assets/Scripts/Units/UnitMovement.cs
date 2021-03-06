﻿using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UnitMovement : NetworkBehaviour
{
   [SerializeField] private NavMeshAgent agent = null;

   [SerializeField] private Targeter targeter = null;
  


   #region Server

   [Command]
   public void CmdMove(Vector3 position)
   {
      targeter.ClearTarget();
      
      if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas ))
      {
         return;
      }

    

      agent.SetDestination(hit.position);
   }
   
   [ServerCallback]
   private void Update()
   {
      if (!agent.hasPath)
      {
         return;
      }
      
      if (agent.remainingDistance > agent.stoppingDistance)
      {
         return;
      }
      
      agent.ResetPath();
   }

   #endregion


   #region Client

  
  

   #endregion


}
