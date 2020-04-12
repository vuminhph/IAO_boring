using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    void OnEnable()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(reset);
    }

    void reset()
    {
       GameManager GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        if (GM.boringModeOn == true)
        {
            GM.resetGameManager();
            Destroy(GM.gameObject);
            Loader.Load("BoringMode");
        }
        else if (GM.adaptiveModeOn == true)
        {
            GM.resetGameManager();
            Destroy(GM.gameObject);
            Loader.Load("AdaptiveMode");
        }
    }
}
