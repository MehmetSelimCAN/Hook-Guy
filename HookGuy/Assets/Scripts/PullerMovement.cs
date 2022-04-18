using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PullerMovement : MonoBehaviour {

    private Rigidbody2D rb;
    [HideInInspector] public Vector2 movementDirection;
    private float speed = 10f;

    private bool hitObstacle;

    [HideInInspector] public bool canPull;

    [SerializeField] private GameObject pfRope;
    private GameObject gameRope;
    private SpriteRenderer gameRopeSpriteRenderer;

    private CharacterController2D player;

    private bool rotated;

    private Sprite HookClosed;
    private Sprite HookOpened;

    private float ropeSize;

    private void Awake() {
        HookOpened = Resources.Load<Sprite>("Sprites/HookOpened");
        HookClosed = Resources.Load<Sprite>("Sprites/HookClosed");
    }

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        gameRope = Instantiate(pfRope, transform.position + new Vector3(0f,-0.02f,0f), Quaternion.identity);
        ropeSize = gameRope.GetComponent<Transform>().localScale.x;
        gameRopeSpriteRenderer = gameRope.GetComponent<SpriteRenderer>();
        movementDirection = CharacterController2D.lastDirection;
        rb.velocity = movementDirection * speed;
        CharacterController2D.isPulling = true;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();

        transform.Find("openedHookLight").gameObject.SetActive(false);
    }

    private void Update() {
        RopeMovement();

        if (!(Input.GetKey(KeyCode.X))) {
            CharacterController2D.isPulling = false;
            CharacterController2D.ChangeAnimationState("PlayerIdle");
            Destroy(gameRope);
            Destroy(gameObject);
        }
    }

    public void HitObstacle() {
        movementDirection = -movementDirection;
        SpeedUp();
        hitObstacle = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Wall") || collision.CompareTag("Obstacle") || collision.CompareTag("Power") || collision.CompareTag("LightBulb")) {
            transform.GetComponent<SpriteRenderer>().sprite = HookClosed;
            gameRope.GetComponent<Transform>().Find("light").GetComponent<Light2D>().intensity = 1f;
            SpeedDown();

            if ((collision.CompareTag("Wall") || collision.CompareTag("LightBulb")) && canPull) {
                CharacterController2D.isPulling = false;
                CharacterController2D.ChangeAnimationState("PlayerIdle");
                Destroy(gameRope);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if ((collision.CompareTag("Wall") || collision.CompareTag("Obstacle") || collision.CompareTag("Power") || collision.CompareTag("LightBulb"))) {
            canPull = false;
            transform.GetComponent<SpriteRenderer>().sprite = HookClosed;
            transform.Find("closedHookLight").gameObject.SetActive(true);
            transform.Find("openedHookLight").gameObject.SetActive(false);
            SpeedUp();
            if (!hitObstacle) {
                SpeedDown();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if ((collision.CompareTag("Wall") || collision.CompareTag("Obstacle") || collision.CompareTag("Power") || collision.CompareTag("LightBulb")) && !hitObstacle) {
            SpeedUp();
            canPull = true;
            transform.GetComponent<SpriteRenderer>().sprite = HookOpened;
            transform.Find("closedHookLight").gameObject.SetActive(false);
            transform.Find("openedHookLight").gameObject.SetActive(true);
            SetPullerAngle();
        }
    }

    private void SpeedUp() {
        speed = 10f;
        rb.velocity = movementDirection * speed;
    }

    private void SpeedDown() {
        speed = 5f;
        rb.velocity = movementDirection * speed;

    }

    private void RopeMovement() {
        float distance = Vector2.Distance(transform.position, gameRope.GetComponent<Transform>().position);
        float numberOfRopePiece = distance / ropeSize;

        if (movementDirection == Vector2.left) {
            if (!hitObstacle && !rotated) {
                gameRopeSpriteRenderer.flipX = true;
                rotated = true;
            }
        }
        else if (movementDirection == Vector2.right) {
            if (!hitObstacle && !rotated) {
                gameRopeSpriteRenderer.flipX = false;
                rotated = true;
            }
        }
        else if(movementDirection == Vector2.up) {
            if (!hitObstacle && !rotated) {
                gameRope.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, -90f);

                if (GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().flipX) {
                    transform.position += new Vector3(-0.085f,0f,0f);
                    gameRope.GetComponent<Transform>().position += new Vector3(-0.125f,0f,0f);
                }
                else {
                    transform.position += new Vector3(+0.045f,0f,0f);
                    gameRope.GetComponent<Transform>().position += new Vector3(+0.02f,0f,0f);
                }
                rotated = true;
            }
        }
        else if (movementDirection == Vector2.down) {
            if (!hitObstacle && !rotated) {
                gameRope.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, 90f);
                gameRope.GetComponent<Transform>().position += new Vector3(+0.02f,0f,0f);
                rotated = true;
            }
        }
            gameRope.GetComponent<SpriteRenderer>().size = new Vector2(numberOfRopePiece, 0.5f);
    }

    private void SetPullerAngle() {
        if (movementDirection == Vector2.left)
            transform.rotation = Quaternion.Euler(0, 0, 180f);
        if (movementDirection == Vector2.right)
            transform.rotation = Quaternion.Euler(0, 0, 0f);
        if (movementDirection == Vector2.up)
            transform.rotation = Quaternion.Euler(0, 0, 90f);
        if (movementDirection == Vector2.down)
            transform.rotation = Quaternion.Euler(0, 0, -90f);
    }
}
