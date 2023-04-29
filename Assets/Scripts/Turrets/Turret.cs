using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public enum AnimationState
    {
        Idle,
        Attacking
    }

    [SerializeField] public AnimationState currentState; 
    [SerializeField] private float attackRange = 3f;

    public Enemy CurrentEnemyTarget { get; set; }
    public TurretUpgrade TurretUpgrade { get; set; }
    public float AttackRange => attackRange;
    
    private bool _gameStarted;
    private bool _isBeingPlaced;
    float zPosition;
    private List<Enemy> _enemies;
    private Animator anim;

    private GameObject hitbox;

    private void Start()
    {
        currentState = AnimationState.Idle;
        _isBeingPlaced = true;
        zPosition = this.transform.position.z;

        _gameStarted = true;
        _enemies = new List<Enemy>();
        anim = GetComponent<Animator>();

        TurretUpgrade = GetComponent<TurretUpgrade>();
        hitbox = (this.transform.GetChild(0).gameObject).transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (_isBeingPlaced)
        {
            GetComponent<SpriteRenderer>().sortingOrder = -(1 * (int)Math.Round(transform.position.y)) + 6;
            if (Input.GetMouseButtonDown(0) && hitbox.GetComponent<TurretHitbox>().isPlaceable)
            {
                _isBeingPlaced = false;
                hitbox.GetComponent<TurretHitbox>().isBeingPlaced = false;
                hitbox.GetComponent<TurretHitbox>().SetHitboxColor("selected");
            }
            Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.transform.position = new Vector3(MousePosition.x, MousePosition.y, zPosition);
        }
        else
        {
            GetCurrentEnemyTarget();

            switch (currentState)
            {
                case AnimationState.Idle:
                    anim.SetInteger("animState", 0);
                    break;
                case AnimationState.Attacking:
                    anim.SetInteger("animState", 1);
                    break;
            }

            //RotateTowardsTarget();
        }
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
