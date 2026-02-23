using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;
using UnityEngine.AI;
[Action("Zombie Swarm Enemy Actions/Chase")]
public class A_Chase : GOAction
{
    [InParam("Target")] public Transform Target;
    private NavMeshAgent agent;
    private Vector3 lastTargetPos;
    private readonly Vector3 targetScale = Vector3.one;
    private readonly float scaleSpeed = 5.0f;
    public override void OnStart()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        if (Target != null)
        {
            lastTargetPos = Target.position;
            agent.SetDestination(lastTargetPos);
        }
    }
    public override TaskStatus OnUpdate()
    {
        gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale,
                                                       targetScale,
                                                       Time.deltaTime * scaleSpeed);

        if (Vector3.Distance(gameObject.transform.localScale, targetScale) < 0.01f)
            gameObject.transform.localScale = targetScale;

        if (Target == null || agent == null)
            return TaskStatus.FAILED;

        if ((Target.position - lastTargetPos).sqrMagnitude >= 0.4f)
        {
            lastTargetPos = Target.position;
            agent.SetDestination(lastTargetPos);
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            return TaskStatus.COMPLETED;

        return TaskStatus.RUNNING;
    }
}