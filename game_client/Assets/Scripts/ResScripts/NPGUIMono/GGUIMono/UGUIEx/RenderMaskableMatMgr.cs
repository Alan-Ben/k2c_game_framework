using System.Collections.Generic;
using GOE;
using UnityEngine;


public class RenderMaskableMatMgr
{
    private static RenderMaskableMatMgr _g_instance;
    public static RenderMaskableMatMgr instance
    {
        get
        {
            if(null == _g_instance)
                _g_instance = new RenderMaskableMatMgr();
            return _g_instance;
        }
    }
    
    // 材质实例缓存：原材质 -> (Rect -> 材质实例)
    Dictionary<Material, Dictionary<Rect, Material>> _m_dMats = new Dictionary<Material, Dictionary<Rect, Material>>();
    
    // 材质实例引用计数：材质实例 -> 引用计数
    Dictionary<Material, int> _m_dMatRefCount = new Dictionary<Material, int>();
    
    // Renderer 材质映射：Renderer -> RendererMaterialInfo
    Dictionary<Renderer, RendererMaterialInfo> _m_dRendererMats = new Dictionary<Renderer, RendererMaterialInfo>();
    
    // Renderer 的材质信息
    class RendererMaterialInfo
    {
        public Material[] originalSharedMats;  // 原始的 sharedMaterials（用于恢复）
        public List<MaterialInstance> instances;  // 材质实例列表
    }
    
    // 材质实例信息
    class MaterialInstance
    {
        public int originalIndex;      // 原材质在数组中的索引
        public Material originalMat;   // 原始共享材质
        public Material instanceMat;   // 实例材质
        public Rect rect;              // 对应的裁剪矩形
    }
    
    /// <summary>
    /// 为 Renderer 启用矩形裁剪，使用材质实例避免污染共享材质
    /// </summary>
    public void EnableRectClipping(Renderer render, Rect _rect, Vector4 _clipRect)
    {
        if(render == null)
            return;

        // 保存原始的 sharedMaterials（重要：必须在清理之前保存）
        Material[] originalSharedMats = null;
        
        // 如果已经有记录，说明之前已经 Enable 过，复用之前保存的原始材质
        if (_m_dRendererMats.TryGetValue(render, out var existingInfo))
        {
            originalSharedMats = existingInfo.originalSharedMats;
        }
        else
        {
            // 第一次 Enable，保存当前的 sharedMaterials
            originalSharedMats = render.sharedMaterials;
        }
        
        // 清理旧的材质实例引用（不会移除 originalSharedMats 的记录）
        if (_m_dRendererMats.ContainsKey(render))
        {
            // 只释放材质引用，不删除 Renderer 记录（因为我们要保留 originalSharedMats）
            var matInfo = _m_dRendererMats[render];
            foreach (var matInstance in matInfo.instances)
            {
                if (matInstance.instanceMat != null)
                {
                    _RemoveMaterialReference(matInstance.instanceMat, matInstance.originalMat, matInstance.rect);
                }
            }
        }

        if (originalSharedMats == null || originalSharedMats.Length == 0)
            return;

        Material[] newMats = new Material[originalSharedMats.Length];
        List<MaterialInstance> matInstances = new List<MaterialInstance>();

        for (int i = 0; i < originalSharedMats.Length; i++)
        {
            Material sharedMat = originalSharedMats[i];
            if (sharedMat == null)
            {
                newMats[i] = null;
                continue;
            }

            // 获取或创建材质实例
            Material instanceMat = _GetOrCreateMaterialInstance(sharedMat, _rect, _clipRect);
            newMats[i] = instanceMat;

            // 记录材质实例信息
            MaterialInstance matInstance = new MaterialInstance
            {
                originalIndex = i,
                originalMat = sharedMat,
                instanceMat = instanceMat,
                rect = _rect
            };
            matInstances.Add(matInstance);

            // 增加引用计数
            _AddMaterialReference(instanceMat);
        }

        // 设置新材质数组
        render.materials = newMats;

        // 记录 Renderer 的材质映射和原始材质
        _m_dRendererMats[render] = new RendererMaterialInfo
        {
            originalSharedMats = originalSharedMats,
            instances = matInstances
        };
    }

    /// <summary>
    /// 禁用 Renderer 的矩形裁剪，恢复使用共享材质
    /// </summary>
    /// <param name="render">要恢复的 Renderer</param>
    /// <param name="_rect">裁剪矩形（仅用于日志/调试，实际释放不依赖此参数）</param>
    public void DisableRectClipping(Renderer render, Rect _rect)
    {
        if(render == null)
            return;

        // 检查是否有记录
        if (!_m_dRendererMats.ContainsKey(render))
        {
            // 该 Renderer 没有被 Enable 过，无需释放
            return;
        }

        // 获取保存的原始 sharedMaterials
        Material[] originalSharedMats = null;
        if (_m_dRendererMats.TryGetValue(render, out var matInfo))
        {
            originalSharedMats = matInfo.originalSharedMats;
        }
        
        // 释放材质实例引用（会自动清理引用计数为0的材质）
        _ReleaseRendererMaterials(render);

        // 恢复 Renderer 使用原始的 sharedMaterials
        if (originalSharedMats != null && originalSharedMats.Length > 0)
        {
            render.sharedMaterials = originalSharedMats;
        }
    }

