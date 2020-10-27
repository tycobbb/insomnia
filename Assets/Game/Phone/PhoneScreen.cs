using System;
using UnityEngine;
using TMPro;

public class PhoneScreen: MonoBehaviour {
    // -- fields --
    [SerializeField]
    [Tooltip("The time field.")]
    private TextMeshPro fTime = null;

    [SerializeField]
    [Tooltip("The material when the screen is on.")]
    private Material fScreenOn = null;

    // -- props --
    private Renderer mRenderer;
    private AmbientSound mAmbientSound;

    // -- lifecycle --
    protected void Awake() {
        mRenderer = GetComponent<Renderer>();
        mAmbientSound = GetComponent<AmbientSound>();
    }

    // -- commands --
    public void TurnOn() {
        fTime.gameObject.SetActive(true);
        mRenderer.material = fScreenOn;
        mAmbientSound.Play();
    }

    public void SetTime(string time) {
        fTime.text = time;
    }
}
