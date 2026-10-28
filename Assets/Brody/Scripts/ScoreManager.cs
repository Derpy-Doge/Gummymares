using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TMP_Text ScoreText;
    private int score = 0;

    public void AddScore(int amount)
    {
        score += amount;
        ScoreText.SetText("Score: " + score); 
    }
}
