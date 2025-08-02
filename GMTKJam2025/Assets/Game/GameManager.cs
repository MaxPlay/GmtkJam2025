using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [ReadOnly]
    [SerializeField]
    private List<Conveyor> conveyors = new();
    private readonly Dictionary<Vector2Int, Conveyor> conveyorAtCoordinate = new();

    [ReadOnly]
    [SerializeField]
    private List<Machine> machines = new();

    [SerializeField]
    private float conveyorMoveDuration = 0.3f;
    public float ConveyorMoveDuration => conveyorMoveDuration;

    [SerializeField]
    [Min(0.1f)]
    private float tickDuration = 1;
    private float lastTick;


    public static Vector2Int CoordinateFromWorld(Vector3 position)
        => new Vector2Int(Mathf.RoundToInt(position.x) - (position.x < 0.0f ? 1 : 0), Mathf.RoundToInt(position.z) - (position.z < 0.0f ? 1 : 0)) / 2;

    private void Start()
    {
        machines.AddRange(FindObjectsByType<Machine>(FindObjectsInactive.Include, FindObjectsSortMode.None));

        foreach (Conveyor conveyor in FindObjectsByType<Conveyor>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            conveyor.Initialize(this);
            conveyors.Add(conveyor);
            Vector2Int coordinate = CoordinateFromWorld(conveyor.transform.position);
            Debug.Assert(!conveyorAtCoordinate.ContainsKey(coordinate), $"Multiple Conveyors at coordinate {coordinate}.");
            conveyorAtCoordinate.Add(coordinate, conveyor);
        }

        foreach (Conveyor conveyor in conveyors)
        {
            Vector2Int next = conveyor.GetDeliveryCoordinate();
            if (conveyorAtCoordinate.TryGetValue(next, out Conveyor other))
            {
                conveyor.SetNext(other);
            }
        }

        foreach (Machine machine in machines)
        {
            machine.Initialize(this);
        }
    }

    private void Update()
    {
        lastTick += Time.deltaTime;

        while (lastTick > tickDuration)
        {
            lastTick -= tickDuration;
            foreach (Conveyor conveyor in conveyors)
            {
                conveyor.Tick();
            }
            foreach (Conveyor conveyor in conveyors)
            {
                conveyor.Receive();
            }
            foreach (Machine machine in machines)
            {
                machine.Tick();
            }
        }
    }

    public Conveyor GetClosestConveyor(Vector3 transformPosition)
    {
        Conveyor closestConveyor = null;
        float closestDistance = -1;

        foreach (Conveyor conveyor in conveyors)
        {
            if (closestDistance < 0)
            {
                closestConveyor = SetClosestConveyor(transformPosition, conveyor, out closestDistance);
            }

            float distance = Vector3.SqrMagnitude(transformPosition - conveyor.transform.position);
            if (distance < closestDistance)
            {
                closestConveyor = SetClosestConveyor(transformPosition, conveyor, out closestDistance);
            }
        }

        return closestConveyor;
    }

    private static Conveyor SetClosestConveyor(Vector3 transformPosition, Conveyor conveyor, out float closestDistance)
    {
        closestDistance = Vector3.SqrMagnitude(transformPosition - conveyor.transform.position);
        return conveyor;
    }
}
