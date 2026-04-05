package GameLogicServer.GroupMgr.ActivityMgr.FirstTeamActivity;

import GLSDB.Bo.FirstTeamActivityPlayerBO;
import GLSDB.Bo.FirstTeamActivityTeamBO;
import GLSDB.Bo.FirstTeamActivityTeamMemberBO;
import GameLogicServer.GameLogicServer;
import GameLogicServer.GroupMgr.ActivityMgr._ATActivityInfo;
import GameLogicServer.GroupMgr.PlayerGroupMgr._ATGroupMgr;
import GameLogicServer.GroupMgr.PlayerGroupMgr._ATPlayerGroupPlayerMgr;
import NPCommon.ErrMain.CommErr;
import NPCommon.Log.CommLog;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.nio.ByteBuffer;
import java.util.HashMap;
import java.util.List;

/**
 * 首组队活动主体
 *
 * 主要功能：
 * 1. 提供活动级别的玩家与队伍管理器创建
 * 2. 承接活动消息分发入口
 */
public class FirstTeamActivity extends _ATActivityInfo<FirstTeamActivityPlayerInfo, FirstTeamActivityTeamInfo>
{
    /**
     * 构造活动实例
     * @param _groupId 分组ID
     * @param _activityId 活动ID
     */
    public FirstTeamActivity(long _groupId, long _activityId)
    {
        super(_groupId, _activityId);
    }

    /**
     * 从预加载的全量BO数据中完成本活动实例的内存数据初始化
     * @param _playerBoList 全量玩家BO列表
     * @param _teamBoList   全量队伍BO列表
     * @param _memberBoMap  成员BO数据（key=teamId，已预分组）
     */
    public void sInit(List<FirstTeamActivityPlayerBO> _playerBoList,
                      List<FirstTeamActivityTeamBO> _teamBoList,
                      HashMap<Long, List<FirstTeamActivityTeamMemberBO>> _memberBoMap)
    {
        getPlayerMgr().sInit(_playerBoList);
        getGroupMgr().sInit(_teamBoList, _memberBoMap);
    }

    /**
     * 初始化活动对象
     * @param _addInfo 附加初始化数据
     * @return true 表示初始化成功
     */
    @Override
    public boolean init(ByteBuffer _addInfo)
    {
        return true;
    }

    /**
     * 分发活动消息到具体业务逻辑
     * @param _committer 回包提交器
     * @param _player 玩家数据
     * @param _group 队伍数据
     * @param _msg 请求消息体
     * @param _addInfo 附加消息体
     */
    @Override
    protected void _dispatchMsg(_IWCGBasicRequestCommiter _committer, FirstTeamActivityPlayerInfo _player, FirstTeamActivityTeamInfo _group, byte[] _msg, byte[] _addInfo)
    {
        // 当前功能尚未接入具体协议分发，统一返回系统错误
        CommLog.error("FirstTeamActivity._dispatchMsg - message dispatch not implemented, groupId={}, activityId={}", getGroupId(), getActivityId());
        _committer.commitFailRes(CommErr.SYS_ERR.getCode());
    }

    /**
     * 创建活动玩家管理器
     * @return 玩家管理器
     */
    @Override
    protected _ATPlayerGroupPlayerMgr<FirstTeamActivityPlayerInfo> _createPlayerGroupPlayerMgr()
    {
        return new FirstTeamActivityPlayerMgr(this);
    }

    /**
     * 创建活动队伍管理器
     * @return 队伍管理器
     */
    @Override
    protected _ATGroupMgr<FirstTeamActivityPlayerInfo, FirstTeamActivityTeamInfo> _createPlayerGroupMgr()
    {
        return new FirstTeamActivityTeamMgr(this);
    }

    /**
     * 销毁活动对象：删除本活动 groupId 下的所有数据库记录
     */
    @Override
    public void _discard()
    {
        CommLog.info("FirstTeamActivity discard, groupId:{} activityId:{}", getGroupId(), getActivityId());

        long groupId = getGroupId();

        // 删除玩家数据
        GameLogicServer.getInstance().getBM().getBM(FirstTeamActivityPlayerBO.class).delAll("group_id", groupId);
        // 删除队伍成员数据
        GameLogicServer.getInstance().getBM().getBM(FirstTeamActivityTeamMemberBO.class).delAll("group_id", groupId);
        // 删除队伍数据
        GameLogicServer.getInstance().getBM().getBM(FirstTeamActivityTeamBO.class).delAll("group_id", groupId);
    }

    /**
     * 获取类型化玩家管理器
     * @return 玩家管理器
     */
    @Override
    public FirstTeamActivityPlayerMgr getPlayerMgr()
    {
        return (FirstTeamActivityPlayerMgr) super.getPlayerMgr();
    }

    /**
     * 获取类型化队伍管理器
     * @return 队伍管理器
     */
    @Override
    public FirstTeamActivityTeamMgr getGroupMgr()
    {
        return (FirstTeamActivityTeamMgr) super.getGroupMgr();
    }
}
