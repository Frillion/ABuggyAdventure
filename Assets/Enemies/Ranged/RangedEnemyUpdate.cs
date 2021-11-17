using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyUpdate : MonoBehaviour
{
    [Header("AI Stats")]
    [SerializeField] private float stopping_distance;
    
    private IChase chaser;
    private Hover hover;
    private DoesRunAway runner;
    private FieldOfView is_scared;

    private void Start()
    {
        chaser = GetComponent<IChase>();
        hover = gameObject.GetComponent<Hover>();
        runner = gameObject.GetComponent<DoesRunAway>();
        is_scared = GameObject.FindGameObjectWithTag("Player").GetComponent<FieldOfView>();
    }

    void Update()
    {
        if (is_scared.visible_targets.Contains(transform))
        {
            runner.RunAway();
        }
        else if (chaser.CalculateDistance() > stopping_distance)
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
