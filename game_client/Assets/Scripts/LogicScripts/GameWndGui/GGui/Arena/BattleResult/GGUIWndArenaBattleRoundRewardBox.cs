using System;
using ALPackage;
using NPCommon;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场回合连胜奖励宝箱
    /// </summary>
    public class GGUIWndArenaBattleRoundRewardBox : _ATALBasicUISubWnd<GGUIMonoArenaBattleRoundRewardBox>
    {
        //奖励道具
        private NPGGUIWndCommonItem _m_wItem;
        //点击宝箱
        private Action _m_aOnClickBox;
        //是否可以点击宝箱
        private Func<bool> _m_fCanClickBox;

        public GGUIWndArenaBattleRoundRewardBox(GGUIMonoArenaBattleRoundRewardBox _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wItem?.discard();
            _m_wItem = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickBox);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoItem != null)
                _m_wItem = new NPGGUIWndCommonItem(wnd.monoItem);

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickBox);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(NPCommon_ItemInfo _itemInfo, Action _onClickBox, Func<bool> _canClickBox)
        {
            if (wnd == null)
                return;

            _m_aOnClickBox = _onClickBox;
            _m_fCanClickBox = _canClickBox;

            //设置道具
            if (_m_wItem != null)
            {
                _m_wItem.showWnd();
                _m_wItem.setItem(_itemInfo.toCommonItemData());
            }

            //默认隐藏道具
            ALUGUICommon.setGameObjEnable(wnd.goSelectShowList,false);
            ALUGUICommon.setGameObjEnable(wnd.goSelectHideList,true);
        }

        /// <summary>
        /// 设置打开宝箱
        /// </summary>
        public void setOpenBox()
        {
            _onClickBox(null);
        }

        //点击宝箱
        private void _onClickBox(GameObject _go)
        {
            if (wnd == null || (_m_fCanClickBox != null && _m_fCanClickBox()))
                return;

            //显示道具
            ALUGUICommon.setGameObjEnable(wnd.goSelectShowList, true);
            ALUGUICommon.setGameObjEnable(wnd.goSelectHideList, false);

            _m_aOnClickBox?.Invoke();
        }
    }
}
