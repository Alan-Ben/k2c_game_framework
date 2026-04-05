using ALPackage;
using System;
using NPCommon;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 获得装扮类道具界面（头像、头像框、气泡框、称号）
    /// </summary>
    public class NPGGUIWndGetShowOffItem : _ANPGGUIBasicWnd<NPGGUIMonoGetShowOffItem>
    {
        private static NPGGUIWndGetShowOffItem _g_instance;
        public static NPGGUIWndGetShowOffItem instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new NPGGUIWndGetShowOffItem();
                return _g_instance;
            }
        }

        private NPGGUIWndGetShowOffItemSub _m_itemSubWnd;

        private NPCommon_ItemInfo _m_item;

        private Action _m_doneAction;

        private long _m_lShowSerialize;//显示序列号

        private NPGGUIWndGetShowOffItem() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return NPGGUIMonoGetShowOffItem.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoGetShowOffItem.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _refresh();
            _delayPlayIdleAni();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            if (null != _m_itemSubWnd)
                _m_itemSubWnd.hideWnd();
        }

        protected override void _onReset()
        {
            if (null != _m_itemSubWnd)
                _m_itemSubWnd.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (null == wnd)
                return;

            if (null != _m_itemSubWnd)
                _m_itemSubWnd.discard();
            _m_itemSubWnd = null;
            
            if (_m_doneAction != null)
            {
                Action onClose = _m_doneAction;
                _m_doneAction = null;
                onClose?.Invoke();
            }

            ALUGUICommon.uncombineBtnClick(wnd.closeBtn, _closeBtnDidClick);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.closeBtn, _closeBtnDidClick);
        }

        public void setItem(NPCommon_ItemInfo _item, Action _doneAction)
        {
            if (null == _item)
                return;

            _m_item = _item;
            _m_doneAction = _doneAction;
            _refresh();
        }

        private void _refresh()
        {
            if (null == _m_item || null == wnd)
                return;

            //加载对应类型预制体
            NPGGUIGetShowOffItemMono temp = null;
            for (int i = 0; i < wnd.specialItemMonoList.Count; i++)
            {
                temp = wnd.specialItemMonoList[i];
                if (null == temp || null == wnd.itemSubParent)
                    continue;

                if ((int)temp.type == _m_item.getItemType())
                {
                    if(null == _m_itemSubWnd)
                    {
                        _m_itemSubWnd = new NPGGUIWndGetShowOffItemSub(temp.resPathid, wnd.itemSubParent);
                        _m_itemSubWnd.load(() =>
                        {
                            _m_itemSubWnd.setItem(_m_item);
                            _m_itemSubWnd.showWnd();
                        });
                    }
                    if(null != _m_itemSubWnd)
                    {
                        if(_m_itemSubWnd.uiPathId == temp.resPathid)
                        {
                            _m_itemSubWnd.setItem(_m_item);
                            _m_itemSubWnd.showWnd();
                        }
                        else
                        {
                            _m_itemSubWnd.discard();
                            _m_itemSubWnd = null;
                            _m_itemSubWnd = new NPGGUIWndGetShowOffItemSub(temp.resPathid, wnd.itemSubParent);
                            _m_itemSubWnd.load(() =>
                            {
                                _m_itemSubWnd.setItem(_m_item);
                                _m_itemSubWnd.showWnd();
                            });
                        }
                    }
                    break;
                }
            }

            //名称
            ALUGUICommon.setLabelTxt(wnd.txtName, GCommon.getItemName((ENPItemType)_m_item.getItemType(), _m_item.getSubId()));

            //类型
            ALUGUICommon.setLabelTxt(wnd.txtType, TextTranslate.instance.getLanguage(_getTypeName()));

            //有效期
            string expStr = null;
            if (_m_item.getCount() > 0)
                expStr = TimeUtil.millisecondsToTime_Two(_m_item.getCount() * 1000);
            else
                expStr = TextTranslate.instance.getLanguage(TransKeyConst.playerDress_alwaysEnable_str);
            ALUGUICommon.setLabelTxt(wnd.txtLeftTime, TextTranslate.instance.getLanguage(TransKeyConst.getItem_expiredTime_time, expStr));
        }

        //获取类型名称
        private string _getTypeName()
        {
            if (_m_item == null)
                return null;

            switch ((ENPItemType)_m_item.getItemType())
            {
                case ENPItemType.ICON:
                    return TransKeyConst.getItem_iconTypeName_none;
                case ENPItemType.ICON_BGK:
                    return TransKeyConst.getItem_iconBgkTypeName_none;
                case ENPItemType.BUBBLE:
                    return TransKeyConst.getItem_bubbleTypeName_none;
                case ENPItemType.TITLE:
                    return TransKeyConst.getItem_titleTypeName_none;
            }

            return null;
        }

        //延时播放停留动画
        private void _delayPlayIdleAni()
        {
            if (wnd == null || wnd.idleAni == null)
                return;

            Action playIdleAni = () => { wnd?.idleAni?.forcePlay(); };

            if (wnd.wndAnimation != null && !string.IsNullOrEmpty(wnd.showAniName))
            {
                //在显示动画后面播放停留动画
                AnimationClip clip = wnd.wndAnimation.GetClip(wnd.showAniName);
                if (clip != null && clip.length > 0)
                {
                    long serialize = _m_lShowSerialize;
                    ALCommonActionMonoTask.addMonoTask(() =>
                    {
                        if (!isShow || serialize != _m_lShowSerialize)
                            return;

                        playIdleAni();
                    }, clip.length);
                }
                else
                {
                    playIdleAni();
                }
            }
        }

        private void _closeBtnDidClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_SHOW_OFFITEM_GET);
        }
    }
}
