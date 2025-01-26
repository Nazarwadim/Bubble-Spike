using System;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private Score _score;

    [SerializeField] private TextMeshProUGUI _textMeshPro;

    void Start()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        _score.ScoreChanged += OnScoreChanged;
    }

    private void OnDisable()
    {
        _score.ScoreChanged -= OnScoreChanged;
    }

    private void OnScoreChanged(int amount)
    {
        _textMeshPro.text = "Score: " + amount.ToString();
    }
}
