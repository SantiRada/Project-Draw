using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {

    public float minSize = 2f;
    public float maxSize = 5f;

    [Header("Saved Data")]
    public List<Vector2>[] _goals = new List<Vector2>[7];
    private string[] _nameGoals = new string[7];

    private void Start() { GenerarForma(); }
    private void GenerarForma()
    {
        GenerarU();
        GenerarInversaU();
        GenerarMenorQue();
        GenerarMayorQue();
        GenerarZ();
        GenerarN();
        GenerarI();
    }
    // --------------------------------- //
    private void GenerarU()
    {
        float ancho = Random.Range(minSize, maxSize);
        float alto = Random.Range(minSize, maxSize);
        List<Vector2> puntosU = new List<Vector2>
        {
            new Vector2(-ancho / 2, alto / 2),
            new Vector2(-ancho / 2, -alto / 2),
            new Vector2(ancho / 2, -alto / 2),
            new Vector2(ancho / 2, alto / 2)
        };

        _nameGoals[0] = "U";
        _goals[0] = puntosU;
    }
    private void GenerarInversaU()
    {
        float ancho = Random.Range(minSize, maxSize);
        float alto = Random.Range(minSize, maxSize);
        int puntos = 20;

        List<Vector2> points = new List<Vector2>();

        for (int i = 0; i <= puntos; i++)
        {
            float t = i / (float)puntos;
            float x = Mathf.Lerp(-ancho / 2, ancho / 2, t);
            float y = Mathf.Sqrt(1 - Mathf.Pow((x / (ancho / 2)), 2)) * alto / 2;
            points.Add(new Vector2(x, y - alto / 2));
        }

        _goals[1] = points;
        _nameGoals[1] = "UInvertida";
    }
    private void GenerarMenorQue()
    {
        float tamano = Random.Range(minSize, maxSize);
        List<Vector2> puntosMenor = new List<Vector2>
    {
        new Vector2(tamano / 2, tamano / 2),
        new Vector2(-tamano / 2, 0),
        new Vector2(tamano / 2, -tamano / 2)
    };

        _nameGoals[2] = "MenorQue";
        _goals[2] = puntosMenor;
    }
    private void GenerarMayorQue()
    {
        float tamano = Random.Range(minSize, maxSize);
        List<Vector2> puntosMayor = new List<Vector2>
    {
        new Vector2(-tamano / 2, tamano / 2),
        new Vector2(tamano / 2, 0),
        new Vector2(-tamano / 2, -tamano / 2)
    };

        _nameGoals[3] = "MayorQue";
        _goals[3] = puntosMayor;
    }
    private void GenerarZ()
    {
        float ancho = Random.Range(minSize, maxSize);
        float alto = Random.Range(minSize, maxSize);
        List<Vector2> puntosZ = new List<Vector2>
    {
        new Vector2(-ancho / 2, alto / 2),
        new Vector2(ancho / 2, alto / 2),
        new Vector2(-ancho / 2, -alto / 2),
        new Vector2(ancho / 2, -alto / 2)
    };

        _nameGoals[4] = "Z";
        _goals[4] = puntosZ;
    }
    private void GenerarN()
    {
        float ancho = Random.Range(minSize, maxSize);
        float alto = Random.Range(minSize, maxSize);
        List<Vector2> puntosN = new List<Vector2>
    {
        new Vector2(-ancho / 2, -alto / 2),
        new Vector2(-ancho / 2, alto / 2),
        new Vector2(ancho / 2, -alto / 2),
        new Vector2(ancho / 2, alto / 2)
    };

        _nameGoals[5] = "N";
        _goals[5] = puntosN;
    }
    private void GenerarI()
    {
        float alto = Random.Range(minSize, maxSize);
        List<Vector2> puntosI = new List<Vector2>
    {
        new Vector2(0, -alto / 2),
        new Vector2(0, alto / 2)
    };

        _nameGoals[6] = "I";
        _goals[6] = puntosI;
    }
    // --------------------------------- //
    public List<Vector2>[] GetGoals() { return _goals; }
    public string GetName(int i) { return _nameGoals[i]; }
}