using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class GameHud : MonoBehaviour
{
    public GameManager GameManager { get; private set; }
    private GameManager.GameState state = GameManager.GameState.Starting;

    [SerializeField]
    private PauseScreen pauseScreen;
    [SerializeField]
    private GoalDisplay goalDisplay;
    [SerializeField]
    TextMeshProUGUI timer;
    [SerializeField]
    TextMeshProUGUI start;

    private void Start()
    {
        GameManager = FindFirstObjectByType<GameManager>();
        pauseScreen.SetOwner(this);
        goalDisplay.SetOwner(this);
    }

    private void Update()
    {
        int totalSeconds = Mathf.FloorToInt(GameManager.Timer);
        timer.text = $"{totalSeconds / 60:00}:{totalSeconds % 60:00}";

        if (state != GameManager.State)
        {
            // Leave state
            switch (state)
            {
                case GameManager.GameState.Starting:
                    Sequence s = DOTween.Sequence(start.transform);
                    s.Append(start.transform.DOScale(Vector2.one, 0.4f));
                    s.Append(start.transform.DOScale(Vector2.one * 1.2f, 0.2f));
                    s.Append(start.transform.DOScale(Vector2.zero, 0.4f));
                    s.Play();
                    break;
                case GameManager.GameState.Paused:
                    pauseScreen.enabled = false;
                    break;
                case GameManager.GameState.Running:
                    break;
                case GameManager.GameState.Over:
                    break;
            }

            state = GameManager.State;

            // Enter state
            switch (state)
            {
                case GameManager.GameState.Starting:
                    break;
                case GameManager.GameState.Paused:
                    pauseScreen.enabled = true;
                    break;
                case GameManager.GameState.Running:
                    break;
                case GameManager.GameState.Over:
                    break;
            }
        }
    }
}

public abstract class GameHudModule : MonoBehaviour
{
    public GameHud Owner { get; private set; }

    public void SetOwner(GameHud owner)
    {
        Owner = owner;
        Initialize();
    }

    protected virtual void Initialize() { }
}
