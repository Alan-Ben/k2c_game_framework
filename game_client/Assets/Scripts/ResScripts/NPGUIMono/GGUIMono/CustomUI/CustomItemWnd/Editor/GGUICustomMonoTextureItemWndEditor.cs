using System;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR

namespace GOE
{
	/// <summary>
	/// Texture的自定义扩展
	/// </summary>
	[CustomEditor(typeof(NPGGUIMonoCustomTextureMono))]
	[CanEditMultipleObjects]
	public class NPGGUIMonoCustomTextureMonoEditor : Editor
	{
	    private void OnEnable()
	    {
	        if(targets == null)
	            return;
	        foreach (var tag in targets)
	        {
	            NPGGUIMonoCustomTextureMono mono = tag as NPGGUIMonoCustomTextureMono;
	            if(mono == null)
	                continue;
            
	            if(mono.textureUIObj == null)
	            {
	                RawImage temp2 = mono.GetComponent<RawImage>();
	                mono.textureUIObj = temp2;
	            }
	            RawImage textureUIObj = mono.textureUIObj;
	            if(textureUIObj == null)
	                continue;

	            textureUIObj.RegisterDirtyVerticesCallback(_onTextureChange);
            
	            _checkTextureName(textureUIObj.texture, mono.textureIndex, mono.gameObject);
	        }
	    }

	    private void OnDisable()
	    {
	        if(targets == null)
	            return;
	        foreach (var tag in targets)
	        {
	            NPGGUIMonoCustomTextureMono mono = tag as NPGGUIMonoCustomTextureMono;
	            if(mono == null)
	                continue;

	            RawImage temp = mono.textureUIObj;
	            if(temp == null)
	                continue;

	            temp.UnregisterDirtyVerticesCallback(_onTextureChange);
	        }
	    }
    
	    /// <summary>
	    /// 校验设置index索引
	    /// </summary>
	    /// <param name="_texture"></param>
	    /// <param name="_index"></param>
	    /// <param name="_go"></param>
	    private void _checkTextureName(Texture _texture, NPGTextureIndex _index, GameObject _go)
	    {
	        if(_texture == null)
	            return;
	        //拆分字符串后进行读取
	        string[] strs = _texture.name.Split(new string[] {"||", ":", "_"},
	            StringSplitOptions.RemoveEmptyEntries);

	        if (strs.Length == 3)
	        {
	            int mainId = int.Parse(strs[1]);
	            int subId = int.Parse(strs[2]);

	            if (_index == null)
	                _index = new NPGTextureIndex();
	            if (_index.mainId != mainId || _index.subId != subId)
	            {
	                _index.mainId = mainId;
	                _index.subId = subId;
	                EditorUtility.SetDirty(_go);
	                AssetDatabase.SaveAssets();
	                PrefabStage stage = PrefabStageUtility.GetPrefabStage(_go);
	                if(stage != null)
	                {
	                    EditorSceneManager.MarkSceneDirty(stage.scene);
	                }
	            }
	        }
	    }
    
	    /// <summary>
	    /// 贴图替换的回调
	    /// </summary>
	    private void _onTextureChange()
	    {
	        if(targets == null)
	            return;
	        foreach (var tag in targets)
	        {
	            NPGGUIMonoCustomTextureMono mono = tag as NPGGUIMonoCustomTextureMono;
	            if(mono == null)
	                continue;
            
	            if(mono.textureUIObj == null)
	            {
	                RawImage temp2 = mono.GetComponent<RawImage>();
	                mono.textureUIObj = temp2;
	            }
	            RawImage textureUIObj = mono.textureUIObj;
	            if(textureUIObj == null)
	                continue;
	            _checkTextureName(textureUIObj.texture, mono.textureIndex, mono.gameObject);
	        }
	    }
	}
#endif
}