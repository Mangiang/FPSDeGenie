using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour {

    AudioSource audioSource;
    public AudioClip fireClip;
    public AudioClip reloadClip;
    float fireRate = 0.08f;
    float fireCount = 0;
    float shootDist = 100;
    public bool isMapMode = false;
    Aiming aim;
    WeaponManagement weaponManagement;
    PlayerDatas playerDatas;
    PlayerNetworkAgent playerNetAgent;

    void OnEnable() {
        fireCount = fireRate;
        audioSource = GetComponent<AudioSource>();
        aim = transform.GetChild(0).GetComponent<Aiming>();
        weaponManagement = transform.GetChild(0).GetChild(0).GetComponent<WeaponManagement>();
        GetComponent<AudioSource>().enabled = true;
        GetComponent<Camera>().enabled = true;
        transform.Find("ClipingCam").gameObject.SetActive(true);
        transform.Find("WeaponSlot").gameObject.GetComponent<Aiming>().enabled = true;
        transform.Find("WeaponSlot").gameObject.layer = LayerMask.NameToLayer("MainWeapon");
        playerDatas = transform.parent.GetComponent<PlayerDatas>();
        playerNetAgent = transform.parent.GetComponent<PlayerNetworkAgent>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (!isMapMode)
        {
            if (Input.GetMouseButton(0) && fireCount >= fireRate && weaponManagement.ammosInCurrentMag > 0)
            {
                weaponManagement.ammosInCurrentMag--;
                aim.ApplyRecoil();
                fireCount = 0;
                audioSource.PlayOneShot(fireClip);
                Shoot();
            }
            else
            {
                fireCount += Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.R) && weaponManagement.ammosInCurrentMag != weaponManagement.ammosPerMag)
            {
                StartCoroutine(ReloadCoroutine());
                audioSource.PlayOneShot(reloadClip);
            }
        }
    }

    void Shoot()
    {
        RaycastHit hitInfo;
        Transform trans = aim.GetBarrelEndTransform();
        Ray shootRay = new Ray(trans.position, trans.forward);
        if (Physics.Raycast(shootRay, out hitInfo, shootDist, LayerMask.GetMask("Wall", "Robot", "RemotePlayer")))
        {
            if (hitInfo.collider.gameObject.tag == "Robot")
            {
                hitInfo.collider.gameObject.GetComponent<RobotController>().Damage(2);
            }
            else if (hitInfo.collider.gameObject.tag == "OtherPlayers")
            {
                playerDatas.CmdPlayerShot(hitInfo.collider.gameObject.GetComponent<PlayerDatas>()._username);
            }
            else
            {
                playerNetAgent.CmdSpawnBulletHole(hitInfo.point + hitInfo.normal * 0.01f);
            }
        }
    }


    IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(3);
        weaponManagement.ammosInCurrentMag = weaponManagement.ammosPerMag;
    }
}
