package  NPUSServer.NPUserMsgDispather.p015_ConsortOp;

import GC2GS.p015_ConsortOp.GC2GS_015_003_ReqUpgradeBlessSkill;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.ConsortErr;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.Consort.RefConsortBlessSkillLvl;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortBless.ConsortBlessSkillInfo;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;
import USLOGDB.OptBo.Opt015003ConsortUpBlessSkillBO;
public class  MsgDealer_GC2GS_015_003_ReqUpgradeBlessSkill extends NPUserMsgDealer<GC2GS_015_003_ReqUpgradeBlessSkill>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_015_003_ReqUpgradeBlessSkill _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        //检查家人数据
        ConsortInfo consort = userData.getConsortComponent().lookup(_msg.getConsortId());
        if(null == consort)
        {
        	_commiter.commitFailRes(ConsortErr.CONSORT_NOT_EXISTS.getCode());
        	return;
        }
        
        //检查技能数据
        ConsortBlessSkillInfo skill = consort.getBlessSkillInfoMgr().lookup(_msg.getSkillId());
        if(null == skill)
        {
        	_commiter.commitFailRes(ConsortErr.CONSORT_SKILL_NOT_EXISTS.getCode());
        	return;
        }
        if(null == skill.getLvlRef())
        {
        	_commiter.commitFailRes(CommErr.DATA_STATE_ERR.getCode());
        	return;
        }
        
        //检查技能等级
        int nextLvl = skill.getLvl() + 1;
        RefConsortBlessSkillLvl nextLvlRef = skill.getRef().getLevelMapMgr().getLevelData(nextLvl);
        if(null == nextLvlRef)
        {
        	_commiter.commitFailRes(ConsortErr.CONSORT_SKILL_LVL_FULL.getCode());
        	return;
        }
        
        //检查消耗
        if(consort.getCharmPoint() < skill.getLvlRef().cost_skill_point)
        {
        	_commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
        	return;
        }
        
        int preLvl = skill.getLvl();
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CONSORT_UP_BLESS_SKILL);
        //扣除对应的加护点
        consort.descCharmPoint(skill.getLvlRef().cost_skill_point, context);
        //设置新加护技能等级
        skill.setLvl(nextLvlRef, context);
        
        _commiter.commitSucRes(US2GCWriter_015_ConsortOp.make_003_RetUpgradeBlessSkill());

        //日志
        Opt015003ConsortUpBlessSkillBO optBo = new Opt015003ConsortUpBlessSkillBO();
        optBo.setConsortId(getUSServer().getBM(), _msg.getConsortId());
        optBo.setSkillId(getUSServer().getBM(), _msg.getSkillId());
        optBo.setPreLvl(getUSServer().getBM(), preLvl);
        optBo.setNewLvl(getUSServer().getBM(), skill.getLvl());
        userData.logEvent(optBo, context);
    }
}