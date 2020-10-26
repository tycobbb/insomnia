using UnityEngine;

public class Kitchen: MonoBehaviour, Room {
    // -- fields --
    [SerializeField]
    [Tooltip("The room's entrance door.")]
    private GameObject fDoor = null;

    [SerializeField]
    [Tooltip("The wall between the room and bedroom.")]
    private GameObject fDoorWall = null;

    // -- Room --
    // -- Room/commands
    public void Enter() {
        fDoorWall.SetActive(true);
    }

    public void SetActive(bool isActive) {
        gameObject.SetActive(isActive);
    }

    // -- Room/queries
    public GameObject Door() {
        return fDoor;
    }
}
