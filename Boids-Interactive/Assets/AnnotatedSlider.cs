using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AnnotatedSlider : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField] private Slider _slider;

    private void Update()
    {
        _valueText.text = _slider.value.ToString("F2");
    }
}
