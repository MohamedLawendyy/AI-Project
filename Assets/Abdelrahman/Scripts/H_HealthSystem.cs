using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System;
using cowsins;
using JetBrains.Annotations; // Required for FPS Engine
// If you get an error here, uncomment the next line:
// using Padula.BehaviorBricks; 

public class HealthSystem : MonoBehaviour, IDamageable
{
    [Header("UI Reference")]
    public Slider healthSlider;

    [Header("Stats")]
    
    public float maxHealth = 100f;

    [ReadOnly]
    public float currentHealth;
    public bool destroyOnDeath = true;

    public event Action OnDeath;
    public bool IsDead { get { return currentHealth <= 0; }}


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
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

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
        OnDeath?.Invoke();

        if (gameObject.layer == LayerMask.NameToLayer("Grass"))
            return;

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

            if (destroyOnDeath)
                Destroy(gameObject, 4f); // Give death animation time before removing object
        }
        else
        {
            if (destroyOnDeath)
                Destroy(gameObject);
        }
    }
}