using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

[ExecuteInEditMode]
public class ParticleClippable : UIBehaviour, IClippable
{
    private static readonly int UIMaskSoftnessX = Shader.PropertyToID("_UIMaskSoftnessX");
    private static readonly int UIMaskSoftnessY = Shader.PropertyToID("_UIMaskSoftnessY");
    private static readonly int ClipRect = Shader.PropertyToID("_ClipRect");
    
    public List<ParticleSystemRenderer> particleSystemRenderers = new List<ParticleSystemRenderer>();
    
    public bool useSharedMaterial = false; 
    
    MaterialPropertyBlock props;
    private RectTransform m_RectTransform;
    public RectTransform rectTransform
    {
        get
        {
            if (ReferenceEquals(m_RectTransform, null))
            {
                m_RectTransform = GetComponent<RectTransform>();
            }
            return m_RectTransform;
        }
    }

    private RectMask2D m_ParentMask;
    protected override void OnEnable()
    {
        UpdateClipParent();
    }
    public void Cull(Rect clipRect, bool validRect)
    {
    }
    private void UpdateClipParent()
    {
        // if the new parent is different OR is now inactive
        if (!gameObject.activeSelf)
        {
            return;
        }
        var newParent = MaskUtilities.GetRectMaskForClippable(this);
        if (m_ParentMask != null && (newParent != m_ParentMask || !newParent.IsActive()))
        {
            m_ParentMask.RemoveClippable(this);
        }
        // don't re-add it if the newparent is inactive
        if (newParent != null && newParent.IsActive())
            newParent.AddClippable(this);
 
        m_ParentMask = newParent;
    }
    //状态改变自动调用
    public void RecalculateClipping()
    {
        SetClipRect(Rect.zero, false);
        UpdateClipParent();
    }
    
    /// <summary>
    /// Find a root Canvas.
    /// </summary>
    /// <param name="start">Transform to start the search at going up the hierarchy.</param>
    /// <returns>Finds either the most root canvas, or the first canvas that overrides sorting.</returns>
    public static Canvas FindRootSortOverrideCanvas(Transform start)
    {
        var canvasList = ListPool<Canvas>.Get();
        start.GetComponentsInParent(false, canvasList);
        
        Canvas canvas = null;
        if (canvasList.Count >= 1)
        {
            canvas = canvasList[canvasList.Count-1];
        }
        
        ListPool<Canvas>.Release(canvasList);

        return canvas != null ? canvas : null;
    }
    
    //设置材质UIMask属性
    public void SetClipRect(Rect value, bool validRect)
    {
        if (props == null)
        {
            props = new MaterialPropertyBlock();
        }
        
        Canvas rootCanvas = FindRootSortOverrideCanvas(transform);;
        if(rootCanvas == null)
            return;
        
        foreach (ParticleSystemRenderer psRenderers in particleSystemRenderers)
        {
            if(psRenderers == null) continue;

            Material material = GetMaterial(psRenderers);
            if(material == null) continue;
                
            if (!isActiveAndEnabled)
            {
                material.DisableKeyword("UNITY_UI_CLIP_RECT");
                continue;
            }
            
            if (validRect)
            {
                Vector3[] wcs = new Vector3[2];
                wcs[0] = new Vector3(value.xMin, value.yMin, 0);
                wcs[1] = new Vector3(value.xMax, value.yMax, 0);
                

                Matrix4x4 mat = rootCanvas.transform.localToWorldMatrix;
                for (int i = 0; i < 2; ++i)
                    wcs[i] = mat.MultiplyPoint(wcs[i]);
                
                props.SetVector(ClipRect, new Vector4(wcs[0].x, wcs[0].y, wcs[1].x, wcs[1].y));
                material.EnableKeyword("UNITY_UI_CLIP_RECT");
                //Debug.Log("开启UNITY_UI_CLIP_RECT");
                psRenderers.SetPropertyBlock(props);
            }
            else
            {
                material.DisableKeyword("UNITY_UI_CLIP_RECT");
                //Debug.Log("关闭UNITY_UI_CLIP_RECT");
            }
        }
    }
    
    private Vector2 clipSoftness;
    public void SetClipSoftness(Vector2 _clipSoftness)
    {
        if (clipSoftness != _clipSoftness)
        {
            clipSoftness = _clipSoftness;
            UpdateSoftness();
        }
        
    }

    private void UpdateSoftness()
    {
        if (props == null)
        {
            props = new MaterialPropertyBlock();
        }
        
        Canvas rootCanvas = FindRootSortOverrideCanvas(transform);;
        if(rootCanvas == null) return;
            

        var renderCamera = rootCanvas.worldCamera != null ? rootCanvas.worldCamera : Camera.main;
        if (renderCamera == null) return;

        foreach (ParticleSystemRenderer psRenderers in particleSystemRenderers)
        {
            if(psRenderers == null) continue;
            
            //像素：世界坐标 比例，乘0.5计算中心
            Vector3 wcs = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
            
            Matrix4x4 mat = renderCamera.projectionMatrix;
            wcs = mat.MultiplyPoint(wcs);

            //将软边缘大小转为世界坐标尺寸大小
            props.SetFloat(UIMaskSoftnessX, clipSoftness.x / wcs.x );
            props.SetFloat(UIMaskSoftnessY, clipSoftness.y / wcs.y);
            
            psRenderers.SetPropertyBlock(props);
        }
        
    }
    
    private Material GetMaterial(ParticleSystemRenderer particleSystemRenderer)
    {
        if (useSharedMaterial)
        {
            return particleSystemRenderer.sharedMaterial;
        }
        
        return particleSystemRenderer.material;
    }
    
    protected override void OnTransformParentChanged()
    {
        UpdateClipParent();
    }
#if UNITY_EDITOR
    protected override void OnValidate()
    {
        UpdateClipParent();
    }
#endif
    protected override void OnDisable()
    {
        
    }
    
    protected override void OnDestroy()
    {
        if (m_ParentMask != null)
        {
            m_ParentMask.RemoveClippable(this);
        }
        base.OnDestroy();
    }
}
 
