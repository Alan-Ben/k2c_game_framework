using System;
using System.Collections.Generic;
using ALPackage;
using Common.ConsortObj;
using CommonEnum;
using UnityEngine;

namespace GOE
{
    public interface _IGGUIWndUnLockConsortDetailBusinessPageParam
    {
        /// <summary>
        /// 选中的技能id
        /// </summary>
        long consortDetailBusinessPageSelectSkillId { get; set; }
    }
    
    /// <summary>
    /// 妃子解锁详情页面经营page
    /// </summary>
    public class GGUIWndUnLockConsortDetailBusinessPage : _AGGUIWndUnLockConsortDetailTabPage<GGUIMonoUnLockConsortDetailBusinessPage>
    {
        private _IGGUIWndUnLockConsortDetailBusinessPageParam _m_param;
        
        // private GGUIWndConsortBusinessSkillSlider _m_wndConsortBusinessSkillSlider;//经营技能进度条
        private GGUIWndConsortBusinessSkillSliderStageMgr _m_wConsortBusinessSkillStageContainer;//经营技能阶段列表
        private NPGGuiWndTexture _m_wSelectSkillIcon;//选中的技能icon
        private NPGGUIWndCommonItem _m_wAdvanceComprehendCostItem;//高级领悟消耗道具
        private NPGGUIWndCommonItem _m_wNormalComprehendCostItem;//普通领悟消耗道具
        private NPGGUIWndCommonToggleEx _m_wNormalAdvanceChgToggle;//普通领悟和高级领悟切换按钮
        
        public GGUIWndUnLockConsortDetailBusinessPage(_IGGUIWndUnLockConsortDetailBusinessPageParam _param, NPCommonAssetPathInfo _commonAssetPathInfo, Transform _parent) : base(_commonAssetPathInfo, _parent)
        {
            _m_param = _param;
        }

        /// <summary>
        /// 本窗口对应的页签类型
        /// </summary>
        public override EUnLockConsortDetailWndTabType tabPageType { get { return EUnLockConsortDetailWndTabType.BUSINESS; } }

        protected override void _onWndInitDoneSub()
        {
            if(wnd == null)
                return;

            if (wnd.businessSkillStageContainer != null)
            {
                _m_wConsortBusinessSkillStageContainer = new GGUIWndConsortBusinessSkillSliderStageMgr(wnd.businessSkillStageContainer, _m_param, _checkBusinessSkillItemNeedShowRedTip);
                _m_wConsortBusinessSkillStageContainer.onSkillItemClick += _onSkillItemClick;
            }
            
            if(wnd.nowSelectSKillIcon != null)
                _m_wSelectSkillIcon = new NPGGuiWndTexture(wnd.nowSelectSKillIcon);

            if (wnd.advanceComprehendCostItem != null)
                _m_wAdvanceComprehendCostItem = new NPGGUIWndCommonItem(wnd.advanceComprehendCostItem);

            if (wnd.normalComprehendCostItem != null)
                _m_wNormalComprehendCostItem = new NPGGUIWndCommonItem(wnd.normalComprehendCostItem);

            if (wnd.normalAdvanceChgToggle != null)
            {
                _m_wNormalAdvanceChgToggle = new NPGGUIWndCommonToggleEx(wnd.normalAdvanceChgToggle);
                _m_wNormalAdvanceChgToggle.clickDelegate += _onNormalAdvanceChgToggleClick;
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnNormalComprehend, _onNormalComprehendClick);
            ALUGUICommon.combineBtnClick(wnd.btnAdvanceComprehend, _onAdvanceComprehendClick);
            ALUGUICommon.combineBtnClick(wnd.gotoEnhance, _onGotoEnhanceBtnClick);
        }

