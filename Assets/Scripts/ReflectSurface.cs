using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectSurface : MonoBehaviour
{
    // #region Members

    // [Header("Reflect properties")]
    // public Vector3 localPosition = new Vector3(0, -2, 0);
    // public Vector3 localRotation = new Vector3(0, 0, 0);
    // [Tooltip("Optionnal: force the reflected sprite. If null it will be a copy of the source.")]
    // public string spriteLayer = "Default";
    // public int spriteLayerOrder = -5;

    // private SpriteRenderer spriteRenderer;

    // #endregion

    // #region Timeline

    // private List<Collider2D> Actors = new List<Collider2D>();
    // private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    // // void Awake()
    // // {
        
    // // }

    // void Awake()
    // {
    //     Actors = ArrayToList(Physics2D.OverlapAreaAll(pointA.position, pointB.position, whatAreActors));
    //     for (int i = 0; i < Actors.Count; i++)
    //     {
    //         createReflection(Actors[i].gameObject);
    //     }
    // }

    // void createReflection(GameObject Go)
    // {
    //     GameObject reflectGo = new GameObject("Reflection");
    //     reflectGo.transform.parent = Go.transform;
    //     reflectGo.transform.localPosition = localPosition;
    //     reflectGo.transform.localRotation = Quaternion.Euler(localRotation);
    //     Vector3 parentScale = Go.transform.localScale;
    //     reflectGo.transform.localScale = new Vector3(parentScale.x, -parentScale.y, parentScale.z);

    //     spriteRenderer = reflectGo.AddComponent<SpriteRenderer>();
    //     spriteRenderer.sortingLayerName = spriteLayer;
    //     spriteRenderer.sortingOrder = spriteLayerOrder;
    //     spriteRenderers.Add(spriteRenderer);
    // }

    // void Update()
    // {
    //     List<Collider2D> actors = ArrayToList(Physics2D.OverlapAreaAll(pointA.position, pointB.position, whatAreActors));
    //     Debug.Log(actors.Count);
    //     for (int i = 0; i < Math.Max(actors.Count, Actors.Count); i++)
    //     {
    //         if (i == actors.Count || i == Actors.Count) break;
    //         if (!Actors.Contains(actors[i])){
    //             createReflection(actors[i].gameObject);
    //             Actors.Add(actors[i]);
    //         }
    //         if (!actors.Contains(Actors[i])){
    //             DestroyReflection(Actors[i].gameObject);
    //             Actors.RemoveAt(i);
    //         }
    //     }
    // }
    
    HashSet<Collider2D> ArrayToList(Collider2D[] actors)
    {
        HashSet<Collider2D> Actors = new HashSet<Collider2D>();
        for (int i = 0; i < actors.Length; i++)
        {
            Actors.Add(actors[i]);
        }
        return Actors;
    }

    // void DestroyReflection(GameObject Go)
    // {
    //     for (int i = 0;i < Go.transform.childCount; i++)
    //     {
    //         Transform child = Go.transform.GetChild(i);
    //         if (child.name.Contains("Reflection")) Destroy(child);
    //     }
    // }

    // void OnDestroy()
    // {
    //     if (spriteRenderer != null)
    //     {
    //         Destroy(spriteRenderer.gameObject);
    //     }
    // }

    // void LateUpdate()
    // {
    //     for (int i = 0; i < spriteRenderers.Count; i++)
    //     {
    //         if (Actors[i].gameObject.GetComponent<SpriteRenderer>() != null)
    //         {
    //             spriteRenderers[i].sprite = Actors[i].gameObject.GetComponent<SpriteRenderer>().sprite;
    //             spriteRenderers[i].flipX = Actors[i].gameObject.GetComponent<SpriteRenderer>().flipX;
    //             spriteRenderers[i].flipY = Actors[i].gameObject.GetComponent<SpriteRenderer>().flipY;
    //             spriteRenderers[i].color = new Color(1f,1f,1f,0.25f);
    //         }
    //     }
    // }


    // #endregion

    public LayerMask whatAreActors; 
    public Transform pointA;
    public Transform pointB;

    void Update()
    {
        if (pointA.gameObject.active == true && pointB.gameObject.active== true)
        {
            HashSet<Collider2D> actors = ArrayToList(Physics2D.OverlapAreaAll(pointA.position, pointB.position, whatAreActors));
            GameObject[] Reflections = GameObject.FindGameObjectsWithTag("Reflection");
            for (int i =0; i < Reflections.Length; i++)
            {
                if (!actors.Contains(Reflections[i].transform.parent.gameObject.GetComponent<Collider2D>())){
                    Reflections[i].transform.parent.GetComponent<WaterReflectableScript>().active = false;
                }
                else Reflections[i].transform.parent.GetComponent<WaterReflectableScript>().active = true;
            }
        }
    }
}