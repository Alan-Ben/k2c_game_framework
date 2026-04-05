package NPCommon.DB.Version.TbIDMgr;

import ALBasicServer.ALServerAsynTask.ALAsynTaskManager;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALMySqlCommon.ALMySqlDBConditionObj;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlDBExcutor;
import ALMySqlCommon.ALMySqlSafeOp._TALMySqlSafeOpDBObj;
import ALServerLog.ALServerLog;
import NPCommon.DB.Version.DBVersionInfoBO;
import NPCommon.DB.WCGDBFactory;
import NPCommon.DB.WCGDBObj;
import WCGCommon.ALProtocolBuf;

import java.util.ArrayList;
import java.util.Hashtable;

/***************
 * TbId管理器
 * 针对每个表的ID进行统一管理，避免清库或者删数据导致Id重新开始的问题
 *
 * 字节存储的格式如下：
 * 数量
 * 数据队列，每个数据格式为：表明+当前最大Id
 */
public class TBIdMgr
{
    //数据库对应数据Id
    private long _m_lDbId;
    private String _m_sDBTbName;
    //归属数据库标记
    private WCGDBObj _m_dbObj;

    //所有Id数据存储的队列，用于后续定时写入数据库进行处理
    private ArrayList<TBIdInfo> _m_lTbIdInfoList;
    //构建Map减少检索时间
    private Hashtable<String, TBIdInfo> _m_mapTbTbIdInfoDict;

    //总Byte尺寸大小
    private int _m_iTotalByteSize;

    //数据是否已经脏了，需要处理
    private boolean _m_bIsDirty;

    public TBIdMgr(WCGDBObj _dbObj)
    {
        _m_lDbId = 0;
        _m_sDBTbName = null;
        _m_dbObj = _dbObj;

        _m_lTbIdInfoList = new ArrayList<>();
        _m_mapTbTbIdInfoDict = new Hashtable<>();
        _m_iTotalByteSize = 2;

        _m_bIsDirty = false;
    }

    //设置数据脏了需要处理
    public void setDirty() {_m_bIsDirty = true;}

    /**
     * 初始化数据
     * @param _bo 带入数据库对象
     */
    public void init(DBVersionInfoBO _bo)
    {
        if(null == _bo) {
            ALServerLog.Fatal("TBIdMgr.init DBVersionInfoBO is null");
            return;
        }

        _m_lDbId = _bo.getId();
        _m_sDBTbName = _bo.getTbName();

        //无数据直接返回
        if(null == _bo.getIdBlob() || _bo.getIdBlob().length <= 0) {
            //此处需要开启定时任务，定时更新数据库Id记录数据
            ALSynTaskManager.getInstance().regTask(new SyncTaskSaveDBIDInfo(this, 300000));
            return;
        }

        //构建读取对象
        ALProtocolBuf bufReader = new ALProtocolBuf(_bo.getIdBlob());
        synchronized (this)
        {
            //先读取总数量
            short count = bufReader.getShort();
            for(int i = 0; i < count; i++)
            {
                //逐个读取
                TBIdInfo newInfo = new TBIdInfo(this, bufReader.getString(), bufReader.getLong());

                //先查询是否已有数据
                TBIdInfo existInfo = _m_mapTbTbIdInfoDict.get(newInfo.getTbName());
                if(null == existInfo)
                {
                    //设置为当前创建数据
                    existInfo = newInfo;

                    //放入数据集
                    _m_lTbIdInfoList.add(newInfo);
                    _m_mapTbTbIdInfoDict.put(newInfo.getTbName(), newInfo);

                    //累加总大小
                    _m_iTotalByteSize += newInfo.getBufferSize();
                }
                else
                {
                    //如新数据Id比较大，说明数据库中数据是正确的，需要将已有数据重置
                    if(newInfo.getMaxId() >= existInfo.getMaxId())
                    {
                        existInfo.initMaxId(newInfo.getMaxId());
                    }
                }
            }
        }

        //强制设置dirty为true，确保如果有新的表数据一定会被记录
        _m_bIsDirty = true;
        //此处需要开启定时任务，定时更新数据库Id记录数据
        ALSynTaskManager.getInstance().regTask(new SyncTaskSaveDBIDInfo(this, 300000));
    }

    /***
     * 获取当前对应表的Id信息对象
     * 如无对象会直接创建一个对象
     * @param _tbName
     * @return
     */
    public TBIdInfo ensureIdInfo(String _tbName)
    {
        synchronized (this)
        {
            //如有数据直接返回
            TBIdInfo tbInfo = _m_mapTbTbIdInfoDict.get(_tbName);
            if(null != tbInfo)
                return tbInfo;

            //创建一个新数据
            tbInfo = new TBIdInfo(this, _tbName);
            //放入数据集
            _m_lTbIdInfoList.add(tbInfo);
            _m_mapTbTbIdInfoDict.put(tbInfo.getTbName(), tbInfo);

            //累加总大小
            _m_iTotalByteSize += tbInfo.getBufferSize();

            //设置需要存储
            setDirty();

            return tbInfo;
        }
    }

    /*****
     * 重新同步TbIdBlob数据
     * 定时每5分钟执行一次，确保数据不会有过大差异
     */
    public void resyncTbIdBlob()
    {
        //如果无脏数据则不存储
        if(!_m_bIsDirty)
            return ;

        _TALMySqlSafeOpDBObj<WCGDBObj> dbObj = WCGDBFactory.getDbObj(_m_dbObj.getDbTag());
        if(null == dbObj)
        {
            ALServerLog.Error("BM.resyncTbIdBlob WCGDBObj is null DBTag:" + _m_dbObj.getDbTag());
            return ;
        }

        //设置标记
        _m_bIsDirty = false;

        //注册异步任务记录Id数据
        ALAsynTaskManager.getInstance().regTask(dbObj.getThreadIdx(), () ->
            {
                ALMySqlDBConditionObj cnd = new ALMySqlDBConditionObj();
                cnd.setTablesName(_m_sDBTbName);
                cnd.addAndEquals("ID", _m_lDbId);

                ArrayList<byte[]> byteList = new ArrayList<>();
                byteList.add(_remakeBuffer());

                //执行update
                int count = ALMySqlDBExcutor.updateByCondition(dbObj.getDB(), cnd, "IdBlob=? ", byteList);
                if(count <= 0)
                {
                    ALServerLog.Fatal("Update TbIdInfo Fail!!!");
                }
            });
    }

    /****
     * 重新生成一个Buffer用于写入数据
     * @return
     */
    private byte[] _remakeBuffer()
    {
        //此处需要加锁，避免队列访问错误
        synchronized (this)
        {
            ALProtocolBuf makeBuffer = ALProtocolBuf.allocate(_m_iTotalByteSize);

            //插入总数量
            makeBuffer.putShort((short)_m_lTbIdInfoList.size());
            //逐个插入数据
            for(int i = 0; i < _m_lTbIdInfoList.size(); i++)
            {
                _m_lTbIdInfoList.get(i).pushToBuffer(makeBuffer);
            }

            return makeBuffer.exportBuffer();
        }
    }
}
