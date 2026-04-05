using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 关卡地图表现接口
    /// </summary>
    public interface _IChapterMapEffectInterface
    {
        /// <summary>
        /// 播放章节完成表现
        /// </summary>
        void playChapterCompleteEffect(ChapterRefObj _chapterRef, int _targetNodeIndex);

        /// <summary>
        /// 播放自动前进表现
        /// </summary>
        void dealAutoForwardToChapter(int _targetNodeIndex, float _fade, int _coefficient, long _rewardExp, long _rewardPlayerExp, List<NPCommon.NPCommon_ItemInfo> _itemList);
        
        /// <summary>
        /// 播放自动前进表现
        /// </summary>
        void dealAutoBossToChapter(Action _playDone);
        
        /// <summary>
        /// 进入自动状态
        /// </summary>
        void enterAutoState();
        
        /// <summary>
        /// 退出自动状态
        /// </summary>
        void quitAutoState();
    }
}