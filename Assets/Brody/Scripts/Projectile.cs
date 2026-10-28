using UnityEngine;
using UnityEngine.SceneManagement;

public class Projectile : MonoBehaviour
{
    private ScoreManager scoreManager;

    void Start()
    {
        scoreManager = FindAnyObjectByType<ScoreManager>();
    }

    private void OnDestroy()
    {
        scoreManager.AddScore(100);
    }
}
