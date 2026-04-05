using UnityEngine;
using System.Collections;
using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 邮件列表加载的预制体缓存(骑士)
    /// </summary>
    public class NPGGUIMailItemCache_Hero : _AALUnsafeCacheController<GGUISubWndMailItemPrefab_Hero, long>
    {
        //存储的root对象
        private GameObject _m_gRootGo;

        //判断Id，避免字符串判断
        private long _m_iUIResPathId;

        public NPGGUIMailItemCache_Hero(long _typeId) : base(2, 20)
        {
            _m_iUIResPathId = _typeId;
            _m_gRootGo = new GameObject();
            _m_gRootGo.name = "mailItem_typd_" + _m_iUIResPathId;
            _m_gRootGo.transform.position = Vector3.zero;
        }

        protected override GGUISubWndMailItemPrefab_Hero _createItem(long _template)
        {
            if (null == _m_gRootGo || null == _m_gRootGo.transform)
                return null;

            //创建一个子窗口管理对象
            GGUISubWndMailItemPrefab_Hero newWndBar = new GGUISubWndMailItemPrefab_Hero(_template, _m_gRootGo.transform);
            //直接进行加载
            newWndBar.load();

            return newWndBar;
        }

        //警告信息文字
        protected override string _warningTxt { get { return "NPGGUIMailItemCache"; } }
        /// <summary>
        /// 加载的预制体路径
        /// </summary>
        public long UIResPathId { get => _m_iUIResPathId; }

        protected override void _discardItem(GGUISubWndMailItemPrefab_Hero _item)
        {
            if(null != _item)
                _item.discard();
        }

        protected override void _onInit(long _template) { }

        protected override void _resetItem(GGUISubWndMailItemPrefab_Hero _item)
        {
            if (null == _item)
                return;

            _item.setParent(_m_gRootGo.transform);
            //重置窗口
            _item.resetWnd();
        }

        protected override void _discard()
        {
            if (null != _m_gRootGo)
                ALUnityCommon.releaseGameObj(_m_gRootGo);
            _m_gRootGo = null;
        }

        
        /// <summary>
        /// 执行窗口的加载,并返回窗口
        /// </summary>
        /// <param name="onLoaded"></param>
        public void popItem(Action<GGUISubWndMailItemPrefab_Hero> _onLoaded)
        {
            GGUISubWndMailItemPrefab_Hero item = popItem();
            if (null == item)
            {
                return;
            }

            //直接使用回调进行处理
            item.regLoadDoneDelegate(() =>
                {
                    if (null != _onLoaded)
                        _onLoaded(item);
                });
        }
    }
}
