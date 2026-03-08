using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;
[Action("Swarm Enemy/Stun")]
public class A_SwarmStun : GOAction
{
    private A_SwarmData state;
    private A_SwarmAnimationController AnimationController;
    private float stunTimer;
    private readonly float stunDuration = 2f;
    public override void OnStart()
    {
        stunTimer = 0f;
        state = gameObject.GetComponent<A_SwarmData>();
        AnimationController = gameObject.GetComponent<A_SwarmAnimationController>();
        AnimationController.Stun();
        gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = true;
    }
    public override TaskStatus OnUpdate()
    {
        stunTimer += Time.deltaTime;

        if (stunTimer >= stunDuration)
        {
            state.IsStunned = false;
            gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = false;
            AnimationController.StopStun();
            return TaskStatus.COMPLETED;
        }
        return TaskStatus.RUNNING;
    }
}