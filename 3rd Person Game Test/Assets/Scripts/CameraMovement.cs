using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    private Vector3 minDistance;
    private CharacterMovement playerScript;
    private float startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.y;
        if (player!=null)
        {
            playerScript = player.GetComponent<CharacterMovement>();
            minDistance = player.position - transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript!=null)
        {
            if (!playerScript.gameOver)
            {
                transform.position = player.position - minDistance;
            } else
            {
                float newY = Mathf.Lerp(transform.position.y, startPos, Time.deltaTime * 2.5f);
                Vector3 newPos = new Vector3(transform.position.x, newY, transform.position.z);
                transform.position = newPos;
            }
        }
    }
}
