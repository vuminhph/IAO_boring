using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    public GameObject Timeline1;
    public GameObject Timeline2;

    void OnEnable()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(Continue);
    }

    void Continue()
    {
        Timeline1.SetActive(false);
        Timeline2.SetActive(true);
    }
}
