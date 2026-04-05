using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场战斗选择增益弹窗
    /// </summary>
    public class GGUIWndArenaBattleSelectBuff : _ANPGGUIBasicWnd<GGUIMonoArenaBattleSelectBuff>
    {
        private static GGUIWndArenaBattleSelectBuff _g_instance;
        public static GGUIWndArenaBattleSelectBuff instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndArenaBattleSelectBuff();
                return _g_instance;
            }
        }

        //增益道具列表
        private GGUIWndArenaBattleSelectBuffContainer _m_wBuffContainer;
        //是否正在处理购买增益道具
        private bool _m_bIsDealingBuyBuff;
        //显示序列号
        private long _m_lShowSerialize;

        public GGUIWndArenaBattleSelectBuff() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoArenaBattleSelectBuff.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoArenaBattleSelectBuff.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_bIsDealingBuyBuff = false;
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_bIsDealingBuyBuff = false;
            _m_wBuffContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wBuffContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wBuffContainer?.discard();
            _m_wBuffContainer = null;

            if (wnd == null)
                return; 

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoSelectBuffContainer != null)
            {
                _m_wBuffContainer = new GGUIWndArenaBattleSelectBuffContainer(wnd.monoSelectBuffContainer);
                _m_wBuffContainer.onClickBuy += _onClickBuffBuy;
            }


            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            ArenaBattleInfo battleInfo = NPPlayer.instance.arenaComp.arenaBattleInfo;
            if (battleInfo == null)
                return;

            //总增益万分比
            long totalBuffAddPer = battleInfo.getTotalBuffAddPer();
            //总血量
            long totalHp = battleInfo.getTotalPower();
            //是否是第一轮
            bool isFirst = battleInfo.getIsFirstBattleRound();
            //获取buff道具id列表
            List<long> buffIdList = null;
            if (isFirst)
                buffIdList = GRefdataCoreMgr.instance.npGeneral.arena_initial_choose_buff_list;
            else
                buffIdList = GRefdataCoreMgr.instance.npGeneral.arena_choose_buff_list;

            //刷新增益道具列表
            _m_wBuffContainer?.showWnd();
            _m_wBuffContainer?.showItemList(buffIdList);

            //设置总增益百分比值
            ALUGUICommon.setLabelTxt(wnd.txtBuffValue,
                TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num, totalBuffAddPer / 100f));

            long curHp = battleInfo.getCurLeftPower();
            if (curHp < 0)
                curHp = 0;
            //设置当前血量值
            if (totalHp == 0)
                ALUGUICommon.setSliderScale(wnd.sldBlood, 0);
            else
                ALUGUICommon.setSliderScale(wnd.sldBlood, curHp * 1.0f / totalHp);
            ALUGUICommon.setLabelTxt(wnd.txtHP,
                TextTranslate.instance.getLanguage(TransKeyConst.arena_curBlood_num_num,
                    curHp.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT), totalHp.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
        }

        #region 点击事件

        //点击关闭
        private void _onClickClose(GameObject obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_BATTLE_SELECT_BUFF);
        }

        //点击购买加成道具
        private void _onClickBuffBuy(GGUIWndArenaBattleSelectBuffContainerItem _item)
        {
            if (_item == null || _item.buffRef == null || _m_bIsDealingBuyBuff)
                return;

            if (!GCommon.isItemEnough(_item.buffRef.cost, true))
                return;

            _m_bIsDealingBuyBuff = true;
            long serialize = _m_lShowSerialize;
            NPPlayer.instance.arenaComp.reqChooseBuff(_item.buffRef.id, (_isSuc) =>
            {
                //临时增益购买成功
                if(_isSuc)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.arena_battleBuyBuffSuc_none);

                if (wnd == null || !isShow || _m_lShowSerialize != serialize)
                    return;

                _m_bIsDealingBuyBuff = false;
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_BATTLE_SELECT_BUFF);
            });
        }

        #endregion
    }
}