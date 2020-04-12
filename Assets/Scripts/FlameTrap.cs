using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameTrap : MonoBehaviour
{
    private float releaseDelay;
    public float startReleaseDelay;

    void Start()
    {
        releaseDelay = startReleaseDelay; 
    }

    private List<GameObject> flames = new List<GameObject>();
    
    void Update()
    {
        if (releaseDelay <= 0)
        {
            
            if (flames.Count != 0)
            {
                if (flames[0].GetComponent<FlameController>().rangeMet == false)
                {
                    Vector3 flamePos = transform.position;
                    flamePos.y -= transform.localScale.y * 0.49f;
                    GameObject flame = (GameObject) Instantiate(Resources.Load("Traps/flame"), flamePos, transform.rotation);
                    flame.transform.parent = transform;
                    flames.Add(flame);
                }
                else{
                    releaseDelay = startReleaseDelay;
                    flames = new List<GameObject>();
                }
                
            }
            else
            {
                Vector3 flamePos = transform.position;
                flamePos.y -= transform.localScale.y * 0.49f;
                GameObject flame = (GameObject) Instantiate(Resources.Load("Traps/flame"), flamePos, transform.rotation);
                flame.transform.parent = transform;
                flames.Add(flame);
            }
        }
        else releaseDelay -= Time.deltaTime;
    }

    
}
