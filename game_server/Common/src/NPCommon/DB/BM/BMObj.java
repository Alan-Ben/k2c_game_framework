package NPCommon.DB.BM;

import ALBasicServer.ALServerAsynTask.ALAsynTaskManager;
import ALMySqlCommon.ALMySqlDBConditionObj;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlDBExcutor;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import ALMySqlCommon.ALMySqlSafeOp._TALMySqlSafeOpDBObj;
import NPCommon.DB.ErrDealer.DBErrException;
import NPCommon.DB.ErrDealer.SelectExceptionDealer;
import NPCommon.DB.ErrDealer.UpdateExceptionDealer;
import NPCommon.DB.*;
import NPCommon.DB.Version.TbIDMgr.TBIdInfo;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.Log.CommLog;

import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.util.HashMap;
import java.util.List;

public class BMObj<T extends IBaseBO>
{
    //服务器创建的时候会带入本对象用于对不同的DB进行操作
    private NPCommonEnum.EDBTag _m_eDBTag;

    //表名
    private String _m_sTableName;
    //一个Bo的实例接口对象，可以通过此对象获取如归属数据库等的信息
    private T _m_boInstance;

    //是否已经检查了数据库中最大Id
    private boolean _m_bIsMaxIdChecked;
    //当前的Id管理器数据对象
    private TBIdInfo _m_tbIdInfo;

    public BMObj(WCGDBObj _dbObj, Class<T> classType)
    {
        try
        {
            _m_boInstance = classType.newInstance();
        } catch (InstantiationException | IllegalAccessException e)
        {
        }
        _m_sTableName = _m_boInstance.getTbName();

        //获取真实的DBTag
        _m_eDBTag = _dbObj.getDbTag();

        //设置数据库Id还未检查
        _m_bIsMaxIdChecked = false;
        //获取Id数据对象
        _m_tbIdInfo = _dbObj.ensureTbIdInfo(_m_sTableName);
    }

    /*********
     * 获取对应的数据对象
     * @return
     */
    public _TALMySqlSafeOpDBObj<WCGDBObj> getDbObj()
    {
        return WCGDBFactory.getDbObj(_m_eDBTag);
    }

    //// ========== ID操作部分
    public synchronized long getNextId()
    {
        //尝试获取数据对象
        _s_checkDBId();

        return _m_tbIdInfo.popNewId();
    }


    /********
     * 对外开放的检测函数
     */
    public boolean s_checkDBId()
    {
        return _s_checkDBId();
    }

    /*********
     * 尝试初始化相关数据标记
     * 第一次需要访问数据库，会需要在服务器开启的时候就进行一次遍历执行
     */
    private synchronized boolean _s_checkDBId()
    {
//        ALServerLog.Sys("check DB " + _m_sTableName + " Id Info");
        //如果已检查Id则直接返回
        if(_m_bIsMaxIdChecked)
            return true;

        //后面查询数据库，并在数据库得出结果后创建
        PreparedStatement pstm = null;
        ResultSet rs = null;
        java.sql.Connection conn = null;
        try
        {
            conn = getDbObj().getDB().getConn();
            if (null == conn)
            {
                return false;
            }
            String sql = String.format("SELECT MAX(id) FROM %s", this._m_sTableName);
            pstm = conn.prepareStatement(sql);
            if (null == pstm)
            {
                return false;
            }
            rs = pstm.executeQuery();
            if (null == rs)
            {
                return false;
            }

            long maxId = 0;
            if (rs.next())
            {
                maxId = rs.getLong(1);
            }

            //设置检查标记
            _m_bIsMaxIdChecked = true;
            //尝试修正最大Id
            _m_tbIdInfo.trySyncMaxId(maxId);

            return true;
        } catch (Throwable e)
        {
            CommLog.error(e.getMessage(), e);
            return false;
        } finally
        {
            SQLUtil.close(rs, pstm, conn);
        }
    }
    //// ========== ID操作部分結束
    public void delAll()
    {
        ALAsynTaskManager.getInstance().regTask(getDbObj().getThreadIdx(), () ->
        {
            ALMySqlDBConditionObj cond = new ALMySqlDBConditionObj();
            cond.setTablesName(_m_sTableName);
            getDbObj().delByCondition(cond);
        });
    }

