
using System.Collections.Generic;
using System;
using JetBrains.Annotations;

namespace ChatPackage.Internal
{
    // 玩家聊天记录存储
    public class HistorySaverMgr
    {
        [NotNull] public static HistorySaverMgr instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new HistorySaverMgr();
                return _g_instance;
            }
        }
        private static HistorySaverMgr _g_instance;

        [NotNull] private readonly Dictionary<string, HistorySaver> _m_historySaverDic;

        private HistorySaverMgr()
        {
            _m_historySaverDic = new Dictionary<string, HistorySaver>();
        }

        /// <summary>
        /// 存储私聊消息
        /// </summary>
        /// <param name="_id">聊天对象id</param>
        /// <param name="_info">聊天消息</param>
        public void saveNewMsg(string _id, MsgInfo _info)
        {
            if (string.IsNullOrEmpty(_id) || _info == null)
                return;

            _m_historySaverDic.TryGetValue(_id, out HistorySaver saver);
            if (saver == null)
            {
                saver = new HistorySaver(_id);
                saver.init();
                _m_historySaverDic.Add(_id, saver);
            }

            HistorySaverData data = new HistorySaverData(_info);
            saver.addData(data);
            saver.saveSetting();
        }

        public void saveNewMsg(string _id, List<MsgInfo> _infoList)
        {
            if (_id == null || _infoList == null || _infoList.Count == 0)
                return;

            _m_historySaverDic.TryGetValue(_id, out HistorySaver saver);
            if (saver == null)
            {
                saver = new HistorySaver(_id);
                saver.init();
                _m_historySaverDic.Add(_id, saver);
            }

            for (int i = 0; i < _infoList.Count; i++)
            {
                MsgInfo temp = _infoList[i];
                if (null == temp)
                    continue;

                HistorySaverData data = new HistorySaverData(temp);
                saver.addData(data);
            }

            saver.saveSetting();
        }

        /// <summary>
        /// 获取与某个人私聊的消息前几条消息
        /// </summary>
        /// <param name="_id">会话唯一标识</param>
        /// <param name="_msgId">指定获取该消息之前的历史消息，不大于0则表示从最新消息开始</param>
        /// <param name="_msgCount">消息个数</param>
        /// <param name="_action">回调</param>
        public void getMsgListBefore(string _id, long _msgId, int _msgCount, Action<List<HistorySaverData>> _action)
        {
            if (string.IsNullOrEmpty(_id) || _action == null)
                return;
            
            _m_historySaverDic.TryGetValue(_id, out HistorySaver saver);

            if(saver == null)
            {
                saver = new HistorySaver(_id);
                saver.init();
                _m_historySaverDic.Add(_id, saver);
            }

            List<HistorySaverData> msgInfoList = saver.historyDataList;

            // if (msgInfoList == null || msgInfoList.Count < _msgCount)
            // {
            //     _action(msgInfoList);
            //     return;
            // }

            int count = 0;
            List<HistorySaverData> countInfoList = new List<HistorySaverData>();
            for (int i = msgInfoList.Count - 1; i >= 0; i--)
            {
                HistorySaverData temp = msgInfoList[i];
                if (temp == null)
                    continue;
                if(temp.msgId >= _msgId && _msgId > 0)
                    continue;
                countInfoList.Insert(0, temp);
                count++;
                if (count == _msgCount)
                    break;
            }

            _action(countInfoList);
        }
    }
}