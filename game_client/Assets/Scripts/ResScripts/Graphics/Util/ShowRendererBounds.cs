
using System;
using UnityEngine;

public class ShowRendererBounds: MonoBehaviour
{
    private Renderer renderer;
    private void OnDrawGizmos()
    {
        if (renderer == null)
            renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            Color beforeColor = Gizmos.color;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(renderer.bounds.center, renderer.bounds.size);
            Gizmos.color = beforeColor;
        }
    }
}
