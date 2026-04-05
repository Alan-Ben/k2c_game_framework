using ALPackage;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星居民补充View
    /// </summary>
    public class MarsResidentReplenishView
    {
        private bool _m_bIsInited;
        private long _m_lInitSerialize;
        
        private GTDMonoMarsResidentReplenish _m_mono;

        private GGUIWndMarsResidentReplenishFollowerController _m_followerController;
        private GGUICommonFollowTarget _m_followTarget;

        public MarsResidentReplenishView()
        {
        }

        public void init(GTDMonoMarsResidentReplenish _mono)
        {
            if(_m_bIsInited)
                return;

            long initSerialize = _m_lInitSerialize = ALSerializeOpMgr.next();
            
            if (_mono == null)
            {
                Debug.LogError_EditorOnly($"[MarsResidentReplenishView] init error: _mono is null!");
                return;
            }

            _m_mono = _mono;
            
            // 绑定点击事件
            if (_m_mono.monoClick != null)
                _m_mono.monoClick.onClick += _onClickReplenish;

            // 创建Follow目标
            if (_m_mono.uiFollowTarget != null)
                _m_followTarget = new GGUICommonFollowTarget(_m_mono.uiFollowTarget, Vector3.zero);
            // 创建Follower控制器（使用配置的资源路径ID）
            if (_m_mono.followUiAssetPathId > 0)
                _m_followerController = new GGUIWndMarsResidentReplenishFollowerController(_m_mono.followUiAssetPathId, this);
            GGUIWndMarsHud.instance.regLoadDoneDelegate(() =>
            {
                if(!_m_bIsInited || _m_lInitSerialize != initSerialize)
                    return;
                
                GGUIWndMarsHud.instance.regInstance(_m_followTarget);
                GGUIWndMarsHud.instance.addController(_m_followTarget, _m_followerController);
            });

            WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_PEOPLE_IMMIGRANT_ADD, _onImmigrantDataChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_PEOPLE_IMMIGRANT_DEL, _onImmigrantDataChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_PEOPLE_IMMIGRANT_COUNT_CHG, _onImmigrantDataChg);

            _m_bIsInited = true;
            
            // 刷新显示
            refreshShow();
        }

        public void discard()
        {
            if(!_m_bIsInited)
                return;
            
            WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_PEOPLE_IMMIGRANT_ADD, _onImmigrantDataChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_PEOPLE_IMMIGRANT_DEL, _onImmigrantDataChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_PEOPLE_IMMIGRANT_COUNT_CHG, _onImmigrantDataChg);

            // 解绑点击事件
            if (_m_mono != null && _m_mono.monoClick != null)
                _m_mono.monoClick.onClick -= _onClickReplenish;

            // 清理Follow相关
            if (_m_followTarget != null)
            {
                GGUIWndMarsHud.instance.removeInstance(_m_followTarget);
                _m_followTarget.discard();
                _m_followTarget = null;
            }

            _m_followerController?.discard();
            _m_followerController = null;

            _m_mono = null;
            _m_lInitSerialize = ALSerializeOpMgr.next();
            _m_bIsInited = false;
        }

        /// <summary>
        /// 刷新状态
        /// </summary>
        public void refreshShow()
        {
            if(!_m_bIsInited || _m_mono == null)
                return;
            
            EMarsResidentReplenishState residentReplenishState = MarsUtil.getMarsResidentReplenishState();
            NPCommonEnumStatMutexShowInfo<EMarsResidentReplenishState>.setStat(_m_mono.replenishStateShowList, residentReplenishState);

            _m_followerController?.refreshWnd();
        }

        /// <summary>
        /// 点击居民补充回调
        /// </summary>
        internal void _onClickReplenish()
        {
            GCommon.enterUIMainNodeShow(ESysSceneType.MARS_RESIDENT_REPLENISH);
        }

        /// <summary>
        /// 火星移民数据变化
        /// </summary>
        private void _onImmigrantDataChg()
        {
            refreshShow();
        }
    }
}
