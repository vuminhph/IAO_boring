using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    GameObject E;
    GameObject PressBar;
    GameObject Player;
    FindChildrenWithTag ChildrenFinder;
    GameObject ScaleBar;
    Animator animator;
    
    KeyHider kh;

    void Start()
    {
        animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        E = transform.GetChild(0).gameObject;
        E.SetActive(false);
        PressBar = transform.GetChild(1).gameObject;
        PressBar.SetActive(false);
        ChildrenFinder = GetComponent<FindChildrenWithTag>();
        ScaleBar = ChildrenFinder.GetChildWithName(PressBar.transform, "Bar");
        Vector3 scale = ScaleBar.transform.localScale;
        scale.x = 0;
        ScaleBar.transform.localScale = scale;
        kh = GameObject.Find("Canvas").GetComponent<KeyHider>();
    }

    public float PressSpeed;
    public bool opened = false;

    public enum contains
    {
        nothing,
        key,
        HP
    };

    public contains dropDown;
    void Update()
    {
        if (opened == false)
        {
            if (Input.GetKey(KeyCode.E) && canOpen == true)
            {
                Vector3 scale = ScaleBar.transform.localScale;
                PressBar.SetActive(true);
                if (scale.x >= 1)
                {
                    E.SetActive(false);
                    PressBar.SetActive(false);
                    animator.Play("Open");

                    if (dropDown == contains.key)
                    {
                        GameObject Key = (GameObject)Instantiate(Resources.Load("SystemObjects/Key"), transform.position, transform.rotation);
                        Key.transform.parent = transform;
                        Key.GetComponent<Animator>().Play("KeyFly");
                    }
                    opened = true;
                    if (kh != null)
                        kh.CheckChest();

                    if (dropDown == contains.HP)
                    {
                        GameObject HP = (GameObject)Instantiate(Resources.Load("SystemObjects/HP_pickup"), transform.position, transform.rotation);
                        HP.transform.parent = transform;
                        HP.GetComponent<Animator>().Play("HPFly");
                    }
                }
                else
                {
                    scale.x += PressSpeed * Time.deltaTime;
                    ScaleBar.transform.localScale = scale;
                }
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                Vector3 scale = ScaleBar.transform.localScale;
                scale.x = 0;
                ScaleBar.transform.localScale = scale;
                PressBar.SetActive(false);
            }
        }
    }

    bool canOpen = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (opened == false)
        {
            if (other.CompareTag("Player"))
            {
                E.SetActive(true);
                canOpen = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            E.SetActive(false);
            canOpen = false;
        }
    }
}
