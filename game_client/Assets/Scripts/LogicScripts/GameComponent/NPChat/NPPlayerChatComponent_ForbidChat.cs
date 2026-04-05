using ChatPackage;
using GC2GS.p004_PlayerOp;
using GS2GC.p002_InitOp;
using GS2GC.p004_PlayerOp;
using NPCommon;
using NPEnum;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 聊天组件-禁言功能
    /// </summary>
    public partial class NPPlayerChatComponent
    {
        //禁言数据列表
        private List<NPCommon_ForbidChatInfo> _m_lForbidChatList;


        /// <summary>
        /// 是否正在被禁言中
        /// </summary>
        /// <param name="_chatRoom"></param>
        /// <returns></returns>
        public bool isInForbidChat(ENPChatRoomType _chatRoom, bool _showTip)
        {
            if (_m_lForbidChatList == null || _m_lForbidChatList.Count == 0)
                return false;

            long nowTimeMs = FpsAndPingMgr.instance.serverTimeTag;
            for (int i = 0; i < _m_lForbidChatList.Count; i++)
            {
                if(_m_lForbidChatList[i] == null)
                    continue;

                //如果禁言类型为0，表示全局禁言，或者禁言类型与当前聊天频道类型相同，并且禁言未过期，则表示正在被禁言中
                if ((_m_lForbidChatList[i].getRoomType() == 0 || _m_lForbidChatList[i].getRoomType() == (int) _chatRoom) &&
                    (_m_lForbidChatList[i].getEndMs() == -1 || _m_lForbidChatList[i].getEndMs() > nowTimeMs))
                {
                    //是否显示被禁言提示
                    if (_showTip)
                    {
                        string leftTimeStr = "";
                        if (_m_lForbidChatList[i].getEndMs() == -1)
                            leftTimeStr = TextTranslate.instance.getLanguage(TransKeyConst.chat_forbidChatForever_none);//永久
                        else
                            leftTimeStr = TextTranslate.instance.getLanguage(TransKeyConst.chat_forbidChatLeftTime_str,
                                TimeUtil.millisecondsToTime_Two(_m_lForbidChatList[i].getEndMs() - nowTimeMs));//剩余{0}

                        NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.chat_forbidChatTip_str, leftTimeStr));
                    }
                    return true;
                }

            }
            return false;
        }

        /// <summary>
        /// 是否正在被禁言中
        /// </summary>
        /// <param name="_chatInfo"></param>
        /// <param name="_showTip"></param>
        /// <returns></returns>
        public bool isInForbidChat(_AChatInfo _chatInfo, bool _showTip)
        {
            if (_chatInfo == null)
                return false;

            NPRoomChatInfo roomChatInfo = _chatInfo as NPRoomChatInfo;
            if (roomChatInfo == null)
                return false;

            ENPChatRoomType chatRoomType = roomChatInfo.type;
            return isInForbidChat(chatRoomType, _showTip);
        }

        #region S2C

        /// <summary>
        /// 禁言数据初始化
        /// </summary>
        /// <param name="_msg"></param>
        public void retForbidChatInit(GS2GC_002_089_RetForbidChatInit _msg)
        {
            if (_msg == null)
                return;

            _m_lForbidChatList = _msg.getForbidChatList();
            _m_initStepCounter.addDoneStepCount();
        }

        /// <summary>
        /// 禁言数据变更推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onForbidChatChg(GS2GC_004_068_OnForbidChatChg _msg)
        {
            if (_msg == null) 
                return;

            NPCommon_ForbidChatInfo forbidChatInfo = _msg.getForbidChat();
            if(_m_lForbidChatList == null)
                _m_lForbidChatList = new List<NPCommon_ForbidChatInfo>();

            bool isFind = false;
            for (int i = 0; i < _m_lForbidChatList.Count; i++)
            {
                if(_m_lForbidChatList[i] == null)
                    continue;
                //如果禁言类型与当前变更的禁言类型相同，则更新禁言数据
                if (_m_lForbidChatList[i].getRoomType() == forbidChatInfo.getRoomType())
                {
                    _m_lForbidChatList[i] = forbidChatInfo;
                    isFind = true;
                    break;
                }
            }
            if (!isFind)
                _m_lForbidChatList.Add(forbidChatInfo);
        }

        /// <summary>
        /// 解除禁言数据推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onRemoveForbidChat(GS2GC_004_069_OnRemoveForbidChat _msg)
        {
            if (_msg == null) 
                return;
            if(_m_lForbidChatList == null || _m_lForbidChatList.Count == 0)
                return;

            for (int i = _m_lForbidChatList.Count - 1; i >= 0; i--)
            {
                if(_m_lForbidChatList[i] == null)
                    continue;
                //如果禁言类型与当前变更的禁言类型相同，则移除该禁言数据
                if (_m_lForbidChatList[i].getRoomType() == _msg.getRoomType())
                {
                    _m_lForbidChatList.RemoveAt(i);
                }
            }
        }

        #endregion

        #region C2S

        /// <summary>
        /// 请求禁言数据初始化
        /// </summary>
        public void reqForbidChatInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_089_ReqForbidChatInit());
        }

        /// <summary>
        /// 请求举报指定玩家
        /// </summary>
        /// <param name="_targetCid"></param>
        /// <param name="_content"></param>
        public void reqReportPlayer(long _targetCid, string _content)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_004_013_ReqReportPlayer(_targetCid, _content),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_013_RetReportPlayer>(null));
        }

        #endregion
    }
}
