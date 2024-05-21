using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CharacterMovement : MonoBehaviour
{
    //Scene components
    public Animator animator;
    CharacterController controller;
    AudioSource audioSource;

    //Movement variables
    float swipeSpeed = 0.5f;
    public float speed = 5f;
    public float rotationSpeed = 90;
    public float jumpHeight = 5;
    private float gravity = -20f;
    bool jumping;
    private float JumpStartTime = 0f;
    Vector3 movement;
    Vector3 prevPos;
    Vector3 StartPos;
    Vector3 pos;
    Vector3 turn;
    Quaternion turnRotation;
    [SerializeField] private float CheckPointFrequency = 3f;

    //Mobile Variables
    private Touch touch;
    private Vector2 touchStartPosition, touchEndPosition;

    //Player variables
    public int TempCoinCount = 0;
    public int coinGain = 1;
    public ParticleSystem particleSystem;

    //UI elements
    public int score;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverTextObject;
    public bool gameOver { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        gameOverTextObject.SetActive(false);
        score = 0;
        SetScore();
        audioSource = GetComponent<AudioSource>();
        StartPos = transform.position;
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
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

            //Check to see they have stopped working
            
                //controller.enabled = false;
        }
        else
        {
            animator.SetBool("running", false);
        }
    }

    void HandlePCInput()
    {
        //Get user input
        var hInput = Input.GetAxis("Horizontal");
        var vInput = Input.GetAxis("Vertical");

        if (controller.isGrounded)
        {

            //Update their animation
            animator.SetBool("running", IsRunning(movement));
            //jumping = false;
            //Handle movement
            movement = transform.forward * speed;
            turn = transform.up * rotationSpeed * hInput;

            //Handle jumping
            if (Input.GetButtonDown("Jump"))
            {
                StartCoroutine(Jump());
            }
        }

        //Apply gravity
        movement.y += gravity * Time.deltaTime;
        //movement.z = 5f;

        //Move the character
        controller.Move(movement * Time.deltaTime);
        transform.Rotate(turn * Time.deltaTime);
    }

    void HandleMobileInput()
    {
        float rotationAmount = 0f;
        if (controller.isGrounded)
        {
            animator.SetBool("running", IsRunning(movement));
            movement = transform.forward * speed;
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    touchStartPosition = touch.position;

                }
                else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Ended)
                {

                    touchEndPosition = touch.position;
                    float x = touchEndPosition.x - touchStartPosition.x;
                    float y = touchEndPosition.y - touchStartPosition.y;

                    if (Mathf.Abs(y) < 1 && Mathf.Abs(x) < 1)
                    {
                        StartCoroutine(Jump());
                    }
                    else
                    {
                        float swipeDelta = touch.deltaPosition.x * swipeSpeed;
                        float horizontalInput = Mathf.Sign(swipeDelta);
                        rotationAmount = rotationSpeed * horizontalInput;
                        //float rotationAmount = rotationSpeed * horizontalInput * Time.deltaTime;
                        //turn = transform.up * rotationSpeed * horizontalInput;
                    }
                    //transform.Rotate(turn * Time.deltaTime);
                }
            }
        }

        turn = transform.up * rotationAmount;
        movement.y += gravity * Time.deltaTime;

        controller.Move(movement * Time.deltaTime);
        transform.Rotate(turn * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            AudioClip coinAudio = GetAudio("AudioFiles/Retro_Game_Coin_Sound_Effect");
            audioSource.clip = coinAudio;
            audioSource.Play();
            score+= coinGain;
            other.gameObject.SetActive(false);
            if (particleSystem != null)
            {
                particleSystem.Play();
            }
            TempCoinCount+=coinGain;
            SetScore();
        }
    }

    bool IsRunning(Vector3 movement)
    {
        if ((movement.x > 0 || movement.z > 0) && !jumping)
        {
            return true;
        }
        else return false;
    }

    public IEnumerator Jump()
    { 
        Debug.Log("Jumping to height of: " + jumpHeight);
        jumping = true;
        movement.y = jumpHeight;
        animator.SetBool("jumping", true);
        //yield return new WaitUntil(() => controller.isGrounded);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("jumping"));
        //yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("running"));
        animator.SetBool("jumping", false);
        jumping = false;
    }

    void SetScore()
    {
        scoreText.SetText("Score: " + score);
    }

    public AudioClip GetAudio(string filePath)
    {
        AudioClip clip = Resources.Load<AudioClip>(filePath);
        return clip;
    }

    public void GameOver()
    {
        gameOver = true;
        gameOverTextObject.SetActive(true);
    }
}
