using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;
using UnityEngine.AI;
[Action("Zombie Swarm Enemy/Leader/LeaderHunt")]
public class A_LeaderHunt : GOAction
{
    private NavMeshAgent agent;
    private Vector3 lastGoalPos;
    public override void OnStart()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        if (A_ZombieFlockManager.instance.Goal != null)
        {
            lastGoalPos = A_ZombieFlockManager.instance.Goal.transform.position;
            agent.SetDestination(lastGoalPos);
        }
        A_ZombieFlockManager.instance.isAttacking = false;
    }
    public override TaskStatus OnUpdate()
    {
        if (A_ZombieFlockManager.instance.Goal == null || agent == null)
            return TaskStatus.FAILED;

        if ((A_ZombieFlockManager.instance.Goal.transform.position - lastGoalPos).sqrMagnitude >= 0.2f)
        {
            lastGoalPos = A_ZombieFlockManager.instance.Goal.transform.position;
            agent.SetDestination(lastGoalPos);
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            return TaskStatus.COMPLETED;

        return TaskStatus.RUNNING;
    }
}