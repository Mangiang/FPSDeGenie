using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;

public enum SpawnableTypes
{
    BULLET_HOLE
}


public class GameManager : NetworkBehaviour {
    [HideInInspector]
    public static GameManager Singleton;

    private float bulletHolesNumber = 100;

    [HideInInspector]
    public List<BulletHole> _lBulletHoles = new List<BulletHole>();
    [HideInInspector]
    public List<NodeResourceManagement> _lResources = new List<NodeResourceManagement>();
    [HideInInspector]
    public PlayerDatas _player;
    [HideInInspector]
    public List<PlayerDatas> _lAllPlayers = new List<PlayerDatas>();
    [HideInInspector]
    public AllyBaseManager _allyBaseManager;
    [HideInInspector]
    public EnnemyBaseManager _ennemyBaseManager;
    [HideInInspector]
    public List<GameObject> _spawns = new List<GameObject>();

    int playerCount = 0;

    public Dictionary<string, PlayerDatas> _playerDict = new Dictionary<string, PlayerDatas>();
    public Dictionary<SpawnableTypes, NetworkHash128> _networkHashDict = new Dictionary<SpawnableTypes, NetworkHash128>();


    public delegate GameObject SpawnDelegate(Vector3 position, NetworkHash128 assetId);
    public delegate void UnSpawnDelegate(GameObject spawned);


    public string RegisterPlayer(PlayerDatas playerDatas)
    {
        string id = "Player" + (playerCount++).ToString();
        _playerDict.Add(id, playerDatas);
        playerDatas.transform.name = id;
        return id;
    }

    public void UnregisterPlayer(string id)
    {
        _playerDict.Remove(id);
    }

    private void Awake()
    {
        Singleton = this;
    }

    public void Init()
    {
        ResourcesData.Init();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDatas>();
        List<GameObject> players = GameObject.FindGameObjectsWithTag("OtherPlayers").ToList();
        foreach (GameObject player in players)
        {
            _lAllPlayers.Add(player.GetComponent<PlayerDatas>());
        }

        _spawns = GameObject.FindGameObjectsWithTag("PlayerSpawns").ToList();

        InitBulletHoles();
        InitResourceNodes();
        InitBases();
    }

    private void InitBulletHoles()
    {
        Transform bulletHolesParent = transform.Find("BulletHoles"); 

        GameObject hole = Instantiate(ResourcesData.GetObject(ResourcesData._fx, FxEnum.BULLETHOLE), Vector3.zero, Quaternion.identity, bulletHolesParent) as GameObject;
        hole.SetActive(false);
        NetworkHash128 assetId = hole.GetComponent<NetworkIdentity>().assetId;
        _networkHashDict.Add(SpawnableTypes.BULLET_HOLE, assetId);

		for (int i = 0; i < bulletHolesNumber; i++)
        {
            hole = Instantiate(hole, Vector3.zero, Quaternion.identity, bulletHolesParent) as GameObject;
            hole.SetActive(false);
            _lBulletHoles.Add(hole.GetComponent<BulletHole>());
        }
        ClientScene.RegisterSpawnHandler(assetId, SpawnBulletHole, UnSpawnBulletHole);
    }

    public GameObject SpawnBulletHole(Vector3 position, NetworkHash128 assetId)
    {
        GameObject bulletHole = GetBulletHole(position);
        if (bulletHole != null)
            return bulletHole;

        Transform bulletHolesParent = transform.Find("BulletHoles");
        return Instantiate(ResourcesData.GetObject(ResourcesData._fx, FxEnum.BULLETHOLE), Vector3.zero, Quaternion.identity, bulletHolesParent) as GameObject;
    }
    public void UnSpawnBulletHole(GameObject spawned)
    {
        spawned.SetActive(false);
    }


    private void InitResourceNodes()
    {
        GameObject[] resourcesArr = GameObject.FindGameObjectsWithTag("ResourceNode");
        GameObject resourceObj = ResourcesData.GetObject(ResourcesData._resources, ResourcesEnum.RESOURCE) as GameObject; 
        foreach (var resource in resourcesArr)
        { 
            _lResources.Add(Instantiate(resourceObj, resource.transform.position, resource.transform.rotation).GetComponent<NodeResourceManagement>());
        }
    }

    private void InitBases()
    {
        GameObject baseSpawn = GameObject.FindGameObjectWithTag("AllyBaseSpawn");
        if (baseSpawn != null)
        {
            GameObject allyBaseObj = Instantiate(ResourcesData.GetObject(ResourcesData._bases, BasesEnum.BASE), baseSpawn.transform.position, baseSpawn.transform.rotation) as GameObject;
            _allyBaseManager = allyBaseObj.GetComponent<AllyBaseManager>();
            _allyBaseManager.InitRobots();
        }
        else
        {
            throw new System.Exception("Ally base spawn not found");
        }

        baseSpawn = GameObject.FindGameObjectWithTag("EnnemyBaseSpawn");
        if (baseSpawn != null)
        {
            GameObject ennemyBaseObj = Instantiate(ResourcesData.GetObject(ResourcesData._bases, BasesEnum.ENNEMYBASE), baseSpawn.transform.position, baseSpawn.transform.rotation) as GameObject;
            _ennemyBaseManager = ennemyBaseObj.GetComponent<EnnemyBaseManager>();
            _ennemyBaseManager.InitRobots();
        }
        else
        {
            throw new System.Exception("Ennemy base spawn not found");
        }
    }

    public ResourceManagement GetClosestResource(Vector3 pos)
    {
        ResourceManagement tmpResource = _lResources[0];
        float dist = int.MaxValue;

        foreach (var resource in _lResources)
        {
            float newDist = Vector3.Distance(pos, resource.transform.position);
            if (newDist < dist && resource._currentLoad > 0)
            {
                dist = newDist;
                tmpResource = resource;
            }
        }

        return tmpResource;
    }

    public GameObject GetBulletHole(Vector3 position)
    {
        GameObject hole = _lBulletHoles.Find(x => !x.gameObject.activeSelf).gameObject;
        if (hole != null)
        {
            hole.transform.position = position;
            hole.SetActive(true);
            return hole;
        }
        return hole;
    }
}