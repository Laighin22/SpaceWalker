using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{

    public Rigidbody rb;
    public ParticleSystem explosion;
    public LayerMask whatisEnemies;

    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;

    public int explosionDamage;
    public float explosionRange;

    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;

    private int collisions;
    PhysicMaterial physics_mat;

    private bool hasExploded = false;

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        if (collisions>maxCollisions || maxLifetime <=0)
        {
            Explode();
        }

        maxLifetime -= Time.deltaTime;
    }

    private void Explode()
    {

        Collider[] asteroids = Physics.OverlapSphere(transform.position, explosionRange, whatisEnemies);

        for (int i=0; i<asteroids.Length; i++)
        {
            Debug.Log("Damaging asteroid");
            if (asteroids[i].GetComponent<Entity>() != null)
            {
                asteroids[i].GetComponent<Entity>().Damage(explosionDamage);
            }
            explosion.Play();
        }

        Invoke("Delay", 0.05f);

    }

    private void Delay()
    {
        Destroy(gameObject);
    }

    private void Setup()
    {
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;

        GetComponent<SphereCollider>().material = physics_mat;
        rb.useGravity = useGravity;

        explosion = gameObject.GetComponentInChildren<ParticleSystem>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        collisions++;

        if (collision.collider.CompareTag("Asteroid") && explodeOnTouch)
        {
            Explode();
        }
    }
}
