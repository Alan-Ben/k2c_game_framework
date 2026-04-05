using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class SevenDayLoginBtnWnd
    {
        private SevenDayLoginBtnMono wnd = null;
        private List<NPGGUIWndCommonItem> _m_rewardItemWnd = null;
        private ESevenDayLoginBtnState _m_curState = ESevenDayLoginBtnState.None;
        public SevenDayLoginBtnWnd(SevenDayLoginBtnMono mono)
        {
            wnd = mono;
            if (wnd != null)
            {
                ALUGUICommon.combineBtnClick(wnd.btn, _onBtnClick);
                ALUGUICommon.combineBtnClick(wnd.btnShowTips, _onBtnShowTipClick);
                if (wnd.rewardItems != null)
                {
                    _m_rewardItemWnd = new List<NPGGUIWndCommonItem>();
                    foreach (var item in wnd.rewardItems)
                    {
                        _m_rewardItemWnd.Add(new NPGGUIWndCommonItem(item));
                    }
                }
            }
            
        }

        
        public void discard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btn, _onBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnShowTips, _onBtnShowTipClick);
            }

            if (_m_rewardItemWnd != null)
            {
                foreach (NPGGUIWndCommonItem rewardWnd in _m_rewardItemWnd)
                {
                    rewardWnd?.discard();
                }
                _m_rewardItemWnd.Clear();
            }
            _m_rewardItemWnd = null;
            wnd = null;
        }

        public void refresh()
        {
            if(wnd == null)
                return;
            long loginDayCount = NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.SEVEN_DAYS_LOGIN_COUNT);

            if (wnd.day <= loginDayCount)
            {
                if(NPPlayer.instance.sevenDayLoginComp.isHadDrawReward(wnd.day))
                    _m_curState = ESevenDayLoginBtnState.HasGet;
                else
                    _m_curState = ESevenDayLoginBtnState.CanGet;
            }
            else if (wnd.day == loginDayCount + 1)
            {
                _m_curState = ESevenDayLoginBtnState.NextGet;
            }
            else
            {
                _m_curState = ESevenDayLoginBtnState.None;
            }

            SevenDayLoginRefObj sevenDayLoginRef = GRefdataCoreMgr.instance.sevenDayLoginRefCore.getRef(wnd.day);

            if (sevenDayLoginRef != null && _m_rewardItemWnd != null)
            {
                //这个列表展示的是同一个item,只是为了用于做多状态显示，加个list，显隐不同状态的item
                foreach (NPGGUIWndCommonItem rewardWnd in _m_rewardItemWnd)
                {
                    if (rewardWnd != null)
                    {
                        rewardWnd.showWnd();
                        rewardWnd.setItem(sevenDayLoginRef.show_reward_item);
                    }
                }
            }
            NPCommonEnumStatInfo<ESevenDayLoginBtnState>.setStat(wnd.statInfos, _m_curState);
        }

        private void _onBtnClick(GameObject _)
        {
            if(_m_curState != ESevenDayLoginBtnState.CanGet)
                return;
            if (wnd != null) GGUIWndSevenDayLogin.instance.onBtnClick(wnd.day);
        } 
        private void _onBtnShowTipClick(GameObject _go)
        {
            if (_go == null || wnd == null || wnd.rewardItems == null)
                return;
            SevenDayLoginRefObj sevenDayLoginRef = GRefdataCoreMgr.instance.sevenDayLoginRefCore.getRef(wnd.day);

            if (sevenDayLoginRef != null && sevenDayLoginRef.show_reward_item != null)
                QueueMgr.instance.AddNode(new GNodeCommonToolTip_Title_Text(wnd.tipsUIPathId,
                    sevenDayLoginRef.show_reward_item.getItemName(),
                    sevenDayLoginRef.show_reward_item.getItemDesc(), _go.GetComponent<RectTransform>(), wnd.detailIntervalX, wnd.detailIntervalY));
        }
        
    }
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndSevenDayLogin : _ATALBasicUIWnd<GGUIMonoSevenDayLogin>
    {
        private static GGUIWndSevenDayLogin _g_instance = new GGUIWndSevenDayLogin();
    
        public static GGUIWndSevenDayLogin instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndSevenDayLogin();
                return _g_instance;
            }
        }
        
        public List<SevenDayLoginBtnWnd> btnWndList = new List<SevenDayLoginBtnWnd>();
        private int _m_timeDownSer;
    
        public GGUIWndSevenDayLogin() : base(EALUIWndLayer.ADDITION)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoSevenDayLogin.assetPath; }
        protected override string _monoObjName { get => GGUIMonoSevenDayLogin.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
    
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
        }
    
        protected override void _onReset()
        {
            
        }
    
        protected override void _onDiscard()
        {
            if (btnWndList != null)
            {
                foreach (var btn in btnWndList)
                {
                    if (btn != null) btn.discard();
                }

                btnWndList.Clear();
            }

            btnWndList = null;
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if (wnd.btnList != null)
            {
                btnWndList = new List<SevenDayLoginBtnWnd>();
                foreach (SevenDayLoginBtnMono btn in wnd.btnList)
                {
                    SevenDayLoginBtnWnd wnd = new SevenDayLoginBtnWnd(btn);
                    btnWndList.Add(wnd);
                }
            }
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;

            if (btnWndList != null)
            {
                foreach (var btn in btnWndList)
                {
                    if (btn != null) btn.refresh();
                }
            }
            ALUGUICommon.setGameObjEnable(wnd.hasGetSevenDayRewardShowList, NPPlayer.instance.sevenDayLoginComp.hasGetSevenDayReward());
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
            bool hasGetTodayAndBeforeReward = NPPlayer.instance.sevenDayLoginComp.hasGetTodayAndBeforeReward();
            bool hasGetAllReward = NPPlayer.instance.sevenDayLoginComp.hasGetAllReward();
            if (!hasGetTodayAndBeforeReward || hasGetAllReward)
            {
                ALUGUICommon.setGameObjEnable(wnd.hasCDShowGos, false);
                ALUGUICommon.setGameObjEnable(wnd.hasCDHidewGos, true);
                
                return;
            }
            ALUGUICommon.setGameObjEnable(wnd.hasCDShowGos, true);     
            ALUGUICommon.setGameObjEnable(wnd.hasCDHidewGos, false);

            //服务端时间
            long nowTime = FpsAndPingMgr.instance.serverTimeTag;
            DateTime dt = TimeUtil.FromUTCMilliseconds(nowTime);
            DateTime dt2 = new DateTime(dt.Year, dt.Month, dt.Day, 0,0,0).AddDays(1);
            long nextTime = TimeUtil.dateTime2Milliseconds(dt2);
            long remainTime =  nextTime - nowTime;
            string timeStr = TimeUtil.millisecondsToTime_hms(remainTime);
        
            ALUGUICommon.setLabelTxt(wnd.txtCD,  timeStr);
            if (_timeDownSer != _m_timeDownSer)
                return;
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                _refreshTimeDown(_timeDownSer);
            },1f);
        }
        
        public void onBtnClick(int day)
        {
            NPPlayer.instance.sevenDayLoginComp.reqDrawLoginCountReward(day, _suc =>
            {
                _refreshWnd();
            });
        }
    }
}