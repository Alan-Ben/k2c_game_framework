using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 对象池
    /// </summary>
    public class NPCommonNumTipWndCache : _AALUICacheController<NPGGUIWndCommonNumTip, NPGGUIMonoCommonNumTip>
    {

        private Transform _m_tParentTrans;//父节点
        public NPCommonNumTipWndCache(Transform _trans, int _minCount, int _maxCount)
            : base(_minCount, _maxCount)
        {
            if(null != _trans)
                _m_tParentTrans = _trans;
        }

        protected override NPGGUIWndCommonNumTip _createItem(NPGGUIMonoCommonNumTip _template)
        {
            if(_template == null)
                return null;
            NPGGUIMonoCommonNumTip monoTipUI = GameObject.Instantiate(_template) as NPGGUIMonoCommonNumTip;
            if(null == monoTipUI)
                return null;
            monoTipUI.transform.SetParent(_m_tParentTrans);
            monoTipUI.transform.localPosition = Vector3.zero;
            monoTipUI.transform.localScale = Vector3.one;
            //创建一个子窗口管理对象
            NPGGUIWndCommonNumTip newWndTip = new NPGGUIWndCommonNumTip(monoTipUI);
            if(null == newWndTip)
            {
                //创建对象无效，删除创建对象资源并退出
                ALUnityCommon.releaseGameObj(monoTipUI);
                return null;
            }

            newWndTip.showWnd();

            return newWndTip;
        }

        //警告信息文字
        protected override string _warningTxt { get { return "NPCommonNumTipWndCache"; } }

        protected override void _discardItem(NPGGUIWndCommonNumTip _item)
        {
            NPGGUIMonoCommonNumTip mono = null;
            if(null != _item)
            {
                mono = _item.wnd;
                _item.discard();
            }

            ALUnityCommon.releaseGameObj(mono);
        }

        protected override void _onInit(NPGGUIMonoCommonNumTip _template)
        {

        }

        protected override void _resetItem(NPGGUIWndCommonNumTip _item)
        {
            if(null != _item && _item.wnd != null)
            {
                _item.wnd.transform.SetParent(_m_tParentTrans);
                _item.wnd.transform.localPosition = Vector3.zero;
                _item.resetWnd();
            }
        }

        protected override void _discard()
        {
        }
    }
}
