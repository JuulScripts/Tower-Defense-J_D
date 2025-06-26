using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitParams
{
    public int attackFunction;
    public GameObject target;
    public List<GameObject> targets;
    public float number;
    public Animator[] animator;
    public UnityEvent<GameObject> effect;
    public MonoBehaviour routinerunner;
    public Unit.UnitTypes unittype;
     
    // Constructor to initialize all unit parameters for an attack v
    public UnitParams(int attackFunction, GameObject target, List<GameObject> targets, float number, Animator[] animator, MonoBehaviour routinerunner, Unit.UnitTypes unittype, UnityEvent<GameObject> effect = null)
    {
        this.attackFunction = attackFunction;
        this.target = target;
        this.targets = targets;
        this.number = number;
        this.animator = animator;
        this.effect = effect;
        this.routinerunner = routinerunner;
        this.unittype = unittype;
    }
}
