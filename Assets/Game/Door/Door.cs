using UnityEditor;
using UnityEngine;

public class Door: MonoBehaviour, Interact.Target {
    // -- constants --
    private const string kOpenAnim = "Open";
    private const string kCloseAnim = "Close";

    // -- fields --
    [SerializeField]
    [Tooltip("The audio source. The first source on the component if unset.")]
    private AudioSource fAudioSource = null;

    [SerializeField]
    [Tooltip("The audio source. The first source on the component if unset.")]
    private AudioClip fOpenSound = null;

    [SerializeField]
    [Tooltip("The audio source. The first source on the component if unset.")]
    private AudioClip fCloseSound = null;

    // -- props --
    private Animator mAnimator;

    // -- lifecycle --
    protected void Awake() {
        mAnimator = GetComponent<Animator>();

        if (fAudioSource == null) {
            fAudioSource = GetComponent<AudioSource>();
        }
    }

    // -- commands --
    public void Open() {
        mAnimator.Play(kOpenAnim);
        PlaySound(fOpenSound);
    }

    public void Close() {
        mAnimator.Play(kCloseAnim);
        PlaySound(fCloseSound);
    }

    private void PlaySound(AudioClip clip) {
        fAudioSource.clip = clip;
        fAudioSource.Play();
    }
}
