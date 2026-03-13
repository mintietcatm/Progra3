using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 9f;
    public float groundDrag = 5f;
    public float airMultiplier = 0.5f;

    public float jumpForce = 7f;
    public float jumpCoolDown = 0.25f;
    public KeyCode jumpKey = KeyCode.Space;

    public float playerHeight = 2f;
    public LayerMask whatIsGround;
    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;
    private Rigidbody rb;

    private bool grounded;
    private bool readyToJump = true;

    private float baseJumpForce;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        baseJumpForce = jumpForce;
    }

    void Update()
    {

        grounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            playerHeight * 0.5f + 0.2f,
            whatIsGround
        );

        GetInput();
        SpeedControl();


        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCoolDown);
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");   
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput
                      + orientation.right * horizontalInput;

        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    public void MultiplyJump(float multiplier)
    {
        jumpForce = baseJumpForce * multiplier;
    }

    public void ResetJumpForce()
    {
        jumpForce = baseJumpForce;
    }
    public void StartJumpBuff(float multiplier, float duration)
    {
        StopAllCoroutines(); 
        StartCoroutine(JumpBuffRoutine(multiplier, duration));
    }

    private IEnumerator JumpBuffRoutine(float multiplier, float duration)
    {
        jumpForce = baseJumpForce * multiplier;
        yield return new WaitForSeconds(duration);
        jumpForce = baseJumpForce;
    }

}