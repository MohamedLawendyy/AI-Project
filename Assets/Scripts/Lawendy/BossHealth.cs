using UnityEngine;
using UnityEngine.Events;

public class BossHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField]
    [Tooltip("The maximum health of the boss.")]
    private float maxHealth = 100f;

    [SerializeField]
    [Tooltip("The current health of the boss. You can read this in the Inspector.")]
    private float currentHealth;

    [Header("Damage Settings")]
    [SerializeField]
    [Tooltip("Tag of the GameObject that represents the sword.")]
    private string swordTag = "Sword";

    [SerializeField]
    [Tooltip("Damage dealt by a single sword hit.")]
    private float swordDamage = 20f;

    [SerializeField]
    [Tooltip("Tag of the GameObject that represents a bullet.")]
    private string bulletTag = "Bullet";

    [SerializeField]
    [Tooltip("Damage dealt by a single bullet hit.")]
    private float bulletDamage = 10f;

    [Header("Events")]
    [Tooltip("Event triggered when the boss takes damage.")]
    public UnityEvent onTakeDamage;

    [Tooltip("Event triggered when the boss's health reaches zero.")]
    public UnityEvent onDeath;

    private bool isDead = false;

    private void Awake()
    {
        // Initialize current health to max health at start
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Function to apply damage to the boss.
    /// </summary>
    /// <param name="damageAmount">The amount of damage to deduct.</param>
    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        // Invoke damage event (useful for linking to particles or UI in the Inspector)
        onTakeDamage?.Invoke();
        Debug.Log($"{gameObject.name} took {damageAmount} damage. Current Health: {currentHealth}");

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log($"{gameObject.name} has been defeated!");
        
        // Invoke death event (useful for triggering death animation or destroying the object via Inspector)
        onDeath?.Invoke();
        
        // Optional: Destroy(gameObject); -> Can be linked to OnDeath event instead to play animation first.
    }

    //---------------------------------------------------------
    // Collision Detections (3D Physics)
    //---------------------------------------------------------
    
    private void OnCollisionEnter(Collision collision)
    {
        HandleHit(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleHit(other.gameObject);
    }

    //---------------------------------------------------------
    // Collision Detections (2D Physics) - Include just in case
    //---------------------------------------------------------
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleHit(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleHit(collision.gameObject);
    }

    //---------------------------------------------------------
    // Shared Hit Logic
    //---------------------------------------------------------

    private void HandleHit(GameObject hitObj)
    {
        // Check if the collided object has the specific tags and apply damage accordingly
        if (hitObj.CompareTag(swordTag))
        {
            TakeDamage(swordDamage);
        }
        else if (hitObj.CompareTag(bulletTag))
        {
            TakeDamage(bulletDamage);
        }
    }
}
