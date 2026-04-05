using System;
using System.Collections.Generic;
using ALPackage;
using NPCommon;
using NPEnum;

namespace GOE
{
    public partial class CommonRewardDealer
    {
        private static void _dealGainShowOffItem(List<NPCommon_ItemInfo> _itemList, Action _doneDelegate)
        {
            if (_itemList == null)
            {
                _doneDelegate?.Invoke();
                return;
            }

            ALProcess alProcess = ALProcess.CreateProcess();
            foreach (NPCommon_ItemInfo itemInfo in _itemList)
            {
                if (null == itemInfo || itemInfo.getItemType() == (int) ENPItemType.ICON)
                {
                    //配置了不展示就不展示
                    PlayerIconRefObj iconRefObj = GRefdataCoreMgr.instance.playerIconCore.getRef(itemInfo.getSubId());
                    if(null == iconRefObj || !iconRefObj.gain_is_show)
                        continue;
                }
                    
                alProcess.addDelegateProcess((_done) =>
                {
                    QueueMgr.instance.addNode_InGame_SingleWnd(NPGGUIWndGetShowOffItem.instance, () =>
                    {
                        NPGGUIWndGetShowOffItem.instance.setItem(itemInfo, _done);
                        NPGGUIWndGetShowOffItem.instance.showWnd();
                    }, UINodeTagConst.C_SHOW_OFFITEM_GET);
                });
            }

            alProcess.addProcess(_doneDelegate);
            alProcess.deal();
        }
    }
}