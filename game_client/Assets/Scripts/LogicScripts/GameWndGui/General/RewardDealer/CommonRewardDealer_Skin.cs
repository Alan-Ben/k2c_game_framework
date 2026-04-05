using System;
using System.Collections.Generic;
using ALPackage;
using NPCommon;

namespace GOE
{
    public partial class CommonRewardDealer
    {
        private static void _dealGainPlayerSkin(List<NPCommon_ItemInfo> _itemList, Action _onFinish)
        {
            if (null == _itemList || _itemList.Count <= 0)
            {
                if (_onFinish != null) 
                    _onFinish();
                return;
            }
            
            
            ALProcess alProcess = ALProcess.CreateProcess();
            foreach (NPCommon_ItemInfo itemInfo in _itemList)
            {
                if (itemInfo == null)
                    continue;

                alProcess.addDelegateProcess((_done) =>
                {
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndPlayerSkinUnlock.instance, () =>
                    {
                        GGUIWndPlayerSkinUnlock.instance.setItem(itemInfo, _done);
                        GGUIWndPlayerSkinUnlock.instance.showWnd();
                    }, UINodeTagConst.C_PLAYER_SKIN_GET);
                });
            }

            alProcess.addProcess(_onFinish);
            alProcess.deal();
        }
    }
}