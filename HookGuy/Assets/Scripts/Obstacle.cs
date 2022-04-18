using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 movementDirection;

    private float roundedPosX;
    private float roundedPosY;

    private bool isPulling;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        PullerMovement puller = collision.GetComponent<PullerMovement>();

        if (collision.CompareTag("Puller") && puller.canPull) {
            movementDirection = -puller.movementDirection;
            puller.HitObstacle();
            rb.velocity = movementDirection * 10f;
            isPulling = true;
        }
        else {
            if (!(collision.CompareTag("Trap") || collision.CompareTag("Door") || collision.CompareTag("Collider") || collision.CompareTag("Puller"))) {
                rb.velocity = Vector2.zero;

                if (isPulling) {
                    CharacterController2D.isPulling = false;
                    Destroy(GameObject.FindGameObjectWithTag("Rope"));
                    Destroy(GameObject.FindGameObjectWithTag("Puller"));
                }

                isPulling = false;

                #region Place exactly
                if (movementDirection == Vector2.left || movementDirection == Vector2.right) {
                    roundedPosX = Mathf.Round(transform.position.x);
                    transform.position = new Vector3(roundedPosX, transform.position.y, 0f);

                }
                else if (movementDirection == Vector2.up || movementDirection == Vector2.down) {
                    roundedPosY = Mathf.Round(transform.position.y);
                    transform.position = new Vector3(transform.position.x, roundedPosY, 0f);
                }
                    #endregion
            }

        }
    }
}
