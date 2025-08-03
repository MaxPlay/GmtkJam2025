using UnityEngine;

[CreateAssetMenu(menuName = "Loop/Conveyor Item Data", fileName = "ConveyorItem")]
public class ConveyorItemData : ScriptableObject
{
    [SerializeField]
    private new string name;
    public string Name => name;

    [SerializeField]
    private ConveyorItem prefab;
    public ConveyorItem Prefab => prefab;

    [SerializeField]
    private Sprite sprite;
    public Sprite Sprite => sprite;
}
