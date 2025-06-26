using System.Collections;
using System.Collections.Generic;
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
    public List<EnemyStates> enemysubstate = new List<EnemyStates> { };
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
        poisoned,
        stunned
    }


    
    private void Start()
    {
        animator = GetComponent<Animator>();
        move();
    }
    public EnemyStates state = EnemyStates.Idle;


    public void DecreaseHealth(float amount) // Reduces health and calls Die() if health drops to 0 or below
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
    
    private void Die() // Handles enemy death state, animation, sound, and starts deletion coroutine
    {
        isdead = true;
        state = Enemy.EnemyStates.Idle;
        AnimationHandler.SetTrueBool("IsDead", animator);
        SoundManager.Instance.PlaySFX(SoundManager.Instance.enemyDeathSound);
        StartCoroutine(DeleteEnemy());
    }

    private IEnumerator DeleteEnemy() // Waits for the death animation to finish, then adds money and kills to the UI, and finally destroys the enemy object
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
    public void move() // Sets enemy state to Moving and updates animator parameter
    {
        state = EnemyStates.Moving;
  
        print((int)state);
        animator.SetInteger("State", (int)state);
    }
}
