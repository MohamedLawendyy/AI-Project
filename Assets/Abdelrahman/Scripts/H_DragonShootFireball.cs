using UnityEngine;
using UnityEngine.AI;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("Dragon/FlyAndShoot")]
public class DragonFlyAndShoot : GOAction
{
    [InParam("Target")]
    public GameObject target;

    [InParam("Time Between Shots")]
    public float fireRate = 3.0f;

    private NavMeshAgent agent;
    private Animator anim;

    private float timer;

    public override void OnStart()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        anim = gameObject.GetComponentInChildren<Animator>();

        // 1. FLY UP
        if (agent)
        {
            agent.isStopped = true; // Stop moving
            agent.baseOffset = 2.0f; // Lift up
        }
        if (anim) anim.SetBool("IsFlying", true);

        // 2. Set timer to shoot immediately
        timer = fireRate;
    }

    public override TaskStatus OnUpdate()
    {
        if (target == null) return TaskStatus.FAILED;

        // 1. ROTATE TO FACE PLAYER (Added this Fix)
        Vector3 direction = (target.transform.position - gameObject.transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        // 2. TRIGGER ANIMATION LOOP
        timer += Time.deltaTime;

        if (timer >= fireRate)
        {
            timer = 0;
            // The "DragonEvents" script on the Dragon handles the fireball spawning
            if (anim) anim.SetTrigger("Trig_Fireball");
        }

        return TaskStatus.RUNNING;
    }

    public override void OnAbort()
    {
        base.OnAbort();
        // Force Land when interrupted
        if (agent)
        {
            agent.baseOffset = 0f;
            agent.isStopped = false;
        }
        if (anim) anim.SetBool("IsFlying", false);
    }
}