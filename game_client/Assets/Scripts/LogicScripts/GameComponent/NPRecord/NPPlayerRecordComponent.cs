using Common.NpPlayerInfoObj;
using NPEnum;
using GS2GC.p002_InitOp;
using GS2GC.p004_PlayerOp;
using System;
using System.Linq;
using UnityEngine;

namespace GOE
{
    public class NPPlayerRecordComponent : _ANPBasicPlayerComponent
    {
        public Action<ENPPlayerRecordParam, long, long> onRecordValueChg
        {
            get { return _m_aOnRecordValueChg; }
            set { _m_aOnRecordValueChg = value; }
        }
        /// <summary> 是否必须初始化 </summary>
        public override bool isMustInit { get { return true; } }
        /// <summary> 组件类型</summary>
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.RECORD; } }
        /// <summary>依赖的组件 </summary>
        public override ENPPlayerCompType[] dependCompList { get { return null; } }

        private static readonly int g_RecordTypeCount = Enum.GetValues(typeof(ENPPlayerRecordParam)).Length;
        //成员变量
        private long[] _m_lRecordList = new long[g_RecordTypeCount];//记录列表
        private Action<ENPPlayerRecordParam, long, long> _m_aOnRecordValueChg;//当前记录值变化的事件Action<enum,srcValue, destValue>

        public NPPlayerRecordComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {

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
            //请求玩家记录初始化数据
            reqRecordList();
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            //初始化所有属性
            for (int i = 0; i < g_RecordTypeCount; i++)
            {
                _m_lRecordList[i] = 0;
            }
            _m_aOnRecordValueChg = default(Action<ENPPlayerRecordParam, long, long>);
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
        public long getValue(ENPPlayerRecordParam _type)
        {
            int idx = (int)_type;
            if (idx >= _m_lRecordList.Length)
            {
#if UNITY_EDITOR
                Debug.LogError("数据不匹配， ENPPlayerRecordParam枚举： " + _type);
#endif
                return 0;
            }
            return _m_lRecordList[idx];
        }

        //设置值，服务器更新
        public void setValue(PlayerInfo_Record _record)
        {
            setValue((ENPPlayerRecordParam)_record.getType(), _record.getCount());
        }

        public void setValue(ENPPlayerRecordParam _type, long _value)
        {
            if ((int)_type >= _m_lRecordList.Length)
                return;

            long srcValue = _m_lRecordList[(int)_type];
            _m_lRecordList[(int)_type] = _value;

            if (_m_aOnRecordValueChg != null)
                _m_aOnRecordValueChg(_type, srcValue, _value);
        }


        /// <summary>
        /// 数据清空
        /// </summary>
        private void _clear()
        {
            for (int i = 0; i < g_RecordTypeCount; i++)
            {
                _m_lRecordList[i] = 0;
            }

            _m_aOnRecordValueChg = default(Action<ENPPlayerRecordParam, long, long>);
        }




        #region S2C

        /// <summary>
        /// 初始化玩家记录列表
        /// </summary>
        public void retRecordList(GS2GC_002_038_RetRecordInit _msg)
        {
            if (_msg == null || _msg.getRecordList() == null)
                return;

            //设置组件初始化完成
            setInitDone();

            for (int i = 0; i < _msg.getRecordList().Count; ++i)
            {
                setValue(_msg.getRecordList()[i]);
            }
        }

        /// <summary>
        /// 玩家记录变更推送
        /// </summary>
        public void onPlayerRecordUpdated(GS2GC_004_060_OnPlayerRecordChg _msg)
        {
            if (_msg == null || _msg.getRecord() == null)
                return;

            setValue(_msg.getRecord());

            //发送消息刷新自定义加载prefab
            GCommon.reloadCustomLoadPrefab();

            WinMsg.SendMsg(WinMsgType.PLAYER_RECORD_CHG);
        }



        #endregion


        #region C2S

        /// <summary>
        /// 初始化 玩家记录列表
        /// </summary>
        public void reqRecordList()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_038_ReqRecordInit());
        }

        #endregion
    }
}
