package ShareCodeCenter;

import NPCommon.Enum.ESccParam;
import NPCommon.Log.CommLog;
import ShareCodeDB.Bo.SccParamBO;

import java.util.Hashtable;
import java.util.List;

public class SCCParams
{
    private static final SCCParams _g_instance = new SCCParams();

    public static SCCParams getInstance()
    {
        return _g_instance;
    }

    //参数保存表
    private Hashtable<Integer, SccParamBO> _m_htParamTable;

    public SCCParams()
    {
        _m_htParamTable = new Hashtable<Integer, SccParamBO>();
    }


    /************
     * 从数据库初始化所有数据
     * @return
     */
    public boolean initFromDB()
    {
        try
        {
            List<SccParamBO> boList = ShareCodeCenter.getInstance().getBM().getBM(SccParamBO.class).s_findAll();
            if (null == boList)
                return false;

            //逐个数据放入数据集
            for (SccParamBO bo : boList)
            {
                if (null == bo)
                    continue;

                //判断是否有重复数据
                if (_m_htParamTable.containsKey(bo.getTag()))
                {
                    CommLog.error("SCCParams.initFromDB found duplicate data tag:{}", bo.getTag());
                    return false;
                }

                //放入数据集
                _m_htParamTable.put(bo.getTag(), bo);
            }
        } catch (Exception e)
        {
            CommLog.error("SCCParams.initFromDB", e);
            return false;
        }

        return true;
    }

    /************
     * 根据参数类型获取对应参数
     * @param _eParam
     * @return
     */
    public long getParam(ESccParam _eParam)
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
    public void setParam(ESccParam _eParam, long _value)
    {
        setParam(_eParam.ordinal(), _value);
    }

    public synchronized void setParam(int _tag, long _value)
    {
        SccParamBO valueBo = _m_htParamTable.get(_tag);

        if (null == valueBo)
        {
            //创建新数据
            SccParamBO newBo = new SccParamBO();
            newBo.setTag(ShareCodeCenter.getInstance().getBM(), _tag);
            newBo.setParam(ShareCodeCenter.getInstance().getBM(), _value);

            //插入数据库
            newBo.insert(ShareCodeCenter.getInstance().getBM());
            //放入数据集
            _m_htParamTable.put(_tag, newBo);
        } else
        {
            //只有值不同才会设置
            if (valueBo.getParam() != _value)
                valueBo.saveParam(ShareCodeCenter.getInstance().getBM(), _value);
        }
    }

    /*************
     * 增加参数计数
     * @param _eParam
     * @return
     */
    public long incParam(ESccParam _eParam)
    {
        return incParam(_eParam.ordinal(), 1);
    }

    public long incParam(ESccParam _eParam, long _value)
    {
        return incParam(_eParam.ordinal(), _value);
    }

    public long incParam(int _tag)
    {
        return incParam(_tag, 1);
    }

    public synchronized long incParam(int _tag, long _value)
    {
        SccParamBO valueBo = _m_htParamTable.get(_tag);

        if (null == valueBo)
        {
            //创建新数据
            SccParamBO newBo = new SccParamBO();
            newBo.setTag(ShareCodeCenter.getInstance().getBM(), _tag);
            newBo.setParam(ShareCodeCenter.getInstance().getBM(), _value);

            //插入数据库
            newBo.insert(ShareCodeCenter.getInstance().getBM());
            //放入数据集
            _m_htParamTable.put(_tag, newBo);

            return _value;
        } else
        {
            valueBo.saveParam(ShareCodeCenter.getInstance().getBM(), valueBo.getParam() + _value);

            return valueBo.getParam();
        }
    }

    /***************
     * 重置参数为0
     * @param _eParam
     */
    public void resetParam(ESccParam _eParam)
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
        for (ESccParam eParam : ESccParam.values())
        {
            sb.append(String.format("[%d]%s = %d \n", eParam.ordinal(), eParam, getParam(eParam)));
        }
        return sb.toString();
    }
}
