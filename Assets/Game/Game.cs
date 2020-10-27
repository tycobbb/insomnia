using System;
using System.Collections;
using UnityEngine;

public class Game: MonoBehaviour {
    private static Game sInstance;

    // -- types --
    [Flags]
    public enum Step: ushort {
        Fan = 1 << 0,
        Phone = 1 << 1,
        Foot1 = 1 << 2,
        Door1 = 1 << 3,
        Sheep = 1 << 4,
        Exit1 = 1 << 5,
        Foot2 = 1 << 6,
        Door2 = 1 << 7,
        Food = 1 << 8,
        Exit2 = 1 << 9,
        Foot3 = 1 << 10,
        Door3 = 1 << 11,
        Exit3 = 1 << 12,
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
    private Player fPlayer = null;

    [SerializeField]
    [Tooltip("The bedroom.")]
    private Bedroom fBedroom = null;

    // -- props --
    private Step mStep;
    private Step? mNewStep;

    // -- lifecycle --
    protected void Awake() {
        sInstance = this;

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
        IdentifyFan(GetComponentInChildren<Fan>());
        PickUp(GetComponentInChildren<Phone>());
        StandUp(GetComponentInChildren<Body>());
        // ExitBedroom(GetComponentInChildren<BedroomExit>());

        // var r1 = GetComponentInChildren<Field>();
        // DidStartEnterRoom(r1);
        // DidFinishEnterRoom(r1);
        // CatchSheep(GetComponentInChildren<Sheep>(true));
        // ExitField();
        //
        // yield return 0;
        // StandUp(GetComponentInChildren<Body>());
        // IdentifyMoon(GetComponentInChildren<Moon>(true));
        // ExitBedroom(GetComponentInChildren<BedroomExit>());
        //
        // var r2 = GetComponentInChildren<Kitchen>();
        // DidStartEnterRoom(r2);
        // DidFinishEnterRoom(r2);
        // EatFood(GetComponentInChildren<Food>(true));
        // ExitKitchen();

        fIsDebug = false;
    }

    // -- commands --
    public void Reset() {
        AdvanceToStep(Step.Fan);
        EnterBedroom((b) => b.WarpToSheep());
        fPlayer.SetPhoneTime("1:15 AM");
    }

    private void EnterBedroom(Action<Bedroom> warp) {
        fBedroom.Show();
        warp(fBedroom);
        fPlayer.Sleep();
    }

    private void IdentifyFan(Fan _) {
        AdvanceStep();
    }

    private void PickUp(Phone phone) {
        fPlayer.PickUp(phone);
        AdvanceStep();
    }

    private void StandUp(Body _) {
        fPlayer.StandUp();
        AdvanceStep();
    }

    private void ExitBedroom(BedroomExit exit) {
        if (!fIsDebug) {
            exit.Open();
        }

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

    private void CatchSheep(Sheep sheep) {
        fPlayer.PickUp(sheep);
        AdvanceStep();
    }

    public void EnterKitchen() {
        fBedroom.Hide();
    }

    private void EatFood(Food food) {
        fPlayer.PickUp(food);
        AdvanceStep();
    }

    public void ExitKitchen() {
        EnterBedroom((b) => b.ConnectToHall());
        AdvanceStep();
        fPlayer.SetPhoneTime("3:47 AM");
    }

    public void EnterHall() {
        fBedroom.Hide();
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

    public Step GetStep() {
        return mStep;
    }

    public bool CanAdvancePast(Step step) {
        return mStep == step;
    }

    public bool DidChangeToStep(Step step) {
        if (mNewStep == null) {
            return false;
        }

        return (mNewStep & step) != 0;
    }

    // -- events --
    public void DidInteract(Interact.Target target) {
        switch (target) {
            case Fan fan:
                IdentifyFan(fan); break;
            case Phone phone:
                PickUp(phone); break;
            case Body body:
                StandUp(body); break;
            case BedroomExit exit:
                ExitBedroom(exit); break;
            case Sheep sheep:
                CatchSheep(sheep); break;
            case Food food:
                EatFood(food); break;
            default:
                Log.Error("Game - Interact w/ Unknown Target: {0}", target); break;
        }
    }

    // -- module --
    public static Game Get() {
        return sInstance;
    }
}
