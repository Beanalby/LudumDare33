using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace LudumDare33 {
    public delegate void StageStartListener();
    public delegate void StageEndListener();

    public class StageDriver : MonoBehaviour {
        private static StageDriver _instance;
        public static StageDriver Instance {
            get {
                if (_instance == null) {
                    Debug.LogError("No StageDriver instance");
                }
                return _instance;
            }
        }

        public Text uiScore, uiTime;
        public StageStartListener OnStageStart;
        public StageEndListener OnStageEnd;
        public GameObject StartStageUI, EndStageUI;

        private ShowControls controls;
        private float timeTotal = 10;
        private float timeStart = -1f;
        private int score = 0;

        private bool _isRunning = false;
        public bool IsRunning { get { return _isRunning; } }
        BirdTarget[] birdTargets;

        #region MonoBehavior

        public void Awake() {
            if (_instance != null) {
                Debug.LogError("Already have a StageDriver " + _instance);
                GameObject.Destroy(this.gameObject);
                return;
            }
            _instance = this;

        }

        public void Start() {
            birdTargets = FindObjectsOfType<BirdTarget>();
            controls = ShowControls.CreateDocked(new ControlItem[] {
                new ControlItem("Use arrow keys to walk left and right",
                    new KeyCode[] { KeyCode.LeftArrow, KeyCode.RightArrow }),
                new ControlItem("Press space to jump", KeyCode.Space)
            });
            controls.position = ShowControlPosition.Bottom;
            controls.showDuration = -1;
            controls.Show();
        }


        public void Update() {
            CheckForStageStart();
            UpdateUI();
            CheckForStageEnd();
        }

        #endregion

        #region public
        public BirdTarget getNewBirdTarget(BirdTarget current) {
            BirdTarget target;
            while (true) {
                target = birdTargets[Random.Range(0, birdTargets.Length - 1)];
                if (target != current) {
                    return target;
                }
            }
        }

        public void BirdKilled() {
            score += 1;
            uiScore.text = score.ToString();
        }

        public void PlayAgainClicked() {
            Application.LoadLevel(Application.loadedLevel);
        }
        #endregion

        #region private
        private void CheckForStageStart() {
            if (!IsRunning && timeStart == -1 && Input.GetButtonDown("Jump")) {
                StartStage();
            }
        }

        private void StartStage() {
            timeStart = Time.time;
            _isRunning = true;
            StartStageUI.SetActive(false);
            controls.Hide(2);
            if (OnStageStart != null) {
                OnStageStart();
            }
        }

        private void UpdateUI() {
            if (IsRunning) {
                uiTime.text = (timeTotal - (Time.time - timeStart)).ToString(".0");
            }
        }

        private  void CheckForStageEnd() {
            if (IsRunning && Time.time - timeStart > timeTotal) {
                EndStage();
            }
        }
        private void EndStage() {
            _isRunning = false;
            uiTime.text = "0";
            EndStageUI.SetActive(true);
            if (OnStageEnd != null) {
                OnStageEnd();
            }
        }
        #endregion
    }
}