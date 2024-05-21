using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public interface Movement
{
    CharacterController controller { get; set; }
    public bool gameOver { get; set; }
    Vector3 movement { get; set; }
    float speed { get; set; } 
    Vector3 turn { get; set; }
    float rotationSpeed { get; set; }
    AudioSource audioSource { get; set;}

    //UI elements
    public int score { get; set; }
    public TextMeshProUGUI scoreText { get; set; }
    public GameObject gameOverTextObject { get; set; }

    void HandlePCInput();

    void HandleMobileInput();

    void GameOver();
}
