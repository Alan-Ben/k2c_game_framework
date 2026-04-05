package NPUSServer;

import NPCommon.Enum.EUsParam;
import USDB.Bo.UsParamBO;

import java.util.Hashtable;
import java.util.List;

public class USParams
{
    private NPUserServer _m_usUSServer;
    //参数保存表
    private Hashtable<Integer, UsParamBO> _m_htParamTable;

    public USParams(NPUserServer _usServer)
    {
        _m_usUSServer = _usServer;

        _m_htParamTable = new Hashtable<Integer, UsParamBO>();
    }

    public NPUserServer getUSServer() {return _m_usUSServer;}

    /************
     * 从数据库初始化所有数据
     * @return
     */
    public boolean initFromDB()
    {
        try
        {
            List<UsParamBO> bos = getUSServer().getBM().getBM(UsParamBO.class).s_findAll();
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
                    USLog.error(_m_usUSServer, "Multiple Param Tag: " + bos.get(i).getTag());

                    //删除数据
                    bos.get(i).del(getUSServer().getBM());

                    continue;
                }

                //放入数据集
                _m_htParamTable.put(bos.get(i).getTag(), bos.get(i));
            }
        } catch (Exception e)
        {
            USLog.error(_m_usUSServer, "init common params error", e);
            return false;
        }

        return true;
    }

    /************
     * 根据参数类型获取对应参数
     * @param _eParam
     * @return
     */
    public long getParam(EUsParam _eParam)
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
    public void setParam(EUsParam _eParam, long _value)
    {
        setParam(_eParam.ordinal(), _value);
    }

    public synchronized void setParam(int _tag, long _value)
    {
        UsParamBO valueBo = _m_htParamTable.get(_tag);

        if (null == valueBo)
        {
            //创建新数据
            UsParamBO newBo = new UsParamBO();
            newBo.setTag(getUSServer().getBM(), _tag);
            newBo.setParam(getUSServer().getBM(), _value);

            //插入数据库
            newBo.insert(getUSServer().getBM());
            //放入数据集
            _m_htParamTable.put(_tag, newBo);
        } else
        {
            //只有值不同才会设置
            if (valueBo.getParam() != _value)
                valueBo.saveParam(getUSServer().getBM(), _value);
        }
    }

    /*************
     * 增加参数计数
     * @param _eParam
     * @return
     */
    public long incParam(EUsParam _eParam)
    {
        return incParam(_eParam.ordinal(), 1);
    }

    public long incParam(EUsParam _eParam, long _value)
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

        UsParamBO valueBo = _m_htParamTable.get(_tag);

        if (null == valueBo)
        {
            //创建新数据
            UsParamBO newBo = new UsParamBO();
            newBo.setTag(getUSServer().getBM(), _tag);
            newBo.setParam(getUSServer().getBM(), _value);

            //插入数据库
            newBo.insert(getUSServer().getBM());
            //放入数据集
            _m_htParamTable.put(_tag, newBo);

            return _value;
        } else
        {
            valueBo.saveParam(getUSServer().getBM(), valueBo.getParam() + _value);
            return valueBo.getParam();
        }
    }

    /***************
     * 重置参数为0
     * @param _eParam
     */
    public void resetParam(EUsParam _eParam)
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
        for (EUsParam eParam : EUsParam.values())
        {
            sb.append(String.format("[%d]%s = %d \n", eParam.ordinal(), eParam, getParam(eParam)));
        }
        return sb.toString();
    }
}
