using UnityEngine;

public interface IPathCalculations
{
    float CalculateDistance();
    Vector2 GetDirection();
    void Move();
}