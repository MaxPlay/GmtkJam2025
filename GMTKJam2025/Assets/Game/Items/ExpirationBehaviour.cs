using DG.Tweening;
using UnityEngine;

public class ExpirationBehaviour : MonoBehaviour
{
    [SerializeField] private int lifeTime;
    [SerializeField] private float expirationAnimationDuration = 0.3f;
    private int currentLife;

    public bool Tick()
    {
        currentLife++;
        float progress = (float)(currentLife) / (lifeTime + 1);
        var scaleTween = transform.DOScale((1 - progress) * Vector3.one, expirationAnimationDuration).SetEase(Ease.Linear);
        if (currentLife > lifeTime)
        {
            scaleTween.OnComplete(() => Destroy(gameObject));
            return false;
        }

        return true;
    }
}
