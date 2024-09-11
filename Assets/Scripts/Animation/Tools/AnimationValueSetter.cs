using UnityEngine;

/// <summary>
/// Allows for Changing parameters like bools in the animator controller.
/// This is useful for streamlining the process and decluttering code.
/// Written by: Sean
/// Modified by:
/// </summary>
public class AnimationValueSetter : StateMachineBehaviour
{
    public BoolValue[] onEntetBools;
    public BoolValue[] onExitBools;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 0; i < onExitBools.Length; i++)
        {
            animator.SetBool(onExitBools[i].boolName, onExitBools[i].value);
        }
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

    [System.Serializable]
    public class BoolValue
    {
        public string boolName;
        public bool value;
    }
}
