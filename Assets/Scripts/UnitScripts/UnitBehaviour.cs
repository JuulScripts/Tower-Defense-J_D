using System;
using UnityEngine;
using UnityEngine.Events;

public class UnitBehaviour : MonoBehaviour
{
   

  private static void DamageEnemy( GameObject target, float damage , UnityEvent effect = null)
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (effect == null)
        {
            enemy.DecreaseHealth(damage);
        }
    }
    private static void BuffUnits(GameObject target, float multiplier)
    {
        if (target.CompareTag("Unit"))
        {
            Unit unit = target.GetComponent<Unit>();
            unit.damage *= multiplier;
        }
    }


   public static void Attack(int attackfunction, GameObject target, float Number, Animator animator, UnityEvent effect = null)
    {

        Action[] Attacks = {
        () => DamageEnemy(target, Number, effect),
        () => BuffUnits(target, Number),
        () =>  DamageEnemy(target, Number, effect)
        };

        UnitAnimationHandler.Trigger("Attack", animator);
        Attacks[attackfunction].Invoke();
    }



}
