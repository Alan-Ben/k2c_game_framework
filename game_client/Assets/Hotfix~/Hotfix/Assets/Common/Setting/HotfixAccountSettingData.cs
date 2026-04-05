using System;

namespace Hotfix
{
    public class HotfixAccountSettingData
    {
        public HotfixAccountSettingData(){}

        public TileMatchEnum.ETileMatch_ModeType tileMatchModelType;//三消活动游戏模式类型
        public NumMergeEnum.ENumMerge_ModeType numMergeModeType;//2048活动游戏模式类型

        public long latestFirstEnterTileMatchDialogActivityStartTimeMs;//最近的一个首次进入三消活动对话的活动开始时间戳
        public long latestFirstEnterNumMergeDialogActivityStartTimeMs;//最近的一个首次进入2048活动对话的活动开始时间戳
    }
}