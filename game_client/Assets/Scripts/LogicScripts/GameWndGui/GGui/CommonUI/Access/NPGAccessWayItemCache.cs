using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 获取途径item的对象池
    /// </summary>
    public class NPGAccessWayItemCache<T, TEMP> : _AALUICacheController<T, TEMP>
        where T : _ATALBasicUISubWnd<TEMP> 
        where TEMP : _AALBasicUIWndMono
    {
        [NotNull]private List<T> _m_itemList = new List<T>();
        private Transform _m_tParentTrans;//父节点
        public NPGAccessWayItemCache(Transform _trans, int _minCount, int _maxCount)
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
        protected override string _warningTxt { get { return "NPGAccessWayItemCache"; } }

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
            _m_itemList.Clear();
        }

        public T popAccessItem()
        {
            T curItem = popItem();
            curItem.rectTransform.SetAsLastSibling();
            _m_itemList.Add(curItem);
            return curItem;
        }

        public void pushBackAccessItemByCond(Predicate<T> _match)
        {
            if (_match == null)
                return;

            for (int i = _m_itemList.Count - 1; i >= 0; i--)
            {
                if (_match(_m_itemList[i]))
                {
                    _m_itemList[i].hideWnd();
                    pushBackCacheItem(_m_itemList[i]);
                    _m_itemList.RemoveAt(i);
                }
            }
        }

        public void pushBackAllAccessItem()
        {
            for (int i = 0; i < _m_itemList.Count; i++)
            {
                _m_itemList[i].hideWnd();
            }
            _m_itemList.Clear();
            pushBackAllCacheItems();
        }
    }
}