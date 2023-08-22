using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBehaviour : StateMachineBehaviour
{
    public bool lockMove = false;
    public bool lockJump = false;
    public bool lockEquip = false;
    public bool lockAttack = false;
    public bool lockUseSkill = false;
    public bool lockAll = true;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (lockAll)
        {
            //通过 animator 可以拿到当前游戏物体身上的任何组件和参数
            animator.transform.GetComponent<PlayerController>().canGetPlayerInputValue = false;
        }
        else if (lockMove)
            animator.transform.GetComponent<PlayerController>().canMove = false;
        else if (lockJump)
            animator.transform.GetComponent<PlayerController>().canJump = false;
        else if (lockEquip)
            animator.transform.GetComponent<PlayerController>().canEquip = false;
        else if (lockAttack)
            animator.transform.GetComponent<PlayerController>().canAttack = false;
        else if (lockUseSkill)
            animator.transform.GetComponent<PlayerController>().canUseSkill = false;

        //输出当前动画的哈希值
        Debug.Log("当前进入的状态是：" + stateInfo.shortNameHash);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("正在装备");
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (lockAll)
        {
            //通过 animator 可以拿到当前游戏物体身上的任何组件和参数
            animator.transform.GetComponent<PlayerController>().canGetPlayerInputValue = true;
        }
        else if (lockMove)
            animator.transform.GetComponent<PlayerController>().canMove = true;
        else if (lockJump)
            animator.transform.GetComponent<PlayerController>().canJump = true;
        else if (lockEquip)
            animator.transform.GetComponent<PlayerController>().canEquip = true;
        else if (lockAttack)
            animator.transform.GetComponent<PlayerController>().canAttack = true;
        else if (lockUseSkill)
            animator.transform.GetComponent<PlayerController>().canUseSkill = true;

        Debug.Log("当前退出的动画状态所在状态机挂载的游戏物体是：" + animator.gameObject);
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
