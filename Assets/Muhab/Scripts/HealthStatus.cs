using System;
using UnityEngine;

public class HealthStatus : MonoBehaviour
{
    [SerializeField, Min(1)] private int maxHealth = 100;
    [SerializeField, Min(0)] private int currentHealth = 100;

    public int MaxHealth
    {
        get => maxHealth;
        set
        {
            maxHealth = Mathf.Max(1, value);
            SetCurrentHealthInternal(currentHealth);
        }
    }

    public int CurrentHealth => currentHealth;
    public bool IsDead => currentHealth == 0;

    public event Action<HealthStatus> Died;

    private bool deathEventRaised;

    private void Awake()
    {
        maxHealth = Mathf.Max(1, maxHealth);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        deathEventRaised = currentHealth == 0;
    }

    public void SetCurrentHealth(int value)
    {
        SetCurrentHealthInternal(value);
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || IsDead)
        {
            return;
        }

        SetCurrentHealthInternal(currentHealth - amount);
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || IsDead)
        {
            return;
        }

        SetCurrentHealthInternal(currentHealth + amount);
    }

    public void RestoreToFullHealth()
    {
        deathEventRaised = false;
        SetCurrentHealthInternal(maxHealth);
    }

    private void SetCurrentHealthInternal(int value)
    {
        currentHealth = Mathf.Clamp(value, 0, maxHealth);

        if (currentHealth == 0)
        {
            if (!deathEventRaised)
            {
                deathEventRaised = true;
                Died?.Invoke(this);
            }
        }
        else
        {
            deathEventRaised = false;
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        maxHealth = Mathf.Max(1, maxHealth);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
#endif
}
