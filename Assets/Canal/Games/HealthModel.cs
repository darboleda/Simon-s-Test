using UnityEngine;
using System.Collections;

using Canal.Unity;

public class HealthModel : Behavior {

    public int MaxHealth;
    private int currentHealth;

    public void Awake()
    {
        currentHealth = MaxHealth;
    }

    public int ReduceHealth(int reduction)
    {
        currentHealth = Mathf.Clamp(currentHealth - reduction, 0, MaxHealth);
        return currentHealth;
    }
}
