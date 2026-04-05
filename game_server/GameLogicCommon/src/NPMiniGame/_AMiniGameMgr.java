package NPMiniGame;

import ALBasicCommon.ALSerializeMaker;
import NPMiniGame.GameLife.ENPMiniGameLifeState;
import NPMiniGame.GameLife.NPMiniGameLifeMachine;
import NPMiniGame.GameLife._IMiniGameLifeState;

/**
 * 小游戏
 */
public abstract class _AMiniGameMgr
{
    /**
     * 小游戏生命周期状态机
     */
    private NPMiniGameLifeMachine _m_ObjGameLifeStateMachine;

    /**
     * 随机种子，双端用来同步随机过程的伪随机码,在创建时就生成好
     */
    private int _m_iRandomSeed;

    /**
     * 游戏序列号
     */
    private long _m_lGameSerial;

    public _AMiniGameMgr()
    {
        _m_ObjGameLifeStateMachine = new NPMiniGameLifeMachine();
    }

    //region get&&set
    protected NPMiniGameLifeMachine getGameLifeStateMachine()
    {
        return _m_ObjGameLifeStateMachine;
    }

    public int getRandomSeed()
    {
        return _m_iRandomSeed;
    }


    protected void _resetRandomSeed(int _randomSeed)
    {
        _m_iRandomSeed = _randomSeed;
    }

    public ENPMiniGameLifeState getStateType()
    {
        if (_m_ObjGameLifeStateMachine == null)
        {
            return ENPMiniGameLifeState.NONE;
        }
        return _m_ObjGameLifeStateMachine.getCurState().getStateType();
    }

    public _IMiniGameLifeState getState()
    {
        if (_m_ObjGameLifeStateMachine == null)
        {
            return null;
        }
        return _m_ObjGameLifeStateMachine.getCurState();
    }

    public long getSerial()
    {
        return _m_lGameSerial;
    }

    protected synchronized void _resetSerial()
    {
        _m_lGameSerial = ALSerializeMaker.makeNewSerialize();
    }
    //endregion
}
