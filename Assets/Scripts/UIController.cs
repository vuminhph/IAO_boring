using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    protected GameManager GM;
    protected GameObject Canvas;
    protected FindChildrenWithTag ChildrenGetter;
    protected GameObject LevelText;
    public TextMeshProUGUI Score;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI ComboCounter;
    protected GameObject Key;
    public Image healthBar;
    protected int MaxHealth;
    protected GameObject gameOverScreen;

    protected virtual void Awake()
    {
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        Canvas = GameObject.Find("Canvas");
        ChildrenGetter = GetComponent<FindChildrenWithTag>();
        LevelText = ChildrenGetter.GetChildWithName(Canvas.transform, "LevelText");
        Score = ChildrenGetter.GetChildWithName(Canvas.transform, "Score").GetComponent<TextMeshProUGUI>();
        ScoreText = ChildrenGetter.GetChildWithName(Canvas.transform, "Sc0reText").GetComponent<TextMeshProUGUI>();
        ComboCounter = ChildrenGetter.GetChildWithName(Canvas.transform, "ComboCounter").GetComponent<TextMeshProUGUI>();
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Image>();
        if (SceneManager.GetActiveScene().name != "Ending")
        {
            PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            MaxHealth = player.MaxHealth;
        }  
        Key = ChildrenGetter.GetChildWithName(Canvas.transform, "Key");
        gameOverScreen = ChildrenGetter.GetChildWithName(Canvas.transform, "GameOverScreen");

        if (GM.health == 0)
            SetHealth(250);
        else SetHealth(GM.health);

        if (GM.combo != 0)
            SetCombo(GM.combo);

        UpdateScore(GM.score);
    }

    protected virtual void Update()
    {
        if (HPgain > 0)
        {
            healthBar.fillAmount += gainRate * Speed * Time.deltaTime;
            HPgain -= gainRate * MaxHealth * Speed * Time.deltaTime;
        }
        else HPgain = 0;

        // Pause in game
        // if (GM.inMenu == false && Input.GetKeyDown("escape"))
        // {
        //     PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        //     GameObject pauseScreen = ChildrenGetter.GetChildWithName(gameObject.transform, "PauseScreen");
        //     if (Time.timeScale == 1)
        //     {
        //         pauseScreen.SetActive(true);
        //         ChildrenGetter.GetChildWithName(pauseScreen.transform, "Score").GetComponent<TextMeshProUGUI>().SetText(GM.score.ToString());
        //         Time.timeScale = 0;
        //         player.allowInput = false;
        //     }
        //     else
        //     {
        //         pauseScreen.SetActive(false);
        //         Time.timeScale = 1;
        //         player.allowInput = true;
        //     }
        // }

    }

    protected float HPgain;
    protected float gainRate;
    protected float Speed = 1.5f;

    public void SetHealth(float health)
    {
        healthBar.fillAmount = health / MaxHealth;
    }

    public void takeDamage(float damage)
    {
        healthBar.fillAmount -= (float)damage / MaxHealth;
    }

    public void gainHealth(int gain)
    {
        HPgain += (float)gain;
        gainRate = (float)gain / MaxHealth;
    }

    // Update combo counter
    public void UpdateCombo(int counter)
    {
        ComboCounter.gameObject.SetActive(true);
        ComboCounter.gameObject.GetComponent<Animator>().Play("Shake");
        ComboCounter.SetText(counter.ToString() + "x");
    }

    public void ComboBreak()
    {
        ComboCounter.gameObject.GetComponent<Animator>().Play("Fade");
    }

    public void SetCombo(int counter)
    {
        ComboCounter.gameObject.SetActive(true);
        ComboCounter.SetText(counter.ToString() + "x");
    }

    public void UpdateScore(int score)
    {
        Score.SetText(score.ToString());
    }

    public virtual void foundKey()
    {
        Key.SetActive(true);
    }

    public virtual void useKey()
    {
        Key.SetActive(false);
    }

    public void endGame(int highestCombo, int score)
    {
        gameOverScreen.SetActive(true);
        ChildrenGetter.GetChildWithName(gameOverScreen.transform, "HighestCombo").GetComponent<TextMeshProUGUI>().SetText(highestCombo.ToString());
        ChildrenGetter.GetChildWithName(gameOverScreen.transform, "Score").GetComponent<TextMeshProUGUI>().SetText(score.ToString());
    }
}
