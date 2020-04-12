using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsUI : MonoBehaviour
{
    GameManager GM;
    GameObject Canvas;
    FindChildrenWithTag ChildrenGetter;

    Slider MaxSkillSlider;
    Slider MinSkillSlider;
    Slider BoringFactorSlider;

    TextMeshProUGUI maxValue;
    TextMeshProUGUI minValue;
    TextMeshProUGUI boringValue;

    void Awake()
    {
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        Canvas = GameObject.Find("Canvas");
        ChildrenGetter = GetComponent<FindChildrenWithTag>();

        MaxSkillSlider = ChildrenGetter.GetChildWithName(Canvas.transform, "MaxSkillSlider").GetComponent<Slider>();
        MinSkillSlider = ChildrenGetter.GetChildWithName(Canvas.transform, "MinSkillSlider").GetComponent<Slider>();
        BoringFactorSlider = ChildrenGetter.GetChildWithName(Canvas.transform, "BoringFactorSlider").GetComponent<Slider>();

        minValue = ChildrenGetter.GetChildWithName(MinSkillSlider.gameObject.transform, "Value").GetComponent<TextMeshProUGUI>();
        maxValue = ChildrenGetter.GetChildWithName(MaxSkillSlider.gameObject.transform, "Value").GetComponent<TextMeshProUGUI>();
        boringValue = ChildrenGetter.GetChildWithName(BoringFactorSlider.gameObject.transform, "Value").GetComponent<TextMeshProUGUI>();

        MaxSkillSlider.value = GM.maxSkillFactor;
        MinSkillSlider.value = GM.minSkillFactor;
        BoringFactorSlider.value = GM.boringFactor;
    }

    // Update is called once per frame
    void Update()
    {
        minValue.SetText(MinSkillSlider.value.ToString("####0.00"));
        maxValue.SetText(MaxSkillSlider.value.ToString("####0.00"));
        boringValue.SetText(BoringFactorSlider.value.ToString("####0.00"));

        GM.minSkillFactor = MinSkillSlider.value;
        GM.maxSkillFactor = MaxSkillSlider.value;
        GM.boringFactor = BoringFactorSlider.value;
    }
}
