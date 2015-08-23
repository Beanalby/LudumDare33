using UnityEngine;
using System.Collections;

namespace LudumDare33 {
    [RequireComponent(typeof(Rigidbody2D), typeof(Mover))]
    public class Cat : MonoBehaviour {

        private Mover mover;

        public void Start() {
            mover = GetComponent<Mover>();
        }

        public void Update() {
            if (Input.GetButtonDown("Jump")) {
                mover.Jump();
            }

        }
        public void FixedUpdate() {
            mover.Move(Input.GetAxis("Horizontal"));
        }
    }
}