using UnityEngine;
using UnityEngine.Events;

public class BossHealth : MonoBehaviour
{
    public float health = 100f;
    public bool isDead = false;

    public void TakeDamage(float amount)
    {
        if (isDead) return;
        health -= amount;
        
        if (health <= 0)
        {
            isDead = true;
            GetComponent<Animator>().SetTrigger("Death");
        }
        else
        {
            GetComponent<Animator>().SetTrigger("TakeDamage");
        }
    }
}
