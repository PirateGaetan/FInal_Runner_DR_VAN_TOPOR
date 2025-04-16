using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class PlayerControler : MonoBehaviour
{

    [Header("REFERENCES")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] Transform aimTarget;
    [SerializeField] Transform playerModel;

    [Header("MOVEMENTS")]
    [SerializeField] private float moveDuration = 1f;

    [Header("AIM")]
    [SerializeField] private float aimSpeed = 0.5f;
    [SerializeField] private float aimeRange = 5f;

    [Header("NOISE")]
    [SerializeField] private float noiseAmplitude = 0.05f;
    [SerializeField] private float noiseSpeed = 0.5f;

    [Header("Slider Serum")]
    [SerializeField] private BlueSliderManager BlueSerum;
    [SerializeField] private GreenSliderManager GreenSerum;
    [SerializeField] private PurpleSliderManager PurpleSerum;
    [SerializeField] private YellowSliderManager YellowSerum;

    [Header("EVENTS")]
    public UnityEvent<float> OnInitSlider;
    public UnityEvent<float> OnBlueSerumCollision;
    public UnityEvent<float> OnGreenSerumCollision;
    public UnityEvent<float> OnPurpleSerumCollision;
    public UnityEvent<float> OnYellowSerumCollision;

    // MOUVEMENT
    private Vector3 targetPosition;
    private bool isMoving = false;

    // SERUM
    private float serumBlue = 100f;
    private float serumGreen = 100f;
    private float serumPurple = 100f;
    private float serumYellow = 100f;


    // LANES
    private float[] lanePositions;
    private int currentLaneIndex = 0;
    private float lastLeftTapTime = -1f;
    private float lastRightTapTime = -1f;
    private float doubleTapThreshold = 0.3f; 

    #region INIT & UPDATE
    public void InitPlayer()
    {
        targetPosition = transform.position;
        aimTarget.position = Vector3.forward * aimeRange;
        aimTarget.parent = null;

        SetLanesForLevel(1); 

        UpdateAllSerums();
    }

    public void UpdatePlayer()
    {
        PlayerInputManagement();
        ApplyPerlinNoise();
        GameOverCheck();
    }
    #endregion

    #region MOVEMENT
    private void PlayerInputManagement()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            float timeSinceLastTap = Time.time - lastLeftTapTime;

            if (!isMoving)
            {
                if (timeSinceLastTap <= doubleTapThreshold) MoveLeft(true);
                else MoveLeft(false);
            }

            lastLeftTapTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            float timeSinceLastTap = Time.time - lastRightTapTime;

            if (!isMoving)
            {
                if (timeSinceLastTap <= doubleTapThreshold) MoveRight(true);
                else MoveRight(false);
            }

            lastRightTapTime = Time.time;
        }
    }
    private void MoveLeft(bool isDoubleTap)
    {
        int lanesToMove = isDoubleTap ? 2 : 1;

        currentLaneIndex = Mathf.Max(0, currentLaneIndex - lanesToMove);
        MoveToLane();
    }

    private void MoveRight(bool isDoubleTap)
    {
        int lanesToMove = isDoubleTap ? 2 : 1;

        currentLaneIndex = Mathf.Min(lanePositions.Length - 1, currentLaneIndex + lanesToMove);
        MoveToLane();
    }

    private void MoveToLane()
    {
        isMoving = true;
        targetPosition = new Vector3(lanePositions[currentLaneIndex], transform.position.y, transform.position.z);
        Aim(targetPosition);
        transform.DOMove(targetPosition, moveDuration).SetEase(Ease.InOutSine).OnComplete(() => isMoving = false);
    }

    private void Aim(Vector3 direction)
    {
        Vector3 target = new Vector3(direction.x, 0, aimeRange);
        aimTarget.DOMove(target, aimSpeed).SetEase(Ease.InOutSine);
    }

    private void ApplyPerlinNoise()
    {
        float perlin = Mathf.PerlinNoise(Time.time * noiseSpeed, 0);
        float offset = (perlin - 0.5f) * 2f * noiseAmplitude;
        playerModel.localPosition = new Vector3(offset, 0, offset);
    }
    #endregion

    #region LANES & LEVEL
    public void SetLanesForLevel(int level)
    {
        switch (level)
        {
            case 1:
                lanePositions = new float[] { -3f, 0f, 3f };
                break;
            case 2:
                lanePositions = new float[] { -6f, -3f, 0f, 3f };
                break;
            case 3:
                lanePositions = new float[] { -6f, -3f, 0f, 3f, 6f };
                break;
        }

        float currentX = transform.position.x;
        currentLaneIndex = GetClosestLaneIndex(currentX);
        targetPosition = new Vector3(lanePositions[currentLaneIndex], transform.position.y, transform.position.z);
        transform.position = targetPosition;
    }

    private int GetClosestLaneIndex(float x)
    {
        int closest = 0;
        float minDist = Mathf.Abs(x - lanePositions[0]);
        for (int i = 1; i < lanePositions.Length; i++)
        {
            float dist = Mathf.Abs(x - lanePositions[i]);
            if (dist < minDist)
            {
                minDist = dist;
                closest = i;
            }
        }
        return closest;
    }
    #endregion

    #region SERUM
    private void UpdateAllSerums()
    {
        OnBlueSerumCollision.Invoke(serumBlue);
        OnGreenSerumCollision.Invoke(serumGreen);
        OnPurpleSerumCollision.Invoke(serumPurple);
        OnYellowSerumCollision.Invoke(serumYellow);
    }
    public void AddBlueSerum()
    {
        if (serumBlue < 100)
        {
            serumBlue += 10;
            serumBlue = Mathf.Clamp(serumBlue + 10, 0, 100);
            OnBlueSerumCollision.Invoke(serumBlue);
        }
    }
    public void AddGreenSerum()
    {
        if (serumGreen < 100)
        {
            serumGreen += 10;
            serumGreen = Mathf.Clamp(serumGreen + 10, 0, 100);
            OnGreenSerumCollision.Invoke(serumGreen);
        }
    }
    public void AddPurpleSerum()
    {
        if (serumPurple < 100)
        {
            serumPurple += 10;
            serumPurple = Mathf.Clamp(serumPurple + 10, 0, 100);
            OnPurpleSerumCollision.Invoke(serumPurple);
        }
    }
    public void AddYellowSerum()
    {
        if (serumYellow < 100)
        {
            serumYellow += 10;
            serumYellow = Mathf.Clamp(serumYellow + 10, 0, 100);
            OnYellowSerumCollision.Invoke(serumYellow);
        }
    }
    public void RemoveBlueSerum(float minusSerum)
    {
        serumBlue -= minusSerum;
        BlueSerum.SetBlueSlider(serumBlue);
    }

    public void RemoveGreenSerum(float minusSerum)
    {
        serumGreen -= minusSerum;
        GreenSerum.SetGreenSlider(serumGreen);
    }
    public void RemovePurpleSerum(float minusSerum)
    {
        serumPurple -= minusSerum;
        PurpleSerum.SetPurpleSlider(serumPurple);
    }
    public void RemoveYellowSerum(float minusSerum)
    {
        serumYellow -= minusSerum;
        YellowSerum.SetYellowSlider(serumYellow);
    }
    #endregion

    #region GAME OVER
    private void GameOverCheck()
    {
        if (serumBlue <= 0 || serumGreen <= 0 || serumPurple <= 0 || serumYellow <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        float currentScore = gameManager.GetScore();
        ScoreManager.SaveScore(currentScore);
        SceneManager.LoadScene("GameOver");
    }
    #endregion
}