using UnityEngine;

public class SheepRoom: MonoBehaviour, Room {
    // -- fields --
    [SerializeField]
    [Tooltip("The room's entrance door.")]
    private GameObject fDoor = null;

    [SerializeField]
    [Tooltip("The sheeps.")]
    private GameObject fSheeps = null;

    // -- commands --
    public void Enter() {
        fSheeps.SetActive(true);
    }

    // -- Room --
    // -- Room/commands
    public void SetActive(bool isActive) {
        gameObject.SetActive(isActive);

        // disable sheeps until entering the room
        fSheeps.SetActive(false);
    }

    // -- Room/queries
    public GameObject Door() {
        return fDoor;
    }
}
