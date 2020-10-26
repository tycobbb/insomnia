using System.Collections;
using UnityEngine;

public class BedroomExit: MonoBehaviour, Interact.Target {
    // -- constants --
    private const Game.Step kStep = Game.Step.Door1 | Game.Step.Door2 | Game.Step.Door3;
    private const float kEnableDelay = 4.5f;

    // -- fields --
    [SerializeField]
    [Tooltip("The sheep sound.")]
    private AudioClip fSheepSound = null;

    // -- props --
    private Door mDoor;
    private Interact.OnHover mHover;
    private AmbientSound mAmbientSound;

    // -- lifecycle --
    protected void Start() {
        mDoor = GetComponent<Door>();
        mHover = GetComponentInChildren<Interact.OnHover>();
        mAmbientSound = GetComponent<AmbientSound>();
    }

    protected void Update() {
        var game = Game.Get();
        if (game.DidChangeToStep(kStep)) {
            Enable(game.GetStep());
        }
    }

    // -- commands --
    private void Enable(Game.Step step) {
        StartCoroutine(EnableAsync(step));
    }

    private IEnumerator EnableAsync(Game.Step step) {
        if (step != Game.Step.Door2) {
            yield return new WaitForSeconds(kEnableDelay);
        }

        mHover.Reset();

        switch (step) {
            case Game.Step.Door1:
                mAmbientSound.Play(fSheepSound); break;
            case Game.Step.Door2:
                break; // TODO: play a sound to cue the door to the kitchen
            case Game.Step.Door3:
                break; // TODO: play a sound to cue the door to the hall
        }
    }

    public void Open() {
        mAmbientSound.Stop();
        mDoor.Open();
    }

    public void Close() {
        mDoor.Close();
    }
}
