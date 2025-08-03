using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
[DisallowMultipleComponent]
public class SetBlendShapes : MonoBehaviour
{
    [SerializeField] private float maxValue;

    private SkinnedMeshRenderer meshRenderer;

    [SerializeField, MinValue(0.01f)] private float blendTime;

    private int currentIndex;

    void Awake()
    {
        meshRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    private IEnumerator DOBlend(int index, bool inverted)
    {
        float startTime = Time.time;

        while (Time.time < startTime + blendTime)
        {
            float progress = (Time.time - startTime) / blendTime;
            meshRenderer.SetBlendShapeWeight(index, inverted ? progress * maxValue : ((1 - progress) * maxValue));
            yield return null;
        }
        meshRenderer.SetBlendShapeWeight(index, inverted ?  maxValue : 0);
    }

    public void ActivateBlendShape(int index)
    {
        StartCoroutine(DOBlend(index, false));
    }

    public void DeactivateBlendShape(int index)
    {
        StartCoroutine(DOBlend(index, true));
    }
}
