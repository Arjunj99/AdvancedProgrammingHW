using System.Collections;
using System.Collections.Generic;
using Obi;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform Camera;

    [Tooltip("Move speed for liquid form, default movement speed")]
    public float MoveSpeed;

    [Tooltip("Move speed for gas form, for floating around")]
    public float GasMoveSpeed;

    [Tooltip("Jump height for liquid form")]
    public float JumpForce;

    [Tooltip("If the player can move")]
    public bool CanMove = true;

    [Header("Control Scheme")]
    public KeyCode GasJumpKey = KeyCode.Space;
    public KeyCode IceSlideKey = KeyCode.LeftShift;

    //Player state change FSM
    private StateChange states;

    //Rigidbody and jumping stuff
    private Rigidbody rb;
    private Vector3 tempVel;
    private RaycastHit hit;
    private bool grounded;
    private bool jump;
    private JumpChecker jumpCheck;
    private ObiSoftbody softBody;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        states = GetComponent<StateChange>();
        rb = GetComponent<Rigidbody>();
        Services.Player = this;
        softBody = GetComponent<ObiSoftbody>();
        softBody.solver.OnCollision += Solver_OnCollision;
    }

    private void OnDestroy()
    {
        softBody.solver.OnCollision -= Solver_OnCollision;
    }

    void Update()
    {
        //get the camera angle and rotate player based on camera position
        float cameraAngle = Camera.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f, cameraAngle, 0f);

        if (Input.GetKeyDown(KeyCode.P))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

    }

    private void FixedUpdate()
    {
        tempVel = Input.GetAxis("Horizontal") * transform.right;
        tempVel += Input.GetAxis("Vertical") * transform.forward;

        //Changes player movement speed based on current state
        if (CanMove && states.PlayerState?.CurrentState?.GetType() == typeof(StateChange.Liquid))
        {
            //rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(tempVel.x * MoveSpeed, rb.velocity.y, tempVel.z * MoveSpeed), 0.25f);
            softBody.AddForce(MoveSpeed * tempVel, ForceMode.Acceleration);
        }
        else if (CanMove && states.PlayerState?.CurrentState?.GetType() == typeof(StateChange.Gas))
        {
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(tempVel.x * GasMoveSpeed, rb.velocity.y, tempVel.z * GasMoveSpeed), 0.25f);
        }
    }

    private void Solver_OnCollision(ObiSolver solver, ObiSolver.ObiCollisionEventArgs e)
    {
        jumpCheck.Grounded = false;

        var world = ObiColliderWorld.GetInstance();
        foreach (Oni.Contact contact in e.contacts)
        {
            // look for actual contacts only:
            if (contact.distance > 0.01)
            {
                var col = world.colliderHandles[contact.bodyB].owner;
                if (col != null)
                {
                    jumpCheck.Grounded = true;
                    return;
                }
            }
        }
    }

    public void Push(float force)
    {
        if (states.CompareState<StateChange.Solid>())
        {
            softBody.AddForce(rb.velocity * force, ForceMode.Acceleration);
        }
    }
}
