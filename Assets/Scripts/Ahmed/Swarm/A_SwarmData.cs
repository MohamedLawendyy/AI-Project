using UnityEngine;
public class A_SwarmData : MonoBehaviour
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
    public void Damage(int BulletDamage)
    {
        CurrentHealth -= BulletDamage;
        if (CurrentHealth <= Health * 0.5f && !FirstStun)
        {
            IsStunned = true;
            FirstStun = true;
        }
    }
}