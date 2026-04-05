
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace ChatPackage
{
    /// <summary>
    /// <see cref="GUISubWndChatMsgList"/>的mono类
    /// </summary>
    public class GUIMonoChatMsgList : ALUGUIMonoVerticalMultiSizeLayout
    {
        [ALHeader("判断是否在底部的数值")]
        public float bottomCheckPosition = 100;

        [ALHeader("初始化时显示的消息数量")]
        public int initMsgCount = 15;

        [ALHeader("每次获取历史消息时获取多少条历史消息")]
        public int getHistoryMsgCount = 10;

        [ALHeader("根据网络状况显示的列表")]
        public List<GameObject> netConnectedShow;
        public List<GameObject> netDisconnectedShow;

        /// <summary>
        /// 设置当前页面的网络状态
        /// </summary>
        /// <param name="_connected">是否连接上了</param>
        public void setNetState(bool _connected)
        {
            if (_connected)
            {
                ALUGUICommon.setGameObjEnable(netDisconnectedShow, false);
                ALUGUICommon.setGameObjEnable(netConnectedShow, true);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(netConnectedShow, false);
                ALUGUICommon.setGameObjEnable(netDisconnectedShow, true);
            }
        }
    }
}