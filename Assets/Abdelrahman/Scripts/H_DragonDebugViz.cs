using UnityEngine;

public class DragonBossGizmos : MonoBehaviour
{
    [Header("Drag Player Here")]
    public Transform playerTarget;

    [Header("Match These to Your Behavior Tree!")]
    [Tooltip("Range < 4: Dragon will Bite/Tail Whip")]
    public float meleeRange = 4.0f; 

    [Tooltip("Range < 25: Dragon will Fly & Spit Fire")]
    public float fireballRange = 25.0f;

    [Tooltip("Range < 50: Dragon will Run/Chase")]
    public float chaseRange = 50.0f;

    // This draws the shapes in the Scene View
    private void OnDrawGizmos()
    {
        // 1. Draw Chase Range (Green Ring) - Far
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        // 2. Draw Fireball Range (Yellow Ring) - Middle
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, fireballRange);

        // 3. Draw Melee Range (Red Ring) - Close
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);

        // 4. Draw Logic Line to Player
        if (playerTarget != null)
        {
            float dist = Vector3.Distance(transform.position, playerTarget.position);
            
            // Change the line color based on what the Dragon is thinking
            if (dist <= meleeRange) 
            {
                Gizmos.color = Color.red; // STATE: Melee Attack
            }
            else if (dist <= fireballRange) 
            {
                Gizmos.color = Color.yellow; // STATE: Flying Fireball
            }
            else if (dist <= chaseRange) 
            {
                Gizmos.color = Color.green; // STATE: Chasing
            }
            else 
            {
                Gizmos.color = Color.gray; // STATE: Idle / Too Far
            }

            // Draw a line from Dragon eyes to Player
            Vector3 eyePos = transform.position + Vector3.up * 2;
            Gizmos.DrawLine(eyePos, playerTarget.position);
        }
    }
}