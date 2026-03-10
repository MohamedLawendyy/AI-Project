using cowsins;
using UnityEngine;

public class L_BossLaser : MonoBehaviour
{
    [Header("Fire VFX Settings")]
    // Changed from VisualEffect to ParticleSystem to support the Procedural Fire asset
    public ParticleSystem fireParticles;
    public Transform laserOrigin;

    [Header("Damage Settings")]
    public int damage = 20;
    public float maxLength = 20f;
    public float damageCooldown = 0.5f; // How often the fire damages the player (0.5 = twice per second)

    private bool isFiring = false;
    private float hitTimer = 0f;

    private GameObject player;
    private PlayerStats playerStats;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();
        }

        if (fireParticles != null)
        {
            fireParticles.Stop();
        }
    }

    void Update()
    {
        // 1. If the animation hasn't started the fire yet, do nothing! (Prevents invisible damage)
        if (!isFiring) return;

        // 2. Shoot the invisible raycast to detect the player
        Ray ray = new Ray(laserOrigin.position, laserOrigin.forward);
        RaycastHit hit;

        // Optional: Draws a red line in the Scene View so you can see where the boss is aiming
        Debug.DrawRay(laserOrigin.position, laserOrigin.forward * maxLength, Color.red);

        if (Physics.Raycast(ray, out hit, maxLength))
        {
            if (hit.collider.CompareTag("Player"))
            {
                // 3. Use a timer so we don't hit the player 60 times per second
                hitTimer += Time.deltaTime;
                if (hitTimer >= damageCooldown)
                {
                    Debug.Log("Player Hit by Fire!");

                    if (playerStats != null)
                    {
                        playerStats.Damage(damage, false);
                    }

                    // TODO: Call your player's damage script here
                    // hit.collider.GetComponent<PlayerHealth>().TakeDamage(10);

                    hitTimer = 0f; // Reset timer after dealing damage
                }
            }
        }
    }

    // --- ANIMATION EVENTS ---

    // Put this on the frame where the boss opens his mouth/hand
    public void StartLaser()
    {
        isFiring = true;
        hitTimer = damageCooldown; // Make sure the first hit happens instantly

        if (fireParticles != null)
        {
            fireParticles.Play(); // Start the fire asset
        }
    }

    // Put this on the frame where the boss finishes his attack
    public void StopLaser()
    {
        isFiring = false;

        if (fireParticles != null)
        {
            fireParticles.Stop(); // Stop the fire asset
        }
    }
}