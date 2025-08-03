using NaughtyAttributes;
using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private InputAction pauseAction;

    [ReadOnly]
    [SerializeField]
    private List<Conveyor> conveyors = new();
    private readonly Dictionary<Vector2Int, Conveyor> conveyorAtCoordinate = new();
    [SerializeField] private Material conveyorMaterial;

    [ReadOnly]
    [SerializeField]
    private List<Machine> machines = new();
    private List<Machine> brokenMachines = new();

    [SerializeField]
    private float conveyorMoveDuration = 0.3f;
    public float ConveyorMoveDuration => conveyorMoveDuration;

    [SerializeField]
    [Min(0.1f)]
    private float tickDuration = 1;
    private float lastTick;

    [SerializeField]
    [Header("Win Conditions")]
    private List<WinCondition> conditions = new();
    public IReadOnlyList<WinCondition> WinConditions => conditions;
    [SerializeField]
    private float totalDuration;
    [SerializeField]
    private bool endless = false;

    [SerializeField]
    [ReadOnly]
    private GameState state = GameState.Starting;

    public float Timer { get; private set; }
    public int Stars { get; private set; }

    public GameState State
    {
        get => state;
        private set
        {
            GameState newState = value;
            if (endless)
            {
                newState = newState switch
                {
                    GameState.Starting => GameState.Running,
                    GameState.Over => GameState.Running,
                    _ => newState
                };
            }
            if (newState != state)
            {
                switch (state)
                {
                    case GameState.Paused:
                        Time.timeScale = 1;
                        break;
                }

                state = newState;
                Debug.Log(state);

                switch (state)
                {
                    case GameState.Paused:
                        Time.timeScale = 0;
                        break;
                    case GameState.Over:
                        InputSystem.actions.FindActionMap("Player").Disable();
                        break;
                }
            }
        }
    }

    public bool CollectItemForWinCondition(ConveyorItemData item)
    {
        foreach (WinCondition condition in conditions)
        {
            if (condition.Item == item)
            {
                condition.CurrentCount++;
                return true;
            }
        }
        return false;
    }

    public static Vector2Int CoordinateFromWorld(Vector3 position)
        => new Vector2Int(Mathf.RoundToInt(position.x) - (position.x < 0.0f ? 1 : 0), Mathf.RoundToInt(position.z) - (position.z < 0.0f ? 1 : 0)) / 2;

    private void Start()
    {
        InputSystem.actions.FindActionMap("UI").Enable();
        InputSystem.actions.FindActionMap("Menu").Enable();

        pauseAction = InputSystem.actions.FindAction("Pause");
        pauseAction.started += PauseAction_started;

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

        Timer = 3;
        Stars = 0;
        State = GameState.Starting;
        Time.timeScale = 1;
    }

    private void Update()
    {
        switch (State)
        {
            case GameState.Starting:
                Timer -= Time.deltaTime;
                if (Timer <= 0.0f)
                {
                    State = GameState.Running;
                    Timer = totalDuration;
                }
                break;

            case GameState.Running:
                lastTick += Time.deltaTime;
                Timer -= Time.deltaTime;

                while (lastTick > tickDuration)
                {
                    lastTick -= tickDuration;
                    if (brokenMachines.Count > 0)
                        break;
                    conveyorMaterial.SetFloat("_TextureOffset", 0f);
                    conveyorMaterial.DOFloat(1f, "_TextureOffset", conveyorMoveDuration).SetEase(Ease.InOutQuad);
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

                RefreshStars();

                if (Timer <= 0)
                {
                    State = GameState.Over;
                }
                break;
        }
    }

    private void OnDestroy()
    {
        pauseAction.started -= PauseAction_started;
    }

    private void PauseAction_started(InputAction.CallbackContext obj)
    {
        State = state switch
        {
            GameState.Paused => GameState.Running,
            GameState.Running => GameState.Paused,
            _ => state
        };
    }

    private void RefreshStars()
    {
        for (int i = 0; i < 3; i++)
        {
            foreach (WinCondition condition in conditions)
            {
                if (condition.GetTargetCount(i) > condition.CurrentCount)
                    return;
            }
            Stars = i;
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

    public void Unpause()
    {
        if (State == GameState.Paused)
            State = GameState.Running;
    }

    public enum GameState
    {
        Starting,
        Paused,
        Running,
        Over
    }

    [Serializable]
    public class WinCondition
    {
        public int GetTargetCount(int index) => index switch
        {
            0 => TargetCountStar0,
            1 => TargetCountStar1,
            2 => TargetCountStar2,
            _ => int.MaxValue
        };

        public ConveyorItemData Item;
        public int TargetCountStar0;
        public int TargetCountStar1;
        public int TargetCountStar2;
        [ReadOnly]
        public int CurrentCount;
    }

    public void MachineBroken(Machine machine)
    {
        brokenMachines.Add(machine);
    }

    public bool TryFixMachine(Machine machine)
    {
        return brokenMachines.Remove(machine);
    }
}
