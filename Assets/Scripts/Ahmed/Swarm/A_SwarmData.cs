using UnityEngine;
public class A_SwarmData : MonoBehaviour
{
    public float Health;
    public float CurrentHealth;
    public int AttackDamage;
    public bool IsStunned { get; set; }
    private bool FirstStun = false;
    private HealthSystem HS;
    private void Awake()
    {
        CurrentHealth = Health;
        HS = GetComponent<HealthSystem>();
        Health = HS.maxHealth;
        CurrentHealth = Health;
    }
    private void Update()
    {
        CurrentHealth = HS.currentHealth;
        if (CurrentHealth <= Health * 0.5f && !FirstStun)
        {
            IsStunned = true;
            FirstStun = true;
        }
    }
    //public void Damage(int BulletDamage)
    //{
    //    CurrentHealth -= BulletDamage;
    //    if (CurrentHealth <= Health * 0.5f && !FirstStun)
    //    {
    //        IsStunned = true;
    //        FirstStun = true;
    //    }
    //}
}