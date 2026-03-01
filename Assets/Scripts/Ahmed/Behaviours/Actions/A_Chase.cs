using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;
using UnityEngine.AI;
[Action("Zombie Swarm Enemy Actions/Chase")]
public class A_Chase : GOAction
{
    [InParam("ZombieSwarmManager")] public A_ZombieSwarmManager SwarmManager;
    [InParam("PlayerTarget")] public GameObject Target;
    private NavMeshAgent agent;
    private Vector3 lastTargetPos;
    public override void OnStart()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        if (Target != null)
        {
            lastTargetPos = Target.transform.position;
            agent.SetDestination(lastTargetPos);
        }
    }
    public override TaskStatus OnUpdate()
    {
        if (Target == null || agent == null)
            return TaskStatus.FAILED;

        if ((Target.transform.position - lastTargetPos).sqrMagnitude >= 0.4f)
        {
            lastTargetPos = Target.transform.position;
            agent.SetDestination(lastTargetPos);
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            return TaskStatus.COMPLETED;
        SwarmManager.SetTargetPosition(gameObject.transform.position);
        return TaskStatus.RUNNING;
    }
}