using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyShooting : MonoBehaviour
{
    //bulletZone is an object that should be placed where the tip of the gun is for the character sprites/animations. This creates the projectiles

    private float bulletSpeed = 400f;
    public GameObject bulletPrefab;
    public GameObject bulletStart;

    //Shooting variables
    public float firerate = 2f;
    private float firerateWait = 0f;

    private void Start()
    {
        
    }

    private void Update()
    {
        //firerateWait changes based on fps time
        firerateWait -= Time.deltaTime;
        //if firerateWait is 0, time to fire and reset the wait
        if (firerateWait <=0)
        {
            Shoot();
            firerateWait = firerate;
        }
    }

    void Shoot()
    {
        //Bullet is created at bulletZone's position
        GameObject b = Instantiate(bulletPrefab) as GameObject;
        b.transform.position = bulletStart.transform.position;
        //Bullet object shifts position and rotation based on direction
        if (gameObject.transform.localScale.x < 0)
        {
            b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90f);
            b.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletSpeed);
        }
        else
        {
            b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90f);
            b.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletSpeed);
        }
    }
}
