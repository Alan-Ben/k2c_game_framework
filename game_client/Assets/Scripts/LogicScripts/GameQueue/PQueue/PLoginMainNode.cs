using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class PLoginMainNode : UIQueueBaseNode
    {
        /// <summary>
        /// 是否所有UI的根节点，如是则在进入本节点的时候会清空所有节点队列
        /// </summary>
        public override bool isRootNode { get { return true; } }
        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public override bool IsCanRollBackQuit { get { return false; } }

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
        /// 本节点是否一个纯粹的UI节点，如果是则将关闭主摄像头
        /// </summary>
        public override bool isOnlyUINode { get { return false; } }

        //处理完成之后的回调
        private Action _m_dDoneDelegate;

        public PLoginMainNode(Action _doneDelegate)
            : base(EUIQueueStageType.LOGIN)
        {
            _m_dDoneDelegate = _doneDelegate;
        }

        public override void EnterNode()
        {
            //进入加载场景
            GStageLogin.instance.enterStage();
            GStageLogin.instance.regInitDelegate(() =>
            {
                if(null != _m_dDoneDelegate)
                    _m_dDoneDelegate();
                _m_dDoneDelegate = null;
            });
        }

        public override void QuitNode()
        {
        }
        
        /// <summary>
        /// 在本节点不允许esc回退时执行的逻辑。
        /// </summary>
        public override void onCannotEscBack()
        {
            //TODo 打开游戏的退出游戏确认框 比如：
            NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.quit_game)
                , TextTranslate.instance.getLanguage(TransKeyConst.play_more)
                , null
                , TextTranslate.instance.getLanguage(TransKeyConst.quit)
                , () =>
                {
                    Application.Quit();
                }
            );
        }
    }
}
