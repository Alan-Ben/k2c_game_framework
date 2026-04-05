package NPGameRes.GameObjs.Mars.UsMarsBattle;

/**
 * 参与火星战斗计算的对象
 * 不同对象只需要实现相关接口即可以参与战斗结算
 */
public interface _IUsMarsBattleObj {
    /**
     * 获取当前可参战的战斗人员数量
     * @return
     */
    long getTeamSoldierNum();

    /**
     * 获取参战的单兵实力
     * @return
     */
    long getTeamSoldierPower();

    /**
     * 增加伤兵数量，注意这里是增量不是全量
     * @param _hurtNum
     */
    void addHurtSoldierNum(long _hurtNum);

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
    void logBattle(boolean _isAttacker, boolean _isAttWin, _IUsMarsBattleObj _enemy
            , long _attackPower, long _attckHurtNum, long _attackTroopNum
            , long _defencePower, long _defenceHurtNum, long _defenceTroopNum);
}
