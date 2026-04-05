using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("Unity.RenderPipelines.Universal.Editor")]

namespace UnityEditor.Rendering.Universal
{
    public static class MaterialAccess
    {
        public static int ReadMaterialRawRenderQueue(Material mat)
        {
            return mat.rawRenderQueue;
        }
    }
}
