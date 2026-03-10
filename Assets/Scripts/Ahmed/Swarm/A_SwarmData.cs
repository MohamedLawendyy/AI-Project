using cowsins;
using UnityEngine;
public class A_SwarmData : MonoBehaviour, IDamageable
{
    public int Health;
    public int CurrentHealth;
    public int AttackDamage;
    public bool IsStunned { get; set; }
    private bool FirstStun = false;
    private void Awake()
    {
        CurrentHealth = Health;
    }
    public void Damage(float damage, bool isHeadshot)
    {
        CurrentHealth -= (int)damage;
        if (CurrentHealth <= Health * 0.5f && !FirstStun)
        {
            IsStunned = true;
            FirstStun = true;
        }
    }
}