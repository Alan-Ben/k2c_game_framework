using System;
using System.Collections.Generic;

namespace ALPackage
{
    /**********************
     * 用于处理客户端连接对象对消息的处理操作的任务
     **/
    public class ALBasicClientMesDealMonoTask : _IALBaseMonoTask
    {
        private _AALBasicClient _m_bcClient;
        private List<byte[]> _m_lRecMsgList;

        public ALBasicClientMesDealMonoTask(_AALBasicClient _client)
        {
            _m_bcClient = _client;
            _m_lRecMsgList = new List<byte[]>();
        }

        /*******************
         * 任务具体的执行函数
         **/
        public void deal()
        {
            //获取所有消息对象
            while (true)
            {
                _m_bcClient.popRecMsg(_m_lRecMsgList);

                if (_m_lRecMsgList.Count <= 0)
                    break;

                for (int i = 0; i < _m_lRecMsgList.Count; i++)
                {
                    try
                    {
                        if(!_m_bcClient.isExit)
                            _m_bcClient.receiveMes(_m_lRecMsgList[i]);
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogException(e);
                    }
                }
            }

            //连接对象无效则不处理
            if (null == _m_bcClient || _m_bcClient.isExit)
                return;

            //将本任务放入下一帧执行
            ALMonoTaskMgr.instance.addNextFrameTask(this);
        }
    }
}
