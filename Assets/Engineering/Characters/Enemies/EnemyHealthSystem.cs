using Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour, ITarget, IHealthSystem
{
    [SerializeField] HealthData healthData;
    [SerializeField] string _unitName;
    [SerializeField] TargetType targetType;
    public string Name => unitName;
    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
    public bool IsDead => isDead;

    public event EventHandler<HitDataContainer> OnTargetHit;

    string unitName;
    int maxHealth;
    int currentHealth;
    bool isDead;


    private void Start() {
        InitializeHealth();
    }


    public void Damage(int value) {
        throw new System.NotImplementedException();
    }

    public void Death() {
        throw new System.NotImplementedException();
    }

    public void Heal(int value) {
        throw new System.NotImplementedException();
    }

    public void Hit(HitDataContainer damage) {
        OnTargetHit.Invoke(this, damage);

        this.Damage(damage.GetTotalDamage());
    }

    public TargetType GetTargetType() { return targetType; }

    
    void InitializeHealth() {
        this.maxHealth = healthData.maxHealth;
        this.currentHealth = maxHealth;
        this.unitName = _unitName;
    }
}
