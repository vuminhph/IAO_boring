using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ToSettingsButton : MonoBehaviour
{
    void OnEnable()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(ToSettings);
    }

    void ToSettings()
    {
        GameManager GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        if (SceneManager.GetActiveScene().name == "BoringMode")
        {
            GM.boringModeOn = true;
            GM.adaptiveModeOn = false;
        }
        else if (SceneManager.GetActiveScene().name == "AdaptiveMode")
        {
            GM.boringModeOn = false;
            GM.adaptiveModeOn = true;
        }
        Loader.Load("Settings");
    }
}
