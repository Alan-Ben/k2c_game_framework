using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 可为空的大臣头像item
    /// </summary>
    public class GGUIWndHeroIconNullableItem : _ATALBasicUISubWnd<GGUIMonoHeroIconNullableItem>
    {
        private _IHeroCardShow _m_heroShowData;//骑士配置信息
        
        private GGUIWndHeroIconItem _m_wHeroInfo;//骑士信息

        public _IHeroCardShow heroShowData { get { return _m_heroShowData; } }

        public event Action<GGUIWndHeroIconNullableItem> onItemClick;

        public GGUIWndHeroIconNullableItem(GGUIMonoHeroIconNullableItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoHeroInfo != null)
                _m_wHeroInfo = new GGUIWndHeroIconItem(wnd.monoHeroInfo);
            
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onItemClick);
        }
        
        protected override void _onDiscard()
        {
            _m_wHeroInfo?.discard();
            _m_wHeroInfo = null;

            onItemClick = default;
            
            if(wnd != null)
                ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onItemClick);
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wHeroInfo?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wHeroInfo?.resetWnd();
        }
        
        public void setData(_IHeroCardShow _heroShowData)
        {
            _m_heroShowData = _heroShowData;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            if (_m_heroShowData == null)
            {
                ALUGUICommon.setGameObjEnable(wnd.noInfoShow, true);
                ALUGUICommon.setGameObjEnable(wnd.hasInfoShow, false);
                if(_m_wHeroInfo != null)
                    _m_wHeroInfo.hideWnd();
                return;
            }
            
            ALUGUICommon.setGameObjEnable(wnd.noInfoShow, false);
            ALUGUICommon.setGameObjEnable(wnd.hasInfoShow, true);

            if (_m_wHeroInfo != null)
            {
                _m_wHeroInfo.showWnd();
                _m_wHeroInfo.setData(_m_heroShowData);
            }
        }

        /// <summary>
        /// 当item被点击时
        /// </summary>
        /// <param name="_go"></param>
        private void _onItemClick(GameObject _go)
        {
            onItemClick?.Invoke(this);
        }
    }
}