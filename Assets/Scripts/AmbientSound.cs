using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AmbientSound: MonoBehaviour {
    // -- fields --
    [SerializeField]
    [Tooltip("The min delay in seconds between sounds.")]
    private float fMinDelay = 4.0f;

    [SerializeField]
    [Tooltip("The max delay in seconds between sounds.")]
    private float fMaxDelay = 10.0f;

    [SerializeField]
    [Tooltip("The ambient audio source. The first source on the component if unset.")]
    private AudioSource fAudioSource = null;

    // -- props --
    private AudioClip[] mSounds = null;
    private Coroutine mActive = null;

    // -- lifecycle --
    protected void Awake() {
        if (fAudioSource == null) {
            fAudioSource = GetComponent<AudioSource>();
        }
    }

    // -- commands --
    public void Play(AudioClip[] sounds = null) {
        Stop();
        mSounds = sounds;
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
            // assign a random sound if necessary
            if (mSounds != null) {
                fAudioSource.clip = mSounds[Random.Range(0, mSounds.Length)];
            }

            // play a sound
            fAudioSource.Play();

            // wait a random amount of time to play the next clip
            var duration = fAudioSource.clip.length;
            var delay = duration + Random.Range(fMinDelay, fMaxDelay);

            yield return new WaitForSeconds(delay);
        }
    }
}
