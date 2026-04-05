
using System;
using System.Collections.Generic;
using ALPackage;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Object = UnityEngine.Object;

[Serializable]
public class UIMatAnimProperty
{
    public string name;
    public ShaderPropertyType type;
}
[Serializable]
public class UIMatAnimItem
{
    public MaskableGraphic graphic;
    public MeshRenderer meshRenderer;
    public List<UIMatAnimProperty> properties = new List<UIMatAnimProperty>();
    private Material m_Material;
    private MaterialPropertyBlock m_MaterialPropertyBlock;

    public void init()
    {
        m_MaterialPropertyBlock = new MaterialPropertyBlock();
        if (graphic == null)
            return;
        m_Material = Object.Instantiate(graphic.material);
        graphic.material = m_Material;
#if UNITY_EDITOR
        if (meshRenderer != null) meshRenderer.material = m_Material;
#endif
    }

    public void update()
    {
        if (graphic != null && meshRenderer != null&& meshRenderer.HasPropertyBlock())
        {
            meshRenderer.GetPropertyBlock(m_MaterialPropertyBlock);
            if(m_MaterialPropertyBlock == null)
                return;
            if(m_Material == null)
                return;
            foreach (var item in properties)
            {
                if(item == null)
                    continue;
                switch (item.type)
                {
                    case ShaderPropertyType.Color:
                        m_Material.SetColor(item.name, m_MaterialPropertyBlock.GetColor(item.name));
                        break;
                    case ShaderPropertyType.Float:
                    case ShaderPropertyType.Range:
                        m_Material.SetFloat(item.name,m_MaterialPropertyBlock.GetFloat(item.name));
                        break;
                    case ShaderPropertyType.Vector:
                        m_Material.SetVector(item.name, m_MaterialPropertyBlock.GetVector(item.name));
                        break;
                    case ShaderPropertyType.Int:
                        m_Material.SetInt(item.name, m_MaterialPropertyBlock.GetInt(item.name));
                        break;
                    case ShaderPropertyType.Texture:
                        m_Material.SetTexture(item.name, m_MaterialPropertyBlock.GetTexture(item.name));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
public class UIMatAnimV3 : MonoBehaviour
{
    public Animation anim;
    public List<UIMatAnimItem> uiMatAnimItems = new List<UIMatAnimItem>();
    private void Awake()
    {
        foreach (var item in uiMatAnimItems)
        {
            if (item != null) item.init();
        }
    }

    private void LateUpdate()
    {
        foreach (var uiMatAnimItem in uiMatAnimItems)
        {
            if (uiMatAnimItem != null) uiMatAnimItem.update();
        }
    }
   

  
}
#if UNITY_EDITOR
[CustomEditor(typeof(UIMatAnimV3))]
public class UIMatAnimV3Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Refresh"))
        {
            UIMatAnimV3 mono = (target as UIMatAnimV3);
            refresh(mono);
        }
 
    }
    
    public void refresh(UIMatAnimV3 mono)
    {
        if (mono != null && mono.anim != null)
        {
            Dictionary<GameObject, List<string>> goProperties = new Dictionary<GameObject, List<string>>();
            foreach (AnimationState item in mono.anim)
            {
                var clipName = item.name;
                AnimationClip clip = mono.anim.GetClip(clipName);
                SerializedObject psSource = new SerializedObject(clip);
                SerializedProperty floatCurves = psSource.FindProperty("m_FloatCurves");
                for (int j = floatCurves.arraySize - 1; j >= 0; j--)
                {
                    var element = floatCurves.GetArrayElementAtIndex(j);
                    string attribute = element.FindPropertyRelative("attribute").stringValue;
                    string path = element.FindPropertyRelative("path").stringValue;
                    Transform child = mono.transform.Find(path);
                    if (child != null)
                    {
                        List<string> properties;
                        if (!goProperties.TryGetValue(child.gameObject, out properties))
                        {
                            properties = new List<string>();
                            goProperties.Add(child.gameObject,properties);
                        }
                        properties.Add(attribute);
                    }
                }
            }
            
            foreach (var item in mono.uiMatAnimItems)
            {
                if (item != null && item.graphic != null)
                {
                    Shader shader = item.graphic.material.shader;
                    item.meshRenderer = item.graphic.AddMissingComponent<MeshRenderer>();
                    if (item.meshRenderer != null)
                    {                 
                        item.meshRenderer.enabled = false;
                        item.meshRenderer.sharedMaterial = item.graphic.material;
                    }
                    if (goProperties.TryGetValue(item.graphic.gameObject, out List<string> attributes))
                    {
                        List<string> validAttributes = new List<string>();
                        foreach (var attribute in attributes)
                        {
                            if(attribute == null)
                                continue;
                            string[] sp = attribute.Split('.');
                            
                            if (sp != null && sp.Length > 0 && sp[0] == "material")
                            {
                                string realProperty = sp[1];
                                if(validAttributes.Contains(realProperty))
                                    continue;
                                else
                                    validAttributes.Add(realProperty);
                            }
                        }

                        if (shader != null)
                        {
                            item.properties.Clear();
                            foreach (var validAttribute in validAttributes)
                            {
                                var index = shader.FindPropertyIndex(validAttribute);
                                if (index < 0)
                                {
                                    Debug.LogWarning("Property not found: " + validAttribute);
                                    continue;
                                }
                                var propertyName = shader.GetPropertyName(index);
                                ShaderPropertyType propertyType = shader.GetPropertyType(index);
                                item.properties.Add(new UIMatAnimProperty() { name = propertyName, type = propertyType });
                            }
                        }
                     
                    }
                }
            }
        }
    }
}
#endif