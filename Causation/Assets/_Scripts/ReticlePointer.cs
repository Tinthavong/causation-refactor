using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticlePointer : MonoBehaviour
{
    private Vector3 target;
    public GameObject recticle;
    public GameObject player; //because the mouse controls the player rotation, consider making this rotate the arm isntead of the entire player object
    public GameObject bulletPrefab;
    public GameObject bulletStart; //presumably where the bullet is instanced out of. TODO create a method that destroys the bullet on collision or after time

    public float bulletSpeed = 60f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        target = GetComponent<Camera>().ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
        recticle.transform.position = new Vector2(target.x, target.y);
        
        Vector3 difference = target - player.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        player.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);

        if (Input.GetMouseButtonDown(0))
        {
            float distence = difference.magnitude;
            Vector2 direction = difference / distence;
            direction.Normalize();
            fireBullet(direction, rotationZ);
        }

        void fireBullet(Vector2 direction, float directionZ)
        {
            GameObject b = Instantiate(bulletPrefab) as GameObject;
            b.transform.position = bulletStart.transform.position;
            b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
            b.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        }
    }
}
