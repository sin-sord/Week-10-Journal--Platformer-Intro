using System.Net.Sockets;
using System.Security.Cryptography;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Rendering;
using UnityEngine.TextCore;

public class PlayerController : MonoBehaviour
{
    [Header("Rigidboy, speed, and direction")]
    public Rigidbody2D rb;  //  rigidbody
    public float speed;     //  speed of player
    Vector2 playerMovement; // players position for movement
    FacingDirection directionOfPlayer = FacingDirection.left;
    public float maxSpeed = 5;


    [Header("Gravity and Jumping")]
    public float apexHeight;
    public float apexTime;
    float gravity;
    float jumpVelocity;
    float velocity;
    public float currentTime;
    Vector2 position;
    bool isJumping;


    [Header("Terminal Speed")]
    public float terminalSpeed;


    [Header("Coyote Time")]
    public float coyoteTime = 0.5f;
    float coyoteTimeCounter;


    [Header("Dashing")]
    public float dashingSpeed = 25;
    public float dashBuildUp = 2;
    bool isDashing = false;


    public enum FacingDirection
    {
        left, right
    }

    // Start is called before the first frame update
    void Start()
    {
        rb.GetComponent<Rigidbody2D>();

        //  The equations for gravity and jump velocity
        gravity = -2 * apexHeight / Mathf.Pow(apexHeight, 2);
        jumpVelocity = 2 * apexHeight / apexTime;
        position = transform.position;
        dashBuildUp = 2;


    }

    // Update is called once per frame
    void Update()
    {
        // The input from the player needs to be determined and
        // then passed in the to the MovementUpdate which should
        // manage the actual movement of the character.
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal") * speed, gravity);
       
        MovementUpdate(playerInput);
        playerMovement = playerInput;


        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (isJumping == true)
        {
            currentTime += Time.deltaTime;
            dashBuildUp += Time.deltaTime;
        }
        if (isJumping == false)
        {
            currentTime = 0;
            
        }

        dashBuildUp += Time.deltaTime;
        if(dashBuildUp >= 2)
        {
            dashBuildUp = 2;
        }
    }

    public void MovementUpdate(Vector2 playerInput)
    {

        //  the equation for velocity and position
        velocity = gravity * Mathf.Pow(currentTime, 2) + jumpVelocity;
        position = 0.5f * gravity * Mathf.Pow(currentTime, 2) * jumpVelocity * currentTime * position;


        //  if the spacebar is pressed then the player is jumping, isJumping = true
        if (coyoteTimeCounter > 0 && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashBuildUp >= 2)
        {
            Debug.Log("is dashing");
            isDashing = true;
        }


    }

    private void FixedUpdate()
    {
        //  the movement of the player
        rb.AddForce(playerMovement * speed);


        // if isJumping is true then...
        if (isJumping == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, velocity); //  the rigidbody's y value is the velocity
        }

        //  if thhe y value is greater or equal to the apexHeight   or     the currentTime is greater or equal to apexTime...
        if (rb.velocity.y >= apexHeight | currentTime >= apexTime)
        {
            isJumping = false;  //  isJumping = false  turning on gravity
            coyoteTimeCounter = 0;
            currentTime = 0;    //  currentTime resets
        }

        //  if the y value is less than or equal to terminalSpeed
        if (rb.velocity.y <= terminalSpeed)
        {

            rb.velocity = new Vector2(rb.velocity.x, terminalSpeed);  //  the rb's y value is the terminal speed, capping the falling speed
        }
        Debug.Log(rb.velocity.y);

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);


        //  if isDashing is true then...
        if(isDashing == true)
        {
            rb.velocity = new Vector2(rb.velocity.x * speed * dashingSpeed, rb.velocity.y);  //  add to the X value of the player to dash forward
;           isDashing = false; //  set isDashing to false
            dashBuildUp = 0;  // the time to dash is reset
        }


    }



    public bool IsWalking()
    {
        return false;

    }
    public bool IsGrounded()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, 1, LayerMask.GetMask("Ground")))
        {
            Debug.DrawRay(transform.position, Vector2.down * 1, Color.blue);
            //  Debug.Log("Player is on the ground");
            return true;

        }
        else
        {
            Debug.DrawRay(transform.position, Vector2.down * 1, Color.red);
            //  Debug.Log("Player is not on the ground");
            return false;
        }

    }

    public FacingDirection GetFacingDirection()
    {
        if (rb.velocity.x > 0)  //  if the rb value is less than zero...
        {
            directionOfPlayer = FacingDirection.right;
        }
        else if (rb.velocity.x < 0)  //  if the rb value is greater than zero...
        {
            directionOfPlayer = FacingDirection.left;
        }

        return directionOfPlayer;

    }
}
