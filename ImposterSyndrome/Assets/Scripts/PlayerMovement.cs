using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private float speed = 5.5F; // Comfortable Goldilocks speed
    private Animator anim;
    private bool grounded;
    
    private void Awake()
    {
        // Create References
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        // Update X-axis position
        body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);

        // flip player to face right or left based on Horizontal Input (Scale x3 for proper character size)
        if (horizontalInput > 0.01F)
            transform.localScale = new Vector3(3,3,1); 
        else if (horizontalInput < -0.01F)
            transform.localScale = new Vector3(-3,3,1);

        // Jump when Space is pressed
        // TODO allow for other keys?
        if (Input.GetKey(KeyCode.Space) && grounded)
            Jump();

        // Set Animation parameters for running and jumping
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", grounded);
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, speed);
        grounded = false;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            grounded = true;
    }
}
