using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.Rendering.DebugUI;

public class UnitBehaviour : MonoBehaviour
{

    public GameObject currenttarget;
    public GameObject[] currenttargets;
    private static GameObject DamageEnemy(GameObject target, float damage, Unit.UnitTypes unit, UnityEvent<GameObject> effect = null) // Damages a single enemy and optionally applies a status effect.
    {

        if (target != null)
        {
        
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy.enemytype == Enemy.EnemyTypes.None || unit == Unit.UnitTypes.Special)
            {
                enemy.DecreaseHealth(damage);
                if (effect != null && effect.GetPersistentEventCount() >  0 && target != null)
                    {
                    string funcname = effect.GetPersistentMethodName(0);
                    Action<GameObject> listener = EffectFunctions.GetFunctionViaString(funcname);
                    if (listener != null)
                    {
                        UnityEvent<GameObject> NewEvent = new UnityEvent<GameObject> { };
                        NewEvent.AddListener(new UnityAction<GameObject>(listener));
                        NewEvent.Invoke(target);
                    }
                }
            }
            return target;
        }
        return null;
    }
    private static void DamageEnemys(List<GameObject> targets, float damage, Unit.UnitTypes unit, UnityEvent<GameObject> effect = null) // Damages multiple enemies and optionally applies a status effect to each.
    {
        for (int i = 0; i < targets.Count; i++)
        {
            GameObject target = targets[i];
            // Do something with target
            print(targets);
            if (target != null)
            {
                Enemy enemy = target.GetComponent<Enemy>();
                if (enemy.enemytype == Enemy.EnemyTypes.None || unit == Unit.UnitTypes.Special)
                {
                    enemy.DecreaseHealth(damage);
                    if (effect != null && effect.GetPersistentEventCount() > 0 && target != null)
                    {
                        string funcname = effect.GetPersistentMethodName(0);
                        Action<GameObject> listener = EffectFunctions.GetFunctionViaString(funcname);
                        if (listener != null)
                        {
                            UnityEvent<GameObject> NewEvent = new UnityEvent<GameObject> { };
                            NewEvent.AddListener(new UnityAction<GameObject>(listener));
                            NewEvent.Invoke(target);
                        }
                  
                    
                 
                        
                    }
                }
            }

        }
    }
    private static void BuffUnits(GameObject target, float multiplier) // Increases the damage of a unit by a given multiplier.
    {

        if (target != null)
        {
            if (target.CompareTag("Unit"))
            {
                Unit unit = target.GetComponent<Unit>();
                unit.damage *= multiplier;
            }
        }
    }
    private static void TriggerAnimations(Animator[] animators) // Triggers the "Attack" animation on a set of animators.
    {
        if (animators == null || animators.Length == 0)
        {
            Debug.LogWarning("No animators provided.");
            return;
        }

        foreach (Animator animator in animators)
        {
            if (animator == null)
            {
                Debug.LogError("Animator is null in array!");
                continue;
            }

            AnimationHandler.Trigger("Attack", animator);
        }
    }

    private static void SetAnimators(GameObject[] targets) //  Updates animator states to match the current state of associated enemies.
    {
        foreach (GameObject target in targets)
        {

            target.GetComponent<Animator>().SetInteger("State", (int)target.GetComponent<Enemy>().state);
        }
    }
    private static void SetAnimator(GameObject target) // Updates a single animator�s state to match its enemy�s state.
    {
            target.GetComponent<Animator>().SetInteger("State", (int)target.GetComponent<Enemy>().state);   
    }


    public static void Attack(UnitParams data) //  Executes an attack based on unit parameters, including applying damage, buffs, effects, and triggering animations and sound.
    {



            Func<object>[] Attacks = {
                  () => { DamageEnemys(data.targets, data.number, data.unittype, data.effect); return null; },
                () => { BuffUnits(data.target, data.number); return null; },
                () => DamageEnemy(data.target, data.number, data.unittype, data.effect)
        };


            TriggerAnimations(data.animator);
           object returnedvalue = Attacks[data.attackFunction].Invoke();

        if (returnedvalue == null)
        { 
            return;
        }
        if (returnedvalue is GameObject[]) SetAnimators((GameObject[])returnedvalue);
        if (returnedvalue is GameObject) SetAnimator((GameObject)returnedvalue);

        SoundManager.Instance.PlaySFX(SoundManager.Instance.attackSound);
    }
    }