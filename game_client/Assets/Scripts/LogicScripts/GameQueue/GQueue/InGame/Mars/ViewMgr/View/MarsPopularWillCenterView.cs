using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星民意中心View
    /// </summary>
    public class MarsPopularWillCenterView
    {
        private bool _m_bIsInited;
        private long _m_lInitSerialize;
        
        private GTDMonoMarsPopularWillCenter _m_mono;

        private GGUIWndMarsPopularWillCenterFollowerController _m_followerController;
        private GGUICommonFollowTarget _m_followTarget;


        public MarsPopularWillCenterView()
        {
        }


        public void init(GTDMonoMarsPopularWillCenter _mono)
        {
            if (_m_bIsInited)
                return;

            long initSerialize = _m_lInitSerialize = ALSerializeOpMgr.next();
            
            if (_mono == null)
            {
                Debug.LogError_EditorOnly($"[MarsPopularWillCenterView] init error: _mono is null!");
                return;
            }

            _m_mono = _mono;
            
            // 绑定点击事件
            if (_m_mono.monoClick != null)
                _m_mono.monoClick.onClick += _onClickPopularWillCenter;

            // 创建Follow目标
            if (_m_mono.uiFollowTarget != null)
                _m_followTarget = new GGUICommonFollowTarget(_m_mono.uiFollowTarget, Vector3.zero);
            // 创建Follower控制器（使用配置的资源路径ID）
            if (_m_mono.followUiAssetPathId > 0)
                _m_followerController = new GGUIWndMarsPopularWillCenterFollowerController(_m_mono.followUiAssetPathId, this);
            GGUIWndMarsHud.instance.regLoadDoneDelegate(() =>
            {
                if(!_m_bIsInited || _m_lInitSerialize != initSerialize)
                    return;
                
                GGUIWndMarsHud.instance.regInstance(_m_followTarget);
                GGUIWndMarsHud.instance.addController(_m_followTarget, _m_followerController);
            });

            // // 注册民意相关消息
            // WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_LETTER_ADD, _onMarsPeopleWillChg);
            // WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_LETTER_UPDATE, _onMarsPeopleWillChg);
            // WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_LETTER_DEL, _onMarsPeopleWillChg);
            // WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_HELP_ADD, _onMarsPeopleWillChg);
            // WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_HELP_UPDATE, _onMarsPeopleWillChg);
            // WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_HELP_DEL, _onMarsPeopleWillChg);
            // WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_SATISFACTION_DREGREE_CHG, _onMarsPeopleWillChg);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_POPULAR_WILL_CENTER, _onClickPopularWillCenter);

            _m_bIsInited = true;
            
            // 刷新显示
            _refreshShow();
        }


        public void discard()
        {
            if (!_m_bIsInited)
                return;
            
            // // 注销消息
            // WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_LETTER_ADD, _onMarsPeopleWillChg);
            // WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_LETTER_UPDATE, _onMarsPeopleWillChg);
            // WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_LETTER_DEL, _onMarsPeopleWillChg);
            // WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_HELP_ADD, _onMarsPeopleWillChg);
            // WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_HELP_UPDATE, _onMarsPeopleWillChg);
            // WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_HELP_DEL, _onMarsPeopleWillChg);
            // WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_SATISFACTION_DREGREE_CHG, _onMarsPeopleWillChg);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_POPULAR_WILL_CENTER, _onClickPopularWillCenter);

            // 解绑点击事件
            if (_m_mono != null && _m_mono.monoClick != null)
                _m_mono.monoClick.onClick -= _onClickPopularWillCenter;

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
        private void _refreshShow()
        {
            if (!_m_bIsInited || _m_mono == null)
                return;
            
            _m_followerController?.refreshWnd();
        }


        /// <summary>
        /// 点击民意中心回调
        /// </summary>
        internal void _onClickPopularWillCenter()
        {
            if(_m_mono == null)
                return;

            bool hadFocusPos = false; //是否有聚焦位置
            Vector3 focusPos = Vector3.zero;
            Vector2 focusViewportPos = Vector2.zero;
            float focusScale = 1f;
            float focusTime = 0f;
            if (_m_mono.clickFocusPosition != null)
            {
                hadFocusPos = true;
                focusPos = _m_mono.clickFocusPosition.position;
                focusViewportPos = _m_mono.focusViewportPos;
                focusScale = _m_mono.focusScale;
                focusTime = _m_mono.focusTime;
            }
            
            // 打开民意界面
            QueueMgr.instance.AddNode(new GNodeMarsPeopleWill(EMarsPopularWillTabType.NONE,
                () =>
                {
                    if (!hadFocusPos) return;
                    MainAdditionMarsTDScene.instance.focusToPos(focusPos, focusViewportPos, focusScale, focusTime);
                },
                () =>
                {
                    if (!hadFocusPos) return;
                    MainAdditionMarsTDScene.instance.cancelThePosFocus(focusTime);
                }));
        }

        /// <summary>
        /// 民意变化消息回调
        /// </summary>
        private void _onMarsPeopleWillChg()
        {
            _refreshShow();
        }
    }
}