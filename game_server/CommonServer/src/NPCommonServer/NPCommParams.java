package NPCommonServer;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALServerLog.ALServerLog;
import CSDB.Bo.CommParamBO;
import NPCommon.DB.BM.BM;
import NPCommon.Log.CommLog;
import WCGCommon.Enum.ECommParam;

import java.util.Hashtable;
import java.util.List;

/**************
 * Common服务器的参数管理类
 * @author mj
 *
 */
public class NPCommParams
{
    static NPCommParams _g_instance = new NPCommParams();

    public static NPCommParams getInstance()
    {
        if (null == _g_instance)
            _g_instance = new NPCommParams();

        return _g_instance;
    }

    //参数保存表
    private Hashtable<Integer, CommParamBO> _m_htParamTable;
    //锁对象
    private MutexAtom _m_mutex;

    protected NPCommParams()
    {
        _m_htParamTable = new Hashtable<Integer, CommParamBO>();
        _m_mutex = new MutexAtom();
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    /************
     * 从数据库初始化所有数据
     * @return
     */
    public boolean initFromDB()
    {
        try
        {
            List<CommParamBO> bos = NPCommonServer.getInstance().getBM().getBM(CommParamBO.class).s_findAll();
            if (null == bos)
            {
                return false;
            }
            //逐个数据放入数据集
            for (int i = 0; i < bos.size(); i++)
            {
                if (null == bos.get(i))
                    continue;

                //判断是否有重复数据
                if (_m_htParamTable.containsKey(bos.get(i).getTag()))
                {
                    ALServerLog.Error("Multiple Param Tag: " + bos.get(i).getTag());

                    //删除数据
                    bos.get(i).del(NPCommonServer.getInstance().getBM());

                    continue;
                }

                //放入数据集
                _m_htParamTable.put(bos.get(i).getTag(), bos.get(i));
            }
        } catch (Exception e)
        {
            CommLog.error("init common params error", e);
            return false;
        }

        return true;
    }

    /************
     * 根据参数类型获取对应参数
     * @param _eParam
     * @return
     */
    public long getParam(ECommParam _eParam)
    {
        return getParam(_eParam.ordinal());
    }

    public long getParam(int _tag)
    {
        _lock();

        try
        {
            if (!_m_htParamTable.containsKey(_tag))
                return 0L;

            return _m_htParamTable.get(_tag).getParam();
        } finally
        {
            _unlock();
        }
    }

    /**************
     * 设置参数
     * @param _eParam
     * @param _value
     */
    public void setParam(ECommParam _eParam, long _value)
    {
        setParam(_eParam.ordinal(), _value);
    }

    public void setParam(int _tag, long _value)
    {
        _lock();

        try
        {
            CommParamBO valueBo = _m_htParamTable.get(_tag);

            BM bmObj = NPCommonServer.getInstance().getBM();
            if (null == valueBo)
            {
                //创建新数据
                CommParamBO newBo = new CommParamBO();
                newBo.setTag(bmObj, _tag);
                newBo.setParam(bmObj, _value);

                //插入数据库
                newBo.insert(bmObj);
                //放入数据集
                _m_htParamTable.put(_tag, newBo);
            } else
            {
                //只有值不同才会设置
                if (valueBo.getParam() != _value)
                    valueBo.saveParam(bmObj, _value);
            }
        } finally
        {
            _unlock();
        }
    }

    /*************
     * 增加参数计数
     * @param _eParam
     * @return
     */
    public long incParam(ECommParam _eParam)
    {
        return incParam(_eParam.ordinal(), 1);
    }

    public long incParam(ECommParam _eParam, long _value)
    {
        return incParam(_eParam.ordinal(), 1);
    }

    public long incParam(int _tag)
    {
        return incParam(_tag, 1);
    }

    public long incParam(int _tag, long _value)
    {
        _lock();

        try
        {
            CommParamBO valueBo = _m_htParamTable.get(_tag);

            BM bmObj = NPCommonServer.getInstance().getBM();
            if (null == valueBo)
            {
                //创建新数据
                CommParamBO newBo = new CommParamBO();
                newBo.setTag(bmObj, _tag);
                newBo.setParam(bmObj, _value);

                //插入数据库
                newBo.insert(bmObj);
                //放入数据集
                _m_htParamTable.put(_tag, newBo);

                return _value;
            } else
            {
                //增加计数值
                valueBo.saveParam(bmObj, valueBo.getParam() + _value);

                return valueBo.getParam();
            }
        } finally
        {
            _unlock();
        }
    }

    /***************
     * 重置参数为0
     * @param _eParam
     */
    public void resetParam(ECommParam _eParam)
    {
        resetParam(_eParam.ordinal());
    }

    public void resetParam(int _tag)
    {
        setParam(_tag, 0);
    }

    public void dump()
    {
        StringBuilder sb = new StringBuilder();
        sb.append("\n================Common Params===============");
        for (ECommParam eParam : ECommParam.values())
        {
            String str = String.format("\n\t[%s]=%d", eParam.toString(), getParam(eParam));
            sb.append(str);
        }
        CommLog.info(sb.toString());
    }
}
