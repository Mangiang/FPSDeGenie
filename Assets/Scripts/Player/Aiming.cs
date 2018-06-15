using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    private Vector3 NormalPos = new Vector3(0.331f, -0.449f, 1.005f);
    private Vector3 NormalRotation = new Vector3(0, 0, 0);
    private Vector3 AimPos = new Vector3(-0.0018f, -0.235f, 0.601f);
    private Vector3 AimRotation = new Vector3(0, 0, 0);
    [HideInInspector]
    public bool isMapMode = false;
    private GameObject BarrelEnd;
    private Vector3 rotationPositiveAngles = new Vector3(0.8f, 0f, 0f); // Positive recoil rotation
    private Vector3 rotationNegativeAngles = new Vector3(0.2f, 0f, 0f); // Negative recoil rotation
    private Vector3 maxRotation = new Vector3(6, 6, 6);
    private float rotationSpeed = 1f;

    // Use this for initialization
    void OnEnable()
    {
        transform.localPosition = NormalPos;
        BarrelEnd = transform.GetChild(0).Find("WeaponBarrelEnd").gameObject;
        transform.GetChild(0).GetComponent<WeaponManagement>().enabled = true;
    }

    public Transform GetBarrelEndTransform()
    {
        return BarrelEnd.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 aimRotation = Vector3.zero;
        if (!isMapMode)
        {
            if (Input.GetMouseButtonDown(1))
            {
                transform.localPosition = AimPos;
                transform.localRotation = Quaternion.Euler(AimRotation);
                aimRotation = AimRotation;
            }

            if (Input.GetMouseButtonUp(1))
            {
                transform.localPosition = NormalPos;
                transform.localRotation = Quaternion.Euler(NormalRotation);
                aimRotation = NormalRotation;
            }
        }

        // Lerp toward the normal rotation
        Vector3 currentRotation = transform.localRotation.eulerAngles;
        currentRotation.x = (currentRotation.x > 180 ? 360 - currentRotation.x : currentRotation.x);
        currentRotation.y = (currentRotation.y > 180 ? 360 - currentRotation.y : currentRotation.y);
        currentRotation.z = (currentRotation.z > 180 ? 360 - currentRotation.z : currentRotation.z);
        float xRotation = 0, yRotation = 0, zRotation = 0;
        if (Mathf.Abs(aimRotation.x - currentRotation.x) > 0.1f)
        {
            xRotation = Mathf.Lerp(currentRotation.x, aimRotation.x, rotationSpeed * Time.deltaTime);
        }
        if (Mathf.Abs(aimRotation.y - currentRotation.y) > 0.1f)
        {
            yRotation = Mathf.Lerp(currentRotation.y, aimRotation.y, rotationSpeed * Time.deltaTime);
        }
        if (Mathf.Abs(aimRotation.z - currentRotation.z) > 0.1f)
        {
            zRotation = Mathf.Lerp(currentRotation.z, aimRotation.z, rotationSpeed * Time.deltaTime);
        }
        transform.localRotation = Quaternion.Euler(new Vector3(xRotation, yRotation, zRotation));
    }

    public void ApplyRecoil()
    {
        Vector3 toRotate = Vector3.zero;
        Vector3 currentRotation = transform.localRotation.eulerAngles;
        currentRotation.x = (currentRotation.x > 180 ? 360 - currentRotation.x : currentRotation.x);
        currentRotation.y = (currentRotation.y > 180 ? 360 - currentRotation.y : currentRotation.y);
        currentRotation.z = (currentRotation.z > 180 ? 360 - currentRotation.z : currentRotation.z);
        if (isXRotationWithinBounds(currentRotation))
        {
            // Invert because the weapon has a 180° rotation
            toRotate.x = Random.Range(-rotationPositiveAngles.x, rotationNegativeAngles.x);
        }
        else
        {
            toRotate.x = Random.Range(0, rotationPositiveAngles.x) * (maxRotation.x > currentRotation.x ? 1 : -1);
        }

        if (isYRotationWithinBounds(currentRotation))
        {
            // Invert because the weapon has a 180° rotation
            toRotate.y = Random.Range(-rotationPositiveAngles.y, rotationNegativeAngles.y);
        }
        else
        {
            toRotate.y = Random.Range(0, rotationPositiveAngles.y) * (maxRotation.y > currentRotation.y ? 1 : -1);
        }

        if (isZRotationWithinBounds(currentRotation))
        {
            // Invert because the weapon has a 180° rotation
            toRotate.z = Random.Range(-rotationPositiveAngles.z, rotationNegativeAngles.z);
        }
        else
        {
            toRotate.z = Random.Range(0, rotationPositiveAngles.z) * (maxRotation.z > currentRotation.z ? 1 : -1);
        }

        transform.Rotate(toRotate);
    }

    private bool isXRotationWithinBounds(Vector3 currentRotation)
    {
        return currentRotation.x < maxRotation.x && currentRotation.x > -maxRotation.x;
    }

    private bool isYRotationWithinBounds(Vector3 currentRotation)
    {
        return currentRotation.y < maxRotation.y && currentRotation.y > -maxRotation.y;
    }

    private bool isZRotationWithinBounds(Vector3 currentRotation)
    {
        return currentRotation.z < maxRotation.z && currentRotation.z > -maxRotation.z;
    }
}
