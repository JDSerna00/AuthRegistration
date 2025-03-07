using UnityEngine;

public class Projectile: MonoBehaviour
{
    public Vector3 direction = Vector3.up;
    public float speed = 20f;

    private void Update()
    {
        transform.position += speed * Time.deltaTime * direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("touched that" + other);
        CheckCollision(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("stayed onto that" + other);
        CheckCollision(other);
    }

    private void CheckCollision(Collider2D other)
    {
        Destroy(gameObject);
    }
}