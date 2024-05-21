using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class DamageGun : MonoBehaviour
{

    public float BulletRange;
    public Transform PlayerCam;

    public void Shoot()
    {
        Debug.Log("Start of shoot method");

        Ray GunRay = new Ray(PlayerCam.position, PlayerCam.forward);
        if (Physics.Raycast(GunRay, out RaycastHit hitInfo, BulletRange)) 
        {
            if (hitInfo.collider.TryGetComponent(out Entity enemy))
            {
                Debug.Log("I hit someone");
                enemy.Damage(5);
            }
        }
    }
}
