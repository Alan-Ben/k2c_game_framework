using System;
using System.Collections.Generic;
using ALPackage;
using NPCommon;

namespace GOE
{
    public partial class CommonRewardDealer
    {
        private static void _dealItemExchange(List<NPCommon_ItemInfo> _itemList, Action _doneDelegate)
        {
            if (_itemList == null)
            {
                if (_doneDelegate != null) 
                    _doneDelegate();
                return;
            }

            ALProcess alProcess = ALProcess.CreateProcess();
            foreach (NPCommon_ItemInfo commonItemInfo in _itemList)
            {
                List<NPCommon_ItemInfo> finalItems = getFinalExchangedItem(commonItemInfo, out ItemExchangeRefObj exchangeRef);
                    
                alProcess.addDelegateProcess((_done) =>
                {
                    QueueMgr.instance.addNode_InGame_SingleWnd(NPGGUIWndCommonAutoConversion.instance, () =>
                    {
                        NPGGUIWndCommonAutoConversion.instance.showWnd(commonItemInfo?.toCommonItemData(), finalItems.GetFirst()?.toCommonItemData(), 
                            TextTranslate.instance.getLanguage(exchangeRef?.desc, exchangeRef?.desc_args), _done);
                    }, UINodeTagConst.C_ITEM_AUTO_CONVERSION);
                });
            }

            alProcess.addProcess(_doneDelegate);
            alProcess.deal();
        }
        
        private static List<NPCommon_ItemInfo> getFinalExchangedItem(NPCommon_ItemInfo _itemInfo, out ItemExchangeRefObj _exchangeRef)
        {
            _exchangeRef = null;
            if (_itemInfo == null)
                return null;

            int safeNum = 1000;
            List<NPCommon_ItemInfo> finalItemList = new List<NPCommon_ItemInfo>();
            List<NPCommon_ItemInfo> checkList = new List<NPCommon_ItemInfo> { _itemInfo };
            while (checkList.Count > 0)
            {
                if (safeNum-- <= 0)
                {
                    ALLog.Error("【ItemExchange】物品转换循环次数超过 1000 次，检查逻辑是否有问题");
                    break;
                }
                
                NPCommon_ItemInfo checkItem = checkList.GetFirstAndRemove();
                if (checkItem == null)
                    continue;

                if (!willItemExchange(checkItem))
                {
                    finalItemList.Add(checkItem);
                    continue;
                }
                
                byte[] exchangeData = checkItem.getExtData();
                Common_ItemExchangeInfo exchangeInfo = new Common_ItemExchangeInfo();
                exchangeInfo.readPackage(exchangeData);
                checkList.AddRange(exchangeInfo.getItemList() ?? new List<NPCommon_ItemInfo>(0));
                _exchangeRef = GRefdataCoreMgr.instance.itemExchangeCore.getRef(exchangeInfo.getExchangeRefId()); 
            }

            return finalItemList;
        }
        
        private static bool willItemExchange(NPCommon_ItemInfo _itemInfo)
        {
            byte[] exchangeData = _itemInfo?.getExtData();
            if (exchangeData == null || exchangeData.Length == 0)
                return false;

            return true;
        }
    }
}