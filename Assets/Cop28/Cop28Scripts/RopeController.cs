using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    [SerializeField] private float lineWidth = 0.1f;
    [SerializeField] private Vector3 offSet;
    private LineRenderer lineRenderer;
    private Transform[] points;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    void Update()
    {
        DrawLine();
        for (int i = 0; i < points.Length; i++)
        {
            lineRenderer.SetPosition(i, points[i].position + offSet);
        }
    }
    // take the array of points then store them.
    public void SetUpLine(Transform[] points)
    {
        lineRenderer.positionCount = points.Length;
        this.points = points;
    }
    void DrawLine()
    {
        //setting up the width of the rope
        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }
}
