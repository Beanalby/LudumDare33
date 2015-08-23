using UnityEngine;
using System.Collections;

namespace LudumDare33 {
    [RequireComponent(typeof(Mover), typeof(Animator))]
    public class Bird : MonoBehaviour {

        private float idleDuration = 3;
        private BirdTarget target = null;
        private Mover mover;
        private Animator anim;
        private float idleStart = -1;

        public void Start() {
            StageDriver.Instance.OnStageEnd += OnStageEnd;
            mover = GetComponent<Mover>();
            anim = GetComponent<Animator>();
            GetNewTarget();
        }

        public void Update() {
            CheckNewTarget();
            DoMovement();
        }

        private void DoMovement() {
            float dist = (target.transform.position - transform.position).sqrMagnitude;
            if(dist < .0001) {
                // we're where we want to be, kill all our speed
                if (idleStart == -1) {
                    mover.Stop();
                    idleStart = Time.time;
                }
            } else {
                Vector3 dir = (target.transform.position - transform.position);
                if (dist > 0.05) {
                    dir.Normalize();
                }
                mover.Move(dir);
            }
        }

        private void CheckNewTarget() {
            // if we're idling and have waited long enough, get a new target
            if (idleStart != -1 && Time.time - idleStart > idleDuration) {
                GetNewTarget();
                idleStart = -1;
            }
        }
        private void GetNewTarget() {
            target = StageDriver.Instance.getNewBirdTarget(target);
            anim.SetBool("canIdle", target.isLanded);
        }

        public void Attacked(Cat cat) {
            mover.Die();
            StageDriver.Instance.OnStageEnd -= OnStageEnd;
        }
        public void OnStageEnd() {
            Destroy(gameObject);
        }
   }
}