    public void delAll(String key, Object value)
    {
        ALAsynTaskManager.getInstance().regTask(getDbObj().getThreadIdx(), () ->
        {
            ALMySqlDBConditionObj cond = new ALMySqlDBConditionObj();
            cond.addAndEquals(String.format("`%s`", key.replace("'", "''").replace("\\", "\\\\")), value);
            cond.setTablesName(_m_sTableName);
            getDbObj().delByCondition(cond);
        });
    }

    public void s_delAll(HashMap<String, Object> conditions)
    {
        ALMySqlDBConditionObj cond = new ALMySqlDBConditionObj();
        cond.setTablesName(_m_sTableName);
        for (String key : conditions.keySet())
            cond.addAndEquals(String.format("`%s`", key.replace("'", "''").replace("\\", "\\\\")), conditions.get(key));
        getDbObj().delByCondition(cond);
    }

    public void delAll(HashMap<String, Object> conditions)
    {
        ALAsynTaskManager.getInstance().regTask(getDbObj().getThreadIdx(), () -> s_delAll(conditions));
    }

    /**
     * 删除指定列表的记录
     * @param key  字段名
     * @param list 列表
     */
    public void delAllInList(String key, List<?> list)
    {
        if (list == null || list.isEmpty())
            return;

        ALAsynTaskManager.getInstance().regTask(getDbObj().getThreadIdx(), () ->
        {
            ALMySqlDBConditionObj cond = new ALMySqlDBConditionObj();
            cond.addAndInList(String.format("`%s`", key.replace("'", "''").replace("\\", "\\\\")), list);
            cond.setTablesName(_m_sTableName);
            getDbObj().delByCondition(cond);
        });
    }

    @SuppressWarnings("unchecked")
    public List<T> s_findAll()
    {
        SelectExceptionDealer errDealer = new SelectExceptionDealer();
        ALMySqlDBConditionObj cond = new ALMySqlDBConditionObj();
        List<T> retList = ALMySqlDBExcutor.getListByCondition(getDbObj().getDB(), cond, _m_boInstance, errDealer);
        if (errDealer.isHasException())
        {
            return null;
        }
        return retList;

    }

    @SuppressWarnings("unchecked")
    public List<T> s_findAll(String key, Object value)
    {
        ALMySqlDBConditionObj cond = new ALMySqlDBConditionObj();
        cond.addAndEquals(String.format("`%s`", key.replace("'", "''").replace("\\", "\\\\")), value);
        SelectExceptionDealer errDealer = new SelectExceptionDealer();
        List<T> retList = ALMySqlDBExcutor.getListByCondition(getDbObj().getDB(), cond, _m_boInstance, errDealer);
        if (errDealer.isHasException())
        {
            return null;
        }
        return retList;
    }


    @SuppressWarnings("unchecked")
    public List<T> s_findAll(HashMap<String, Object> conditions)
    {
        ALMySqlDBConditionObj cond = new ALMySqlDBConditionObj();
        if (null != conditions)
        {
            for (String key : conditions.keySet())
                cond.addAndEquals(String.format("`%s`", key.replace("'", "''").replace("\\", "\\\\")), conditions.get(key));
        }
        SelectExceptionDealer errDealer = new SelectExceptionDealer();
        List<T> retList = ALMySqlDBExcutor.getListByCondition(getDbObj().getDB(), cond, _m_boInstance, errDealer);
        if (errDealer.isHasException())
            return null;
        return retList;
    }

