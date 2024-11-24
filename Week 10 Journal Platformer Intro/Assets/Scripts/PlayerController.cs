using System.Net.Sockets;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public SpriteRenderer spriteRenderer;




    public enum FacingDirection
    {
        left, right
    }

    // Start is called before the first frame update
    void Start()
    {
        rb.GetComponent<Rigidbody2D>();
        spriteRenderer.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // The input from the player needs to be determined and
        // then passed in the to the MovementUpdate which should
        // manage the actual movement of the character.
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        MovementUpdate(playerInput);

    }

    private void MovementUpdate(Vector2 playerInput)
    {
        rb.AddForce(playerInput * speed * Time.deltaTime);
        spriteRenderer.flipX = playerInput.x > 0;
        


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
        if (spriteRenderer.flipX == true)
        {

            return FacingDirection.right;
        }


        return FacingDirection.left;



    }
}
