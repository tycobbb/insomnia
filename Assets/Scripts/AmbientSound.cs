using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AmbientSound: MonoBehaviour {
    // -- fields --
    [SerializeField]
    [Tooltip("If the sound should automatically start playing.")]
    private bool fAutoplay = false;

    [SerializeField]
    [Tooltip("How long to wait before autoplaying.")]
    private float fAutoplayDelay = 0.0f;

    [SerializeField]
    [Tooltip("The min delay in seconds between sounds.")]
    private float fMinDelay = 4.0f;

    [SerializeField]
    [Tooltip("The max delay in seconds between sounds.")]
    private float fMaxDelay = 10.0f;

    [SerializeField]
    [Tooltip("The ambient audio source. The first source on the component if unset.")]
    private AudioSource fAudioSource = null;

    [SerializeField]
    [Tooltip("The ambient sounds to randomize.")]
    private AudioClip[] fSounds = null;

    // -- props --
    private Coroutine mActive = null;

    // -- lifecycle --
    protected void Awake() {
        if (fAudioSource == null) {
            fAudioSource = GetComponent<AudioSource>();
        }
    }

    protected void Start() {
        if (fAutoplay) {
            Autoplay();
        }
    }

    // -- commands --
    public void Play(AudioClip[] sounds = null) {
        Stop();

        if (sounds != null) {
            fSounds = sounds;
        }

        mActive = StartCoroutine(PlayAsync());
    }

    public void Stop() {
        if (mActive != null) {
            StopCoroutine(mActive);
        }

        mActive = null;
    }

    private IEnumerator PlayAsync() {
        while (true) {
            // select a random sound if necessary
            if (fSounds != null) {
                fAudioSource.clip = fSounds[Random.Range(0, fSounds.Length)];
            }

            // play the sound
            fAudioSource.Play();

            // add random delay before playing the next sound
            var duration = fAudioSource.clip.length;
            var delay = duration + Random.Range(fMinDelay, fMaxDelay);
            yield return new WaitForSeconds(delay);
        }
    }

    public void Autoplay() {
        StartCoroutine(AutoplayAsync());
    }

    public IEnumerator AutoplayAsync() {
        yield return new WaitForSeconds(fAutoplayDelay);
        Play();
    }
}
