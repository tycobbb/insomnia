using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Menu: MonoBehaviour {
    // -- constants --
    private const string kGameScene = "MainScene";
    private const float kStartDelay = 5.0f;
    private const float kFadeDuration = 1.0f;

    // -- props --
    private VideoPlayer mVideo;
    private AsyncOperation mGame;
    private float mLoadTime;

    // -- lifecycle --
    private void Start() {
        mVideo = GetComponent<VideoPlayer>();
    }

    protected void Update() {
        if (Input.GetMouseButtonDown(0) && Time.time >= kStartDelay) {
            LoadGame();
        }

        if (mLoadTime != 0.0f) {
            var elapsed = Mathf.Min(Time.time - mLoadTime, 1.0f);
            mVideo.targetCameraAlpha = 1.0f - elapsed / kFadeDuration;

            if (elapsed >= kFadeDuration) {
                StartGame();
            }
        }
    }

    // -- commands --
    private void LoadGame() {
        mGame = SceneManager.LoadSceneAsync(kGameScene);
        mGame.allowSceneActivation = false;
        mLoadTime = Time.time;
    }

    private void StartGame() {
        mGame.allowSceneActivation = true;
    }
}
