using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 关卡派遣事件页面
    /// </summary>
    public class GGUIWndCommonSimpleDispatchEvent : _AGGUIWndCommonDispatchEvent<GGUIMonoCommonSimpleDispatchEvent>
    {
        private static GGUIWndCommonSimpleDispatchEvent _g_instance;
        public static GGUIWndCommonSimpleDispatchEvent instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndCommonSimpleDispatchEvent();
                return _g_instance;
            }
        }
        
        protected override string _monoAssetPath { get { return GGUIMonoCommonSimpleDispatchEvent.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoCommonSimpleDispatchEvent.objName; } }

        public GGUIWndCommonSimpleDispatchEvent() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override void _onInitDoneDispatchEventWnd()
        {
            if (wnd == null)
                return;

        }

        protected override void _onDiscardDispatchEventWnd()
        {
        }

        protected override void _onShowWndEventWnd()
        {
        }

        protected override void _onHideWndEventWnd()
        {
        }

        protected override void _onResetEventWnd()
        {
        }

        protected override void _onSetEventDataSubWnd()
        {
        }

        protected override void _refreshWndEventWnd()
        {
        }

        // protected override void _onClickHeroHeadIconSub(GGUIWndHeroIconNullableItem _item)
        // {
        //     if (_item == null)
        //         return;
        //
        //     if (_item.heroShowData != null)//当有大臣信息时
        //     {
        //         selectedHeroShowList.Remove(_item.heroShowData);//移除选中大臣
        //         _refreshHeroHeadContainer();
        //         _refreshCondContainer(false);
        //         _refreshDispatchBtn();//刷新派遣按钮
        //     }
        //     else
        //     {
        //         _openSelectHeroWnd();
        //     }
        // }

        protected override void _onAKeyDispatchBtnClickSub()
        {
        }

        /// <summary>
        /// 打开选择大臣窗口
        /// </summary>
        private void _openSelectHeroWnd()
        {
            GUIAddSceneCommonDispatchEvent.instance.showGGUIWndCommonDispatchEventSelectHero();
        }
    }
}