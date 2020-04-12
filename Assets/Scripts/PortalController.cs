using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    PlayerController Player;
    Animator animator;
    GameObject E;
    GameObject PressBar;
    FindChildrenWithTag ChildrenFinder;
    bool canTransport = false;
    GameObject ScaleBar;
    public float PressSpeed;
    bool transported = false;

    public enum Scene
    {
        GameModeSelector,
        Level1,
        Level2,
        Level3,
        Level4,
        Level5,
        Ending,
    }

    public Scene dropDown;

    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        animator = Player.GetComponent<Animator>();
        E = transform.GetChild(0).gameObject;
        E.SetActive(false);
        PressBar = transform.GetChild(1).gameObject;
        PressBar.SetActive(false);
        ChildrenFinder = GetComponent<FindChildrenWithTag>();
        ScaleBar = ChildrenFinder.GetChildWithName(PressBar.transform, "Bar");
        Vector3 scale = ScaleBar.transform.localScale;
        scale.x = 0;
        ScaleBar.transform.localScale = scale;
    }

    void Update()
    {
        if (transported == false)
        {
            if (Input.GetKey(KeyCode.E) && canTransport == true)
            {
                Vector3 scale = ScaleBar.transform.localScale;
                PressBar.SetActive(true);
                if (scale.x >= 1)
                {
                    E.SetActive(false);
                    PressBar.SetActive(false);
                    GameManager GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();

                    GM.health = Player.health;
                    transported = true;
                    animator.Play("transportAway");
                    Player.allowInput = false;
                    //Don't adapt to tutorial
                    GM.calculatePlayerSkill();

                    Invoke("LoadNextScene", 1.05f);
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

    void LoadNextScene()
    {
        Loader.Load(dropDown.ToString());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            E.SetActive(true);
            canTransport = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            E.SetActive(false);
            canTransport = false;
        }
    }
}
