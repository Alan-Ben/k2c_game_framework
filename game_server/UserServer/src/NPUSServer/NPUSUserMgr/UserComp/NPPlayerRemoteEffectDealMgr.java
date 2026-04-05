package NPUSServer.NPUSUserMgr.UserComp;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.NPLogDB.CommLogDB;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.RefRemoteEffect;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.EffectDealer.NPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;
import USLOGDB.Bo.LogRemoteEffectUseBO;

import java.util.ArrayList;

/************
 * 玩家的远程请求处理对象
 * @author Administrator
 *
 */
public class NPPlayerRemoteEffectDealMgr
{
    //每次效果的信息
    protected class NPUserRemoteEffectInfo
    {
        public long clientSerialize;
        public RefRemoteEffect effectRef;
    }

    //处理玩家效果的任务
    protected class NPUserRemoteEffectDealTask implements _IALSynTask
    {
        private NPPlayerRemoteEffectDealMgr _m_edmEffectDealMgr;

        public NPUserRemoteEffectDealTask(NPPlayerRemoteEffectDealMgr _mgr)
        {
            _m_edmEffectDealMgr = _mgr;
        }

        @Override
        public void run()
        {
            if (_m_edmEffectDealMgr._readDealEffect())
                ALSynTaskManager.getInstance().regTask(this);
        }
    }

    //玩家数据对象
    private NPUSUserData _m_udUserData;
    //待处理的数据队列
    private ArrayList<NPUserRemoteEffectInfo> _m_lNeedDealEffectIdList;

    private MutexAtom _m_mutex;

    public NPPlayerRemoteEffectDealMgr(NPUSUserData _userData)
    {
        _m_udUserData = _userData;
        _m_lNeedDealEffectIdList = new ArrayList<NPUserRemoteEffectInfo>();

        _m_mutex = new MutexAtom();
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    /**
     * 处理远程效果
     */
    public void tryDealRemoteEffect(long _clientSerialize, long _effectId)
    {
        RefRemoteEffect effectObj = RefRemoteEffect.getMgr().get(_effectId);
        if (null == effectObj)
        {
            //返回错误消息
            _m_udUserData.pushMsgToGC(US2GCWriter_007_CommOp.make_001_RetDealRemoteEffect(CommErr.REF_NOT_FOUND.getCode(), _clientSerialize, _effectId));
            return;
        }

        boolean needAddTask = false;
        //加入队列
        _lock();

        try
        {
            needAddTask = _m_lNeedDealEffectIdList.isEmpty();

            NPUserRemoteEffectInfo info = new NPUserRemoteEffectInfo();
            info.clientSerialize = _clientSerialize;
            info.effectRef = effectObj;

            _m_lNeedDealEffectIdList.add(info);
        } finally
        {
            _unlock();
        }

        //开启任务处理
        if (needAddTask)
            ALSynTaskManager.getInstance().regTask(new NPUserRemoteEffectDealTask(this));
    }

    /**
     * 实际处理任务的效果，返回是否继续
     */
    protected boolean _readDealEffect()
    {
        NPUserRemoteEffectInfo effect = null;
        //加入队列
        _lock();

        try
        {
            if (_m_lNeedDealEffectIdList.isEmpty())
                return false;

            effect = _m_lNeedDealEffectIdList.get(0);
        } finally
        {
            _unlock();
        }

        try
        {
            do
            {
                //检查条件
                if (!NPPlayerConditionDealerMgr.IsEnable(effect.effectRef.condition, _m_udUserData, null))
                {
                    //返回失败
                    _m_udUserData.pushMsgToGC(US2GCWriter_007_CommOp.make_001_RetDealRemoteEffect(CommErr.CONDITION_NOT_ENABLE.getCode(), effect.clientSerialize, effect.effectRef.id));
                    break;
                }

                NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.REMOTE_EFFECT);

                //处理事务效果
                _m_udUserData.lockUser();
                try
                {
                    //检查物品列表
                    if (!_m_udUserData.hasCostItemList(effect.effectRef.cost_item_list.getItemTypeObjList()))
                    {
                        //返回失败
                        _m_udUserData.pushMsgToGC(US2GCWriter_007_CommOp.make_001_RetDealRemoteEffect(CommErr.ITEM_NOT_ENOUGH.getCode(), effect.clientSerialize, effect.effectRef.id));
                        break;
                    }

                    //消耗物品
                    if (!_m_udUserData.spendItem(effect.effectRef.cost_item_list.getItemTypeObjList(), context))
                    {
                        //返回失败
                        _m_udUserData.pushMsgToGC(US2GCWriter_007_CommOp.make_001_RetDealRemoteEffect(CommErr.CONSUME_FAIL.getCode(), effect.clientSerialize, effect.effectRef.id));
                        break;
                    }

                    //执行效果
                    NPPlayerEffectDealer.dealEffect(effect.effectRef.effect_list.getPlayerEffectList(), _m_udUserData, null, context);

                    //发送成功协议
                    _m_udUserData.pushMsgToGC(US2GCWriter_007_CommOp.make_001_RetDealRemoteEffect(Result.SUCC.getCode(), effect.clientSerialize, effect.effectRef.id));

                    //物品展示协议，只有在不屏蔽通知的情况下才通知
                    if (!effect.effectRef.ignore_notice_bonus && !context.getCollector().isEmpty())
                    {
                        _m_udUserData.pushMsgToGC(context.getCollector().toProto());
                    }
                } finally
                {
                    _m_udUserData.unlockUser();
                }

                //记录RemoteEffect使用日志
                _logRemoteEffectUse(effect, context);
            }
            while (false);
        } catch (Exception _ex)
        {
            _ex.printStackTrace();
            //返回失败
            _m_udUserData.pushMsgToGC(US2GCWriter_007_CommOp.make_001_RetDealRemoteEffect(CommErr.SYS_ERR.getCode(), effect.clientSerialize, effect.effectRef.id));
        }
        //从队列移除，并判断是否开启任务
        //加入队列
        _lock();

        try
        {
            NPUserRemoteEffectInfo delInfo = _m_lNeedDealEffectIdList.remove(0);
            if (delInfo != effect)
                _m_lNeedDealEffectIdList.add(0, delInfo);

            return !_m_lNeedDealEffectIdList.isEmpty();
        } finally
        {
            _unlock();
        }
    }

    /**
     * 记录RemoteEffect使用日志
     * @param _effect   远程效果信息
     * @param _context  事件上下文
     */
    private void _logRemoteEffectUse(NPUserRemoteEffectInfo _effect, NPPlayerContext _context)
    {
        BM bmObj = _m_udUserData.getUSServer().getBM();

        LogRemoteEffectUseBO logBo = new LogRemoteEffectUseBO();
        logBo.setCid(bmObj, _m_udUserData.getCid());
        logBo.setRemoteEffectId(bmObj, _effect.effectRef.id);
        logBo.setClientSerialize(bmObj, _effect.clientSerialize);

        CommLogDB.log(bmObj, logBo, _context);
    }
}
