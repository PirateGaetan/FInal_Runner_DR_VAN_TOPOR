using UnityEngine;
using UnityEngine.UI;

public class GreenSliderManager : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void InitGreenSlider(float value)
    {
        SetGreenSlider(value);
    }
    public void SetGreenSlider(float value)
    {
        slider.value = value;
    }
}
