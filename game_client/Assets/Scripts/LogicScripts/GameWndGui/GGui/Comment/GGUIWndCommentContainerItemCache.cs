using UnityEngine;
using System.Collections;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 弹幕容器的缓存
    /// </summary>
    public class GGUIWndCommentContainerItemCache : _AALUICacheController<GGUIWndCommentContainerItem, GGUIMonoCommentContainerItem>
    {
        private Transform _m_cacheRoot;
        public GGUIWndCommentContainerItemCache(Transform _cacheRoot, int _minCount = 1, int _maxCount = 6) : base(_minCount, _maxCount)
        {
            _m_cacheRoot = _cacheRoot;
        }

        protected override GGUIWndCommentContainerItem _createItem(GGUIMonoCommentContainerItem _template)
        {
            if (_template == null)
                return null;

            GGUIMonoCommentContainerItem mono = UnityEngine.Object.Instantiate(_template, _m_cacheRoot, false);
            if (mono == null)
                return null;

            return new GGUIWndCommentContainerItem(mono);
        }

        //警告信息文字
        protected override string _warningTxt { get { return "GGUIWndCommentContainerItemCache"; } }

        protected override void _discardItem(GGUIWndCommentContainerItem _item)
        {
            GGUIMonoCommentContainerItem mono = null;
            if (null != _item)
            {
                mono = _item.wnd;
                _item.discard();
            }

            ALUnityCommon.releaseGameObj(mono);
        }

        protected override void _onInit(GGUIMonoCommentContainerItem _template)
        {

        }

        protected override void _resetItem(GGUIWndCommentContainerItem _item)
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
