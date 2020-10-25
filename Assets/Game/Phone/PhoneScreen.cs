using UnityEngine;
using TMPro;

public class PhoneScreen: MonoBehaviour {
    // -- fields --
    [SerializeField]
    [Tooltip("The time label.")]
    private TextMeshPro fTime;

    [SerializeField]
    [Tooltip("The material when the screen is on.")]
    private Material fScreenOn;

    // -- commands --
    public void TurnOn() {
        GetComponent<Renderer>().material = fScreenOn;
        fTime.gameObject.SetActive(true);
    }

    public void SetTime(string time) {
        fTime.text = time;
    }
}
