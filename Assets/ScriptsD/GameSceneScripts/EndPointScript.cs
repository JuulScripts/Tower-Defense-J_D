using UnityEngine;
using UnityEngine.SceneManagement;

public class Endpoint : MonoBehaviour
{
    [SerializeField] private int health = 1000;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && !enemy.IsDead)
            {
                int damage = Mathf.CeilToInt(enemy.Health);
                health -= damage;

                Debug.Log($"Enemy deed {damage} damage. Endpoint health: {health}");

                Destroy(other.gameObject);

                if (health <= 0)
                {
                    GameOver();
                }
            }
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
