using System;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 藏品升级页签界面
    /// </summary>
    public class GGUIWndEquipSkillPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoEquipSkillPage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字

        //藏品信息
        private EquipInfo _m_equipInfo;
        //点击关闭按钮
        private Action _m_aOnClickClose;
        //播放替换特效回调
        private Action<bool> _m_aOnPlaySfx;
        //当前的藏品技能item
        private GGUIWndEquipSkillContainerItem _m_wCurEquipSkillItem;
        //重塑后新的藏品技能item
        private GGUIWndEquipSkillContainerItem _m_wNewEquipSkillItem;
        //高级重塑消耗
        private NPGGUIWndCommonItem _m_wAdvancedCost;
        //普通重塑消耗
        private NPGGUIWndCommonItem _m_wNormalCost;
        //普通重塑消耗
        private NPGGUIWndCommonItem _m_wToggleShowCost;
        //按钮切换开关
        private NPGGUIWndCommonToggleEx _m_wButtonSwitchToggle;
        //当前选中的技能信息
        private EquipSkillInfo _m_curSelectEquipSkillInfo;
        //特效列表
        private List<CommonUISfxObj> _m_lSfxObjList;
        //上个加成值，用于判断是否需要播放替换特效
        private long _m_lLastAddValue;

        public GGUIWndEquipSkillPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent)
            : base(_parent)
        {
            _m_sAssetPath = _assetPathInfo.asset_path;
            _m_sObjName = _assetPathInfo.obj_name;
        }

        /**************
         * 窗口相关加载配置
         **/
        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_EQUIP_Skill_CHG, _onSkillChg);
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _onCurrencyChg);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_EQUIP_SKILL_REBUILD, _onSimulateClickEquipSkillRebuild);
            _m_curSelectEquipSkillInfo = null;

            _m_wButtonSwitchToggle?.showWnd();
            _m_wButtonSwitchToggle?.setSelected(NPPlayer.instance.equipComp.isUseAdvanced, true, false);
        }
        
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_EQUIP_Skill_CHG, _onSkillChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _onCurrencyChg);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_EQUIP_SKILL_REBUILD, _onSimulateClickEquipSkillRebuild);

            _m_wCurEquipSkillItem?.hideWnd();
            _m_wNewEquipSkillItem?.hideWnd();
            _m_wAdvancedCost?.hideWnd();
            _m_wNormalCost?.hideWnd();
            _m_wToggleShowCost?.hideWnd();

            _m_curSelectEquipSkillInfo = null;
            _discardSfx();
        }
        
        protected override void _onReset()
        {
            _m_wCurEquipSkillItem?.resetWnd();
            _m_wNewEquipSkillItem?.resetWnd();
            _m_wAdvancedCost?.resetWnd();
            _m_wNormalCost?.resetWnd();
            _m_wToggleShowCost?.resetWnd();
        }
        
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_wCurEquipSkillItem?.discard();
            _m_wCurEquipSkillItem = null;
            _m_wNewEquipSkillItem?.discard();
            _m_wNewEquipSkillItem = null;
            _m_wAdvancedCost?.discard();
            _m_wAdvancedCost = null;
            _m_wNormalCost?.discard();
            _m_wNormalCost = null;
            _m_wToggleShowCost?.discard();
            _m_wToggleShowCost = null;
            _m_wButtonSwitchToggle?.discard();
            _m_wButtonSwitchToggle = null;
            _discardSfx();

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnAdvanced, _onClickAdvanced);
            ALUGUICommon.uncombineBtnClick(wnd.btnNormal, _onClickNormal);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoCurSkillItem != null)
                _m_wCurEquipSkillItem = new GGUIWndEquipSkillContainerItem(wnd.monoCurSkillItem);

            if (wnd.monoNewSkillItem != null)
                _m_wNewEquipSkillItem = new GGUIWndEquipSkillContainerItem(wnd.monoNewSkillItem);

            if (wnd.monoAdvancedCost != null)
                _m_wAdvancedCost = new NPGGUIWndCommonItem(wnd.monoAdvancedCost);

            if (wnd.monoNormalCost != null)
                _m_wNormalCost = new NPGGUIWndCommonItem(wnd.monoNormalCost);

            if (wnd.monoToggleCostItem != null)
                _m_wToggleShowCost = new NPGGUIWndCommonItem(wnd.monoToggleCostItem);

            if (wnd.monoBtnSwitchToggle != null)
            {
                _m_wButtonSwitchToggle = new NPGGUIWndCommonToggleEx(wnd.monoBtnSwitchToggle);
                _m_wButtonSwitchToggle.clickDelegate += _onClickToggle;
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnAdvanced, _onClickAdvanced);
            ALUGUICommon.combineBtnClick(wnd.btnNormal, _onClickNormal);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(EquipInfo _info, Action _onClickClose, Action<bool> _onPlaySfx)
        {
            _m_equipInfo = _info;
            _m_aOnClickClose = _onClickClose;
            _m_aOnPlaySfx = _onPlaySfx;

            //默认设置没达到满级
            ALUGUICommon.setGameObjEnable(wnd?.goAddPropMaxHideList, true);
            ALUGUICommon.setGameObjEnable(wnd?.goAddPropMaxShowList, false);

            _refreshButtonState();
        }

        /// <summary>
        /// 点击技能item
        /// </summary>
        /// <param name="_item"></param>
        public void onClickSkillItem(GGUIWndEquipSkillContainerItem _item)
        {
            if (_item == null || _item.equipSkillInfo == null)
                return;

            _m_curSelectEquipSkillInfo = _item.equipSkillInfo;
            _m_lLastAddValue = _m_curSelectEquipSkillInfo.curValue;

            //刷新技能详情
            _refreshSkillDetail();
        }

        //刷新技能及新技能
        private void _refreshSkillDetail()
        {
            if (_m_curSelectEquipSkillInfo == null || _m_equipInfo == null)
                return;

            //显示当前的技能
            if (_m_wCurEquipSkillItem != null)
            {
                _m_wCurEquipSkillItem.showWnd();
                _m_wCurEquipSkillItem.setInfo(_m_curSelectEquipSkillInfo.curValue);
            }

            //显示新的技能
            if (_m_wNewEquipSkillItem != null)
            {
                _m_wNewEquipSkillItem.showWnd();
                _m_wNewEquipSkillItem.setInfo(_m_curSelectEquipSkillInfo.pendingValue);
            }

            //是否达到最大加成值了
            bool isMaxAddProValue = _m_curSelectEquipSkillInfo.curValue >= GRefdataCoreMgr.instance.npGeneral.equip_add_pro_per_max_value;
            ALUGUICommon.setGameObjEnable(wnd.goAddPropMaxHideList, !isMaxAddProValue);
            ALUGUICommon.setGameObjEnable(wnd.goAddPropMaxShowList, isMaxAddProValue);

            //新技能是否更好
            bool isBetter = _m_curSelectEquipSkillInfo.pendingValue > _m_curSelectEquipSkillInfo.curValue;
            ALUGUICommon.setGameObjEnable(wnd.goBetterThenCurHideList, !isBetter);
            ALUGUICommon.setGameObjEnable(wnd.goBetterThenCurShowList, isBetter);

            //刷新一下按钮状态
            _refreshButtonState();
        }

        //刷新按钮状态
        private void _refreshButtonState()
        {
            if (wnd == null || _m_equipInfo == null)
                return;

            //高级按钮里显示的消耗
            if (_m_curSelectEquipSkillInfo != null)
            {
                NPCommonCostItem costItem = new NPCommonCostItem(_m_curSelectEquipSkillInfo.getNormalRebuildCostItem());
                costItem?.setCount(0);
                _m_wAdvancedCost?.showWnd();
                _m_wAdvancedCost?.setItem(costItem);
            }

            //普通按钮里显示的消耗
            if (_m_curSelectEquipSkillInfo != null)
            {
                _m_wNormalCost?.showWnd();
                _m_wNormalCost?.setItem(_m_curSelectEquipSkillInfo.getNormalRebuildCostItem());
            }

            //按钮切换开关里显示的消耗
            if (_m_wToggleShowCost != null)
            {
                _m_wToggleShowCost.showWnd();
                _m_wToggleShowCost.setItem(GRefdataCoreMgr.instance.npGeneral.equip_advance_cost);
            }

            //设置概率区间
            long minAddValue = 0;
            long maxAddValue = 0;
            ProAddGroupRefObj proAddGroupRef = null;
            List<ProAddRefObj> proAddRefList = null;
            //普通概率区间
            proAddGroupRef = GRefdataCoreMgr.instance.proAddGroupRefCore.getRef(GRefdataCoreMgr.instance.npGeneral.equip_normal_add_pro_group_id);
            if (proAddGroupRef != null)
            {
                proAddRefList = proAddGroupRef.add_list;
                if (proAddRefList != null && proAddRefList.Count > 0)
                {
                    minAddValue = proAddRefList[0].add;
                    maxAddValue = proAddRefList[proAddRefList.Count - 1].add;
                }

                ALUGUICommon.setLabelTxt(wnd.txtNormalProbability,
                    TextTranslate.instance.getLanguage(TransKeyConst.equip_skillRebuildPropProbability_num_num,
                        minAddValue / 100f, maxAddValue / 100f));
            }
            //高级概率区间
            proAddGroupRef = GRefdataCoreMgr.instance.proAddGroupRefCore.getRef(GRefdataCoreMgr.instance.npGeneral.equip_advance_add_pro_group_id);
            if (proAddGroupRef != null)
            {
                proAddRefList = proAddGroupRef.add_list;
                if (proAddRefList != null && proAddRefList.Count > 0)
                {
                    minAddValue = proAddRefList[0].add;
                    maxAddValue = proAddRefList[proAddRefList.Count - 1].add;
                }

                ALUGUICommon.setLabelTxt(wnd.txtAdvancedProbability,
                    TextTranslate.instance.getLanguage(TransKeyConst.equip_skillRebuildPropProbability_num_num,
                        minAddValue / 100f, maxAddValue / 100f));
            }
        }

        //播放重铸特效,生效并且更好时播放，或者生效并且不好时播放
        private void _playRebuildSfx()
        {
            if (wnd == null || wnd.rebuildSfxParent == null || _m_equipInfo == null || _m_curSelectEquipSkillInfo == null)
                return;

            //是否更好
            bool isBetter = _m_curSelectEquipSkillInfo.pendingValue > _m_lLastAddValue;

            //播放特效
            if (_m_lSfxObjList == null)
                _m_lSfxObjList = new List<CommonUISfxObj>();

            CommonUISfxObj sfxObj = null;
            //单次升级成功特效
            if (isBetter)
                sfxObj = PlaySfxMgr.instance.playUISfx(wnd.effectiveAndBetterSfxId, wnd.rebuildSfxParent);
            else
                sfxObj = PlaySfxMgr.instance.playUISfx(wnd.effectiveAndWorseSfxId, wnd.rebuildSfxParent);

            if (sfxObj != null)
                _m_lSfxObjList.Add(sfxObj);

            //播放替换技能特效
            if (_m_lLastAddValue < _m_curSelectEquipSkillInfo.curValue)
            {
                _m_wCurEquipSkillItem?.playReplaceSfx();
                _m_aOnPlaySfx?.Invoke(true);

                //记录加成值
                _m_lLastAddValue = _m_curSelectEquipSkillInfo.curValue;
            }
            else
            {
                _m_aOnPlaySfx?.Invoke(false);
            }
        }

        //销毁特效
        private void _discardSfx()
        {
            if (_m_lSfxObjList != null)
            {
                for (int i = 0; i < _m_lSfxObjList.Count; i++)
                {
                    _m_lSfxObjList[i]?.forceDiscard();
                }
                _m_lSfxObjList.Clear();
                _m_lSfxObjList = null;
            }
        }

        #region 消息事件

        //道具变更
        private void _onBagItemChg(params object[] _objects)
        {
            _refreshButtonState();
        }

        //技能变更
        private void _onSkillChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 2)
                return;

            long dbId = (long) _objects[0];
            int skillIndex = (int) _objects[1];
            if (_m_equipInfo == null || _m_curSelectEquipSkillInfo == null)
                return;

            if (_m_equipInfo.dbId == dbId && _m_curSelectEquipSkillInfo.index == skillIndex)
            {
                _refreshSkillDetail();
                _playRebuildSfx();
            }
        }

        //货币变化
        private void _onCurrencyChg(params object[] _objs)
        {
            _refreshButtonState();
        }

        //模拟点击重塑
        private void _onSimulateClickEquipSkillRebuild()
        {
            _onClickNormal(null);
        }

        #endregion

        #region 点击事件

        //点击按钮切换开关
        private void _onClickToggle(NPGGUIWndCommonToggleEx _commonToggleEx)
        {
            if (_commonToggleEx == null || _m_wButtonSwitchToggle == null)
                return;

            bool isOn = !_commonToggleEx.isOn;
            _m_wButtonSwitchToggle.setSelected(isOn, true);
            NPPlayer.instance.equipComp.isUseAdvanced = isOn;
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            _m_aOnClickClose?.Invoke();
        }

        //点击高级重塑
        private void _onClickAdvanced(GameObject _go)
        {
            if (_m_equipInfo == null || _m_curSelectEquipSkillInfo == null)
                return;
            
            if (!GCommon.isItemEnough(GRefdataCoreMgr.instance.npGeneral.equip_advance_cost, true))
                return;

            //生成新加成效果
            NPPlayer.instance.equipComp.reqEquipSkillRebuild(_m_equipInfo.dbId, _m_curSelectEquipSkillInfo.index, true);
        }

        //点击普通重塑
        private void _onClickNormal(GameObject _go)
        {
            if (_m_equipInfo == null || _m_curSelectEquipSkillInfo == null)
                return;
            
            if (!GCommon.isItemEnough(_m_curSelectEquipSkillInfo.getNormalRebuildCostItem(), true))
                return;

            //生成新加成效果
            NPPlayer.instance.equipComp.reqEquipSkillRebuild(_m_equipInfo.dbId, _m_curSelectEquipSkillInfo.index, false);
        }

        #endregion
    }
}