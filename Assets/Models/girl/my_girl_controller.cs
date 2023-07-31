using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class my_girl_controller : MonoBehaviour
{
    public GameObject test_object;

    Animator animator;
    public Camera main_camera;
    Rigidbody rigidbody;
    public GameObject looking_target;
    public GameObject laser_object;

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

       animator = GetComponent<Animator>(); 
       speedFloat = Animator.StringToHash("Speed");

       Cursor.lockState = CursorLockMode.Locked;
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

        Vector3 camera_center_point = main_camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        Vector3 ray_end_point = camera_center_point + (main_camera.transform.forward * 5);

        if ((ray_end_point.y - 1.3f) > transform.position.y) {
            ray_end_point.y = transform.position.y + 1.3f;
        }

        if (on_right_mouse_click_holding == false) {
            looking_target.transform.position = ray_end_point;
            test_object.transform.rotation = main_camera.transform.rotation;
        } else {
            animator.Play("shooting");
            // Always rotates the player according to the camera horizontal rotation in aim mode.
            Quaternion targetRotation =  Quaternion.Euler(0, main_camera.transform.eulerAngles.y, 0);
            transform.rotation = targetRotation;
        }
    }

    void Prone_Management() {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (stand) {
                animator.Play("prone_idle");
                stand = false;
            } else {
                animator.Play("stand_up");
                stand = true;
            }
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (stand == false) {
            if (h == 0 && v == 0) {
                animator.Play("prone_idle");
            } else {
                animator.Play("prone_forward");
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

        animator.SetFloat(speedFloat, speed, speedDampTime, Time.deltaTime);
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

