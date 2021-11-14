using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float view_radius;
    [Range(0,360)]
    public float view_angle;
    public float angle_offset;

    public LayerMask target_mask;
    public LayerMask obsticle_mask;

    [HideInInspector]
    public List<Transform> visible_targets = new List<Transform>();

    private void Start()
    {
        StartCoroutine("FindVisibleTargetsWithDelay",.2f);
    }


    public float InputToDegrees(Vector3 inputs)
    {
        if ((float)Math.Round((double)inputs.y,1) == 0.7f)
        {
            if ((float)Math.Round((double)inputs.x, 1) == 0.7f) {  return 90f;  }
            else if((float)Math.Round((double)inputs.x, 1) == -0.7f) {  return 630f; }
        }
        else if ((float)Math.Round((double)inputs.y, 1) == -0.7f)
        {
            if ((float)Math.Round((double)inputs.x, 1) == 0.7f) {  return 270f; }
            else if ((float)Math.Round((double)inputs.x, 1) == -0.7f) { return 450f; }
        }
        return 0;
    }

    IEnumerator FindVisibleTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        visible_targets.Clear();
        Collider2D[] targets_in_radius = Physics2D.OverlapCircleAll(new Vector2(transform.position.x,transform.position.y),view_radius,target_mask, -Mathf.Infinity, Mathf.Infinity);

        for (int i = 0; i < targets_in_radius.Length; i++)
        {
            Transform target = targets_in_radius[i].transform;
            Vector2 direction_to_target = (target.position - transform.position).normalized;
            Debug.Log(direction_to_target);
            if (Vector2.Angle((new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))).normalized  ,direction_to_target) < view_angle/2 && Vector2.Angle(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized, direction_to_target) != 0)
            {
                float distance_to_target = Vector2.Distance(transform.position,target.position);
                if (!Physics2D.Linecast(transform.position,target.position,obsticle_mask))
                {
                    visible_targets.Add(target);
                }
            }
        }
    }

    public Vector3 DirectionFromAngle(float angle_in_degrees,bool angle_is_global)
    {
        if (!angle_is_global)
        {
            angle_in_degrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angle_in_degrees * Mathf.Deg2Rad), Mathf.Cos(angle_in_degrees * Mathf.Deg2Rad), 0);
    }
}
