using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health;
    public float Health => health;
    public float speed;
    private Animator animator;
    [SerializeField] private int value;
    [Header("Enemy Type")]
    public EnemyTypes enemytype;
    private bool isdead = false; 
    public bool IsDead => isdead;

    public enum EnemyTypes
    {
        None, 
        Special
    }
    public enum EnemyStates
    {
        Moving,
        Attacking,
        Idle,
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        move();
    }
    public EnemyStates state = EnemyStates.Idle;


    public void DecreaseHealth(float amount)
    {
        if (!isdead)
        {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
        }
    }
    
    private void Die()
    {
        isdead = true;
        state = Enemy.EnemyStates.Idle;
        AnimationHandler.SetTrueBool("IsDead", animator);
        StartCoroutine(DeleteEnemy());
    }

    private IEnumerator DeleteEnemy()
    {
        yield return new WaitForSeconds(AnimationHandler.GetWaitTime(animator) + 0.5f);

        GameUIManager uiManager = FindFirstObjectByType<GameUIManager>();
        if (uiManager != null)
        {
            uiManager.AddMoney(value);
            uiManager.AddKill(); 
        }

        PlayerHandling.AddMoney(value);

        Destroy(gameObject);
    }
    public void move()
    {
        state = EnemyStates.Moving;
        print((int)state);
        animator.SetInteger("State", (int)state);
    }
}
