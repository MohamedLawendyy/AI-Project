using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;
using UnityEngine.AI;

[Action("MyGame/AttackLaser")]
public class Action_AttackLaser : GOAction
{
    [InParam("Target")] public GameObject target;

    private float timer = 0;
    private float attackDuration = 5.0f; // NOTE: Make sure this matches your animation length!

    private NavMeshAgent agent;
    private L_BossLaser laserScript;

    public override void OnStart()
    {
        if (target == null || target.scene.rootCount == 0)
            target = GameObject.FindWithTag("Player");

        base.OnStart();
        timer = 0;

        agent = gameObject.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.velocity = Vector3.zero; // GUARANTEE he stops sliding
        }

        laserScript = gameObject.GetComponentInChildren<L_BossLaser>();
        if (laserScript != null) laserScript.StartLaser();

        Animator anim = gameObject.GetComponent<Animator>();
        if (anim != null)
        {
            anim.ResetTrigger("Attack"); // Clear old triggers just in case
            anim.SetTrigger("Attack");   // Start the attack
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (target == null) return TaskStatus.FAILED;

        // Forced rotation
        Vector3 direction = (target.transform.position - gameObject.transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            gameObject.transform.rotation = Quaternion.Slerp(
                gameObject.transform.rotation,
                Quaternion.LookRotation(direction),
                Time.deltaTime * 10f
            );
        }

        timer += Time.deltaTime;

        // When timer finishes, STOP the action so the Repeat node can restart the tree
        if (timer >= attackDuration)
        {
            if (laserScript != null) laserScript.StopLaser();

            // Clean up the animator trigger so it doesn't get stuck
            gameObject.GetComponent<Animator>().ResetTrigger("Attack");

            return TaskStatus.COMPLETED; // This tells the tree "I am done attacking"
        }

        return TaskStatus.RUNNING;
    }
    public override void OnAbort()
    {
        if (laserScript != null) laserScript.StopLaser();
        base.OnAbort();
    }
}