using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCameraMovement : MonoBehaviour
{
    public Transform player;
    private Vector3 minDistance;
    private ShipMovement shipScript;
    private float startPos;
    private AudioSource audioSource;
    private AudioClip backgroundMusic;

    public bool finalLevel;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.y;
        audioSource = GetComponent<AudioSource>();
        if (player != null)
        {
            shipScript = player.GetComponent<ShipMovement>();
            minDistance = player.position - transform.position;
        }

        if (finalLevel)
        {
            backgroundMusic = Resources.Load<AudioClip>("Music/Tronicles SciFi - Free Music  [Royalty Free No Copyright]");
            audioSource.clip = backgroundMusic;
            audioSource.Play();
            audioSource.loop = true;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (shipScript != null)
        {
            if (!shipScript.gameOver)
            {
                transform.position = player.position - minDistance;
            }
            else
            {
                float newY = Mathf.Lerp(transform.position.y, startPos, Time.deltaTime * 2.5f);
                Vector3 newPos = new Vector3(transform.position.x, newY, transform.position.z);
                transform.position = newPos;
            }
        }
    }
}
