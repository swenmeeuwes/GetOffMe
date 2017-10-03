using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ComboCircle : MonoBehaviour {
    [SerializeField]
    private Camera orthographicCamera;
    [SerializeField][Tooltip("Quality of the circle, amount of points that will be rendered")]
    private int circleQuality = 10;
    [SerializeField]
    private float baseDistortInterval = 0.05f;
    [SerializeField]
    private float maxDistortInterval = 0.01f;

    public float circleRadius = 1f;
    public float invisibleCircleRadiusOffset = 0.1f; // The distance outside of the visible server that still allows for a combo
    public AnimationCurve sizeInterpolation;

    private LineRenderer lineRenderer;    

    private float screenDiagonal;
    private float screenRadius;
    private Vector3[] originalVerticies;
    private float distortInterval;

    private void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.loop = true;

        Initialize();        

        distortInterval = baseDistortInterval;

        DistortingScale = 0;
        Keyframe = 0;
    }

    private void Update()
    {
        
    }

    public Color Color
    {
        set
        {
            if (lineRenderer == null)
                return;

            lineRenderer.startColor = value;
            lineRenderer.endColor = value;
        }
    }

    public float Radius
    {
        set
        {
            circleRadius = value;
            GenerateCircle();
        }
        get {
            return circleRadius;
        }
    }

    public float Keyframe
    {
        set
        {
            var scalePercentage = sizeInterpolation.Evaluate(value) / 100f; // % of screen covered            
            Radius = screenRadius * scalePercentage;
        }
    }

    private float m_distortingScale;
    public float DistortingScale // Ranges from 0 to 1
    {
        get
        {
            return m_distortingScale;
        }
        set
        {
            m_distortingScale = value;
            distortInterval = Mathf.Lerp(baseDistortInterval, maxDistortInterval, Mathf.Clamp01(value));

            if (value > 0)
                StartCoroutine(Distort());
        }
    }

    public void GenerateCircle()
    {
        if (lineRenderer == null)
            return;

        var vertices = ComputeVertices();
        lineRenderer.positionCount = vertices.Length;
        lineRenderer.SetPositions(vertices);
    }

    public bool Intersects(Vector2 point)
    {
        return Vector2.Distance(transform.position, point) - invisibleCircleRadiusOffset < circleRadius;
    }

    private void Initialize()
    {
        var cameraHeight = orthographicCamera.orthographicSize * 2f;
        var cameraWidth = cameraHeight * orthographicCamera.aspect;

        screenDiagonal = (float)Math.Sqrt(Math.Pow(cameraHeight, 2) + Math.Pow(cameraWidth, 2));
        screenRadius = screenDiagonal / 2;
    }

    private Vector3[] ComputeVertices()
    {
        var points = new Vector3[circleQuality];
        for (int i = 0; i < circleQuality; i++)
        {
            var degree = (i / (float)circleQuality) * 360f;
            var radianAngle = degree * Mathf.Deg2Rad;

            var x = (float)(orthographicCamera.transform.position.x + circleRadius * Math.Cos(radianAngle));
            var y = (float)(orthographicCamera.transform.position.y + circleRadius * Math.Sin(radianAngle));

            points[i] = new Vector2(x, y);
        }

        originalVerticies = points;

        return points;
    }

    private IEnumerator Distort()
    {
        while (DistortingScale > 0) {
            for (int i = 0; i < lineRenderer.positionCount; i++)
                {
                    // Wish: Put offset range in inspector
                    var offsetX = UnityEngine.Random.value / 8 - 0.0625f;
                    var offsetY = UnityEngine.Random.value / 8 - 0.0625f;

                    var position = originalVerticies[i];
                    position.x += offsetX;
                    position.y += offsetY;

                    lineRenderer.SetPosition(i, position);
                }

            yield return new WaitForSeconds(distortInterval);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, circleRadius + invisibleCircleRadiusOffset);
    }
}
