using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;
using UnityEngine.AI;

[Action("MyGame/TrackAndMove")]
public class Action_TrackAndMove : GOAction
{
    [InParam("Target")] public GameObject target;
    private NavMeshAgent agent;

    public override void OnStart()
    {
        base.OnStart();
        agent = gameObject.GetComponent<NavMeshAgent>();
        if (agent != null) agent.isStopped = false;
    }

    public override TaskStatus OnUpdate()
    {
        if (target == null || agent == null) return TaskStatus.FAILED;

        // Update the destination every single frame
        agent.SetDestination(target.transform.position);

        // This action never "completes" on its own, 
        // the Behavior Tree will interrupt it when IsWithinRange becomes true.
        return TaskStatus.RUNNING;
    }
}