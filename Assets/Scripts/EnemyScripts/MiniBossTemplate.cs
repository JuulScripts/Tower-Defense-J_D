using System;
using UnityEngine;

public class MiniBossTemplate : Enemy
{
     [SerializeField] private int attackwait; 
    private bool canattack = true;
    private Action customlogic;

    MiniBossTemplate(Action Function)
    {
        customlogic = Function;
    }

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
