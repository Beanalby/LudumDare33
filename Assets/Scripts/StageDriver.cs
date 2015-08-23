using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LudumDare33 {
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

        private bool _isRunning = true;
        public bool IsRunning { get { return _isRunning; } }
        BirdTarget[] birdTargets;

        public void Awake() {
            if (_instance != null) {
                Debug.LogError("Already have a StageDriver " + _instance);
                GameObject.Destroy(this.gameObject);
                return;
            }
            _instance = this;

            birdTargets = FindObjectsOfType<BirdTarget>();
        }

        public BirdTarget getNewBirdTarget(BirdTarget current) {
            BirdTarget target;
            while (true) {
                target = birdTargets[Random.Range(0, birdTargets.Length - 1)];
                if (target != current) {
                    return target;
                }
            }
        }
    }
}