using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRender : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform player;
    public float lineWidth = 0.1f;
    public float removalRate = 2f;
    public float movementSpeed = 5f;
    public Vector3 mousePosition;
    private Camera mainCamera;
    private bool isMousePressed;
    private int pointsCount;
    private Vector3 previousPosition;
    public float minDistance = 0.1f, offset;

    private Coroutine coroutine;
    void Start()
    {
        mainCamera = Camera.main;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 1;
        previousPosition = transform.position;
        StopCoroutine(coroutine);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMousePressed = true;
            Start();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMousePressed = false;
            ReverseLineRendererPositions();
            coroutine = StartCoroutine(MovePointsSmoothly());
        }

        if (Input.GetMouseButton(0))
        {
            DrawLine();
        }
    }

    void DrawLine()
    {
        
        this.mousePosition = Input.mousePosition;
        this.mousePosition.z = offset; // Ensure the line stays in the 2D plane
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(this.mousePosition);

        if (Vector3.Distance(mousePosition, previousPosition) > minDistance)
        {
            if (previousPosition == transform.position)
            {
                lineRenderer.SetPosition(0, mousePosition);
            }
            else
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, mousePosition);
            }

            previousPosition = mousePosition;
        }
    }

    IEnumerator MovePointsSmoothly()
    {
        int lineLength = lineRenderer.positionCount;
        for (int i = lineLength - 1; i > 0; i--)
        {
            Vector3 targetPosition = lineRenderer.GetPosition(i - 1);

            while (Vector3.Distance(lineRenderer.GetPosition(i), targetPosition) > 0.01f)
            {
                lineRenderer.SetPosition(i, Vector3.MoveTowards(lineRenderer.GetPosition(i), targetPosition, Time.deltaTime * movementSpeed));
                player.position = targetPosition;
                yield return null;
            }
            lineRenderer.positionCount--;
        }

    }

    void ReverseLineRendererPositions()
    {
        int lineLength = lineRenderer.positionCount;
        Vector3[] positions = new Vector3[lineLength];

        for (int i = 0; i < lineLength; i++)
        {
            positions[i] = lineRenderer.GetPosition(lineLength - 1 - i);
        }

        lineRenderer.SetPositions(positions);
    }
}
