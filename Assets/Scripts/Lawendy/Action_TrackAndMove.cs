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
    private Animator anim;

    public override void OnStart()
    {
        if (target == null || target.scene.rootCount == 0)
            target = GameObject.FindWithTag("Player");

        base.OnStart();

        agent = gameObject.GetComponent<NavMeshAgent>();
        anim = gameObject.GetComponent<Animator>();

        // SAFETY: If the agent isn't on the mesh at the start, try to snap it to the nearest valid point.
        if (agent != null && !agent.isOnNavMesh)
        {
            if (NavMesh.SamplePosition(gameObject.transform.position, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
            }
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (agent == null) return TaskStatus.FAILED;

        // 1. Critical Check: Is the agent actually on the NavMesh?
        if (!agent.isOnNavMesh)
        {
            // If it's not on the mesh, we can't give it orders. 
            // We return running or failed to prevent the error spam.
            return TaskStatus.RUNNING;
        }

        // 2. Check if the attack animation is still playing
        if (anim != null)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("Fire Beem Magic"))
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
                return TaskStatus.RUNNING;
            }
        }

        // 3. If he is NOT attacking, he is allowed to move
        agent.isStopped = false;

        // 4. Update the tracking
        if (target == null || target.scene.rootCount == 0)
            target = GameObject.FindWithTag("Player");

        if (target == null) return TaskStatus.FAILED;

        // Follow the player
        agent.SetDestination(target.transform.position);

        return TaskStatus.RUNNING;
    }
}