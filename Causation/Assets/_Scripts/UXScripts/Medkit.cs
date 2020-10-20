using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Medkit : MonoBehaviour
{
    public MedkitController mkc;
    private void Start()
    {
        mkc = GameObject.Find("MedkitCounter").GetComponent<MedkitController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if (mkc.medkitNumber != 3)
            {
                switch (mkc.medkitNumber)
                {
                    case 0:
                        {
                            mkc.medkitNumber += 1;
                            mkc.medkits[0].SetActive(true);
                            Debug.Log("Number of Medkits: " + mkc.medkitNumber);
                            break;
                        }
                    case 1:
                        {
                            mkc.medkitNumber += 1;
                            mkc.medkits[1].SetActive(true);
                            Debug.Log("Number of Medkits: " + mkc.medkitNumber);
                            break;
                        }
                    case 2:
                        {
                            mkc.medkitNumber += 1;
                            mkc.medkits[2].SetActive(true);
                            Debug.Log("Number of Medkits: " + mkc.medkitNumber);
                            break;
                        }
                }

                AudioSource.PlayClipAtPoint(this.GetComponent<AudioSource>().clip, this.transform.position);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
