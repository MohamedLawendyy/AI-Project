using UnityEngine;
using cowsins; // Required for PlayerStats

public class ParticleDamage : MonoBehaviour
{
    [Header("Settings")]
    public float damagePerParticle = 5f;

    [Tooltip("How often can the player take damage from these particles? (Seconds)")]
    public float damageCooldown = 0.1f;
    private float nextDamageTime;

    // [Tooltip("Optional: Effect spawned at the point of impact")]
    // public GameObject hitEffect;

    // This is the magic function for Particle Systems
    private void OnParticleCollision(GameObject other)
    {
        // 1. Check if we hit the Player and if cooldown is over
        if (other.CompareTag("Player") && Time.time >= nextDamageTime)
        {
            // 2. Get the Cowsins PlayerStats component
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                // 3. Apply Damage
                playerStats.Damage(damagePerParticle, false);

                // Set the cooldown so we don't kill the player in 1 frame
                nextDamageTime = Time.time + damageCooldown;

                Debug.Log($"💨 Particle Hit! Damage: {damagePerParticle}");
            }

            // 4. (Optional) Spawn Hit Effect at the player's position
            //
        }
    }
}