using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationSync : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // This calculates how fast the agent is moving
        // and sends that value to the "Speed" parameter in your Animator
        float currentSpeed = agent.velocity.magnitude;
        
        // Use a small threshold to avoid "jitter"
        if (currentSpeed < 0.1f) currentSpeed = 0;

        anim.SetFloat("Speed", currentSpeed);
    }
}