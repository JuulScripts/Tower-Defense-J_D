using System;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private int rotationSpeed;
    private Enemy Enemy;
    private float MoveSpeed;
    private bool shouldmove = true;
    private int currentindex;

    private void Start()
    {
        Enemy = GetComponent<Enemy>();
        MoveSpeed = Enemy.speed*Time.deltaTime;


    }

    private void MoveToWayPoints()
    {
        GameObject waypoint;
      
        if (currentindex >= 0 && currentindex < waypoints.Length)
        {
            waypoint = waypoints[currentindex];
        } else
        {
            Debug.LogError($"{currentindex} is out of bounds. Please provide an currentindex within the range of the 'waypoints' array.");
            return;
        }

        if (Enemy.state == Enemy.EnemyStates.Moving && (transform.position - waypoint.transform.position).magnitude >= 0.1)
        {
         
            Vector3 Wpos = waypoint.transform.position;
            Vector3 direction = Wpos - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.position += direction.normalized * MoveSpeed;

        } else if (Enemy.state == Enemy.EnemyStates.Moving && currentindex+1 >= 0 && currentindex+1 < waypoints.Length) {
            currentindex++;
        }
    }
  

    private void Update()
    {
        MoveToWayPoints();

    }

}
