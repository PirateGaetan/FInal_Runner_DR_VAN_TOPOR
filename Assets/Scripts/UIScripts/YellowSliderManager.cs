using UnityEngine;
using UnityEngine.UI;

public class YellowSliderManager : MonoBehaviour
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
        slider.gameObject.SetActive(level == GameManager.GameLevel.Level2 || level == GameManager.GameLevel.Level3);
    }
    public void InitYellowSlider(float value)
    {
        SetYellowSlider(value);
    }
    public void SetYellowSlider(float value)
    {
        slider.value = value;
    }
}