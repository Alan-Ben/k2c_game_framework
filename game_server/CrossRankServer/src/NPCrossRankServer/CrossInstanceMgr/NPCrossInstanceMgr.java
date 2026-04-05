package NPCrossRankServer.CrossInstanceMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALProcess._IALProcessAction;
import ALServerLog.ALServerLog;
import CRSDB.Bo.NpCrossInstanceBO;
import NPCommon.DB.BM.BM;
import NPCrossRankServer.GeneralV.CrsID;
import NPCrossRankServer.NPCrossRankServer;

import java.util.ArrayList;
import java.util.Hashtable;
import java.util.List;

/**
 * 跨服排行榜分组管理器
 * 这个管理器和排行榜基类分开独立处理
 * 排行榜作为全部排行榜的基本数据处理器存在
 * 
 * 分组逻辑在这个管理器中进行处理
 * @author mark
 */
public class NPCrossInstanceMgr
{
    private static NPCrossInstanceMgr _g_instance = new NPCrossInstanceMgr();
    public static NPCrossInstanceMgr getInstance()
    {
        return _g_instance;
    }
    
    //参与这个排行榜分组的所有参与对象
    private Hashtable<Long, NPCrossInstanceInfo> _m_htCrossInstanceTable;
    
    //锁对象，保护UsList
    private MutexAtom _m_mutex;

    public NPCrossInstanceMgr()
    {
        _m_htCrossInstanceTable = new Hashtable<Long, NPCrossInstanceInfo>();
        
        _m_mutex = new MutexAtom();
    }
    
    protected void _lock() {_m_mutex.lock();}
    protected void _unlock() {_m_mutex.unlock();}

    /******************
     * 从数据库初始化已经创建的跨服实例数据
     * @return
     */
    public boolean initFromDB()
    {
        //排行榜数据
        List<NpCrossInstanceBO> boList = NPCrossRankServer.getInstance().getBM().getBM(NpCrossInstanceBO.class).s_findAll();
        if (null == boList)
        {
            return false;
        }

        //需要后续直接关闭的排行列表数据集
        ArrayList<NPCrossInstanceInfo> needCloseRankList = new ArrayList<NPCrossInstanceInfo>();
        //逐个创建实例对象
        for (int i = 0; i < boList.size(); i++)
        {
            NpCrossInstanceBO bo = boList.get(i);
            if (null == bo)
                continue;

            NPCrossInstanceInfo crossInstance = new NPCrossInstanceInfo(bo);

            _m_htCrossInstanceTable.put(crossInstance.getInstanceId(), crossInstance);

            //如果排行无效，则直接加入需要关闭列表
            if(!crossInstance.isEnable())
            {
                needCloseRankList.add(crossInstance);
            }
        }

        //关闭需要关闭的集合，避免部分数据无法放入排行榜而在数据库驻留
        for(int i = 0; i < needCloseRankList.size(); i++)
        {
            discardCrossInstance(needCloseRankList.get(i).getInstanceId());
        }
        needCloseRankList.clear();

        return true;
    }
    
    /*****************
     * 确保找到某个排行榜集合
     * 注意：提供这个接口主要是避免初始化因为数据库写入问题导致集合未创建，实际逻辑还是要避免用这个
     * 
     * @param _crossInstanceId
     * @return
     */
    public NPCrossInstanceInfo ensureCrossInstance(long _crossInstanceId)
    {
        _lock();
        
        try
        {
            NPCrossInstanceInfo instanceInfo = _m_htCrossInstanceTable.get(_crossInstanceId);
            if(null == instanceInfo)
            {
                //进入这里表示是异常情况需要报错
                ALServerLog.Fatal("Ensure Cross Instance: " + _crossInstanceId + " when cross instance is not created!");

                BM bmObj = NPCrossRankServer.getInstance().getBM();

                //创建一个新的实例对象
                NpCrossInstanceBO crossInstanceBo = new NpCrossInstanceBO();
                crossInstanceBo.setCrossInstanceId(bmObj, _crossInstanceId);
                crossInstanceBo.setNeedDiscard(bmObj, false);
                
                //插入数据库
                crossInstanceBo.insert(bmObj);
                
                instanceInfo = new NPCrossInstanceInfo(crossInstanceBo);
                //放入数据集
                _m_htCrossInstanceTable.put(_crossInstanceId, instanceInfo);
            }
            
            return instanceInfo;
        }
        finally
        {
            _unlock();
        }
    }

    /*******************
     * 单纯的查询语句
     * 
     * @param _crossInstanceId
     * @return
     */
    public NPCrossInstanceInfo lookupCrossInstance(long _crossInstanceId)
    {
        _lock();
        
        try
        {
            return _m_htCrossInstanceTable.get(_crossInstanceId);
        }
        finally
        {
            _unlock();
        }
    }

    
    /*****************
     * 创建一个跨服排行集群
     * @return
     */
    public NPCrossInstanceInfo createCrossInstance()
    {
        _lock();
        
        try
        {
            BM bmObj = NPCrossRankServer.getInstance().getBM();

            //创建一个新的实例对象
            NpCrossInstanceBO crossInstanceBo = new NpCrossInstanceBO();
            //申请跨服实例id
            crossInstanceBo.setCrossInstanceId(bmObj, CrsID.makeCrossInstanceId());
            crossInstanceBo.setNeedDiscard(bmObj, false);

            //插入数据库
            crossInstanceBo.insert(bmObj);
            
            //输出日志
            ALServerLog.Sys("Create Cross Instance: " + crossInstanceBo.getId());
            
            NPCrossInstanceInfo instanceInfo = new NPCrossInstanceInfo(crossInstanceBo);
            //放入数据集
            _m_htCrossInstanceTable.put(instanceInfo.getInstanceId(), instanceInfo);
            
            return instanceInfo;
        }
        finally
        {
            _unlock();
        }
    }

    /*********************
     * 释放关联的跨服排行实例
     * @param _instanceId 排行榜实例ID
     */
    public void discardCrossInstance(long _instanceId)
    {
        _lock();
        try
        {
            NPCrossInstanceInfo crossInstanceInfo = _m_htCrossInstanceTable.remove(_instanceId);

            if (null == crossInstanceInfo)
                return;

            //输出日志
            ALServerLog.Sys("Close Cross Instance: " + _instanceId);

            //此处设置排行榜为需要删除的状态
            crossInstanceInfo.tryDiscardCrossInstance(
                    new _IALProcessAction() {
                            @Override
                            public void dealAction() { crossInstanceInfo._discardAllInfo(); }
                        });
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取所有的跨服排行集合数量
     * @return 数量
     */
    public int getAllInstanceCount()
    {
        return _m_htCrossInstanceTable.size();
    }
}
