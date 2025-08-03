using System.Collections.Generic;
using UnityEngine;

public class TransformationDisplayList : MonoBehaviour
{
    private List<TransformationDisplay> shownDisplays = new List<TransformationDisplay>();

    [SerializeField] private TransformationDisplay prefab;

    [SerializeField] private float verticalOffset;

    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }

    public void AddDisplay(IEnumerable<ConveyorItemData> from, IEnumerable<ConveyorItemData> to)
    {
        TransformationDisplay newDisplay = Instantiate(prefab);
        newDisplay.SetTransformation(from, to);
        newDisplay.transform.position = transform.position + Vector3.up * shownDisplays.Count * verticalOffset;
        newDisplay.transform.SetParent(transform);

        shownDisplays.Add(newDisplay);
    }

    public void AddDisplay(ConveyorItemData from, ConveyorItemData to)
    {
        TransformationDisplay newDisplay = Instantiate(prefab);
        newDisplay.SetTransformation(new []{ from }, new []{ to });
        newDisplay.transform.position = transform.position + Vector3.up * shownDisplays.Count * verticalOffset;
        newDisplay.transform.SetParent(transform);

        shownDisplays.Add(newDisplay);
    }

    public void Clear()
    {
        foreach (TransformationDisplay display in shownDisplays)
        {
            Destroy(display.gameObject);
        }
        shownDisplays.Clear();
    }
}
