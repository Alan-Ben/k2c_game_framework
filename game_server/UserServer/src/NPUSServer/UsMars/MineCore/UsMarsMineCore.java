package NPUSServer.UsMars.MineCore;

import ALBasicCommon.ALSerializeMaker;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.CrossDataType.ECrossDataType;
import Common.CrossDataType.MarsMineInfo;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Mars.RefMarsExploreMine;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.CrossDataBasicPack._ATCrossDataMgr;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import NPUSServer.UsMars.MineCore.SynTask.SynUsMarsMineCoreTickTask;
import NPUSServer.UsMars.UsMarsAction.UsMineAction.UsMarsMineAttackAction;
import USDB.Bo.UsMarsMineBO;

import java.util.ArrayList;
import java.util.List;

/**
 * US上管理所有矿数据的管理器
 */
public class UsMarsMineCore extends _ATCrossDataMgr<MarsMineInfo, UsMarsMineObj>
{
    //US服务器对象
    private NPUserServer _m_usUSServer;

    //矿的分等级管理数据，底层做了通过Id的映射，因此此处不做处理
    private ArrayList<UsMarsMineLvlMgr> _m_lMineLvlMgr;

    //tick操作序列号
    private long _m_lTickSerialize;

    public UsMarsMineCore(NPUserServer _usServer)
    {
        //先暂定一个临时数据类型值，后续需要根据实际情况规划
        super(_usServer, ECrossDataType.MARS_MINE.ordinal());

        _m_usUSServer = _usServer;

        _m_lMineLvlMgr = new ArrayList<UsMarsMineLvlMgr>();

        _m_lTickSerialize = ALSerializeMaker.makeNewSerialize();
    }

    public NPUserServer getUSServer() {return _m_usUSServer;}
    public BM getBM() {return _m_usUSServer.getBM();}

    /**
     * 服务器初始化，需要将所有矿都初始化出来
     * 矿放入各自等级管理器，生命周期管理由管理器负责通过任务调用总对象关闭矿
     * @return
     */
    public boolean s_init()
    {
        //获取所有本地火星矿数据
        List<UsMarsMineBO> mineBoList = getUSServer().getBM().getBM(UsMarsMineBO.class).s_findAll();
        if(null == mineBoList)
        {
            USLog.error(_m_usUSServer, "MarsMinePool.init UsMarsMineBO fail.");
            return false;
        }

        for(int i = 0; i < mineBoList.size(); i++)
        {
            UsMarsMineBO bo = mineBoList.get(i);
            if(null == bo)
                continue;

            //创建数据对象
            UsMarsMineObj info = new UsMarsMineObj(this, bo);
            //所有数据对象都放入本管理器
            addData(info);

            //获取等级管理对象
            UsMarsMineLvlMgr lvlMgr = _ensureMineLvlMgr(info.getRef());
            if(null == lvlMgr)
                continue;

            //加入管理器中
            lvlMgr._initMineObj(info);
        }

        //初始化完成之后，针对每个等级管理器都做排序
        for(UsMarsMineLvlMgr lvlMgr : _m_lMineLvlMgr)
        {
            if(null == lvlMgr)
                continue;

            lvlMgr._refreshMineOrder();
        }

        //开启Tick任务
        startTickTask();

        return true;
    }

    /**
     * 开启Tick任务处理
     */
    public void startTickTask()
    {
        _lock();

        try {
            //刷新序列号
            _m_lTickSerialize = ALSerializeMaker.makeNewSerialize();

            //开启任务处理
            ALSynTaskManager.getInstance().regTask(new SynUsMarsMineCoreTickTask(this, _m_lTickSerialize), 1000);
        }
        finally {
            _unlock();
        }
    }

