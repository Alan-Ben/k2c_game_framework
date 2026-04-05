using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public abstract class _ANPGGUIBasicDiffPrefabSubWndContainer<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND> : _ATALUGUISubWndBasicContainer<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND>
        where _T_ITEM_MONO : _AALBasicUIWndMono
        where _T_CONTAINER_MONO : _TALUGUIMonoContainerWnd<_T_ITEM_MONO>
        where _T_ITEM_WND : _ATALBasicUISubWnd<_T_ITEM_MONO>
    {
        
        protected _ANPGGUIBasicDiffPrefabSubWndContainer(_T_CONTAINER_MONO _containerMono)
                : base(_containerMono)
        {
        }

        /// <summary>
        /// 在添加了一个子窗口的时候调用的事件函数
        /// </summary>
        /// <param name="_itemWnd"></param>
        protected override void _onAddItemWnd(_T_ITEM_WND _itemWnd)
        {

        }

        /// <summary>
        /// 通过ui_path_id,添加一个item
        /// </summary>
        /// <param name="_uiPathId"></param>
        /// <param name="_done"></param>
        public void addItemWndByPathId(long _uiPathId,Action<_T_ITEM_WND> _done)
        {
            ALAssetLoader<GameObject> assetLoader = new ALAssetLoader<GameObject>(GameResCore.instance, UIResPathAssistant.getAssetPath(_uiPathId), UIResPathAssistant.getObjName(_uiPathId));
            assetLoader.loadAsset((GameObject _go) =>
            {
                //判断是否为空
                if (null == _go)
                {
                    //报错
                    ALLog.Error($"Load Item Failed,PathId: {_uiPathId},objName:{UIResPathAssistant.getObjName(_uiPathId)}");
                    _done(addItemWnd());//保底有个ui上的预制体
                    return;
                }
                //获取对应的脚本
                _T_ITEM_MONO itemMono = _go.GetComponent<_T_ITEM_MONO>();
                if (null == itemMono)
                {
                    //报错
                    ALLog.Error($"Load Item Has no Mono: {UIResPathAssistant.getObjName(_uiPathId)}");
                    _done(addItemWnd());//保底有个ui上的预制体
                    return;
                }

                _done(addItemWnd(itemMono));
            });
        }
        
        /// <summary>
        /// 通过NPCommonAssetPathInfo,添加一个item
        /// </summary>
        /// <param name="_assetPathInfo"></param>
        /// <param name="_done"></param>
        public void addItemWndByAssetPathInfo(NPCommonAssetPathInfo _assetPathInfo, Action<_T_ITEM_WND> _done)
        {
            if (_assetPathInfo == null || !_assetPathInfo.enable)
            {
                ALLog.Error($"Load Item Failed, _assetPathInfo:{_assetPathInfo}");
                _done?.Invoke(null);
                return;
            }
            
            ALAssetLoader<GameObject> assetLoader = new ALAssetLoader<GameObject>(GameResCore.instance, _assetPathInfo.asset_path, _assetPathInfo.obj_name);
            assetLoader.loadAsset((GameObject _go) =>
            { 
                if (null == _go)
                {
                    //报错
                    ALLog.Error($"Load Item Failed,PathId: {_assetPathInfo.asset_path},objName:{_assetPathInfo.obj_name}");
                    _done?.Invoke(addItemWnd());//保底有个ui上的预制体
                    return;
                }
                
                _T_ITEM_MONO itemMono = _go.GetComponent<_T_ITEM_MONO>();
                if (null == itemMono)
                {
                    //报错
                    ALLog.Error($"Load Item Has no Mono: {_assetPathInfo.obj_name}");
                    _done?.Invoke(addItemWnd());//保底有个ui上的预制体
                    return;
                }
                
                _done?.Invoke(addItemWnd(itemMono));
            });
        }
    }
}
