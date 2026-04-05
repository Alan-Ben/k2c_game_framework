package NPCommon.RefData.Ref;

import NPCommon.Log.CommLog;
import NPCommon.RefData.AbstractRefDataMgr;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.Util.RunResult;

import java.lang.reflect.Field;
import java.util.Locale;
import java.util.Map;

public abstract class RefBase
{

    /**
     * 支持基本类型 public int _int; public int _long; public String _str; public
     * boolean _bool; 支持数组 (非简单类型) public Integer[] _ints; 支持List<>嵌套 public
     * ArrayList<String> _sList; 支持本路径下的其他结构体 public RefData _ref;
     * 支持ConstEnum.java里的枚举 public TestEnum _enum;
     */

    public abstract void resetRef(RefBase _newRef);

    /**********
     * 获取对象数据Id，尽量唯一
     *
     * @author alzq.z
     * @time 2019年4月3日 下午11:35:24
     */
    public abstract long Id();

    /**
     * 进行断言，发现问题配表存在问题时终止服务器启动
     * @return FALSE 终止启动，TRUE 校验通过
     */
    public boolean Assert()
    {
        return true;
    }

    /**
     * 获取容器对象
     * @author alzq.z
     * @time 2019年4月4日 下午8:44:03
     */
    public abstract RefContainerBase<? extends RefBase> getStaticContainer();

    public abstract void setStaticContainer(RefContainerBase<? extends RefBase> _mgr);


    @Override
    public String toString()
    {
        return super.toString();
    }

    public boolean gmSetValue(String key, String value)
    {
        CommLog.info("Gm 修改配置:{} = {} ", key, value);
        return internalSetValue(key, value);
    }

    public boolean internalSetValue(String key, String value)
    {
        try
        {
            Field field = null;
            try
            {
                field = this.getClass().getField(key);
            } catch (Exception e)
            {

            }
            if (field == null)
            {
                return false;
            }

            Object obj = AbstractRefDataMgr.parseCreateObjAndList(field, value, getStaticContainer().getTableName(), field.getName());
            if (null == obj)
            {
                CommLog.error("internalSetValue 表：{} 字段 {} 无法解析值：{}", getStaticContainer().getTableName(), key, value);
                return false;
            }

            field.set(this, obj);
            return true;
        } catch (Exception e)
        {
            CommLog.error("internalSetValue error:", e);
            return false;
        }
    }

    public RunResult gmGetValue(String key)
    {
        Field field;
        try
        {
            field = this.getClass().getField(key);
            String ret = field.get(this).toString();
            return RunResult.succ(ret);
        } catch (Exception e)
        {
            CommLog.error("get field [" + key + "] not found:" + e.getClass().getSimpleName());
            return RunResult.failed(e.getClass().getSimpleName());
        }
    }

    /**
     * 给定所有filed值，重新设置
     * @param _keyValMap
     */
    public void internalSetValue(Map<String, String> _keyValMap)
    {
        try
        {
            for (Field field : this.getClass().getFields())
            {
                if (field == null)
                {
                    continue;
                }
                RefField annotation = field.getAnnotation(RefField.class);
                //忽略此filed
                if (annotation != null && annotation.isIgnore())
                {
                    continue;
                }
                String value = _keyValMap.get(field.getName().toLowerCase(Locale.ROOT));
                if (value == null)
                {
                    continue;
                }
                //实例化一个field的对象
                Object obj = AbstractRefDataMgr.parseCreateObjAndList(field, value,
                        getStaticContainer().getTableName(), field.getName());
                if (null == obj)
                {
                    CommLog.error("internalSetValue 表：{} 字段 {} 无法解析值：{}", getStaticContainer().getTableName(), field.getName(), value);
                    continue;
                }

                field.set(this, obj);
            }
        } catch (Exception e)
        {
            CommLog.error("internalSetValue error:", e);
        }
    }


}
