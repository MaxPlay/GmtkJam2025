using NaughtyAttributes;
using UnityEngine;

public class ConveyorItem : MonoBehaviour
{
    [SerializeField]
    private ConveyorItemData data;
    public ConveyorItemData Data => data;

    [ReadOnly, SerializeField]
    private Conveyor conveyor;
    public Conveyor Conveyor { get => conveyor; private set => conveyor = value; }

    public void SetConveyor(Conveyor conveyor)
    {
        Conveyor = conveyor;
    }
}
