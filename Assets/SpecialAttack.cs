using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : StateMachineBehaviour
{
    GameObject areaFX;
    BossController boss;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = GameObject.Find("Boss").GetComponent<BossController>();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.specialInit = false;
        boss.specialPlayed = false;
        boss.invincible = false;

        boss.specialAtkCooldown = boss.StartSpecialCooldown;
        boss.rollSpeed = 10;
        boss.reset("Attacking");
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
