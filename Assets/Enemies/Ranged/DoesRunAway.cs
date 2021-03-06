using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoesRunAway : MonoBehaviour
{
    [Header("Target")]
    public Transform scaryman;
    [Header("Stats")]
    [SerializeField]private float run_speed;

    private Vector2 direction_away_from_scaryman;

    public void RunAway()
    {
        direction_away_from_scaryman = (transform.position - scaryman.position).normalized;
        transform.position += (Vector3)(run_speed * direction_away_from_scaryman * Time.deltaTime);
    }
}
