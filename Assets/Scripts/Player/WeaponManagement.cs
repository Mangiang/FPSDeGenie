using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManagement : MonoBehaviour {
    [HideInInspector]
    public int ammosPerMag = 60;
    [HideInInspector]
    public int ammosInCurrentMag = 60;

    Text weaponAmmoText;

    private void OnEnable()
    {
        gameObject.layer = LayerMask.NameToLayer("MainWeapon");

        foreach(Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("MainWeapon");
        }
    }

    private void Start()
    {
        weaponAmmoText = (GameObject.Find("Canvas/WeaponAmmos") as GameObject).GetComponent<Text>();
    }

    private void OnGUI()
    {
        weaponAmmoText.text = string.Format("{0} / {1}", ammosInCurrentMag, ammosPerMag);
    }
}
