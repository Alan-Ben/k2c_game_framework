using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 到达火星着陆选择界面
    /// </summary>
    public class GGUIWndMarsLandingSelect : _ANPGGUIBasicResBarWnd<GGUIMonoMarsLandingSelect>
    {
        private static GGUIWndMarsLandingSelect _g_instance;
        public static GGUIWndMarsLandingSelect instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndMarsLandingSelect();
                return _g_instance;
            }
        }

        // 选中的对话id
        private long _m_lDialogueId;
        // 是否已经选择
        private bool _m_bIsSelect;
        // 着陆地点列表
        private List<GGUIWndMarsLandingAreaItem> _m_lLandingAreaList;
        // 着陆选择回调
        private Action _m_aOnSelectLanding;

        public GGUIWndMarsLandingSelect() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoMarsLandingSelect.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsLandingSelect.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override bool isShowAniPlayOnlyOne { get { return true; } }
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_LANDING_CONFIRM, _onSimulateClickLandingConfirm);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_LANDING_CONFIRM, _onSimulateClickLandingConfirm);
            _m_bIsSelect = false;
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            if (_m_lLandingAreaList != null)
            {
                for (int i = 0; i < _m_lLandingAreaList.Count; i++)
                {
                    _m_lLandingAreaList[i]?.discard();
                }
                _m_lLandingAreaList.Clear();
                _m_lLandingAreaList = null;
            }

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnConfirm, _onClickConfirm);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_bIsSelect = false;
            _m_lLandingAreaList = new List<GGUIWndMarsLandingAreaItem>();
            if (wnd.landingAreaList != null)
            {
                for (int i = 0; i < wnd.landingAreaList.Count; i++)
                {
                    GGUIWndMarsLandingAreaItem areaItem = new GGUIWndMarsLandingAreaItem(wnd.landingAreaList[i]);
                    areaItem.onClickItem += _onClickItem;
                    areaItem.setSelect(false);
                    _m_lLandingAreaList.Add(areaItem);
                }
            }

            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onClickConfirm);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_onSelectLanding"></param>
        public void setInfo(Action _onSelectLanding)
        {
            _m_aOnSelectLanding = _onSelectLanding;
            _m_bIsSelect = false;
            if (_m_lLandingAreaList != null)
            {
                for (int i = 0; i < _m_lLandingAreaList.Count; i++)
                {
                    _m_lLandingAreaList[i]?.setSelect(false);
                }
            }
        }

        //点击选择item
        private void _onClickItem(GGUIWndMarsLandingAreaItem _item)
        {
            if (_item == null || _m_lLandingAreaList == null)
                return;

            for (int i = 0; i < _m_lLandingAreaList.Count; i++)
            {
                _m_lLandingAreaList[i]?.setSelect(false);
            }

            _m_bIsSelect = true;
            _item.setSelect(true);
            _m_lDialogueId = _item.wnd != null ? _item.wnd.dialogueId : 0;

            //发送引导触发消息
            WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.MARS_SELECT_LANDING_AREA);
        }

        //点击确认按钮
        private void _onClickConfirm(GameObject _go)
        {
            //未选中弹提示
            if (!_m_bIsSelect)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.marsGoTo_landingNeedSelectArea_none);
                return;
            }

            //发送埋点-点击确认着陆火星
            GCommon.sendStepReport(TraceConst.CLICK_LANDING_MARS);

            // 设置已选择着陆点
            NPPlayer.instance.playerInfoComp.clientDataRemarkInfo.setIsSelectLandingMarsArea();

            // 发送登录跑马灯
            NPPlayer.instance.marsComp.goToSubComponent.reqSendDoneMarquee();

            // 没有对话则直接关闭界面
            if (_m_lDialogueId <= 0)
            {
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_LANDING_SELECT);
                _m_aOnSelectLanding?.Invoke();
                return;
            }

            // 进入对话节点
            GCommon.enterDialogueNode(_m_lDialogueId, () =>
            {
                //对话结束关闭窗口
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_LANDING_SELECT);
                _m_aOnSelectLanding?.Invoke();
            });
        }

        //模拟点击火星着陆确认按钮
        private void _onSimulateClickLandingConfirm()
        {
            _onClickConfirm(null);
        }
    }
}