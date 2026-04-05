using System;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子详细信息子窗口
    /// </summary>
    public class GGUIWndUnLockConsortSubMainInfoWnd : _ATALBasicUIWnd<GGUIMonoUnLockConsortSubMainInfoWnd>
    {
        private static GGUIWndUnLockConsortSubMainInfoWnd _g_instance;
        public static GGUIWndUnLockConsortSubMainInfoWnd instance { get { return _g_instance ??= new GGUIWndUnLockConsortSubMainInfoWnd(); } }

        private GGottenConsortInfo _m_consortShowInfo;//妃子展示信息
        
        private GGUISubWndUnlockConsortDetailInfo _m_wConsortDetailInfoWnd;//妃子详细信息子窗口
        
        public GGUIWndUnLockConsortSubMainInfoWnd() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoUnLockConsortSubMainInfoWnd.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoUnLockConsortSubMainInfoWnd.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoUnlockConsortDetailInfo != null)
                _m_wConsortDetailInfoWnd = new GGUISubWndUnlockConsortDetailInfo(wnd.monoUnlockConsortDetailInfo);
        }
        
        protected override void _onDiscard()
        {
            _m_wConsortDetailInfoWnd?.discard();
            _m_wConsortDetailInfoWnd = null;
        }
        
        protected override void _onShowWnd()
        {
            if (wnd != null)
            {
                _sampleAnimation(wnd.onlyShowActorAnim, wnd.onlyShowActorFuncOffAnimName, 1f);
            }
            
            WinMsg.RegisterMsg(WinMsgType.SHOW_CONSORT_BUSINESS_SKILL_UNLOCK, _showConsortBusinessSkillUnlock);
            WinMsg.RegisterMsg(WinMsgType.SHOW_CONSORT_STORY_UNLOCK, _showConsortStoryUnlock);

            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SHOW_CONSORT_BUSINESS_SKILL_UNLOCK, _showConsortBusinessSkillUnlock);
            WinMsg.UnregisterMsg(WinMsgType.SHOW_CONSORT_STORY_UNLOCK, _showConsortStoryUnlock);
            
            _m_wConsortDetailInfoWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wConsortDetailInfoWnd?.resetWnd();
        }

        public void setData(GGottenConsortInfo _consortShowInfo)
        {
            _m_consortShowInfo = _consortShowInfo;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(_m_consortShowInfo == null || wnd == null)
                return;

            if (_m_wConsortDetailInfoWnd != null)
            {
                _m_wConsortDetailInfoWnd.showWnd();
                _m_wConsortDetailInfoWnd.setData(_m_consortShowInfo);
            }
        }

        #region 仅展示形象功能

        /// <summary>
        /// 仅展示形象
        /// </summary>
        /// <param name="_show"></param>
        public void onlyShowActor(bool _show)
        {
            if(wnd == null)
                return;
            
            if (_show)
            {
                _playAnimation(wnd.onlyShowActorAnim, wnd.onlyShowActorFuncOnAnimName);
            }
            else
            {
                _playAnimation(wnd.onlyShowActorAnim, wnd.onlyShowActorFuncOffAnimName);
            }
        }

        #endregion

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="_anim"></param>
        /// <param name="_animationName"></param>
        private void _playAnimation(Animation _anim, string _animationName, Action _playDone = null)
        {
            if (_anim == null || string.IsNullOrEmpty(_animationName))
            {
                _playDone?.Invoke();
                return;
            }

            _anim.ForcePlay(_animationName, 0f, _playDone);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_anim"></param>
        /// <param name="_animationName"></param>
        private void _sampleAnimation(Animation _anim, string _animationName, float _normalizeTime)
        {
            if(_anim == null || string.IsNullOrEmpty(_animationName))
                return;
            
            _anim.Sample(_animationName, _normalizeTime);
        }

        /// <summary>
        /// 展示经验技能解锁弹窗
        /// </summary>
        private void _showConsortBusinessSkillUnlock(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 2 || !(_objs[0] is ConsortBusinessSkillRefObj _businessSkillRefObj) || !(_objs[1] is GGottenConsortInfo _consortInfo))
                return;
            
            NPUINoticeMgr.instance.addDealer(new NoticeDealer_ConsortBusinessSkillUnlock(_businessSkillRefObj, _consortInfo));
        }
        
        /// <summary>
        /// 展示故事解锁弹窗
        /// </summary>
        private void _showConsortStoryUnlock(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || !(_objs[0] is ConsortStoryRefObj _storyRefObj))
                return;
            
            NPUINoticeMgr.instance.addDealer(new NoticeDealer_ConsortStoryUnlock(_storyRefObj));
        }
    }
}