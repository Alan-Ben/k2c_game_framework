
using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 主界面几个主要功能节点的基类
    /// </summary>
    public abstract class _AGNodeMain : _AGNodeMainSub
    {
        protected _AGNodeMain(EMainFunctionTabType _functionTabType, string _nodeUITag) 
            : base(_functionTabType, _nodeUITag)
        {
        }

        
        /**********************
         * 在本节点不允许esc回退时执行的逻辑。
         **/
        public override void onCannotEscBack()
        {
            //打开游戏的退出游戏确认框
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
        
        /// <summary>
        /// 是否所有UI的根节点，如是则在进入本节点的时候会清空所有节点队列
        /// </summary>
        public sealed override bool isRootNode { get { return true; } }
        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public sealed override bool IsCanRollBackQuit { get { return false; } }
    }
}