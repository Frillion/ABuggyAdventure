using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequests : MonoBehaviour
{
    Queue<PathRequest> PathRequestQueue = new Queue<PathRequest>();
    PathRequest CurrentRequest;

    static PathRequests Instance;
    AStarPathFinding PathFinder;

    bool IsProcessing;

    private void Awake()
    {
        Instance = this;
        PathFinder = GetComponent<AStarPathFinding>();
    }

    void TryProcessNext()
    {
        if (!IsProcessing && PathRequestQueue.Count > 0)
        {
            CurrentRequest = PathRequestQueue.Dequeue();
            IsProcessing = true;
            PathFinder.StartFindPath(CurrentRequest.PathStart, CurrentRequest.PathEnd);
        }
    }

    public void FinishedPath(Vector3[] Path, bool Success)
    {
        CurrentRequest.CallBack(Path, Success);
        IsProcessing = false;
        TryProcessNext();
    }

    public static void RequestPath(Vector3 PathStart, Vector3 PathEnd, Action<Vector3[], bool> CallBack)
    {
        PathRequest NewRequest = new PathRequest(PathStart, PathEnd, CallBack);
        Instance.PathRequestQueue.Enqueue(NewRequest);
        Instance.TryProcessNext();
    }

    struct PathRequest
    {
        public Vector3 PathStart;
        public Vector3 PathEnd;
        public Action<Vector3[], bool> CallBack;

        public PathRequest(Vector3 _Start, Vector3 _End, Action<Vector3[], bool> _CallBack)
        {
            PathStart = _Start;
            PathEnd = _End;
            CallBack = _CallBack;
        }
    }
}
