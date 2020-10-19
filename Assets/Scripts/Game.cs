using System.Collections;
using UnityEngine;

public class Game: MonoBehaviour {
    private static Game _instance;

    // -- types --
    public enum Step: ushort {
        Phone,
        Foot,
        Door,
    }

    // -- model --
    private Step mStep = Step.Phone;
    private Step? mNewStep = Step.Phone;
    private Player mPlayer;

    // -- lifecycle --
    protected void Awake() {
        _instance = this;
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

    private void AdvanceStep() {
        mStep++;
        mNewStep = mStep;
        StartCoroutine(ClearNewStep());
    }

    private IEnumerator ClearNewStep() {
        yield return 0;
        mNewStep = null;
    }

    // -- queries --
    public Step? NewStep() {
        return mNewStep;
    }

    // -- module --
    public static Game Get() {
        return _instance;
    }
}
