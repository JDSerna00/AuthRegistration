using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{

    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] Invaders[] prefabs = new Invaders[5];
    private Vector3 direction = Vector3.right;
    [SerializeField] private float initialSpeed = 2f; // Initial speed of the invaders
    [SerializeField] private float speedIncreaseAmount = 0.3f; // Amount to increase speed by
    [SerializeField] private float maxSpeed = 5f;
    private float currentSpeed;
    private Vector3 initialPosition;
    public Projectile missilePrefab;
    public float missileSpawnRate = 1f;

    private void CreateInvaderGrid()
    {
        for (int i = 0; i < rows; i++)
        {
            float width = 3f * (columns - 1);
            float height = 2f * (rows - 1);

            Vector2 centerOffset = new Vector2(-width * 0.5f, -height * 0.5f);
            Vector3 rowPosition = new Vector3(centerOffset.x, (2f * i) + centerOffset.y, 0f);

            for (int j = 0; j < columns; j++)
            {
                // Create a new invader and parent it to this transform
                Invaders invader = Instantiate(prefabs[i], transform);

                // Calculate and set the position of the invader in the row
                Vector3 position = rowPosition;
                position.x += 3f * j;
                invader.transform.localPosition = position;
            }
        }
    }
    private void Awake()
    {
        initialPosition = transform.position;
        currentSpeed = initialSpeed;
        CreateInvaderGrid();
    }
    private void Start()
    {
        InvokeRepeating(nameof(MissileAttack), missileSpawnRate, missileSpawnRate);
    }
    private void MissileAttack()
    {
        int amountAlive = GetAliveCount();

        // No missiles should spawn when no invaders are alive
        if (amountAlive == 0)
        {
            return;
        }

        foreach (Transform invader in transform)
        {
            // Any invaders that are killed cannot shoot missiles
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            // Random chance to spawn a missile based upon how many invaders are
            // alive (the more invaders alive the lower the chance)
            if (Random.value < (1f / amountAlive))
            {
                Instantiate(missilePrefab, invader.position, Quaternion.identity);
                break;
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Calculate the percentage of invaders killed
        int totalCount = rows * columns;
        int amountAlive = GetAliveCount();
        int amountKilled = totalCount - amountAlive;
        float percentKilled = amountKilled / (float)totalCount;

        transform.position += currentSpeed * Time.deltaTime * direction;
        // Transform the viewport to world coordinates so we can check when the
        // invaders reach the edge of the screen


        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        // The invaders will advance to the next row after reaching the edge of
        // the screen
        foreach (Transform invader in transform)
        {
            // Skip any invaders that have been killed
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            // Check the left edge or right edge based on the current direction
            if (direction == Vector3.right && invader.position.x >= (rightEdge.x - 1f))
            {
                AdvanceRow();
                break;
            }
            else if (direction == Vector3.left && invader.position.x <= (leftEdge.x + 1f))
            {
                AdvanceRow();
                break;
            }
        }
    }
    private void AdvanceRow()
    {
        // Flip the direction the invaders are moving
        direction = new Vector3(-direction.x, 0f, 0f);

        // Move the entire grid of invaders down a row
        Vector3 position = transform.position;
        position.y -= 1f;
        transform.position = position;
    }
    private void IncreaseSpeed()
    {
        // Increase speed by the specified amount, but don't exceed the max speed
        currentSpeed = Mathf.Min(currentSpeed + speedIncreaseAmount, maxSpeed);
    }

    public void ResetInvaders()
    {
        direction = Vector3.right;
        transform.position = initialPosition;
        currentSpeed = initialSpeed;
        foreach (Transform invader in transform)
        {
            invader.gameObject.SetActive(true);
        }
    }

    public int GetAliveCount()
    {
        int count = 0;

        foreach (Transform invader in transform)
        {
            if (invader.gameObject.activeSelf)
            {
                count++;
            }
        }

        return count;
    }
}
