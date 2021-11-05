using System.Collections;
using UnityEngine;

public interface IChase : IPathCalculations
{
    IEnumerator FollowPath();
    void OnPathFound(Vector3[] NewPath, bool PathSuccess);
    void StartPath();
    void StopPath();
}
