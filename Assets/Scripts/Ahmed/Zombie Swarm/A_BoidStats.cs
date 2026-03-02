using UnityEngine;
public class A_BoidStats : MonoBehaviour
{
    public int Health;
    public int CurrentHealth;
    public int AttackDamage;
    public bool IsLeader;
    private void Start()
    {
        CurrentHealth = Health;
    }
    public void Damage(int BulletDamage)
    {
        CurrentHealth -= BulletDamage;
        if (CurrentHealth <= Health * 0.5f)
        {
            A_ZombieFlockManager.instance.LastLeader = gameObject;
            A_ZombieFlockManager.instance.Leader = A_ZombieFlockManager.instance.Boids[Random.Range(0, A_ZombieFlockManager.instance.Count)];
        }
        if (CurrentHealth <= 0)
        {
            //A_ZombieFlockManager.instance.Boids.Remove(gameObject);
            //Destroy(gameObject, 3.0f);
        }
    }
}