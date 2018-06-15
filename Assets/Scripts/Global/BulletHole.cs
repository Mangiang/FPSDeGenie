using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletHole : NetworkBehaviour {
    public float timeout = 10; // Timeout in seconds before self deactivating
    [SyncVar]
    float timer = 0;
	
    [Server]
	void Update () {
        if (timer >= timeout)
        {
            RpcDeactivate();
        }		
        else
        {
            timer += Time.deltaTime;
        }
	}

    void RpcDeactivate()
    {
        timer = 0;
        GameManager.Singleton.UnSpawnBulletHole(gameObject);
        NetworkServer.UnSpawn(gameObject);
    }
}
