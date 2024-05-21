using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    CharacterController controller;
    public bool gameOver;
    Vector3 movement;
    float speed = 10f;
    AudioSource audioSource;
    public Transform ship;
    public ParticleSystem explosion;
    AudioClip explosionAudio;

    //Mobile Variables
    private Touch touch;
    private Vector2 touchStartPosition, touchEndPosition;
    float swipeSpeed = 0.5f;

    //UI elements
    public int score;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverTextObject;

    public bool gameFinished = false;

    public ShipGuns gun1;
    public ShipGuns gun2;

    public Button shootButton;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        gameOver = false;
        gameOverTextObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        //StartPos = transform.position;
        gameOver = false;
        explosionAudio = Resources.Load<AudioClip>("AudioFiles/Explosion sound effect");
        
        if (!Application.isMobilePlatform)
        {
            shootButton.interactable = false;
            shootButton.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameFinished)
        {
            Debug.Log("Game is finished");
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        if (controller.enabled)
        {
            if (!gameOver)
            {
                if (Application.isMobilePlatform)
                {
                    HandleMobileInput();
                }
                else
                {
                    HandlePCInput();
                }
            }
        }   
    }

    void HandlePCInput()
    {
        var hInput = Input.GetAxis("Horizontal");
        movement = transform.forward * speed;
        var strafe = transform.right * speed * hInput;
        //turn = transform.up * rotationSpeed * hInput;
        movement += strafe;
        controller.Move(movement * Time.deltaTime);
        //transform.Rotate(turn * Time.deltaTime);

        float rollAngle = -hInput * 30;

        ship.rotation = Quaternion.Euler(0f, 0f, rollAngle);
    }

    void HandleMobileInput()
    {
        float rollAngle = 0;
        movement = transform.forward * speed;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (IsTouchingShootButton(touch.position))
            {
                return;
            }
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Ended)
            {

                touchEndPosition = touch.position;
                float x = touchEndPosition.x - touchStartPosition.x;
                float y = touchEndPosition.y - touchStartPosition.y;
                if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    float horizontalInput = Mathf.Sign(x);
                    movement += transform.right * horizontalInput * speed;

                    rollAngle = -horizontalInput * 30;
                }
            }
        }

        ship.rotation = Quaternion.Euler(0f, 0f, rollAngle);
        controller.Move(movement * Time.deltaTime);
    }

    public bool IsTouchingShootButton(Vector2 vector)
    {
        var corners = new Vector3[4];
        shootButton.gameObject.GetComponent<RectTransform>().GetWorldCorners(corners);

        Vector3 touchWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(vector.x, vector.y, Camera.main.nearClipPlane));

        if (touchWorldPosition.x >= corners[0].x && touchWorldPosition.x <= corners[1].x && touchWorldPosition.y >= corners[0].y && touchWorldPosition.y <= corners[1].y)
        {
            return true;
        }
        else return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Asteroid"))
        {
            Debug.Log("Player hit an asteroid.");
            controller.enabled = false;
            StartCoroutine(DestroyShip());
        }
        Debug.Log("Hit something");

    }

    public void GameOver()
    {
        gameOver = true;
        gameOverTextObject.SetActive(true);
    }

    public IEnumerator DestroyShip()
    {
        explosion.Play();
        audioSource.clip = explosionAudio;
        audioSource.Play();
        ship.gameObject.SetActive(false);
        yield return new WaitUntil(()=> explosion.isStopped);
        GameOver();
    }


}