    @SuppressWarnings("unchecked")
    public List<T> s_findAll(ALMySqlDBConditionObj conditon)
    {
        SelectExceptionDealer errDealer = new SelectExceptionDealer();
        List<T> retList = ALMySqlDBExcutor.getListByCondition(getDbObj().getDB(), conditon, _m_boInstance, errDealer);
        if (errDealer.isHasException())
        {
            return null;
        }
        return retList;
    }

    @SuppressWarnings("unchecked")
    public List<T> s_findAllBySort(String sortKey, boolean asc)
    {
        ALMySqlDBConditionObj cond = new ALMySqlDBConditionObj();
        cond.setTablesName(_m_sTableName);
        cond.setOrderItemsName(sortKey);
        cond.setOrderType(asc ? "ASC" : "DESC");
        SelectExceptionDealer errDealer = new SelectExceptionDealer();
        List<T> retList = ALMySqlDBExcutor.getListByCondition(getDbObj().getDB(), cond, _m_boInstance, errDealer);
        if (errDealer.isHasException())
        {
            return null;
        }
        return retList;
    }

    public List<T> s_findAll(int _limitCount)
    {
        return s_findAll(0, _limitCount);
    }

    @SuppressWarnings("unchecked")
    public List<T> s_findAll(int _limitStartIdx, int _limitCount)
    {
        SelectExceptionDealer errDealer = new SelectExceptionDealer();
        ALMySqlDBConditionObj cond = new ALMySqlDBConditionObj();
        List<T> retList = ALMySqlDBExcutor.getListByCondition(getDbObj().getDB(), cond, _m_boInstance, _limitStartIdx, _limitCount, errDealer);
        if (errDealer.isHasException())
        {
            return null;
        }
        return retList;
    }

    public List<T> s_findAll(String key, Object value, int _limitCount)
    {
        return s_findAll(key, value, 0, _limitCount);
    }

    @SuppressWarnings("unchecked")
    public List<T> s_findAll(String key, Object value, int _limitStartIdx, int _limitCount)
    {
        ALMySqlDBConditionObj cond = new ALMySqlDBConditionObj();
        cond.addAndEquals(String.format("`%s`", key.replace("'", "''").replace("\\", "\\\\")), value);
        SelectExceptionDealer errDealer = new SelectExceptionDealer();
        List<T> retList = ALMySqlDBExcutor.getListByCondition(getDbObj().getDB(), cond, _m_boInstance, _limitStartIdx, _limitCount, errDealer);
        if (errDealer.isHasException())
        {
            return null;
        }
        return retList;
    }


    public List<T> s_findAll(HashMap<String, Object> conditions, int _limitCount)
    {
        return s_findAll(conditions, 0, _limitCount);
    }

    @SuppressWarnings("unchecked")
    public List<T> s_findAll(HashMap<String, Object> conditions, int _limitStartIdx, int _limitCount)
    {
        ALMySqlDBConditionObj cond = new ALMySqlDBConditionObj();
        if (null != conditions)
        {
            for (String key : conditions.keySet())
                cond.addAndEquals(String.format("`%s`", key.replace("'", "''").replace("\\", "\\\\")), conditions.get(key));
        }
        SelectExceptionDealer errDealer = new SelectExceptionDealer();
        List<T> retList = ALMySqlDBExcutor.getListByCondition(getDbObj().getDB(), cond, _m_boInstance, _limitStartIdx, _limitCount, errDealer);
        if (errDealer.isHasException())
            return null;
        return retList;
    }

    public List<T> s_findAll(ALMySqlDBConditionObj conditon, int _limitCount)
    {
        return s_findAll(conditon, 0, _limitCount);
    }

    @SuppressWarnings("unchecked")
    public List<T> s_findAll(ALMySqlDBConditionObj conditon, int _limitStartIdx, int _limitCount)
    {
        SelectExceptionDealer errDealer = new SelectExceptionDealer();
        List<T> retList = ALMySqlDBExcutor.getListByCondition(getDbObj().getDB(), conditon, _m_boInstance, _limitStartIdx, _limitCount, errDealer);
        if (errDealer.isHasException())
        {
            return null;
        }
        return retList;
    }

