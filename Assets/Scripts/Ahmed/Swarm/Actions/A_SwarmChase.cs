using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;
using UnityEngine.AI;
[Action("Swarm Enemy/Chase")]
public class A_SwarmChase : GOAction
{
    private NavMeshAgent agent;
    private Vector3 lastGoalPos;
    public override void OnStart()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        gameObject.GetComponent<A_SwarmAnimationController>().StopAttack();
        if (A_SwarmSpawnManager.instance.Goal != null)
        {
            lastGoalPos = A_SwarmSpawnManager.instance.Goal.transform.position;
            agent.SetDestination(lastGoalPos);
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (A_SwarmSpawnManager.instance.Goal == null || agent == null)
            return TaskStatus.FAILED;

        Vector3 goalPos = A_SwarmSpawnManager.instance.Goal.transform.position;

        if ((goalPos - lastGoalPos).sqrMagnitude >= 0.2f)
        {
            lastGoalPos = goalPos;
            agent.SetDestination(lastGoalPos);
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance+0.2f)
            return TaskStatus.COMPLETED;

        return TaskStatus.RUNNING;
    }
}