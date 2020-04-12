using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickShooting : MonoBehaviour
{
    public Transform attackPos;

    public void switchGun()
    {
        Vector2 pos = attackPos.position;
        pos.x += 1.46f * GetComponent<EnemyController>().isFacingRight;
        attackPos.position = pos;
    }

    public void resetGun()
    {
        Vector2 pos = attackPos.position;
        pos.x -= 1.46f * GetComponent<EnemyController>().isFacingRight;
        attackPos.position = pos;
    }
}
