using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;
using UnityEngine.AI;
[Action("Zombie Swarm Enemy/Boids/ReturnToLeader")]
public class A_ReturnToLeader : GOAction
{
    private NavMeshAgent agent;
    private Vector3 lastTargetPos;
    public override void OnStart()
    {
        gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.enabled = true;
        if (A_ZombieFlockManager.instance.Leader != null)
        {
            lastTargetPos = A_ZombieFlockManager.instance.Leader.transform.position;
            agent.SetDestination(lastTargetPos);
        }
    }
    public override TaskStatus OnUpdate()
    {
        if (A_ZombieFlockManager.instance.Leader == null || agent == null)
            return TaskStatus.FAILED;

        if ((A_ZombieFlockManager.instance.Leader.transform.position - lastTargetPos).sqrMagnitude >= 0.2f)
        {
            lastTargetPos = A_ZombieFlockManager.instance.Leader.transform.position;
            agent.SetDestination(lastTargetPos);
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.enabled = false;
            return TaskStatus.COMPLETED;
        }

        return TaskStatus.RUNNING;
    }
}