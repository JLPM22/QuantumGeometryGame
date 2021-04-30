using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 1.0f;
    public float JumpForce = 1.0f;
    public float RotationSpeed = 1.0f;
    public Transform GroundCheck;
    public Transform CubeVisual;
    public ParticleSystem GroundParticles;
    public float TimeBetweenParticles = 0.1f;
    public ParticleSystem DeadParticles;
    public StressReceiver CameraStress;

    private int EnemyLayer;
    private Rigidbody2D Rigidbody2D;

    private bool Jump;
    private bool Dead;
    private float LastTimeParticle;
    private bool WasGround;
    private int MapIndex;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        Rigidbody2D = GetComponent<Rigidbody2D>();

        EnemyLayer = LayerMask.NameToLayer("Enemy");
    }

    private void Update()
    {
        if (Dead) return;

        bool ground = Physics2D.Raycast(GroundCheck.transform.position, Vector2.down, 0.05f);

        if (ground)
        {
            Quaternion rot = CubeVisual.rotation;
            rot.z = 0.0f;
            CubeVisual.rotation = rot;

            if (LastTimeParticle + TimeBetweenParticles < Time.time)
            {
                GroundParticles.Emit(1);
                LastTimeParticle = Time.time;
            }

            if (!WasGround)
            {
                CameraStress.InduceStress(0.25f);
                PostprocessManager.Instance.ChangeColor();
            }
        }
        else
        {
            CubeVisual.Rotate(Vector3.back * RotationSpeed * Time.deltaTime);
        }

        Jump = ground && (Jump || Input.GetKey(KeyCode.Space));
        WasGround = ground;
    }

    private void FixedUpdate()
    {
        if (Dead)
        {
            Rigidbody2D.velocity = Vector2.zero;
            return;
        }

        Vector2 velocity = Rigidbody2D.velocity;
        velocity.x = Speed * Time.fixedDeltaTime;
        Rigidbody2D.velocity = velocity;

        if (Jump)
        {
            Rigidbody2D.AddForce(Vector3.up * JumpForce * Rigidbody2D.mass, ForceMode2D.Impulse);
            Jump = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == EnemyLayer)
        {
            Die();
        }
    }

    private void Die()
    {
        Dead = true;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        DeadParticles.Play();
        CameraStress.InduceStress(1.0f);
        GameManager.Instance.NotifyDead(MapIndex);
    }


    public void SetMapIndex(int index)
    {
        MapIndex = index;
    }
}
