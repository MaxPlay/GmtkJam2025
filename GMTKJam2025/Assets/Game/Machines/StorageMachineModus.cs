using DG.Tweening;
using UnityEngine;

[AddComponentMenu("Machine Modules/Storage Machine Module")]
public class StorageMachineModus : MachineModus
{
    private ConveyorItem storedItem;
    private ConveyorItem spawningItem;
    private bool spawningStoredItem = false;
    [SerializeField] private Transform targetPosition;

    public override bool Tick(ConveyorItem currentItem, out ConveyorItem newItem)
    {
        newItem = null;
        if (storedItem)
        {
            newItem = currentItem;
        }
        else
        {
            if (currentItem)
            {
                ConveyorItem newStoredItem = Instantiate(currentItem);
                newStoredItem.transform.SetParent(transform);
                newStoredItem.transform.DOLocalMove(targetPosition.localPosition, 0.3f).SetEase(Ease.OutQuad).OnComplete((() =>
                {
                    storedItem = newStoredItem;
                }));
            }
            newItem = spawningItem;
            spawningItem = null;
        }

        return true;
    }

    public void Interact(int interactionIndex)
    {
        if (interactionIndex == 2 || spawningItem || spawningStoredItem || !storedItem)
            return;

        spawningStoredItem = true;
        storedItem.transform.SetParent(null);
        storedItem.transform.DOMove(transform.position, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            spawningItem = storedItem;
            storedItem = null;
            spawningStoredItem = false;
        });
    }
}