    /**
     * 取出一个对应等级的已有矿
     * 这个接口是个常规接口
     * 如对应等级的矿数量不足，则仍然会在管理器中创建一个新矿
     * @param _mineRef
     * @return
     */
    public UsMarsMineObj popExistMine(RefMarsExploreMine _mineRef)
    {
        //获取管理器对象
        UsMarsMineLvlMgr lvlMgr = _ensureMineLvlMgr(_mineRef);
        if(null == lvlMgr)
            return null;

        //此时从管理器获取已有矿数据，如返回为空，则创建新矿
        UsMarsMineObj mineObj = lvlMgr.tryPopRandExistMine();
        if(null != mineObj)
            return mineObj;

        //此时直接创建一个新矿并返回
        UsMarsMineObj newMine = null;
        _lock();

        try
        {
            UsMarsMineBO bo = new UsMarsMineBO();
            bo.setRefId(getBM(), _mineRef.id);
            bo.setEndShowMs(getBM(), CommonFunc.getNowTimeMS() + RefGeneral.Ref().mars_mine_secs * 1000);
            bo.setRemainNum(getBM(), _mineRef.res_num);

            //插入数据库
            bo.insert(getBM());

            //构造数据对象
            newMine = new UsMarsMineObj(this, bo);

            //插入数据
            addData(newMine);
        }
        finally {
            _unlock();
        }

        //放入等级管理器，此处放在锁外侧是避免锁冲突
        //可以暂时使用init加入方式，如果后续逻辑不同需要写新函数
        lvlMgr._initMineObj(newMine);

        return newMine;
    }


    /**
     * 直接创建一个新矿
     * @param _mineRef
     * @return
     */
    public UsMarsMineObj popNewMine(RefMarsExploreMine _mineRef)
    {
        //获取管理器对象
        UsMarsMineLvlMgr lvlMgr = _ensureMineLvlMgr(_mineRef);
        if(null == lvlMgr)
            return null;

        //判断等级矿是否超过上限，目前不做上限判断，避免不准确的判断影响生态
//        if(lvlMgr.getMineCount() > RefGeneral.Ref().mars_mine_num_max)
//        {
//            //超过上限则直接返回旧矿
//            UsMarsMineObj mineObj = lvlMgr.tryPopRandExistMine();
//            if(null != mineObj)
//                return mineObj;
//        }

        //此时直接创建一个新矿并返回
        UsMarsMineObj newMine = null;
        _lock();

        try
        {
            UsMarsMineBO bo = new UsMarsMineBO();
            bo.setRefId(getBM(), _mineRef.id);
            bo.setEndShowMs(getBM(), CommonFunc.getNowTimeMS() + RefGeneral.Ref().mars_mine_secs * 1000);
            bo.setRemainNum(getBM(), _mineRef.res_num);

            //插入数据库
            bo.insert(getBM());

            //构造数据对象
            newMine = new UsMarsMineObj(this, bo);

            //插入数据
            addData(newMine);
        }
        finally {
            _unlock();
        }

        //放入等级管理器，此处放在锁外侧是避免锁冲突
        //可以暂时使用init加入方式，如果后续逻辑不同需要写新函数
        lvlMgr._initMineObj(newMine);

        return newMine;
    }

    /**
     * 结算已经结束的矿数据
     * 此操作只处理总数据集，等级管理器中的操作不做处理。
     * @param _mineDataId
     */
    public void settleEndMine(long _mineDataId)
    {
        UsMarsMineObj mineObj = lookupData(_mineDataId);
        if(null == mineObj)
            return ;

        //判断是否已经结束，如非结束状态则不处理
        if(!mineObj.isDisable())
            return ;

        //移除矿数据
        rmvData(_mineDataId);

        //进行结束结算，结束结算会删除数据Bo
        mineObj._settleEnd();
    }

    /**
     * 移除超时空矿，此超时函数仅由LvlMgr的监控任务调用，因此这里不对LvlMgr内做处理
     * @param _mineDataId
     */
    public void rmvEmptyOutOfDateMine(long _mineDataId)
    {
        UsMarsMineObj mineObj = lookupData(_mineDataId);
        if(null == mineObj)
            return ;

        //判断是否已经超时，如未超时则不处理
        if(!mineObj.isOutofDate())
            return ;

        //移除矿数据
        rmvData(_mineDataId);

        //判断是否有队伍在采集，如有队伍采集则直接遣返处理
        //多线程情况下可能触发了删除矿但是又被占领的极端情况，此时直接遣返
        if(mineObj.getTeamId() > 0)
        {
            mineObj._forceClear();
        }
    }

