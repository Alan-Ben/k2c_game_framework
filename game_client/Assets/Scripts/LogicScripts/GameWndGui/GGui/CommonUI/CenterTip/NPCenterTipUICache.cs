using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using System;

namespace GOE
{
    public class NPCenterTipUICache : _AALUITemplateWndSafeCache<NPGGUIWndCenterTipItem, NPGGUIMonoCenterTipItem>
    {
        public NPCenterTipUICache(Transform _parent, int _minCount, int _maxCount)
            : base(_parent, _minCount, _maxCount, 5)
        {
        }

        //警告信息文字
        protected override string _warningTxt { get { return "WCGNoticeTipUICache"; } }

        protected override void _onInit(NPGGUIMonoCenterTipItem _template)
        {

        }

        protected override void _resetItem(NPGGUIWndCenterTipItem _item)
        {
            if(null != _item)
                _item.hideTip();
        }

        /// <summary>
        /// 根据脚本创建窗口对象
        /// </summary>
        /// <param name="_mono"></param>
        /// <returns></returns>
        protected override NPGGUIWndCenterTipItem _createWndByMono(NPGGUIMonoCenterTipItem _mono)
        {
            return new NPGGUIWndCenterTipItem(_mono);
        }
        /// <summary>
        /// 初始化实例化窗口对象
        /// </summary>
        /// <param name="_wnd"></param>
        protected override void _initNewWnd(NPGGUIWndCenterTipItem _wnd)
        {
            if(null == _wnd)
                return;

            //先显示再缩小
            _wnd.showWnd();
            _wnd.hideTip();
        }
    }
}
