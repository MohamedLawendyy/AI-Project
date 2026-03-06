using UnityEngine;
public class A_BoidStats : MonoBehaviour
{
    public int Health;
    public int CurrentHealth;
    public int AttackDamage;
    [HideInInspector] public bool IsLeader;

    private Rigidbody RB;
    private Animator AnimationController;
    private void Start()
    {
        RB = GetComponent<Rigidbody>();
        AnimationController = GetComponent<Animator>();
        CurrentHealth = Health;
    }
    private void Update()
    {
        AnimationController.SetFloat("Speed", RB.linearVelocity.magnitude);
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