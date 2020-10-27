using System;
using System.Collections;
using UnityEngine;

public class Game: MonoBehaviour {
    private static Game sInstance;

    // -- types --
    [Flags]
    public enum Step: uint {
        Fan = 1 << 0,
        Phone = 1 << 1,
        Foot1 = 1 << 2,
        Door1 = 1 << 3,
        Sheep = 1 << 4,
        Exit1 = 1 << 5,
        Foot2 = 1 << 6,
        Moon = 1 << 7,
        Door2 = 1 << 8,
        Food1 = 1 << 9,
        Food2 = 1 << 10,
        Exit2 = 1 << 11,
        Foot3 = 1 << 12,
        Door3 = 1 << 13,
        Exit3 = 1 << 14,
        Foot4 = 1 << 15,
        Door4 = 1 << 16,
    }

    // -- fields --
    [SerializeField]
    [Tooltip("Whether the game is in debug mode.")]
    private bool fIsFree = false;

    [SerializeField]
    [Tooltip("Whether the game is executing debug steps.")]
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

    [SerializeField]
    [Tooltip("The eye transition.")]
    private Eye fEye = null;

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
            StartCoroutine(DebugAsync());
        }
    }

    private IEnumerator DebugAsync() {
        yield return 0;

        IdentifyFan(GetComponentInChildren<Fan>());
        PickUp(GetComponentInChildren<Phone>());
        StandUp(GetComponentInChildren<Body>());
        ExitBedroom(GetComponentInChildren<BedroomExit>());

        var r1 = GetComponentInChildren<Field>();
        DidStartEnterRoom(r1);
        DidFinishEnterRoom(r1);
        CatchSheep(GetComponentInChildren<Sheep>(true));
        ExitField();

        yield return 0;
        StandUp(GetComponentInChildren<Body>());
        IdentifyMoon(GetComponentInChildren<Moon>(true));
        // ExitBedroom(GetComponentInChildren<BedroomExit>());

        // var r2 = GetComponentInChildren<Kitchen>();
        // DidStartEnterRoom(r2);
        // DidFinishEnterRoom(r2);
        // EatFood(GetComponentInChildren<Food>(true));
        // ExitKitchen();
        //
        // yield return 0;
        // StandUp(GetComponentInChildren<Body>());

        fIsDebug = false;
    }

    // -- commands --
    public void Reset() {
        AdvanceToStep(Step.Fan);
        EnterBedroom((b) => b.ConnectToField());
        fPlayer.SetPhoneTime("1:15 AM");
    }

    private void EnterBedroom(Action<Bedroom> warp) {
        fBedroom.Show();
        warp(fBedroom);
        fPlayer.Sleep();
    }

    private void IdentifyFan(Fan _) {
        AdvancePast(Step.Fan);
    }

    private void PickUp(Phone phone) {
        fPlayer.PickUp(phone);
        AdvancePast(Step.Phone);
    }

    private void StandUp(Body _) {
        fPlayer.StandUp(isAnimated: !fIsDebug);
        AdvancePast(mStep);
    }

    private void ExitBedroom(BedroomExit exit) {
        if (mStep == Step.Door4) {
            Quit();
            return;
        }

        if (!fIsDebug) {
            exit.Open();
        }

        AdvancePast(mStep);
    }

    private void EnterField(Field room) {
        StartEnterRoom(room);
    }

    public void ExitField() {
        fEye.Open();
        fPlayer.RecenterView();
        EnterBedroom((b) => b.ConnectToKitchen());
        AdvancePast(Step.Exit1);
        fPlayer.SetPhoneTime("2:33 AM");
    }

    private void CatchSheep(Sheep sheep) {
        fPlayer.PickUp(sheep);
        AdvancePast(Step.Sheep);
    }

    private void IdentifyMoon(Moon _) {
        AdvancePast(Step.Moon);
    }

    private void EnterKitchen(Kitchen room) {
        StartEnterRoom(room);
    }

    private void EatFood(Food food) {
        fPlayer.PickUp(food);

        if (mStep == Step.Food1) {
            AdvancePast(Step.Food1);
        } else if (fPlayer.FoodEaten() >= 3) {
            AdvancePast(Step.Food2);
        }
    }

    public void ExitKitchen() {
        fEye.Open();
        fPlayer.TrimFood();
        EnterBedroom((b) => b.ConnectToHall());
        AdvancePast(Step.Exit2);
        fPlayer.SetPhoneTime("3:47 AM");
    }

    private void EnterHall(Hall room) {
        StartEnterRoom(room);
    }

    public void ExitHall() {
        fEye.Open(isWhite: true);
        fPlayer.RecenterView();
        fPlayer.ClearInventory();

        EnterBedroom((b) => b.SetDaytime());
        AdvancePast(Step.Exit3);
        fPlayer.SetPhoneTime("6:15 AM");
    }

    private void StartEnterRoom(Room room) {
        room.EnterStart();
        fBedroom.Hide();
    }

    private void FinishEnterRoom(Room room) {
        room.EnterEnd();
        fBedroom.HideVolume();
    }

    private void Quit() {
        Application.Quit();
    }

    // -- commands/step
    private void AdvancePast(Step step) {
        AdvanceToStep((Step)((ushort)step << 1));
    }

    private void AdvanceToStep(Step step) {
        if (step == mStep) {
            return;
        }

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
            case Moon moon:
                IdentifyMoon(moon); break;
            case Food food:
                EatFood(food); break;
            default:
                Log.Error("Game - Interact w/ Unknown Target: {0}", target); break;
        }
    }

    public void DidStartEnterRoom(Room target) {
        switch (target) {
            case Field room:
                EnterField(room); break;
            case Kitchen room:
                EnterKitchen(room); break;
            case Hall room:
                EnterHall(room); break;
        }
    }

    public void DidFinishEnterRoom(Room room) {
        switch (room) {
            default:
                FinishEnterRoom(room); break;
        }
    }

    // -- module --
    public static Game Get() {
        return sInstance;
    }
}
