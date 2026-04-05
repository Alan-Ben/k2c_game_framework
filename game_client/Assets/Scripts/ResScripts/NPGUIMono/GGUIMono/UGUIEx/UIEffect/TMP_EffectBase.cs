using System;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

[ExecuteAlways]
[RequireComponent(typeof(TMP_Text))]
public abstract class TMP_EffectBase : MonoBehaviour
{
    protected TMP_Text m_textComponent;
    protected bool m_hasTextChanged;
    
    [SerializeField] protected bool autoUpdate = true;
    
    protected virtual void Awake()
    {
        m_textComponent = GetComponent<TMP_Text>();
    }
    
    protected virtual void OnEnable()
    {
        if (m_textComponent != null)
        {
            // 订阅文本改变事件
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
        }
    }
    
    protected virtual void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
    }
    
    protected virtual void LateUpdate()
    {
        if (m_textComponent != null && autoUpdate)
        {
            if (m_hasTextChanged || ShouldUpdateContinuously())
            {
                ApplyEffect();
                m_hasTextChanged = false;
            }
        }
    }
    
    private void OnTextChanged(Object obj)
    {
        if (obj == m_textComponent)
        {
            m_hasTextChanged = true;
        }
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        m_hasTextChanged = true;
    }
#endif

    protected virtual bool ShouldUpdateContinuously()
    {
        return false; // 子类可以重写决定是否需要持续更新
    }
    
    public virtual void ApplyEffect()
    {
        if (m_textComponent == null) return;
        
        m_textComponent.ForceMeshUpdate();
        
        ModifyMesh();
        
        m_textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }
    
    protected abstract void ModifyMesh();
}