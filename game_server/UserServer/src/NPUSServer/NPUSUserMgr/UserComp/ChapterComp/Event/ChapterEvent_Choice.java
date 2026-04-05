package NPUSServer.NPUSUserMgr.UserComp.ChapterComp.Event;

import ALBasicProtocolPack._IALProtocolStructure;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPGameRes.Refs.Chapter.Event.RefChapterEventChoice;
import NPGameRes.Refs.Chapter.Event.RefChapterEventChoiceOption;
import NPGameRes.Refs.Chapter.Event._ARefChapterEvent;
import NPGameRes.Refs.Chapter.RefChapterEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.ChapterComp.ChapterComponent;
import USDB.Bo.PlayerChapterEventBO;

public class ChapterEvent_Choice extends _AChapterEvent
{
    public ChapterEvent_Choice(ChapterComponent _comp, PlayerChapterEventBO _bo, RefChapterEvent _ref)
    {
        super(_comp, _bo, _ref);
    }

    @Override
    public _IALProtocolStructure makeExtraData()
    {
        return null;
    }

    /**
     * 选择选项
     * @param _optionId
     * @param _context
     * @return
     */
    public Result chooseOption(long _optionId, NPPlayerContext _context)
    {
        //查询配置
        _ARefChapterEvent detailRef = getEventRef().detailRef;
        RefChapterEventChoice refChoice = detailRef instanceof RefChapterEventChoice ? ((RefChapterEventChoice) detailRef) : null;
        if (refChoice == null)
            return CommErr.REF_NOT_FOUND;

        //检查选项是否存在
        if (!refChoice.option_id_list.contains(_optionId))
            return CommErr.PARAM_ERROR;

        RefChapterEventChoiceOption refOption = RefChapterEventChoiceOption.getMgr().get(_optionId);
        if (refOption == null)
            return CommErr.REF_NOT_FOUND;

        getUserData().gainItemList(refOption.reward_item_list, _context);

        return Result.SUCC;
    }

    @Override
    public boolean isDone()
    {
        return true;
    }
}
