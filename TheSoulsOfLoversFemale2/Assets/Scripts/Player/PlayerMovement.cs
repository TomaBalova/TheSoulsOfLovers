using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IDataPersistence
{
    public float speed = 3f;

    public Rigidbody2D rb;
    public Animator animator;
    public AudioSource movementSound;

    Vector2 movement;


    private void Update()
    {
        movement = Vector2.zero;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (animator != null)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);

        }
    }


    private void FixedUpdate()
    {
        if (this.gameObject.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static)
        {
            movement.Normalize();
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
            if (movementSound != null)
            {
                if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.35f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.35f)
                {
                    if (movementSound.isPlaying) return;
                    else movementSound.Play();
                }
                else
                {
                    movementSound.Stop();
                }
            }
        }
    }

    public void LoadData(GameData gameData)
    {
        this.transform.position = gameData.playerPos;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.playerPos = this.transform.position;
    }
}
