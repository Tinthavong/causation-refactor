using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContactDamage : MonoBehaviour
{
    Renderer render;
    Color c;
    public float invulnerabilityTime = 1f;


    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<Renderer>();
        c = render.material.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && !gameObject.GetComponent<PlayerStateManager>().IsDead())
        {
            StartCoroutine("GetInvulnerable");
        }    
    }

    IEnumerator GetInvulnerable()
    {
        Physics2D.IgnoreLayerCollision(1, 2, true); //player and enemy
        Physics2D.IgnoreLayerCollision(1, 2, true); //player abd bullet
        c.a = 0.5f;
        render.material.color = c;
        yield return new WaitForSeconds(invulnerabilityTime);
        Physics2D.IgnoreLayerCollision(1, 2, false); //player and enemy
        Physics2D.IgnoreLayerCollision(1, 2, false); //player abd bullet
        c.a = 1f;
        render.material.color = c;
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
