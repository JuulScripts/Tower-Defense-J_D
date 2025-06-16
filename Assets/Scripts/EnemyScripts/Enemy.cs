using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health;
     public int speed;
    private Animator animator;



    public enum EnemyStates
    {
        Moving,
        Attacking,
        Idle
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        move();
    }
    public EnemyStates state = EnemyStates.Idle;
    public void DecreaseHealth(float amount)
    {
        health -= amount;
    }
    



    public void move()
    {
        state = EnemyStates.Moving;
        print((int)state);
        animator.SetInteger("State", (int)state);
    }
}
