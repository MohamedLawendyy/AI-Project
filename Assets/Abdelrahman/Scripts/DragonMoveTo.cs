using UnityEngine;
using UnityEngine.AI;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("Dragon/MoveTo")]
public class DragonMoveTo : GOAction
{
    [InParam("Target")]
    public GameObject target;

    // Add this to make him faster in the inspector
    [InParam("Speed")]
    public float chaseSpeed = 12.0f;

    private NavMeshAgent agent;
    private Animator anim;

    public override void OnStart()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        anim = gameObject.GetComponentInChildren<Animator>();

        if (agent)
        {
            // CRITICAL FIX: Make sure we are allowed to move
            agent.isStopped = false;
            agent.speed = chaseSpeed;
            agent.baseOffset = 0f; // Ensure we are on the ground
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (target == null) return TaskStatus.FAILED;

        // 1. MOVE
        agent.SetDestination(target.transform.position);

        // 2. FORCE LOOK AT PLAYER (Fixes "Doesn't look at player")
        // We manually turn the dragon to face the target while running
        Vector3 direction = (target.transform.position - gameObject.transform.position).normalized;
        direction.y = 0; // Keep head level
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            // Rotate fast (10f)
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookRotation, Time.deltaTime * 10f);
        }

        // 3. ANIMATION
        if (anim != null)
        {
            anim.SetFloat("Speed", agent.velocity.magnitude);
        }

        // 4. STOPPING CONDITION
        // Only stop if we are VERY close (Stopping Distance)
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (anim) anim.SetFloat("Speed", 0);
            return TaskStatus.COMPLETED;
        }

        return TaskStatus.RUNNING;
    }

    public override void OnAbort()
    {
        base.OnAbort();
    }
}