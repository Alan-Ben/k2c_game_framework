using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using System;


namespace GOE
{
    /// <summary>
    /// 通用返回对象池
    /// </summary>
    public class NPUICacheCommonBack : _AALCacheUnExpandController<NPGGUIWndInstanceCommonBack, NPGGUIMonoCommonBack>
    {
        //对应资源样式id
        private long _m_uiResPathId;
        
        public NPUICacheCommonBack(long _uiResPathId)
            : base(2, 16)
        {
            _m_uiResPathId = _uiResPathId;
        }

        protected override NPGGUIWndInstanceCommonBack _createItem(NPGGUIMonoCommonBack _template)
        {
            //创建一个子窗口管理对象
            NPGGUIWndInstanceCommonBack newWndBar = new NPGGUIWndInstanceCommonBack(_m_uiResPathId);
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
        protected override string _warningTxt { get { return "NPUICacheCommonBack"; } }

        protected override void _discardItem(NPGGUIWndInstanceCommonBack _item)
        {
            if(null != _item)
                _item.discard();
        }

        protected override void _onInit(NPGGUIMonoCommonBack _template)
        {

        }

        protected override void _resetItem(NPGGUIWndInstanceCommonBack _item)
        {
            if(null != _item)
                _item.resetWnd();
        }
    }
}
