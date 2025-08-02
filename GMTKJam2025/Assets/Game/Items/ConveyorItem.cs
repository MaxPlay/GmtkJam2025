using UnityEngine;

public class ConveyorItem : MonoBehaviour
{
    public Conveyor Conveyor { get; private set; }

    public void SetConveyor(Conveyor conveyor)
    {
        Conveyor = conveyor;
    }
}
