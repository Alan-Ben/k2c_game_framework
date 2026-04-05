package CrossTeamServer.CrossTeam;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import AllRpcData.US_Service.Team.UsNotifyRemoveGroup;
import AllRpcData.US_Service.Team.UsNotifySyncGroupTeam;
import CTSDB.Bo.CrossTeamApplyBO;
import CTSDB.Bo.CrossTeamBO;
import CTSDB.Bo.CrossTeamMemberBO;
import Common.CrossTeamEnum.ENPCrossTeamJoinCond;
import Common.CrossTeamEnum.ENPCrossTeamJoinType;
import Common.CrossTeamObj.CrossTeam_BaseInfo;
import Common.CrossTeamObj.CrossTeam_Info;
import Common.CrossTeamObj.CrossTeam_SetInfo_Join;
import CrossTeamServer.CrossTeamServer;
import NPCommon.DB.BM.BM;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import RPC._ARpcCallBack;

import java.util.ArrayList;

/**
 * 队伍数据
 */
public class CrossTeamInfo
{
    //分组数据
    private CrossGroup _m_group;

    //队伍数据
    private CrossTeamBO _m_bo;

    //队伍加入方式
    private ENPCrossTeamJoinType _m_joinType;

    //队伍申请条件
    private CrossTeam_SetInfo_Join _m_joinCond;

    //队伍玩家数据管理器
    private CrossTeamMemberMgr _m_memberMgr;

    //队伍聊天房间
    private CrossTeamChatRoomInfo _m_chatRoom;

    //队伍申请数据管理器
    private CrossTeamApplyMgr _m_applyMgr;

    public CrossTeamInfo(CrossGroup _group, CrossTeamBO _bo)
    {
        _m_group = _group;
        _m_bo = _bo;

        // 从BO独立字段还原加入方式
        ENPCrossTeamJoinType joinType = ENPCrossTeamJoinType.ENPCrossTeamJoinType_FromInt(_bo.getJoinType());
        _m_joinType = (joinType != null) ? joinType : ENPCrossTeamJoinType.NONE;

        // 从BO独立字段还原申请条件
        _m_joinCond = new CrossTeam_SetInfo_Join();
        ENPCrossTeamJoinCond condType = ENPCrossTeamJoinCond.ENPCrossTeamJoinCond_FromInt(_bo.getJoinCondType());
        _m_joinCond.setCond((condType != null) ? condType : ENPCrossTeamJoinCond.NONE);
        _m_joinCond.setValue(_bo.getJoinCondValue());

        _m_memberMgr = new CrossTeamMemberMgr(this);
        _m_chatRoom = new CrossTeamChatRoomInfo(getTeamId());
        _m_applyMgr = new CrossTeamApplyMgr(this);
    }

    public BM getBM() {return CrossTeamServer.getInstance().getBM();}

    public CrossGroup getGroup() {return _m_group;}
    public long getGroupId() {return _m_group.getGroupId();}

    public CrossTeamBO getBo() {return _m_bo;}
    public long getTeamId() {return _m_bo.getTeamId();}
    public int getMemberLimit() {return _m_bo.getMemberLimit();}
    public String getTeamName() {return _m_bo.getTeamName();}
    public String getTeamDec() {return _m_bo.getTeamDec();}

    // 获取队伍加入方式
    public ENPCrossTeamJoinType getJoinType() {return _m_joinType;}

    // 获取队伍申请条件
    public CrossTeam_SetInfo_Join getJoinCond() {return _m_joinCond;}

    /**
     * 仅更新申请条件，写入BO独立字段（供 CTSSetTeamApplyCond 使用）
     */
    public void updateJoinCond(CrossTeam_SetInfo_Join _joinCond)
    {
        _m_joinCond = _joinCond;
        _m_bo.saveJoinCondType(getBM(), _joinCond.getCond().ordinal());
        _m_bo.saveJoinCondValue(getBM(), _joinCond.getValue());
    }

    public CrossTeamMemberMgr getMemberMgr() {return _m_memberMgr;}

    public CrossTeamChatRoomInfo getChatRoom() {return _m_chatRoom;}

    public CrossTeamApplyMgr getApplyMgr() {return _m_applyMgr;}

    /**
     * 启动队伍聊天房间
     */
    protected void _startRegChatRoom()
    {
        //注册队伍聊天房间到聊天服务器
        CrossTeamServer.getInstance().getChatRoomMgr().regRoom(_m_chatRoom);
    }

    /**
     * 队伍数据初始化完成时调用
     */
    protected void _onInited()
    {
        _m_applyMgr._onInited();
    }

    /**
     * 构造队伍基础信息协议
     * @return
     */
    public CrossTeam_BaseInfo toBaseInfo()
    {
        CrossTeam_BaseInfo baseInfo = new CrossTeam_BaseInfo();
        baseInfo.setTeamId(getTeamId());
        baseInfo.setMemberLimit(getMemberLimit());
        baseInfo.setTeamName(getTeamName());
        baseInfo.setTeamDec(getTeamDec());

        return baseInfo;
    }

    /**
     * 按需构造协议对象（用于推送给客户端），字段直接展开到 CrossTeam_Info
     */
    public CrossTeam_Info toProto()
    {
        CrossTeam_Info info = new CrossTeam_Info();
        info.setTeamBase(toBaseInfo());
        info.setJoinType(_m_joinType);
        info.setJoinCond(_m_joinCond);
        _m_memberMgr.makeProto(info.getMemberList());
        return info;
    }

