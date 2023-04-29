using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public static Action OnTurretSold;
    public static Action<Turret> OnTurretSelected;

    public enum AnimationState
    {
        Idle,
        Attacking,
        Upgrade_Idle,
        Upgrade_Attacking
    }
    public enum TurretType
    {
        Brain,
        Eye,
        Snail,
        Friend
    }

    [SerializeField] public AnimationState currentState; 
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private GameObject attackRangeSprite;
    private float rangeSize;
    private Vector3 rangeOriginalSize;

    public Enemy CurrentEnemyTarget { get; set; }
    public TurretUpgrade TurretUpgrade { get; set; }
    public float AttackRange => attackRange;
    public TurretType turretType;
    
    private bool _gameStarted;
    private bool _isBeingPlaced;
    float zPosition;
    private List<Enemy> _enemies;
    private Animator anim;

    private GameObject hitbox;
    public int friendBuffs;

    private void Start()
    {
        if (turretType == TurretType.Friend)
        {
            friendBuffs = 0;
        }
        currentState = AnimationState.Idle;
        _isBeingPlaced = true;
        zPosition = this.transform.position.z;

        _gameStarted = true;
        _enemies = new List<Enemy>();
        anim = GetComponent<Animator>();

        TurretUpgrade = GetComponent<TurretUpgrade>();
        hitbox = (this.transform.GetChild(0).gameObject).transform.GetChild(0).gameObject;

        rangeSize = attackRangeSprite.GetComponent<SpriteRenderer>().bounds.size.y;
        rangeOriginalSize = attackRangeSprite.transform.localScale;
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
                attackRangeSprite.SetActive(false);

                if (turretType == TurretType.Snail)
                {
                    transform.GetChild(1).tag = "SnailLight";
                }

                SetFriendBuffs();
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
                case AnimationState.Upgrade_Idle:
                    if (turretType == TurretType.Friend)
                    {
                        anim.SetInteger("animState", 0);
                    }
                    else
                    {
                        anim.SetInteger("animState", 2);
                    }
                    break;
                case AnimationState.Upgrade_Attacking:
                    anim.SetInteger("animState", 3);
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

    private void SetFriendBuffs()
    {
        GameObject[] friends = GameObject.FindGameObjectsWithTag("Friend");
        GameObject[] turrets = GameObject.FindGameObjectsWithTag("TurretHitbox");
        Vector3 target;
        Vector3 pointToCheck;
        float targetAngle;
        Vector3 hitboxSize = hitbox.GetComponent<TurretHitbox>().hitboxRenderer.size;

        for (int i = 0; i < friends.Length; i++)
        {
            friends[i].GetComponent<Turret>().ResetFriendBuffs();

            // Set the updated number of buffs for each friend
            for (int j = 0; j < turrets.Length; j++)
            {
                target = friends[i].transform.position - turrets[j].transform.position;
                targetAngle = Vector3.Angle(turrets[j].transform.position, target) * Mathf.Deg2Rad;

                if (friends[i].transform.position.y >= turrets[j].transform.position.y)
                {
                    pointToCheck = friends[i].transform.position + new Vector3(Mathf.Cos(targetAngle) * hitboxSize.x / 2, Mathf.Sin(targetAngle) * hitboxSize.y / 2 + .3f, 0);
                }
                else
                {
                    pointToCheck = friends[i].transform.position + new Vector3(Mathf.Cos(targetAngle) * hitboxSize.x / 2, Mathf.Sin(targetAngle) * -1 * hitboxSize.y / 2 - .3f, 0);
                }

                if (Vector3.Distance(turrets[j].transform.position, pointToCheck) <= this.attackRange)
                {
                    friends[i].GetComponent<Turret>().AddFriendBuff();
                }
            }
            friends[i].GetComponent<Turret>().RemoveFriendBuff();
            // Actually buff each friend

            // Large buff at 5 nearby towers
            if (friends[i].GetComponent<Turret>().friendBuffs >= 5)
            {
                friends[i].GetComponent<Turret>().attackRange = 4;
            }
            else
            {
                friends[i].GetComponent<Turret>().attackRange = 3;
            }

            // Medium buff at 3 nearby towers
            if (friends[i].GetComponent<Turret>().friendBuffs >= 3)
            {
                friends[i].GetComponent<MachineTurretProjectile>().spreadRange = 15;
            }
            else
            {
                friends[i].GetComponent<MachineTurretProjectile>().spreadRange = 20;
            }

            float currentDamage = 4f;
            // Small buff for each nearby tower
            for (int j = 0; j < friends[i].GetComponent<Turret>().friendBuffs; j++)
            {
                currentDamage += .1f;
            }
            friends[i].GetComponent<MachineTurretProjectile>().SetDamage(currentDamage);
        }
    }

    public void AddFriendBuff()
    {
        friendBuffs++;
    }

    public void RemoveFriendBuff()
    {
        friendBuffs--;
    }

    public void ResetFriendBuffs()
    {
        this.friendBuffs = 0;
    }

    private void OnDrawGizmos()
    {
        if (!_gameStarted)
        {
            GetComponent<CircleCollider2D>().radius = attackRange;
        }
        
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void CloseAttackRangeSprite()
    {
        attackRangeSprite.SetActive(false);
    }

    public void SelectTurret()
    {
        ShowTurretInfo();
        OnTurretSelected?.Invoke(this);
    }

    public void SellTurret()
    {

            CurrencySystem.Instance.AddCoins(this.TurretUpgrade.GetSellValue());
            Destroy(this.gameObject);
            attackRangeSprite.SetActive(false);
            OnTurretSold?.Invoke();
    }

    private void ShowTurretInfo()
    {
        attackRangeSprite.SetActive(true);
        attackRangeSprite.transform.localScale = rangeOriginalSize * AttackRange / (rangeSize / 2);
    }
}
