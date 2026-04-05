package NPMiniGame.MiniGameProcess;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import NPCommon.Util.CallBack._ICallBackBool;
import NPMiniGame._IMiniGameContext;

/**
 * @description: 小游戏操作处理结果提交类
 * @author: ricci
 * @date: 2022-08-03 16:04:25
 */
public abstract class _ANPMiniGameOptionCommiter implements _IALProtocolReceiver
{

    /**
     * 处理上下文
     */
    private _IMiniGameContext _m_gameContext;

    /**
     * 处理完成回调
     */
    private _ICallBackBool _m_callBack;

    public _ANPMiniGameOptionCommiter(_IMiniGameContext _gameContext, _ICallBackBool _dealAction)
    {
        _m_gameContext = _gameContext;
        _m_callBack = _dealAction;
    }

    public _IMiniGameContext getGameContext()
    {
        return _m_gameContext;
    }

    public void commit(boolean _isSuc)
    {
        _m_callBack.onRunOver(_isSuc);
    }
}
