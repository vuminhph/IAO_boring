using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoringModeButton : MonoBehaviour
{
    void OnEnable()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(boringMode);
    }

    void boringMode()
    {
        GameManager GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        GM.adaptiveModeOn = false;
        GM.boringModeOn = true;
        Loader.Load("Intro");
    }
}
