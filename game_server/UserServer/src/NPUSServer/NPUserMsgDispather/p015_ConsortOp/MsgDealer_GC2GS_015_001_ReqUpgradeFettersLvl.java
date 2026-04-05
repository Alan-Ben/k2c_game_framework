package  NPUSServer.NPUserMsgDispather.p015_ConsortOp;
import GC2GS.p015_ConsortOp.GC2GS_015_001_ReqUpgradeFettersLvl;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.ConsortErr;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.Consort.RefConsortFettersLvl;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;
import USLOGDB.OptBo.Opt015001ConsortUpFettersLvlBO;
public class  MsgDealer_GC2GS_015_001_ReqUpgradeFettersLvl extends NPUserMsgDealer<GC2GS_015_001_ReqUpgradeFettersLvl>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_015_001_ReqUpgradeFettersLvl _msg)
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
        
        //获取等级升级数据
        RefConsortFettersLvl fettersLvlRef = consort.getFettersInfo().getLvlRef();
        if(null == fettersLvlRef)
        {
        	_commiter.commitFailRes(ConsortErr.CONSORT_NOT_EXISTS.getCode());
        	return;
        }
        
        //检查下一个等级的配表
        int nextLvl = fettersLvlRef.lvl + 1;
        RefConsortFettersLvl nextFettersLvlRef = RefConsortFettersLvl.getMgr().get(nextLvl);
        if(null == nextFettersLvlRef)
        {
        	_commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
        	return;
        }
        
        //检查是否达到升级标准
        if(fettersLvlRef.need_consort_intimacy > consort.getIntimacy())
        {
        	_commiter.commitFailRes(ConsortErr.CONSORT_FETTERS_UP_NOT_ENABLE.getCode());
        	return;
        }
        if(fettersLvlRef.need_consort_charm > consort.getCharm())
        {
        	_commiter.commitFailRes(ConsortErr.CONSORT_FETTERS_UP_NOT_ENABLE.getCode());
        	return;
        }
        if(fettersLvlRef.need_consort_num > userData.getConsortComponent().getConsortNum())
        {
        	_commiter.commitFailRes(ConsortErr.CONSORT_FETTERS_UP_NOT_ENABLE.getCode());
        	return;
        }
        
        int preLvl = consort.getFettersInfo().getLvl();
        //更新羁绊等级
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CONSORT_UP_FETTERS_LVL);
        consort.getFettersInfo().setLvl(nextFettersLvlRef, context);
        
        _commiter.commitSucRes(US2GCWriter_015_ConsortOp.make_001_RetUpgradeFettersLvl());

        //日志
        Opt015001ConsortUpFettersLvlBO optBo = new Opt015001ConsortUpFettersLvlBO();
        optBo.setConsortId(getUSServer().getBM(), _msg.getConsortId());
        optBo.setPreLvl(getUSServer().getBM(), preLvl);
        optBo.setNewLvl(getUSServer().getBM(), consort.getFettersInfo().getLvl());
        userData.logEvent(optBo, context);
    }
}