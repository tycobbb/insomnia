using System;
using UnityEngine;
using TMPro;

public class PhoneScreen: MonoBehaviour {
    // -- fields --
    [SerializeField]
    [Tooltip("If the screen is on by default.")]
    private bool fIsOn = false;

    [SerializeField]
    [Tooltip("The time field.")]
    private TextMeshPro fTime = null;

    [SerializeField]
    [Tooltip("The material when the screen is on.")]
    private Material fScreenOn = null;

    [SerializeField] [Tooltip("The sounds to play when the screen is on.")]
    private AudioClip[] fSounds = null;

    // -- props --
    private Renderer mRenderer;
    private AmbientSound mAmbientSound;

    // -- lifecycle --
    protected void Awake() {
        mRenderer = GetComponent<Renderer>();
        mAmbientSound = GetComponent<AmbientSound>();
    }

    protected void Start() {
        if (fIsOn) {
            TurnOn();
        }
    }

    // -- commands --
    public void TurnOn() {
        fTime.gameObject.SetActive(true);
        mRenderer.material = fScreenOn;
        mAmbientSound.Play(fSounds);
    }

    public void SetTime(string time) {
        fTime.text = time;
    }
}
