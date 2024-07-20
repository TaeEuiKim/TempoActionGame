using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTempoState : StateMachineBehaviour
{
    [SerializeField] private AtkTempoData _data;
    private PlayerManager _player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_player == null)
        {
            _player = animator.transform.parent.GetComponent<PlayerManager>();
        }

        _player.Atk.CurAtkTempoData = _data;

    }

    
 

   /*override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {  
        
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }


    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that processes and affects root motion
    }


    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that sets up animation IK (inverse kinematics)
    }*/
}
