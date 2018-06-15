using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllyBaseManager : BaseManager
{
    private ViewManager _viewManager;

    protected override void InitRobot()
    {
        _robotObj = Instantiate(ResourcesData.GetObject(ResourcesData._robots, RobotsEnum.ALLYROBOT), transform) as GameObject;
        _robotObj.SetActive(false);
        _viewManager = GameObject.FindGameObjectWithTag("Player").GetComponent<ViewManager>();
    }

    private void OnGUI()
    {
		if (Input.GetMouseButton(0) && _viewManager.MapMode)
        {
            Ray ray = _viewManager.mapCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "Resource")
                {
                    _dest = hit.transform.gameObject;
                    SetDestination(_dest);
                }
            }
        }
    }
}
