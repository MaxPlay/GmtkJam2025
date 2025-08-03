using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GoalDisplayEntry : MonoBehaviour
{
    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private RectTransform progress;
    [SerializeField]
    private WinStar star0;
    [SerializeField]
    private WinStar star1;
    [SerializeField]
    private WinStar star2;

    private int score;

    public GameManager.WinCondition Condition { get; private set; }

    public void SetCondition(GameManager.WinCondition condition)
    {
        Condition = condition;
        iconImage.sprite = condition.Item.Sprite;
        score = condition.CurrentCount;
        scoreText.text = score.ToString();
        progress.anchorMax = new Vector2(GetPercentage(score, out int star), 1f);
        star0.SetVisible(star > 0);
        star1.SetVisible(star > 1);
        star2.SetVisible(star > 2);
    }

    private void Update()
    {
        if (score != Condition.CurrentCount)
        {
            score = Condition.CurrentCount;
            scoreText.text = score.ToString();
            float percentage = GetPercentage(score, out int star);
            progress.DOAnchorMax(new Vector2(percentage, 1f), 0.25f);
            star0.SetVisible(star > 0);
            star1.SetVisible(star > 1);
            star2.SetVisible(star > 2);
        }
    }

    private float GetPercentage(int score, out int star)
    {
        int threshold = 0;
        star = 0;
        if (Condition.TargetCountStar0 > score)
            return ((score - threshold) / (float)(Condition.TargetCountStar0 - threshold)) * 0.3f;
        star = 1;
        threshold = Condition.TargetCountStar0;
        if (Condition.TargetCountStar1 > score)
            return ((Condition.TargetCountStar1 - threshold) == 0 ? 0 : ((score - threshold) / (float)(Condition.TargetCountStar1 - threshold)) * 0.3f) + 0.3f;
        star = 2;
        threshold = Condition.TargetCountStar1;
        if (Condition.TargetCountStar2 > score)
            return ((Condition.TargetCountStar2 - threshold) == 0 ? 0 : ((score - threshold) / (float)(Condition.TargetCountStar2 - threshold)) * 0.3f) + 0.6f;
        star = 3;
        return 1;
    }
}
