using UnityEngine;

public class ConveyorItem : MonoBehaviour
{
    [SerializeField]
    private ConveyorItemData data;
    public ConveyorItemData Data => data;

    public Conveyor Conveyor { get; private set; }

    public void SetConveyor(Conveyor conveyor)
    {
        Conveyor = conveyor;
    }
}
