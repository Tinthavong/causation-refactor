using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shooting : MonoBehaviour
{
    //bulletZone is an object that should be placed where the tip of the gun is for the character sprites/animations. This creates the projectiles
    public PlayerMovement playerRef;

    public TMP_Text ammoText;
    public int currentAmmo;
    public int maxAmmo = 6;

    private float bulletSpeed = 200f;
    public GameObject bulletPrefab;
    public GameObject bulletStart;

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        if (ammoText.text != null)
        {
            ammoText.text = currentAmmo.ToString();

            if (Input.GetButtonDown("Fire1") && currentAmmo > 0)
            {
                playerRef.isShooting = true;
                Debug.Log(playerRef.isShooting);
                if (currentAmmo == 0)
                {
                    playerRef.isShooting = false;
                    Debug.Log("Ammo is Empty");
                }
                else
                {
                    Shoot();
                }
                playerRef.isShooting = false;

                currentAmmo--;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                currentAmmo = maxAmmo;
            }
        }
    }

    void Shoot()
    {
        //this.gameObject.GetComponentInParent<GameObject>().active = false;

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
