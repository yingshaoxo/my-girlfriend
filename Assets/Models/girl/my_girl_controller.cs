using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class my_girl_controller : MonoBehaviour
{
    Animator animator;
    public Camera main_camera;
    Rigidbody rigidbody;
    public GameObject looking_target;
    public GameObject laser_object;

    private RaycastHit hit_point;
    public float gun_range = 1000f;

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
        move_looking_target();

        rotate_body_by_using_mouse();

        move_body_by_using_keyboard();

        jump_management();

        prone_management();
    }

    void move_looking_target() {
        Vector3 camera_center_point = main_camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        Vector3 ray_start_point = camera_center_point + (main_camera.transform.forward * 5);
        if(Physics.Raycast(ray_start_point, main_camera.transform.forward, out hit_point, gun_range))
        {
            // laserLine.SetPosition(1, hit_point.point);
        }
        else
        {
            hit_point.point = ray_start_point + (main_camera.transform.forward * gun_range);
        }
        looking_target.transform.position = hit_point.point;
    }

    bool is_on_ground()
    {
        float _distanceToTheGround = GetComponent<Collider>().bounds.extents.y;
        return Physics.Raycast(transform.position, Vector3.down, _distanceToTheGround + 0.1f);
    }

    void jump_management() {
        if (Input.GetButton("Jump") && is_on_ground())
        {
            animator.Play("jump_start");
        }
    }

    void rotate_body_by_using_mouse() {
        if (Input.GetMouseButtonDown(1)) {
            on_right_mouse_click_holding = true;
        }
        if (Input.GetMouseButtonUp(1)) {
            on_right_mouse_click_holding = false;
        }

        if (on_right_mouse_click_holding == false) {
        } else {
            animator.Play("shooting");
        }

        Quaternion targetRotation =  Quaternion.Euler(0, main_camera.transform.eulerAngles.y, 0);
        transform.rotation = targetRotation;
    }

    void move_body_by_using_keyboard() {
        if (stand == true) {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (!(h == 0 && v == 0)) {
                move_management_for_standing(h, v);
            }
        }
    }

    void prone_management() {
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
            looking_target.transform.position = new Vector3(looking_target.transform.position.x, transform.position.y - 50, looking_target.transform.position.z);

            if (h == 0 && v == 0) {
                animator.Play("prone_idle");
            } else {
                animator.Play("prone_forward");
                move_management_for_prone(h, v);
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

    void move_management_for_standing(float horizontal, float vertical)
    {
        float move_delta = 0.28f;

        if (horizontal > 0) {
            transform.Translate(move_delta,0,0);
        } else if (horizontal < 0) {
            transform.Translate(-move_delta,0,0);
        }

        if (vertical > 0) {
            // forward
            move_delta = 0.15f;
            transform.Translate(0,0,move_delta);
        } else if (vertical < 0) {
            // backward
            move_delta = 0.4f;
            transform.Translate(0,0,-move_delta);
        }

        // if (inputDirection != Vector2.zero) // otherwise it snaps back to forward without input
        // {
        //     // using the rotation of the camera as well as our input axis to determine target rotation
        //     float targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + main_camera.transform.eulerAngles.y;
        //     transform.eulerAngles = Vector3.up * targetRotation;
        // }
    }


    void move_management_for_prone(float horizontal, float vertical)
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

