using UnityEngine;
using UnityEngine.UI;

public class PurpleSliderManager : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private GameManager gameManager;  
    private void Start()
    {
        gameManager.OnLevelChanged += CheckVisibility;
        CheckVisibility(gameManager.GetCurrentLevel());
    }
    private void CheckVisibility(GameManager.GameLevel level)
    {
        slider.gameObject.SetActive(level == GameManager.GameLevel.Level3);
    }

    public void InitPurpleSlider(float value)
    {
        SetPurpleSlider(value);
    }
    public void SetPurpleSlider(float value)
    {
        slider.value = value;
    }
}