    public List<T> s_findAllBySort(String sortKey, boolean asc, int _limitCount)
    {
        return s_findAllBySort(sortKey, asc, 0, _limitCount);
    }

    @SuppressWarnings("unchecked")
    public List<T> s_findAllBySort(String sortKey, boolean asc, int _limitStartIdx, int _limitCount)
    {
        ALMySqlDBConditionObj cond = new ALMySqlDBConditionObj();
        cond.setTablesName(_m_sTableName);
        cond.setOrderItemsName(sortKey);
        cond.setOrderType(asc ? "ASC" : "DESC");
        SelectExceptionDealer errDealer = new SelectExceptionDealer();
        List<T> retList = ALMySqlDBExcutor.getListByCondition(getDbObj().getDB(), cond, _m_boInstance, _limitStartIdx, _limitCount, errDealer);
        if (errDealer.isHasException())
        {
            return null;
        }
        return retList;
    }

    public Long s_Count()
    {
        ALMySqlDBConditionObj cond = new ALMySqlDBConditionObj();
        cond.setTablesName(_m_sTableName);
        SelectExceptionDealer errDealer = new SelectExceptionDealer();
        long count = ALMySqlDBExcutor.getCountByCondition(getDbObj().getDB(), cond, _m_boInstance, errDealer);
        if (errDealer.isHasException())
        {
            return null;
        }
        return count;
    }

    public Long s_Count(String key, Object value)
    {
        ALMySqlDBConditionObj cond = new ALMySqlDBConditionObj();
        cond.addAndEquals(String.format("`%s`", key.replace("'", "''").replace("\\", "\\\\")), value);
        cond.setTablesName(_m_sTableName);
        SelectExceptionDealer _errDealer = new SelectExceptionDealer();
        long count = ALMySqlDBExcutor.getCountByCondition(getDbObj().getDB(), cond, _m_boInstance, _errDealer);
        if (_errDealer.isHasException())
        {
            return null;
        }
        return count;
    }
    public Long s_Count(ALMySqlDBConditionObj _cond)
    {
        _cond.setTablesName(_m_sTableName);
        SelectExceptionDealer errDealer = new SelectExceptionDealer();
        long count = ALMySqlDBExcutor.getCountByCondition(getDbObj().getDB(), _cond, _m_boInstance, errDealer);
        if (errDealer.isHasException())
        {
            return null;
        }
        return count;
    }
    public T s_findOne() throws DBErrException
    {
        return s_findOne(null);
    }

    public T s_findOne(HashMap<String, Object> conditions) throws DBErrException
    {
        List<T> bos = s_findAll(conditions);
        if (bos == null)
        {
            throw new DBErrException();
        }
        return bos.isEmpty() ? null : bos.get(0);
    }

    public T s_findOne(String key, Object value) throws DBErrException
    {
        List<T> bos = s_findAll(key, value);
        if (bos == null)
        {
            throw new DBErrException();
        }
        return bos.isEmpty() ? null : bos.get(0);
    }

    public void count(_ASelectCallback<Long> _callBackObj)
    {
        ALAsynTaskManager.getInstance().regTask(getDbObj().getThreadIdx(), () ->
        {
            Long count = s_Count();
            if (count == null)
            {
                _callBackObj.markErr();
                return null;
            }
            return count;
        }, _callBackObj);
    }

