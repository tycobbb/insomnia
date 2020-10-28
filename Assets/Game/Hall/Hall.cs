using UnityEngine;

public class Hall : MonoBehaviour, Room {
    // -- fields --
    [SerializeField] [Tooltip("The room's entrance door.")]
    private GameObject fDoor = null;

    [SerializeField] [Tooltip("The wall between the room and bedroom.")]
    private GameObject fDoorWall = null;

    // -- props --
    private RoomPost mPost;
    private AmbientSound[] mAmbientSounds;

    // -- lifecycle --
    protected void Awake() {
        mPost = GetComponentInChildren<RoomPost>();
        mAmbientSounds = GetComponentsInChildren<AmbientSound>();
    }

    // -- Room --
    // -- Room/commands
    public void SetActive(bool isActive) {
        gameObject.SetActive(isActive);
        mPost.SetBlended(true);
    }

    public void EnterStart() {
        fDoorWall.SetActive(true);

        // star playing ambient sounds
        foreach (var sound in mAmbientSounds) {
            sound.Play();
        }
    }

    public void EnterEnd() {
        mPost.SetBlended(false);
    }

    // -- Room/queries
    public GameObject Door() {
        return fDoor;
    }
}

