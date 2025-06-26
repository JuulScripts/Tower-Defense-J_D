using System;
using UnityEngine;
using UnityEngine.Events;

public class MiniBossTemplate : Enemy
{
     [SerializeField] private int attackwait; 
    private bool canattack = true;
    [SerializeField] private UnityAction customlogic;

 

    private void Attack() // Executes the assigned custom attack logic
    {
        customlogic();
    }



    public void Hit() // Triggers an attack if allowed based on cooldown flag and attack wait time
    {
        if (attackwait > 0 && canattack == true)
        {
            canattack = false;
            Attack();    
        }
    }
}
