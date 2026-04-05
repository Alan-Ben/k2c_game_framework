using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public enum GGUIWndAnecdoteBannerShowType
    {
        SPECIAL_EVENT,
        TO_BE_CONTINUED,
        EVENT_ENDED,
    }
    public class GGUIWndAnecdoteBannerShow : _ATALBasicUIWnd<GGUIMonoAnecdoteBannerShow>
    {
        // 特殊事件的单例
        [NotNull] public static GGUIWndAnecdoteBannerShow specialEventInstance { get { return _g_specialEventInstance ??= new GGUIWndAnecdoteBannerShow(GGUIMonoAnecdoteBannerShow.specialEventAssetPath, GGUIMonoAnecdoteBannerShow.specialEventObjName); } }
        private static GGUIWndAnecdoteBannerShow _g_specialEventInstance;
        // 事件未完待续的单例
        [NotNull] public static GGUIWndAnecdoteBannerShow toBeContinuedInstance { get { return _g_toBeContinuedInstance ??= new GGUIWndAnecdoteBannerShow(GGUIMonoAnecdoteBannerShow.toBeContinuedAssetPath, GGUIMonoAnecdoteBannerShow.toBeContinuedObjName); } }
        private static GGUIWndAnecdoteBannerShow _g_toBeContinuedInstance;
        // 事件结束的单例
        [NotNull] public static GGUIWndAnecdoteBannerShow eventEndedInstance { get { return _g_eventEndedInstance ??= new GGUIWndAnecdoteBannerShow(GGUIMonoAnecdoteBannerShow.eventEndedAssetPath, GGUIMonoAnecdoteBannerShow.eventEndedObjName); } }
        private static GGUIWndAnecdoteBannerShow _g_eventEndedInstance;
        
        
        private readonly string _m_assetPath;
        private readonly string _m_objName;
        
        private bool _m_isAnimating;
        private int _m_showSerialize;
        
        
        private GGUIWndAnecdoteBannerShow(string _assetPath, string _objName) 
            : base(EALUIWndLayer.ADDITION)
        {
            _m_assetPath = _assetPath;
            _m_objName = _objName;
        }
        
        
        protected override string _monoAssetPath { get { return _m_assetPath; } }
        protected override string _monoObjName { get { return _m_objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        public bool isAnimating { get { return _m_isAnimating; } }


        protected override void _onShowWnd()
        {
            if (wnd == null)
                return;

            float showAnimTime = 0;
            int serialize = _m_showSerialize = ALSerializeOpMgr.next();
            
            if (wnd.wndAnimation != null)
            {
                AnimationClip clip = wnd.wndAnimation.GetClip(wnd.showAniName);
                if (null != clip)
                {
                    showAnimTime = wnd.showAniTime;
                    if (showAnimTime < 0.001f)
                        showAnimTime = clip.length;
                    wnd.wndAnimation.Play(wnd.showAniName, PlayMode.StopAll);

                    _m_isAnimating = true;
                    ALCommonActionMonoTask.addMonoTask(() =>
                    {
                        if (serialize != _m_showSerialize)
                            return;
                        
                        _m_isAnimating = false;
                    }, showAnimTime);
                }
            }
            
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                if (serialize != _m_showSerialize)
                    return;
                
                QueueMgr.instance.DoUIRollBackByEsc();
            }, showAnimTime + wnd.autoCloseTime);
        }
        protected override void _onHideWnd()
        {
            _m_isAnimating = false;
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }


        private void _onCloseBtnClick(GameObject _obj)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }
    }
}