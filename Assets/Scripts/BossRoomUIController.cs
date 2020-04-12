using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossRoomUIController : UIController
{
    Image BossHealthBar;
    float BossMaxHealth;
    float BossHealth;

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
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        MaxHealth = player.MaxHealth;
        Key = ChildrenGetter.GetChildWithName(Canvas.transform, "Key");
        gameOverScreen = ChildrenGetter.GetChildWithName(Canvas.transform, "GameOverScreen");

        BossHealthBar = ChildrenGetter.GetChildWithName(Canvas.transform, "BossHealthBar").GetComponent<Image>();
        BossMaxHealth = 500f;

        if (GM.health == 0)
            SetHealth(250);
        else SetHealth(GM.health);

        if (GM.combo != 0)
            SetCombo(GM.combo);

        UpdateScore(GM.score);
    }

    private void Update()
    {
        if (HPgain > 0)
        {
            healthBar.fillAmount += gainRate * Speed * Time.deltaTime;
            HPgain -= gainRate * MaxHealth * Speed * Time.deltaTime;
        }
        else HPgain = 0;

        if (BossHPGain > 0)
        {
            BossHealthBar.fillAmount += bossGainRate * Speed * Time.deltaTime;
            BossHPGain -= bossGainRate * BossMaxHealth * Speed * Time.deltaTime;
        }
        else if (BossHPGain < 0)
        {
            GameObject.Find("Boss").GetComponent<BossController>().recovered = true;
            BossHPGain = 0;
        }
    }

    public void bossDamage(float damage)
    {
        BossHealthBar.fillAmount -= (float)damage / BossMaxHealth;
    }

    float BossHPGain = 0;
    float bossGainRate;

    public void bossHPGain()
    {
        BossHPGain = BossMaxHealth * 0.75f;
        bossGainRate = 0.1f;
    }
}
