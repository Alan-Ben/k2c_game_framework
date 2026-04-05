package NPGameRes.Refs.Chapter.Event;

import Common.ChapterEnum.EChapterEventType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;

import java.util.List;

public abstract class _ARefChapterEvent extends RefBase
{
    public abstract EChapterEventType getEventType();

    public abstract List<NPCommonCostItem> getAKeyForwordRewardList();
}
