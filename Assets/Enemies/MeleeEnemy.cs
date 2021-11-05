using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour, IChase
{
    [Header("Stats")]
    [SerializeField] private float speed;

    [Header("Chaser")]
    public Transform target;
    private Vector3 current_waypoint;
    private Vector3[] path;
    private int target_index;

    [Header("Pathfinding")]
    [HideInInspector] public static AStarPathFinding path_finder;


    private void Awake()
    {
        path_finder = GetComponent<AStarPathFinding>();
    }

    public IEnumerator FollowPath()
    {
        if (path.Length > 0 && path != null)
        {
            current_waypoint = path[0];

            while (true)
            {
                if (transform.position == current_waypoint)
                {
                    target_index++;
                    if (target_index >= path.Length)
                    {
                        yield break;
                    }
                    current_waypoint = path[target_index];
                }
                yield return null;
            }
        }
    }
    //Return The Distance Between The Chaser And His target
    public float CalculateDistance()
    {
        return Vector2.Distance(target.transform.position, transform.position);
    }

    //Starting a new path if the path is done
    public void OnPathFound(Vector3[] NewPath, bool PathSuccess)
    {
        if (PathSuccess)
        {
            path = NewPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    public Vector3 GetForce(float Speed, Vector2 Direction)
    {
        return Speed * Direction * Time.deltaTime;
    }

    public Vector2 GetDirection()
    {
        return new Vector2(current_waypoint.x - transform.position.x, current_waypoint.y - transform.position.y).normalized;
    }

    public void Move()
    {
        transform.position += GetForce(speed, GetDirection());
    }

    public void StartPath()
    {
        PathRequests.RequestPath(transform.position, target.position, OnPathFound);
    }

    //Stop Path 
    public void StopPath()
    {
        StopCoroutine("FollowPath");
        path_finder.StopFindPath();
    }

    private void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = target_index; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == target_index)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }


}
