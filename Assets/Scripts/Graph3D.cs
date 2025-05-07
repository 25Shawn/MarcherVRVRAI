using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(LineRenderer))]
public class Graph3D : MonoBehaviour
{
    [Header("Fond de graphe (Quad 3D)")]
    public GameObject backgroundQuad;

    [Header("Affichage du prix actuel")]
    public TextMeshPro prixText;
    public TextMeshPro minValeurText;
    public TextMeshPro maxValeurText;

    [Header("Réglages de la courbe")]
    public int pointCount = 100;
    public float logicalMinY = 0f;
    public float logicalMaxY = 10f;

    [Header("Réglages dynamiques")]
    [Range(0.001f, 0.2f)] public float scrollSpeed = 0.02f;
    [Range(0.1f, 10f)] public float amplitudeMultiplier = 1f;

    private LineRenderer lineRenderer;
    private List<float> values = new List<float>();
    private float graphWidth;
    private float graphHeight;
    private float xSpacing;

    private float scrollTimer = 0f;
    private float scrollThreshold = 0.05f;

    public float prixActuel { get; private set; }

    void Start()
    {
        if (prixText == null)
            Debug.LogError("[Graph3D] prixText n’est pas assigné !");
        if (minValeurText == null)
            Debug.LogWarning("[Graph3D] minValeurText n’est pas assigné !");
        if (maxValeurText == null)
            Debug.LogWarning("[Graph3D] maxValeurText n’est pas assigné !");
        if (backgroundQuad == null)
            Debug.LogError("[Graph3D] backgroundQuad n’est pas assigné !");

        lineRenderer = GetComponent<LineRenderer>();
        SetupRenderer();
        SetupGraphDimensions();

        float initialValue = Random.Range(logicalMinY, logicalMaxY);
        for (int i = 0; i < pointCount; i++)
        {
            values.Add(initialValue);
        }

        prixActuel = initialValue;
        SafeSetText(minValeurText, $"${logicalMinY:F2}");
        SafeSetText(maxValeurText, $"${logicalMaxY:F2}");
        SafeSetText(prixText, $"${initialValue:F2}");

        lineRenderer.positionCount = pointCount;
        UpdateLinePositions();
    }

    void Update()
    {
        if (prixText == null) return;

        scrollTimer += Time.deltaTime;

        if (scrollTimer >= scrollThreshold / scrollSpeed)
        {
            scrollTimer = 0f;

            for (int i = 0; i < values.Count - 1; i++)
            {
                values[i] = values[i + 1];
            }

            float lastValue = values[values.Count - 2];
            float direction = Random.Range(-0.5f, 0.5f);
            float noise = Random.Range(-1f, 1f) * 0.3f;
            float newY = lastValue + (direction + noise) * amplitudeMultiplier;
            newY = Mathf.Clamp(newY, logicalMinY, logicalMaxY);

            prixActuel = newY;
            values[values.Count - 1] = newY;

            SafeSetText(prixText, $"${newY:F2}");
            UpdateLinePositions();
        }
    }

    void SafeSetText(TextMeshPro textComponent, string value)
    {
        if (textComponent != null)
        {
            textComponent.text = value;
        }
    }

    void SetupRenderer()
    {
        lineRenderer.useWorldSpace = false;
        lineRenderer.widthMultiplier = 0.01f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
    }

    void SetupGraphDimensions()
    {
        if (backgroundQuad != null)
        {
            Vector3 scale = backgroundQuad.transform.localScale;
            graphWidth = scale.x;
            graphHeight = scale.y;
            xSpacing = graphWidth / (pointCount - 1);

            Debug.Log($"[Graph3D] Taille Quad: {graphWidth} x {graphHeight}");
        }
        else
        {
            Debug.LogError("[Graph3D] Le Quad de fond n'est pas assigné !");
        }
    }

    void UpdateLinePositions()
    {
        Vector3[] positions = new Vector3[pointCount];

        float xStart = -graphWidth / 2f;
        float xEnd = graphWidth / 2f;

        float verticalPadding = 0.05f; // 5% padding
        float innerHeight = graphHeight * (1f - verticalPadding * 2f);
        float yBase = -graphHeight / 2f + verticalPadding * graphHeight;

        for (int i = 0; i < pointCount; i++)
        {
            float t = 1f - ((float)i / (pointCount - 1)); // droite -> gauche
            float x = Mathf.Lerp(xStart, xEnd, t);

            float normalizedY = Mathf.InverseLerp(logicalMinY, logicalMaxY, values[i]);
            float y = yBase + normalizedY * innerHeight;

            positions[i] = new Vector3(x, y, 0f);
        }

        lineRenderer.SetPositions(positions);
    }

    void OnDestroy()
    {
        prixText = null;
    }
}
