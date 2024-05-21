using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShipGuns : MonoBehaviour
{
    public GameObject bullet;

    public float shootForce, upwardForce;

    public float timeBetweenShooting, spread, timeBetweenShots;
    public int bulletsPerTap;
    public bool allowButtonHold;

    int bulletsShot;

    public bool shooting, readyToShoot;

    public Transform shipPov;
    public Camera fpsCam;
    public Transform attackPoint;

    public bool allowInvoke = true;

    private void Awake()
    {
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();
    }

    public void SetShooting(bool value)
    {
        shooting = value;
    }

    private void MyInput()
    {
        if (!Application.isMobilePlatform && !Application.isConsolePlatform)
        {
            shooting = (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Space));
        }

        if (readyToShoot && shooting)
        {
            bulletsShot = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;
        bulletsShot++;

        Ray ray = new Ray(shipPov.position, shipPov.forward);
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else targetPoint = ray.GetPoint(75);

        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);

        currentBullet.transform.forward = directionWithoutSpread.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(shipPov.transform.up * upwardForce, ForceMode.Impulse);

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        if (bulletsShot < bulletsPerTap)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    public void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }
}
