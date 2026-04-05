using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndAdultMarriedDetail : _ATNPGGUIWndCommonItemToolTip<GGUIMonoAdultMarriedDetail>
    {
        [NotNull] public static GGUIWndAdultMarriedDetail instance { get { return _g_instance ??= new GGUIWndAdultMarriedDetail(); } }
        private static GGUIWndAdultMarriedDetail _g_instance;


        private GGUISubWndChildInfo _m_adultInfoWnd;
        
        private AdultInfo _m_adultInfo;
        private RectTransform _m_followTarget;
        private Vector2 _m_interval;
        
        
        public GGUIWndAdultMarriedDetail() 
            : base(GGUIMonoAdultMarriedDetail.assetPath, GGUIMonoAdultMarriedDetail.objName)
        {
        }


        protected override void _onShowWnd()
        {
            base._onShowWnd();
            _m_adultInfoWnd?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            base._onHideWnd();
            _m_adultInfoWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            base._onReset();
            _m_adultInfoWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            base._onDiscard();
            _m_adultInfoWnd?.discard();
            _m_adultInfoWnd = null;
        }
        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
            if (wnd == null)
                return;

            if (wnd.monoAdultInfo != null)
                _m_adultInfoWnd = new GGUISubWndChildInfo(wnd.monoAdultInfo);
        }

        
        public void refreshWnd(AdultInfo _adultInfo, RectTransform _followTarget, Vector2 _interval)
        {
            _m_adultInfo = _adultInfo;
            _m_followTarget = _followTarget;
            _m_interval = _interval;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_adultInfo == null)
                return;
            
            _m_adultInfoWnd?.refreshWnd(_m_adultInfo);
            setPos(_m_followTarget, _m_interval.x, _m_interval.y);
        }
        
        
        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ADULT_MARRIED_DETAIL_TOOL_TIP);
        }
    }
}