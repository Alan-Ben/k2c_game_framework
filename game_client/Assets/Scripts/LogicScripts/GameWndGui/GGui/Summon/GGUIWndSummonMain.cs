using System;
using System.Collections.Generic;
using ALPackage;
using Spine;
using UnityEngine;

namespace GOE
{
    public class GGUIWndSummonMain : _ANPGGUIBasicResBarWnd<GGUIMonoSummonMain>
    {
        private static GGUIWndSummonMain _g_instance;
        public static GGUIWndSummonMain instance { get { return _g_instance ??= new GGUIWndSummonMain(); } }

        private long _m_lGachaPoolId;//抽卡卡池id
        private GachaPoolInfo _m_iGachaPoolInfo;//抽卡卡池信息
        
        private NPGGUIWndCommonItem _m_wOneDrawCostItem;
        private NPGGUIWndCommonItem _m_wTenDrawCostItem;
        private GGUIWndCommonFixedCd _m_wFreeDrawFixedCd;//免费抽卡固定CD展示

        private NPGGUIWndCommonToggleEx _m_wSkipDrawAnimationToggle;
        private GGUIWndSummonPublicRollRecordGrid _m_wRollRecordGrid;

        private int _m_lDrawInputMaskSerialize;//抽卡遮罩序列号
        private long _m_lShowSerializeId;
        
        public GGUIWndSummonMain() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoSummonMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoSummonMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        public GachaPoolRefObj gachaPoolRefObj
        {
            get
            {
                return _m_iGachaPoolInfo?.poolRefObj ?? GRefdataCoreMgr.instance.gachaPoolRefCore.getRef(_m_lGachaPoolId);
            }
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            _m_lGachaPoolId = GRefdataCoreMgr.instance.npGeneral.summon_gacha_pool_id;
            
            if (wnd.oneDrawCostItem != null)
                _m_wOneDrawCostItem = new NPGGUIWndCommonItem(wnd.oneDrawCostItem);

            if (wnd.tenDrawCostItem != null)
                _m_wTenDrawCostItem = new NPGGUIWndCommonItem(wnd.tenDrawCostItem);

            if (wnd.monoSkipDrawAnimationToggle != null)
            {
                _m_wSkipDrawAnimationToggle = new NPGGUIWndCommonToggleEx(wnd.monoSkipDrawAnimationToggle);
                _m_wSkipDrawAnimationToggle.clickDelegate += _onSkipToggleClick;
            }

            if (wnd.monoRollRecordGrid != null)
                _m_wRollRecordGrid = new GGUIWndSummonPublicRollRecordGrid(wnd.monoRollRecordGrid);

            // 构建免费抽卡固定CD展示
            if (wnd.freeDrawFixedCd != null)
                _m_wFreeDrawFixedCd = new GGUIWndCommonFixedCd(wnd.freeDrawFixedCd);
            
            ALUGUICommon.combineBtnClick(wnd.btnCumulativeNum, _onCumulativeNumBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnRewardProbability, _onRewardProbabilityBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnRecruit, _onRecruitBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onReturnBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnOneDraw, _onOneDrawBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnTenDraw, _onTenDrawBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnFreeDraw, _onFreeDrawBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnCumulativeNum, _onCumulativeNumBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnRewardProbability, _onRewardProbabilityBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnRecruit, _onRecruitBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onReturnBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnOneDraw, _onOneDrawBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnTenDraw, _onTenDrawBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnFreeDraw, _onFreeDrawBtnClick);
            }
            
            _m_wOneDrawCostItem?.discard();
            _m_wOneDrawCostItem = null;
            
            _m_wTenDrawCostItem?.discard();
            _m_wTenDrawCostItem = null;

            if (_m_wSkipDrawAnimationToggle != null)
            {
                _m_wSkipDrawAnimationToggle.clickDelegate -= _onSkipToggleClick;
                _m_wSkipDrawAnimationToggle.discard();
                _m_wSkipDrawAnimationToggle = null;    
            }
            
