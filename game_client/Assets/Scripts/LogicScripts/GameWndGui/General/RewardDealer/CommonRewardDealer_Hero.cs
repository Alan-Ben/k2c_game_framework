using System;
using System.Collections.Generic;
using ALPackage;
using NPCommon;

namespace GOE
{
    public partial class CommonRewardDealer
    {
        private static void _dealGainHero(List<NPCommon_ItemInfo> _itemList, Action _onFinish)
        {
            List<HeroRefObj> heroRefList = _itemList.toHeroRefList();
            heroRefList.Sort(_sortShowHeroRef);
            
            ALProcess alProcess = ALProcess.CreateProcess();
            foreach (HeroRefObj itemInfo in heroRefList)
            {
                if (itemInfo == null)
                    continue;

                //获得前播放一段对话
                if(itemInfo.gain_hero_dialog_id > 0)
                {
                    alProcess.addDelegateProcess((_done) =>
                    {
                        GCommon.enterDialogueNode(itemInfo.gain_hero_dialog_id, _done);
                    });
                }
                    
                alProcess.addDelegateProcess((_done) =>
                {
                    QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndHeroGet.instance, UINodeTagConst.C_HERO_GET, null, () =>
                    {
                        GGUIWndHeroGet.instance.setInfo(itemInfo, _done);
                    }, 0);
                });
            }

            alProcess.addProcess(_onFinish);
            alProcess.deal();
        }
        
        //处理获得骑士排序，第一优先级：按星级从高到低展示，第二优先级：按角色id顺序展示
        private static int _sortShowHeroRef(HeroRefObj _a, HeroRefObj _b)
        {
            if (_a == null || _b == null)
                return 0;

            // if (_a.init_star.CompareTo(_b.init_star) != 0)
            //     return -(_a.init_star.CompareTo(_b.init_star));
            // else
            return _a.id.CompareTo(_b.id);
        }
    }
}