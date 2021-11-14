using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyUpdate : MonoBehaviour
{
    [Header("AI Stats")]
    [SerializeField] private float stopping_distance;
    
    private IChase chaser;
    private Hover hover;

    private void Start()
    {
        chaser = GetComponent<IChase>();
        hover = gameObject.GetComponent<Hover>();
    }

    void Update()
    {
        if (chaser.CalculateDistance() > stopping_distance)
        {
            chaser.StartPath();
            chaser.Move();
        }
        else
        {
            hover.HoverBehaviour();
        }
    }
}
