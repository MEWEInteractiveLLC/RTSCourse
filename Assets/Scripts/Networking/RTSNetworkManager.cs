using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class RTSNetworkManager : NetworkManager
{

   [SerializeField] private GameObject unitSpawnerPrefab = null;
   // ReSharper disable Unity.PerformanceAnalysis
   public override void OnServerAddPlayer(NetworkConnection conn)
   {
      base.OnServerAddPlayer(conn);

      var playerTransform = conn.identity.transform;
      GameObject unitSpawnerInstance = Instantiate(unitSpawnerPrefab, playerTransform.position, playerTransform.rotation);

      NetworkServer.Spawn(unitSpawnerInstance, conn);



   }
}
