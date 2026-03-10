using UnityEngine;
using UnityEngine.AI;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("Dragon/MeleeAttack")]
public class DragonMeleeAttack : GOAction
{
    [InParam("Target")]
    public GameObject target;

    [InParam("Animation Trigger")]
    public string animTrigger = "Trig_Attack"; // Type "Bite" or "TailWhip" here

    [InParam("Damage Delay")]
    public float damageDelay = 0.5f;

    [InParam("Total Duration")]
    public float duration = 1.5f;

    private NavMeshAgent agent;
    private Animator anim;
    private float timer;
    private bool damageDealt = false;

    public override void OnStart()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        anim = gameObject.GetComponentInChildren<Animator>();
        timer = 0;
        damageDealt = false;

        // --- SAFETY CHECK ---
        // Only try to stop the agent if it is actually alive and on the floor
        if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.updateRotation = false;
        }
        else
        {
            Debug.LogWarning("Dragon is NOT on NavMesh! Physics might have pushed him.");
        }

        // Play Animation
        if (anim)
        {
            anim.SetFloat("Speed", 0);
            anim.SetTrigger(animTrigger);
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (target == null) return TaskStatus.FAILED;

        timer += Time.deltaTime;

        // 3. ROTATE TO FACE PLAYER
        if (timer < damageDelay + 0.2f)
        {
            Vector3 direction = (target.transform.position - gameObject.transform.position).normalized;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookRotation, Time.deltaTime * 15f);
            }
        }

        // Inside OnUpdate...
        if (!damageDealt && timer >= damageDelay)
        {
            float dist = Vector3.Distance(gameObject.transform.position, target.transform.position);
            if (dist < 5.0f)
            {
                // FIND HEALTH SYSTEM ON TARGET
                HealthSystem hp = target.GetComponent<HealthSystem>();
                if (hp != null)
                {
                    hp.TakeDamage(20); // Deal 20 damage
                    Debug.Log("Player Bitten!");
                }
            }
            damageDealt = true;
        }
        // 5. FINISH
        if (timer >= duration)
        {
            return TaskStatus.COMPLETED;
        }

        return TaskStatus.RUNNING;
    }

    public override void OnAbort()
    {
        base.OnAbort();
        if (agent) agent.isStopped = false;
    }
}