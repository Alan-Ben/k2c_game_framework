using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

namespace GOE
{
    public enum UEWCGUISceneLoginViewEnum
    {
        DEFAULT,
        ALL_WAY,//所有登录方式
        ACCOUNT_SELECT,
        INPUT_ACCOUNT,
        LOGINING, //正在登录中
    }


    /*********************
     * 初次进入的UI视图
     **/
    public class NPGUISceneLogin : _ABasicUIScene
    {
        private static NPGUISceneLogin _g_instance = new NPGUISceneLogin();
        public static NPGUISceneLogin instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new NPGUISceneLogin();

                return _g_instance;
            }
        }

        /** 当前ui视图的现实视图枚举 */
        private UEWCGUISceneLoginViewEnum _m_eViewType;

        public NPGUISceneLogin()
            : base()
        {
            //判断只有使用外网环境才使用SDK登录
            _m_eViewType = UEWCGUISceneLoginViewEnum.DEFAULT;
        }

        protected override void _onEnterScene()
        {
            NPPGUIWndOutGameFuncs.instance.load(setSceneInited);
        }

        protected override void _dealQuitScene()
        {
            NPPGUIWndOutGameFuncs.instance.discard();
        }

        protected override void _onSceneInited()
        {
            //显示基本客服窗口
            NPPGUIWndOutGameFuncs.instance.showWnd();
        }

        /***************
         * 是否需要检查教程
         **/
        public override bool needCheckTutorial { get { return false; } }
        /***************
         * 是否允许展示获取物品信息
         **/
        public override bool canShowNotice { get { return false; } }
    }
}
