using UnityEngine;

public class DragonEvents : MonoBehaviour
{
    [Header("Fireball Settings")]
    public GameObject fireballPrefab;
    public Transform mouthPoint;
    public Transform playerTarget;

    [Header("Earthquake Settings")]
    public CrackControll earthquakeController;
    public Transform handBone;

    // --- FIREBALL EVENT ---
    public void SpawnFireballFrame()
    {
        if (fireballPrefab != null && mouthPoint != null)
        {
            GameObject fb = Instantiate(fireballPrefab, mouthPoint.position, Quaternion.identity);

            // Ignore collision with dragon
            Collider fbCol = fb.GetComponent<Collider>();
            Collider dragonCol = GetComponent<Collider>();
            if (fbCol != null && dragonCol != null) Physics.IgnoreCollision(fbCol, dragonCol);

            // Aim at player
            if (playerTarget != null) fb.transform.LookAt(playerTarget.position + Vector3.up * 1.5f);
            else fb.transform.rotation = mouthPoint.rotation;
        }
    }

    // --- EARTHQUAKE EVENT ---
    public void TriggerEarthquakeEvent()
    {
        if (earthquakeController != null && handBone != null)
        {
            // 1. Calculate the spawn position (at Dragon's feet level)
            Vector3 spawnPos = handBone.position;
            spawnPos.y = transform.position.y;

            // 2. Detach the Earthquake object from any parent!
            // This prevents it from moving if the Dragon or Player moves.
            earthquakeController.transform.SetParent(null);

            // 3. Move the effect to the hit position
            earthquakeController.transform.position = spawnPos;

            // 4. Rotate it ONCE to face the player's current spot
            if (playerTarget != null)
            {
                // We calculate the direction only ONE time.
                Vector3 direction = (playerTarget.position - spawnPos).normalized;
                direction.y = 0; // Keep it flat on the ground

                if (direction != Vector3.zero)
                {
                    earthquakeController.transform.rotation = Quaternion.LookRotation(direction);
                }
            }
            else
            {
                earthquakeController.transform.rotation = transform.rotation;
            }

            // 5. Trigger the effect
            earthquakeController.TriggerCrack();
        }
        else
        {
            Debug.LogError("Earthquake Controller or Hand Bone is MISSING in DragonEvents!");
        }
    }
}