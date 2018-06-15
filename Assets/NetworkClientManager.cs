using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class InputMessage : MessageBase
{
    public int connId;
    public float vertical;
    public float horizontal;
}
public class MyMsgType
{
    public static short Input = MsgType.Highest + 1;
};

public class NetworkClientManager : NetworkBehaviour
{
    NetworkClient myClient;
    GameObject _playerNetworkController;
    bool _isReady;

    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to server");

        ClientScene.Ready(netMsg.conn);
        foreach (var controller in netMsg.conn.playerControllers)
        {
            ClientScene.AddPlayer(controller.playerControllerId);
        }
        ClientScene.RegisterPrefab(_playerNetworkController);
        myClient.RegisterHandler(MsgType.Ready, _playerNetworkController.GetComponent<PlayerNetworkController>().OnRename);

        Debug.Log("Client Side : Client " + netMsg.conn.connectionId + " Connected!");
        _isReady = true;
    }

    public void ReadyMessage(NetworkMessage networkMessage)
    {
        Debug.Log("Client Ready! ");
        string newName = networkMessage.ReadMessage<StringMessage>().value;
        _playerNetworkController.GetComponent<PlayerNetworkController>().RpcRename(newName);
    }


    public void OnDisconnected(NetworkMessage netMsg)
    {
        Debug.Log("Disconnected from server");
    }

    public void OnError(NetworkMessage netMsg)
    {
        Debug.Log("Error connecting with code " + netMsg.conn.lastError);
    }

    public void Start()
    {
        myClient = new NetworkClient();

        _playerNetworkController = SpawnClient();
        _playerNetworkController.GetComponent<PlayerNetworkController>()._networkClientManager = this;
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.RegisterHandler(MsgType.Disconnect, OnDisconnected);
        myClient.RegisterHandler(MsgType.Error, OnError);

        myClient.Connect("127.0.0.1", 7777);
    }

    GameObject SpawnClient()
    {
        return Instantiate(Resources.Load("Game/Player"), transform) as GameObject;
    }

    public void SendInput(InputMessage msg)
    {
        if (!_isReady)
            return;
        msg.connId = myClient.connection.connectionId;
        print(msg.horizontal + " " + msg.vertical);
        myClient.Send(MyMsgType.Input, msg);
    }
}
