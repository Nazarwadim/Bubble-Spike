using UnityEngine;
using UnityEngine.UI;

public class ProgressBarView : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private float _max = 1;

    [SerializeField] private ScoreProgress _score;

    private void OnEnable()
    {
        _score.ProgressChanged += SetProgress;
    }

    private void OnDisable()
    {
        _score.ProgressChanged -= SetProgress;
    }

    public void SetProgress(int progress)
    {
        float value = progress / _max;
        value = Mathf.Clamp01(value);
        _fillImage.fillAmount = value;
    }
}