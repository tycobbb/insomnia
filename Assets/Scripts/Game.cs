using System.Collections;
using UnityEngine;

public class Game: MonoBehaviour {
    private static Game _instance;

    // -- types --
    public enum Step: ushort {
        Phone,
        Foot,
        Door,
        Sheep,
        Sleep,
    }

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
        // abort game logic if debugging w/ a different drifter
        if (mPlayer == null) {
            return;
        }

        mBedroom.WarpToSheep();
        mPlayer.Sleep();

        // toggle this line to debug different game states
        StartCoroutine(DebugSetup());
    }

    private IEnumerator DebugSetup() {
        yield return 0;
        PickUp(GetComponentInChildren<Phone>());
        StandUp();
        Open(GetComponentInChildren<Door>());
        Pet(GetComponentInChildren<Sheep>());
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

    public void Pet(Sheep sheep) {
        mPlayer.PickUp(sheep);
        AdvanceStep();
    }

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
    public bool DidChangeToStep(Step step) {
        return mNewStep == step;
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
                Pet(sheep); break;
            default:
                Debug.LogErrorFormat("Interacting with unknown target: {0}", target); break;
        }
    }

    // -- module --
    public static Game Get() {
        return _instance;
    }
}
