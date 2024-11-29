using System.Net.Sockets;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TextCore;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    Vector2 playerMovement;


    FacingDirection directionOfPlayer = FacingDirection.left;

    public float apexHeight;
    public float apexTime;

    float gravity;
    float jumpVelocity;
    float velocity;
    public float currentTime;
    Vector2 position;

    bool isJumping;

    public float terminalSpeed;

    public enum FacingDirection
    {
        left, right
    }

    // Start is called before the first frame update
    void Start()
    {
        rb.GetComponent<Rigidbody2D>();


        gravity = -2 * apexHeight / Mathf.Pow(apexHeight, 2);
        jumpVelocity = 2 * apexHeight / apexTime;
        position = transform.position;



    }

    // Update is called once per frame
    void Update()
    {
        // The input from the player needs to be determined and
        // then passed in the to the MovementUpdate which should
        // manage the actual movement of the character.
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal") * speed, gravity);
        MovementUpdate(playerInput);

    }

    public void MovementUpdate(Vector2 playerInput)
    {
        playerMovement = playerInput;
        rb.AddForce(playerInput);
        velocity = gravity * Mathf.Pow(currentTime, 2) + jumpVelocity;
        position = 0.5f * gravity * Mathf.Pow(currentTime, 2) * jumpVelocity * currentTime * position;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;

        }
    }

    private void FixedUpdate()
    {
        if (isJumping == true)
        {
            rb.velocity = new Vector2(0, position.y + velocity);
            currentTime += Time.deltaTime;
        }
        if (velocity > apexHeight | currentTime > apexTime)
        {
            isJumping = false;
            rb.velocity = new Vector2(position.x, position.y - terminalSpeed);
            currentTime = 0;

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
            Debug.Log("Player is on the ground");
            return true;

        }
        else
        {
            Debug.DrawRay(transform.position, Vector2.down * 1, Color.red);
            Debug.Log("Player is not on the ground");
            return false;
        }


    }

    public FacingDirection GetFacingDirection()
    {
        if (playerMovement.x > 0)
        {
            directionOfPlayer = FacingDirection.right;
        }
        else if (playerMovement.x < 0)
        {
            directionOfPlayer = FacingDirection.left;
        }

        return directionOfPlayer;

    }
}
