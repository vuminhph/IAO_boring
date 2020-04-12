using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoringToMenu : MonoBehaviour
{
    void OnEnable()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(ToMenu);
    }

    void ToMenu()
    {
        Loader.Load("BoringMode");
    }
}
