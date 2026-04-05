using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**********************
 * 容器对象的刷新任务
 **/
namespace ALPackage
{
    public class ALUGUIRefreshMonoTask : _IALBaseMonoTask
    {
        /** 刷新接口 */
        private _IALBasicRefreshUIWndInterface _m_iRefreshInterface;
        /** 刷新的序列号 */
        private int _m_iShowSerialize;

        public ALUGUIRefreshMonoTask(_IALBasicRefreshUIWndInterface _interface)
        {
            _m_iRefreshInterface = _interface;
            _m_iShowSerialize = _interface.showOpSerialize;
        }

        /*******************
         * 任务具体的执行函数
         **/
        public void deal()
        {
            if (_m_iRefreshInterface.showOpSerialize != _m_iShowSerialize)
                return;

            //刷新操作
            _m_iRefreshInterface.frameRefresh();

            //加入下一帧
            ALMonoTaskMgr.instance.addNextFrameTask(this);
        }
    }
}
