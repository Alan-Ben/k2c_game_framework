using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using ALPackage;


namespace GOE
{
    /// <summary>
    /// 游戏UI场景的管理对象，不同的视图切换只是在本UI管理器内进行的子类型切换
    /// </summary>
    public class NPGUISceneBattleEditor : _ABasicUIScene
    {
        private static NPGUISceneBattleEditor _g_instance = new NPGUISceneBattleEditor();
        public static NPGUISceneBattleEditor instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new NPGUISceneBattleEditor();

                return _g_instance;
            }
        }

        protected override void _onEnterScene()
        {
            //加载相关的战斗编辑窗口
            //直接设置完成，窗口变更为在addscene处理
            setSceneInited();
        }

        protected override void _onSceneInited()
        {
            //注册消息
            //WinMsg.RegisterMsgAct(WinMsgType.ENTER_BOSS_BATTLE, _onEnterBossBattle);
            //WinMsg.RegisterMsgAct(WinMsgType.QUIT_BOSS_BATTLE, _onQuitBossBattle);
        }

        protected override void _dealQuitScene()
        {
            //注销消息
            //WinMsg.UnregisterMsgAct(WinMsgType.ENTER_BOSS_BATTLE, _onEnterBossBattle);
            //WinMsg.UnregisterMsgAct(WinMsgType.QUIT_BOSS_BATTLE, _onQuitBossBattle);
        }

        /***************
         * 是否需要检查教程
         **/
        public override bool needCheckTutorial { get { return true; } }
        /***************
         * 是否允许展示获取物品信息
         **/
        public override bool canShowNotice { get { return true; } }

        /****************
         * 进入boss战时触发的消息
         **/
        //protected void _onEnterBossBattle()
        //{
        //    //判断当前所在视图
        //    if(_m_eViewType == ENPMainUIView.DEFAULT)
        //    {
        //        //默认视图则刷新显示
        //        _refreshView(null);
        //    }
        //}

        /****************
         * 退出boss战时触发的消息
         **/
        //protected void _onQuitBossBattle()
        //{
        //    //判断当前所在视图
        //    if(_m_eViewType == ENPMainUIView.DEFAULT)
        //    {
        //        //默认视图则刷新显示
        //        _refreshView(null);
        //    }
        //}
    }
}
