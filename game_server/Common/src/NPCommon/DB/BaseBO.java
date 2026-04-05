package NPCommon.DB;

import ALBasicServer.ALServerAsynTask.ALAsynTaskManager;
import ALBasicServer.ALTask._IALAsynCallBackTask;
import ALBasicServer.ALTask._IALAsynRunnableTask;
import ALMySqlCommon.ALMySqlDBConditionObj;
import ALMySqlCommon.ALMySqlSafeOp._TALMySqlSafeOpDBObj;
import ALServerLog.ALServerLog;
import NPCommon.DB.Annotation.RefBo;
import NPCommon.DB.BM.BM;
import NPCommon.DB.BoChecker.BoChecker;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommClass;
import NPCommon.Util.CommonFunc;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.BitSet;
import java.util.HashMap;

/**
 * 对BaseBo添加异步修改接口
 * <p>
 * 作者：clark
 */
public abstract class BaseBO implements IBaseBO, Comparable<BaseBO>
{

    private BitSet m_lFieldMark = new BitSet(64);
    private long _m_lCheckSerial = 0; //验证bo是否保存的事务序列号

    /******
     * 返回当前检测序列号
     * @return
     */
    public long getCheckSerial()
    {
        return _m_lCheckSerial;
    }

    /******
     * 清除BoCheck
     */
    public void clearCheck()
    {
        _m_lCheckSerial = 0;
    }

    public void clearMark()
    {
        synchronized (m_lFieldMark)
        {
            m_lFieldMark.clear();
        }

    }

    protected BaseBO()
    {
    }

    protected void markField(BM _bm, int bitIndex)
    {
        synchronized (m_lFieldMark)
        {
            m_lFieldMark.set(bitIndex);
        }
        _m_lCheckSerial = BoChecker.getInstance().addCheck(_bm, this);
    }

    public boolean hasMarkedField()
    {
        synchronized (m_lFieldMark)
        {
            return !m_lFieldMark.isEmpty();
        }
    }

    public boolean isFieldMarked(int bitIndex)
    {
        synchronized (m_lFieldMark)
        {
            return m_lFieldMark.get(bitIndex);
        }
    }

    /** 获取数据库对象 */
    private _TALMySqlSafeOpDBObj<WCGDBObj> getDBObj(BM _bm)
    {
        return WCGDBFactory.getDbObj(_bm.getRealDBTag(getDBTag()));
    }

    public static BaseBO createFromByteBuffer(ByteBuffer buffer)
    {
        if (null == buffer)
        {
            CommLog.error("createFromByteBuffer failed,buffer==null");
            return null;
        }
        String className = "";
        try
        {
            className = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buffer);
        } catch (Exception e)
        {
            CommLog.error(" createFromByteBuffer read class name failed:", e);
            return null;
        }
        if (className.isEmpty())
        {
            CommLog.error(" createFromByteBuffer read class name failed ,class name is empty");
            return null;
        }
        Class<?> clazz = null;
        try
        {
            clazz = CommClass.forName(className);
        } catch (Exception e)
        {
            CommLog.error(" createFromByteBuffer get class for name:{} failed", className, e);
            return null;
        }
        if (null == clazz)
        {
            CommLog.error(" createFromByteBuffer can not get class for name:{}", className);
            return null;
        }

        BaseBO bo = null;
        try
        {
            bo = (BaseBO) clazz.newInstance();
        } catch (Exception e)
        {
            CommLog.error(" createFromByteBuffer can not instance class for name:{}", className, e);
            return null;
        }
        if (null == bo)
        {
            CommLog.error(" createFromByteBuffer can not instance class for name:{}", className);
            return null;
        }
        try
        {
            bo.readFromByteBuffer(buffer);
        } catch (Exception e)
        {
            CommLog.error(" createFromByteBuffer read data from bo failed ,class name:{}", className, e);
            return null;
        }

