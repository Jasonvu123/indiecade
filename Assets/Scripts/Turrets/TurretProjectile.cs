using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    [SerializeField] protected Transform projectileSpawnPosition;
    [SerializeField] public float delayBtwAttacks = 2f;
    [SerializeField] public float Damage = 2f;

    public float DelayPerShot { get; set; }
    
    protected float _nextAttackTime;
    protected ObjectPooler _pooler;
    protected Turret _turret;
    protected Projectile _currentProjectileLoaded;

    private void Start()
    {
        _turret = GetComponent<Turret>();
        _pooler = GetComponent<ObjectPooler>();

        DelayPerShot = delayBtwAttacks;
        LoadProjectile();
    }

    protected virtual void Update()
    {
        if (IsTurretEmpty())
        {
            LoadProjectile();
        }

        if (Time.time > _nextAttackTime)
        {
            if (_turret.CurrentEnemyTarget != null && _currentProjectileLoaded != null &&
                _turret.CurrentEnemyTarget.EnemyHealth.CurrentHealth > 0f)
            {
                _currentProjectileLoaded.transform.parent = null;
                _currentProjectileLoaded.SetEnemy(_turret.CurrentEnemyTarget);
                AudioManager.Instance.PlayerSound(AudioManager.Sound.rocket);
            }

            _nextAttackTime = Time.time + DelayPerShot;
        }
    }

    protected virtual  void LoadProjectile()
    {
        GameObject newInstance = _pooler.GetInstanceFromPool();
        newInstance.transform.localPosition = projectileSpawnPosition.position;
        newInstance.transform.SetParent(projectileSpawnPosition);

        _currentProjectileLoaded = newInstance.GetComponent<Projectile>();
        _currentProjectileLoaded.TurretOwner = this;
        _currentProjectileLoaded.ResetProjectile();
        _currentProjectileLoaded.Damage = Damage;
        newInstance.SetActive(true);
    }

    private bool IsTurretEmpty()
    {
        return _currentProjectileLoaded == null;
    }
    
    public void ResetTurretProjectile()
    {
        _currentProjectileLoaded = null;
    }
}