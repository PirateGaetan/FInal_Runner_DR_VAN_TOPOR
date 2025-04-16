using UnityEngine;
using System;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private PlayerControler player;

    [Header("UI")]
    [SerializeField] private GameObject adrenalineEffectUI;

    [Header("GAME DESIGN")]
    [SerializeField] private float SerumDecreaseSpeed = 1f;
    [SerializeField] public float dammageBlueCatalyser = 10f;
    [SerializeField] public float dammageGreenCatalyser = 10f;
    [SerializeField] public float dammageYellowCatalyser = 10f;
    [SerializeField] public float dammagePurpleCatalyser = 10f;
    [SerializeField] public float speedChunk = 6f;
    [SerializeField] public float increaseSpeedChunkFactor = 1.01f;
    [SerializeField] public float timeToLevel2;
    [SerializeField] public float timeToLevel3;
    [SerializeField] public float CataColisionDuration = 0.1f;
    [SerializeField] public float CataColisionSpeedReduction = 0.2f;

    //EVENT
    public event Action<GameLevel> OnLevelChanged;

    //CHUNK
    private float initialChunkSpeed;
    private float maxChunkSpeed;

    //TIMER & SCORE
    private float timerGamePlay = 0f;
    private float nextTimerGamePlay = 0f;
    private float lastSpeedIncreaseTime = -1f;
    private bool isAdrenalineActive = false;
    public float score = 0f;
    
    //LEVELS
    public enum GameLevel { Level1, Level2, Level3 }
    private GameLevel currentLevel = GameLevel.Level1;

    #region AWAKE, START & UPDATE
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        player.InitPlayer();
        initialChunkSpeed = speedChunk;
        maxChunkSpeed = initialChunkSpeed * 3f;
    }
    void Update()
    {
        player.UpdatePlayer();
        TimeIncrease();
    }
    #endregion

    #region GAME PROGRESSION & TIMER
    private void TimeIncrease()
    {
        if (Time.time >= nextTimerGamePlay)
        {
            TimerManagement();
            CheckLevelProgression();
            score += 1f;
            ApplyGamePlayTimerEffect();
            ConditionToIncreaseSpeedChunk();
        }
    }

    private void TimerManagement()
    {
        nextTimerGamePlay = Time.time + 1f;
        timerGamePlay++;
    }
    private void CheckLevelProgression()
    {
        if (timerGamePlay >= timeToLevel3) SetLevel(GameLevel.Level3);
        else if (timerGamePlay >= timeToLevel2) SetLevel(GameLevel.Level2);
    }

    private void ConditionToIncreaseSpeedChunk()
    {
        if (timerGamePlay % 3 == 0 && timerGamePlay != lastSpeedIncreaseTime)
        {
            IncreaseChunkSpeed();
            lastSpeedIncreaseTime = timerGamePlay;
        }
    }
    private void IncreaseChunkSpeed()
    {
        speedChunk *= increaseSpeedChunkFactor;
        if (speedChunk > maxChunkSpeed)
        {
            speedChunk = maxChunkSpeed;
        }
    }
    public float GetScore()
    {
        return score;
    }
    public float GetCurrentChunkSpeed()
    {
        return speedChunk;
    }
    #endregion


    #region LEVELS
    private void SetLevel(GameLevel newLevel)
    {
        currentLevel = newLevel;
        OnLevelChanged?.Invoke(currentLevel);

        switch (currentLevel)
        {
            case GameLevel.Level1:
                player.SetLanesForLevel(1);
                break;
            case GameLevel.Level2:
                player.SetLanesForLevel(2);
                break;
            case GameLevel.Level3:
                player.SetLanesForLevel(3);
                break;
        }
    }
    public GameLevel GetCurrentLevel()
    {
        return currentLevel;
    }
    #endregion


    

    #region ACTION ON SERUM
    private void ApplyGamePlayTimerEffect()
    {
        if (isAdrenalineActive) return;
        player.RemoveGreenSerum(SerumDecreaseSpeed);
        player.RemoveBlueSerum(SerumDecreaseSpeed);
        if (currentLevel == GameLevel.Level2 || currentLevel == GameLevel.Level3) player.RemoveYellowSerum(SerumDecreaseSpeed);
        if (currentLevel == GameLevel.Level3) player.RemovePurpleSerum(SerumDecreaseSpeed);
    }

    public void TriggerAdrenaline()
    {
        StartCoroutine(AdrenalineRoutine());
    }

    private IEnumerator AdrenalineRoutine()
    {
        isAdrenalineActive = true;
        adrenalineEffectUI.SetActive(true);
        yield return new WaitForSeconds(5f);
        isAdrenalineActive = false;
        adrenalineEffectUI.SetActive(false);
    }
    #endregion

    #region ACTION ON CATALYSER
    public void TriggerSpeedReduction(float CataColisionDuration, float CataColisionSpeedReduction)
    {
        StartCoroutine(TemporarySpeedReduction(CataColisionDuration, CataColisionSpeedReduction));
    }

    private IEnumerator TemporarySpeedReduction(float CataColisionDuration, float CataColisionSpeedReduction)
    {
        if (speedChunk < initialChunkSpeed )
        {
            float originalSpeed = speedChunk;

            yield return new WaitForSeconds(CataColisionDuration);

            speedChunk = originalSpeed;
        }
        else
        {
            float originalSpeed = speedChunk;
            speedChunk *= CataColisionSpeedReduction;

            yield return new WaitForSeconds(CataColisionDuration);

            speedChunk = originalSpeed;
        }
    }
    #endregion
}