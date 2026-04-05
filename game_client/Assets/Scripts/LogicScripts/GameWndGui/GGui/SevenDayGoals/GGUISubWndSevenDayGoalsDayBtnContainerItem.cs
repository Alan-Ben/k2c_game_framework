using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndSevenDayGoalsDayBtnContainerItem : _ATALBasicUISubWnd<GGUIMonoSevenDayGoalsDayBtnContainerItem>
    {
        // 点击按钮后的回调
        private readonly Action<int> _m_onDayBtnClick;
        
        // 这个 item 表示的是第几天
        private int _m_day;
        // 当前服务端解锁到的天数
        private int _m_serverDayNow;
        // 当前选中的天数
        private int _m_selectedDay;
        
        
        public GGUISubWndSevenDayGoalsDayBtnContainerItem(GGUIMonoSevenDayGoalsDayBtnContainerItem _wnd, Action<int> _onDayBtnClick) : base(_wnd)
        {
            _m_onDayBtnClick = _onDayBtnClick;
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onBtnClicked);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onBtnClicked);
        }
        

        public void refreshWnd(int _day, int _serverDayNow, int _selectedDay)
        {
            _m_day = _day;
            _m_serverDayNow = _serverDayNow;
            _m_selectedDay = _selectedDay;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            // 设置日期的文本
            string dayText = TextTranslate.instance.getLanguage(TransKeyConst.sevenDayGoals_day_num, _m_day);
            ALUGUICommon.setLabelTxt(wnd.txtDay, dayText);
            ALUGUICommon.setLabelTxt(wnd.txtDayGray, dayText);
            ALUGUICommon.setLabelTxt(wnd.txtDayWhite, dayText);
            // 设置是否解锁
            ALUGUICommon.setGameObjEnable(wnd.listUnlockShow, false);
            ALUGUICommon.setGameObjEnable(wnd.listLockShow, false);
            ALUGUICommon.setGameObjEnable(_m_day > _m_serverDayNow ? wnd.listLockShow : wnd.listUnlockShow, true);
            // 设置是否选中
            ALUGUICommon.setGameObjEnable(wnd.listSelectShow, _m_day == _m_selectedDay);
            // 设置是否有红点
            ALUGUICommon.setGameObjEnable(wnd.listRedShow, NPPlayer.instance.sevenDayGoalsComp.getNeedShowRedTip(_m_day, ESevenDayGoalsRedTipType.NEW) ||
                                                           NPPlayer.instance.sevenDayGoalsComp.getNeedShowRedTip(_m_day, ESevenDayGoalsRedTipType.GIFT) ||
                                                           NPPlayer.instance.sevenDayGoalsComp.getNeedShowRedTip(_m_day, ESevenDayGoalsRedTipType.TASK));
        }


        private void _onBtnClicked(GameObject _)
        {
            bool isUnlock = _m_day <= _m_serverDayNow;
            if (!isUnlock)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.sevenDayGoals_unlockDaysLeft_num, _m_day - _m_serverDayNow));
                return;
            }
            //点击回调
            _m_onDayBtnClick?.Invoke(_m_day);
        }
    }
}