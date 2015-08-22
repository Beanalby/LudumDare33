using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Cat : MonoBehaviour {

    private Animator anim;
    private Rigidbody2D rb;
    private Transform tSprite;

    public void Start() {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        tSprite = transform.Find("sprite");
    }

    public void Update() {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal"), 0);
        anim.SetFloat("hSpeed", Mathf.Abs(rb.velocity.x));
        // flip the sprite if necessary
        if ((rb.velocity.x > 0 && tSprite.localScale.x < 0f)
                || (rb.velocity.x < 0 && tSprite.localScale.x > 0f)) {
            tSprite.localScale = new Vector3(
                -tSprite.localScale.x,
                tSprite.localScale.y,
                tSprite.localScale.z);
            }
    }
    
}
