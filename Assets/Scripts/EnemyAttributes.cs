using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAttributes
{
    public float health;
    public float bulletSpeed;
    public float speed;
    public float attackPow;

    public enemyAttributes(float Health, float BulletSpeed, float Speed, float AttacKPow)
    {
        this.health = Health;
        this.bulletSpeed = BulletSpeed;
        this.speed = Speed;
        this.attackPow = AttacKPow;
    }
}