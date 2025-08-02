using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [ReadOnly]
    [SerializeField]
    private List<Conveyor> conveyors = new();
    
    [ReadOnly]
    [SerializeField]
    private List<Machine> machines = new();

    [SerializeField]
    [Min(0.1f)]
    private float tickDuration;
    private float lastTick;

    private void Start()
    {
        conveyors.AddRange(FindObjectsByType<Conveyor>(FindObjectsInactive.Include, FindObjectsSortMode.None));
        machines.AddRange(FindObjectsByType<Machine>(FindObjectsInactive.Include, FindObjectsSortMode.None));
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
            foreach (Machine machine in machines)
            {
                machine.Tick();
            }
        }
    }
}