    public void count(final String key, final Object value, _ASelectCallback<Long> _callBackObj)
    {
        ALAsynTaskManager.getInstance().regTask(getDbObj().getThreadIdx(), () ->
        {
            Long count = s_Count(key, value);
            if (null == count)
            {
                _callBackObj.markErr();
                return null;
            }
            return count;
        }, _callBackObj);
    }
    public void count(ALMySqlDBConditionObj conditions, _ASelectCallback<Long> _callBackObj)
    {
        ALAsynTaskManager.getInstance().regTask(getDbObj().getThreadIdx(), () -> {
            Long count = s_Count(conditions);
            if(null ==count)
            {
                _callBackObj.markErr();
                return  null;
            }
            return count;
        }, _callBackObj);
    }
    public void findOne(final String key, final Object value, _ASelectCallback<T> _callBackObj)
    {
        ALAsynTaskManager.getInstance().regTask(getDbObj().getThreadIdx(), () ->
        {
            T obj = null;
            try
            {
                obj = s_findOne(key, value);
            } catch (DBErrException e)
            {
                _callBackObj.markErr();
                return null;
            }
            return obj;
        }, _callBackObj);
    }

    public void findOne(HashMap<String, Object> conditions, _ASelectCallback<T> _callBackObj)
    {
        ALAsynTaskManager.getInstance().regTask(getDbObj().getThreadIdx(), () ->
        {
            T obj = null;
            try
            {
                obj = s_findOne(conditions);
            } catch (DBErrException e)
            {
                _callBackObj.markErr();
                return null;
            }
            return obj;

        }, _callBackObj);
    }

    public void findAll(final String key, final Object value, _ASelectCallback<List<T>> _callBackObj)
    {
        ALAsynTaskManager.getInstance().regTask(getDbObj().getThreadIdx(), () ->
        {
            List<T> retList = s_findAll(key, value);
            if (retList == null)
            {
                _callBackObj.markErr();
                return null;
            }
            return retList;
        }, _callBackObj);
    }

    public void findAll(HashMap<String, Object> conditions, _ASelectCallback<List<T>> _callBackObj)
    {
        ALAsynTaskManager.getInstance().regTask(getDbObj().getThreadIdx(), () ->
        {
            List<T> retList = s_findAll(conditions);
            if (null == retList)
            {
                _callBackObj.markErr();
                return null;
            }
            return retList;
        }, _callBackObj);
    }

    public void findAll(ALMySqlDBConditionObj conditions, _ASelectCallback<List<T>> _callBackObj)
    {
        ALAsynTaskManager.getInstance().regTask(getDbObj().getThreadIdx(), () ->
        {
            List<T> retList = s_findAll(conditions);
            if (null == retList)
            {
                _callBackObj.markErr();
                return null;
            }
            return retList;
        }, _callBackObj);
    }

    public void findAll(_ASelectCallback<List<T>> _callBackObj)
    {
        ALAsynTaskManager.getInstance().regTask(getDbObj().getThreadIdx(), () ->
        {
            List<T> retList = s_findAll();
            if (retList == null)
            {
                _callBackObj.markErr();
                return null;
            }
            return retList;
        }, _callBackObj);
    }


    public void findAll(final String key, final Object value, final int _limitCount, _ASelectCallback<List<T>> _callBackObj)
    {
        findAll(key, value, 0, _limitCount, _callBackObj);
    }

    public void findAll(final String key, final Object value, final int _limitStartIdx, final int _limitCount, _ASelectCallback<List<T>> _callBackObj)
    {
        ALAsynTaskManager.getInstance().regTask(getDbObj().getThreadIdx(), () ->
        {
            List<T> retList = s_findAll(key, value, _limitStartIdx, _limitCount);
            if (retList == null)
            {
                _callBackObj.markErr();
                return null;
            }
            return retList;
        }, _callBackObj);
    }

    public void findAll(HashMap<String, Object> conditions, final int _limitCount, _ASelectCallback<List<T>> _callBackObj)
    {
        findAll(conditions, 0, _limitCount, _callBackObj);
    }

    public void findAll(HashMap<String, Object> conditions, final int _limitStartIdx, final int _limitCount, _ASelectCallback<List<T>> _callBackObj)
    {
        ALAsynTaskManager.getInstance().regTask(getDbObj().getThreadIdx(), () ->
        {
            List<T> retList = s_findAll(conditions, _limitStartIdx, _limitCount);
            if (null == retList)
            {
                _callBackObj.markErr();
                return null;
            }
            return retList;
        }, _callBackObj);
    }

