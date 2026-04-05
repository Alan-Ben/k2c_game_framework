package NPMiniGame.GameLife;

/**
 * 小游戏生命周期状态
 */
public interface _IMiniGameLifeState
{
    /**
     * 获取当前状态
     * @return ENPMiniGameLifeState
     */
    ENPMiniGameLifeState getStateType();

    /**
     * 退出当前状态
     */
    void quitState();

    /**
     * 进入当前状态
     */
    void enterState();

    /**
     * 判断能否进入新状态
     * @param _newState 新状态
     * @return boolean
     */
    boolean judgeCanTransToThatState(_IMiniGameLifeState _newState);

}
