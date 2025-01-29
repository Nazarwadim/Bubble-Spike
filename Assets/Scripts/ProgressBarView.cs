using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarView : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private float _max = 1;
    [SerializeField] private float _smoothTime = 0.7f;

    [SerializeField] private ScoreProgress _score;

    private float _targetFillAmount;
    private Coroutine _smoothFillCoroutine;

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
        _targetFillAmount = Mathf.Clamp01(progress / _max);

        if (_smoothFillCoroutine == null)
        {
            _smoothFillCoroutine = StartCoroutine(SmoothFill());
        }
    }

    private IEnumerator SmoothFill()
    {
        while (true)
        {
            _fillImage.fillAmount = Mathf.Lerp(_fillImage.fillAmount, _targetFillAmount, Time.deltaTime / _smoothTime);

            if (Mathf.Abs(_fillImage.fillAmount - _targetFillAmount) < 0.01f)
            {
                _fillImage.fillAmount = _targetFillAmount;
                _smoothFillCoroutine = null;
                yield break;
            }
            yield return null;
        }
    }
}