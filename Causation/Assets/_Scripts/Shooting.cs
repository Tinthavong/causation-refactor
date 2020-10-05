using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    //bulletZone is an object that should be placed where the tip of the gun is for the character sprites/animations. This creates the projectiles
    public Transform bulletZone;
    public GameObject bulletPrefab;

    private void Update()
    {
        //Left click shoots
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        //Bullet is created at bulletZone's position
        Instantiate(bulletPrefab, bulletZone.position, bulletZone.rotation);
    }
}
