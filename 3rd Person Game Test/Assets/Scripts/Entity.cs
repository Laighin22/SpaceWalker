using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int health;
    public ParticleSystem explosion;

    private float coolDownTimer = 0f;

    private bool coolDown = false;

    public void Damage(int damage)
    {

        if (!coolDown)
        {
            Debug.Log("I have been damaged");

            health -= damage;

            if (health <= 0)
            {
                Destroy(gameObject);
            }

            coolDown = true;
            StartCoroutine(DamageCoolDownTimer());
        }
    }

    private IEnumerator DamageCoolDownTimer()
    {
        yield return new WaitForSeconds(0.1f);
        coolDown = false;
    }
}
