using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

[AddComponentMenu("Machine Modules/Transform Item Machine Module")]
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

    [SerializeField] private TransformationDisplayList transformationDisplay;

    private void Awake()
    {
        Debug.Assert(fallback);
        if(transformationDisplay)
            ModusEntered.AddListener(CreateTransformationVisuals);
    }

    private void CreateTransformationVisuals()
    {
        foreach (TransformPair transformation in transformations)
        {
            transformationDisplay.AddDisplay(transformation.From, transformation.To);
        }
    }

    public override bool Tick(ConveyorItem currentItem, out ConveyorItem newItem)
    {
        if (currentItem && currentItem.Data == fallback)
        {
            newItem = null;
            return false;
        }

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
