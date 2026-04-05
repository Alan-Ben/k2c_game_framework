package NPUSServer.NPUserMsgDispather.p042_GuildRelatedOp;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.MarsEnum.EMarsBuildingType;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_041_001_AddMarsHelp;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_RetLongInfo;
import GC2GS.p042_GuildRelatedOp.GC2GS_042_001_ReqSendMarsHelp;
import NPCommon.ErrMain.GuildErr;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerPropertyType;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter._ATGuildUserMsgRedirectCommiter;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsComp._IGuildMarsHelp;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_042_GuildRelatedOp;
import USLOGDB.OptBo.Opt042001GuildMarsSendHelpBO;

public class MsgDealer_GC2GS_042_001_ReqSendMarsHelp extends NPUserMsgDealer<GC2GS_042_001_ReqSendMarsHelp>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_042_001_ReqSendMarsHelp _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        if(userData.getGuildComponent().getGuildId() <= 0)
        {
            _commiter.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }
        
        //检查对应的建筑
        if(null == userData.getMarsBuildingComponent().lookupBuildingByType(EMarsBuildingType.HELP))
        {
        	_commiter.commitFailRes(GuildErr.GUILD_MARS_HELP_BUILDING_NOT_FOUND.getCode());
        	return;
        }
        
        //求助数据
        _IGuildMarsHelp helpObj = userData.getMarsComponent().getGuildMarsHelpMgr().lookupGuildHelp(_msg.getObjType(), _msg.getObjId());
        if(null == helpObj)
        {
        	_commiter.commitFailRes(GuildErr.GUILD_MARS_HELP_NOT_FOUND.getCode());
        	return;
        }
        
        if(!helpObj.canSendHelp(_msg.getObjType()))
        {
        	_commiter.commitFailRes(GuildErr.GUILD_MARS_HELP_OBJ_FAIL.getCode());
        	return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GUILD_MARS_HELP_SEND);

        int dealLimit = (int) userData.getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.MARS_HELP_ACCEPT_LIMIT);
        int dealSecs = (int) userData.getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.MARS_HELP_ACCEPT_LIMIT);

        GuildOp_041_001_AddMarsHelp addInfo = new GuildOp_041_001_AddMarsHelp();
        addInfo.setDealLimit(dealLimit);
        addInfo.setDealSecs(dealSecs);
        addInfo.setUsHelpDBId(helpObj.getObjId());

        //尝试获取额外信息
        _IALProtocolStructure extData = helpObj.makeExt();
        if(null != extData)
        {
            addInfo.setHelpExData(extData.makePackage());
        }

        //此时转发消息开启
        //转化为统一跨服消息进行处理
        getUSServer().dealGuildMsgByRedirectCommiter(
                new _ATGuildUserMsgRedirectCommiter<GuildOp_RetLongInfo>(_commiter) {
                    @Override
                    protected GuildOp_RetLongInfo _createNewTmpObj() {
                        return new GuildOp_RetLongInfo();
                    }

                    @Override
                    protected void _dealTmpCommitMsg(_ANPUSUserBasicMsgItem _commiter, GuildOp_RetLongInfo _retMsg) {

                        //更新玩家数据
                        boolean res = userData.getMarsComponent().getGuildMarsHelpMgr().sendGuildHelp(_msg.getObjType(), helpObj.getObjId(), _retMsg.getNum());
                        if(!res)
                        {
                            userData.getGuildComponent().sendRmvMarsHelpRPC(_retMsg.getNum(), false);

                            _commiter.commitFailRes(GuildErr.GUILD_MARS_HELP_SEND_FAIL.getCode());

                            return;
                        }

                        //日志数据
                        Opt042001GuildMarsSendHelpBO optBo = new Opt042001GuildMarsSendHelpBO();
                        optBo.setGuid(getUSServer().getBM(), userData.getGuildComponent().getGuildId());
                        optBo.setObjType(getUSServer().getBM(), _msg.getObjType().ordinal());
                        optBo.setObjId(getUSServer().getBM(), _msg.getObjId());
                        _commiter.getUserData().logEvent(optBo, context);

                        //返回操作结果
                        _commiter.commitSucRes(US2GCWriter_042_GuildRelatedOp.make_001_RetSendMarsHelp());
                    }
                },
                _commiter.getUserData().getCid(),
                _commiter.getUserData().getGuildComponent().getGuildId(),
                _msg,
                addInfo
        );
    }
}
