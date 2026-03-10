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
    }

    public override TaskStatus OnUpdate()
    {
        // 1. Check if the attack animation is still playing
        if (anim != null)
        {
            // We get the current state of the Animator
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

            // If the Boss is still doing the "Fire Beem Magic" animation, lock him in place!
            if (stateInfo.IsName("Fire Beem Magic"))
            {
                if (agent != null)
                {
                    agent.isStopped = true;
                    agent.velocity = Vector3.zero; // Force him to stop sliding
                }

                // Return running so the tree waits for the attack to finish
                return TaskStatus.RUNNING;
            }
        }

        // 2. If he is NOT attacking, he is allowed to move and chase the player again
        if (agent != null)
        {
            agent.isStopped = false;
        }

        // 3. Update the tracking
        if (target == null || target.scene.rootCount == 0)
            target = GameObject.FindWithTag("Player");

        if (target == null || agent == null) return TaskStatus.FAILED;

        // Follow the player
        agent.SetDestination(target.transform.position);

        return TaskStatus.RUNNING;
    }
}