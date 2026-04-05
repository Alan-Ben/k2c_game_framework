package NPUSServer.NPUSUserMgr.UserComp.AnecdoteComp.Impl;

import ALBasicProtocolPack._IALProtocolStructure;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPGameRes.Refs.Anecdote.RefAnecdoteEvent;
import NPGameRes.Refs.Anecdote.RefAnecdoteEventChoice;
import NPGameRes.Refs.Anecdote.RefAnecdoteEventChoiceOption;
import NPGameRes.Refs.Anecdote._ARefAnecdoteEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.AnecdoteComp.AnecdoteComponent;
import NPUSServer.NPUSUserMgr.UserComp.AnecdoteComp._AAnecdoteEvent;
import USDB.Bo.PlayerAnecdoteEventBO;

public class AnecdoteEvent_Choice extends _AAnecdoteEvent
{
    public AnecdoteEvent_Choice(AnecdoteComponent _comp, PlayerAnecdoteEventBO _bo, RefAnecdoteEvent _ref)
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
        _ARefAnecdoteEvent detailRef = getEventRef().detailRef;
        RefAnecdoteEventChoice refChoice = detailRef instanceof RefAnecdoteEventChoice ? ((RefAnecdoteEventChoice) detailRef) : null;
        if (refChoice == null)
            return CommErr.REF_NOT_FOUND;

        //检查选项是否存在
        if (!refChoice.option_id_list.contains(_optionId))
            return CommErr.PARAM_ERROR;

        RefAnecdoteEventChoiceOption refOption = RefAnecdoteEventChoiceOption.getMgr().get(_optionId);
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
