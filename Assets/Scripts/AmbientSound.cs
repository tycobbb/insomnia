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
    private Coroutine mActiveSound = null;

    // -- lifecycle --
    protected void Start() {
        if (fAudioSource == null) {
            fAudioSource = GetComponent<AudioSource>();
        }
    }

    // -- commands --
    public void Play(AudioClip clip = null) {
        Stop();

        // play the specified clip, if passed
        if (clip != null) {
            fAudioSource.clip = clip;
        }

        mActiveSound = StartCoroutine(PlayAsync());
    }

    public void Stop() {
        if (mActiveSound != null) {
            StopCoroutine(mActiveSound);
        }

        mActiveSound = null;
    }

    private IEnumerator PlayAsync() {
        var duration = fAudioSource.clip.length;

        while (true) {
            fAudioSource.Play();
            var delay = duration + Random.Range(fMinDelay, fMaxDelay);
            yield return new WaitForSeconds(delay);
        }
    }
}
