using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor (typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fow = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.forward, Vector3.up, 360,fow.view_radius);
        Vector3 view_angle_a = fow.DirectionFromAngle((-fow.view_angle)/2, true);
        Vector3 view_angle_b = fow.DirectionFromAngle((fow.view_angle)/2, true);

        Handles.DrawLine(fow.transform.position, (fow.transform.position + view_angle_a * fow.view_radius));
        Handles.DrawLine(fow.transform.position, fow.transform.position + view_angle_b * fow.view_radius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + (new Vector3(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),0).normalized * fow.view_radius));
        Handles.color = Color.red;
        foreach (Transform visible_target in fow.visible_targets)
        {
            Handles.DrawLine(fow.transform.position, visible_target.position);
        }
    }
}
