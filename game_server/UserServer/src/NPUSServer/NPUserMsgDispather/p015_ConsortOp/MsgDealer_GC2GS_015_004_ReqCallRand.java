package NPUSServer.NPUserMsgDispather.p015_ConsortOp;

import Common.ConsortEnum.EConsortStoryType;
import Common.ConsortObj.Consort_CallRes;
import GC2GS.p015_ConsortOp.GC2GS_015_004_ReqCallRand;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.ConsortErr;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
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
import USLOGDB.OptBo.Opt015004ConsortCallRandBO;

public class MsgDealer_GC2GS_015_004_ReqCallRand extends NPUserMsgDealer<GC2GS_015_004_ReqCallRand>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_015_004_ReqCallRand _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        //本次邀约的妃子
        ConsortInfo consort;
        //先获取必定邀约的妃子
        consort = userData.getConsortComponent().popRandCallConsortId();
        //无必定邀约的妃子，则进行随机获取
        if (null == consort)
        {
            consort = userData.getConsortComponent().lookupRnd();
        }
        if (null == consort)
        {
            _commiter.commitFailRes(ConsortErr.CONSORT_NOT_EXISTS.getCode());
            return;
        }

        //检查CD
        if (!userData.hasItem(ENPItemType.LAZY_CD, RefGeneral.Ref().consort_rand_call_cd, 1))
        {
            _commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
            return;
        }

        //消耗CD
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CONSORT_RND_CALL);
        if (!userData.spendItem(ENPItemType.LAZY_CD, RefGeneral.Ref().consort_rand_call_cd, 1, context))
        {
            _commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
            return;
        }

        //CG
        boolean hasCg;
        EConsortStoryType storyType;

        int curRecordCount = (int) userData.getRecordComponent().getRecordCount(ENPPlayerRecordParam.CONSORT_RND_CALL_TIMES);
        if (curRecordCount == 0) //首次邀约
        {
            hasCg = true;
            storyType = EConsortStoryType.FIRST_CALL;
        } else //其他情况
        {
            hasCg = RefGeneral.Ref().consortCallRandCgPer.random();
            storyType = EConsortStoryType.CALL;
        }

        //随机CG事件
        ConsortTriggerStoryResult result = consort.getTriggeredCallStoryInfo().triggerStory(hasCg, storyType, context);
        RefConsortStory refConsortStory = (result == null ? null : result.getConsortStoryRef());
        boolean gainCg = (result == null ? false : result.getGainCg());
        long refConsortStoryId = (refConsortStory == null ? 0 : refConsortStory.Id());
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

        //构造邀约结果
        Consort_CallRes callRes = new Consort_CallRes();
        callRes.setConsortId(consort.getConsortId());
        callRes.setConsortStoryId(refConsortStoryId);
        callRes.setAddCharmPoint(addCharmPoint);
        callRes.setIsGainCg(gainCg);
        callRes.setChildId(childId);

        //触发事件
        Event_P_CONSORT_RAND_CALL event = new Event_P_CONSORT_RAND_CALL(context, consort.getConsortId(), addCharmPoint);
        userData.onLogicEvent(event);

        //回包处理
        _commiter.commitSucRes(US2GCWriter_015_ConsortOp.make_004_RetCallRand(callRes));

        //增加计数
        userData.getRecordComponent().addRecord(ENPPlayerRecordParam.CONSORT_RND_CALL_TIMES, 1, context);

        //日志
        Opt015004ConsortCallRandBO optBo = new Opt015004ConsortCallRandBO();
        optBo.setConsortId(getUSServer().getBM(), consort.getConsortId());
        optBo.setStoryId(getUSServer().getBM(), refConsortStoryId);
        optBo.setAddCharmPoint(getUSServer().getBM(), addCharmPoint);
        optBo.setChildId(getUSServer().getBM(), childId);
        userData.logEvent(optBo, context);
    }
}