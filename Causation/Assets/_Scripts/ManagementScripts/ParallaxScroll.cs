using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroll : MonoBehaviour
{
    //refactor class into the future levelmanager script
    private float length, startPos;
    public GameObject cam;
    public float parallaxEffect;

    public float[] lengths, startPoss, pxPerIndex, dist;
    public GameObject[] parallaxedObjects; //imo the smaller the index (the number inside of the brackets) the closest away they are to the foreground. 0 would be the closest
    float ipx = 0; //initial parallax

    public bool plainRepeat; //intended to be used for basic repeatng backgrounds without parallax.

    // Start is called before the first frame update
    void Start()
    {
        lengths = new float[parallaxedObjects.Length];
        startPoss = new float[parallaxedObjects.Length];
        pxPerIndex = new float[parallaxedObjects.Length];
        dist = new float[parallaxedObjects.Length];
        //temp = new float[parallaxedObjects.Length];

        for (int i = 0; i < parallaxedObjects.Length; i++)
        {
            startPoss[i] = parallaxedObjects[i].transform.position.x;
            lengths[i] = FindObjectOfType<SpriteRenderer>().bounds.size.x;

            ipx += .2f;
            pxPerIndex[i] = ipx;
        }

        //startPos = transform.position.x;
        //length = FindObjectOfType<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        //float dist = (cam.transform.position.x * parallaxEffect);

        if (!plainRepeat)
        //having to do a for loop every frame seems kinda weird but i'll fix this later
        {
            for (int i = 0; i < parallaxedObjects.Length; i++)
            {
                dist[i] = (cam.transform.position.x * pxPerIndex[i]);

                parallaxedObjects[i].transform.position = new Vector3(startPoss[i] + dist[i],
                    parallaxedObjects[i].transform.position.y, parallaxedObjects[i].transform.position.z);
            }
        }
        else
        {
            //repeating doesn't work as intended right now, just commented out
            //now the bool will just disable parallax but why would it be attached to anything to begin with then
            /*
            for (int i = 0; i < parallaxedObjects.Length; i++)
            {
                float temp = (cam.transform.position.x * (1 - pxPerIndex[i]));
                if (temp > startPoss[i] + lengths[i]) startPoss[i] += lengths[i];
                else if (temp < startPoss[i] - lengths[i]) startPoss[i] -= lengths[i];
            }

            for (int i = 0; i < parallaxedObjects.Length; i++)
            {
                float temp = (cam.transform.position.x * (1 - pxPerIndex[i]));
                if (temp > startPoss[i] + lengths[i]) startPoss[i] += lengths[i];
                else if (temp < startPoss[i] - lengths[i]) startPoss[i] -= lengths[i];
            }*/
        }
    }
}
