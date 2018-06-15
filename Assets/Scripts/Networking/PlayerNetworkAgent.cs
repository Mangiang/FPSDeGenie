using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerNetworkAgent : NetworkBehaviour
{
    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    void Start()
    {
        if (isLocalPlayer)
        {
            GetComponent<FirstPersonController>().enabled = true;
            GetComponent<CharacterController>().enabled = true;
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<ViewManager>().enabled = true;
            GetComponent<AudioSource>().enabled = true;
            GetComponent<PlayerGui>().enabled = true;
            transform.GetChild(0).GetComponent<ShootScript>().enabled = true;
            transform.GetChild(0).GetComponent<AudioListener>().enabled = true;
            GetComponent<PlayerGui>().enabled = true;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<PlayerDatas>().Init();
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
            gameObject.tag = "OtherPlayers";
            GetComponent<CapsuleCollider>().enabled = true;
        }
    }


    [Command]
    public void CmdSpawnBulletHole(Vector3 pos)
    {
        var bullet = GameManager.Singleton.GetBulletHole(pos);
        NetworkServer.Spawn(bullet, GameManager.Singleton._networkHashDict[SpawnableTypes.BULLET_HOLE]);
    }
}