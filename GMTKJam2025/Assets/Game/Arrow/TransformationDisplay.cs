using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransformationDisplay : MonoBehaviour
{
    [SerializeField] private Transform fromParent;
    [SerializeField] private Transform toParent;

    [SerializeField] private float itemDistance;
    [SerializeField] private float itemScaling;

    public void SetTransformation(IEnumerable<ConveyorItemData> from, IEnumerable<ConveyorItemData> to)
    {
        int fromCount = from.Count();
        int toCount = to.Count();

        int index = 0;

        foreach (ConveyorItemData fromItem in from)
        {
            ConveyorItem newItem = Instantiate(fromItem.Prefab);
            newItem.transform.position = fromParent.position;
            newItem.transform.localScale = Vector3.one * itemScaling;
            newItem.transform.SetParent(fromParent);
            newItem.transform.localPosition = Vector3.forward * index + Vector3.back * (fromCount - 1) / 2f;
            index++;
        }
        index = 0;
        foreach (ConveyorItemData toItem in to)
        {
            ConveyorItem newItem = Instantiate(toItem.Prefab);
            newItem.transform.position = toParent.position;
            newItem.transform.localScale = Vector3.one * itemScaling;
            newItem.transform.SetParent(toParent);
            newItem.transform.localPosition = Vector3.forward * index + Vector3.back * (toCount - 1) / 2f;
            index++;
        }
    }
}
