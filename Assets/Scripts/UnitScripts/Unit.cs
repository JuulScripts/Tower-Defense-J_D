using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.UI.CanvasScaler;

public class Unit : MonoBehaviour
{

    [Header("Attack Settings")]
    [SerializeField] private AttackTypes attackType;
    public float damage;
    [SerializeField] private float hitcooldown;
    [SerializeField] private float rotationSpeed;
    private Animator[] animators;
    [Header("Targeting & Effects")]
    [SerializeField] private UnityEvent<GameObject> Effect;
    private GameObject target;
    private GameObject LookTarget;
    public UnitTypes unitType;
    EffectFunctions effectHandler;
    [Header("Internal State")]
    [SerializeField, Tooltip("Used to control hit cooldown internally")]
    private bool canhit = true;
    private List<GameObject> targets;
    [Header("Misc")]
    [SerializeField] public GameObject upgradedunit;
    public static event Action<GameObject> OnUnitUpgraded;
    [Header("Upgrade Metadata")]
    public string unitName;


    public enum states
    {
        idle,
        attacking,
    }


    public enum AttackTypes
    {
        MultiTarget,
        Heal,
        SingleTarget
    }

    public enum UnitTypes
    {
        None,
        Special
    }

    private void Start()
    {
        animators = GetComponentsInChildren<Animator>();
        targets = new List<GameObject>();
        effectHandler =  GetComponent<EffectFunctions>();
        effectHandler.setunit(this);
    }

    private void OnTriggerEnter(Collider other) // Sets initial targets for single-target and look tracking
    {


        if (attackType == AttackTypes.SingleTarget && target == null)
        {
            target = other.gameObject;
        }
        if (LookTarget == null)
        {
            LookTarget = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other) // Clears targets when exiting collider
    {
        LookTarget = null;

        if (target == other.gameObject)
        {
            target = null;
        }

        if (targets != null && targets.Contains(other.gameObject))
        {
            targets.Remove(other.gameObject);
        }
    }
    private IEnumerator resethitcooldown() // Waits for cooldown duration and resets hit availability
    {
        yield return new WaitForSeconds(hitcooldown);
        canhit = true;
    }
    private void OnTriggerStay(Collider other) // Triggers attack logic while enemies remain in range
    {


        if (other.CompareTag("Enemy") && other.GetComponent<Enemy>().enemytype == Enemy.EnemyTypes.None || unitType == Unit.UnitTypes.Special)
        {


            if (attackType != AttackTypes.SingleTarget && !targets.Contains(other.gameObject))
            {
                target = other.gameObject;
                targets.Add(target);
                print(targets);
            }
            if (canhit == true)
            {
                canhit = false;
                UnitParams unitParams = new UnitParams(
   (int)attackType,
   target,  
   targets,
   damage,
   animators,
   this,
   unitType,
   Effect
);
                UnitBehaviour.Attack(unitParams);
                StartCoroutine(resethitcooldown());
            }
        }
    }

    public void Upgrade() // Replaces this unit with an upgraded version and destroys current unit
    {
        GameObject newUnit = Instantiate(upgradedunit, transform.position, transform.rotation);
        OnUnitUpgraded?.Invoke(newUnit);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (target != null)
        {
            foreach (Transform trans in transform)
            {
                GameObject character = trans.gameObject;

                Vector3 direction = (target.transform.position - trans.transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(direction);


                Vector3 euler = targetRotation.eulerAngles;
                euler.x = 0f;
                targetRotation = Quaternion.Euler(euler);


                trans.transform.rotation = Quaternion.Lerp(trans.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            }
        }
    }
}