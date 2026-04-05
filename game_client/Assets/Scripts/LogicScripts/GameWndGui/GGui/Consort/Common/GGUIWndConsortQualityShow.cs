using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子品质展示
    /// </summary>
    public class GGUIWndConsortQualityShow : _ATALBasicUISubWnd<GGUIMonoConsortQualityShow>
    {
        private NPGGuiWndTexture _m_wBgImg;
        private GGUISubWndQualityShowGo _m_wQualityShowGo;

        public GGUIWndConsortQualityShow(GGUIMonoConsortQualityShow _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgBg != null)
                _m_wBgImg = new NPGGuiWndTexture(wnd.imgBg);
            
            // 使用wnd.qualityShowGoMono字段初始化品质显示组件
            if (wnd.qualityShowGoMono != null)
                _m_wQualityShowGo = new GGUISubWndQualityShowGo(wnd.qualityShowGoMono);
        }

        protected override void _onDiscard()
        {
            _m_wBgImg?.discard();
            _m_wBgImg = null;
            
            _m_wQualityShowGo?.discard();
            _m_wQualityShowGo = null;
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wBgImg?.hideWnd();
            _m_wQualityShowGo?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wBgImg?.discardTexture();
            _m_wQualityShowGo?.resetWnd();
        }

        /// <summary>
        /// 设置展示的品质
        /// </summary>
        public void setQuality(EQuality _quality)
        {
            if (wnd == null)
                return;

            ConsortQualityShowConfig showCfg = wnd.getQualityShowConfig(_quality);
            if(showCfg == null)
                return;

            // 背景图
            if (_m_wBgImg != null && showCfg.bgImgIndex != null)
            {
                _m_wBgImg.showWnd();
                _m_wBgImg.setTexture(showCfg.bgImgIndex);
            }

            // 名字背景颜色
            if (wnd.nameBg != null)
                wnd.nameBg.color = showCfg.nameBgColor;

            // 设置品质特效GO
            if (_m_wQualityShowGo != null)
            {
                _m_wQualityShowGo.showWnd();
                _m_wQualityShowGo.setData(_quality);
            }
        }
    }
}