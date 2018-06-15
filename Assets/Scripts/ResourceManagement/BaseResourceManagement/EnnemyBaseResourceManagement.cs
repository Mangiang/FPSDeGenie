using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnnemyBaseResourceManagement : BaseResourceManagement
{
    private EnnemyBaseManager _baseManager;

    protected override void Start()
    {
        base.Start();

        _baseManager = GetComponent<EnnemyBaseManager>();
    }

    public override void DumpResources(int load)
    {
        base.DumpResources(load);

        _baseManager.SetDestination(GameManager.Singleton.GetClosestResource(transform.position).gameObject);
    }

    protected override void OnGUI()
    {
    }
}
