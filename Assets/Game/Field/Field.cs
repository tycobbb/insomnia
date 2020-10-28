using UnityEngine;

public class Field: MonoBehaviour, Room {
    // -- fields --
    [SerializeField]
    [Tooltip("The room's entrance door.")]
    private GameObject fDoor = null;

    [SerializeField]
    [Tooltip("The sheeps.")]
    private GameObject fSheeps = null;

    // -- props --
    private RoomPost mPost;
    private AudioSource mAudio;

    // -- lifecycle --
    protected void Awake() {
        mPost = GetComponentInChildren<RoomPost>();
        mAudio = GetComponent<AudioSource>();
    }

    // -- Room --
    // -- Room/commands
    public void SetActive(bool isActive) {
        gameObject.SetActive(isActive);

        // disable sheeps and unblended post-processing until entering the room
        fSheeps.SetActive(false);
        mPost.SetBlended(true);
    }

    public void EnterStart() {
        fSheeps.SetActive(true);
        mAudio.Play();
    }

    public void EnterEnd() {
        mPost.SetBlended(false);
    }

    // -- Room/queries
    public GameObject Door() {
        return fDoor;
    }
}
