using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{

    [Header("Attack Settings")]
    [SerializeField] private AttackTypes attackType;
    public float damage;
    [SerializeField] private float hitcooldown;
    [SerializeField] private float rotationSpeed;
    private Animator[] animators;
    [Header("Targeting & Effects")]
    [SerializeField] private UnityEvent Effect;
    private GameObject target;
    private GameObject LookTarget;
    public UnitTypes unitType;

    
    [Header("Internal State")]
    [SerializeField, Tooltip("Used to control hit cooldown internally")]
    private bool canhit = true;
    private List<GameObject> targets;
    [Header("Misc")]
    [SerializeField] private GameObject upgradedunit;
    
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
    }

    private void OnTriggerEnter(Collider other)
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

    private void OnTriggerExit(Collider other)
    {
        LookTarget = null;
   
        if (target == other.gameObject)
        {
            target = null;
        }
        if (targets.Contains(other.gameObject))
        {
            targets.Remove(other.gameObject);
        }
    }
    private IEnumerator resethitcooldown()
    {
        yield return new WaitForSeconds(hitcooldown);
        canhit = true;
    }
    private void OnTriggerStay(Collider other)
    {


        if (other.CompareTag("Enemy") )
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

    public void Upgrade()
    {
        Instantiate(upgradedunit, transform.position, transform.rotation);
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
