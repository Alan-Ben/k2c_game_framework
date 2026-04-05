package NPUSServer.Guild;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPGameRes.Refs.RefGeneral;
import NPGameRes.Refs.RefUnicodeLengthCheck;

import java.util.HashSet;
import java.util.Set;

public class GuildNameChecker
{
    //名字Set
    private Set<String> _m_nameSet;
    //简称Set
    private Set<String> _m_simpleNameSet;
    //名字锁
    private MutexAtom _m_mutex;

    public GuildNameChecker()
    {
        _m_nameSet = new HashSet<>();
        _m_simpleNameSet = new HashSet<>();
        _m_mutex = new MutexAtom();
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    public void initName(String _name, String _simpleName)
    {
        _m_nameSet.add(_name);
        _m_simpleNameSet.add(_simpleName);
    }

    /**
     * 记录名称
     * @param _name
     * @param _simpleName
     * @return
     */
    public Result recordName(String _name, String _simpleName)
    {
        _lock();
        try
        {
            Result checkResult = checkName(null, _name, null, _simpleName);
            if (!checkResult.isSucc())
                return checkResult;

            _m_nameSet.add(_name);
            _m_simpleNameSet.add(_simpleName);

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 修改名称
     */
    public Result chgName(String _oldName, String _newName, String _oldSimpleName, String _newSimpleName)
    {
        _lock();
        try
        {
            Result checkResult = checkName(_oldName, _newName, _oldSimpleName, _newSimpleName);
            if (!checkResult.isSucc())
                return checkResult;

            _m_nameSet.remove(_oldName);
            _m_nameSet.add(_newName);

            _m_simpleNameSet.remove(_oldSimpleName);
            _m_simpleNameSet.add(_newSimpleName);

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查名称和简称是否合法
     * @param _name
     * @param _simpleName
     */
    public Result checkName(String _oldName, String _name, String _oldSimpleName, String _simpleName)
    {
        //检查合法性
        if (!checkLength(_name, _simpleName))
            return GuildErr.STRING_LENGTH_OVER_LIMIT;

        //检查名称和简称是否重复
        Result checkNameResult = checkNameRepeat(_oldName, _name, _oldSimpleName, _simpleName);
        if (!checkNameResult.isSucc())
            return checkNameResult;

        return Result.SUCC;
    }

    /**
     * 检查长度
     * @param _name
     * @param _simpleName
     * @return
     */
    private boolean checkLength(String _name, String _simpleName)
    {
        if (_name == null || _name.isEmpty() || _simpleName == null || _simpleName.isEmpty())
            return false;

        //检查名字长度
        if (!RefGeneral.Ref().guild_name_length_limit.inRange(RefUnicodeLengthCheck.getMgr().getCharLength(_name)))
            return false;

        //检查简称长度
        if (!RefGeneral.Ref().guild_simple_name_length_limit.inRange(RefUnicodeLengthCheck.getMgr().getCharLength(_simpleName)))
            return false;

        return true;
    }

    /**
     * 检查名称和简称是否重复
     * @param _name
     * @param _simpleName
     */
    public Result checkNameRepeat(String _oldName, String _name, String _oldSimpleName, String _simpleName)
    {
        _lock();
        try
        {
            if ((_oldName == null || !_oldName.equals(_name)) && _m_nameSet.contains(_name))
                return GuildErr.GUILD_NAME_EXIST;

            if ((_oldSimpleName == null || !_oldSimpleName.equals(_simpleName)) && _m_simpleNameSet.contains(_simpleName))
                return GuildErr.GUILD_SIMPLE_NAME_EXIST;

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 移除记录
     * @param _name
     * @param _simpleName
     */
    public void removeRecord(String _name, String _simpleName)
    {
        _lock();
        try
        {
            _m_nameSet.remove(_name);
            _m_simpleNameSet.remove(_simpleName);
        } finally
        {
            _unlock();
        }
    }
}
