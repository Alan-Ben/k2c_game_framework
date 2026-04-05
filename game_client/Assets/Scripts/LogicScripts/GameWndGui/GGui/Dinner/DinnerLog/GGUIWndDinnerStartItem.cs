using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerEnum;
using Common.DinnerObj;
using NPCommon;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 宴会宾客历史item
    /// </summary>
    public class GGUIWndDinnerStartItem : _ANPGGUIBasicGridItemWnd<GGUIMonoDinnerStartItem>
    {
        private DinnerStartLogIdx _m_dinnerStartLog;
        private GDinnerTypeRefObj _m_dinnerTypeRef;
        
        private NPGGuiWndTexture _m_dinnerBanner;
        private GGUIWndConsortIconItem _m_consortCardItem;

        public GGUIWndDinnerStartItem(GGUIMonoDinnerStartItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
            _m_dinnerBanner?.discardTexture();
            _m_consortCardItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_dinnerBanner?.discard();
            _m_dinnerBanner = null;
            
            _m_consortCardItem?.discard();
            _m_consortCardItem = null;
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (null != wnd.texBanner)
                _m_dinnerBanner = new NPGGuiWndTexture(wnd.texBanner);

            if (null != wnd.consortIconItem)
                _m_consortCardItem = new GGUIWndConsortIconItem(wnd.consortIconItem);
            
            ALUGUICommon.combineBtnClick(wnd.btnClick, _clickInfo);
        }

        protected override void _resetGridItem()
        {
            _m_dinnerStartLog = null;
        }

        /// <summary>
        /// 点击按钮，查看宾客信息
        /// </summary>
        /// <param name="obj"></param>
        private void _clickInfo(GameObject obj)
        {
            //查看宾客信息
            NPPlayer.instance.dinnerComp.reqGetStartLogInfo(_m_dinnerStartLog.instanceId, _info =>
            {
                List<GDinnerGuestInfo> guestInfos = new List<GDinnerGuestInfo>();
                foreach (var joiner in _info.getInfo().getGuestLog())
                {
                    guestInfos.Add(new GDinnerGuestInfo(joiner));
                }
                GGUIWndDinnerGuestInfoList.instance.setInfo(guestInfos);
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndDinnerGuestInfoList.instance, GGUIWndDinnerGuestInfoList.instance.showWnd, UINodeTagConst.C_DINNER_GUEST_LIST);

            });
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        /// <param name="_dinnerJoinLog"></param>
        public void setInfo(DinnerStartLogIdx _dinnerJoinLog)
        {
            _m_dinnerStartLog = _dinnerJoinLog;
            _m_dinnerTypeRef = GRefdataCoreMgr.instance.dinnerTypeRefCore.getRef((long)_m_dinnerStartLog.dinnerId);
            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面显示
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            if (null == _m_dinnerStartLog)
                return;
            if (null == _m_dinnerTypeRef)
                return;

            DateTime msgDay = TimeUtil.FromUTCSeconds(_m_dinnerStartLog.startTs);
            ALUGUICommon.setLabelTxt(wnd.txtTime, TimeUtil.DateTime2StringMDYHMS(msgDay));
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_dinnerTypeRef.name));
            ALUGUICommon.setLabelTxt(wnd.txtJoinCount, TextTranslate.instance.getLanguage(TransKeyConst.dinner_common_joiner_count,_m_dinnerStartLog.joinerCount, _m_dinnerTypeRef.default_seat_num));
            ALUGUICommon.setLabelTxt(wnd.txtScore, _m_dinnerStartLog.gainScore);
            if (_m_dinnerStartLog.permitType == EDinnerPermitType.FAMILY)
            {
                _m_consortCardItem?.showWnd();
                _m_consortCardItem?.setInfo(new ConsortInfo(_m_dinnerStartLog.permitTypeId), 0); 
                ALUGUICommon.setLabelTxt(wnd.txtConsortDesc, TextTranslate.instance.getLanguage(TransKeyConst.dinner_start_log_item_consort_desc, _m_dinnerStartLog.getPermitConsortName()));
            }
            ALUGUICommon.setGameObjEnable(wnd.consortDinnerShowGos, _m_dinnerStartLog.permitType == EDinnerPermitType.FAMILY);
            ALUGUICommon.setGameObjEnable(wnd.consortDinnerHideGos, _m_dinnerStartLog.permitType != EDinnerPermitType.FAMILY);
            
            DinnerTypeShow.SetDinnerType(wnd.dinnerTypeShowList, _m_dinnerTypeRef.dinner_show_type);
            
            _m_dinnerBanner?.showWnd();
            _m_dinnerBanner?.setTexture(_m_dinnerTypeRef.banner);
            

        }
    }
}