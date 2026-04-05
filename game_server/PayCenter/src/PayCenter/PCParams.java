package PayCenter;

import NPCommon.Log.CommLog;
import PayDB.Bo.PcParamBO;

import java.util.Hashtable;
import java.util.List;

/**
 * PCParams - PayCenter系统参数管理器
 * 
 * 主要功能：
 * 1. 管理PayCenter系统级参数配置
 * 2. 从数据库加载和缓存系统参数
 * 3. 提供参数的读取和更新接口
 * 4. 支持参数的热更新机制
 * 
 * 设计特点：
 * - 单例模式管理全局参数
 * - 数据库持久化存储
 * - 内存缓存提高访问性能
 * - 同步加载初始化数据
 * 
 * 线程安全：使用同步机制保护参数访问
 */
public class PCParams
{
    private static final PCParams _g_instance = new PCParams();

    public static PCParams getInstance()
    {
        return _g_instance;
    }

    // 参数保存表
    private Hashtable<Integer, PcParamBO> _m_htParamTable;

    public PCParams()
    {
        _m_htParamTable = new Hashtable<Integer, PcParamBO>();
    }

    /**
     * 从数据库初始化所有数据
     * 
     * @return true=初始化成功, false=初始化失败
     */
    public boolean initFromDB()
    {
        try
        {
            List<PcParamBO> boList = PayCenter.getInstance().getBM().getBM(PcParamBO.class).s_findAll();
            if (null == boList)
                return false;

            // 逐个数据放入数据集
            for (PcParamBO bo : boList)
            {
                if (null == bo)
                    continue;

                // 判断是否有重复数据
                if (_m_htParamTable.containsKey(bo.getTag()))
                {
                    CommLog.error("PCParams.initFromDB found duplicate data tag:{}", bo.getTag());
                    return false;
                }

                // 放入数据集
                _m_htParamTable.put(bo.getTag(), bo);
            }
        } catch (Exception e)
        {
            CommLog.error("PCParams.initFromDB", e);
            return false;
        }

        return true;
    }

    /**
     * 根据参数类型获取对应参数
     * 
     * @param _tag 参数标记
     * @return 参数值
     */
    public long getParam(int _tag)
    {
        if (!_m_htParamTable.containsKey(_tag))
            return 0L;

        return _m_htParamTable.get(_tag).getParam();
    }

    /**
     * 设置参数
     * 
     * @param _tag 参数标记
     * @param _value 参数值
     */
    public synchronized void setParam(int _tag, long _value)
    {
        PcParamBO valueBo = _m_htParamTable.get(_tag);

        if (null == valueBo)
        {
            // 创建新数据
            PcParamBO newBo = new PcParamBO();
            newBo.setTag(PayCenter.getInstance().getBM(), _tag);
            newBo.setParam(PayCenter.getInstance().getBM(), _value);

            // 插入数据库
            newBo.insert(PayCenter.getInstance().getBM());
            // 放入数据集
            _m_htParamTable.put(_tag, newBo);
        } else
        {
            // 只有值不同才会设置
            if (valueBo.getParam() != _value)
                valueBo.saveParam(PayCenter.getInstance().getBM(), _value);
        }
    }

    /**
     * 增加参数计数
     * 
     * @param _tag 参数标记
     * @return 增加后的参数值
     */
    public long incParam(int _tag)
    {
        return incParam(_tag, 1);
    }

    public synchronized long incParam(int _tag, long _value)
    {
        PcParamBO valueBo = _m_htParamTable.get(_tag);

        if (null == valueBo)
        {
            // 创建新数据
            PcParamBO newBo = new PcParamBO();
            newBo.setTag(PayCenter.getInstance().getBM(), _tag);
            newBo.setParam(PayCenter.getInstance().getBM(), _value);

            // 插入数据库
            newBo.insert(PayCenter.getInstance().getBM());
            // 放入数据集
            _m_htParamTable.put(_tag, newBo);

            return _value;
        } else
        {
            valueBo.saveParam(PayCenter.getInstance().getBM(), valueBo.getParam() + _value);
            return valueBo.getParam();
        }
    }

    /**
     * 重置参数为0
     * 
     * @param _tag 参数标记
     */
    public void resetParam(int _tag)
    {
        setParam(_tag, 0);
    }
}