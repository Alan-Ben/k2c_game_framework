using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 宴会入口界面
    /// </summary>
    public class GGUIWndDinnerEnterMain : _ATALBasicUIWnd<GGUIMonoDinnerEnterMain>
    {
        private static GGUIWndDinnerEnterMain _g_instance;

        public static GGUIWndDinnerEnterMain instance
        {
            get
            {
                if (null == _g_instance)
                {
                    _g_instance = new GGUIWndDinnerEnterMain();
                }

                return _g_instance;
            }
        }
        
        private GDinnerInfo _m_dinnerInfo;
        private int _m_timeDownSer;

        public GGUIWndDinnerEnterMain() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoDinnerEnterMain.assetPath; }
        protected override string _monoObjName { get => GGUIMonoDinnerEnterMain.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_HOLD_DINNER, _simulateClickHoldDinner);
            WinMsg.RegisterMsgAct(WinMsgType.ON_DINNER_CHG, _refreshWnd);
            WinMsg.RegisterMsgAct(WinMsgType.ON_MY_DINNER_ADD, _refreshWnd);
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_HOLD_DINNER, _simulateClickHoldDinner);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_DINNER_CHG, _refreshWnd);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_MY_DINNER_ADD, _refreshWnd);

        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _clickBtnClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnMyDinner, _clickMyDinner);
            ALUGUICommon.uncombineBtnClick(wnd.btnJoinDinner, _clickJoinDinner);
            ALUGUICommon.uncombineBtnClick(wnd.btnRank, _clickDinnerRank);
            ALUGUICommon.uncombineBtnClick(wnd.btnRecord, _clickDinnerRecord);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnClose, _clickBtnClose);
            ALUGUICommon.combineBtnClick(wnd.btnMyDinner, _clickMyDinner);
            ALUGUICommon.combineBtnClick(wnd.btnJoinDinner, _clickJoinDinner);
            ALUGUICommon.combineBtnClick(wnd.btnRank, _clickDinnerRank);
            ALUGUICommon.combineBtnClick(wnd.btnRecord, _clickDinnerRecord);
        }
        private void _clickBtnClose(GameObject obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_DINNER_ENTER_MAIN);
        }

        private void _clickMyDinner(GameObject obj)
        {
            if (NPPlayer.instance.dinnerComp.hasDinnerOpen)
            {
                GDinnerInfo dinnerInfo = new GDinnerInfo(NPPlayer.instance.dinnerComp.dinnerInstanceId);
                GCommon.enterDinner(dinnerInfo, null, null, () => { });
            }
            else
            {
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndDinnerCreate.instance, GGUIWndDinnerCreate.instance.showWnd, UINodeTagConst.C_DINNER_CREATE);
            }
        }
        
        private void _clickJoinDinner(GameObject obj)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndDinnerListMain.instance, GGUIWndDinnerListMain.instance.showWnd, UINodeTagConst.C_DINNER_LIST);
        }
        
        //点击排行榜
        private void _clickDinnerRank(GameObject obj)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndDinnerRank.instance, GGUIWndDinnerRank.instance.showWnd, UINodeTagConst.C_DINNER_RANK);

        }
        
        private void _clickDinnerRecord(GameObject obj)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndDinnerLogMain.instance, GGUIWndDinnerLogMain.instance.showWnd, UINodeTagConst.C_DINNER_LOG);
        }

        private void _refreshWnd()
        {
            _m_dinnerInfo = null;
            _refreshMyDinner();
            
            if (NPPlayer.instance.dinnerComp.hasDinnerOpen)
            {
                if (_m_dinnerInfo == null || _m_dinnerInfo.dinnerId != NPPlayer.instance.dinnerComp.dinnerInstanceId)
                {
                    _m_dinnerInfo = new GDinnerInfo(NPPlayer.instance.dinnerComp.dinnerInstanceId);
                    _m_dinnerInfo.regDetailInfo((info) =>
                    {
                        _refreshMyDinner();
                    }, () => { _m_dinnerInfo = null;});
                }
            }
            
            _dealOfflineReward();
            NPPlayer.instance.dinnerComp.setHasEnterDinner();
        }


        private void _refreshMyDinner()
        {
            if(wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.myDinnerOpenShowList, _m_dinnerInfo != null);
            ALUGUICommon.setGameObjEnable(wnd.myDinnerOpenHideList, _m_dinnerInfo == null);

            if (_m_dinnerInfo != null && _m_dinnerInfo.dinnerTypeRef != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtMyDinnerName, TextTranslate.instance.getLanguage(_m_dinnerInfo.dinnerTypeRef.name));
                ALUGUICommon.setLabelTxt(wnd.txtMyDinnerCd,  TimeUtil.millisecondsToTime_hms(_m_dinnerInfo.getRemainTimeMs()));
                ALUGUICommon.setLabelTxt(wnd.txtMyDinnerSeat, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num,_m_dinnerInfo.joinerCount, _m_dinnerInfo.dinnerTypeRef.default_seat_num));
            }
            
            _m_timeDownSer = ALSerializeOpMgr.next();
            _refreshTimeDown(_m_timeDownSer);
        }

        /// <summary>
        /// 刷新倒计时
        /// </summary>
        private void _refreshTimeDown(int _timeDownSer)
        {
            if (null == wnd)
                return;
            if (null == _m_dinnerInfo)
                return;
            
            long leftTimeMs = _m_dinnerInfo.getRemainTimeMs();
            long hour = leftTimeMs / (1000 * 60 * 60);
            long minute = (leftTimeMs / (1000 * 60)) % 60;
            long second = (leftTimeMs / 1000) % 60;
            string cdTxt =  TextTranslate.instance.getLanguage(TransKeyConst.dinner_enterTimestamp_h_m_s, hour, minute, second);

            ALUGUICommon.setLabelTxt(wnd.txtMyDinnerCd,  cdTxt);
            if (_timeDownSer != _m_timeDownSer)
                return;
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                _refreshTimeDown(_timeDownSer);
            },1f);
        }
        
        /// <summary>
        /// 模拟点击举办宴会
        /// </summary>
        private void _simulateClickHoldDinner()
        {
            GCommon.enterUIMainNodeShow(ESysSceneType.DINNER_MAIN);
        }

        private void _dealOfflineReward()
        {
            if (!NPPlayer.instance.dinnerComp.hasOwenrReward)
                return;
            NPPlayer.instance.dinnerComp.reqTakeOpenReward((_info) =>
            {
                NPUINoticeMgr.instance.addDealer(new NPNoticeDealer_DinnerCreateResult(_info.getResult()));
            });
        }
    }
}