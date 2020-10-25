using System;
using System.Collections;
using UnityEngine;

public class Game: MonoBehaviour {
    private static Game _instance;

    // -- types --
    [Flags]
    public enum Step: ushort {
        Phone = 1 << 0,
        Foot1 = 1 << 1,
        Door1 = 1 << 2,
        Sheep = 1 << 3,
        Exit1 = 1 << 4,
        Foot2 = 1 << 5,
        Door2 = 1 << 6,
        Food = 1 << 7,
        Exit2 = 1 << 8,
        Foot3 = 1 << 9,
        Door3 = 1 << 10,
    }

    // -- fields --
    [SerializeField]
    [Tooltip("Whether the game is in debug mode.")]
    private bool fIsFree = false;

    [SerializeField]
    [Tooltip("Whether the game is in debug mode.")]
    private bool fIsDebug = false;

    [SerializeField]
    [Tooltip("The log level.")]
    private Log.Level fLogLevel = Log.Level.Info;

    [SerializeField]
    [Tooltip("The player.")]
    private Player fPlayer;

    [SerializeField]
    [Tooltip("The bedroom.")]
    private Bedroom fBedroom;

    // -- model --
    private Step mStep;
    private Step? mNewStep;

    // -- lifecycle --
    protected void Awake() {
        _instance = this;

        // configure services
        Log.SetLevel(fLogLevel);
    }

    protected void Start() {
        // abort game logic if in free mode
        if (fIsFree) {
            return;
        }

        // set initial state
        Reset();

        // run debug setup if enabled
        if (fIsDebug) {
            StartCoroutine(DebugSetup());
        }
    }

    private IEnumerator DebugSetup() {
        yield return 0;
        PickUp(GetComponentInChildren<Phone>());
        StandUp(GetComponentInChildren<Body>());
        // Open(GetComponentInChildren<Door>());
        // EnterSheepRoom();
        // Catch(GetComponentInChildren<Sheep>());
        // ExitSheepRoom();
        //
        // yield return 0;
        // StandUp(GetComponentInChildren<Body>());
        // Open(GetComponentInChildren<Door>());
    }

    // -- commands --
    public void Reset() {
        AdvanceToStep(Step.Phone);
        EnterBedroom((b) => b.WarpToSheep());
        fPlayer.SetPhoneTime("1:15 AM");
    }

    public void EnterBedroom(Action<Bedroom> warp) {
        fBedroom.Show();
        warp(fBedroom);
        fPlayer.Sleep();
    }

    public void PickUp(Phone phone) {
        fPlayer.PickUp(phone);
        AdvanceStep();
    }

    public void StandUp(Body _) {
        fPlayer.StandUp();
        AdvanceStep();
    }

    public void Open(Door door) {
        door.Open();
        AdvanceStep();
    }

    public void EnterSheepRoom() {
        fBedroom.Hide();
    }

    public void ExitSheepRoom() {
        EnterBedroom((b) => b.WarpToFood());
        AdvanceStep();
        fPlayer.SetPhoneTime("2:33 AM");
    }

    public void Catch(Sheep sheep) {
        fPlayer.PickUp(sheep);
        AdvanceStep();
    }

    public void EnterKitchen() {
        fBedroom.Hide();
    }

    public void Eat(Food food) {
        fPlayer.PickUp(food);
        AdvanceStep();
    }

    public void ExitKitchen() {
        EnterBedroom((b) => b.WarpToHall());
        AdvanceStep();
        fPlayer.SetPhoneTime("3:47 AM");
    }

    // -- commands/step
    private void AdvanceStep() {
        AdvanceToStep((Step)((ushort)mStep << 1));
    }

    private void AdvanceToStep(Step step) {
        Log.Debug("Game - AdvanceToStep: {0}", step);

        mStep = step;
        mNewStep = step;
        StartCoroutine(ClearNewStep(step));
    }

    private IEnumerator ClearNewStep(Step step) {
        yield return 0;
        if (mNewStep == step) {
            mNewStep = null;
        }
    }

    // -- queries --
    public bool IsFree() {
        return fIsFree;
    }

    public bool DidChangeToStep(Step step) {
        if (mNewStep == null) {
            return false;
        }

        return (mNewStep & step) != 0;
    }

    // -- events --
    public void OnInteract(Interact.Target target) {
        switch (target) {
            case Phone phone:
                PickUp(phone); break;
            case Body body:
                StandUp(body); break;
            case Door door:
                Open(door); break;
            case Sheep sheep:
                Catch(sheep); break;
            case Food food:
                Eat(food); break;
            case ExitKitchen _:
                ExitKitchen(); break;
            default:
                Debug.LogErrorFormat("Interacting with unknown target: {0}", target); break;
        }
    }

    // -- module --
    public static Game Get() {
        return _instance;
    }
}