            _m_wRollRecordGrid?.discard();
            _m_wRollRecordGrid = null;

            _m_wFreeDrawFixedCd?.discard();
            _m_wFreeDrawFixedCd = null;
        }
        
        protected override void _onShowWnd()
        {
            _m_lShowSerializeId = ALSerializeOpMgr.next();

            if (_m_wSkipDrawAnimationToggle != null)
            {
                _m_wSkipDrawAnimationToggle.showWnd();
                _m_wSkipDrawAnimationToggle.setSelected(AccountSettingMgr.instance.gachaSetting.getGachaSkipShow(_m_lGachaPoolId));
            }
            _refreshWnd();
            
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_SUMMON_ONE_DRAW, _simulateSummonOneDraw);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_SUMMON_TEN_DRAW, _simulateSummonTenDraw);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_SUMMON_FREE_DRAW, _simulateSummonFreeDraw);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ITEM_COUNT_CHG_TICK_TOTAL, _onCommonItemChgTotal);
        }

        protected override void _onHideWnd()
        {
            MainCameraMono.selfInstance.closeAllInputMask(_m_lDrawInputMaskSerialize);
            _m_lShowSerializeId = ALSerializeOpMgr.next();

            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_SUMMON_ONE_DRAW, _simulateSummonOneDraw);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_SUMMON_TEN_DRAW, _simulateSummonTenDraw);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_SUMMON_FREE_DRAW, _simulateSummonFreeDraw);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ITEM_COUNT_CHG_TICK_TOTAL, _onCommonItemChgTotal);
            
            _m_wOneDrawCostItem?.hideWnd();

            _m_wTenDrawCostItem?.hideWnd();
            
            _m_wSkipDrawAnimationToggle?.hideWnd();
            
            _m_wRollRecordGrid?.hideWnd();

            _m_wFreeDrawFixedCd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wOneDrawCostItem?.resetWnd();

            _m_wTenDrawCostItem?.resetWnd();
            
            _m_wSkipDrawAnimationToggle?.resetWnd();
            
            _m_wRollRecordGrid?.resetWnd();

            _m_wFreeDrawFixedCd?.resetWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            if(_m_iGachaPoolInfo == null)
                _m_iGachaPoolInfo = NPPlayer.instance.gachaComp.getGachaPoolInfo(_m_lGachaPoolId);
            
            GachaPoolRefObj refObj = gachaPoolRefObj;

            if (refObj != null && _m_wOneDrawCostItem != null)
            {
                _m_wOneDrawCostItem.showWnd();
                _m_wOneDrawCostItem.setItem(refObj.roll_cost);
            }

            if (refObj != null && _m_wTenDrawCostItem != null)
            {
                _m_wTenDrawCostItem.showWnd();
                _m_wTenDrawCostItem.setItem(refObj.ten_roll_cost);
            }

            if (_m_wRollRecordGrid != null)
            {
                _m_wRollRecordGrid.showWnd();
                _m_wRollRecordGrid.setData(_m_lGachaPoolId);
            }

            if (_m_wFreeDrawFixedCd != null)
            {
                if(refObj != null && refObj.fixed_cd_id > 0)
                {
                    _m_wFreeDrawFixedCd.showWnd();
                    _m_wFreeDrawFixedCd.setFixedCdId(refObj.fixed_cd_id);
                }
                else
                {
                    _m_wFreeDrawFixedCd.hideWnd();
                }
            }
        }

        /// <summary>
        /// 免费抽卡按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onFreeDrawBtnClick(GameObject _go)
        {
            if (!_checkCanFreeDraw())
                return;

            _reqDraw(false);
        }

        /// <summary>
        /// 检查是否可以免费抽卡
        /// </summary>
        private bool _checkCanFreeDraw(bool _showTip = true)
        {
            GachaPoolRefObj refObj = gachaPoolRefObj;
            if (refObj == null)
                return false;

            // 存在可抽卡条件判断, 且判断未通过
            if (refObj.condition != null && refObj.condition.hasCondition && !refObj.condition.IsEnable(null))
            {
                if (_showTip)
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(refObj.condition_desc, refObj.condition_desc_args_list));
                return false;
            }

            // 检查固定CD次数是否充足
            if (refObj.fixed_cd_id <= 0)
                return false;

            NPPlayerFixedCDInfo cdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(refObj.fixed_cd_id);
            if (cdInfo == null || cdInfo.getCount() <= 0)
            {
                // if (_showTip)
                //     NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(GCommon.getItemDesc(NPEnum.ENPItemType.FIXED_CD, refObj.fixed_cd_id)));
                return false;
            }

            return true;
        }

        /// <summary>
        /// 召唤计数按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onCumulativeNumBtnClick(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndSummonCumulativeNumRewardDraw.instance, () =>
            {
                GGUIWndSummonCumulativeNumRewardDraw.instance.showWnd();
                GGUIWndSummonCumulativeNumRewardDraw.instance.setData(_m_lGachaPoolId);
            }, UINodeTagConst.C_SUMMON_CUMULATIVE_NUM_REWARD_DRAW);
        }
        
        /// <summary>
        /// 奖励概率按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onRewardProbabilityBtnClick(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndSummonRewardProbability.instance, () =>
            {
                GGUIWndSummonRewardProbability.instance.showWnd();
                GGUIWndSummonRewardProbability.instance.setData(gachaPoolRefObj);
            }, UINodeTagConst.C_SUMMON_REWARD_PROBABILITY);
        }
        
        /// <summary>
        /// 招募按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onRecruitBtnClick(GameObject _go)
        {
            QueueMgr.instance.AddNode(new GNodeRecruitMain());
        }
        
        /// <summary>
        /// 返回按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onReturnBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_SUMMON_MAIN);
        }
        
        /// <summary>
        /// 单抽按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onOneDrawBtnClick(GameObject _go)
        {
            if(!_checkCanDraw(false))
                return;

            _reqDraw(false);
        }
        
        /// <summary>
        /// 十抽按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onTenDrawBtnClick(GameObject _go)
        {
            if(!_checkCanDraw(true))
                return;
            
            _reqDraw(true);
        }

        /// <summary>
        /// 检查是否可以抽奖
        /// </summary>
        private bool _checkCanDraw(bool _isTenDraw, bool _showTip = true)
        {
            GachaPoolRefObj refObj = gachaPoolRefObj;
            if (refObj == null)
                return false;

            // 存在可抽卡条件判断, 且 判断未通过
            if (refObj.condition != null && refObj.condition.hasCondition && !refObj.condition.IsEnable(null))
            {
                if(_showTip)
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(refObj.condition_desc, refObj.condition_desc_args_list));

                return false;
            }
            
            NPCommonCostItem costItem = _isTenDraw ? refObj.ten_roll_cost : refObj.roll_cost;
            if (!GCommon.isItemEnough(costItem, _showTip))
                return false;

            return true;
        }

        /// <summary>
        /// 请求抽奖
        /// </summary>
        private void _reqDraw(bool _isTenDraw)
        {
            _m_lDrawInputMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            NPPlayer.instance.gachaComp.reqGachaRoll(_m_lGachaPoolId, _isTenDraw, (_msg) =>
            {
                if (_msg == null || gachaPoolRefObj == null || wnd == null)
                {
                    MainCameraMono.selfInstance.closeAllInputMask(_m_lDrawInputMaskSerialize);
                    return;
                }
                
                // 在表现流程后
                Action afterShowProcess = () =>
                {
                    //抽卡表现完成
                    WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.SUMMON_SHOW_DONE);
                    MainCameraMono.selfInstance.closeAllInputMask(_m_lDrawInputMaskSerialize);
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndSummonResult.instance, () =>
                    {
                        GGUIWndSummonResult.instance.showWnd();
                        GGUIWndSummonResult.instance.setData(gachaPoolRefObj, _msg);

                        _refreshWnd();//刷新一下窗口
                    }, UINodeTagConst.C_SUMMON_RESULT, true, () =>
                    {
                        GGUIWndSummonResult.instance.onClickMask();
                    });
                };

                if (!AccountSettingMgr.instance.gachaSetting.getGachaSkipShow(_m_lGachaPoolId))
                {
                    QueueMgr.instance.AddNode(new BaseOnAddContainerSceneUIWndQueueNode(EUIQueueStageType.MAIN, UINodeTagConst.C_SUMMON_SHOW_PROCESS, true
                        , false, false, null, GGUIWndSummonShowProcess.instance, true, false,
                        () =>
                        {
                            MainCameraMono.selfInstance.closeAllInputMask(_m_lDrawInputMaskSerialize);
                            
                            GGUIWndSummonShowProcess.instance.setData(_isTenDraw, _msg);
                        }, null, null, afterShowProcess));

                }
                else
                {
                    afterShowProcess();
                }
            }, () =>
            {
                MainCameraMono.selfInstance.closeAllInputMask(_m_lDrawInputMaskSerialize);
            });
        }

        /// <summary>
        /// 检查是否获得大奖
        /// </summary>
        /// <returns></returns>
        private bool _checkHasGreatReward(List<long> _gachaItemIdList)
        {
            if (_gachaItemIdList == null || _gachaItemIdList.Count <= 0)
                return false;

            foreach (long gachaItemId in _gachaItemIdList)
            {
                GachaItemRefObj gachaItemRefObj = GRefdataCoreMgr.instance.gachaItemRefCore.getRef(gachaItemId);
                if(gachaItemRefObj != null && gachaItemRefObj.if_show)//用if_show判断是否抽到大奖
                    return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// 模拟点击单抽
        /// </summary>
        private void _simulateSummonOneDraw()
        {
            _onOneDrawBtnClick(null);
        }
        
        /// <summary>
        /// 模拟点击十抽
        /// </summary>
        private void _simulateSummonTenDraw()
        {
            _onTenDrawBtnClick(null);
        }

        /// <summary>
        /// 模拟点击免费抽
        /// </summary>
        private void _simulateSummonFreeDraw()
        {
            _onFreeDrawBtnClick(null);
        }

        private void _onCommonItemChgTotal(params object[] _objs)
        {
            if(_objs == null || _objs.Length < 1 || !(_objs[0] is List<NPCommonCostItem> itemList) 
               || _m_iGachaPoolInfo == null || _m_iGachaPoolInfo.poolRefObj == null)
                return;

            bool rollCostItemChg = false;//抽卡消耗道具数量是否变化
            foreach (var chgItem in itemList)
            {
                if(chgItem == null)
                    continue;

                if ((_m_iGachaPoolInfo.poolRefObj.roll_cost != null && chgItem.item == _m_iGachaPoolInfo.poolRefObj.roll_cost.item) ||
                    (_m_iGachaPoolInfo.poolRefObj.ten_roll_cost != null && chgItem.item == _m_iGachaPoolInfo.poolRefObj.ten_roll_cost.item))
                {
                    rollCostItemChg = true;
                    break;
                }
            }

            if (rollCostItemChg)
            {
                _refreshWnd();
            }
        }
        
        /// <summary>
        /// 当跳过抽卡展示Toggle被点击
        /// </summary>
        private void _onSkipToggleClick(NPGGUIWndCommonToggleEx _toggle)
        {
            if(_m_wSkipDrawAnimationToggle == null || gachaPoolRefObj == null || 
               !GCommon.isSimpleUnlock(gachaPoolRefObj.roll_show_skip_simple_unlock_id, true))
                return;
            
            bool isOn = !_m_wSkipDrawAnimationToggle.isOn;
            
            AccountSettingMgr.instance.gachaSetting.setGachaSkipShow(_m_lGachaPoolId, isOn);
            _m_wSkipDrawAnimationToggle.setSelected(isOn);
        }
    }
}