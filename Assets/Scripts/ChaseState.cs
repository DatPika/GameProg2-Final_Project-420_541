public class ChaseState : IState
{
    private AIController aiController;

    public StateType Type => StateType.Chase;

    public ChaseState(AIController aiController)
    {
        this.aiController = aiController;
    }

    public void Enter()
    {
	//    aiController.Animator.SetBool("isChasing", true);
        // No animations, so no need to set any animator parameters
    }

    public void Execute()
    {
        if (!aiController.CanSeePlayer())
        {
            aiController.StateMachine.TransitionToState(StateType.Patrol);
            return;
        }

        if (aiController.IsPlayerInAttackRange())
        {
            aiController.StateMachine.TransitionToState(StateType.Attack);
            return;
        }

        aiController.Agent.destination = aiController.Player.position;
    }

    public void Exit()
    {
        // No cleanup necessary
    }
}
