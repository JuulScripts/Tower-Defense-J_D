using System;
using System.Collections;
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
       
        if (other.CompareTag("Enemy") && canhit == true)
        {
            Debug.Log("atacked");
            canhit = false;
            if (attackType != AttackTypes.SingleTarget)
            {
                target = other.gameObject;
                
            }
            //  int attackfunction, GameObject target, float Number, Action effect = null
      
            UnitBehaviour.Attack((int)attackType, target, damage, animator, Effect);
            StartCoroutine(resethitcooldown());
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