        protected override void _onDiscardSub()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnNormalComprehend, _onNormalComprehendClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnAdvanceComprehend, _onAdvanceComprehendClick);
                ALUGUICommon.uncombineBtnClick(wnd.gotoEnhance, _onGotoEnhanceBtnClick);
            }
            
            if (_m_wConsortBusinessSkillStageContainer != null)
            {
                _m_wConsortBusinessSkillStageContainer.onSkillItemClick -= _onSkillItemClick;
                _m_wConsortBusinessSkillStageContainer.discard();
            }
            _m_wConsortBusinessSkillStageContainer = null;                
            
            _m_wSelectSkillIcon?.discard();
            _m_wSelectSkillIcon = null;
            
            _m_wAdvanceComprehendCostItem?.discard();
            _m_wAdvanceComprehendCostItem = null;
            
            _m_wNormalComprehendCostItem?.discard();
            _m_wNormalComprehendCostItem = null;

            if (_m_wNormalAdvanceChgToggle != null)
            {
                _m_wNormalAdvanceChgToggle.clickDelegate -= _onNormalAdvanceChgToggleClick;
                _m_wNormalAdvanceChgToggle.discard();
                _m_wNormalAdvanceChgToggle = null;       
            }
        }

        protected override void _onShowWndSub()
        {
            if (_m_wNormalAdvanceChgToggle != null)
            {
                _m_wNormalAdvanceChgToggle.showWnd();
                _m_wNormalAdvanceChgToggle.setSelected(ConsortUtil.businessSkillAdvanceComprehendIsOn);
            }
            
            WinMsg.RegisterMsg(WinMsgType.ON_CONSORT_BUSINESS_SKILL_INFO_CHG, _onConsortBusinessSkillInfoChg);
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _onCostItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onCostItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onCostItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onCostItemChg);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_CONSORT_NORMAL_COMPREHEND, _onSimulateClickNormalComprehend);
        }

        protected override void _onHideWndSub()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_CONSORT_BUSINESS_SKILL_INFO_CHG, _onConsortBusinessSkillInfoChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _onCostItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onCostItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onCostItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onCostItemChg);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_CONSORT_NORMAL_COMPREHEND, _onSimulateClickNormalComprehend);

            _m_wConsortBusinessSkillStageContainer?.hideWnd();
            
            _m_wSelectSkillIcon?.hideWnd();
            
            _m_wAdvanceComprehendCostItem?.hideWnd();
            _m_wNormalComprehendCostItem?.hideWnd();
            _m_wNormalAdvanceChgToggle?.hideWnd();
        }

        protected override void _onResetSub()
        {
            _m_wConsortBusinessSkillStageContainer?.resetWnd();
            
            _m_wSelectSkillIcon?.discardTexture();
            
            _m_wAdvanceComprehendCostItem?.resetWnd();
            _m_wNormalComprehendCostItem?.resetWnd();
            _m_wNormalAdvanceChgToggle?.resetWnd();
        }
        
        protected override void _setDataSub()
        {
            _m_wConsortBusinessSkillStageContainer?.resetAllItemShowInfo();//重置技能item显示数据
            //进入页面时, 请求技能信息
            _m_iConsortShowInfo?.getAllBusinessSkillInfoList(null);
        }
        
        protected override void _refreshWndSub()
        {
            _refreshTotalAddPerTextList();
            _refreshSkillSlider();
            _refreshSelectSkillItemInfo();//刷新当前选中技能item信息
        }
        
        /// <summary>
        /// 当前技能item被点击时
        /// </summary>
        private void _onSkillItemClick(GGUIPrefabSubWndConsortBusinessSkillItem _item)
        {
            if(_item == null || _item.businessSkillRefObj == null)
                return;

            if (_m_wConsortBusinessSkillStageContainer != null)
            {
                _m_wConsortBusinessSkillStageContainer.setSelectSkill(_item.businessSkillRefObj.id, true, false);
                _refreshSelectSkillItemInfo();
            }
        }

        /// <summary>
        /// 刷新建筑总加成
        /// </summary>
        private void _refreshTotalAddPerTextList()
        {
            if(wnd == null || wnd.totalAddPerTextList == null || _m_iConsortShowInfo == null || _m_iConsortShowInfo.businessSkillInfoList == null)
                return;

            Consort_BusinessSkillPropertySum allPropertyAddSum = _m_iConsortShowInfo.businessSkillInfoList.getSkillPropertyAddPropertySum(ESpecAttrType.NONE);
            foreach (var addPerText in wnd.totalAddPerTextList)
            {
                if(addPerText == null || addPerText.txtAddPer == null)
                    continue;
                
                Consort_BusinessSkillPropertySum propertySum = _m_iConsortShowInfo.businessSkillInfoList.getSkillPropertyAddPropertySum(addPerText.specAttrType);
                ALUGUICommon.setLabelTxt(addPerText.txtAddPer,
                    TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num, ((propertySum?.getProAddSum() ?? 0) + (allPropertyAddSum?.getProAddSum() ?? 0)) / 100f));
            }
        }
        
        /// <summary>
        /// 刷新技能进度条
        /// </summary>
        private void _refreshSkillSlider()
        {
            if(_m_iConsortShowInfo == null)
                return;

            if(wnd != null)
                ALUGUICommon.setLabelTxt(wnd.txtCurIntimacy, _m_iConsortShowInfo.intimacy);
            
            if (_m_wConsortBusinessSkillStageContainer != null)
            {
                // 设置技能信息
                _m_iConsortShowInfo.getAllBusinessSkillInfoList((_skillList) =>
                {
                    _m_wConsortBusinessSkillStageContainer.showWnd();
                    
                    _m_wConsortBusinessSkillStageContainer.setBusinessSkillInfo(_skillList);
                    _m_wConsortBusinessSkillStageContainer.setNowIntimacy(_m_iConsortShowInfo.intimacy);
                    
                    _m_wConsortBusinessSkillStageContainer.setSelectSkill(_m_param?.consortDetailBusinessPageSelectSkillId ?? 0);
                });
            }
        }
        
        /// <summary>
        /// 刷新选中技能item信息
        /// </summary>
        private void _refreshSelectSkillItemInfo()
        {
            if(_m_iConsortShowInfo == null || wnd == null)
                return;

            ConsortBusinessSkillRefObj nowSelectSkillRefObj = _m_wConsortBusinessSkillStageContainer?.nowSelectSkillItem?.businessSkillRefObj;
            
            if (nowSelectSkillRefObj == null)
            {
                ALUGUICommon.setGameObjEnable(wnd.hasSelectSkillShow, false);
                ALUGUICommon.setGameObjEnable(wnd.hasSelectSkillHide, true);
                return;
            }
            
            ALUGUICommon.setGameObjEnable(wnd.hasSelectSkillShow, true);
            ALUGUICommon.setGameObjEnable(wnd.hasSelectSkillHide, false);

            BasicAttrRefObj selectSKillAttrRefObj = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long) nowSelectSkillRefObj.property);
            if (selectSKillAttrRefObj != null)
            {
                if (_m_wSelectSkillIcon != null)
                {
                    _m_wSelectSkillIcon.showWnd();
                    _m_wSelectSkillIcon.setTexture(selectSKillAttrRefObj.icon);
                }
                
                ALUGUICommon.setLabelTxt(wnd.nowSelectSkillName, TextTranslate.instance.getLanguage(selectSKillAttrRefObj.name));
            }

            _m_iConsortShowInfo.getBusinessSkillInfo(nowSelectSkillRefObj.id, (selectSkillInfo) =>
            {
                if (selectSKillAttrRefObj != null)
                {
                    float proAdd = (selectSkillInfo?.proAdd ?? 0) / 100f;//获取当前加成值
                    if (selectSKillAttrRefObj.type != ESpecAttrType.NONE)
                    {
                        // 设置当前效果描述
                        ALUGUICommon.setLabelTxt(wnd.nowSelectSkillAddEffect,
                            TextTranslate.instance.getLanguage(TransKeyConst.consort_business_skillEffectDesc_str_num, selectSKillAttrRefObj.name, proAdd));
                    }
                    else
                    {
                        // 设置当前效果描述
                        ALUGUICommon.setLabelTxt(wnd.nowSelectSkillAddEffect,
                            TextTranslate.instance.getLanguage(TransKeyConst.consort_business_skillEffectDescAllAttr_num, proAdd));
                    }
                }
                else
                {
                    ALUGUICommon.setLabelTxt(wnd.nowSelectSkillAddEffect, "");
                }
                
                // 设置解锁条件描述
                ALUGUICommon.setLabelTxt(wnd.txtSkillUnlockCondition, TextTranslate.instance.getLanguage(TransKeyConst.consort_skill_unlock_tip_num, nowSelectSkillRefObj.unlock_need_intimacy - _m_iConsortShowInfo.intimacy));
            
                EConsortBusinessSkillItemState skillItemState = ConsortUtil.getBusinessSkillState(nowSelectSkillRefObj, selectSkillInfo, _m_iConsortShowInfo.intimacy);
                wnd.setNowSelectSkillState(skillItemState);//设置状态
                
                // 若当前选中技能处于 已解锁未进行过操作 状态 设置红点已经读过
                if (skillItemState == EConsortBusinessSkillItemState.UNLOCK_NO_OP)
                {
                    NPPlayer.instance.consortComp.setReadBusinessSkillRed(_m_iConsortShowInfo.consortId, nowSelectSkillRefObj.id);
                    _m_wConsortBusinessSkillStageContainer?.refreshRedTipShow(nowSelectSkillRefObj.id);
                }
            });

            // 刷新领悟信息
            _refreshComprehendInfo();
        }

        /// <summary>
        /// 刷新领悟信息
        /// </summary>
        private void _refreshComprehendInfo()
        {
            if(_m_iConsortShowInfo == null)
                return;
            
            ConsortBusinessSkillRefObj businessSkillRefObj = _m_wConsortBusinessSkillStageContainer?.nowSelectSkillItem?.businessSkillRefObj;
            if(businessSkillRefObj == null)
                return;

            _m_iConsortShowInfo.getBusinessSkillInfo(businessSkillRefObj.id, (businessSkillInfo) =>
            {
                long add = businessSkillInfo?.proAdd ?? 0;//获取当前加成值
            
                float normalAddUpPer = (businessSkillRefObj.normalAddProGroupRefObj?.getAddUpPro(add) ?? 0) * 100;//普通领悟获取提升概率
                float advanceAddUpPer = (businessSkillRefObj.advanceAddProGroup?.getAddUpPro(add) ?? 0) * 100;//高级领悟获取提升概率
            
                if (wnd != null)
                {
                    ALUGUICommon.setLabelTxt(wnd.txtNormalComprehendSuccessRateDesc, TextTranslate.instance.getLanguage(TransKeyConst.consort_skill_rate_title, normalAddUpPer.ToString("0.##")));
                    ALUGUICommon.setLabelTxt(wnd.txtAdvanceComprehendSuccessRateDesc, TextTranslate.instance.getLanguage(TransKeyConst.consort_skill_rate_title, advanceAddUpPer.ToString("0.##")));
                }
            
                if (_m_wNormalComprehendCostItem != null)
                {
                    OpCostRefObj opCostRefObj = businessSkillRefObj.normalOpCostGroupRefObj?.getOpCostRefObj(businessSkillInfo?.normalOpCount ?? 0);
                    
                    _m_wNormalComprehendCostItem.showWnd();
                    _m_wNormalComprehendCostItem.setItem(opCostRefObj?.cost_item);
                }

                if (_m_wAdvanceComprehendCostItem != null)
                {
                    OpCostRefObj opCostRefObj = businessSkillRefObj.advanceOpCostGroupRefObj?.getOpCostRefObj(businessSkillInfo?.advanceOpCount ?? 0);
                    
                    _m_wAdvanceComprehendCostItem.showWnd();
                    _m_wAdvanceComprehendCostItem.setItem(opCostRefObj?.cost_item);
                }
            });
        }
        
        /// <summary>
        /// 普通领悟按钮被点击
        /// </summary>
        private void _onNormalComprehendClick(GameObject _go)
        {
            if(_m_iConsortShowInfo == null)
                return;
            
            ConsortBusinessSkillRefObj businessSkillRefObj = _m_wConsortBusinessSkillStageContainer?.nowSelectSkillItem?.businessSkillRefObj;
            if(businessSkillRefObj == null)
                return;

            _m_iConsortShowInfo.getBusinessSkillInfo(businessSkillRefObj.id, (_businessInfo) =>
            {
                int opCount = _businessInfo?.normalOpCount ?? 0;
                OpCostRefObj opCostRefObj = businessSkillRefObj.normalOpCostGroupRefObj?.getOpCostRefObj(opCount);

                // 消耗不足, 提示后返回
                if (opCostRefObj != null && !GCommon.isItemEnough(opCostRefObj.cost_item, true))
                    return;

                // 请求普通领悟
                NPPlayer.instance.consortComp.reqUnderstandBusinessSkill(_m_iConsortShowInfo.consortId,
                    businessSkillRefObj.id, false,
                    (_msg) =>
                    {
                        if (_msg != null && _msg.getIsProUp())
                        {
                            _m_wConsortBusinessSkillStageContainer?.playProAddSuccessSfx(businessSkillRefObj.id);                            
                        }
                        else
                        {
                            _m_wConsortBusinessSkillStageContainer?.playProAddFailSfx(businessSkillRefObj.id);                            
                        }
                    }, null);
            });
        }

        /// <summary>
        /// 高级领悟按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onAdvanceComprehendClick(GameObject _go)
        {
            if(_m_iConsortShowInfo == null)
                return;
            
            ConsortBusinessSkillRefObj businessSkillRefObj = _m_wConsortBusinessSkillStageContainer?.nowSelectSkillItem?.businessSkillRefObj;
            if(businessSkillRefObj == null)
                return;
            
            _m_iConsortShowInfo.getBusinessSkillInfo(businessSkillRefObj.id, (_businessInfo) =>
            {
                int opCount = _businessInfo?.normalOpCount ?? 0;
                OpCostRefObj opCostRefObj = businessSkillRefObj.advanceOpCostGroupRefObj?.getOpCostRefObj(opCount);

                // 消耗不足, 提示后返回
                if (opCostRefObj != null && !GCommon.isItemEnough(opCostRefObj.cost_item, true))
                    return;

                // 请求普通领悟
                NPPlayer.instance.consortComp.reqUnderstandBusinessSkill(_m_iConsortShowInfo.consortId,
                    businessSkillRefObj.id, true,
                    (_msg) =>
                    {
                        if (_msg != null && _msg.getIsProUp())
                        {
                            _m_wConsortBusinessSkillStageContainer?.playProAddSuccessSfx(businessSkillRefObj.id);                            
                        }
                        else
                        {
                            _m_wConsortBusinessSkillStageContainer?.playProAddFailSfx(businessSkillRefObj.id);                            
                        }
                    }, null);
            });
        }

        /// <summary>
        /// 前往提升按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onGotoEnhanceBtnClick(GameObject _go)
        {
            WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CONSORT_DETAIL_TAB, EUnLockConsortDetailWndTabType.INTERACTION, EUnlockConsortDetailWndInteractionPageTabType.SEND_GIFT);
        }
        
        /// <summary>
        /// 切换普通领悟和高级领悟toggle
        /// </summary>
        /// <param name="_toggle"></param>
        private void _onNormalAdvanceChgToggleClick(NPGGUIWndCommonToggleEx _toggle)
        {
            if(_toggle == null)
                return;
            
            ConsortUtil.businessSkillAdvanceComprehendIsOn = !_toggle.isOn;
            _toggle.setSelected(ConsortUtil.businessSkillAdvanceComprehendIsOn);
        }
        
        /// <summary>
        /// 当妃子经营技能信息变化时
        /// </summary>
        private void _onConsortBusinessSkillInfoChg(params object[] _objs)
        {
            if(_objs == null || _objs.Length < 2 || !(_objs[0] is long) || !(_objs[1] is ConsortBusinessSkillInfo))
                return;
            
            if(_m_iConsortShowInfo == null)
                return;
            
            long consortId = (long) _objs[0];
            ConsortBusinessSkillInfo businessSkillInfo = (ConsortBusinessSkillInfo) _objs[1];
            
            if(consortId != _m_iConsortShowInfo.consortId)
                return;
            
            _m_wConsortBusinessSkillStageContainer?.setBusinessSkillInfo(businessSkillInfo);//刷新技能信息

            // 刷新建筑总加成
            _refreshTotalAddPerTextList();
            // 刷新当前选中技能item信息
            _refreshSelectSkillItemInfo();
        }

        //消耗资源变更
        private void _onCostItemChg(params object[] _objects)
        {
            _refreshComprehendInfo();
        }
        
        //模拟点击情人普通领悟按钮
        private void _onSimulateClickNormalComprehend()
        {
            _onNormalComprehendClick(null);
        }
        
        private bool _checkBusinessSkillItemNeedShowRedTip(GGUIPrefabSubWndConsortBusinessSkillItem _itemWnd)
        {
            if (_itemWnd == null)
                return false;
            
            EConsortBusinessSkillItemState skillItemState = ConsortUtil.getBusinessSkillState(_itemWnd.businessSkillRefObj, _itemWnd.businessSkillInfo, _itemWnd.nowIntimacy);
            return skillItemState == EConsortBusinessSkillItemState.UNLOCK_NO_OP && _m_iConsortShowInfo != null && _itemWnd.businessSkillRefObj != null &&
                   NPPlayer.instance.consortComp.needShowConsortBusinessSkillRed(_m_iConsortShowInfo.consortId, _itemWnd.businessSkillRefObj.id);
        }
    }
}