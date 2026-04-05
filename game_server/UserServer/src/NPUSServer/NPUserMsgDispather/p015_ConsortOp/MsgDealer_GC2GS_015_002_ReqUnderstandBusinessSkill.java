package  NPUSServer.NPUserMsgDispather.p015_ConsortOp;

import GC2GS.p015_ConsortOp.GC2GS_015_002_ReqUnderstandBusinessSkill;
import MJLog.MJEventLog;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.ConsortErr;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.Common.RefOpCost;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortBusiness.ConsortBusinessSkillInfo;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;
import USLOGDB.OptBo.Opt015002ConsortUpBusinessSkillBO;
public class  MsgDealer_GC2GS_015_002_ReqUnderstandBusinessSkill extends NPUserMsgDealer<GC2GS_015_002_ReqUnderstandBusinessSkill>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_015_002_ReqUnderstandBusinessSkill _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        ConsortInfo consort = userData.getConsortComponent().lookup(_msg.getConsortId());
        if(null == consort)
        {
        	_commiter.commitFailRes(ConsortErr.CONSORT_NOT_EXISTS.getCode());
        	return;
        }
        
        ConsortBusinessSkillInfo skill = consort.getBusinessSkillMgr().lookup(_msg.getSkillId());
        if(null == skill)
        {
        	_commiter.commitFailRes(ConsortErr.CONSORT_SKILL_NOT_EXISTS.getCode());
        	return;
        }
        
        //检查解锁条件
        if(skill.getRef().unlock_need_intimacy > consort.getIntimacy())
        {
        	_commiter.commitFailRes(ConsortErr.CONSORT_SKILL_NOT_UNLOCK.getCode());
        	return;
        }
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CONSORT_UP_BUSINESS_SKILL);
        
        if(_msg.getIsAdvanced()) //高级领悟
        {
        	_advanceUnderstand(_commiter, skill, context);
        }
        else //普通领悟
        {
        	_normalUnderstand(_commiter, skill, context);
        }

		//MJ日志
    }
    
    /**
     * 普通领悟
     */
    private void _normalUnderstand(_ANPUSUserBasicMsgItem _commiter, ConsortBusinessSkillInfo _skill, NPPlayerContext _context)
    {
    	//检查消耗
    	int curOpCostCount = _skill.getNormalOpCount() + 1;
    	RefOpCost opCostRef = RefOpCost.getMgr().getOpCostRef(_skill.getRef().normal_cost_group_id,curOpCostCount);
    	if(null == opCostRef)
    	{
    		_commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
    		return;
    	}
    	
    	if(!_commiter.getUserData().hasItem(opCostRef.cost_item))
    	{
    		_commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
    		return;
    	}
    	
    	if(!_commiter.getUserData().spendItem(opCostRef.cost_item, _context))
    	{
    		_commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
    		return;
    	}
    	
    	//记录次数
		_skill.incrOpCount(false, _context);

    	int preProAdd = _skill.getProAdd();
    	//更新本次概率
		int curProAdd = RefGeneral.Ref().randomConsortSkillProAdd(_skill.getOpCount(), _skill.getRef().normal_add_pro_group_id);
    	//只记录高过本次加成的数值
    	boolean isUp = false;
    	if(_skill.getProAdd() < curProAdd)
    	{
    		_skill.setProAdd(curProAdd, _context);
    		
    		isUp = true;
    	}
    	else //未升级也要做一次数据推送
    	{
    		_commiter.getUserData().sendMsgToGC(US2GCWriter_015_ConsortOp.make_058_OnBusinessSkillChg(_skill));
    	}
    
    	_commiter.commitSucRes(US2GCWriter_015_ConsortOp.make_002_RetUnderstandBusinessSkill(isUp));

        //日志
        Opt015002ConsortUpBusinessSkillBO optBo = new Opt015002ConsortUpBusinessSkillBO();
        optBo.setConsortId(getUSServer().getBM(), _skill.getConsortId());
        optBo.setSkillId(getUSServer().getBM(), _skill.getSkillId());
        optBo.setIsAdvanced(getUSServer().getBM(), false);
        optBo.setOpCount(getUSServer().getBM(), _skill.getNormalOpCount());
        optBo.setPreProAdd(getUSServer().getBM(), preProAdd);
        optBo.setNewProAdd(getUSServer().getBM(), _skill.getProAdd());
        _commiter.getUserData().logEvent(optBo, _context);

        //MJ日志
        MJEventLog.logLoverRefine(_commiter.getUserData(), _skill.getConsortId(), _skill.getConsort().getIntimacy(),
                1, _skill.getSkillId(), isUp ? 1 : 0, preProAdd, _skill.getProAdd(), _context.getContextId());
    }
    /**
     * 高级领悟
     */
    private void _advanceUnderstand(_ANPUSUserBasicMsgItem _commiter, ConsortBusinessSkillInfo _skill, NPPlayerContext _context)
    {
    	//检查消耗
    	int curOpCostCount = _skill.getNormalOpCount() + 1;
    	RefOpCost opCostRef = RefOpCost.getMgr().getOpCostRef(_skill.getRef().advance_cost_group_id, curOpCostCount);
    	if(null == opCostRef)
    	{
    		_commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
    		return;
    	}
    	
    	if(!_commiter.getUserData().hasItem(opCostRef.cost_item))
    	{
    		_commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
    		return;
    	}
    	
    	if(!_commiter.getUserData().spendItem(opCostRef.cost_item, _context))
    	{
    		_commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
    		return;
    	}
    	
    	//记录次数
    	_skill.incrOpCount(true, _context);

    	int preProAdd = _skill.getProAdd();
    	//更新本次概率
    	int curProAdd = RefGeneral.Ref().randomConsortSkillProAdd(_skill.getOpCount(), _skill.getRef().advance_add_pro_group_id);
    	//只记录高过本次加成的数值
    	boolean isUp = false;
    	if(_skill.getProAdd() < curProAdd)
    	{
    		_skill.setProAdd(curProAdd, _context);
    		
    		isUp = true;
    	}
    	else //未升级也要做一次数据推送
    	{
    		_commiter.getUserData().sendMsgToGC(US2GCWriter_015_ConsortOp.make_058_OnBusinessSkillChg(_skill));
    	}
    
    	_commiter.commitSucRes(US2GCWriter_015_ConsortOp.make_002_RetUnderstandBusinessSkill(isUp));

        //日志
        Opt015002ConsortUpBusinessSkillBO optBo = new Opt015002ConsortUpBusinessSkillBO();
        optBo.setConsortId(getUSServer().getBM(), _skill.getConsortId());
        optBo.setSkillId(getUSServer().getBM(), _skill.getSkillId());
        optBo.setIsAdvanced(getUSServer().getBM(), true);
        optBo.setOpCount(getUSServer().getBM(), _skill.getAdvanceOpCount());
        optBo.setPreProAdd(getUSServer().getBM(), preProAdd);
        optBo.setNewProAdd(getUSServer().getBM(), _skill.getProAdd());
        _commiter.getUserData().logEvent(optBo, _context);

        //MJ日志
        MJEventLog.logLoverRefine(_commiter.getUserData(), _skill.getConsortId(), _skill.getConsort().getIntimacy(),
                2, _skill.getSkillId(), isUp ? 1 : 0, preProAdd, _skill.getProAdd(), _context.getContextId());
    }
}