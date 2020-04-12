using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int numOfHearts;
    public float healthPerHeart;
    private float health;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private int fullHeartIndex;
    // private Animator animator;

    void Update()
    {
        health = GetComponent<PlayerController>().health;
        for (int i = 0; i < hearts.Length; i++)
        {

            if (i * healthPerHeart < health)
            {
                hearts[i].sprite = fullHeart;
                // fullHeartIndex = i - 1;
            }
            else
            {
                // if (i - 1 > fullHeartIndex){
                //     animator = hearts[i].GetComponent<Animator>();
                //     StartCoroutine(playAnim("Heart_flash"));
                // }
                hearts[i].sprite = emptyHeart;
            }

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    // IEnumerator playAnim(string AnimationCall)
    // {
    //     animator.Play(AnimationCall);

    //     //Wait until Animator is done playing
    //     while (animator.GetCurrentAnimatorStateInfo(0).IsName(AnimationCall) &&
    //     animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
    //     {
    //         yield return null;
    //     }
    //     Debug.Log("lose health");
    // }
}