    /**
     * 修改队伍设置：名称、宣言、加入方式、申请条件，同步写入BO
     * @param _teamName 新队伍名称
     * @param _teamDec  新队伍宣言
     * @param _joinType 新加入方式
     * @param _joinCond 新申请条件
     */
    public void setTeamSetting(String _teamName, String _teamDec,
                               ENPCrossTeamJoinType _joinType, CrossTeam_SetInfo_Join _joinCond)
    {
        _m_joinType = _joinType;
        _m_joinCond = _joinCond;
        _m_bo.saveTeamName(getBM(), _teamName);
        _m_bo.saveTeamDec(getBM(), _teamDec);
        _m_bo.saveJoinType(getBM(), _joinType.ordinal());
        _m_bo.saveJoinCondType(getBM(), _joinCond.getCond().ordinal());
        _m_bo.saveJoinCondValue(getBM(), _joinCond.getValue());
    }

    /**
     * 检查申请条件
     * @param _joinCondList
     * @return
     */
    public boolean checkJoinCond(ArrayList<CrossTeam_SetInfo_Join> _joinCondList)
    {
        // 无申请条件限制
        if (null == _m_joinCond.getCond()
                || _m_joinCond.getCond() == ENPCrossTeamJoinCond.NONE)
            return true;

        long applyCondValue = 0;
        for (int i = 0; i < _joinCondList.size(); i++)
        {
            CrossTeam_SetInfo_Join applyCond = _joinCondList.get(i);
            if (applyCond.getCond() == _m_joinCond.getCond())
            {
                applyCondValue = applyCond.getValue();
                break;
            }
        }

        return _m_joinCond.getValue() <= applyCondValue;
    }

    /**
     * 解散队伍数据
     */
    protected void _onDissolve()
    {
        //销毁队伍数据
        _m_bo.del(getBM());

        //销毁队伍成员数据
        getMemberMgr()._onDissolve();

        //销毁队伍申请数据
        getApplyMgr()._onDissolve();

        //聊天房间销毁
        ALSynTaskManager.getInstance().regTask(()->
        {
            CrossTeamServer.getInstance().getChatRoomMgr().unRegRoom(getChatRoom());
        });
    }

    /**
     * 数据销毁时调用
     */
    protected void _onDiscard()
    {
        //销毁队伍数据
        _m_bo.del(getBM());

        //成员数据销毁
        getBM().getBM(CrossTeamMemberBO.class).delAll("team_id", getTeamId());

        //申请数据销毁
        getBM().getBM(CrossTeamApplyBO.class).delAll("team_id", getTeamId());

        //聊天房间销毁
        ALSynTaskManager.getInstance().regTask(()->
        {
            CrossTeamServer.getInstance().getChatRoomMgr().unRegRoom(getChatRoom());
        });
    }

    /**
     * 通知队长所在 US 同步 Group 成员数据，成员人数变动时调用
     */
    public void notifyGroupSync()
    {
        long leaderCid = _m_memberMgr.getLeaderCid();
        if (leaderCid <= 0)
        {
            CommLog.error("CrossTeamInfo.notifyGroupSync leaderCid invalid: groupId={}, teamId={}", getGroupId(), getTeamId());
            return;
        }

        int leaderUsId = CommonFunc.parseServerTypeIdFromCid(leaderCid);

        UsNotifySyncGroupTeam rpc = new UsNotifySyncGroupTeam();
        rpc.req().setGroupId(getGroupId());
        rpc.req().setTeamId(getTeamId());

        CrossTeamServer.getInstance().rpc2us().requestTo(leaderUsId, rpc, new _ARpcCallBack<UsNotifySyncGroupTeam>()
        {
            @Override
            public void call_back(int _errCode, UsNotifySyncGroupTeam _rpc)
            {
                if (_errCode > 0)
                    CommLog.error("CrossTeamInfo.notifyGroupSync failed: groupId={}, teamId={}, errCode={}",
                            getGroupId(), getTeamId(), _errCode);
            }
        });
    }

    /**
     * 通知队长所在 US 移除 Group，队伍解散时调用（在 dissolve 之前调用，确保成员数据仍可访问）
     */
    public void notifyGroupRemove()
    {
        long leaderCid = _m_memberMgr.getLeaderCid();
        if (leaderCid <= 0)
        {
            CommLog.error("CrossTeamInfo.notifyGroupRemove leaderCid invalid: groupId={}, teamId={}", getGroupId(), getTeamId());
            return;
        }

        int leaderUsId = CommonFunc.parseServerTypeIdFromCid(leaderCid);

        UsNotifyRemoveGroup rpc = new UsNotifyRemoveGroup();
        rpc.req().setGroupId(getGroupId());

        CrossTeamServer.getInstance().rpc2us().requestToRepeat(leaderUsId, rpc, new _ARpcCallBack<UsNotifyRemoveGroup>()
        {
            @Override
            public void call_back(int _errCode, UsNotifyRemoveGroup _rpc)
            {
                // 成功无需处理
            }
        }, 10, () ->
                CommLog.error("CrossTeamInfo.notifyGroupRemove failed: groupId={}", getGroupId()));
    }
}
