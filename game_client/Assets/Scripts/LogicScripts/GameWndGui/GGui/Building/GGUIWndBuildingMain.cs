
using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class GGUIWndBuildingMain : _ANPGGUIBasicResBarWnd<GGUIMonoBuildingMain>
    {
        [NotNull] public static GGUIWndBuildingMain instance { get { return _g_instance ??= new GGUIWndBuildingMain(); } }
        private static GGUIWndBuildingMain _g_instance;


        private GGUISubWndStageGoalBtn _m_stageGoalBtnWnd;
        private NPGGUISubWndMiniChat _m_miniChatWnd;
        private NPGGuiWndTexture _m_wVIPEntryIcon;
        private GGUISubWndLoverCollectBtn _m_loverCollectBtnWnd;
        
        public GGUIWndBuildingMain() 
            : base(EALUIWndLayer.NORMAL)
        {
        }
        

        protected override string _monoAssetPath { get { return GGUIMonoBuildingMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBuildingMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_CITY_MAIN_EXPAND_BAR, _onSimulateClickExpandBar);
            
            //由于每日任务有展示条件，某些操作可能会有新的每日任务可展示，这里打开界面时刷新每日任务红点
            NPPlayer.instance.dailyQuestComp.refreshDailyRewardRedTip();
            //由于可能跨天，组件没有实时计算，这里再刷新一次排行榜入口红点
            NPPlayer.instance.rankCommonComp.refreshRedTip();
            
            // 这里Loading对应GNodeRoom和GNodeCity中的Loading
            // 弹出需要在主城或者卧室弹出的弹出，这个时候保证ui跟td都已经完全显示了,并且云也完全散开了
            Loading.regHideDoneDelegate(() =>
            {
                WinMsg.SendMsg(WinMsgType.CUSTOM_RELOAD);
            });

            _m_stageGoalBtnWnd?.showWnd();
            _m_miniChatWnd?.showWnd();
            _m_wVIPEntryIcon?.showWnd();
            _m_loverCollectBtnWnd?.showWnd();

            //重置动画到第一帧
            wnd?.aniExpandBar?.sample(0);
            //刷新vip入口图标
            _refreshVIPEntryIcon();
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_CITY_MAIN_EXPAND_BAR, _onSimulateClickExpandBar);

            _m_stageGoalBtnWnd?.hideWnd();
            _m_miniChatWnd?.hideWnd();
            _m_wVIPEntryIcon?.hideWnd();
            _m_loverCollectBtnWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_stageGoalBtnWnd?.resetWnd();
            _m_miniChatWnd?.resetWnd();
            _m_wVIPEntryIcon?.discardTexture();
            _m_loverCollectBtnWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_stageGoalBtnWnd?.discard();
            _m_miniChatWnd?.discard();
            _m_wVIPEntryIcon?.discard();
            _m_loverCollectBtnWnd?.discard();
            _m_stageGoalBtnWnd = null;
            _m_miniChatWnd = null;
            _m_wVIPEntryIcon = null;
            _m_loverCollectBtnWnd = null;

            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnExpandBar, _onClickExpandBar);
            ALUGUICommon.combineBtnClick(wnd.btnContractBar, _onClickContractBar);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoStageGoalBtn != null)
                _m_stageGoalBtnWnd = new GGUISubWndStageGoalBtn(wnd.monoStageGoalBtn);
            if (wnd.monoMiniChat != null)
                _m_miniChatWnd = new NPGGUISubWndMiniChat(wnd.monoMiniChat);
            if (wnd.imgVIPEntryIcon != null)
                _m_wVIPEntryIcon = new NPGGuiWndTexture(wnd.imgVIPEntryIcon);
            if (wnd.monoLoverCollectBtn != null)
                _m_loverCollectBtnWnd = new GGUISubWndLoverCollectBtn(wnd.monoLoverCollectBtn);

            ALUGUICommon.combineBtnClick(wnd.btnExpandBar, _onClickExpandBar);
            ALUGUICommon.combineBtnClick(wnd.btnContractBar, _onClickContractBar);
        }

        //刷新vip入口图标
        private void _refreshVIPEntryIcon()
        {
            if (wnd == null)
                return;

            List<VipRefObj> vipRefList = GRefdataCoreMgr.instance.vipRefCore.refList;
            long curVIPLevel = NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.VIP_LVL);
            VipRefObj initShowVIPRef = GRefdataCoreMgr.instance.vipRefCore.getRef(wnd.initShowVIPLevel);
            VipRefObj targetVIPRef = null;
            if (curVIPLevel == 0 && wnd.initShowVIPLevel > 0 && initShowVIPRef != null)
            {
                targetVIPRef = initShowVIPRef;
            }
            else
            {
                for (int i = 0; i < vipRefList.Count; i++)
                {
                    if (vipRefList[i] == null)
                        continue;

                    //选中可领取的 或者 下一个vip等级 或者 是最后一个
                    if ((vipRefList[i].vip_lvl != 0 && !NPPlayer.instance.playerInfo.isGetVipReward((int)vipRefList[i].vip_lvl)) ||
                        (vipRefList[i].vip_lvl == curVIPLevel + 1) ||
                        (i == vipRefList.Count - 1))
                    {
                        targetVIPRef = vipRefList[i];
                        break;
                    }
                }
            }

            if (targetVIPRef != null && targetVIPRef.show_special_reward_list != null && targetVIPRef.show_special_reward_list.Count > 0)
            {
                _m_wVIPEntryIcon?.showWnd();
                _m_wVIPEntryIcon?.setTexture(GCommon.getItemTexIcon(targetVIPRef.show_special_reward_list[0].itemType, targetVIPRef.show_special_reward_list[0].itemId));

                ALUGUICommon.setGameObjEnable(wnd.goVIPHeroShowList, targetVIPRef.show_special_reward_list[0].itemType == ENPItemType.HERO);
                ALUGUICommon.setGameObjEnable(wnd.goVIPConsortShowList, targetVIPRef.show_special_reward_list[0].itemType == ENPItemType.CONSORT);
            }
        }

        //节点变更
        private void _onNodeChg()
        {
        }

        //点击展开bar按钮
        private void _onClickExpandBar(GameObject _go)
        {
            if (wnd == null || wnd.aniExpandBar == null)
                return;

            wnd.aniExpandBar.forcePlay();
        }

        //点击收缩bar按钮
        private void _onClickContractBar(GameObject _go)
        {
            if (wnd == null || wnd.aniContractBar == null)
                return;

            wnd.aniContractBar.forcePlay();
        }

        // 模拟点击展开bar按钮
        private void _onSimulateClickExpandBar(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            string str = _objects[0] as string;
            if (string.IsNullOrEmpty(str))
                return;

            bool isExpand = ALCommon.GetBool(str);
            if (isExpand)
                _onClickExpandBar(null);
            else
                _onClickContractBar(null);
        }
    }
}