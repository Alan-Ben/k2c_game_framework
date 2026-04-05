using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 推送礼包Banner容器窗口
    /// </summary>
    public class GGUIWndPushGiftPackBannerItemContainer : _ATNPGGUIWndSelectContainer<GGUIMonoPushGiftPackBannerItem, GGUIMonoPushGiftPackBannerItemContainer, GGUIWndPushGiftPackBannerItem>
    {
        private List<PushGiftPackInfo> _m_lPushGiftPackInfoList;
        
        /// <summary>
        /// 选中礼包信息变化事件
        /// </summary>
        public event Action<PushGiftPackInfo> onSelectPushGiftPackInfo;
        
        public GGUIWndPushGiftPackBannerItemContainer(GGUIMonoPushGiftPackBannerItemContainer _containerMono) : base(_containerMono)
        {
        }

        protected override void _onShowWndEx()
        {
        }

        protected override void _onHideWndEx()
        {
        }

        protected override void _onResetEx()
        {
        }

        protected override void _onDiscardEx()
        {
            onSelectItem -= _onSelectItemWnd;
            onSelectPushGiftPackInfo = null;

            _m_lPushGiftPackInfoList = null;
        }

        protected override void _onWndInitDoneEx()
        {
            if (wnd == null)
                return;
            
            // 监听选中item事件
            onSelectItem += _onSelectItemWnd;
        }

        protected override GGUIWndPushGiftPackBannerItem _createItemWnd(GGUIMonoPushGiftPackBannerItem _itemMono)
        {
            GGUIWndPushGiftPackBannerItem itemWnd = new GGUIWndPushGiftPackBannerItem(_itemMono);
            return itemWnd;
        }
        
        /// <summary>
        /// 设置推送礼包信息列表
        /// </summary>
        /// <param name="_pushGiftPackInfoList">推送礼包信息列表</param>
        /// <param name="_defaultSelectIndex">默认选中索引</param>
        public void setInfoList(List<PushGiftPackInfo> _pushGiftPackInfoList, int _defaultSelectIndex = 0)
        {
            _m_lPushGiftPackInfoList = _pushGiftPackInfoList;
            if(_m_lPushGiftPackInfoList == null)
                return;
            
            // 使用基类提供的setShowData方法设置数据
            setShowData(_m_lPushGiftPackInfoList, _setItemDataShowInfo);
            
            // 设置默认选中
            if (_m_lPushGiftPackInfoList.Count > 0)
            {
                int selectIndex = _defaultSelectIndex >= 0 && _defaultSelectIndex < _m_lPushGiftPackInfoList.Count ? _defaultSelectIndex : 0;
                scrollMoveTo(selectIndex);
            }
        }
        
        private void _setItemDataShowInfo(GGUIWndPushGiftPackBannerItem _itemWnd, PushGiftPackInfo _info)
        {
            _itemWnd?.setInfo(_info);
        }

        /// <summary>
        /// 选中推送礼包
        /// </summary>
        public void selectPushGiftPack(PushGiftPackInfo _info)
        {
            if(_m_lPushGiftPackInfoList == null)
                return;
            
            int index = _m_lPushGiftPackInfoList.IndexOf(_info);
            if (index >= 0 && index < _m_lPushGiftPackInfoList.Count)
            {
                scrollMoveTo(index);
            }
        }
        
        /// <summary>
        /// 选中item回调
        /// </summary>
        /// <param name="_itemWnd">选中的item窗口</param>
        private void _onSelectItemWnd(GGUIWndPushGiftPackBannerItem _itemWnd)
        {
            if (_itemWnd == null)
                return;
            
            // 通知外部选中的礼包信息
            onSelectPushGiftPackInfo?.Invoke(_itemWnd.pushGiftPackInfo);
        }
    }
}