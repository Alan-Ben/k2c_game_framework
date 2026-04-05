
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 跑马灯展示item
    /// </summary>
    public class GGUIWndSubMarqueeItem : _ANPGGUIBasicSubWnd<GGUIMonoSubMarqueeItem>
    {
        //跑马灯数据
        private MarqueeInfo _m_marqueeInfo;

        public GGUIWndSubMarqueeItem(GGUIMonoSubMarqueeItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        /// <summary>
        /// 跑马灯数据
        /// </summary>
        public MarqueeInfo marqueeInfo { get { return _m_marqueeInfo; } }
        /// <summary>
        /// 文字的宽度
        /// </summary>
        public float width { get { return wnd != null ? wnd.txtContent.preferredWidth : 0f; } }
        /// <summary>
        /// 整个文字最左边的坐标
        /// </summary>
        public Vector2 leftPosition
        {
            get
            {
                if (wnd == null || wnd.txtContent == null || wnd.txtContent.rectTransform == null)
                    return Vector2.zero;
                return wnd.txtContent.rectTransform.anchoredPosition;
            }
            set
            {
                if (wnd == null || wnd.txtContent == null || wnd.txtContent.rectTransform == null)
                    return;
                wnd.txtContent.rectTransform.anchoredPosition = value;
            }
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_marqueeInfo = null;
        }

        protected override void _onReset()
        {
            _m_marqueeInfo = null;
        }

        protected override void _onDiscard()
        {
            _m_marqueeInfo = null;
        }

        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        public void setInfo(MarqueeInfo _info)
        {
            if (wnd == null || _info == null)
                return;

            _m_marqueeInfo = _info;
            ALUGUICommon.setLabelTxt(wnd.txtContent, _m_marqueeInfo.content);
        }
    }
}