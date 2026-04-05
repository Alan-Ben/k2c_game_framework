using System;
using System.Collections.Generic;
using ALPackage;
using NPCommon;
using NPEnum;

namespace GOE
{
    public partial class CommonRewardDealer
    {
        private static void _dealGainEquip(List<NPCommon_ItemInfo> _itemList, Action _onFinish)
        {
            if (_itemList == null || _itemList.Count == 0)
            {
                if (_onFinish != null) 
                    _onFinish();
                return;
            }

            _itemList.Sort(_sortGainEquipList);

            
            ALProcess alProcess = ALProcess.CreateProcess();
            foreach (NPCommon_ItemInfo itemInfo in _itemList)
            {
                if (itemInfo == null)
                    continue;

                alProcess.addDelegateProcess((_done) =>
                {
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndEquipGet.instance, () =>
                    {
                        GGUIWndEquipGet.instance.setInfo(itemInfo, _done);
                        GGUIWndEquipGet.instance.showWnd();
                    }, UINodeTagConst.C_EQUIP_GET);
                });
            }

            alProcess.addProcess(_onFinish);
            alProcess.deal();
        }
        
        //排序列表：品质从高到低排序 > 根据藏品ID排序
        private static int _sortGainEquipList(NPCommon_ItemInfo _a, NPCommon_ItemInfo _b)
        {
            if (_a == null || _b == null)
                return 0;

            EQuality qualityA = GCommon.getItemQuality(ENPItemType.EQUIP, _a.getSubId());
            EQuality qualityB = GCommon.getItemQuality(ENPItemType.EQUIP, _b.getSubId());
            if (qualityA.CompareTo(qualityB) != 0)
                return -(qualityA.CompareTo(qualityB));

            return _a.getSubId().CompareTo(_b.getSubId());
        }
    }
}