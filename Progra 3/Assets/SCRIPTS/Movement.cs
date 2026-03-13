using UnityEngine;


/// <summary>
/// Este movimiento va a requerir de un Rigidbody para funcionar
/// </summary>
[RequireComponent(typeof(Rigidbody), typeof(Gravity))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5;

    private Rigidbody rb;

    private InputHandler inputHandler;

    public Joystick Joystick;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        inputHandler = GetComponent<InputHandler>();

    }

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        rb.linearVelocity = new Vector3(Joystick.axis.x,
            0,
            Joystick.axis.y * Time.deltaTime * (walkSpeed * 100));
    }


}