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
    }

    // -- model --
    private Step mStep = Step.Phone;
    private Step? mNewStep = Step.Phone;
    private Player mPlayer;

    // -- lifecycle --
    protected void Awake() {
        _instance = this;

        // toggle this line to debug different game states
        StartCoroutine(DebugSetup());
    }

    private IEnumerator DebugSetup() {
        yield return 0;
        PickUp(GetComponentInChildren<Phone>());
        StandUp();
    }

    // -- commands --
    public void Setup(Player player) {
        mPlayer = player;
    }

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
            default:
                Debug.LogErrorFormat("Interacting with unknown target: {0}", target); break;
        }
    }

    // -- module --
    public static Game Get() {
        return _instance;
    }
}