        return bo;
    }


    // 异步插入带回调
    @SuppressWarnings("unchecked")
    public <T extends BaseBO> void insert(BM _bm, _IALAsynCallBackTask<T> _CallBackTask)
    {
        _m_lCheckSerial = 0;

        //判断是否系统控制Id
        RefBo boInfo = this.getClass().getAnnotation(RefBo.class);
        if ((null == boInfo || boInfo.isIdAuto()) && this.getId() != 0)
        {
            ALServerLog.Fatal("插入数据，但是已经设置了Id，插入失败！" + getClass().getName());
            return;
        }
        clearMark();
        if (this.getId() == 0)
        {
            this.setId(_bm.getBM(this.getClass()).getNextId());
        }
        final BaseBO bo = this;
        _TALMySqlSafeOpDBObj<WCGDBObj> dbObj = getDBObj(_bm);
        ALAsynTaskManager.getInstance().regTask(dbObj.getThreadIdx(), () ->
        {
            dbObj.InsertBO(bo, bo.getInsertValueBytes());
            return (T) (bo);
        }, _CallBackTask);
    }

    public boolean insert_sync(final BM _bm)// 同步插入
    {
        _m_lCheckSerial = 0;
        clearMark();
        //判断是否系统控制Id
        RefBo boInfo = this.getClass().getAnnotation(RefBo.class);
        if ((null == boInfo || boInfo.isIdAuto()) && this.getId() != 0)
        {
            ALServerLog.Fatal("插入数据，但是已经设置了Id，插入失败！" + getClass().getName());
            return false;
        }

        if (this.getId() == 0)
        {
            this.setId(_bm.getBM(this.getClass()).getNextId());
        }
        getDBObj(_bm).InsertBO(this, this.getInsertValueBytes());
        return true;
    }

    public void insert(final BM _bm) // 异步插入，如果存在则变成更新。
    {
        _m_lCheckSerial = 0;

        //判断是否系统控制Id
        RefBo boInfo = this.getClass().getAnnotation(RefBo.class);
        if ((null == boInfo || boInfo.isIdAuto()) && this.getId() != 0)
        {
            ALServerLog.Fatal("插入数据，但是已经设置了Id，插入失败！" + getClass().getName());
            return;
        }

        if (this.getId() == 0)
        {
            this.setId(_bm.getBM(this.getClass()).getNextId());
        }
        clearMark();

        final BaseBO bo = this;
        _TALMySqlSafeOpDBObj<WCGDBObj> dbObj = getDBObj(_bm);
        ALAsynTaskManager.getInstance().regTask(dbObj.getThreadIdx(), () ->
        {
            dbObj.InsertBO(bo, bo.getInsertValueBytes());
        });
    }


    public void replace(final BM _bm) // 异步插入，如果存在则变成更新。
    {
        _m_lCheckSerial = 0;
        clearMark();
        if (this.getId() == 0)
        {
            this.setId(_bm.getBM(this.getClass()).getNextId());
        }

        final BaseBO bo = this;
        _TALMySqlSafeOpDBObj<WCGDBObj> dbObj = getDBObj(_bm);
        ALAsynTaskManager.getInstance().regTask(dbObj.getThreadIdx(), () ->
        {
            dbObj.ReplaceBO(bo);
        });
    }

    //异步删除
    public void del(final BM _bm)
    {
        final BaseBO _bo = this;

        _TALMySqlSafeOpDBObj<WCGDBObj> dbObj = getDBObj(_bm);
        ALAsynTaskManager.getInstance().regTask(dbObj.getThreadIdx(), () ->
        {
            ALMySqlDBConditionObj cond = new ALMySqlDBConditionObj();
            cond.setTablesName(_bo.getTbName());
            cond.addAndEquals("`id`", _bo.getId());
            dbObj.delByCondition(cond);
        });
    }

    //同步删除
    public void del_sync(final BM _bm)
    {
        ALMySqlDBConditionObj cond = new ALMySqlDBConditionObj();
        cond.setTablesName(getTbName());
        cond.addAndEquals("`id`", getId());
        getDBObj(_bm).delByCondition(cond);
    }

    protected void saveField(final BM _bm, final String _field, final Object _newValue)
    {
        _TALMySqlSafeOpDBObj<WCGDBObj> dbObj = getDBObj(_bm);
        ALAsynTaskManager.getInstance().regTask(dbObj.getThreadIdx(), () ->
        {
            ALMySqlDBConditionObj cond = new ALMySqlDBConditionObj();
            cond.setTablesName(getTbName());
            cond.addAndEquals("`id`", getId());

            Object val = _newValue;
            if (_newValue instanceof Boolean)
            { // boolean 在db中用tinyint表示
                boolean tmp = (Boolean) _newValue;
                val = tmp ? 1 : 0;
            }
            if (_newValue instanceof String)
            {
                val = ((String) _newValue).replace("'", "''").replace("\\", "\\\\");
            }

            String sql = String.format("`%s` = '%s'", _field, val);
            dbObj.updateByCondition(cond, sql);
        });

    }

    protected void saveFieldBytes(final BM _bm, final String _field, final byte[] _newValue)
    {
        _TALMySqlSafeOpDBObj<WCGDBObj> dbObj = getDBObj(_bm);
        ALAsynTaskManager.getInstance().regTask(dbObj.getThreadIdx(), () ->
        {
            ALMySqlDBConditionObj cond = new ALMySqlDBConditionObj();
            cond.setTablesName(getTbName());
            cond.addAndEquals("`id`", getId());

            String sql = String.format("`%s` = ?", _field);
            ArrayList<byte[]> _binaryList = new ArrayList<>();
            _binaryList.add(_newValue);
            dbObj.updateByCondition(cond, sql, _binaryList);
        });
    }

    public void saveAllMarkedByKey(final BM _bm, final HashMap<String, Object> conditions)
    {
        String sMarkedUpdateKeyValue;
        ArrayList<byte[]> markedValueBytes;
        synchronized (m_lFieldMark)
        {
            if (m_lFieldMark.isEmpty())
            {
                //CommLog.error("== saveAllMarkedByKey field mark ==0,Bo:{} id:{}",getClass().getSimpleName(),hashCode());
                return;
            }
            sMarkedUpdateKeyValue = getMarkedUpdateKeyValue();
            markedValueBytes = getMarkedValueBytes();
            clearMark();
        }
        final BaseBO bo = this;
        final String finalMarkedUpdateKeyValue = sMarkedUpdateKeyValue;
        final ArrayList<byte[]> finalMarkedValueBytes = markedValueBytes;
        ALAsynTaskManager.getInstance().regTask(getDBObj(_bm).getThreadIdx()
                , () -> bo.saveFileds_sync(_bm, finalMarkedUpdateKeyValue, finalMarkedValueBytes, conditions));

    }


    public void saveAllMarkedByKey(final BM _bm, final String key, final Object value)
    {
        final HashMap<String, Object> conditions = new HashMap<String, Object>();
        conditions.put(key, value);
        saveAllMarkedByKey(_bm, conditions);
    }

    public void saveAllMarked(final BM _bm)
    {
        _m_lCheckSerial = 0;
        if (this.getId() == 0)
        {
            //CommLog.error("[{}]数据还未创建，不能部分更新", this.getTbName(),new Exception());
            return;
        }
        String sMarkedUpdateKeyValue;
        ArrayList<byte[]> markedValueBytes;
        synchronized (m_lFieldMark)
        {
            if (m_lFieldMark.isEmpty())
            {
                //CommLog.error("== saveAllMarked field mark ==0,Bo:{} id:{}",getClass().getSimpleName(),hashCode());
                return;
            }
            sMarkedUpdateKeyValue = getMarkedUpdateKeyValue();
            markedValueBytes = getMarkedValueBytes();
            clearMark();
        }
        final BaseBO bo = this;
        final String finalMarkedUpdateKeyValue = sMarkedUpdateKeyValue;
        final ArrayList<byte[]> finalMarkedValueBytes = markedValueBytes;
        ALAsynTaskManager.getInstance().regTask(getDBObj(_bm).getThreadIdx(), new _IALAsynRunnableTask()
        {
            @Override
            public void run()
            {
                bo.saveFileds_sync(_bm, finalMarkedUpdateKeyValue, finalMarkedValueBytes);
            }
        });


    }

    protected void saveFileds_sync(final BM _bm, String sUpdateKeyValue, ArrayList<byte[]> valueBytes, HashMap<String, Object> conditions)
    {
        ALMySqlDBConditionObj cond = new ALMySqlDBConditionObj();
        cond.setTablesName(getTbName());
        for (String key : conditions.keySet())
            cond.addAndEquals(String.format("`%s`", key), conditions.get(key));
        getDBObj(_bm).updateByCondition(cond, sUpdateKeyValue, valueBytes);
    }

    protected void saveFileds_sync(final BM _bm, String sUpdateKeyValue, ArrayList<byte[]> valueBytes)
    {
        ALMySqlDBConditionObj cond = new ALMySqlDBConditionObj();
        cond.setTablesName(getTbName());
        cond.addAndEquals("`id`", getId());

        getDBObj(_bm).updateByCondition(cond, sUpdateKeyValue, valueBytes);
    }

    protected abstract ArrayList<byte[]> getMarkedValueBytes();

    public abstract String getMarkedUpdateKeyValue();

    public void saveAll(final BM _bm)
    {
        _m_lCheckSerial = 0;
        clearMark();

        _TALMySqlSafeOpDBObj<WCGDBObj> dbObj = getDBObj(_bm);
        if (this.getId() == 0)
        {
            CommLog.error("[{}]数据还未创建，进行insert操作", this.getTbName());
            this.setId(_bm.getBM(this.getClass()).getNextId());
            final BaseBO bo = this;
            ALAsynTaskManager.getInstance().regTask(dbObj.getThreadIdx(), () ->
            {
                dbObj.InsertBO(bo, bo.getInsertValueBytes());
            });
        } else
        {
            final BaseBO bo = this;
            ALAsynTaskManager.getInstance().regTask(dbObj.getThreadIdx(), () -> bo.saveAll_sync(_bm));
        }
    }

    public void saveAll_sync(final BM _bm)
    {
        ALMySqlDBConditionObj cond = new ALMySqlDBConditionObj();
        cond.setTablesName(getTbName());
        cond.addAndEquals("`id`", getId());

        getDBObj(_bm).updateByCondition(cond, getUpdateKeyValue(), getInsertValueBytes());
    }

    /**
     * `%s` = %s, `%s` = %s, ...
     * @return
     */
    protected abstract String getUpdateKeyValue();

    protected abstract ArrayList<byte[]> getInsertValueBytes();

    public abstract ByteBuffer toByteBuffer();

    protected abstract void readFromByteBuffer(ByteBuffer _buffer);


    @Override
    public int compareTo(BaseBO o)
    {
        return (int) (getId() - o.getId());
    }

    @Override
    public String getInsertItemsName()
    {
        return getItemsName();
    }

    @Override
    public String getInsertValuesStr()
    {
        return getItemsValue();
    }

    @Override
    public String getSelectItemsName()
    {
        return getItemsName();
    }

    @Override
    public String getTbName()
    {
        return getTableName();
    }

    public int getRecordExpiredTimeSec()
    {
        return 0;
    }


    public String toLineString()
    {
        String[] names = CommonFunc.charSplit(getItemsName(), ',');
        String[] values = CommonFunc.charSplit(getItemsValue(), ',');
        StringBuilder sb = new StringBuilder();
        sb.append("Table:"+getTableName()+" ");
        for(int i=0;i<names.length;i++)
        {
            sb.append(String.format("[%s] = %s ",names[i],values[i]));
        }
        return sb.toString();
    }
}