    /// <summary>
    /// 获取或创建材质实例（同一个原材质+Rect组合复用同一个实例）
    /// </summary>
    private Material _GetOrCreateMaterialInstance(Material originalMat, Rect rect, Vector4 clipRect)
    {
        // 查找缓存
        if (!_m_dMats.TryGetValue(originalMat, out var rectDict))
        {
            rectDict = new Dictionary<Rect, Material>();
            _m_dMats[originalMat] = rectDict;
        }

        if (!rectDict.TryGetValue(rect, out Material instanceMat))
        {
            // 创建新的材质实例
            instanceMat = Material.Instantiate(originalMat);
            instanceMat.name = $"{originalMat.name}_ClipRect_{rect.GetHashCode()}";
            
            // 设置裁剪参数
            instanceMat.SetVector(ShaderPropertyMgr.g_ClipRect, clipRect);
            instanceMat.EnableKeyword("UNITY_UI_CLIP_RECT");
            
            // 缓存实例
            rectDict[rect] = instanceMat;
            
            // 初始化引用计数
            _m_dMatRefCount[instanceMat] = 0;
        }
        else
        {
            // 材质实例已存在，更新 ClipRect 参数（可能不同的 Renderer 使用相同 Rect 但 clipRect 值不同）
            instanceMat.SetVector(ShaderPropertyMgr.g_ClipRect, clipRect);
        }

        return instanceMat;
    }

    /// <summary>
    /// 增加材质引用计数
    /// </summary>
    private void _AddMaterialReference(Material mat)
    {
        if (mat == null)
            return;

        if (!_m_dMatRefCount.ContainsKey(mat))
            _m_dMatRefCount[mat] = 0;

        _m_dMatRefCount[mat]++;
    }

    /// <summary>
    /// 释放 Renderer 的材质实例引用
    /// </summary>
    private void _ReleaseRendererMaterials(Renderer render)
    {
        if (!_m_dRendererMats.TryGetValue(render, out var matInfo))
            return;

        // 减少每个材质实例的引用计数
        foreach (var matInstance in matInfo.instances)
        {
            if (matInstance.instanceMat != null)
            {
                _RemoveMaterialReference(matInstance.instanceMat, matInstance.originalMat, matInstance.rect);
            }
        }

        // 移除 Renderer 记录
        _m_dRendererMats.Remove(render);
    }

    /// <summary>
    /// 减少材质引用计数，引用为0时清理
    /// </summary>
    private void _RemoveMaterialReference(Material instanceMat, Material originalMat, Rect rect)
    {
        if (instanceMat == null)
            return;

        if (!_m_dMatRefCount.TryGetValue(instanceMat, out int refCount))
            return;

        refCount--;
        
        if (refCount <= 0)
        {
            // 引用计数归零，销毁材质实例
            _m_dMatRefCount.Remove(instanceMat);

            // 从缓存中移除
            if (_m_dMats.TryGetValue(originalMat, out var rectDict))
            {
                rectDict.Remove(rect);
                
                // 如果该原材质没有任何实例了，移除整个字典
                if (rectDict.Count == 0)
                {
                    _m_dMats.Remove(originalMat);
                }
            }

            // 销毁材质对象
            if (Application.isPlaying)
                Object.Destroy(instanceMat);
            else
                Object.DestroyImmediate(instanceMat);
        }
        else
        {
            _m_dMatRefCount[instanceMat] = refCount;
        }
    }

    /// <summary>
    /// 清理所有缓存（用于场景切换或资源清理）
    /// </summary>
    public void ClearAll()
    {
        // 销毁所有材质实例
        foreach (var rectDict in _m_dMats.Values)
        {
            foreach (var instanceMat in rectDict.Values)
            {
                if (instanceMat != null)
                {
                    if (Application.isPlaying)
                        Object.Destroy(instanceMat);
                    else
                        Object.DestroyImmediate(instanceMat);
                }
            }
        }

        _m_dMats.Clear();
        _m_dMatRefCount.Clear();
        _m_dRendererMats.Clear();
    }

    /// <summary>
    /// 清理无效的 Renderer 引用（Renderer 已被销毁但未调用 Disable）
    /// 建议定期调用或在场景切换时调用，避免内存泄漏
    /// </summary>
    public void CleanupInvalidRenderers()
    {
        // 收集所有无效的 Renderer
        List<Renderer> invalidRenderers = new List<Renderer>();
        
        foreach (var kvp in _m_dRendererMats)
        {
            if (kvp.Key == null)
            {
                invalidRenderers.Add(kvp.Key);
            }
        }
        
        // 释放无效 Renderer 的材质引用
        foreach (var render in invalidRenderers)
        {
            _ReleaseRendererMaterials(render);
        }
    }
}
