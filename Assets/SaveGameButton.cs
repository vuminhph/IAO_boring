using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveGameButton : MonoBehaviour
{
    public GameObject confirmText;

    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(SaveGame);
    }

    void SaveGame()
    {
        GameManager GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        GM.AddNewScore(GM.Username, GM.score);
        confirmText.SetActive(true);
    }
}
