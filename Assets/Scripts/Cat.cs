using UnityEngine;
using System.Collections;

namespace LudumDare33 {
    [RequireComponent(typeof(Rigidbody2D), typeof(Mover))]
    public class Cat : MonoBehaviour {

        private Mover mover;
        private bool canControl = false;
        private float controlNextFrame = -1;

        #region MonoBehavior
        public void Start() {
            mover = GetComponent<Mover>();
            StageDriver.Instance.OnStageStart += OnStageStart;
            StageDriver.Instance.OnStageEnd += OnStageEnd;
        }

        public void Update() {
            CheckEnableControl();
            if (canControl && Input.GetButtonDown("Jump")) {
                mover.Jump();
            }
        }
        public void FixedUpdate() {
            float x = 0;
            if (canControl) {
                x = Input.GetAxis("Horizontal");
            }
            mover.Move(x);
        }
        public void OnTriggerEnter2D(Collider2D other) {
            other.SendMessage("Attacked", this, SendMessageOptions.DontRequireReceiver);
        }
        #endregion

        #region public
        public void OnStageStart() {
            controlNextFrame = Time.time;
        }
        public void OnStageEnd() {
            // just disable control, leave evertyhing enabled so it will
            // still be visible, keep falling, etc.
            canControl = false;
        }
        #endregion

        #region private
        private void CheckEnableControl() {
            if (!canControl && controlNextFrame != -1 && controlNextFrame != Time.time) {
                canControl = true;
                controlNextFrame = -1;
            }
        }
        #endregion
    }
}