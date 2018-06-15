using UnityEngine;
using UnityEngine.Networking;

public class PlayerDatas : NetworkBehaviour
{
    public string _username = "";
    public int _level = 0;

    public int _maxHealth = 50;

    PlayerGui _gui;

    [SyncVar(hook = "OnChangeHealth")]
    public int _currentHealth; 


    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        _username = GameManager.Singleton.RegisterPlayer(this);
    }

    [Client]
    public void Init()
    {
        if (!isLocalPlayer)
            return;

        _gui = GetComponent<PlayerGui>();
        _gui.OnHealthChange(_currentHealth, _maxHealth);
    }

    private void OnDisable()
    {
        GameManager.Singleton.UnregisterPlayer(_username);
    }

    public void Damage(int dmg)
    {
        _currentHealth -= dmg;
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            RpcRespawn();
        }
    }

    [ClientRpc]
    void RpcRespawn()
    {
        GameObject spawn = GameManager.Singleton._spawns[Random.Range(0, GameManager.Singleton._spawns.Count)];
        transform.position = spawn.transform.position;
        transform.rotation = spawn.transform.rotation;
        _currentHealth = _maxHealth;
    }

    [Client]
    void OnChangeHealth(int health)
    {
        if (isLocalPlayer)
            _gui.OnHealthChange(health, _maxHealth);
    }

    [Command]
    public void CmdPlayerShot(string playerHit)
    {
        GameManager.Singleton._playerDict[playerHit].Damage(2);
    }
}
