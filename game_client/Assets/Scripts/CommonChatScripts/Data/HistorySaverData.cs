using System;
using System.Collections.Generic;
using ALPackage;
using System.Text;
using UnityEngine;
using LitJson;


namespace ChatPackage.Internal
{
    //存储的消息结构
    [Serializable]
    public class HistorySaverData
    {
        // 私聊消息唯一id
        public long msgId;

        //私聊时间戳
        public long timeMs;

        // 消息类型,解释消息内容
        public int msgType;

        // 发送者信息
        public byte[] gameSender;

        // 消息内容
        public byte[] gameContent;

        public HistorySaverData(MsgInfo _info) {

            msgId = _info.msgId;
            msgType = _info.detailInfo.msgType;
            timeMs = _info.timeMs;
            gameSender = _info.detailInfo.getSenderBytesData();
            gameContent = _info.detailInfo.getContentBytesData();

        }
    }
}
