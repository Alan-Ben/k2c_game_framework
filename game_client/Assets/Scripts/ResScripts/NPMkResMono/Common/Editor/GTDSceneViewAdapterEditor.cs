
using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GTDSceneViewAdapter), true)]
public class GTDSceneViewAdapterEditor : Editor
{
    private void OnSceneGUI()
    {
        var mono = target as GTDSceneViewAdapter;
        Color oldColor = Handles.color;

        if (!Application.isPlaying)
        {
            float width = mono.normalHeight * mono.normalRatio;
            var viewRect = new Rect(-0.5f * width, mono.minPosY, width, mono.normalHeight);

            Vector3 leftDown  = mono.transform.position + new Vector3(viewRect.xMin, viewRect.yMin);
            Vector3 leftUp    = mono.transform.position + new Vector3(viewRect.xMin, viewRect.yMax);
            Vector3 rightUp   = mono.transform.position + new Vector3(viewRect.xMax, viewRect.yMax);
            Vector3 rightDown = mono.transform.position + new Vector3(viewRect.xMax, viewRect.yMin);
        
            Handles.color = Color.red;
            Handles.DrawLine(leftDown, leftUp);
            Handles.DrawLine(rightUp, leftUp);
            Handles.DrawLine(leftDown, rightDown);
            Handles.DrawLine(rightUp, rightDown);
            
            Handles.color = Color.blue;
            Vector3 pivotL = mono.transform.position + new Vector3(viewRect.xMin, viewRect.yMin + mono.pivotY * mono.normalHeight);
            Vector3 pivotR = mono.transform.position + new Vector3(viewRect.xMax, viewRect.yMin + mono.pivotY * mono.normalHeight);
            Handles.DrawLine(pivotL, pivotR);
        }
        else
        {
            float width = mono.normalHeight * mono.normalRatio;
            var viewRect = new Rect(-0.5f * width, mono.minPosY, width, mono.normalHeight);

            Vector3 leftDown  = mono.transform.position + new Vector3(viewRect.xMin, viewRect.yMin);
            Vector3 leftUp    = mono.transform.position + new Vector3(viewRect.xMin, viewRect.yMax);
            Vector3 rightUp   = mono.transform.position + new Vector3(viewRect.xMax, viewRect.yMax);
            Vector3 rightDown = mono.transform.position + new Vector3(viewRect.xMax, viewRect.yMin);
        
            Handles.color = Color.red;
            Handles.DrawLine(leftDown, leftUp);
            Handles.DrawLine(rightUp, leftUp);
            Handles.DrawLine(leftDown, rightDown);
            Handles.DrawLine(rightUp, rightDown);
            
            var newRect = mono.calculateNewRect();
            leftDown  = mono.transform.position + new Vector3(newRect.xMin, newRect.yMin);
            leftUp    = mono.transform.position + new Vector3(newRect.xMin, newRect.yMax);
            rightUp   = mono.transform.position + new Vector3(newRect.xMax, newRect.yMax);
            rightDown = mono.transform.position + new Vector3(newRect.xMax, newRect.yMin);
        
            Handles.color = Color.green;
            Handles.DrawLine(leftDown, leftUp);
            Handles.DrawLine(rightUp, leftUp);
            Handles.DrawLine(leftDown, rightDown);
            Handles.DrawLine(rightUp, rightDown);
            
            Handles.color = Color.blue;
            Vector3 pivotL = mono.transform.position + new Vector3(viewRect.xMin, viewRect.yMin + mono.pivotY * mono.normalHeight);
            Vector3 pivotR = mono.transform.position + new Vector3(viewRect.xMax, viewRect.yMin + mono.pivotY * mono.normalHeight);
            Handles.DrawLine(pivotL, pivotR);
        
        }
        Handles.color = oldColor;

    }
}
