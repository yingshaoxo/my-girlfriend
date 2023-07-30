using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class my_girl_controller : MonoBehaviour
{
    public GameObject test_object;

    Animator anim;
    public Camera main_camera;
    Rigidbody rigidbody;

    bool stand = true;
    bool on_right_mouse_click_holding = false;

	public float turnSmoothing = 0.06f;                   // Speed of turn when moving to match camera facing.
	public float walkSpeed = 0.15f;                 // Default walk speed.
	public float runSpeed = 1.0f;                   // Default run speed.
	public float sprintSpeed = 2.0f;                // Default sprint speed.
	public float speedDampTime = 0.1f;              // Default damp time to change the animations based on current speed.
	protected int speedFloat;                      // Speed parameter on the Animator.

	private float speed, speedSeeker;               // Moving speed.

    public float mouse_sensitivity = 30f;
    void Start()
    {
       rigidbody = gameObject.GetComponent<Rigidbody>();

       anim = GetComponent<Animator>(); 
       speedFloat = Animator.StringToHash("Speed");

    //    Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        rotate_body_by_using_mouse();
        Prone_Management();
    }

    void rotate_body_by_using_mouse() {
        if (Input.GetMouseButtonDown(1)) {
            on_right_mouse_click_holding = true;
        }
        if (Input.GetMouseButtonUp(1)) {
            on_right_mouse_click_holding = false;
        }


        if (on_right_mouse_click_holding) {
            // when user hold right mouse key, we use mouse to lead the character
            transform.rotation = main_camera.transform.rotation;
            // transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, main_camera.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
    }

    void Prone_Management() {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (stand) {
                anim.Play("prone_idle");
                stand = false;
            } else {
                anim.Play("stand_up");
                stand = true;
            }
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (stand == false) {
            if (h == 0 && v == 0) {
                anim.Play("prone_idle");
            } else {
                anim.Play("prone_forward");
                MovementManagement(h, v);
            }
        }

        // if (Input.GetKeyDown(KeyCode.J)) {
        //     anim.Play("death");
        //     anim.Play("shooting");
        // }
        // else if (Input.GetKeyDown(KeyCode.K)) {
        //     anim.Play("prone_forward");
        // }
    }

    void MovementManagement(float horizontal, float vertical)
    {
        // Call function that deals with player orientation.
        Rotating(horizontal, vertical);

        // Set proper speed.
        Vector2 dir = new Vector2(horizontal, vertical);
        speed = Vector2.ClampMagnitude(dir, 1f).magnitude;
        // This is for PC only, gamepads control speed via analog stick.
        speedSeeker += Input.GetAxis("Mouse ScrollWheel");
        speedSeeker = Mathf.Clamp(speedSeeker, walkSpeed, runSpeed);
        speed *= speedSeeker;

        anim.SetFloat(speedFloat, speed, speedDampTime, Time.deltaTime);
    }

	// Rotate the player to match correct orientation, according to camera and key pressed.
	Vector3 Rotating(float horizontal, float vertical)
	{
		// Get camera forward direction, without vertical component.
		Vector3 forward = main_camera.transform.TransformDirection(Vector3.forward);

		// Player is moving on ground, Y component of camera facing is not relevant.
		forward.y = 0.0f;
		forward = forward.normalized;

		// Calculate target direction based on camera forward and direction key.
		Vector3 right = new Vector3(forward.z, 0, -forward.x);
		Vector3 targetDirection;
		targetDirection = forward * vertical + right * horizontal;

		// Lerp current direction to calculated target direction.
		if ((targetDirection != Vector3.zero))
		{
			Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

			Quaternion newRotation = Quaternion.Slerp(rigidbody.rotation, targetRotation, turnSmoothing);
			rigidbody.MoveRotation(newRotation);
            rigidbody.AddForce(targetDirection*5000);
		}

		return targetDirection;
	}
}

