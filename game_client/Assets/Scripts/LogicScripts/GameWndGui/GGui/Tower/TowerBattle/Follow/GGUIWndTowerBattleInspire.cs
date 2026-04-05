using System;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using Common.DinnerEnum;
using NPEnum;
using GOE.FollowItem;

namespace GOE
{
    /// <summary>
    /// 跟随窗口，
    /// </summary>
    public sealed class GGUIWndTowerBattleInspire : _ATALBasicUISubWnd<GGUIMonoTowerBattleInspire>
    {
        private GGUIWndHeroIconItem _m_wHeroInfo;//骑士信息
        public GGUIWndTowerBattleInspire(GGUIMonoTowerBattleInspire _wnd) : base(_wnd)
        {
            initWnd();
            hideWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onReset()
        {
            _m_wHeroInfo?.resetWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wHeroInfo?.hideWnd();
        }

        protected override void _onDiscard()
        {
            _m_wHeroInfo?.discard();
            _m_wHeroInfo = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoHeroInfo != null)
                _m_wHeroInfo = new GGUIWndHeroIconItem(wnd.monoHeroInfo);
        }

        /// <summary>
        /// 设置tip数据
        /// </summary>
        /// <param name="_icon"></param>
        /// <param name="_str"></param>
        public void setInfo(_IHeroCardShow _heroShowData, Transform _parent, string _desc)
        {
            if (wnd == null)
                return;
            wnd.transform.parent = _parent;
            wnd.transform.localPosition = Vector3.zero;
            wnd.transform.localScale = Vector3.one;
      
            ALUGUICommon.setLabelTxt(wnd.txtStr, TextTranslate.instance.getLanguage(_desc));
            if (_m_wHeroInfo != null)
            {
                _m_wHeroInfo.showWnd();
                _m_wHeroInfo.setData(_heroShowData);
            }
        }
        
    }
}