using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatus : MonoBehaviour
{

    private Vector3 PreviousPosition = Vector3.zero;
    private PlayerInfo playerInfo;
    private float CheckPositionFrequency = 0.5f;
    private Coroutine checkPosition;
    private bool gameOver = false;

    private CharacterMovement PlayerMovement;
    private ShipMovement ShipMovement;
    private bool ExtraLife = false;
    private CharacterController PlayerController;

    private Coroutine checkPointCoroutine;
    [SerializeField] private float CheckPointFrequency = 3f;
    public Vector3 CheckPoint { get; private set; } = Vector3.zero;

    public float airTime = 0.5f;
    private bool fallen = false;
    private Coroutine falling = null;

    public bool FinalLevel;

    // Start is called before the first frame update
    void Start()
    {
        //checkPointCoroutine = StartCoroutine(CheckPointCoroutine());
        //CheckPoint = transform.position;

        PreviousPosition = transform.position;
        playerInfo = GetComponent<PlayerInfo>();
        if (FinalLevel)
        {
            ShipMovement = GetComponent<ShipMovement>();
        } else PlayerMovement = GetComponent<CharacterMovement>();
        PlayerController = GetComponent<CharacterController>();
        checkPosition = StartCoroutine(CheckPosition());
    }

    private IEnumerator CheckPosition()
    {
        if (PlayerController.enabled)
        {
            float ElapsedTime = 0f;

            while (ElapsedTime < CheckPositionFrequency)
            {
                ElapsedTime += Time.deltaTime;
                yield return null;
            }

            if ((Mathf.Abs(transform.position.z - PreviousPosition.z) < 1f) || transform.position.y<0)
            {
                GameOver();
            }
            else
            {
                PreviousPosition = transform.position;
                checkPosition = StartCoroutine(CheckPosition());
            }
        }
    }

    private IEnumerator Falling()
    {
        yield return new WaitForSeconds(airTime);
        fallen = true;
    }

    private IEnumerator CheckPointCoroutine()
    {
        float ElapsedTime = 0f;
        while(ElapsedTime < CheckPointFrequency)
        {
            ElapsedTime += Time.deltaTime;
            yield return null;
        }

        if (PlayerController.isGrounded)
        {
            CheckPoint = transform.position;
        }
    }

    private void GameOver()
    {
        //Debug.Log("Player's position when Game Over initiated: " + transform.position);
        //if (playerInfo.HasExtraLife() && !ExtraLife)
        //{
        //    Debug.Log("Used extra life");
        //    Debug.Log("Moving to CheckPoint at: " + CheckPoint);
        //    transform.position = CheckPoint;
        //    PreviousPosition = transform.position;
        //    ExtraLife = true;
        //}
        //else
        //{
        //}

        if (FinalLevel)
        {
            ShipMovement.GameOver();
        } else PlayerMovement.GameOver();
    }
}
