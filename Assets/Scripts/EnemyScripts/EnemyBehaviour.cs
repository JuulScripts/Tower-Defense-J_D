using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;  
    private Enemy Enemy;
    private float MoveSpeed;
    private bool shouldmove;
    private void Start()
    {
        Enemy = GetComponent<Enemy>();
        MoveSpeed = Enemy.speed*Time.deltaTime;


    }

    private void MoveToWayPoint(GameObject waypoint)
    {
        if (shouldmove)
        {
            Vector3 Wpos = waypoint.transform.position;
            Vector3 direction = Wpos - transform.position;

            transform.position += direction.normalized * MoveSpeed;

        }
    }
    public void MoveToWayPoints()
    {
        
    }

    private void Update()
    {
        MoveToWayPoint (waypoints[0]);
    }

}
