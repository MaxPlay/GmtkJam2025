using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private readonly List<MainMenuElement> elements = new();

    private IEnumerator Start()
    {
        Time.timeScale = 1;
        if (elements.Count == 0)
        {
            GetComponentsInChildren(true, elements);
            elements.Sort((a, b) => a.Priority.CompareTo(b.Priority));
        }

        DOTween.Init();
        foreach (MainMenuElement element in elements)
        {
            element.transform.localScale = Vector3.zero;
        }
        yield return new WaitForEndOfFrame();
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.2f);
        foreach (MainMenuElement element in elements)
        {
            element.transform.DOScale(Vector3.one, 0.3f).SetUpdate(true);
            yield return waitForSeconds;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(Scenes.GAME_SCENE);
    }

    public void ShowCredits()
    {
        SceneManager.LoadScene(Scenes.CREDITS_SCENE);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(Scenes.MENU_SCENE);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }
}
