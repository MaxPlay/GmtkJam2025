using DG.Tweening;
using UnityEngine;

public class GoalDisplay : GameHudModule
{
    [SerializeField]
    private GoalDisplayEntry entryPrefab;

    [SerializeField]
    private Transform container;

    protected override void Initialize()
    {
        foreach (GameManager.WinCondition condition in Owner.GameManager.WinConditions)
        {
            GoalDisplayEntry entry = Instantiate(entryPrefab, container);
            entry.SetCondition(condition);
            entry.transform.localScale = Vector3.zero;
            entry.transform.DOScale(Vector3.one, 0.5f);
        }
    }
}
