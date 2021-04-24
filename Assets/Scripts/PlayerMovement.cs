using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 1.0f;
    public float JumpForce = 1.0f;
    public float RotationSpeed = 1.0f;
    public Transform GroundCheck;
    public Transform CubeVisual;

    private int EnemyLayer;
    private Rigidbody2D Rigidbody2D;

    private bool Jump;
    private bool Dead;

    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();

        EnemyLayer = LayerMask.NameToLayer("Enemy");
    }

    private void Update()
    {
        bool ground = Physics2D.Raycast(GroundCheck.transform.position, Vector2.down, 0.05f);

        if (ground)
        {
            Quaternion rot = CubeVisual.rotation;
            rot.z = Mathf.Round(rot.z / 90) * 90;
            CubeVisual.rotation = rot;
        }
        else
        {
            CubeVisual.Rotate(Vector3.back * RotationSpeed * Time.deltaTime);
        }

        Jump = ground && (Jump || Input.GetKey(KeyCode.Space));
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
        StartCoroutine(ReloadScene());
    }

    private IEnumerator ReloadScene()
    {
        const string sceneName = "MainScene";
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(sceneName);
    }
}
