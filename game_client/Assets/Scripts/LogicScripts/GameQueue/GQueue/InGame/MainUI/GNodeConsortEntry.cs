
using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 寝宫节点，妃子相关入口场景节点
    /// </summary>
    public class GNodeConsortEntry : BaseQueueNode
    {
        //回退按钮
        private NPGGUIWndInstanceCommonBack _m_cbCommonBackObj;

        public GNodeConsortEntry(): base(EUIQueueStageType.MAIN, UINodeTagConst.C_CONSORT_ENTER)
        {
            
        }

        /// <summary>
        /// 是否所有UI的根节点，如是则在进入本节点的时候会清空所有节点队列
        /// </summary>
        public override bool isRootNode { get { return false; } }
        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public override bool IsCanRollBackQuit { get { return true; } }
        /// <summary>
        /// 本节点在进行前进跳转的时候是否需要自动删除
        /// </summary>
        public override bool NeedAutoRemove { get { return false; } }
        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public override bool IsMainViewNode { get { return true; } }
        /** 当前节点是否还有效 */
        public override bool isEnable { get { return true; } }
        
        /// <summary>
        /// 在节点进入队列的时候执行的事件函数
        /// </summary>
        public override void onEnterQueue()
        {
        }
        /// <summary>
        /// 不论在任何时候，当节点被退出总队列的时候，都会调用onClose函数
        /// </summary>
        public override void onClose()
        {
        }
        
        public override void EnterNode()
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(_onAllSceneInited);

            GTDSceneMain.instance.showMainScene(MainAdditionConsortEntryTDScene.instance, stepCounter.addDoneStepCount);//进入主界面3D场景
            GUISceneMain.instance.showMainScene(GMainGUIAddSceneConsortEntry.instance, stepCounter.addDoneStepCount);
            _showBackBtn();
        }

        private void _showBackBtn()
        {
            if (null != _m_cbCommonBackObj)
            {
                NPUIInstanceCommonBackController.instance.hideCommonBack(_m_cbCommonBackObj, UIResPathConst.WIN_COMMON_BACK);
                _m_cbCommonBackObj = null;
            }
            //加载通用回退按钮
            _m_cbCommonBackObj = NPUIInstanceCommonBackController.instance.showCommonBack(EALUIWndLayer.NORMAL, () => { QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CONSORT_ENTER); }, UIResPathConst.WIN_COMMON_BACK);
            
            //处理的窗口加载完再移动到下一级，保证位置
            GMainGUIAddSceneConsortEntry.instance.regInitDelegate(() =>
            {
                //放到最后
                if (_m_cbCommonBackObj != null) 
                    GCommon.moveTransformToLastAndRefreshLayer(_m_cbCommonBackObj.wnd);
            });
        }
        

        /// <summary>
        /// 所有场景加载完毕后的处理
        /// </summary>
        private void _onAllSceneInited()
        {
        }

        public override void QuitNode()
        {
            if (null != _m_cbCommonBackObj)
            {
                NPUIInstanceCommonBackController.instance.hideCommonBack(_m_cbCommonBackObj, UIResPathConst.WIN_COMMON_BACK);
                _m_cbCommonBackObj = null;
            }
        }

        /**********************
         * 在本节点不允许esc回退时执行的逻辑。
         **/
        public override void onCannotEscBack()
        {
        }
    }
}
