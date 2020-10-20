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

    // -- fields --
    [SerializeField]
    [Tooltip("Whether the game is in debug mode.")]
    private bool fIsFree = false;

    [SerializeField]
    [Tooltip("Whether the game is in debug mode.")]
    private bool fIsDebug = false;

    // -- model --
    private Step mStep = Step.Phone;
    private Step? mNewStep = Step.Phone;
    private Player mPlayer;
    private Bedroom mBedroom;

    // -- lifecycle --
    protected void Awake() {
        _instance = this;

    }

    protected void Start() {
        // abort game logic if in free mode
        if (fIsFree) {
            return;
        }

        // move room & player to initial position
        mBedroom.WarpToSheep();
        mPlayer.Sleep();

        // run debug setup if enabled
        if (fIsDebug) {
            StartCoroutine(DebugSetup());
        }
    }

    private IEnumerator DebugSetup() {
        yield return 0;
        PickUp(GetComponentInChildren<Phone>());
        StandUp();
        Open(GetComponentInChildren<Door>());
        Catch(GetComponentInChildren<Sheep>());
    }

    // -- setup --
    public void Register(Player player) {
        mPlayer = player;
    }

    public void Register(Bedroom bedroom) {
        mBedroom = bedroom;
    }

    // -- commands --
    public void PickUp(Phone phone) {
        mPlayer.PickUp(phone);
        AdvanceStep();
    }

    public void StandUp() {
        mPlayer.StandUp();
        AdvanceStep();
    }

    public void Open(Door door) {
        door.Open();
        AdvanceStep();
    }

    public void Catch(Sheep sheep) {
        mPlayer.PickUp(sheep);
        AdvanceStep();
    }

    public void EnterOuthouse() {
        mBedroom.WarpToFood();
        mPlayer.Sleep();
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
            case Foot foot:
                StandUp(); break;
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
