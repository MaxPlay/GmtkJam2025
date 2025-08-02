using DG.Tweening;
using TMPro;
using UnityEngine;

public class ConveyorStraight : Conveyor
{
    protected override void MoveIn(Transform item, Sequence moveSequence)
    {
        moveSequence.Append(item.DOMove(CenterPosition, GameManager.ConveyorMoveDuration / 2).SetEase(Ease.OutQuad));
    }

    protected override void MoveToOut(Transform item, Sequence moveSequence)
    {
        moveSequence.Append(item.DOMove(OutPosition, GameManager.ConveyorMoveDuration / 2).SetEase(Ease.InQuad));
    }
}
