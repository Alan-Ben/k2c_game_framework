using Common.ChapterEnum;
using UnityEngine;

namespace GOE
{
    public partial class GGUIWndChapterBossMain
    {
        //ai控制自动鼓舞任务
        public void aiControlNext()
        {
            if(null == _m_chapterRef || _m_curWndType == EChapterBossWndType.FAIL)
            {
                _stopAutoControl();
                return;
            }
            
            // //鼓舞状态帮他鼓舞,点战斗
            // if (_m_curWndType == EChapterBossWndType.INSPIRE)
            // {
            //     //战力大于boss了直接点击战斗
            //     if (GGUIWndChapterBossInspire.instance.curCanFightBoss())
            //     {
            //         //模拟帮他打boss
            //         GGUIWndChapterBossInspire.instance.clickBattle();
            //     }
            //     else
            //     {
            //         //战力小于boss根据鼓舞限制开始自动鼓舞
            //         //优先金币鼓舞次数判断
            //         if (NPPlayer.instance.chapterComp.getInspireTimes(EChapterInspireType.GOLD) <
            //             NPPlayer.instance.chapterComp.forwardLogicMgr.getMaxInspireTimes(EChapterInspireType.GOLD))
            //         {
            //             //模拟帮他金币鼓舞
            //             GGUIWndChapterBossInspire.instance.dealInspireGold(null, _stopAutoControl);
            //         }
            //         //道具鼓舞次数判断
            //         else if (NPPlayer.instance.chapterComp.getInspireTimes(EChapterInspireType.ITEM) < 
            //                  NPPlayer.instance.chapterComp.forwardLogicMgr.getMaxInspireTimes(EChapterInspireType.ITEM))
            //         {
            //             //模拟帮他道具鼓舞
            //             GGUIWndChapterBossInspire.instance.dealInspireItem(null, _stopAutoControl);
            //         }
            //         else
            //         {
            //             //提示战力不够
            //             NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.chapter_power_less_than_boss);
            //
            //             _stopAutoControl();
            //         }
            //     }
            // }
            // //奖励状态帮他关闭奖励弹窗
            // else if (_m_curWndType == EChapterBossWndType.REWARD)
            // {
            //     QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Chapter.C_CHAPTER_BOSS);
            // }
        }
        
        private void _stopAutoControl()
        {
            //关闭自动
            NPPlayer.instance.chapterComp.forwardLogicMgr.stopAutoForwardToChapter();
        }
    }
}