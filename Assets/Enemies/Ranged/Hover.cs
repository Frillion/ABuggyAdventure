using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    [Header("Hover AI")]
    [SerializeField] private Transform target;
    [SerializeField] private float hovermode_speed;

    public void HoverBehaviour()
    {
        transform.LookAt(target);
        transform.RotateAround(target.position, new Vector3(0, 0, 1), hovermode_speed * Time.deltaTime);
    }
}
