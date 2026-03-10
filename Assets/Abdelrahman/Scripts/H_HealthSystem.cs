using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using cowsins; // Required for FPS Engine
// If you get an error here, uncomment the next line:
// using Padula.BehaviorBricks; 

public class HealthSystem : MonoBehaviour, IDamageable
{
    [Header("UI Reference")]
    public Slider healthSlider;

    [Header("Stats")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("AI Components")]
    public NavMeshAgent agent;

    // CHANGED: BehaviorTree -> BehaviorExecutor (For Behavior Bricks)
    public BehaviorExecutor behaviorExecutor;

    private void Start()
    {
        currentHealth = maxHealth;

        // 1. Setup Slider
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        // 2. Auto-find components if empty
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (behaviorExecutor == null)
            behaviorExecutor = GetComponent<BehaviorExecutor>();
    }

    // =========================================================
    // 1. FUNCTION FOR COWSINS FPS ENGINE
    // =========================================================
    public void Damage(float damage, bool isCritical)
    {
        TakeDamage(damage);
    }

    // =========================================================
    // 2. FUNCTION FOR YOUR SCRIPTS
    // =========================================================
    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0) return; // Don't hurt dead enemy

        currentHealth -= amount;

        // Update UI
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        // Check for Death
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"💀 ENEMY DIED: {gameObject.name}");

        // 1. Disable Collider
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // 2. Stop Movement (NavMesh)
        if (agent != null)
        {
            agent.isStopped = true; // Stop moving instantly
            agent.enabled = false;  // Disable the component
        }

        // 3. Stop Thinking (Behavior Bricks)
        if (behaviorExecutor != null)
        {
            // This stops the Behavior Bricks AI logic
            behaviorExecutor.enabled = false;
        }

        // 4. Play Animation
        Animator anim = GetComponentInChildren<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("die");
        }
        else
        {
            Destroy(gameObject);
        }
    }
}