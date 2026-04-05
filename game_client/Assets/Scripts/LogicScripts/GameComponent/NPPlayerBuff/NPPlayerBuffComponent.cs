using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ALPackage;
using NPCommon;
using System.Text;
using CommonEnum;
using GS2GC.p002_InitOp;
using GS2GC.p004_PlayerOp;
using JetBrains.Annotations;

namespace GOE
{
    //管理器
    public class NPPlayerBuffComponent : _ANPBasicPlayerComponent
    {
        private static int _g_iSerialize = 1;

        private int _m_iDealSerialize;

        //事件
        public Action<NPPlayerBuffInfo, int, long> onChgPlayerBuff;//玩家buff变化监听
        public Action<NPPlayerBuffInfo> onRemovePlayerBuff;//玩家buff移除监听

        //属性
        public override bool isMustInit { get { return true; } }

        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.PLAYER_BUFF; } }

        public override ENPPlayerCompType[] dependCompList { get { return null; } }

        public int dealSerialize { get { return _m_iDealSerialize; } }
        //成员变量
        private List<NPPlayerBuffInfo> _m_lPlayerBuffInfoList;//所有玩家buff对象
        [NotNull] private CommonUnionBonusMgr _m_commonUnionBonusMgr = new CommonUnionBonusMgr(EUnionBonusMgrTag.PLAYER_BUFF);
        [NotNull] private NPPlayerPropertyContainer _m_pcPlayerPropertyContainer; //玩家属性容器
        [NotNull] private MarsPropertyContainer _m_marsPropertyContainer; //火星属性容器（来自buff的火星属性加成）

        //玩家属性容器
        [NotNull] public NPPlayerPropertyContainer playerPropertyContainer { get { return _m_pcPlayerPropertyContainer; } }
        [NotNull] public CommonUnionBonusMgr commonUnionBonusMgr { get { return _m_commonUnionBonusMgr; } }
        [NotNull] public MarsPropertyContainer marsPropertyContainer { get { return _m_marsPropertyContainer; } }

        //构造函数
        public NPPlayerBuffComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_iDealSerialize = _g_iSerialize++;

            _m_lPlayerBuffInfoList = new List<NPPlayerBuffInfo>();
            _m_pcPlayerPropertyContainer = new NPPlayerPropertyContainer("玩家buff系统");
            _m_marsPropertyContainer = new MarsPropertyContainer("玩家buff系统火星属性");
        }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        public int buffCount { get { return _m_lPlayerBuffInfoList.Count; } }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            //请求玩家buff信息列表
            reqInitPlayerBuffList();
        }

        protected override void _dealInit()
        {
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            _m_commonUnionBonusMgr.setParent(NPPlayer.instance.playerBonusMgr);
            _m_commonUnionBonusMgr.onPropertyChg += _onBonusChg;
            //开启周期刷新事件
            ALMonoTaskMgr.instance.addMonoTask(new NPCheckPlayerBuffExpiredTask(this));
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("NPPlayerBuffComponent init Fail!!!");
        }
        //释放资源函数
        protected override void _discard()
        {
            clear();
        }

        
        public void checkExpired()
        {
            if(null == _m_lPlayerBuffInfoList)
                return;

            // 倒序遍历检查是否超时并删除
            for (int i = _m_lPlayerBuffInfoList.Count - 1; i >= 0; i--)
            {
                NPPlayerBuffInfo info = _m_lPlayerBuffInfoList[i];
                if (null == info || info.hasExpired())
                {
                    //删除buff
                    _dealRemoveBuff(info);
                }
            }
        }

        //查找对应的玩家buff
        public NPPlayerBuffInfo lookup(long _buffId)
        {
            for(int i = 0; i < _m_lPlayerBuffInfoList.Count; i++)
            {
                NPPlayerBuffInfo info = _m_lPlayerBuffInfoList[i];
                if(null == info)
                    continue;

                if(info.buffId == _buffId)
                    return info;
            }

            return null;
        }

        public void Foreach(Action<NPPlayerBuffInfo> _do)
        {
            if(_do != null)
            {
                foreach(NPPlayerBuffInfo buffInfo in _m_lPlayerBuffInfoList)
                {
                    _do(buffInfo);
                }
            }
        }

        //析构函数
        public void clear()
        {
            _m_iDealSerialize++;

            //清空数据
            _m_lPlayerBuffInfoList.Clear();
            _m_pcPlayerPropertyContainer.clear();
            _m_marsPropertyContainer.clear();
            
            _m_commonUnionBonusMgr.clear();
            _m_commonUnionBonusMgr.onPropertyChg -= _onBonusChg;
        }

        /** 在添加Buff时的本地处理 */
        protected void _dealAddBuff(NPCommon_PlayerBuffInfo _info)
        {
            if(null == _info)
                return;

            NPPlayerBuffRefObj refObj = GRefdataCoreMgr.instance.playerBuffMap.getRef(_info.getBuffId());
            if(null == refObj)
            {
                Debug.LogError_EditorOnly("Error, NPPlayerBuffRefObj[" + _info.getBuffId() + "]  == null");
                return;
            }

            NPPlayerBuffInfo info = new NPPlayerBuffInfo(this, _info, refObj);

            _m_commonUnionBonusMgr.addBonus(info.refObj?.bonus_add?.unionBonus);
            //添加属性
            _m_pcPlayerPropertyContainer.addModifier(info.refObj.player_pro_add, info.layer);
            // 添加火星属性
            if(info.refObj != null)
                _m_marsPropertyContainer.addModifier(info.refObj.mars_pro_add, info.layer);
            //插入队列
            __addBuffToList(info);

            //修改监听
            if(null != onChgPlayerBuff)
                onChgPlayerBuff(info, info.layer, info.curLeftTimeMS);
        }

        /** 在修改Buff时的本地处理 */
        protected void _dealUpdateBuff(NPCommon_PlayerBuffInfo _info)
        {
            if(null == _info)
                return;

            //从队列移除
            NPPlayerBuffInfo tmpInfo = __removeBuffFromList(_info.getBuffId());
            if(null == tmpInfo)
            {
                //无数据则添加
                _dealAddBuff(_info);
                return;
            }

            _m_commonUnionBonusMgr.removeBonus(tmpInfo.refObj?.bonus_add?.unionBonus);
            //删除原先属性
            _m_pcPlayerPropertyContainer.removeModifier(tmpInfo.refObj.player_pro_add, tmpInfo.layer);
            // 删除原先火星属性
            if(tmpInfo.refObj != null)
                _m_marsPropertyContainer.removeModifier(tmpInfo.refObj.mars_pro_add, tmpInfo.layer);
            //更新数据
            tmpInfo.update(_info);
            //重新添加到队列
            __addBuffToList(tmpInfo);
            
            _m_commonUnionBonusMgr.addBonus(tmpInfo.refObj?.bonus_add?.unionBonus);
            //添加属性
            _m_pcPlayerPropertyContainer.addModifier(tmpInfo.refObj.player_pro_add, tmpInfo.layer);
            // 添加火星属性
            if(tmpInfo.refObj != null)
                _m_marsPropertyContainer.addModifier(tmpInfo.refObj.mars_pro_add, tmpInfo.layer);

            //修改监听
            if(null != onChgPlayerBuff)
                onChgPlayerBuff(tmpInfo, tmpInfo.layer, tmpInfo.curLeftTimeMS);
        }

        /** 在删除Buff时的本地处理 */
        protected void _dealRemoveBuff(NPPlayerBuffInfo _buff)
        {
            if(null == _buff)
                return;

            _m_commonUnionBonusMgr.removeBonus(_buff.refObj?.bonus_add?.unionBonus);
            //移除属性
            _m_pcPlayerPropertyContainer.removeModifier(_buff.refObj.player_pro_add, _buff.layer);
            // 移除火星属性
            if(_buff.refObj != null)
                _m_marsPropertyContainer.removeModifier(_buff.refObj.mars_pro_add, _buff.layer);
            _m_lPlayerBuffInfoList.Remove(_buff);

            if(null != onChgPlayerBuff)
            {
                onChgPlayerBuff(_buff, 0, 0);
            }
            if(null != onRemovePlayerBuff)
            {
                onRemovePlayerBuff(_buff);
            }
        }
        protected void _dealRemoveBuff(long _buffId)
        {
            //从队列移除
            NPPlayerBuffInfo tmpInfo = __removeBuffFromList(_buffId);
            if(null == tmpInfo)
                return;

            _m_commonUnionBonusMgr.addBonus(tmpInfo.refObj?.bonus_add?.unionBonus);
            //移除属性
            _m_pcPlayerPropertyContainer.removeModifier(tmpInfo.refObj.player_pro_add, tmpInfo.layer);
            // 移除火星属性
            if(tmpInfo.refObj != null)
                _m_marsPropertyContainer.removeModifier(tmpInfo.refObj.mars_pro_add, tmpInfo.layer);

            if(null != onChgPlayerBuff)
            {
                onChgPlayerBuff(tmpInfo, 0, 0);
            }
            if(null != onRemovePlayerBuff)
            {
                onRemovePlayerBuff(tmpInfo);
            }
        }

        /** 将buff添加到队列中 */
        private void __addBuffToList(NPPlayerBuffInfo _buff)
        {
            if(null == _buff)
                return;

            //buff剩余时间
            long newTimeLeftMS = _buff.curLeftTimeMS;

            NPPlayerBuffInfo tmpBuf = null;
            for(int i = 0; i < _m_lPlayerBuffInfoList.Count; i++)
            {
                tmpBuf = _m_lPlayerBuffInfoList[i];
                if(null == tmpBuf)
                    continue;

                long timeLeftMs = tmpBuf.curLeftTimeMS;
                //无限长则直接加在这个签前面
                if(-1 == timeLeftMs)
                {
                    _m_lPlayerBuffInfoList.Insert(i, _buff);
                    return;
                }

                //判断是否比当前buff剩余时间长，是则插入前面
                if(timeLeftMs >= newTimeLeftMS)
                {
                    _m_lPlayerBuffInfoList.Insert(i, _buff);
                    return;
                }
            }

            //加入末尾
            _m_lPlayerBuffInfoList.Add(_buff);
        }
        /** 将buff从队列移除并返回 */
        private NPPlayerBuffInfo __removeBuffFromList(long _buffId)
        {
            NPPlayerBuffInfo tmpBuf = null;
            for(int i = 0; i < _m_lPlayerBuffInfoList.Count; i++)
            {
                tmpBuf = _m_lPlayerBuffInfoList[i];
                if(null == tmpBuf)
                    continue;

                if(tmpBuf.refObj.id == _buffId)
                {
                    _m_lPlayerBuffInfoList.RemoveAt(i);
                    return tmpBuf;
                }
            }

            return null;
        }
       
        /// <summary>
        /// 玩家全局属性加成变更
        /// </summary>
        /// <param name="_type"></param>
        private void _onBonusChg(EBonusPropertyType _type)
        {
            
        }
        
        
        #region 消息
        //请求列表
        private void reqInitPlayerBuffList()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_008_ReqPlayerBuffList());
        }
        public void retPlayerBuffList(GS2GC_002_008_RetPlayerBuffList _msg)
        {
            dealPreInitFunc(() =>
            {
                //清理旧数据
                clear();

                NPCommon_PlayerBuffInfo tmpInfo = null;
                //NPPlayerBuffRefObj tmpObj = null;
                List<NPCommon_PlayerBuffInfo> infoList = _msg.getPlayerBuffList();
                for(int i = 0; i < infoList.Count; i++)
                {
                    tmpInfo = infoList[i];
                    if(null == tmpInfo)
                        continue;

                    //处理增加buff
                    _dealAddBuff(tmpInfo);
                }

                //设置加载完成
                setInitDone();
            });
        }

        /// <summary>
        /// 玩家buff变化通知
        /// </summary>
        /// <param name="_msg">消息参数</param>
        public void onPlayerBuffChg(GS2GC_004_053_OnPlayerBuffChg _msg)
        {
            if (_msg == null)
                return;

            // 处理buff变化
            _dealUpdateBuff(_msg.getBuff());
            GCommon.reloadCustomLoadPrefab();
        }

        /// <summary>
        /// 玩家buff移除通知
        /// </summary>
        /// <param name="_msg">消息参数</param>
        public void onPlayerBuffRemove(GS2GC_004_052_OnPlayerBuffRemove _msg)
        {
            if (_msg == null)
                return;

            // 处理buff移除
            _dealRemoveBuff(_msg.getBuffId());
            GCommon.reloadCustomLoadPrefab();
        }

        /// <summary>
        /// 玩家buff触发通知
        /// </summary>
        /// <param name="_msg">消息参数</param>
        public void onPlayerBuffTrigger(GS2GC_004_061_OnPlayerBuffTrigger _msg)
        {
            if (_msg == null)
                return;

            // 查找对应的buff
            NPPlayerBuffRefObj refObj = GRefdataCoreMgr.instance.playerBuffMap.getRef(_msg.getBuffId());

            if (refObj != null)
            {
                NPGUIAddSceneCenterTip.instance.showTextTip(TextTranslate.instance.getLanguage(refObj.name), refObj.trigger_tip_ui_res_id);
                // 发送消息通知UI更新
                WinMsg.SendMsg(WinMsgType.ON_PLAYER_BUFF_TRIGGER, _msg.getBuffId());
            }
            GCommon.reloadCustomLoadPrefab();
        }

        #endregion
    }

    //处理buff的任务
    public class NPCheckPlayerBuffExpiredTask : _IALBaseMonoTask
    {
        private NPPlayerBuffComponent _m_bcBuffComp;
        private int _m_iDealSerialize;

        public NPCheckPlayerBuffExpiredTask(NPPlayerBuffComponent _buffComp)
        {
            _m_bcBuffComp = _buffComp;
            _m_iDealSerialize = _m_bcBuffComp.dealSerialize;
        }

        public void deal()
        {
            if(_m_iDealSerialize != _m_bcBuffComp.dealSerialize)
                return;

            //检测
            _m_bcBuffComp.checkExpired();

            //下次周期事件
            ALMonoTaskMgr.instance.addMonoTask(this, 1f);
        }
    }
}