    public void findAll(ALMySqlDBConditionObj conditions, final int _limitCount, _ASelectCallback<List<T>> _callBackObj)
    {
        findAll(conditions, 0, _limitCount, _callBackObj);
    }

    public void findAll(ALMySqlDBConditionObj conditions, final int _limitStartIdx, final int _limitCount, _ASelectCallback<List<T>> _callBackObj)
    {
        ALAsynTaskManager.getInstance().regTask(getDbObj().getThreadIdx(), () ->
        {
            List<T> retList = s_findAll(conditions, _limitStartIdx, _limitCount);
            if (null == retList)
            {
                _callBackObj.markErr();
                return null;
            }
            return retList;
        }, _callBackObj);
    }

    public void findAll(final int _limitCount, _ASelectCallback<List<T>> _callBackObj)
    {
        findAll(0, _limitCount, _callBackObj);
    }

    public void findAll(final int _limitStartIdx, final int _limitCount, _ASelectCallback<List<T>> _callBackObj)
    {
        ALAsynTaskManager.getInstance().regTask(getDbObj().getThreadIdx(), () ->
        {
            List<T> retList = s_findAll(_limitStartIdx, _limitCount);
            if (retList == null)
            {
                _callBackObj.markErr();
                return null;
            }
            return retList;
        }, _callBackObj);
    }


    /****************
     * 符合条件的情况下修改对应值
     * @param _key
     * @param _value
     */
    public void update(String _key, Object _value, ALMySqlUpdateValue _updateValue)
    {
        ALAsynTaskManager.getInstance().regTask(getDbObj().getThreadIdx(), () ->
        {
            ALMySqlDBConditionObj cond = new ALMySqlDBConditionObj();
            cond.addAndEquals(String.format("`%s`", _key.replace("'", "''").replace("\\", "\\\\")), _value);
            cond.setTablesName(_m_sTableName);
            getDbObj().updateByCondition(cond, _updateValue);

        });
    }
    public void update(String _key, Object _value, ALMySqlUpdateValue _updateValue, _AUpdateCallback<Integer> _callback)
    {
        ALAsynTaskManager.getInstance().regTask(getDbObj().getThreadIdx(), () ->
        {
        	UpdateExceptionDealer errDealer = new UpdateExceptionDealer();
        	
            ALMySqlDBConditionObj cond = new ALMySqlDBConditionObj();
            cond.addAndEquals(String.format("`%s`", _key.replace("'", "''").replace("\\", "\\\\")), _value);
            cond.setTablesName(_m_sTableName);
            
            int resultCount = ALMySqlDBExcutor.updateByCondition(getDbObj().getDB(), cond, _updateValue, errDealer);
            
            if(errDealer.isHasException())
            {
            	_callback.markErr();
            	return 0;
            }
            
            return resultCount;
            
        }, _callback);
    }

    public void update(HashMap<String, Object> conditions, ALMySqlUpdateValue _updateValue)
    {
        ALAsynTaskManager.getInstance().regTask(getDbObj().getThreadIdx(), () ->
        {
            ALMySqlDBConditionObj cond = new ALMySqlDBConditionObj();
            if (null != conditions)
            {
                for (String key : conditions.keySet())
                    cond.addAndEquals(String.format("`%s`", key.replace("'", "''").replace("\\", "\\\\")), conditions.get(key));
            }
            cond.setTablesName(_m_sTableName);
            getDbObj().updateByCondition(cond, _updateValue);

        });
    }
    /**
     * 修改多条件数据
     * @param _cond
     * @param _updateValue
     */
    public void update(ALMySqlDBConditionObj _cond, ALMySqlUpdateValue _updateValue)
    {
        ALAsynTaskManager.getInstance().regTask(getDbObj().getThreadIdx(), () ->
        {
            _cond.setTablesName(_m_sTableName);

            getDbObj().updateByCondition(_cond, _updateValue);

        });
    }
}
