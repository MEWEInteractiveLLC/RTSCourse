using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Targeter : NetworkBehaviour
{
    private Targetable target;



    #region SERVER
    
    [Command]
    public void CmdSetTarget(GameObject targetGameObject)
    {
        // If the object doesnt have the targetable script the exit the code
        if (!targetGameObject.TryGetComponent(out Targetable newTarget))
        {
            return;
        }

        target = newTarget;
    }


    [Server]
    public void ClearTarget()
    {
        target = null;
    }
    

    #endregion
   
}
