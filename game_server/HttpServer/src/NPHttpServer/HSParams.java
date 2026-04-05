package NPHttpServer;

import HSDB.Bo.HsParamBO;
import NPCommon.Enum.EHsParam;
import NPCommon.Log.CommLog;

import java.util.Hashtable;
import java.util.List;

public class HSParams
{
    private static HSParams _g_instance = new HSParams();

    public static HSParams getInstance()
    {
        if (null == _g_instance)
            _g_instance = new HSParams();

        return _g_instance;
    }

    //参数保存表
    private final Hashtable<Integer, HsParamBO> _m_htParamTable;

    protected HSParams()
    {
        _m_htParamTable = new Hashtable<>();
    }

    /************
     * 从数据库初始化所有数据
     * @return
     */
    public boolean initFromDB()
    {
        try
        {
            List<HsParamBO> bos = NPHttpServer.getInstance().getBM().getBM(HsParamBO.class).s_findAll();
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
                    CommLog.error("Multiple Param Tag: " + bos.get(i).getTag());

                    //删除数据
                    bos.get(i).del(NPHttpServer.getInstance().getBM());

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
    public long getParam(EHsParam _eParam)
    {
        return getParam(_eParam.ordinal());
    }

    public long getParam(int _tag)
    {
        if (!_m_htParamTable.containsKey(_tag))
            return 0L;

        return _m_htParamTable.get(_tag).getParam();
    }

    /**************
     * 设置参数
     * @param _eParam
     * @param _value
     */
    public void setParam(EHsParam _eParam, long _value)
    {
        setParam(_eParam.ordinal(), _value);
    }

    public synchronized void setParam(int _tag, long _value)
    {
        HsParamBO valueBo = _m_htParamTable.get(_tag);

        if (null == valueBo)
        {
            //创建新数据
            HsParamBO newBo = new HsParamBO();
            newBo.setTag(NPHttpServer.getInstance().getBM(), _tag);
            newBo.setParam(NPHttpServer.getInstance().getBM(), _value);

            //插入数据库
            newBo.insert(NPHttpServer.getInstance().getBM());
            //放入数据集
            _m_htParamTable.put(_tag, newBo);
        } else
        {
            //只有值不同才会设置
            if (valueBo.getParam() != _value)
                valueBo.saveParam(NPHttpServer.getInstance().getBM(), _value);
        }
    }

    /*************
     * 增加参数计数
     * @param _eParam
     * @return
     */
    public long incParam(EHsParam _eParam)
    {
        return incParam(_eParam.ordinal(), 1);
    }

    public long incParam(EHsParam _eParam, long _value)
    {
        return incParam(_eParam.ordinal(), 1);
    }

    public long incParam(int _tag)
    {
        return incParam(_tag, 1);
    }

    public synchronized long incParam(int _tag, long _value)
    {
        if (_value == 0)
            return 0;

        HsParamBO valueBo = _m_htParamTable.get(_tag);

        if (null == valueBo)
        {
            //创建新数据
            HsParamBO newBo = new HsParamBO();
            newBo.setTag(NPHttpServer.getInstance().getBM(), _tag);
            newBo.setParam(NPHttpServer.getInstance().getBM(), _value);

            //插入数据库
            newBo.insert(NPHttpServer.getInstance().getBM());
            //放入数据集
            _m_htParamTable.put(_tag, newBo);

            return _value;
        } else
        {
            valueBo.saveParam(NPHttpServer.getInstance().getBM(), valueBo.getParam() + _value);
            return valueBo.getParam();
        }
    }

    /***************
     * 重置参数为0
     * @param _eParam
     */
    public void resetParam(EHsParam _eParam)
    {
        resetParam(_eParam.ordinal());
    }

    public void resetParam(int _tag)
    {
        setParam(_tag, 0);
    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        for (EHsParam eParam : EHsParam.values())
        {
            sb.append(String.format("[%d]%s = %d \n", eParam.ordinal(), eParam, getParam(eParam)));
        }
        return sb.toString();
    }
}
