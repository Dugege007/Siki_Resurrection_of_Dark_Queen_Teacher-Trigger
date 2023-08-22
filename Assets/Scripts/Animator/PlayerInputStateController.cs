using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputStateController : StateMachineBehaviour
{
    public bool lockMove;
    public bool lockJump;
    public bool lockEquip;
    public bool lockAttack;
    public bool lockUseSkill;
    public bool lockAll = true;

    public bool onlyLock;
    public bool keepLocking;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (onlyLock)
            return;
        if (lockAll)
        {
            //通过 animator 可以拿到当前游戏物体身上的任何组件和参数
            animator.transform.GetComponent<PlayerController>().canGetPlayerInputValue = false;
        }
        if (lockMove)
            animator.transform.GetComponent<PlayerController>().canMove = false;
        if (lockJump)
            animator.transform.GetComponent<PlayerController>().canJump = false;
        if (lockEquip)
            animator.transform.GetComponent<PlayerController>().canEquip = false;
        if (lockAttack)
            animator.transform.GetComponent<PlayerController>().canAttack = false;
        if (lockUseSkill)
            animator.transform.GetComponent<PlayerController>().canUseSkill = false;

        //输出当前动画的哈希值
        //Debug.Log("当前进入的状态是：" + stateInfo.shortNameHash);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("动画更新中");
        if (keepLocking)
        {
            animator.transform.GetComponent<PlayerController>().canGetPlayerInputValue = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (lockAll)
        {
            //通过 animator 可以拿到当前游戏物体身上的任何组件和参数
            animator.transform.GetComponent<PlayerController>().UnLockAll();
        }
        if (lockMove)
            animator.transform.GetComponent<PlayerController>().canMove = true;
        if (lockJump)
            animator.transform.GetComponent<PlayerController>().canJump = true;
        if (lockEquip)
            animator.transform.GetComponent<PlayerController>().canEquip = true;
        if (lockAttack)
            animator.transform.GetComponent<PlayerController>().canAttack = true;
        if (lockUseSkill)
            animator.transform.GetComponent<PlayerController>().canUseSkill = true;

        //Debug.Log("当前退出的动画状态所在状态机挂载的游戏物体是：" + animator.gameObject);
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
