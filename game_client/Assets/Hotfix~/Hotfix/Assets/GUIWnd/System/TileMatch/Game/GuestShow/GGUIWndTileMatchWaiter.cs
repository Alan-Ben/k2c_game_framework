using System;
using ALPackage;
using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 服务员窗口，管理单个服务员的状态表现
    /// </summary>
    public class GGUIWndTileMatchWaiter : _AHotfixBaseSubPrefabWnd<GGUIMonoTileMatchWaiter>
    {
        private string _m_sAssetPathInfo;
        private string _m_sObjName;
        
        private int _m_iShowSerialId;

        public GGUIWndTileMatchWaiter(Transform _parent) : base(_parent)
        {
        }

        protected override string _monoAssetPath { get { return _m_sAssetPathInfo; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        
        protected override void _onWndInitDoneHotfix()
        {
        }
        
        protected override void _onDiscard()
        {
        }

        protected override void _onShowWnd()
        {
            // 默认进入IDLE状态
            _setState(ETileMatchWaiterState.IDLE);
        }

        protected override void _onHideWnd()
        {
            _m_iShowSerialId = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
        }

        public void setAssetLoadPath(NPCommonAssetPathInfo _assetPathInfo)
        {
            _m_sAssetPathInfo = _assetPathInfo?.asset_path;
            _m_sObjName = _assetPathInfo?.obj_name;
        }
        
        public void setAssetLoadPath(string _assetPathInfo, string _objName)
        {
            _m_sAssetPathInfo = _assetPathInfo;
            _m_sObjName = _objName;
        }
        
        /// <summary>
        /// 设置服务员状态
        /// </summary>
        /// <param name="_state">目标状态</param>
        public void setState(ETileMatchWaiterState _state, Action _onPlayDone = null)
        {
            _setState(_state, _onPlayDone);
        }

        /// <summary>
        /// 内部设置状态
        /// </summary>
        private void _setState(ETileMatchWaiterState _state, Action _onPlayDone = null)
        {
            if (hotfixWnd == null)
            {
                _onPlayDone?.Invoke();
                return;
            }

            switch (_state)
            {
                case ETileMatchWaiterState.None:
                    _onPlayDone?.Invoke();
                    break;
                
                case ETileMatchWaiterState.IDLE:
                    if(hotfixWnd.spineAni != null && hotfixWnd.idleShowAniConfig != null)
                        hotfixWnd.idleShowAniConfig.playAnimation(hotfixWnd.spineAni, _onPlayDone);
                    else
                        _onPlayDone?.Invoke();
                    break;
                
                case ETileMatchWaiterState.SUCCESS:
                    if(hotfixWnd.spineAni != null && hotfixWnd.successShowAniConfig != null)
                        hotfixWnd.successShowAniConfig.playAnimation(hotfixWnd.spineAni, _onPlayDone);
                    else
                        _onPlayDone?.Invoke();
                    break;
                
                case ETileMatchWaiterState.FAIL:
                    if(hotfixWnd.spineAni != null && hotfixWnd.failShowAniConfig != null)
                        hotfixWnd.failShowAniConfig.playAnimation(hotfixWnd.spineAni, _onPlayDone);
                    else
                        _onPlayDone?.Invoke();
                    break;
            }
        }
    }
}