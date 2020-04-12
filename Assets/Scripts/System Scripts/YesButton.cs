using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YesButton : MonoBehaviour
{
    void OnEnable()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(Yes);
    }

    void Yes()
    {
        GameManager GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        GM.inMenu = false;
        Loader.Load("Level1");
    }

}
