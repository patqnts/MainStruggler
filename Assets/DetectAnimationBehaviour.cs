using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectAnimationBehaviour : StateMachineBehaviour
{
    private Plant plant;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        plant = animator.gameObject.GetComponent<Plant>();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        plant.isDetecting = false;
    }
}
