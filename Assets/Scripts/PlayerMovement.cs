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
    public AudioClip DeathSound, JumpSound;

    private int EnemyLayer;
    private Rigidbody2D Rigidbody2D;
    private AudioSource AudioSource;

    private bool Jump;
    [HideInInspector] public bool Dead;
    private float LastTimeParticle;
    private bool WasGround;
    private int MapIndex;

    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        AudioSource = GetComponent<AudioSource>();

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
                CameraStress.InduceStress(0.2f);
                PostprocessManager.Instance.ChangeColor();
            }
        }
        else
        {
            CubeVisual.Rotate(Vector3.back * RotationSpeed * Time.deltaTime);
        }

        Jump = ground && (Jump || IsInput());
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
            // HARDCODED
            PlayerMovement[] players = FindObjectsOfType<PlayerMovement>(false);
            if (players.Length == 1)
                AudioSource.PlayOneShot(JumpSound, 0.2f);
            else
            {
                for (int i = 0; i < players.Length; ++i)
                {
                    if (!players[i].Dead)
                    {
                        if (players[i] == this) AudioSource.PlayOneShot(JumpSound, 0.2f);
                        break;
                    }
                }
            }
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

    public void Die()
    {
        Dead = true;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        DeadParticles.Play();
        CameraStress.InduceStress(1.0f);
        GameManager.Instance.NotifyDead(MapIndex);
        AudioSource.PlayOneShot(DeathSound, 0.7f);
    }


    public void SetMapIndex(int index)
    {
        MapIndex = index;
    }

    private bool IsInput()
    {
        return Input.GetKey(KeyCode.Space) ||
               Input.GetKey(KeyCode.W) ||
               Input.GetKey(KeyCode.UpArrow) ||
               Input.touchCount > 0;
    }
}
