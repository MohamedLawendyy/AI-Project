using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;
using UnityEngine.AI;
[Action("Zombie Swarm Enemy Actions/Chase")]
public class A_Chase : GOAction
{
    private NavMeshAgent agent;
    public override void OnStart()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        if (A_ZombieFlockManager.instance.Target != null)
            agent.SetDestination(A_ZombieFlockManager.instance.Target.position);
    }
    public override TaskStatus OnUpdate()
    {
        if (A_ZombieFlockManager.instance.Target == null || agent == null)
            return TaskStatus.FAILED;

        if ((A_ZombieFlockManager.instance.Target.position - A_ZombieFlockManager.instance.Target.position).sqrMagnitude >= 0.4f)
            agent.SetDestination(A_ZombieFlockManager.instance.Target.position);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            return TaskStatus.COMPLETED;

        return TaskStatus.RUNNING;
    }
}