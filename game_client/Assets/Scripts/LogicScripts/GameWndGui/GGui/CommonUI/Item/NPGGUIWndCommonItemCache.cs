using UnityEngine;
using System.Collections;
using ALPackage;

namespace GOE
{
    public class NPGGUIWndCommonItemCache : _AALUICacheController<NPGGUIWndCommonItem, NPGGUIMonoCommonItem>
    {
        private Transform _m_cacheRoot;
        public NPGGUIWndCommonItemCache(Transform _cacheRoot, int _minCount = 6, int _maxCount = 12) : base(_minCount, _maxCount)
        {
            _m_cacheRoot = _cacheRoot;
        }

        protected override NPGGUIWndCommonItem _createItem(NPGGUIMonoCommonItem _template)
        {
            if (_template == null)
                return null;

            NPGGUIMonoCommonItem mono = UnityEngine.Object.Instantiate(_template, _m_cacheRoot, false);
            if (mono == null)
                return null;

            return new NPGGUIWndCommonItem(mono);
        }

        //警告信息文字
        protected override string _warningTxt { get { return "NPGGUIWndCommonItemCache"; } }

        protected override void _discardItem(NPGGUIWndCommonItem _item)
        {
            NPGGUIMonoCommonItem mono = null;
            if (null != _item)
            {
                mono = _item.wnd;
                _item.discard();
            }

            ALUnityCommon.releaseGameObj(mono);
        }

        protected override void _onInit(NPGGUIMonoCommonItem _template)
        {

        }

        protected override void _resetItem(NPGGUIWndCommonItem _item)
        {
            if (_item == null || _item.wnd == null)
                return;

            _item.resetWnd();
            _item.wnd.transform.SetParent(_m_cacheRoot, false);
        }

        /// <summary>
        /// 回收所有item
        /// </summary>
        public void pushBackAllItem()
        {
            if (usedItemList == null)
                return;

            for (int i = usedItemList.Count - 1; i >= 0; i--)
            {
                pushBackCacheItem(usedItemList[i]);
            }
        }
    }
}
