package NPUSServer.MarsGoRouteMsgMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.MarsObj.Mars_GoRoute_StageMsg;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.MarsGoRouteStageMsgBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

/**
 * 火星前往路线-阶段留言管理器（服务器级别）
 *
 * 功能：
 * 1. 服务器启动时从 DB 同步加载各阶段最新20条留言到内存
 * 2. 新留言到来时写入 DB 并更新内存缓存
 * 3. 超出20条时删除最旧一条（DB + 内存同步裁剪）
 *
 * 线程安全：内存操作通过 MutexAtom 保护，DB 操作在锁外执行
 */
public class MarsGoRouteMsgMgr
{
    // 每个阶段最多保留的留言条数
    private static final int MAX_MSG_PER_STAGE = 20;

    private final NPUserServer _m_server;
    private final MutexAtom _m_mutex = new MutexAtom();

    // 各阶段留言列表：阶段号 -> 留言列表（按时间从旧到新）
    private final HashMap<Integer, List<MarsGoRouteMsgInfo>> _m_stageMsgMap = new HashMap<>();

    public MarsGoRouteMsgMgr(NPUserServer _server)
    {
        _m_server = _server;
    }

    private BM getBM() {return _m_server.getBM();}

    private void _lock() {_m_mutex.lock();}

    private void _unlock() {_m_mutex.unlock();}

    /**
     * 服务器启动时同步加载 DB 数据到内存
     * 使用 s_findAll 同步查询，每阶段只保留 id 最大的20条
     */
    public boolean init()
    {
        List<MarsGoRouteStageMsgBO> boList = getBM().getBM(MarsGoRouteStageMsgBO.class).s_findAll();
        if (boList == null)
        {
            USLog.error(_m_server, "MarsGoRouteMsgMgr.init - s_findAll failed");
            return false;
        }

        // DB 中记录按 id 升序，直接遍历保持时序
        for (MarsGoRouteStageMsgBO bo : boList)
        {
            List<MarsGoRouteMsgInfo> list = _m_stageMsgMap.computeIfAbsent(bo.getStage(), k -> new ArrayList<>());
            list.add(new MarsGoRouteMsgInfo(bo));
        }

        return true;
    }

    /**
     * 添加阶段留言并持久化到 DB
     * MutexAtom 为可重入锁，_checkDeleteMsg 在锁内直接调用
     *
     * @param _stage      阶段号
     * @param _cid        发送留言的玩家CID
     * @param _playerName 玩家名称
     * @param _content    留言内容
     */
    public void addMsg(int _stage, long _cid, String _playerName, String _content)
    {
        _lock();
        try
        {
            List<MarsGoRouteMsgInfo> list = _m_stageMsgMap.computeIfAbsent(_stage, k -> new ArrayList<>());

            // 裁剪超出上限的旧记录（含 DB 删除）
            _checkDeleteMsg(list);

            // 写入 DB，获取自增 ID
            MarsGoRouteStageMsgBO bo = new MarsGoRouteStageMsgBO();
            bo.setCid(getBM(), _cid);
            bo.setStage(getBM(), _stage);
            bo.setPlayerName(getBM(), _playerName);
            bo.setContent(getBM(), _content);
            bo.setCreatedMs(getBM(), CommonFunc.getNowTimeMS());
            bo.insert(getBM());

            // 写入内存
            MarsGoRouteMsgInfo newInfo = new MarsGoRouteMsgInfo(bo);
            list.add(newInfo);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 裁剪列表至上限以下，同步删除 DB 中的旧记录
     */
    private void _checkDeleteMsg(List<MarsGoRouteMsgInfo> list)
    {
        _lock();
        try{
            if (list.size() < MAX_MSG_PER_STAGE)
                return;

            List<Long> toDelete = new ArrayList<>();
            while (list.size() >= MAX_MSG_PER_STAGE)
                toDelete.add(list.remove(0).getDbId());

            getBM().getBM(MarsGoRouteStageMsgBO.class).delAllInList("id", toDelete);
        }finally
        {
            _unlock();
        }
    }

    /**
     * 获取指定阶段的留言协议列表（快照）
     *
     * @param _stage 阶段号
     * @return 留言协议列表，无留言则返回空列表
     */
    public List<Mars_GoRoute_StageMsg> getStageMsgProtoList(int _stage)
    {
        List<Mars_GoRoute_StageMsg> protoList = new ArrayList<>();
        _lock();
        try
        {
            List<MarsGoRouteMsgInfo> list = _m_stageMsgMap.get(_stage);
            if (list != null)
            {
                for (MarsGoRouteMsgInfo info : list)
                    protoList.add(info.toProto());
            }
        }
        finally
        {
            _unlock();
        }
        return protoList;
    }
}
