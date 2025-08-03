using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : GameHudModule
{
    private List<PauseScreenElement> elements = new();

    protected override void Initialize()
    {
        if (elements.Count == 0)
            GetComponentsInChildren(true, elements);

        foreach (PauseScreenElement element in elements)
        {
            element.gameObject.SetActive(false);
            element.transform.localScale = Vector3.zero;
        }
    }

    public void OnEnable()
    {
        foreach (PauseScreenElement element in elements)
        {
            element.gameObject.SetActive(true);
            element.transform.DOScale(Vector3.one, 0.3f).SetUpdate(true);
        }
    }

    private void OnDisable()
    {
        foreach (PauseScreenElement element in elements)
        {
            element.transform.DOScale(Vector3.zero, 0.3f).SetUpdate(true).OnComplete(() => element.gameObject.SetActive(false));
        }
    }

    public void Continue()
    {
        Owner.GameManager.Unpause();
    }

    public void Restart()
    {
        SceneManager.LoadScene(Owner.GameManager.gameObject.scene.name);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(Scenes.MENU_SCENE);
    }
}
