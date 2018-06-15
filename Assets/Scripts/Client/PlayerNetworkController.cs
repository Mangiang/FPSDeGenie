using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class PlayerNetworkController : NetworkBehaviour {

    public NetworkClientManager _networkClientManager;
    public List<InputMessage> msgs = new List<InputMessage>();

    public void RpcRename(string newName)
    {
        Debug.Log("Client rpc");
        gameObject.name = newName;
    }

    public void OnRename(NetworkMessage networkMessage)
    {
        string newName = networkMessage.ReadMessage<StringMessage>().value;
        RpcRename(newName);
    }

    public void Update()
    {
        if (_networkClientManager)
        {
            InputMessage msg = new InputMessage();
            msg.vertical = Input.GetAxis("Vertical");
            msg.horizontal = Input.GetAxis("Horizontal");
            msgs.Add(msg);
            _networkClientManager.SendInput(msg);
        }
    }

    public void FixedUpdate()
    {
        foreach (var message in msgs)
        {
            transform.position += new Vector3(message.horizontal * 1f * Time.deltaTime, 0, message.vertical * 1f * Time.deltaTime);
        }
        msgs.Clear();
    }
}
