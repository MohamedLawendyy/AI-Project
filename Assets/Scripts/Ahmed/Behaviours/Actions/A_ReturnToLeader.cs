using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;
using UnityEngine.AI;
[Action("Zombie Swarm Enemy Actions/ReturnToLeader")]
public class A_ReturnToLeader : GOAction
{
    private NavMeshAgent agent;
    private Vector3 lastTargetPos;
    public override void OnStart()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        if (A_ZombieFlockManager.instance.Leader.position != null)
        {
            lastTargetPos = A_ZombieFlockManager.instance.Leader.position;
            agent.SetDestination(lastTargetPos);
        }
    }
    public override TaskStatus OnUpdate()
    {
        if (A_ZombieFlockManager.instance.Leader.position == null || agent == null)
            return TaskStatus.FAILED;

        if ((A_ZombieFlockManager.instance.Leader.position - lastTargetPos).sqrMagnitude >= 0.4f)
        {
            lastTargetPos = A_ZombieFlockManager.instance.Leader.position;
            agent.SetDestination(lastTargetPos);
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            return TaskStatus.COMPLETED;

        return TaskStatus.RUNNING;
    }
}