using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health;
     public int speed;


    public void DecreaseHealth(float amount)
    {
        health -= amount;
    }


    public void move()
    {

    }
}
