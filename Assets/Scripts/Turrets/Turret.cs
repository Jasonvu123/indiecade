using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private float attackRange = 3f;

    public Enemy CurrentEnemyTarget { get; set; }
    public TurretUpgrade TurretUpgrade { get; set; }
    public float AttackRange => attackRange;
    
    private bool _gameStarted;
    private bool _isBeingPlaced;
    float zPosition;
    private List<Enemy> _enemies;

    private void Start()
    {
        _isBeingPlaced = true;
        zPosition = this.transform.position.z;

        _gameStarted = true;
        _enemies = new List<Enemy>();

        TurretUpgrade = GetComponent<TurretUpgrade>();
    }

    private void Update()
    {
        if (_isBeingPlaced)
        {
            
            Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.transform.position = new Vector3(MousePosition.x, MousePosition.y, zPosition);
        }
        else
        {
            GetCurrentEnemyTarget();
            //RotateTowardsTarget();
        }
    }

    private void OnMouseDown()
    {
        _isBeingPlaced = false;
    }

    private void GetCurrentEnemyTarget()
    {
        if (_enemies.Count <= 0)
        {
            CurrentEnemyTarget = null;
            return;
        }

        CurrentEnemyTarget = _enemies[0];
    }

    private void RotateTowardsTarget()
    {
        if (CurrentEnemyTarget == null)
        {
            return;
        }
        Vector3 targetPos = CurrentEnemyTarget.transform.position - transform.position;
        float angle = Vector3.SignedAngle(transform.up, targetPos, transform.forward);
        transform.Rotate(0f, 0f, angle);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy newEnemy = other.GetComponent<Enemy>();
            _enemies.Add(newEnemy);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (_enemies.Contains(enemy))
            {
                _enemies.Remove(enemy);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!_gameStarted)
        {
            GetComponent<CircleCollider2D>().radius = attackRange;
        }
        
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
