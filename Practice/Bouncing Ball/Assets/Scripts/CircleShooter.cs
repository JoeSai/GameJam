using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CircleShooter : MonoBehaviour
{
    [SerializeField] private Transform pointA;  // 第一个点
    [SerializeField] private Transform pointB;  // 第二个点
    public float speed = 1;   // 移动速度

    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform boundary;
    [SerializeField] private Transform lineParent;
    [SerializeField] private List<Transform> lines;

    [SerializeField] private GameObject ballPrefab;

    private Vector3 tarPos;
    private Vector3 gravity;
    private Vector3 initialVelocity = new Vector3(1, 0, 0); // 初始速度

    private void Start()
    {
        lines = new List<Transform>();
        foreach (Transform child in lineParent)
        {
            lines.Add(child);
        }

        tarPos = GetRandomPoint();

        gravity = Physics.gravity;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            // 向第一个点移动
            transform.position = Vector3.MoveTowards(transform.position, pointA.position, speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            // 向第二个点移动
            transform.position = Vector3.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);
        }

        DrawLines();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootBall();
        }
    }

    private void DrawLines()
    {
        Vector3 velocity = initialVelocity + gravity;
        Vector3 tempPos = transform.position;
        float timeStep = 0.01f;
        int linesCount = lines.Count;
        for (int i = 0; i < linesCount; i++)
        {
            tempPos += velocity * timeStep;
            lines[i].position = tempPos;

            velocity += gravity * timeStep;
        }

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    tarPos = GetRandomPoint();
        //}
    }

    public Vector3 GetRandomPoint()
    {
        BoxCollider2D boxCollider2D = boundary.GetComponent<BoxCollider2D>();
        Bounds bounds = boxCollider2D.bounds;
        Vector3 randomPoint = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            0
        );
        return randomPoint;
    }

    public void ShootBall()
    {
        Instantiate(ballPrefab, transform.position, Quaternion.identity);
    }
}
