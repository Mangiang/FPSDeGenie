using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseManager : MonoBehaviour  {
    protected List<RobotController> _robots = new List<RobotController>();

    protected int _robotNumber = 5;

    protected GameObject _robotObj;

    protected GameObject _dest;

    public GameObject Dest
    {
        get
        {
            return _dest;
        }

        set
        {
            _dest = value;
        }
    }

    protected virtual void InitRobot() {}

    public void InitRobots()
    {
        InitRobot();
        for (int i = 0; i < _robotNumber; ++i)
        {
            GameObject robot = Instantiate(_robotObj, transform.GetChild(0).position, transform.rotation);
            robot.SetActive(true);
            RobotController rc = robot.GetComponent<RobotController>();
            rc.SetBase(gameObject);
            _robots.Add(rc);
        }
    }

    public void SetDestination(GameObject dst)
    {
        _dest = dst;
        if (_dest != null)
        {
            foreach (var robot in _robots)
            {
                robot.SetDestination(_dest);
            }
        }
    }
}
