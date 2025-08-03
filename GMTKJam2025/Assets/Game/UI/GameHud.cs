using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameHud : MonoBehaviour
{
    public GameManager GameManager { get; private set; }
    private GameManager.GameState state = GameManager.GameState.Starting;

    [SerializeField]
    private PauseScreen pauseScreen;
    [SerializeField]
    private GoalDisplay goalDisplay;
    [SerializeField]
    private TextMeshProUGUI timer;
    [SerializeField]
    private TextMeshProUGUI start;
    [SerializeField]
    private TextMeshProUGUI finished;
    [SerializeField]
    private GameObject finishObject;
    [SerializeField]
    private WinStar finishStar0;
    [SerializeField]
    private WinStar finishStar1;
    [SerializeField]
    private WinStar finishStar2;

    private void Start()
    {
        GameManager = FindFirstObjectByType<GameManager>();
        pauseScreen.SetOwner(this);
        goalDisplay.SetOwner(this);
    }

    private void Update()
    {
        int totalSeconds = Mathf.Max(Mathf.FloorToInt(GameManager.Timer), 0);
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
                    finishObject.SetActive(true);
                    List<Button> buttons = new();
                    finishObject.GetComponentsInChildren(buttons);
                    foreach (Button button in buttons)
                    {
                        button.transform.localScale = Vector3.zero;
                        button.transform.DOScale(Vector3.one, 0.3f);
                    }

                    Sequence s = DOTween.Sequence(finished.transform);
                    s.Append(finished.transform.DOScale(Vector2.one * 1.2f, 1f));
                    s.Append(finished.transform.DOScale(Vector2.one, 0.4f));
                    s.OnComplete(() =>
                    {
                        foreach (Button button in buttons)
                        {
                            button.transform.DOScale(Vector3.one, 0.3f);
                        }

                        finishStar0.SetVisible(GameManager.Stars > 0);
                        finishStar1.SetVisible(GameManager.Stars > 1);
                        finishStar2.SetVisible(GameManager.Stars > 2);
                    }).Play();
                    break;
            }
        }
    }
    public void Restart()
    {
        if (GameManager.State == GameManager.GameState.Over)
            SceneManager.LoadScene(GameManager.gameObject.scene.name);
    }

    public void BackToMenu()
    {
        if (GameManager.State == GameManager.GameState.Over)
            SceneManager.LoadScene(Scenes.MENU_SCENE);
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
