using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.EventSystems.EventTrigger;

public class UnitBehaviour : MonoBehaviour
{
   

  private static void DamageEnemy( GameObject target, float damage , Unit.UnitTypes unit ,UnityEvent effect = null)
    {
      
        if (target != null) {
          
        Enemy enemy = target.GetComponent<Enemy>();
            if (enemy.enemytype == Enemy.EnemyTypes.None || unit == Unit.UnitTypes.Special) {
                if (effect != null && effect.GetPersistentEventCount() == 0)
                {
                    enemy.DecreaseHealth(damage);
                }
        }
        }
    }
    private static void DamageEnemys(List<GameObject> targets, float damage,Unit.UnitTypes unit ,UnityEvent effect = null)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            GameObject target = targets[i];
            // Do something with target

            if (target != null)
            {
                Enemy enemy = target.GetComponent<Enemy>();
                if (enemy.enemytype == Enemy.EnemyTypes.None || unit == Unit.UnitTypes.Special)
                {
                    if (effect != null && effect.GetPersistentEventCount() == 0)
                    {
                        enemy.DecreaseHealth(damage);
                        print(target.name);
                    }
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
    private static void TriggerAnimations(Animator[] animators, MonoBehaviour routinerunner)
    {
        foreach (Animator animator in animators)
        {
            Debug.Log("isAttacking: " + animator.GetBool("Attack"));
         
         
            AnimationHandler.Trigger("Attack",animator,  routinerunner);
        }
     
    }

    private static void CheckEnemyType(Enemy enemy, Unit.UnitTypes unittype)
    {
      
    }
    public static void Attack(UnitParams data)
    {
     
        
        Debug.Log(data.effect);

        Action[] Attacks = {
        () => DamageEnemys(data.targets, data.number, data.unittype ,data.effect),
        () => BuffUnits(data.target, data.number),
        () => DamageEnemy(data.target, data.number, data.unittype ,data.effect)
    };

   
      
            TriggerAnimations(data.animator, data.routinerunner);
            Attacks[data.attackFunction].Invoke();
       
    }
}
