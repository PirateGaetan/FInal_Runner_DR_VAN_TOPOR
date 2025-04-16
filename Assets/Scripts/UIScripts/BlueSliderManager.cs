using UnityEngine;
using UnityEngine.UI;

public class BlueSliderManager : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void InitBlueSlider(float value)
    {
        SetBlueSlider(value);
    }
    public void SetBlueSlider(float value)
    {
        slider.value = value;
    }
}
