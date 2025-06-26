using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EffectFunctions : MonoBehaviour
{

    public enum Functions
    {
        Poison,
        Stun
    }

    static public Dictionary<Functions, Action<GameObject>> functionmap = new Dictionary<Functions, Action<GameObject>>();

    void Awake()
    {
        functionmap = new Dictionary<Functions, Action<GameObject>>()
        {
            { Functions.Poison,  Poison },
            { Functions.Stun, Stun },
        };
    }
    public Unit unit { get; private set; }

     static public Action<GameObject> GetFunctionViaString(string funcname) // Converts a string to an enum and returns the corresponding function
    {
        if (Enum.TryParse<Functions>(funcname, out Functions result))
        {
             return functionmap[result];
        }

        return null;
    }

    public void setunit(Unit newunit) // Assigns the unit to be used by this effect system
    {
        unit = newunit;
    }

    public void Poison(GameObject Target)  // Applies poison to an enemy if not already poisoned
    {
        Enemy enemy = Target.GetComponent<Enemy>();

        if (!enemy.enemysubstate.Contains(Enemy.EnemyStates.poisoned))
        {
            enemy.enemysubstate.Add(Enemy.EnemyStates.poisoned);
            StartCoroutine(ApplyPoison(enemy, unit.damage/3, 1f, 3));
        }

    }
    private IEnumerator ApplyPoison(Enemy enemy, float damagePerTick, float interval, int ticks)  // Repeatedly damages the enemy over time and removes poison state
    {
      
     

        for (int i = 0; i <= ticks; i++)
        {
            Debug.Log("Poisond " + i);
            enemy.DecreaseHealth(damagePerTick);
            yield return new WaitForSeconds(interval);
        }

        enemy.enemysubstate.Remove(Enemy.EnemyStates.poisoned);
    }

    public void Stun(GameObject target) // Applies stun to an enemy if not already stunned
    {
        print("stunned");
        Enemy enemy = target.GetComponent<Enemy>();
      
        if (!enemy.enemysubstate.Contains(Enemy.EnemyStates.stunned))
        {
            enemy.enemysubstate.Add(Enemy.EnemyStates.stunned);
            enemy.state = Enemy.EnemyStates.Idle;
            StartCoroutine(resetstun(enemy));
        }
       
    }

    private IEnumerator resetstun(Enemy enemy) // Resets the enemy's state after a delay, allowing it to move again
    {
        yield return new WaitForSeconds(1);
        enemy.state = Enemy.EnemyStates.Moving;
        yield return new WaitForSeconds(2);
        enemy.enemysubstate.Remove(Enemy.EnemyStates.stunned);
  
    }
 }
