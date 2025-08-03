using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class ConveyorItem : MonoBehaviour
{
    [SerializeField]
    private ConveyorItemData data;
    public ConveyorItemData Data => data;

    [ReadOnly, SerializeField]
    private Conveyor conveyor;
    public Conveyor Conveyor { get => conveyor; private set => conveyor = value; }

    private ExpirationBehaviour expirationBehaviour;

    private void Awake()
    {
        expirationBehaviour = GetComponent<ExpirationBehaviour>();
    }

    public void SetConveyor(Conveyor conveyor)
    {
        Conveyor = conveyor;
    }

    public bool Tick()
    {
        if (expirationBehaviour)
        {
            return expirationBehaviour.Tick();
        }

        return true;
    }
}
