using UnityEngine;
using TMPro;

public class PhoneTime: MonoBehaviour {
    // -- fields --
    [SerializeField]
    [Tooltip("The time label.")]
    private TextMeshPro fLabel;

    // -- commands --
    public void Set(string time) {
        fLabel.text = time;
    }
}
