using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Net;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    private int count;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    // added vars for jump info
    private float jumpStrength = 6f;
    public bool isGrounded;
    public int jumpCharges;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
    }
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
            jumpCharges = 1;
        }
    }
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }
    void OnJump()
    {
        if (!isGrounded)
        {
            // check for if we have a jump charge
            if (jumpCharges > 0)
            {
                // We have an air jump allowed, use it and continue
                jumpCharges = jumpCharges - 1;
            }
            else
            {
                // we aren't on the ground and we don't have a jump charge, can't jump.
                return;
            }
        }
        // Add double jump check here
        Vector3 velocity = rb.linearVelocity;
        velocity.y = 0.0f;
        rb.linearVelocity = velocity;
        rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            // we're on the ground, allow a first jump and reset double jump
            isGrounded = true;
            jumpCharges = 1;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            // we left the ground, no longer grounded
            isGrounded = false;
        }
    }
    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count > 7)
        {
            winTextObject.SetActive(true);
        }
    }
}