    /**
     * 尝试占领对应矿,占领行为需要通过LvlMgr将矿放入监控队列
     * @param _attackAction
     */
    public void occupy(UsMarsMineAttackAction _attackAction)
    {
        //获取矿数据
        UsMarsMineObj mineObj = lookupData(_attackAction.getMineInstanceId());
        if(null == mineObj)
        {
            //无数据直接遣返
            _attackAction.teamBack();
            return ;
        }

        //调用发起占领的处理，队伍在函数中会处理，这里如果失败直接返回，不需要放入监控队列
        if(!mineObj._occupy(_attackAction))
        {
            return ;
        }

        //尝试将矿放入对应Lvl管理器的监控队列
        UsMarsMineLvlMgr lvlMgr = _ensureMineLvlMgr(mineObj.getRef());
        lvlMgr._tryMonitorMine(mineObj);
    }

    /**
     * 离开矿的处理操作
     * @param _dataId
     * @param _cid
     * @param _teamId
     */
    public Result leaveMine(long _dataId, long _cid, long _teamId)
    {
        //获取矿数据
        UsMarsMineObj mineObj = lookupData(_dataId);
        if(null == mineObj)
        {
            return MarsErr.MARS_MINE_NOT_FOUND;
        }

        return mineObj._settleByLeave(_cid, _teamId);
    }

    /**
     * 定时检测本管理器内的相关数据处理，此处主要做的事LvlMgr的遍历处理
     * 需要注意锁冲突，遍历前需要拷贝队列，然后再做处理
     * @param _nowTimeMS
     */
    public void tick(long _tickSerialize, long _nowTimeMS)
    {
        ArrayList<UsMarsMineLvlMgr> tmpList;
        //在锁内赋值。而队列每次都是创建，因此不需要顾虑队列锁问题
        _lock();

        try
        {
            //序列号不一致则不处理
            if(_tickSerialize != _m_lTickSerialize)
                return ;

            tmpList = _m_lMineLvlMgr;
        }
        finally {
            _unlock();
        }

        //遍历每个等级管理器进行tick处理
        for(UsMarsMineLvlMgr lvlMgr : tmpList)
        {
            //进行每个LvlMgr的tick处理
            lvlMgr._tick(_nowTimeMS);
        }
    }

    /**
     * 获取或创建一个不同等级的矿管理器对象
     * @param _mineRef
     * @return
     */
    protected UsMarsMineLvlMgr _ensureMineLvlMgr(RefMarsExploreMine _mineRef)
    {
        if(null == _mineRef)
            return null;

        _lock();

        try {
            //先检索已有数据，如无数据则创建新数据
            for (UsMarsMineLvlMgr lvlMgr : _m_lMineLvlMgr) {
                if (null == lvlMgr)
                    continue;

                if (lvlMgr.getMineRef() == _mineRef)
                    return lvlMgr;
            }

            //未找打数据这里直接创建，并加入队列
            UsMarsMineLvlMgr lvlMgr = new UsMarsMineLvlMgr(this, _mineRef);
            //每次创建一个新队列，避免在遍历的时候引发锁问题
            _m_lMineLvlMgr = new ArrayList<UsMarsMineLvlMgr>(_m_lMineLvlMgr);
            _m_lMineLvlMgr.add(lvlMgr);

            return lvlMgr;
        }
        finally {
            _unlock();
        }
    }

    /************************ CrossData 重载函数 ******************/
    @Override
    protected int _getInitSyncDataPerPageCount() {
        return 200;
    }

    @Override
    protected void _onAddData_InLock(UsMarsMineObj _data) {
        //外部先存储数据，这里不做内部处理
    }

    @Override
    protected void _onRmvData_InLock(UsMarsMineObj _data) {
        //外部先存储数据，这里不做内部处理
    }
    /************************ CrossData 重载函数 end ******************/
}
