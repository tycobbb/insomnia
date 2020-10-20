using System.Collections;
using UnityEngine;

public class Game: MonoBehaviour {
    private static Game _instance;

    // -- types --
    public enum Step: ushort {
        Phone = 1 << 0,
        Foot1 = 1 << 1,
        Door1 = 1 << 2,
        Sheep = 1 << 3,
        Outhouse = 1 << 4,
        Foot2 = 1 << 5,
        Door2 = 1 << 6,
    }

    public enum Room: ushort {
        Sheep,
        Kitchen,
        Hall
    }

    // -- fields --
    [SerializeField]
    [Tooltip("Whether the game is in debug mode.")]
    private bool fIsFree = false;

    [SerializeField]
    [Tooltip("Whether the game is in debug mode.")]
    private bool fIsDebug = false;

    [SerializeField]
    [Tooltip("The player.")]
    private Player fPlayer;

    [SerializeField]
    [Tooltip("The bedroom.")]
    private Bedroom fBedroom;

    // -- model --
    private Step mStep = Step.Phone;
    private Step? mNewStep = Step.Phone;
    private Room mNextRoom = Room.Sheep;

    // -- lifecycle --
    protected void Awake() {
        _instance = this;

    }

    protected void Start() {
        // abort game logic if in free mode
        if (fIsFree) {
            return;
        }

        // move bedroom and player to initial position
        EnterBedroom();

        // run debug setup if enabled
        if (fIsDebug) {
            StartCoroutine(DebugSetup());
        }
    }

    private IEnumerator DebugSetup() {
        yield return 0;
        PickUp(GetComponentInChildren<Phone>());
        StandUp(GetComponentInChildren<Body>());
        Open(GetComponentInChildren<Door>());
        Catch(GetComponentInChildren<Sheep>());
    }

    // -- commands --
    public void EnterBedroom() {
        fBedroom.Show();

        // warp bedroom to correct room given game state
        switch (mNextRoom) {
            case Room.Sheep:
                fBedroom.WarpToSheep(); break;
            case Room.Kitchen:
                fBedroom.WarpToFood(); break;
            case Room.Hall:
                fBedroom.WarpToHall(); break;
        }

        // reset player to the sleeping pos
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
        fBedroom.CloseDoor();
        fBedroom.Hide();
        mNextRoom = Room.Kitchen;
    }

    public void Catch(Sheep sheep) {
        fPlayer.PickUp(sheep);
        AdvanceStep();
    }

    public void EnterKitchen() {
        fBedroom.CloseDoor();
        fBedroom.Hide();
        mNextRoom = Room.Hall;
    }

    // -- commands/step
    private void AdvanceStep() {
        AdvanceToStep(mStep + 1);
    }

    private void AdvanceToStep(Step step) {
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
            default:
                Debug.LogErrorFormat("Interacting with unknown target: {0}", target); break;
        }
    }

    // -- module --
    public static Game Get() {
        return _instance;
    }
}
