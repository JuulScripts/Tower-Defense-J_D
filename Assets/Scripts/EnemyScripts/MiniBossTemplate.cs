using System;
using UnityEngine;
using UnityEngine.Events;

public class MiniBossTemplate : Enemy
{
     [SerializeField] private int attackwait; 
    private bool canattack = true;
    [SerializeField] private UnityAction customlogic;

 

    private void Attack()
    {
        customlogic();
    }



    public void Hit()
    {
        if (attackwait > 0 && canattack == true)
        {
            canattack = false;
            Attack();    
        }
    }
}
