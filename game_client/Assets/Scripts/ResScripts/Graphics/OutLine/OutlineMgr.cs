
using System;
using GOE;
using UnityEngine;
using UnityEngine.Rendering;


public class OutlineMgr
{
    private static OutlineMgr _g_instance = new OutlineMgr();
    public static OutlineMgr instance
    {
        get
        {
            if (null == _g_instance)
                _g_instance = new OutlineMgr();
            return _g_instance;
        }
    }
    
#if NP_GAME
    /// <summary>
    /// 开启描边效果显示，退出的时候必须要关闭
    /// </summary>
    /// <param name="_renderer"></param>
    public void openOutlineShow(SkinnedMeshRenderer _renderer)
    {
        // if(_renderer == null || Game.instance.mainCamera.urpSetting == null || Game.instance.mainCamera.urpSetting.outlineRenderFeature == null
        //    || Game.instance.mainCamera.urpSetting.outlineRenderFeatureOverlay == null)
        //     return;
        // NPGame.instance.mainCamera.urpSetting.outlineRenderFeature.AddSkinRenderer(_renderer);
        // NPGame.instance.mainCamera.urpSetting.outlineRenderFeatureOverlay.AddSkinRenderer(_renderer);
    }

    /// <summary>
    /// 关闭描边效果
    /// </summary>
    /// <param name="_renderer"></param>
    public void closeOutlineShow(SkinnedMeshRenderer _renderer)
    {
        // if(_renderer == null || Game.instance.mainCamera.urpSetting == null || Game.instance.mainCamera.urpSetting.outlineRenderFeature == null
        //    || Game.instance.mainCamera.urpSetting.outlineRenderFeatureOverlay == null)
        //     return;
        // NPGame.instance.mainCamera.urpSetting.outlineRenderFeature.RemoveSkinRenderer(_renderer);
        // NPGame.instance.mainCamera.urpSetting.outlineRenderFeatureOverlay.RemoveSkinRenderer(_renderer);
    }

    /// <summary>
    /// 开启贴图的描边效果显示，退出的时候必须要关闭
    /// </summary>
    /// <param name="_renderer"></param>
    public void openSpriteOutlineShow(Transform _targetGo, NPGGoIndex _outLineGoIndex, Action<GameObject> _onPop)
    {
        if(null == _targetGo || !_outLineGoIndex.isValid())
            return;
        
        GGoIndexCacheMgr.instance.popItem(_outLineGoIndex, (_outlinePrefab) =>
        {
            if (null == _outlinePrefab || null == _outlinePrefab.transform)
            {
                if (null != _onPop)
                    _onPop(_outlinePrefab);
                return;
            }
        
            _outlinePrefab.transform.SetParent(_targetGo);
            _outlinePrefab.transform.localPosition = Vector3.zero;
            _outlinePrefab.transform.localRotation = Quaternion.identity;
            _outlinePrefab.transform.localScale = Vector3.one;

            if (null != _onPop)
                _onPop(_outlinePrefab);
        });
    }

    /// <summary>
    /// 关闭贴图的描边效果
    /// </summary>
    public void closeSpriteOutlineShow(NPGGoIndex _outLineGoIndex, GameObject _outlinePrefab)
    {
        GGoIndexCacheMgr.instance.pushbackItem(_outLineGoIndex, _outlinePrefab);
    }
    
#endif
}
