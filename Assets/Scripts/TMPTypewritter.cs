using System.Collections;
using UnityEngine;
using TMPro;

public class TMPTypewritter : MonoBehaviour 
{   
    TextMeshProUGUI TMPText;
    public string text;

    private void OnEnable() {
        TMPText = GetComponent<TextMeshProUGUI>();
        TMPText.SetText("");
        StopAllCoroutines();
        StartCoroutine(EffectTypewriter(text));
    }

    private IEnumerator EffectTypewriter(string text)
    {
       foreach(char character in text.ToCharArray())
       {
           TMPText.text += character;
           yield return new WaitForSeconds(0.05f);
       }
    }
}
