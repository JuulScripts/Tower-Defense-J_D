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
    private Animator animator;
    [Header("Targeting & Effects")]
    [SerializeField] private UnityEvent Effect;
    private GameObject target;
    private GameObject LookTarget;

    [Header("Hitbox Settings")]
    [SerializeField] private float hitboxsize;
    private CapsuleCollider hitbox;
    
    [Header("Internal State")]
    [SerializeField, Tooltip("Used to control hit cooldown internally")]
    private bool canhit = true;
    private List<GameObject> targets;
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



    private void Start()
    {
        hitbox = GetComponent<CapsuleCollider>();
        hitbox.radius = hitboxsize;
        animator = GetComponent<Animator>();
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
            Debug.Log("atacked");
           
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
   animator,
   Effect
);
                UnitBehaviour.Attack(unitParams);
                StartCoroutine(resethitcooldown());
            } 
        }
    }
    private void Update()
    {
        if (target != null)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
