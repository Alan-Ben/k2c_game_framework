using System;
using MG;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(UIMaterialColorForAnimation))]
public class UIMaterialColorForAnimationEdior : Editor
{
    private void OnEnable()
    {
        if(Application.isPlaying)
            return;
            
        UIMaterialColorForAnimation mono = target as UIMaterialColorForAnimation;;
        var img = mono.GetComponent<Graphic>();
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