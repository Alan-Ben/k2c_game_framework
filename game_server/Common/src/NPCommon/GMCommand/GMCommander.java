package NPCommon.GMCommand;

import NPCommon.Context._IContext;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.Util.CallBack._IRunCallBack;

import java.util.*;
import java.util.concurrent.ConcurrentHashMap;

/*****
 * 描述单个命令，如Player,管理其下的执行方法列表
 */
public class GMCommander
{
    private String _m_sName; //命令名，如player
    private String _m_sComment;//命令说明
    private Map<String, GMCommand> _m_commandMap = new ConcurrentHashMap<>(); //子操作列表，一个子命令对应于一个函数

    public GMCommander(ACommander annotation)
    {
        _m_sName = annotation.name();
        _m_sComment = annotation.comment();
    }

    /***
     * 返回命令名称
     * @return
     */
    public String getName()
    {
        return _m_sName;
    }

    /*****
     * 异步执行一个GM命令操作
     * @param _executor
     * @param _handler
     * @param _context
     * @param _cmdName
     * @param args
     * @throws Exception
     */
    public void run(CmdExecutorBase _executor, _IRunCallBack _handler, _IContext _context, String _cmdName, String[] args) throws Exception
    {
        GMCommand gMCommand = _m_commandMap.get(_cmdName);
        if (gMCommand == null || _cmdName.isEmpty())
        {

            String result = "[ERROR]command[" + _cmdName + "] not found in commander[" + this._m_sName + "]";
            _handler.onRunOver(false, result);
            return;
        }
        gMCommand.run(_executor, _handler, _context, args);
    }

    /******
     * 返回所有方法列表
     * @return
     */
    public String getHelp()
    {
        StringBuilder sBuilder = new StringBuilder();
        List<GMCommand> list = new ArrayList<>(_m_commandMap.values());
        Collections.sort(list, Comparator.comparing(GMCommand::getName));
        for (GMCommand gMCommand : list)
        {
            sBuilder.append("\n\t").append(gMCommand.toString());
        }
        return sBuilder.toString();
    }

    public String toString()
    {
        return _m_sName + " : " + _m_sComment;
    }

    /****
     * 增加一个GM操作
     * @param _command
     */
    public void addCommand(GMCommand _command)
    {
        _m_commandMap.put(_command.getName(), _command);
    }
}