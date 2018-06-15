using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class ViewManager : MonoBehaviour {
    public bool MapMode = false;
    ShootScript shoot;
    Aiming aim;
    public Camera playerCam;
    public Camera playerCam2;
    public Camera mapCam;
    public Camera currentCamera;

    FirstPersonController controller;
    // Use this for initialization
    void Start () {
        shoot = transform.GetChild(0).GetComponent<ShootScript>();
        aim = transform.GetChild(0).GetChild(0).GetComponent<Aiming>();
        controller = GetComponent<FirstPersonController>();
        mapCam = GameObject.Find("MapCamera").GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("m"))
        {
            shoot.isMapMode = !shoot.isMapMode;
            aim.isMapMode = !aim.isMapMode;
            controller.isMapMode = !controller.isMapMode;
            MapMode = !MapMode;
        }

        if (MapMode)
        {
            playerCam.enabled = false;
            playerCam2.enabled = false;
            mapCam.enabled = true;
            currentCamera = mapCam;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            playerCam.enabled = true;
            playerCam2.enabled = true;
            mapCam.enabled = false;
            currentCamera = playerCam;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
	}
}
