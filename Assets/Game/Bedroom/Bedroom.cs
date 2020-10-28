using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Bedroom: MonoBehaviour {
    // -- fields --
    [SerializeField]
    [Tooltip("The sheep room.")]
    private Field fField = null;

    [SerializeField]
    [Tooltip("The kitchen.")]
    private Kitchen fKitchen = null;

    [SerializeField]
    [Tooltip("The hall.")]
    private Hall fHall = null;

    [SerializeField]
    [Tooltip("The night sky in the window.")]
    private GameObject fNightSky = null;

    [SerializeField]
    [Tooltip("The nighttime post processing.")]
    private GameObject mPostNight = null;

    [SerializeField]
    [Tooltip("The daytime post processing.")]
    private GameObject mPostDay = null;

    [SerializeField]
    [Tooltip("The fan.")]
    private Fan mFan = null;

    // -- props --
    private Room mConnected;
    private Door mDoor;
    private Vector3 mPostNightOffset;

    // -- lifecycle --
    protected void Awake() {
        mDoor = GetComponentInChildren<Door>();

        // capture the volume's offset and disassociate it from the bedroom
        // so that we can move it idependently
        var t = mPostNight.transform;
        mPostNightOffset = t.localPosition;
        t.parent = transform.parent;
    }

    // protected void Start() {
    //     // mask out the house
    //     var renderers = GetComponentsInChildren<Renderer>();
    //
    //     foreach (var renderer in renderers) {
    //         if (renderer.material.renderQueue < 2001 && renderer.gameObject != mDoor) {
    //             renderer.material.renderQueue = 9001;
    //         }
    //     }
    // }

    // -- commands --
    public void Show() {
        gameObject.SetActive(true);
        mPostNight.SetActive(true);
    }

    public void Hide() {
        mDoor.Close();
        gameObject.SetActive(false);
    }

    public void HideVolume() {
        mPostNight.SetActive(false);
    }

    public void ConnectToField() {
        ConnectToRoom(fField);
    }

    public void ConnectToKitchen() {
        ConnectToRoom(fKitchen);
    }

    public void ConnectToHall() {
        ConnectToRoom(fHall);
    }

    public void SetDaytime() {
        ConnectToRoom(fField);
        fNightSky.SetActive(false);
        mPostNight.SetActive(false);
        mPostDay.SetActive(true);
        mFan.Silence();
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
        position -= mDoor.transform.localPosition;
        // and center the door
        position -= new Vector3(0.5f, 0.0f, 0.0f);

        // move room and hide door
        door.SetActive(false);
        transform.position = position;
        mPostNight.transform.position = position + mPostNightOffset;
    }
}
