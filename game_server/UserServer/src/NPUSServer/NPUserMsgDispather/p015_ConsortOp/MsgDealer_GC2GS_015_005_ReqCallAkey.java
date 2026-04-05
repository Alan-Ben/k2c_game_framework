package  NPUSServer.NPUserMsgDispather.p015_ConsortOp;

import Common.ConsortEnum.EConsortStoryType;
import Common.ConsortObj.Consort_CallRes;
import GC2GS.p015_ConsortOp.GC2GS_015_005_ReqCallAkey;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.ConsortErr;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerPropertyType;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.Consort.RefConsortCg;
import NPGameRes.Refs.Consort.RefConsortStory;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_CONSORT_RAND_CALL;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Child.ChildInfo;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortTriggeredCallStory.ConsortTriggerStoryResult;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;
import NPUSServer.USLog;
import USLOGDB.OptBo.Opt015005ConsortCallAkeyBO;

import java.util.ArrayList;

/***
 * 一键邀约：固定10次邀约处理
 * @author mj
 *
 */
public class  MsgDealer_GC2GS_015_005_ReqCallAkey extends NPUserMsgDealer<GC2GS_015_005_ReqCallAkey>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_015_005_ReqCallAkey _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        //至少拥有一个家人
        if(userData.getConsortComponent().isConsortEmpty())
        {
        	_commiter.commitFailRes(ConsortErr.CONSORT_NOT_EXISTS.getCode());
        	return;
        }
        
        //CD允许的上限
        long cdCount = userData.getLazyCDComponent().getItemCount(RefGeneral.Ref().consort_rand_call_cd);
        //玩家属性允许的上限
        long playerAllowCount = userData.getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.CONSORT_RANDCALL_ENERGY_LIMIT);
        long costCdCount = Math.min(cdCount, playerAllowCount);
        if(costCdCount <= 0)
        {
        	_commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
        	return;
        }
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CONSORT_AKEY_CALL);
        //扣除CD
        if(!userData.getLazyCDComponent().spendItem(RefGeneral.Ref().consort_rand_call_cd, costCdCount, context))
        {
        	_commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
        	return;
        }

        int curRecordCount = (int) userData.getRecordComponent().getRecordCount(ENPPlayerRecordParam.CONSORT_RND_CALL_TIMES);

        //构造结果数据
        ArrayList<Consort_CallRes> resList = new ArrayList<>();
        //处理邀约
        for(int i = 0; i < costCdCount; i++)
        {
        	//本次邀约的妃子
            ConsortInfo consort = null;
            //先获取必定邀约的妃子
            consort = userData.getConsortComponent().popRandCallConsortId();
            //无必定邀约的妃子，则进行随机获取
            if(null == consort)
            {
            	consort = userData.getConsortComponent().lookupRnd();
            }
        	if(null == consort)
        	{
        		USLog.error(getUSServer(), "player:{} consort akey lookup rand fail.", userData.getCid());
        		continue;
        	}

            //增加邀约次数
            curRecordCount++;
            
            //CG
            boolean hasCg;
            EConsortStoryType storyType = EConsortStoryType.CALL;

            if(curRecordCount == 1) //首次邀约
            {
                hasCg = true;
                storyType = EConsortStoryType.FIRST_CALL;
            }
            else
            {
                hasCg = RefGeneral.Ref().consortCallRandCgPer.random();
            }

            //随机CG事件
            ConsortTriggerStoryResult result = consort.getTriggeredCallStoryInfo().triggerStory(hasCg, storyType, context);
            RefConsortStory refConsortStory = (result == null ? null : result.getConsortStoryRef());
            boolean gainCg = (result == null ? false : result.getGainCg());
            long refConsortStoryId = refConsortStory == null ? 0 : refConsortStory.Id();
            //计算获得的加护力
            long addCharmPointPer = 0;
            if (refConsortStory != null)
            {
                RefConsortCg refConsortCg = RefConsortCg.getMgr().get(refConsortStory.unlock_cg);
                if (null != refConsortCg)
                {
                    addCharmPointPer = refConsortCg.add;
                }
            }

            long addCharmPer = consort.getCharmPointPer();
            long charmPointExtraAdd = consort.getCharmPointExtraAdd();
            long addCharmPoint = (long) Math.ceil(consort.getCharm() * (10000 + addCharmPointPer + addCharmPer) / 10000f) + charmPointExtraAdd;
            consort.incrCharmPoint(addCharmPoint, context);

            //检查随机邀约生成子嗣
            long childId = 0;
            ChildInfo child = consort.checkRandCallBirthChild(hasCg, context);
            if(null != child)
            {
                childId = child.getChildId();
            }

            //触发事件
            Event_P_CONSORT_RAND_CALL event = new Event_P_CONSORT_RAND_CALL(context, consort.getConsortId(), addCharmPoint);
            userData.onLogicEvent(event);
            
            //构造邀约结果
            Consort_CallRes callRes = new Consort_CallRes();
            callRes.setConsortId(consort.getConsortId());
            callRes.setConsortStoryId(refConsortStoryId);
            callRes.setAddCharmPoint(addCharmPoint);
            callRes.setIsGainCg(gainCg);
            callRes.setChildId(childId);
            
            resList.add(callRes);

            //日志
            Opt015005ConsortCallAkeyBO optBo = new Opt015005ConsortCallAkeyBO();
            optBo.setConsortId(getUSServer().getBM(), consort.getConsortId());
            optBo.setStoryId(getUSServer().getBM(), refConsortStoryId);
            optBo.setAddCharmPoint(getUSServer().getBM(), addCharmPoint);
            optBo.setChildId(getUSServer().getBM(), childId);
            userData.logEvent(optBo, context);
        }
        
        _commiter.commitSucRes(US2GCWriter_015_ConsortOp.make_005_RetCallAkey(resList));

        //增加计数
        userData.getRecordComponent().addRecord(ENPPlayerRecordParam.CONSORT_RND_CALL_TIMES, resList.size(), context);
    }
}