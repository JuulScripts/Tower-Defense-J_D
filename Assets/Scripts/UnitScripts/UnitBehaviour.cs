using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitBehaviour : MonoBehaviour
{
   

  private static void DamageEnemy( GameObject target, float damage , UnityEvent effect = null)
    {
      
        if (target != null) {
          
        Enemy enemy = target.GetComponent<Enemy>();
            if (effect != null && effect.GetPersistentEventCount() == 0)
            {
            enemy.DecreaseHealth(damage);
        }
        }
    }
    private static void DamageEnemys(List<GameObject> targets, float damage, UnityEvent effect = null)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            GameObject target = targets[i];
            // Do something with target

            if (target != null)
            {
          
                Enemy enemy = target.GetComponent<Enemy>();
                if (effect != null && effect.GetPersistentEventCount() == 0)
                {
                    enemy.DecreaseHealth(damage);
                    print(target.name);
                }
            }
        }

    }
    private static void BuffUnits(GameObject target, float multiplier)
    {
        if (target  != null) {
            if (target.CompareTag("Unit"))
            {
                Unit unit = target.GetComponent<Unit>();
                unit.damage *= multiplier;
            }
        }
    }


    public static void Attack(UnitParams data)
    {
        Debug.Log(data.effect);

        Action[] Attacks = {
        () => DamageEnemys(data.targets, data.number, data.effect),
        () => BuffUnits(data.target, data.number),
        () => DamageEnemy(data.target, data.number, data.effect)
    };

        AnimationHandler.Trigger("Attack", data.animator);
        Attacks[data.attackFunction].Invoke();
    }




}
