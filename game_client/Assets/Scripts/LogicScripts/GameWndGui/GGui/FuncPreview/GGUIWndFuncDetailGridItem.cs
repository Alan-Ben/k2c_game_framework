using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 功能列表item
    /// </summary>
    public class GGUIWndFuncDetailGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoFuncDetailGridItem>
    {
        //功能解锁信息
        private FuncUnlockInfo _m_funcUnlockInfo;
        //图标
        private NPGGuiWndTexture _m_wIcon;
        //点击选择
        private Action<GGUIWndFuncDetailGridItem> _m_aOnSelect;

        /// <summary>
        /// 功能解锁信息
        /// </summary>
        public FuncUnlockInfo funcUnlockInfo { get { return _m_funcUnlockInfo; } }
        /// <summary>
        /// 选中item
        /// </summary>
        public Action<GGUIWndFuncDetailGridItem> onSelect
        {
            get { return _m_aOnSelect; }
            set { _m_aOnSelect = value; }
        }

        public GGUIWndFuncDetailGridItem(GGUIMonoFuncDetailGridItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wIcon?.discardTexture();
        }

        protected override void _resetGridItem()
        {

        }

        protected override void _onDiscard()
        {
            _m_wIcon?.discard();
            _m_wIcon = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnSelect, _onClickSelect);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (wnd.imgIcon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.imgIcon);

            ALUGUICommon.combineBtnClick(wnd.btnSelect, _onClickSelect);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        public void setInfo(FuncUnlockInfo _info)
        {
            if (_info == null)
                return;

            _m_funcUnlockInfo = _info;
            _refreshWnd();
        }

        /// <summary>
        /// 设置选中
        /// </summary>
        /// <param name="_isSelect"></param>
        public void setSelect(bool _isSelect)
        {
            if (wnd == null || _m_funcUnlockInfo == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goListShowOnSelect, _isSelect);
            ALUGUICommon.setGameObjEnable(wnd.goListHideOnSelect, !_isSelect);

            //设置名称
            string nameStr = GCommon.addColorForRichText(_m_funcUnlockInfo.funcName, _isSelect ? wnd.selectTextColor : wnd.noSelectTextColor);
            ALUGUICommon.setLabelTxt(wnd.txtName, nameStr);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_funcUnlockInfo == null)
                return;

            //设置图标
            if (_m_wIcon != null)
            {
                _m_wIcon.showWnd();
                _m_wIcon.setTexture(_m_funcUnlockInfo.texIcon);
            }

            //是否解锁
            bool isUnlock = _m_funcUnlockInfo.isUnlock;
            //是否已领取奖励
            bool hasGetReward = _m_funcUnlockInfo.hasGetReward;

            //根据状态设置显隐
            ALUGUICommon.setGameObjEnable(wnd.goCanGetShowList, false);
            ALUGUICommon.setGameObjEnable(wnd.goLockShowList, false);
            ALUGUICommon.setGameObjEnable(wnd.goAlreadyGetShowList, false);
            if (isUnlock)
            {
                GGameCommonInfo.disgrayImage(wnd.lockGrayList);
                if(hasGetReward)
                    ALUGUICommon.setGameObjEnable(wnd.goAlreadyGetShowList, true);
                else
                    ALUGUICommon.setGameObjEnable(wnd.goCanGetShowList, true);
            }
            else
            {
                GGameCommonInfo.grayImage(wnd.lockGrayList);
                ALUGUICommon.setGameObjEnable(wnd.goLockShowList, true);
            }
        }

        //点击选择
        private void _onClickSelect(GameObject _go)
        {
            _m_aOnSelect?.Invoke(this);
        }
    }
}
