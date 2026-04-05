using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using System;


namespace GOE
{
    /// <summary>
    /// 血条对象池
    /// </summary>
    public class NPUICacheTransparentBk : _AALCacheUnExpandController<NPPGUIWndInstanceTransparentBk, NPPGUIMonoTransparentBk>
    {
        private static NPUICacheTransparentBk _g_instance;
        public static NPUICacheTransparentBk instance { get { return _g_instance; } }

        /// <summary>
        /// 初始化实例化背景UI的操作
        /// </summary>
        /// <param name="_mono"></param>
        public static void g_init()
        {
            //初始化再创建，避免无资源的其他问题
            //由于子对象本身自己会进行加载，所以在cache这里只创建对象不需要加载和初始化资源
            _g_instance = new NPUICacheTransparentBk();
        }

        public NPUICacheTransparentBk()
            : base(2, 16)
        {
        }

        protected override NPPGUIWndInstanceTransparentBk _createItem(NPPGUIMonoTransparentBk _template)
        {
            //创建一个子窗口管理对象
            NPPGUIWndInstanceTransparentBk newWndBar = new NPPGUIWndInstanceTransparentBk();
            if(null == newWndBar)
            {
                return null;
            }

            newWndBar.load();
            //隐藏窗口
            newWndBar.hideWnd();

            return newWndBar;
        }

        //警告信息文字
        protected override string _warningTxt { get { return "NPUICacheTransparentBk"; } }

        protected override void _discardItem(NPPGUIWndInstanceTransparentBk _item)
        {
            if(null != _item)
                _item.discard();
        }

        protected override void _onInit(NPPGUIMonoTransparentBk _template)
        {

        }

        protected override void _resetItem(NPPGUIWndInstanceTransparentBk _item)
        {
            if(null != _item)
                _item.resetWnd();
        }
    }
}
