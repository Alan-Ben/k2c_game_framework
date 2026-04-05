using System;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 穿戴道具预制体item
    /// </summary>
    public class GGUIWndPrefabSubDressItem : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoPrefabSubDressItem>
    {
        private long _m_lUIResId;//资源id
        private NPGGuiWndTexture _m_wIcon;//图标
        private GGuiWndSprite _m_wSptIcon;//图标

        public GGUIWndPrefabSubDressItem(long _uiResId, Transform _parent)
            : base(_parent)
        {
            _m_lUIResId = _uiResId;
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_lUIResId);} }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_lUIResId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        public long uiResId { get { return _m_lUIResId; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wIcon?.hideWnd();
            _m_wSptIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wIcon?.discardTexture();
            _m_wSptIcon?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wIcon?.discard();
            _m_wIcon = null;
            _m_wSptIcon?.discard();
            _m_wSptIcon = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.imgIcon);

            if (wnd.imgSptIcon != null)
                _m_wSptIcon = new GGuiWndSprite(wnd.imgSptIcon);
        }

        /// <summary>
        /// 设置图标
        /// </summary>
        /// <param name="_iconIndex"></param>
        public void setIcon(NPGTextureIndex _iconIndex)
        {
            if (_m_wIcon != null)
            {
                _m_wIcon.showWnd();
                _m_wIcon.setTexture(_iconIndex);
            }
        }
        public void setIcon(ENPItemType _type, long _id)
        {
            if (_m_wIcon != null)
            {
                _m_wIcon.showWnd();
                _m_wIcon.setTexture(GCommon.getItemTexIcon(_type, _id));
            }
        }

        /// <summary>
        /// 设置图标
        /// </summary>
        /// <param name="_iconIndex"></param>
        public void setSptIcon(NPGSpriteIndex _iconIndex)
        {
            if (_m_wSptIcon != null)
            {
                _m_wSptIcon.showWnd();
                _m_wSptIcon.setTexture(_iconIndex);
            }
        }

        /// <summary>
        /// 设置名称
        /// </summary>
        /// <param name="_name"></param>
        public void setName(string _name)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtName, _name);
        }

        /// <summary>
        /// 检查并加载预制体
        /// </summary>
        /// <param name="_item"></param>
        /// <param name="_pathId"></param>
        /// <param name="_parentTransform"></param>
        /// <param name="_onLoad"></param>
        public static void checkAndLoadPrefab(GGUIWndPrefabSubDressItem _item, long _pathId, Transform _parentTransform, Action<GGUIWndPrefabSubDressItem> _onLoad)
        {
            if (_parentTransform == null || _pathId <= 0)
            {
                _onLoad?.Invoke(null);
                return;
            }

            //如果资源不同，先销毁旧的
            if (_item != null && _item.uiResId != _pathId)
            {
                _item.discard();
                _item = null;
            }

            if (_item != null)
                _onLoad?.Invoke(_item);
            else
            {
                _item = new GGUIWndPrefabSubDressItem(_pathId, _parentTransform);
                _item.load(() =>
                {
                    _onLoad?.Invoke(_item);
                });
            }
        }
    }
}