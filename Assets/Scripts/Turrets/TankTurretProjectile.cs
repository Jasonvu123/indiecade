using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTurretProjectile : TurretProjectile
{
    protected override void Update()
    {
        if (Time.time > _nextAttackTime)
        {
            if (_turret.CurrentEnemyTarget != null 
                && _turret.CurrentEnemyTarget.EnemyHealth.CurrentHealth > 0)
            {
                FireProjectile(_turret.CurrentEnemyTarget);

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

    private void FireProjectile(Enemy enemy)
    {
        GameObject instance = _pooler.GetInstanceFromPool();
        instance.transform.position = projectileSpawnPosition.position;

        Projectile projectile = instance.GetComponent<Projectile>();
        _currentProjectileLoaded = projectile;
        _currentProjectileLoaded.TurretOwner = this;
        _currentProjectileLoaded.ResetProjectile();
        _currentProjectileLoaded.SetEnemy(enemy);
        _currentProjectileLoaded.Damage = Damage;
        instance.SetActive(true);
        AudioManager.Instance.PlayerSound(AudioManager.Sound.bigBullet);
    }
}
