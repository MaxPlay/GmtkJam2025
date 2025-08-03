using DG.Tweening;
using UnityEngine;

public class ExpirationBehaviour : MonoBehaviour
{
    [SerializeField] private int lifeTime;
    private int currentLife;

    public bool Tick(GameManager gameManager)
    {
        currentLife++;
        float progress = (float)(currentLife) / (lifeTime + 1);
        var scaleTween = transform.DOScale((1 - progress) * Vector3.one, gameManager.ConveyorMoveDuration).SetEase(Ease.InOutQuad);
        if (currentLife > lifeTime)
        {
            scaleTween.OnComplete(() => Destroy(gameObject));
            return false;
        }

        return true;
    }
}
