using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine.AI;
[Action("Zombie Swarm Enemy/Boids/FollowLeader")]
public class A_FollowLeader : GOAction
{
    private NavMeshAgent agent;
    public override void OnStart()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.enabled = true;
        agent.SetDestination(gameObject.transform.position);
        agent.ResetPath();
        agent.enabled = false;
    }
    public override TaskStatus OnUpdate()
    {
        return TaskStatus.RUNNING;
    }
}