using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour {

    public Shapes spellNecessary;

    [Header("Private Content")]
    [HideInInspector] public List<TeleportPoint> _pointsInScene;

    private void Start()
    {
        _pointsInScene.AddRange(FindObjectsByType<TeleportPoint>(FindObjectsSortMode.None));

        // AGREGAR EL MANAGER A CADA POINT
        for (int i = 0; i < _pointsInScene.Count; i++) { _pointsInScene[i].manager = this; }
    }
    public TeleportPoint GetPoint(int index)
    {
        for(int i = 0; i < _pointsInScene.Count; i++)
        {
            if (_pointsInScene[i].idPoint == index)
            {
                return _pointsInScene[i];
            }
        }

        return _pointsInScene[0];
    }
}
