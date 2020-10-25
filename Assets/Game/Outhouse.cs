using UnityEngine;

public class Outhouse: MonoBehaviour {
    // -- fields --
    [SerializeField]
    [Tooltip("The collider that closes the outhouse.")]
    private Collider fDoor = null;

    // -- lifecycle --
    protected void OnTriggerEnter(Collider closeSwitch) {
        fDoor.enabled = true;
        closeSwitch.enabled = false;
        Game.Get().ExitSheepRoom();
    }
}
