using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnnemyBaseManager : BaseManager  {
    protected override void InitRobot()
    {
        _robotObj = Instantiate(ResourcesData.GetObject(ResourcesData._robots, RobotsEnum.ENNEMYROBOT), transform) as GameObject;
        _robotObj.SetActive(false);

        SetDestination(GameManager.Singleton.GetClosestResource(transform.position).gameObject);
    }
}
