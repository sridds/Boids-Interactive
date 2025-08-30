using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Toggle _alignmentEnableToggle;
    [SerializeField] private Toggle _cohesionEnableToggle;
    [SerializeField] private Toggle _seperationEnableToggle;

    [SerializeField] private Slider _alignmentWeightSlider;
    [SerializeField] private Slider _cohesionWeightSlider;
    [SerializeField] private Slider _seperationWeightSlider;

    [SerializeField] private TextMeshProUGUI _collapseWindowText;
    [SerializeField] private RectTransform _settingsHolder;

    private bool isExpanded = true;

    private void Start()
    {
        ToggleUIVisibility();
    }

    public void ToggleUIVisibility()
    {
        isExpanded = !isExpanded;

        _settingsHolder.gameObject.SetActive(isExpanded);
        _collapseWindowText.text = !isExpanded ? "Expand" : "Collapse";
    }
}
