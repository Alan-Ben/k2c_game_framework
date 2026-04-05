using Common.NpPlayerInfoObj;
using JetBrains.Annotations;
using NPEnum;
using GS2GC.p002_InitOp;
using System;
using System.Collections.Generic;
using System.Linq;
using Common.PlayerEnum;
using Common.PlayerObj;
using UnityEngine;
using GS2GC.p004_PlayerOp;

namespace GOE
{
    public class PlayerEventRecordComponent : _ANPBasicPlayerComponent
    { 
        /// <summary> 是否必须初始化 </summary>
        public override bool isMustInit { get { return true; } }
        /// <summary> 组件类型</summary>
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.EVENT_RECORD; } }
        /// <summary>依赖的组件 </summary>
        public override ENPPlayerCompType[] dependCompList { get { return null; } }

        //成员变量
        [NotNull] private Dictionary<EPlayerEventRecordType, List<Player_EventRecordInfo>> _m_lEventRecordDic;//记录列表

        public PlayerEventRecordComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_lEventRecordDic = new Dictionary<EPlayerEventRecordType, List<Player_EventRecordInfo>>();
        }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        protected override void _dealInit()
        {

        }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            //请求玩家行为记录初始化数据
            reqEventRecordList();
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {

        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {

        }

        //释放资源函数
        protected override void _discard()
        {
            _clear();
        }

        //获取指定类型的记录的值
        public long getValue(EPlayerEventRecordType _type, long _subId)
        {
            // subid = -1表示获取当前枚举所有的个数
            List<Player_EventRecordInfo> list = null;
            _m_lEventRecordDic.TryGetValue(_type, out list);
            long count = 0;
            if (null == list)
                return count;

            Player_EventRecordInfo temp = null;
            for (int i = 0; i < list.Count; i++)
            {
                temp = list[i];
                if (null == temp)
                    continue;

                if (_subId == -1)
                {
                    count += temp.getCount();
                }else if (temp.getSubId() == _subId)
                {
                    count = temp.getCount();
                    break;
                }
            }

            return count;
        }

        /// <summary>
        /// 更新或者添加值
        /// </summary>
        private void _updateValue(Player_EventRecordInfo _eventRecord)
        {
            if (null == _eventRecord)
                return;

            _addEventRecord(_eventRecord,true);
        }

        /// <summary>
        /// 数据清空
        /// </summary>
        private void _clear()
        {
            _m_lEventRecordDic.Clear();
        }

        #region S2C

        /// <summary>
        /// 初始化玩家行为记录列表
        /// </summary>
        public void retEventRecordList(GS2GC_002_043_RetEventRecordInit _msg)
        {
            if (_msg == null || _msg.getRecordList() == null)
                return;

            _m_lEventRecordDic.Clear();
            Player_EventRecordInfo temp = null;
            for (int i = 0; i < _msg.getRecordList().Count; i++)
            {
                temp = _msg.getRecordList()[i];
                if (null == temp)
                    continue;

                _addEventRecord(temp,false);
            }

            //设置组件初始化完成
            setInitDone();
        }

        private void _addEventRecord(Player_EventRecordInfo _record,bool _sendMsg)
        {
            if (_record == null)
                return;

            List<Player_EventRecordInfo> list = null;
            if (!_m_lEventRecordDic.TryGetValue(_record.getType(), out list) || list == null)
            {
                list = new List<Player_EventRecordInfo>();
                _m_lEventRecordDic[_record.getType()] = list;
            }

            bool isContain = false;
            if(list.Count > 0)
            {
                Player_EventRecordInfo temp = null;
                for (int i = 0; i < list.Count; i++)
                {
                    temp = list[i];
                    if (null == temp)
                        continue;

                    if(temp.getSubId() == _record.getSubId()){
                        isContain = true;
                        if(temp.getCount() != _record.getCount())
                        {
                            temp.setCount(_record.getCount());
                            if (_sendMsg)
                                WinMsg.SendMsg(WinMsgType.PLAYER_EVENT_RECORD_CHG);
                        }
                    }
                }
            }
            if (!isContain)
            {
                list.Add(_record);
                if (_sendMsg)
                    WinMsg.SendMsg(WinMsgType.PLAYER_EVENT_RECORD_CHG);
            }
        }
        /// <summary>
        /// 玩家记录变更推送
        /// </summary>
        public void onPlayerEventRecordUpdated(GS2GC_004_065_OnEventRecordChg _msg)
        {
            if (_msg == null || _msg.getRecord() == null)
                return;

            _updateValue(_msg.getRecord());
        }
        #endregion

        #region C2S

        /// <summary>
        /// 初始化 玩家行为记录列表
        /// </summary>
        public void reqEventRecordList()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_043_ReqEventRecordInit());
        }
        #endregion

    }
}
