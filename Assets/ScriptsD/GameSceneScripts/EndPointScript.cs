using UnityEngine;
using UnityEngine.SceneManagement;

public class Endpoint : MonoBehaviour
{
    [SerializeField] private int health = 1000;
    public int CurrentHealth => health;

    private void OnTriggerEnter(Collider other) // Handles enemy collision, reduces endpoint health based on enemy health, plays sound, and triggers game over if health reaches zero.
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
                SoundManager.Instance.PlaySFX(SoundManager.Instance.endpointDamageSound);

                if (health <= 0)
                {
                    GameOver();
                }
            }
        }
    }

    private void GameOver() // Plays loss sound, logs game over, sets game result to failure, and loads the start scene.
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.lossSound);
        Debug.Log("Game Over!");
        GameResult.SetResult(false);
        SceneManager.LoadScene("StartScene");
    }
}
