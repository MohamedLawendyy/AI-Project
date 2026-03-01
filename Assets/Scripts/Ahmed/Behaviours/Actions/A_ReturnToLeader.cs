using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;
using UnityEngine.AI;
[Action("Zombie Swarm Enemy Actions/ReturnToLeader")]
public class A_ReturnToLeader : GOAction
{
    private A_ZombieSwarmManager SwarmManager;
    private NavMeshAgent agent;
    private Vector3 lastTargetPos;
    public override void OnStart()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        SwarmManager = gameObject.GetComponent<A_ZombieBoid>().SwarmManager;
        if (SwarmManager.Leader.transform.position != null)
        {
            lastTargetPos = SwarmManager.Leader.transform.position;
            agent.SetDestination(lastTargetPos);
        }
    }
    public override TaskStatus OnUpdate()
    {
        if (SwarmManager.Leader.transform.position == null || agent == null)
            return TaskStatus.FAILED;

        if ((SwarmManager.Leader.transform.position - lastTargetPos).sqrMagnitude >= 0.4f)
        {
            lastTargetPos = SwarmManager.Leader.transform.position;
            agent.SetDestination(lastTargetPos);
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            return TaskStatus.COMPLETED;

        return TaskStatus.RUNNING;
    }
}