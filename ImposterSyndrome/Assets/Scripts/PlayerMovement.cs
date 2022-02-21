using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private float speed = 5.5F; // Goldilocks speed
    private float jumpHeight = 7F; // Goldilocks jump height
    private Animator anim;
    private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    // private float jumpCooldown;
    private float wallJumpCooldown;
    float horizontalInput;

    private void Awake()
    {
        // Create References
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    } // Awake()


    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // flip player to face right or left based on Horizontal Input (Scale x3 for proper character size)
        if (horizontalInput > 0.01F)
            transform.localScale = new Vector3(3,3,1); 
        else if (horizontalInput < -0.01F)
            transform.localScale = new Vector3(-3,3,1);


        if (wallJumpCooldown > 0.2F)
        {
            
            // Update X-axis position
            body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);

            if (onWall() && isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else body.gravityScale = 2.5F;

            // Jump when Space is pressed
            // TODO allow for other keys?
            if (Input.GetKey(KeyCode.Space))
                Jump();

        } // if wallJumpCooldown
        else wallJumpCooldown += Time.deltaTime;


        // Set Animation parameters for running and jumping
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());
    } // Update()


    private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpHeight);
            anim.SetTrigger("jump");
        }
        else if(onWall() && !isGrounded())
        {
            if(horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x * 10), 0);
                transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x*3), 6);
            wallJumpCooldown = 0;
        }
        
    } // Jump()


    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1F, groundLayer);
        return raycastHit.collider != null;
    } // is Grounded


    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1F, wallLayer);
        return raycastHit.collider != null;
    } // onWall()
}
