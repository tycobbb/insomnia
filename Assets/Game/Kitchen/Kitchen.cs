using UnityEngine;

public class Kitchen: MonoBehaviour, Room {
    // -- fields --
    [SerializeField]
    [Tooltip("The room's entrance door.")]
    private GameObject fDoor = null;

    [SerializeField]
    [Tooltip("The wall between the room and bedroom.")]
    private GameObject fDoorWall = null;

    // -- props --
    private RoomPost mPost;

    // -- lifecycle --
    protected void Awake() {
        mPost = GetComponentInChildren<RoomPost>();
    }

    // -- Room --
    // -- Room/commands
    public void SetActive(bool isActive) {
        gameObject.SetActive(isActive);
        mPost.SetBlended(true);
    }

    public void EnterStart() {
        fDoorWall.SetActive(true);
    }

    public void EnterEnd() {
        mPost.SetBlended(false);
    }

    // -- Room/queries
    public GameObject Door() {
        return fDoor;
    }
}
