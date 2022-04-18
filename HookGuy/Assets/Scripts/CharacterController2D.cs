using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class CharacterController2D : MonoBehaviour {
	[SerializeField] private float jumpForce = 15f;                          // Amount of force added when the player jumps.
	[Range(0, 1f)] [SerializeField] private float movementSmoothing = .05f;  // How much to smooth out the movement

	public static bool isGrounded;            // Whether or not the player is grounded.
	[HideInInspector] public Rigidbody2D rb;
	private Vector3 rb_velocity = Vector3.zero;

	private float horizontalMove = 0f;
	private float speed = 40f;

	public static Vector2 lastDirection;
	private bool directionRight = true;

	public static bool isPulling;

	private float fJumpPressedRemember = 0;
	private float fJumpPressedRememberTime = 0.2f;

	private float fGroundedRemember = 0;
	private float fGroundedRememberTime = 0.1f;

	private Transform pfPuller;

	[SerializeField] private LayerMask obstacleLayer;


	private static Animator animator;
    private static string currentAnimationState;
    private string PLAYER_IDLE = "PlayerIdle";
	private string PLAYER_RUN = "PlayerRun";
    private string PLAYER_JUMP = "PlayerJump";
    private string PLAYER_PULLSTART = "PlayerPullStart";
    private string PLAYER_DOWNPULLSTART = "PlayerDownPullStart";
    private string PLAYER_UPPULLSTART = "PlayerUpPullStart";


	private void Awake() {
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		lastDirection = Vector2.right;
		pfPuller = Resources.Load<Transform>("Prefabs/pfPuller");
	}

    private void Update() {
		if (!isPulling) {
			horizontalMove = Input.GetAxisRaw("Horizontal") * speed;


			#region Jump
			fGroundedRemember -= Time.deltaTime;
			if (isGrounded) {
				fGroundedRemember = fGroundedRememberTime;
			}

			fJumpPressedRemember -= Time.deltaTime;
			if (Input.GetKeyDown(KeyCode.Space)) {
				fJumpPressedRemember = fJumpPressedRememberTime;
			}
			
			if(Input.GetKeyUp(KeyCode.Space)) {
				if (rb.velocity.y > 0) {
					rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
				}
			}

			if ((fJumpPressedRemember > 0) && (fGroundedRemember > 0) ) {
				fJumpPressedRemember = 0;
				fGroundedRemember = 0;
				rb.velocity = new Vector2(rb.velocity.x, jumpForce);
			}
            #endregion

            #region Player's look position
            if (horizontalMove == 0f) {
				Stop();
			}

			if (Input.GetKeyDown(KeyCode.D)) {
				LookRight();
			}

			else if (Input.GetKeyDown(KeyCode.A)) {
				LookLeft();
			}

			else if (Input.GetKey(KeyCode.W)) {
				LookUp();
			}
			else if (Input.GetKey(KeyCode.S)) {
				LookDown();
			}

            #endregion

        }


        if (Input.GetKeyDown(KeyCode.X) && isGrounded && !isPulling && !GameManager.died) {
			Instantiate(pfPuller, transform.position + new Vector3(0f,-0.1f,0f), Quaternion.identity);
		}
	}

	private void FixedUpdate() {
		Vector3 feetPosition = transform.GetChild(0).transform.position;
		Vector2 groundCheckSize = new Vector2(0.25f, 0.05f);
		isGrounded = Physics2D.OverlapBox(feetPosition, groundCheckSize, 0, obstacleLayer);

		if (!isGrounded && !GameManager.died) {
			ChangeAnimationState(PLAYER_JUMP);
        }

		#region PullingCheck
	if (!GameManager.died) {
		if (isPulling) {
			if (isGrounded) {

					if (lastDirection == Vector2.up) {
						ChangeAnimationState(PLAYER_UPPULLSTART);
                    }
					else if (lastDirection == Vector2.down) {
						ChangeAnimationState(PLAYER_DOWNPULLSTART);
                    }
					else {
						ChangeAnimationState(PLAYER_PULLSTART);
                    }
            }
			rb.velocity = Vector2.zero;
		}
		else {
			Move(horizontalMove * Time.fixedDeltaTime);

			if (isGrounded) {
				if (horizontalMove == 0 && !GameManager.died) {
					ChangeAnimationState(PLAYER_IDLE);
				}
				if (horizontalMove != 0) {
					ChangeAnimationState(PLAYER_RUN);
				}
             }
		}
    }
		#endregion

	}

	public void Move(float move) {
		rb.velocity = new Vector2(move * 10f, rb.velocity.y);

		if (isGrounded) {
			Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);
			rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref rb_velocity, movementSmoothing);
		}
	}

    #region Player's look position functions
    public void LookLeft() {
		GetComponent<SpriteRenderer>().flipX = true;
		lastDirection = Vector2.left;
		directionRight = false;
    }

	public void LookRight() {
		GetComponent<SpriteRenderer>().flipX = false;
		lastDirection = Vector2.right;
		directionRight = true;
    }

	public void LookUp() {
		lastDirection = Vector2.up;
	}

	public void LookDown() {
		lastDirection = Vector2.down;
	}

	public void Stop() {
		if (directionRight) {
			lastDirection = Vector2.right;
        }
		else {
			lastDirection = Vector2.left;
        }
    }
	#endregion


	public static void ChangeAnimationState(string newState) {
        if (currentAnimationState == newState)
            return;

        animator.Play(newState);
        currentAnimationState = newState;
    }
}
