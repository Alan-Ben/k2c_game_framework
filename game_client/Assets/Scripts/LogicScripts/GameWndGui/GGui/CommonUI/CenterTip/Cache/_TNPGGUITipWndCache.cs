using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// tip缓存池模板类
    /// </summary>
    /// <typeparam name="_T_WND"></typeparam>
    /// <typeparam name="_T_MONO"></typeparam>
    public class _TNPGGUITipWndCache<_T_WND, _T_MONO> : _AALUICacheController<_T_WND, _T_MONO>, _INPTipCache
        where _T_WND : _ATNPGGUIWndTip<_T_MONO>
        where _T_MONO : NPGGUIMonoCommonTip
    {
        private Transform _m_tParentTrans;//父节点
        public _TNPGGUITipWndCache(Transform _trans, int _minCount, int _maxCount)
            : base(_minCount, _maxCount)
        {
            if (null != _trans)
                _m_tParentTrans = _trans;
        }

        protected override string _warningTxt { get { return "_TNPGGUITipWndCache"; } }


        protected override _T_WND _createItem(_T_MONO _template)
        {
            if (_template == null)
                return null;

            // 实例化对应的GO
            _T_MONO monoTipUI = GameObject.Instantiate(_template);
            if (null == monoTipUI)
                return null;

            // 设置GO所在的位置
            monoTipUI.transform.SetParent(_m_tParentTrans);
            monoTipUI.transform.localPosition = Vector3.zero;
            monoTipUI.transform.localScale = Vector3.one;

            // 创建一个子窗口管理对象
            _T_WND newWndTip = (_T_WND)Activator.CreateInstance(typeof(_T_WND), new object[] { monoTipUI }); // 这里用到了反射来解决泛型无法再构造方法传参的情况，如果有性能上的考虑，再做修改
            newWndTip.showWnd();

            return newWndTip;
        }

        protected override void _discardItem(_T_WND _item)
        {
            if (null != _item)
            {
                var tempWnd = _item.wnd;
                _item.discard();
                ALUnityCommon.releaseGameObj(tempWnd);
            }
        }

        protected override void _onInit(_T_MONO _template)
        {

        }

        protected override void _resetItem(_T_WND _item)
        {
            base._resetItem(_item);
            if (null != _item && _item.wnd != null)
            {
                _item.wnd.transform.SetParent(_m_tParentTrans, false);
                _item.resetWnd();
            }
        }
    }
}
