using UnityEngine;
using UnityEngine.AI;
using cowsins; // Required for the IDamageable interface

public class BossHealth : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public float health = 500f; // Bosses usually need more health!
    public bool isDead = false;

    [Header("Components")]
    private NavMeshAgent agent;
    private BehaviorExecutor behaviorExecutor;
    private Animator anim;

    private void Start()
    {
        // Auto-assign components
        agent = GetComponent<NavMeshAgent>();
        behaviorExecutor = GetComponent<BehaviorExecutor>();
        anim = GetComponent<Animator>();
    }

    // This is the function COWSINS calls when you shoot the boss
    public void Damage(float damage, bool isCritical)
    {
        TakeDamage(damage);
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;
        Debug.Log($"Boss took {amount} damage. Health left: {health}");

        if (health <= 0)
        {
            Die();
        }
        else
        {
            // Play hit animation if one exists
            if (anim != null) anim.SetTrigger("TakeDamage");
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("💀 Boss has been defeated!");

        // 1. Play Death Animation
        if (anim != null) anim.SetTrigger("Death");

        // 2. Stop the AI from moving or thinking
        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        if (behaviorExecutor != null)
        {
            behaviorExecutor.enabled = false;
        }

        // 3. Disable the collider so bullets go through the corpse
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // Optional: Destroy the boss after 5 seconds
        Destroy(gameObject, 5f);
    }
}