package NPUSServer.UsMars.UsMarsAction.UsMineAction;

import AllRpcData.US_Service.Mars.MarsMineOccupyResult;
import Common.ServerObj.ServerObj_MarsTeam_OccupyMinePlayer;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Mars.UsMarsBattle._IUsMarsBattleObj;
import NPUSServer.USLog;
import NPUSServer.UsMars.UsMarsAction._AUsMarsBasicAction;
import RPC._ARpcCallBack;
import USDB.Bo.UsMarsMineOccupyBO;

import java.nio.ByteBuffer;

/**
 * 火星占领矿区的行为
 */
public class UsMarsMineAttackAction extends _AUsMarsBasicAction implements _IUsMarsBattleObj
{
    private UsMarsMineActionMgr _m_amActionMgr;

    //火星矿产占领玩家数据
    private UsMarsMineOccupyBO _m_bo;

    //火星占领玩家数据
    private ServerObj_MarsTeam_OccupyMinePlayer _m_opOccupyPlayer;

    public UsMarsMineAttackAction(UsMarsMineActionMgr _actionMgr, UsMarsMineOccupyBO _bo)
    {
        _m_amActionMgr = _actionMgr;
        _m_bo = _bo;

        _m_opOccupyPlayer = new ServerObj_MarsTeam_OccupyMinePlayer();
        if(null != _m_bo.getOccupyPlayer())
        {
            ByteBuffer buff = ByteBuffer.wrap(_m_bo.getOccupyPlayer());
            _m_opOccupyPlayer.readPackage(buff);
        }
    }

    //返回数据库Id
    public long getActionId() {return _m_bo.getId();}
    public long getMineInstanceId() {return _m_bo.getMineInstaceId();}
    public long getStartCollectMs() {return _m_bo.getStartCollectMs();}
    public long getCollectSpeed() {return _m_bo.getCollectSpeed();}
    public ServerObj_MarsTeam_OccupyMinePlayer getPlayer() {return _m_opOccupyPlayer;}
    public long getCid() {return getPlayer().getCid();}
    public long getTeamId() {return getPlayer().getTeamId();}

    /**
     * 直接遣返队伍
     */
    public void teamBack()
    {
        //队伍返回
        _sendOccupyResult(-1,-1, false);
    }

    /**
     * 战败返回
     */
    public void teamAttFail()
    {
        //队伍返回
        _sendOccupyResult(-1,-1, false);
    }

    /**
     * 队伍战胜，返回占领状态
     */
    public void teamAttSuc(long _startCollectTimeMs, long _endCollectTimeMS)
    {
        _sendOccupyResult(_startCollectTimeMs, _endCollectTimeMS, true);
    }


    /**
     * 发送占领结果
     * @param _endCollectMs
     * @param _isWin
     */
    protected void _sendOccupyResult(long _startCollectMs, long _endCollectMs, boolean _isWin)
    {
        int usId = CommonFunc.parseServerTypeIdFromCid(getCid());
        //推送发送玩家US
        MarsMineOccupyResult rpc = new MarsMineOccupyResult();
        rpc.req().getResult().setCid(getPlayer().getCid());
        rpc.req().getResult().setTeamId(getPlayer().getTeamId());
        rpc.req().getResult().setMineInstanceId(getMineInstanceId());
        rpc.req().getResult().setTeamLossValue(getPlayer().getTeamLossValue());
        rpc.req().getResult().setStartCollectMs(_startCollectMs);
        rpc.req().getResult().setEndCollectMs(_endCollectMs);
        rpc.req().getResult().setBattleTimeMs(CommonFunc.getNowTimeMS());
        rpc.req().getResult().setIsWin(_isWin);

        _m_amActionMgr.getUSServer().rpc2us().requestToRepeat(usId, rpc,
                new _ARpcCallBack<MarsMineOccupyResult>()
                {
                    @Override
                    public void call_back(int _errCode, MarsMineOccupyResult _rpc)
                    {
                    }
                },
                99,
                () ->
                {
                    USLog.error(_m_amActionMgr.getUSServer(), "player:{} teamId:{} mine:{} send rpc MarsMineOccupyResult fail.", getCid(), getTeamId(), getMineInstanceId());
                });
    }

    /************************* 战斗处理对象的防守方重载函数 ******************/
    /**
     * 获取当前可参战的战斗人员数量
     * @return
     */
    public long getTeamSoldierNum(){return _m_opOccupyPlayer.getTeamTroopNum() - _m_opOccupyPlayer.getTeamLossValue();}

    /**
     * 获取参战的单兵实力
     * @return
     */
    public long getTeamSoldierPower(){return _m_opOccupyPlayer.getTeamSoldierPower();}

    /**
     * 增加伤兵数量，注意这里是增量不是全量
     * @param _hurtNum
     */
    public void addHurtSoldierNum(long _hurtNum){
        //计算新伤兵数量
        long totalHurtNum = _m_opOccupyPlayer.getTeamLossValue() + _hurtNum;
        //超出上限则按照上限
        if(totalHurtNum > _m_opOccupyPlayer.getTeamTroopNum())
            totalHurtNum = _m_opOccupyPlayer.getTeamTroopNum();

        //设置伤兵数量，这里不存储数据库因为一旦结算就马上失效
        _m_opOccupyPlayer.setTeamLossValue(totalHurtNum);
    }

    /**
     * 记录战斗日志的接口
     * @param _isAttacker
     * @param _isAttWin
     * @param _attackPower
     * @param _attckHurtNum
     * @param _attackTroopNum
     * @param _defencePower
     * @param _defenceHurtNum
     * @param _defenceTroopNum
     */
    public void logBattle(boolean _isAttacker, boolean _isAttWin, _IUsMarsBattleObj _enemy, long _attackPower, long _attckHurtNum
            , long _attackTroopNum, long _defencePower, long _defenceHurtNum, long _defenceTroopNum)
    {
        //TODO ： 进攻方的进攻行为日志
    }
    /************************* 战斗处理对象的防守方重载函数 end ******************/

    /************************* 火星行为处理对象的重载函数 ******************/
    /**
     * 获取本行为触发的时间点，仅会在触发的时候进行处理。过程不会做tick
     */
    @Override
    public long getTriggerTimeMS()
    {
        //在这个Bo中开始采集时间就是到达时间
        return _m_bo.getStartCollectMs();
    }

    /**
     * 触发结果行为的处理，此行为仅会触发一次
     * @param _nowTimeMs
     */
    @Override
    protected void _trigger(long _nowTimeMs)
    {
        //触发行为，直接触发战斗处理
        _m_amActionMgr.getUSServer().getMarsMineCore().occupy(this);
    }

    /**
     * 处理取消操作的行为
     */
    @Override
    protected void _dealCancel()
    {
        //取消的话直接遣返队伍
        teamBack();
    }

    /**
     * 清理行为数据
     */
    @Override
    protected  void _clearData()
    {
        //从管理器中移除
        _m_amActionMgr._rmvAction(this);

        //删除数据库数据
        _m_bo.del(_m_amActionMgr.getBM());
    }
    /************************* 火星行为处理对象的重载函数 end ******************/
}
