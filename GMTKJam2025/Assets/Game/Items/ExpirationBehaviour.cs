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
        if (currentLife > lifeTime)
        {
            transform.DOScale(Vector3.zero, expirationAnimationDuration).SetEase(Ease.Linear).OnComplete(() => Destroy(gameObject));
            return false;
        }

        return true;
    }
}
