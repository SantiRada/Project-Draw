using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DrawMouse : MonoBehaviour {

    public bool canDraw = false;

    [Header("Dibujar")]
    public LineRenderer linePrefab;
    private LineRenderer _currentLine;
    private List<Vector2> _points = new List<Vector2>();
    private HashSet<Vector2> _pointSet = new HashSet<Vector2>();

    [Header("Objetivos")]
    public Material[] materials;
    public float umbralMinimo = 0.6f;

    [Header("Sistema")]
    private List<Vector2>[] objetivoPuntos = new List<Vector2>[7];
    private Generator _generator;

    public string drawCompleteName;
    private bool _isDrawing = false;

    [Header("Private Content")]
    private PlayerMovement _player;

    private void Start()
    {
        Application.targetFrameRate = 60;

        _generator = FindAnyObjectByType<Generator>();
        _player = GetComponent<PlayerMovement>();

        objetivoPuntos = _generator.GetGoals();
    }
    private void Update()
    {
        if (!canDraw) return;

        if (Input.GetMouseButtonDown(0)) { StartDrawing(); }

        if (Input.GetMouseButton(0) && _isDrawing)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (_pointSet.Add(mousePosition)) { AddPoint(mousePosition); }
        }

        if (Input.GetMouseButtonUp(0) && _isDrawing) { EndDrawing(); }
    }
    private void StartDrawing()
    {
        _isDrawing = true;
        _points.Clear();
        _pointSet.Clear();

        if (_currentLine != null)
        {
            Destroy(_currentLine.gameObject);
            _currentLine = null;
        }

        _currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
    }
    private void AddPoint(Vector2 point)
    {
        _points.Add(point);
        _currentLine.positionCount = _points.Count;
        _currentLine.SetPosition(_currentLine.positionCount - 1, point);
    }
    private void EndDrawing()
    {
        _isDrawing = false;

        float mejorSimilitud = 0f;
        int mejorIndice = -1;

        for (int i = 0; i < objetivoPuntos.Length; i++)
        {
            float similitudDirecta = CompararFormas(_points, objetivoPuntos[i]);
            float similitudInvertida = CompararFormas(_points, objetivoPuntos[i], invertida: true);

            if (similitudDirecta > mejorSimilitud)
            {
                mejorSimilitud = similitudDirecta;
                mejorIndice = i;
            }

            if (similitudInvertida > mejorSimilitud)
            {
                mejorSimilitud = similitudInvertida;
                mejorIndice = i;
            }
        }

        if (mejorIndice != -1 && mejorSimilitud >= umbralMinimo)
        {
            drawCompleteName = _generator.GetName(mejorIndice);
            _currentLine.material = materials[mejorIndice];

            _player.Attack(drawCompleteName, materials[mejorIndice].color);
        }

        if (_currentLine != null)
        {
            Destroy(_currentLine.gameObject, 0.25f);
            _currentLine = null;
        }
    }
    /* ----- VALIDACIÓN DE HECHIZOS ----- */
    private float CompararFormas(List<Vector2> dibujo, List<Vector2> objetivo, bool invertida = false)
    {
        if (invertida) { dibujo.Reverse(); }

        List<Vector2> dibujoSimplificado = SimplifyTrajectory(dibujo, umbral: 0.5f);
        List<Vector2> objetivoSimplificado = SimplifyTrajectory(objetivo, umbral: 0.5f);

        List<Vector2> dibujoNormalizado = NormalizeTrajectory(dibujoSimplificado, 1f);
        List<Vector2> objetivoNormalizado = NormalizeTrajectory(objetivoSimplificado, 1f);

        dibujoNormalizado = ReescalarPuntos(dibujoNormalizado, objetivoNormalizado.Count);

        float distanciaTotal = 0f;
        for (int i = 0; i < objetivoNormalizado.Count; i++)
        {
            distanciaTotal += Vector2.Distance(dibujoNormalizado[i], objetivoNormalizado[i]);
        }

        return 1f / (1f + distanciaTotal / objetivoNormalizado.Count);
    }
    private List<Vector2> SimplifyTrajectory(List<Vector2> puntos, float umbral)
    {
        List<Vector2> puntosSimplificados = new List<Vector2> { puntos[0] };

        for (int i = 1; i < puntos.Count - 1; i++)
        {
            if (Vector2.Distance(puntos[i], puntosSimplificados[(puntosSimplificados.Count - 1)]) > umbral)
            {
                puntosSimplificados.Add(puntos[i]);
            }
        }

        puntosSimplificados.Add(puntos[(puntos.Count - 1)]);
        return puntosSimplificados;
    }
    private List<Vector2> NormalizeTrajectory(List<Vector2> puntos, float escala)
    {
        Vector2 centro = new Vector2(puntos.Average(p => p.x), puntos.Average(p => p.y));
        float maxDistancia = puntos.Max(p => Vector2.Distance(p, centro));

        return puntos.Select(p => (p - centro) / maxDistancia * escala).ToList();
    }
    private List<Vector2> ReescalarPuntos(List<Vector2> puntos, int cantidad)
    {
        List<Vector2> puntosReescalados = new List<Vector2>();

        for (int i = 0; i < cantidad; i++)
        {
            float t = i / (float)(cantidad - 1);
            float distanciaTotal = (puntos.Count - 1) * t;
            int puntoPrevio = Mathf.FloorToInt(distanciaTotal);
            int puntoSiguiente = Mathf.CeilToInt(distanciaTotal);

            if (puntoPrevio == puntoSiguiente)
            {
                puntosReescalados.Add(puntos[puntoPrevio]);
            }
            else
            {
                Vector2 interpolado = Vector2.Lerp(puntos[puntoPrevio], puntos[puntoSiguiente], distanciaTotal - puntoPrevio);
                puntosReescalados.Add(interpolado);
            }
        }

        return puntosReescalados;
    }
}
