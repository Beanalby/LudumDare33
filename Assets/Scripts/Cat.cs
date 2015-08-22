using UnityEngine;
using System.Collections;

namespace LudumDare33 {
    [RequireComponent(typeof(Rigidbody2D), typeof(Mover))]
    public class Cat : MonoBehaviour {

        private Animator anim;
        private Rigidbody2D rb;
        private Mover mover;

        public void Start() {
            anim = GetComponentInChildren<Animator>();
            mover = GetComponent<Mover>();
            rb = GetComponent<Rigidbody2D>();
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