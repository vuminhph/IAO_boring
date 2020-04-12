using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToMenuButton : MonoBehaviour
{
    void OnEnable()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(ToMenu);
    }

    void ToMenu()
    {
        GameManager GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        if (GM.boringModeOn == true)
        {
            GM.resetGameManager();
            Loader.Load("BoringMode");
        }
        else if (GM.adaptiveModeOn == true)
        {
            GM.resetGameManager();
            Loader.Load("AdaptiveMode");
        }
    }
}
