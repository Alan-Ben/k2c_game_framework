using System;
using System.Collections.Generic;
using ALPackage;
using NPCommon;

namespace GOE
{
    public partial class CommonRewardDealer : RewardQueueMgr._ARewardQueueDealer
    {
        //源数据列表
        private List<NPCommon_ItemInfo> _m_srcItemList;
        //当前的处理过程对象
        private ALProcess _m_pProcessObj;
        //标题
        private string _m_titleKey;
        //关闭回调
        private Action _m_onFinish;
        
        public override bool canCurShow { get { return true; } }

        public CommonRewardDealer(List<NPCommon_ItemInfo> _srcItemList, string _titleKey, Action _closeAction)
        {
            _m_srcItemList = _srcItemList;
            _m_titleKey = _titleKey;
            _m_onFinish = _closeAction;
        }
        
        protected override void _dealShowNotice()
        {
            if(null != _m_pProcessObj)
                _m_pProcessObj.stopProcess();
            _m_pProcessObj = null;
            
            //过滤后的特殊物品列表的
            GCommon.GainItemFilterData gainItemFilterData = new GCommon.GainItemFilterData();
            //最终的物品数据列表
            List<NPCommon_ItemInfo> finalItemList = GCommon.commonDealGainSpecialItem(_m_srcItemList, ref gainItemFilterData);

            _m_pProcessObj = ALProcess.CreateProcess("main");

            //先展示特殊物品的提示界面（如果有的话），再展示最终的物品界面
            _m_pProcessObj.addDelegateProcess((_stepDone) => { showSpecial(gainItemFilterData, _stepDone);});
            
            //最终的物品数据列表
            _m_pProcessObj.addDelegateProcess((_stepDone) =>
            {
                GNodeGetItem getItemNode = GNodeGetItem.makeGetItemNode(finalItemList, _m_titleKey, true, _stepDone);
                if (getItemNode != null)
                {
                    QueueMgr.instance.AddNode(getItemNode);
                }
                else
                {
                    if (_stepDone != null) 
                        _stepDone();
                }
            });

            //获得SysInfo特殊展示道具
            if(null != gainItemFilterData && gainItemFilterData.sysInfoList != null && gainItemFilterData.sysInfoList.Count > 0)
                _m_pProcessObj.addDelegateProcess((_stepDone) => { _dealGainSysInfo(gainItemFilterData.sysInfoList, _stepDone);});
            
            _m_pProcessObj.addProcess(setDealerDone);
            _m_pProcessObj.dealProcess();
        }

        protected override void _dealHideNotice()
        {
            if(null != _m_pProcessObj)
                _m_pProcessObj.stopProcess();
            _m_pProcessObj = null;

            if (null != _m_onFinish)
                _m_onFinish();
            _m_onFinish = null;
        }

        public static void showSpecial(GCommon.GainItemFilterData _gainItemFilterData, Action _doneDelegate)
        {
            ALProcess alProcess = ALProcess.CreateProcess();
            
            //解锁玩家皮肤的提示
            if (null != _gainItemFilterData.playerSkinList && _gainItemFilterData.playerSkinList.Count > 0)
                alProcess.addDelegateProcess((_stepDone) => { _dealGainPlayerSkin(_gainItemFilterData.playerSkinList, _stepDone);});

            //骑士获得的提示
            if (null != _gainItemFilterData.heroInfoList && _gainItemFilterData.heroInfoList.Count >  0)
                alProcess.addDelegateProcess((_stepDone) => { _dealGainHero(_gainItemFilterData.heroInfoList, _stepDone);});

            //获得妃子
            if (null != _gainItemFilterData.consortList && _gainItemFilterData.consortList.Count >  0)
                alProcess.addDelegateProcess((_stepDone) => { _dealGainConsort(_gainItemFilterData.consortList, _stepDone);});

            //物品藏品
            if (null != _gainItemFilterData.equipList && _gainItemFilterData.equipList.Count > 0)
                alProcess.addDelegateProcess((_stepDone) => { _dealGainEquip(_gainItemFilterData.equipList, _stepDone);});

            //物品转换的提示
            if (null != _gainItemFilterData.itemExchangeInfoList && _gainItemFilterData.itemExchangeInfoList.Count > 0)
                alProcess.addDelegateProcess((_stepDone) => { _dealItemExchange(_gainItemFilterData.itemExchangeInfoList, _stepDone);});

            //获得装扮类道具界面（头像、头像框、气泡框、称号）
            if (null != _gainItemFilterData.showOffInfoList && _gainItemFilterData.showOffInfoList.Count > 0)
                alProcess.addDelegateProcess((_stepDone) => { _dealGainShowOffItem(_gainItemFilterData.showOffInfoList, _stepDone);});
            
            alProcess.addProcess(_doneDelegate);
            alProcess.deal();
        }
    }
}