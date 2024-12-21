using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackState : IState
{
    private AIController aiController;

    public StateType Type => StateType.Attack;

    public AttackState(AIController aiController)
    {
        this.aiController = aiController;
    }

    public void Enter()
    {
		
        // aiController.Animator.SetBool("isAttacking", true);
        aiController.Agent.isStopped = true; // Stop the AI agent movement
    }

    public void Execute()
    {
        // Check if the player is within attack range
        if (Vector3.Distance(aiController.transform.position, aiController.Player.position) > aiController.AttackRange)
        {
            // If the player moves away, transition back to ChaseState
            aiController.StateMachine.TransitionToState(StateType.Chase);
            return;
        }

        // Restart the level since the player is "hit"
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        aiController.Animator.SetBool("isAttacking", true);
        aiController.Agent.isStopped = false; // Resume the AI agent movement
    }
}
