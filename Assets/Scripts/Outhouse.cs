using UnityEngine;

public class Outhouse: MonoBehaviour {
    // -- fields --
    [SerializeField]
    [Tooltip("The collider that closes the outhouse.")]
    private Collider fDoor;

    // -- lifecycle --
    protected void OnTriggerEnter(Collider floorSwitch) {
        fDoor.enabled = true;
        floorSwitch.enabled = false;
    }
}
