using UnityEngine;
using UnityEngine.UI;

public class CountDownBar : MonoBehaviour
{
    [SerializeField] private float _current, _max, _min;
    [SerializeField] private Image _mask, _fill;

    private void Update()
    {
        CountDown();
    }

    private void CountDown()
    {
        if (_current > 0)
        {
            float currentOffset = _current - _min;
            float maxOffset = _max - _min;
            float fillAmount = currentOffset / maxOffset;
            _mask.fillAmount = fillAmount;
            _current -= Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
