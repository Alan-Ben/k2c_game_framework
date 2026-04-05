using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 关卡前进表现接口
    /// </summary>
    public interface _IChapterEffectInterface
    {
        /// <summary>
        /// 切换到默认表现
        /// </summary>
        void chgIdleEffect();

        /// <summary>
        /// 关卡前进表现
        /// </summary>
        /// <param name="_doneAction"></param>
        void forwardToChapter(float _fade, long _sfxId, long _tdSfxId, int _coefficient, long _rewardExp, long _rewardPlayerExp, long _eventId, List<NPCommon.NPCommon_ItemInfo> _itemList);
        
        /// <summary>
        /// 快速前进表现
        /// </summary>
        /// <param name="_doneAction"></param>
        void quickForwardToChapter(float _fade, long _sfxId, long _tdSfxId, int _coefficient, long _rewardExp, long _rewardPlayerExp, long _eventId, List<NPCommon.NPCommon_ItemInfo> _itemList);

        /// <summary>
        /// 播放前进视频状态动画
        /// </summary>
        /// <param name="_forwardState"></param>
        void playForwardVideoAniState(EChapterVideoForwardState _forwardState);
        
        /// <summary>
        /// 关卡boss战前摇表现
        /// </summary>
        /// <param name="_doneAction"></param>
        void forwardToBossPer(Action _doneAction);
        
        /// <summary>
        /// 关卡boss战前连线表现
        /// </summary>
        /// <param name="_doneAction"></param>
        void forwardToBossWait();
        
        /// <summary>
        /// 关卡boss战表现
        /// </summary>
        /// <param name="_doneAction"></param>
        void forwardToBoss(ChapterRefObj _chapterRef, Action _doneAction);
        
        /// <summary>
        /// 展示进度条上的动画
        /// </summary>
        /// <param name="_doneAction"></param>
        void showDialogSliderTip(long _pointId, Action _doneAction);
        
        /// <summary>
        /// 剧情提示表现
        /// </summary>
        /// <param name="_doneAction"></param>
        void showDialogTipEffect();

        /// <summary>
        /// 现实事件提示表现
        /// </summary>
        void showEventTipEffect();
        
        /// <summary>
        /// node移动表现
        /// </summary>
        /// <param name="_doneAction"></param>
        void _dealChapterNodeMoveEffect();
        
        /// <summary>
        /// node移动Boss表现
        /// </summary>
        /// <param name="_doneAction"></param>
        void _dealChapterNodeMoveBossEffect();
    }
}