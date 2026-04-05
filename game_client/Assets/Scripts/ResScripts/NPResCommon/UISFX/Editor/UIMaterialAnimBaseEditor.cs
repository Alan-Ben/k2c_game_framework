

    using System;
    using MG;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UI;

    [CustomEditor(typeof(UIMaterialAnimBase))]
    public class UIMaterialAnimBaseEditor : Editor
    {
        private void OnEnable()
        {
            if(Application.isPlaying)
                return;
            
            UIMaterialAnimBase mono = target as UIMaterialAnimBase;;
            var img = mono.GetComponent<Image>();
            if(img != null)
            {
                if(mono._renerer == null)
                    mono._renerer = img;
                if(img.material != Graphic.defaultGraphicMaterial)
                {
                    if(mono._material == null)
                        mono._material = img.material;
                }
            }
        }
    }
