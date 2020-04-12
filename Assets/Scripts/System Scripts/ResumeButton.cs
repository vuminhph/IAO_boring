using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{
    void OnEnable()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(ResumeGame);
    }

    void ResumeGame()
    {
        GameManager GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        GameObject Canvas = GameObject.Find("Canvas");
        FindChildrenWithTag ChildrenGetter = Canvas.GetComponent<FindChildrenWithTag>();
        GameObject pauseScreen = ChildrenGetter.GetChildWithName(Canvas.transform, "PauseScreen");
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
        player.allowInput = true;
    }
}
