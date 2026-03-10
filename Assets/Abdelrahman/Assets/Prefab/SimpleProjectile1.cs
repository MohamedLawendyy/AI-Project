using UnityEngine;
using cowsins; // Required for PlayerStats

public class SimpleProjectile : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 5f;
    public float damage = 30f;
    public GameObject explosionEffect;

    void Start()
    {
        // Destroy bullet after 5 seconds if it hits nothing
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // --- THE DEBUGGER ---
        // This line prints EXACTLY what stopped the bullet
        Debug.Log($"🛑 BULLET HIT OBJECT: '{other.name}' || TAG: '{other.tag}' || LAYER: '{LayerMask.LayerToName(other.gameObject.layer)}'");

        // 1. IGNORE DRAGON PARTS
        // If the bullet hits the dragon, or the dragon's arm, ignore it.
        if (other.CompareTag("Dragon") || other.CompareTag("Enemy") || other.CompareTag("BodyShot"))
        {
            Debug.Log("... Ignoring Dragon/Enemy collision.");
            return;
        }

        // 2. HIT PLAYER
        if (other.CompareTag("Player"))
        {
            PlayerStats player = other.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.Damage(damage, false);
                Debug.Log("✅ HIT PLAYER!");
            }
        }

        // 3. EXPLODE
        // If we reached this line, we hit something valid (Floor, Wall, Player)
        Debug.Log($"💥 EXPLODING because I hit: {other.name}");

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}