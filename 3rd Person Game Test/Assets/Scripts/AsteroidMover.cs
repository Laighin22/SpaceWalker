using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMover : MonoBehaviour
{

    public float speed = 5f;

    public Vector3 startPosition;

    public float targetPosition;

    public char direction;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;

        SetPositions();
    }

    // Update is called once per frame
    void Update()
    {

        if (direction == 'L')
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            if (transform.position.x <= targetPosition)
            {
                direction = 'R';
                SetPositions();
            }
        } else
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            if (transform.position.x >= targetPosition)
            {
                direction = 'L';
                SetPositions();
            }
        }
        
    }

    void SetPositions()
    {
        if (direction != 'R')
        {
            direction = 'L';
            targetPosition = startPosition.x - 50;
        }
        else targetPosition = startPosition.x + 50;
    }
}
