using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;
#if UNITY_EDITOR
using UnityEditor;
#endif

/**************
 * 设置本地资源加载的相关信息
 **/
public class ALSetLocalLoadUIMono : MonoBehaviour
{
    public string localResRootPath = "Resources";

    [ALHeader("是否优先加载本地资源的UI")]
    public bool useLocalUI;
    [ALHeader("是否优先加载本地资源的Prefab")]
    public bool useLocalGameObject;
    [ALHeader("是否加载本地资源的场景")]
    public bool uselocalScene;                  //Scene加载是否使用本地Scene，加载需要添加到Build Settings中
    [ALHeader("本地场景资源的路径")]
    public string localScenesPath = "Assets/Resources/GameRes/scenes";
    [ALHeader("是否加载本地的refdata数据")]
    public bool useLocalRefdata;
    [ALHeader("是否加载本地的视频数据")]
    public bool useLocalVideo;
#if AL_ILRUNTIME
    [ALHeader("是否加载本地的Hotfix.dll")]
    public bool useLocalHotfix;
#endif
    [ALHeader("是否显示本地加载的标记【默认会强制设置为true，这里提供编辑器修改渠道】")]
    public bool isShowLocalLoadedTag = true;

    [ALHeader("如果本地资源失效，是否加载远程资源，默认不加载")]
    public bool ifLocalDisableUseRemote = false;

    private GUIStyle _m_gsStyle;
    
#if UNITY_EDITOR
    private static void searchAndAddSceneInPath(string folderAssetPath)
    {
        List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();

        string[] guids = AssetDatabase.FindAssets("t:Scene", new[] {folderAssetPath});
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (!string.IsNullOrEmpty(path))
                editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(path, true));
        }
        EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
    }
#endif

    protected virtual void Start()
    {
        _m_gsStyle = new GUIStyle();

        _m_gsStyle.normal.background = null;
        _m_gsStyle.normal.textColor = Color.red;
        _m_gsStyle.fontSize = 40;

        isShowLocalLoadedTag = true;
#if UNITY_EDITOR
        if(uselocalScene)
            searchAndAddSceneInPath(localScenesPath);
#endif

        ALLocalResLoaderMgr.instance.setLoadFromLocal(localResRootPath, useLocalUI, useLocalGameObject, uselocalScene, useLocalRefdata, useLocalVideo
#if AL_ILRUNTIME
            , useLocalHotfix
#endif
            , ifLocalDisableUseRemote);
    }

    void OnGUI()
    {
        if (!isShowLocalLoadedTag)
            return;
        int height = 50;
        if(ALLocalResLoaderMgr.instance.isLoadUIFromLocal)
        {
            GUI.Label(new Rect(50, height, 150, 50), "使用本地GUI", _m_gsStyle);
            height += 50;
        }

        if(ALLocalResLoaderMgr.instance.isLoadGameObjectFromLocal)
        {
            GUI.Label(new Rect(50, height, 150, 50), "使用本地Game Object", _m_gsStyle);
            height += 50;
        }

        if(ALLocalResLoaderMgr.instance.isLoadSceneFromLocal)
        {
            GUI.Label(new Rect(50, height, 150, 50), "使用本地Scene", _m_gsStyle);
            height += 50;
        }

        if(ALLocalResLoaderMgr.instance.isLoadRefdataFromLocal)
        {
            GUI.Label(new Rect(50, height, 150, 50), "使用本地Refdata对象", _m_gsStyle);
            height += 50;
        }
    }

    private void OnDisable()
    {
        ALLocalResLoaderMgr.instance.resetLoadFromLocal();
    }
}
