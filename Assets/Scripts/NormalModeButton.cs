using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalModeButton : MonoBehaviour
{
    void OnEnable()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(normalMode);
    }

    void normalMode()
    {
        GameManager GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        GM.adaptiveModeOn = false;
        GM.inMenu = false;
        Loader.Load("Level1");
    }
}