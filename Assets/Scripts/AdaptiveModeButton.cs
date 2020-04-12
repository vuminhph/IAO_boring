using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdaptiveModeButton : MonoBehaviour
{
    void OnEnable()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(adaptiveTask);
    }

    void adaptiveTask()
    {
        GameManager GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        GM.adaptiveModeOn = true;
        GM.boringModeOn = false;
        Loader.Load("Intro");
    }

}
