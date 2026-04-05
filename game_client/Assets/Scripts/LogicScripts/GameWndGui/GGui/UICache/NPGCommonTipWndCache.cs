
using UnityEngine;
using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用提示的对象池
    /// </summary>
    public class NPGCommonTipWndCache<T, TEMP> : _AALUICacheController<T, TEMP>
        where T : _ATNPGGUIWndTip<TEMP> 
        where TEMP : NPGGUIMonoCommonTip
    {

        private Transform _m_tParentTrans;//父节点
        public NPGCommonTipWndCache(Transform _trans, int _minCount, int _maxCount)
            : base(_minCount, _maxCount)
        {
            if (null != _trans)
                _m_tParentTrans = _trans;
        }

        protected override T _createItem(TEMP _template)
        {
            if (_template == null)
                return null;

            // 实例化对应的GO
            TEMP monoTipUI =  GameObject.Instantiate(_template);
            if (null == monoTipUI)
                return null;

            // 设置GO所在的位置
            monoTipUI.transform.SetParent(_m_tParentTrans);
            monoTipUI.transform.localPosition = Vector3.zero;
            monoTipUI.transform.localScale = Vector3.one;

            // 创建一个子窗口管理对象
            T newWndTip = (T)Activator.CreateInstance(typeof(T), new object[] { monoTipUI }); // 这里用到了反射来解决泛型无法再构造方法传参的情况，如果有性能上的考虑，再做修改
            newWndTip.showWnd();

            return newWndTip;
        }

        // 警告信息文字
        protected override string _warningTxt { get { return "NPGCommonTipWndCache"; } }

        protected override void _discardItem(T _item)
        {
            if(null != _item)
            {
                var tempWnd = _item.wnd;
                _item.discard();
                ALUnityCommon.releaseGameObj(tempWnd);
            }
        }

        protected override void _onInit(TEMP _template)
        {

        }

        protected override void _resetItem(T _item)
        {
            if (null != _item && _item.wnd != null)
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