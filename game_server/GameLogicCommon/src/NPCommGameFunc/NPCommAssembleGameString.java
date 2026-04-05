package NPCommGameFunc;

import ALBasicServer.ALProcess.ALProcess;
import ALBasicServer.ALProcess._AALProcess;
import ALBasicServer.ALProcess._IALProcessMonitor;
import ALServerLog.ALServerLog;
import Common.NpPlayerInfoObj.PlayerInfo_CommonShow;
import NPCommon.CommonCache.Player.Getter.PlayerCacheGetter;
import NPCommon.Util.CallBack._ICallBackT;
import NPCommon.Util.Delegate.HandlerTwo;
import NPEnum.ENPPlayerCacheDataEnum;

import java.util.ArrayList;

/*****
 * 用于拼装日志字符串，比如聚会系统拼装” **加入聚会“ 等
 * @author mj
 *
 */
public class NPCommAssembleGameString
{
    /**
     * 静态方法，创建新实例
     * @return
     */
    public static NPCommAssembleGameString build()
    {
        return new NPCommAssembleGameString();
    }

    ////////////////////////////// 实例数据 //////////////////////////////

    //参数列表
    private final ArrayList<String> _m_alParamList;
    //promise对象
    private final ALProcess _m_pProcess;

    public NPCommAssembleGameString()
    {
        _m_alParamList = new ArrayList<>();

        _m_pProcess = ALProcess.CreateProcess("Comm_Assemble_Game_String");
    }

    /**
     * 输出字符串列表
     * @return
     */
    public void makeStringList(_ICallBackT<ArrayList<String>> _callback)
    {
        _m_pProcess.dealProcess(new _IALProcessMonitor()
        {
            @Override
            public void onTimeoutDone(long _processTimeMS, String _processTag, String _exInfo)
            {
            }

            @Override
            public void onTimeout(long _processTimeMS, String _processTag, String _exInfo)
            {
            }

            //异常终止的事件函数
            @Override
            public void onRootProecssStop()
            {
            }

            //正常结束的事件函数
            @Override
            public void onRootProecssSuc()
            {
                _callback.onRunOver(new ArrayList<>(_m_alParamList));
            }

            @Override
            public void onProcessFailStop(_AALProcess _process)
            {
            }

            @Override
            public void onRootProecssDone()
            {
            }

            @Override
            public void onErr(long _processTimeMS, String _processTag, String _exInfo, Exception _ex)
            {
                ALServerLog.Error("Comm_Assemble_Game_String err: " + _ex.getMessage());
            }

            @Override
            public long monitorTimeMS(String _processTag)
            {
                return 0;
            }
        });
    }

    /**
     * 增加字符串
     * @param _param
     * @return
     */
    public NPCommAssembleGameString addString(final String _param)
    {
        _m_pProcess.addResDelegateProcess(_doneAction ->
        {

            _m_alParamList.add(_param);

            _doneAction.dealAction(true);

        }, "Comm_Assemble_Game_String.addParam");

        return this;
    }

    /**
     * 获取玩家相关数据
     * @param _getter
     * @param _cid
     * @param _dataEnumList
     * @return
     */
    public NPCommAssembleGameString addPlayer(PlayerCacheGetter _getter, long _cid, ArrayList<ENPPlayerCacheDataEnum> _dataEnumList)
    {
        _m_pProcess.addResDelegateProcess(_doneAction ->
        {
            _getter.getInfo(PlayerInfo_CommonShow.class, _cid, new HandlerTwo<Boolean, PlayerInfo_CommonShow>()
            {
                @Override
                public void handle(Boolean _isSuc, PlayerInfo_CommonShow _playerCache)
                {
                    if (!_isSuc)
                    {
                        //回调方法
                        _doneAction.dealAction(false);
                    } else
                    {
                        for (ENPPlayerCacheDataEnum dataEnum : _dataEnumList)
                        {
                            if (ENPPlayerCacheDataEnum.NAME == dataEnum)
                            {
                                _m_alParamList.add(_playerCache.getIconShow().getPlayerName());
                            } else //其他情况，输出日志
                            {
                                _m_alParamList.add("");
                            }
                        }

                        //回调方法
                        _doneAction.dealAction(true);
                    }
                }
            });
        }, "Comm_Assemble_Game_String.addPlayer");

        return this;
    }
}
