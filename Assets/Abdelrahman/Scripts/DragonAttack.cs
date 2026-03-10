using UnityEngine;
using cowsins; // Required to talk to the Player

public class DragonAttack : MonoBehaviour
{
    [Header("Settings")]
    public float damageAmount = 20f;

    [Tooltip("If true, this object will destroy itself after hitting the player (useful for fireballs)")]
    public bool destroyOnImpact = false;

    [Tooltip("Optional: Particle effect when hitting player")]
    public GameObject hitEffect;

    private void OnTriggerEnter(Collider other)
    {
        // 1. Check if we hit the Player
        if (other.CompareTag("Player"))
        {
            // 2. Try to find the PlayerStats script (Cowsins Standard)
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            // 3. Apply Damage
            if (playerStats != null)
            {
                // We pass 'false' because a dragon attack is never a 'headshot'
                playerStats.Damage(damageAmount, false);
                Debug.Log($"🔥 Player hit by {gameObject.name}! Damage: {damageAmount}");
            }

            // 4. (Optional) Spawn Hit Effect
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            // 5. If this is a fireball, destroy it after hitting
            if (destroyOnImpact)
            {
                Destroy(gameObject);
            }
        }
    }
}