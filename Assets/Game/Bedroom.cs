using UnityEngine;

public class Bedroom: MonoBehaviour {
    // -- fields --
    [SerializeField]
    [Tooltip("The bedroom door.")]
    private Door fDoor = null;

    [SerializeField]
    [Tooltip("The sheep room.")]
    private SheepRoom fSheepRoom = null;

    [SerializeField]
    [Tooltip("The kitchen.")]
    private Kitchen fKitchen = null;

    [SerializeField]
    [Tooltip("The hall.")]
    private Hall fHall = null;

    // -- props --
    private Room mConnected;

    // -- lifecycle --
    // protected void Start() {
    //     // mask out the house
    //     var renderers = GetComponentsInChildren<Renderer>();
    //
    //     foreach (var renderer in renderers) {
    //         if (renderer.material.renderQueue < 2001 && renderer.gameObject != fDoor) {
    //             renderer.material.renderQueue = 9001;
    //         }
    //     }
    // }

    // -- commands --
    public void Hide() {
        fDoor.Close();
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void ConnectToSheep() {
        ConnectToRoom(fSheepRoom);
    }

    public void ConnectToKitchen() {
        ConnectToRoom(fKitchen);
    }

    public void ConnectToHall() {
        ConnectToRoom(fHall);
    }

    private void ConnectToRoom(Room room) {
        // hide the old room
        if (mConnected != null) {
            mConnected.SetActive(false);
        }

        // and show the new one
        mConnected = room;
        mConnected.SetActive(true);

        // align bedroom to the connected room's door
        var door = mConnected.Door();

        // given the new door's world position
        var position = door.transform.position;
        // offset by the bedroom door's local position (our pivots are really bad)
        position -= fDoor.transform.localPosition;
        // and center the door
        position -= new Vector3(0.5f, 0.0f, 0.0f);

        // move room and hide door
        door.SetActive(false);
        transform.position = position;
    }
}
