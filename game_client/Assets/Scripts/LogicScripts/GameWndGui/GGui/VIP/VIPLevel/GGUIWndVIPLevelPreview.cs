using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using NPEnum;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// VIP等级预览
    /// </summary>
    public class GGUIWndVIPLevelPreview : _ANPGGUIBasicWnd<GGUIMonoVIPLevelPreview>
    {
        private static GGUIWndVIPLevelPreview _g_instance = new GGUIWndVIPLevelPreview();

        public static GGUIWndVIPLevelPreview instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndVIPLevelPreview();

                return _g_instance;
            }
        }

        public GGUIWndVIPLevelPreview() : base(EALUIWndLayer.ADDITION)
        {
        }

        private long _m_startSelectVipLevel = 0;
        //当前VIP等级配置
        private VipRefObj _m_selectVIPRef;
        //经验进度
        private NPGGUIWndProgress _m_wExpProgress;
        //奖励列表
        private GGUIWndCommonRewardContainer _m_wItemContainer;
        //属性列表
        private NPGGUIWndCommonTextItemGrid _m_wPropertyItemGrid;

        protected override string _monoAssetPath { get { return GGUIMonoVIPLevelPreview.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoVIPLevelPreview.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        protected override void _onShowWnd()
        {
            //设置选中的VIP等级
            // 如果未指定，则为当前vip的下一级
            long curVIPLevel = _m_startSelectVipLevel == 0 ? NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.VIP_LVL) + 1 : _m_startSelectVipLevel;
            List<VipRefObj> vipRefList = GRefdataCoreMgr.instance.vipRefCore.refList;
            for (int i = 0; i < vipRefList.Count; i++)
            {
                if (vipRefList[i] == null || vipRefList[i].vip_lvl == 0)
                    continue;

                //有可领取奖励，或者是下一个等级，或者是最高等级
                if ((!NPPlayer.instance.playerInfo.isGetVipRechargeReward((int) vipRefList[i].vip_lvl) && vipRefList[i].recharge_reward_list != null && vipRefList[i].recharge_reward_list.Count > 0) ||
                    (vipRefList[i] != null && vipRefList[i].vip_lvl == curVIPLevel ) ||
                    (i == vipRefList.Count - 1))
                {
                    _m_selectVIPRef = vipRefList[i];
                    break;
                }
            }
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_startSelectVipLevel = 0;
            _m_wExpProgress?.hideWnd();
            _m_wItemContainer?.hideWnd();
            _m_wPropertyItemGrid?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wExpProgress?.resetWnd();
            _m_wItemContainer?.resetWnd();
            _m_wPropertyItemGrid?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wExpProgress?.discard();
            _m_wExpProgress = null;
            _m_wItemContainer?.discard();
            _m_wItemContainer = null;
            _m_wPropertyItemGrid?.discard();
            _m_wPropertyItemGrid = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnAccess, _onClickAccess);
            ALUGUICommon.uncombineBtnClick(wnd.btnGetReward, _onClickGetReward);
            ALUGUICommon.uncombineBtnClick(wnd.btnPrevious, _onClickPrevious);
            ALUGUICommon.uncombineBtnClick(wnd.btnNext, _onClickNext);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (wnd.expProgress != null)
                _m_wExpProgress = new NPGGUIWndProgress(wnd.expProgress);

            if (wnd.monoItemContainer != null)
                _m_wItemContainer = new GGUIWndCommonRewardContainer(wnd.monoItemContainer);

            if(wnd.monoPropertyGrid != null)
                _m_wPropertyItemGrid = new NPGGUIWndCommonTextItemGrid(wnd.monoPropertyGrid);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnAccess, _onClickAccess);
            ALUGUICommon.combineBtnClick(wnd.btnGetReward, _onClickGetReward);
            ALUGUICommon.combineBtnClick(wnd.btnPrevious, _onClickPrevious);
            ALUGUICommon.combineBtnClick(wnd.btnNext, _onClickNext);
        }
        
        public void setStartSelectVIPLevel(long _vipLevel)
        { 
            _m_startSelectVipLevel = _vipLevel;
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            _refreshExp();
            _refreshPropertyList();
            _refreshRewardState();
        }

        /// <summary>
        /// 刷新经验进度
        /// </summary>
        private void _refreshExp()
        {
            if (wnd == null || _m_selectVIPRef == null)
                return;

            long vipLevel = NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.VIP_LVL);
            VipRefObj curVIPLevelRef = GRefdataCoreMgr.instance.vipRefCore.getRef(vipLevel);
            VipRefObj nextVIPLevelRef = GRefdataCoreMgr.instance.vipRefCore.getRef(vipLevel + 1);
            bool isMaxLevel = nextVIPLevelRef == null;
            long curVIPExp = GCommon.getItemCount(ENPItemType.CURRENCY, (long)ECurrency.VIP_EXP);
            ALUGUICommon.setLabelTxt(wnd.txtVIP, TextTranslate.instance.getLanguage(TransKeyConst.playerinfo_vip_num, _m_selectVIPRef.vip_lvl));
            if (!isMaxLevel)
            {
                _m_wExpProgress?.showWnd();
                _m_wExpProgress?.setProgress(curVIPExp, nextVIPLevelRef.vip_exp, EValueFormatType.NORMAL_NOT_LARGE_STR);
                ALUGUICommon.setLabelTxt(wnd.txtUpgradeDesc, TextTranslate.instance.getLanguage(TransKeyConst.vip_upgradeVIPDesc_num, nextVIPLevelRef.vip_exp - curVIPExp));
            }
            else
            {
                _m_wExpProgress?.showWnd();
                _m_wExpProgress?.setProgress(curVIPExp, curVIPLevelRef.vip_exp, EValueFormatType.NORMAL_NOT_LARGE_STR);
                ALUGUICommon.setLabelTxt(wnd.txtUpgradeDesc, TextTranslate.instance.getLanguage(TransKeyConst.vip_exceedMaxLevelDesc_none));
            }
        }

        /// <summary>
        /// 刷新属性列表
        /// </summary>
        private void _refreshPropertyList()
        {
            if (_m_selectVIPRef == null)
                return;

            VipRefObj lastVIPRef = GRefdataCoreMgr.instance.vipRefCore.getRef(_m_selectVIPRef.vip_lvl - 1);
            List<CommonTextItemStruct> propertyItemList = new List<CommonTextItemStruct>();

            //和上一级比较属性的变化设置文本颜色
            if (_m_selectVIPRef.player_property != null && _m_selectVIPRef.player_property.propertyObjList != null)
            {
                NPPlayerPropertyModifier playerPropertyModifier = _m_selectVIPRef.player_property.duplicate();
                if (lastVIPRef == null || lastVIPRef.vip_lvl == 0)
                {
                    //第一级的情况
                    for (int i = 0; i < playerPropertyModifier.propertyObjList.Count; i++)
                    {
                        NPPlayerPropertyInfoObj tempPropertyObj = playerPropertyModifier.propertyObjList[i];
                        if (tempPropertyObj == null)
                            continue;

                        CommonTextItemStruct tempStruct = new CommonTextItemStruct();
                        NPPlayerPropertyRefObj playerPropertyRefObj = GRefdataCoreMgr.instance.playerPropertyCore.getRef((long)tempPropertyObj.type);
                        if (playerPropertyRefObj != null && playerPropertyRefObj.is_show)
                        {
                            tempStruct.needShowGo = false;
                            tempStruct.strOne = GCommon.addColorForRichText(GCommon.getPlayerPropertyName(tempPropertyObj.type), wnd.normalPropertyColor);
                            tempStruct.texture = GCommon.getPlayerPropertyIcon(playerPropertyRefObj.player_property_type);
                            tempStruct.strTwo = GCommon.addColorForRichText(TextTranslate.instance.getLanguage(TransKeyConst.common_add_num,tempPropertyObj.value), wnd.normalPropertyColor);
                            propertyItemList.Add(tempStruct);
                        }
                    }
                }
                else
                {
                    //非第一级的情况
                    for (int i = 0; i < playerPropertyModifier.propertyObjList.Count; i++)
                    {
                        NPPlayerPropertyInfoObj tempPropertyObj = playerPropertyModifier.propertyObjList[i];
                        if(tempPropertyObj == null)
                            continue;

                        bool isNew = true;
                        if (lastVIPRef.player_property != null && lastVIPRef.player_property.propertyObjList != null)
                        {
                            for (int j = 0; j < lastVIPRef.player_property.propertyObjList.Count; j++)
                            {
                                if (tempPropertyObj.type == lastVIPRef.player_property.propertyObjList[j].type && tempPropertyObj.value == lastVIPRef.player_property.propertyObjList[j].value)
                                {
                                    isNew = false;
                                    break;
                                }
                            }
                        }

                        CommonTextItemStruct tempStruct = new CommonTextItemStruct();
                        NPPlayerPropertyRefObj playerPropertyRefObj = GRefdataCoreMgr.instance.playerPropertyCore.getRef((long)tempPropertyObj.type);
                        if (playerPropertyRefObj != null && playerPropertyRefObj.is_show)
                        {
                            tempStruct.needShowGo = false;
                            tempStruct.strOne = GCommon.addColorForRichText(GCommon.getPlayerPropertyName(tempPropertyObj.type), isNew ? wnd.newPropertyColor : wnd.normalPropertyColor);
                            tempStruct.texture = GCommon.getPlayerPropertyIcon(playerPropertyRefObj.player_property_type);
                            tempStruct.strTwo = GCommon.addColorForRichText(TextTranslate.instance.getLanguage(TransKeyConst.common_add_num,tempPropertyObj.value), isNew ? wnd.newPropertyColor : wnd.normalPropertyColor);
                            propertyItemList.Add(tempStruct);
                        }
                    }
                }
            }

            //显示属性列表
            _m_wPropertyItemGrid?.showWnd();
            _m_wPropertyItemGrid?.setItemList(propertyItemList);
        }

        /// <summary>
        /// 刷新奖励状态
        /// </summary>
        private void _refreshRewardState()
        {
            if (wnd == null || _m_selectVIPRef == null)
                return;

            long curVIPLevel = NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.VIP_LVL);
            bool haveReward = _m_selectVIPRef.recharge_reward_list != null && _m_selectVIPRef.recharge_reward_list.Count > 0;
            bool canGetReward = !NPPlayer.instance.playerInfo.isGetVipRechargeReward((int)_m_selectVIPRef.vip_lvl) && haveReward && GCommon.getItemCount(ENPItemType.CURRENCY,(long)ECurrency.VIP_EXP) >= _m_selectVIPRef.vip_exp;

            //按钮显隐
            ALUGUICommon.setGameObjEnable(wnd.btnGetReward, canGetReward);
            ALUGUICommon.setGameObjEnable(wnd.btnAccess, !canGetReward && curVIPLevel < _m_selectVIPRef.vip_lvl);
            ALUGUICommon.setGameObjEnable(wnd.btnPrevious,  GRefdataCoreMgr.instance.vipRefCore.getRef(_m_selectVIPRef.vip_lvl - 1) != null && _m_selectVIPRef.vip_lvl != 1);
            ALUGUICommon.setGameObjEnable(wnd.btnNext,  GRefdataCoreMgr.instance.vipRefCore.getRef(_m_selectVIPRef.vip_lvl + 1) != null);

            //奖励列表
            if (haveReward)
            {
                _m_wItemContainer?.showWnd();
                _m_wItemContainer?.setRewardList(_m_selectVIPRef.recharge_reward_list, _getRewardType());
            }
            else
                _m_wItemContainer?.hideWnd();
            ALUGUICommon.setGameObjEnable(wnd.goNoRewardHideList, haveReward);
            ALUGUICommon.setGameObjEnable(wnd.goNoRewardShowList, !haveReward);
        }

        /// <summary>
        /// 获取奖励类型枚举
        /// </summary>
        /// <returns></returns>
        private ECommonRewardType _getRewardType()
        {
            ECommonRewardType rewardType = ECommonRewardType.NONE;

            if (_m_selectVIPRef != null)
            {
                if (NPPlayer.instance.playerInfo.isGetVipRechargeReward((int)_m_selectVIPRef.vip_lvl))
                    rewardType = ECommonRewardType.HAS_GET_REWARD;
                else if (GCommon.getItemCount(ENPItemType.CURRENCY, (long)ECurrency.VIP_EXP) >= _m_selectVIPRef.vip_exp && _m_selectVIPRef.recharge_reward_list != null && _m_selectVIPRef.recharge_reward_list.Count > 0)
                    rewardType = ECommonRewardType.CAN_GET_REWARD;
                else
                    rewardType = ECommonRewardType.NOT_GET_REWARD;
            }
            return rewardType;
        }

        #region 点击事件
        
        /// <summary>
        /// 点击获取途径按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickAccess(GameObject _go)
        {
            GCommon.popItemAccessWays(ENPItemType.CURRENCY, (int) ECurrency.VIP_EXP);
        }

        /// <summary>
        /// 点击领取奖励
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickGetReward(GameObject _go)
        {
            if (_m_selectVIPRef == null || _getRewardType() != ECommonRewardType.CAN_GET_REWARD)
                return;

            NPPlayer.instance.playerInfoComp.reqDrawVipLevelRechargeReward(_m_selectVIPRef.vip_lvl, _isSuc =>
            {
                if(_isSuc)
                    _refreshWnd();
            });
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_VIP_LEVEL_PREVIEW);
        }

        /// <summary>
        /// 点击上一个按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickPrevious(GameObject _go)
        {
            if (_m_selectVIPRef == null || _m_selectVIPRef.vip_lvl == 1)
                return;

            VipRefObj preVIPRef = GRefdataCoreMgr.instance.vipRefCore.getRef(_m_selectVIPRef.vip_lvl - 1);
            if (preVIPRef != null)
            {
                _m_selectVIPRef = preVIPRef;
                _refreshWnd();
            }
        }

        /// <summary>
        /// 点击下一个按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickNext(GameObject _go)
        {
            if (_m_selectVIPRef == null)
                return;

            VipRefObj nextVIPRef = GRefdataCoreMgr.instance.vipRefCore.getRef(_m_selectVIPRef.vip_lvl + 1);
            if (nextVIPRef != null)
            {
                _m_selectVIPRef = nextVIPRef;
                _refreshWnd();
            }
        }

        #endregion
    }
}
