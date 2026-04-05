package NPCommon.CommonNameEqualMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.PlayerErr;
import NPCommon.ErrMain.Result.Result;

import java.util.HashSet;

/**
 * @description: 使用过的名字的记录和是否重名管理器
 * @author: ricci
 * @date: 2023-03-03 10:17:54
 */
public abstract class _ACommNameEqualMgr
{
    /**
     * 已占用名称表
     */
    private HashSet<String> _m_commNameSet = new HashSet<>();

    /**
     * 管理器锁
     */
    private MutexAtom _m_lock = new MutexAtom();

    public void _lock()
    {
        _m_lock.lock();
    }

    public void _unlock()
    {
        _m_lock.unlock();
    }

    /**
     * 初始化恢复数据
     */
    public void initAddName(String _name)
    {
        _lock();
        try
        {
            _m_commNameSet.add(_name.toLowerCase());
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查名称合法或存在
     * @param _name
     * @return
     */
    public Result checkName(String _name)
    {
        _lock();
        try
        {
            if (null == _name || _name.isEmpty())
                return CommErr.PARAM_ERROR;

            //检查名称合法性
            Result legalCheckResult = _checkNameLegal(_name);
            if (!legalCheckResult.isSucc())
                return legalCheckResult;

            //检查是否存在
            if (isNameExist(_name))
                return PlayerErr.PLAYER_NAME_EQUAL;

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 该名称是否存在
     * @param _name 名字
     * @return boolean
     */
    public boolean isNameExist(String _name)
    {
        _lock();
        try
        {
            return _m_commNameSet.contains(_name.toLowerCase());
        } finally
        {
            _unlock();
        }
    }

    /**
     * 更新名字
     * @param _oldName 要放弃占用的旧名字
     * @param _newName 将要占用的新名字
     * @return boolean
     */
    public Result updateName(String _oldName, String _newName)
    {
        _lock();
        try{
            String lowerCaseOldName = _oldName.toLowerCase();
            String lowerCaseNewName = _newName.toLowerCase();

            //检查新名称合法性、是否存在命名
            Result checkResult = checkName(lowerCaseNewName);
            if (!checkResult.isSucc())
                return checkResult;

            //移除旧占用
            _m_commNameSet.remove(lowerCaseOldName);
            __onRemoveNameRecord(lowerCaseOldName);

            //增加新占用
            _m_commNameSet.add(lowerCaseNewName);
            __onAddNameRecord(lowerCaseNewName);

            return Result.SUCC;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 子类需要检查名称
     * @param _name 待检查命名
     * @return 是否通过检查
     */
    protected abstract Result _checkNameLegal(String _name);

    /**
     * 增加名称占用记录，是否占用成功
     * @param _name 名称
     */
    public abstract void __onAddNameRecord(String _name);

    /**
     * 移除名称占用记录
     * @param _name 名称
     */
    public abstract void __onRemoveNameRecord(String _name);
}
