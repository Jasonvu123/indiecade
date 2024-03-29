﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineTurretProjectile : TurretProjectile
{
    [SerializeField] private bool isDualMachine;
    [SerializeField] public float spreadRange;
    
    protected override void Update()
    {
        if (Time.time > _nextAttackTime)
        {
            if (_turret.CurrentEnemyTarget != null)
            {
                Vector3 dirToTarget = _turret.CurrentEnemyTarget.transform.position 
                    - transform.position;
                FireProjectile(dirToTarget);

                if (GetComponent<Turret>().currentState == Turret.AnimationState.Idle)
                {
                    GetComponent<Turret>().currentState = Turret.AnimationState.Attacking;
                }
                else if (GetComponent<Turret>().currentState == Turret.AnimationState.Upgrade_Idle)
                {
                    GetComponent<Turret>().currentState = Turret.AnimationState.Upgrade_Attacking;
                }
                
            }
            
            _nextAttackTime = Time.time + delayBtwAttacks;
        }
        else
        {
            if (GetComponent<Turret>().currentState == Turret.AnimationState.Attacking)
            {
                GetComponent<Turret>().currentState = Turret.AnimationState.Idle;
            }
            else if (GetComponent<Turret>().currentState == Turret.AnimationState.Upgrade_Attacking)
            {
                GetComponent<Turret>().currentState = Turret.AnimationState.Upgrade_Idle;
            }
        }
    }

    protected override void LoadProjectile() { }

    private void FireProjectile(Vector3 direction)
    {
        GameObject instance = _pooler.GetInstanceFromPool();
        instance.transform.position = projectileSpawnPosition.position;

        MachineProjectile projectile = instance.GetComponent<MachineProjectile>();
        projectile.Direction = direction;
        projectile.Damage = Damage;
        AudioManager.Instance.PlayerSound(AudioManager.Sound.machineBullet);

        if (isDualMachine)
        {
            float randomSpread = Random.Range(-spreadRange, spreadRange);
            Vector3 spread = new Vector3(0f, 0f, randomSpread);
            Quaternion spreadValue = Quaternion.Euler(spread);
            Vector2 newDirection = spreadValue * direction;
            projectile.Direction = newDirection;
            AudioManager.Instance.PlayerSound(AudioManager.Sound.machineBullet);
        }

        instance.SetActive(true);
    }

    public void SetDamage(float damage)
    {
        this.Damage = damage;
    }

    public float GetDamage()
    {
        return this.Damage;
    }
}
