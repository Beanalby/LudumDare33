using UnityEngine;
using System.Collections;
using Prime31;

namespace LudumDare33 {
    public delegate void DeathListener(Mover obj);

    [RequireComponent(typeof(CharacterController2D),typeof(Animator))]
    public class Mover : MonoBehaviour {

        public Sprite deathSprite;
        public GameObject deathEffect;
        public AudioClip jumpSound, dieSound;
        public float groundDampening = 20f;
        public float maxSpeed = 3;
        public float jumpSpeed = 10;
        public bool applyGravity = true;
        public DeathListener OnDeath;

        private bool isJumping = false;
        private SpriteRenderer sr;
        private Rigidbody2D rb;
        private Animator anim;
        private bool wasGrounded = true;

        private CharacterController2D cc;
        private Transform tSprite;

        void Start() {
            cc = GetComponent<CharacterController2D>();
            sr = GetComponentInChildren<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponentInChildren<Animator>();
            tSprite = transform.Find("sprite");
        }

        public void Update() {
            // flip the sprite if necessary
            if ((rb.velocity.x > 0 && transform.localScale.x < 0f)
                    || (rb.velocity.x < 0 && tSprite.localScale.x > 0f)) {
                tSprite.localScale = new Vector3(
                    -tSprite.localScale.x,
                    tSprite.localScale.y,
                    tSprite.localScale.z);
            }
        }

        public void Jump() {
            if (cc.isGrounded) {
                isJumping = true;
            }
        }

        public void Move(Vector3 dir) {
            Move(dir.x, dir.y);
        }

        public void Move(float x, float y=0) {
            bool isGrounded = cc.isGrounded, didJump=false;
            Vector3 velocity = cc.velocity;
            if (applyGravity) {
                velocity.y += Time.deltaTime * Physics2D.gravity.y;
            }
            if(isGrounded && !wasGrounded && anim && !isJumping) {
                anim.SetTrigger("didLand");
            }
            wasGrounded = cc.isGrounded;

            if (isJumping) {
                velocity.y = jumpSpeed;
                isJumping = false;
                didJump = true;
                if (anim) {
                    anim.ResetTrigger("didLand");
                    anim.SetTrigger("didJump");
                }
                if(jumpSound)
                    AudioSource.PlayClipAtPoint(jumpSound, Camera.main.transform.position);
            }

            velocity.x = Mathf.Lerp(velocity.x, x * maxSpeed, Time.fixedDeltaTime * groundDampening);
            if (y != 0) {
                velocity.y = Mathf.Lerp(velocity.y, y * maxSpeed, Time.fixedDeltaTime * groundDampening);
            }
            if (anim) {
                anim.SetFloat("hSpeed", Mathf.Abs(cc.velocity.x));
                // if we jumped this frame, bump up our reported velocity
                // to avoid immediately triggering the "falling while jumping" transition state
                anim.SetFloat("vSpeed", cc.velocity.y + (didJump ? jumpSpeed : 0) );
            }

            Vector3 delta = cc.velocity * Time.deltaTime;
            // flip ourselves if our direction changed
            if ((delta.x > 0 && transform.localScale.x < 0f)
                    || (delta.x < 0 && transform.localScale.x > 0f)) {
                transform.localScale = new Vector3(
                    -transform.localScale.x,
                    transform.localScale.y,
                    transform.localScale.z);
            }
            //Debug.Log("isGrounded=" + cc.isGrounded + ", moving " + (velocity * Time.deltaTime).ToString(".000"));
            cc.move(velocity * Time.deltaTime);
        }

        public void Stop() {
            cc.velocity = Vector3.zero;
            if (anim) {
                anim.SetFloat("hSpeed", 0);
                anim.SetFloat("vSpeed", 0);
            }
        }

        public void Die() {
            if(dieSound)
                AudioSource.PlayClipAtPoint(dieSound, Camera.main.transform.position);
            if (deathSprite) {
                cc.enabled = false;
                GetComponent<BoxCollider2D>().enabled = false;
                rb.isKinematic = false;
                rb.velocity = new Vector2(Random.Range(-5, 5), 8);
                float angle = Random.Range(60, 120);
                if (Random.Range(0, 2) == 0) {
                    angle = -angle;
                }
                rb.angularVelocity = angle;
                sr.sprite = deathSprite;
                Destroy(gameObject, 5f);
            } else {
                if (deathEffect) {
                    Instantiate(deathEffect, transform.position, Quaternion.identity);
                }
                Destroy(gameObject);
            }
            this.enabled = false;
            if (OnDeath != null) {
                OnDeath(this);
            }
       }
    }
}