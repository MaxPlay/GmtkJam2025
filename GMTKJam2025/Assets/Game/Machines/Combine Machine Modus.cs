using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[AddComponentMenu("Machine Modules/Combine Machine Module")]
public class CombineMachineModus : StorageMachineModus
{
    [SerializeField] private ConveyorItemData fallback;

    [Serializable]
    private class CombinePair
    {
        public ConveyorItemData ItemA;
        public ConveyorItemData ItemB;
        public ConveyorItemData Result;

        public bool TryCombine(ConveyorItem itemA, ConveyorItem itemB, out ConveyorItemData result)
        {
            if ((itemA.Data == ItemA && itemB.Data == ItemB) || (itemA.Data == ItemB && itemB.Data == ItemA))
            {
                result = Result;
                return true;
            }

            result = null;
            return false;
        }
    }

    [SerializeField] private List<CombinePair> combinationPairs;

    void Awake()
    {
        Debug.Assert(fallback);
    }

    public override bool Tick(ConveyorItem currentItem, out ConveyorItem newItem)
    {
        newItem = null;
        if (storedItem && currentItem)
        {
            foreach (CombinePair pair in combinationPairs)
            {
                if (pair.TryCombine(storedItem, currentItem, out ConveyorItemData result))
                {
                    newItem = Instantiate(result.Prefab);
                    return true;
                }
            }
            newItem = currentItem.Data == fallback ? currentItem : Instantiate(fallback.Prefab);
            Transform storedItemTransform = storedItem.transform;
            storedItem = null;
            storedItemTransform.SetParent(null);
            storedItemTransform.DOMove(transform.position, 0.1f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                Destroy(storedItemTransform.gameObject);
            });
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
            newItem = null;
        }

        return true;
    }
}
