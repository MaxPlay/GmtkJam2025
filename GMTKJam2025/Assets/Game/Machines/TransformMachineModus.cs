using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class TransformMachineModus : MachineModus
{
    [Serializable]
    private class TransformPair
    {
        public ConveyorItemData From;
        public ConveyorItemData To;
    }

    [SerializeField]
    private ConveyorItemData fallback;

    [SerializeField]
    private List<TransformPair> transformations;
    private Dictionary<ConveyorItemData, ConveyorItemData> fromToMapping;

    private void Awake()
    {
        Debug.Assert(fallback);
    }

    public override bool Tick(ConveyorItem currentItem, out ConveyorItem newItem)
    {
        if (fromToMapping == null)
        {
            fromToMapping = new();
            foreach (TransformPair transformPair in transformations)
            {
                fromToMapping[transformPair.From] = transformPair.To;
            }
        }

        newItem = null;
        if (currentItem)
        {
            if (fromToMapping.TryGetValue(currentItem.Data, out ConveyorItemData to))
            {
                if (currentItem.Data == to)
                {
                    newItem = currentItem;
                    return true;
                }

                newItem = Instantiate(to.Prefab);
            }
            else if(currentItem.Data != fallback)
            {
                newItem = Instantiate(fallback.Prefab);
            }
            else
            {
                newItem = currentItem;
            }
            return true;
        }
        return true;
    }
}
