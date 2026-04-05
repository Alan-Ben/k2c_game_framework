using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 情人皮肤item容器
    /// </summary>
    public class GGUIWndConsortSkinItemContainer : _ATNPGGUIWndSingleChoiceContainer<GGUIMonoConsortSkinItem, GGUIMonoConsortSkinItemContainer, GGUIWndConsortSkinItem>
    {
        [NotNull] private List<GGUIWndConsortSkinItem> _m_lItemGroupList = new List<GGUIWndConsortSkinItem>();
        public GGUIWndConsortSkinItemContainer(GGUIMonoConsortSkinItemContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onShowWndEx()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_CONSORT_SKIN_INFO_ADD,_onConsortSkinChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_CONSORT_SKIN_INFO_CHG,_onConsortSkinChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_CONSORT_CUR_SKIN_INFO_CHG,_onConsortSkinChg);
            // WinMsg.RegisterMsgAct(WinMsgType.ON_RED_TIP_CHANGE,_onRedTipChg);
        }

        protected override void _onHideWndEx()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CONSORT_SKIN_INFO_ADD,_onConsortSkinChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CONSORT_SKIN_INFO_CHG,_onConsortSkinChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CONSORT_CUR_SKIN_INFO_CHG,_onConsortSkinChg);
            // WinMsg.UnregisterMsgAct(WinMsgType.ON_RED_TIP_CHANGE,_onRedTipChg);

            foreach (GGUIWndConsortSkinItem iconItem in _m_lItemGroupList)
                iconItem?.hideWnd();
        }

        protected override void _onResetEx()
        {
            foreach (GGUIWndConsortSkinItem iconItem in _m_lItemGroupList)
                iconItem?.resetWnd();
        }

        protected override void _onDiscardEx()
        {
            _m_lItemGroupList.Clear();
        }

        protected override void _onWndInitDoneEx()
        {
        }

        protected override GGUIWndConsortSkinItem _createItemWnd(GGUIMonoConsortSkinItem _itemMono)
        {
            GGUIWndConsortSkinItem itemWnd = new GGUIWndConsortSkinItem(_itemMono);
            return itemWnd;
        }

        public void showItemList(List<GConsortSkinRefObj> _skinList)
        {
            if (_skinList == null)
            {
                foreach (GGUIWndConsortSkinItem itemWnd in _m_lItemGroupList)
                {
                    itemWnd?.hideWnd();
                }
                return;
            }
            
            GGUIWndConsortSkinItem tempItemWnd = null;
            GConsortSkinRefObj itemData = null;
            int count = 0;
            for (int i = 0; i < _skinList.Count; i++)
            {
                itemData = _skinList[i];
                if (null == itemData)
                    continue;

                if (count >= _m_lItemGroupList.Count)
                {
                    tempItemWnd = addItemWnd();
                    if (tempItemWnd == null)
                        continue;
                    //放入数据队列
                    _m_lItemGroupList.Add(tempItemWnd);
                }
                else
                {
                    tempItemWnd = _m_lItemGroupList[count];
                    if (tempItemWnd == null)
                    {
                        tempItemWnd = addItemWnd();
                        _m_lItemGroupList[count] = tempItemWnd;
                    }
                    
                    if (tempItemWnd == null)
                        continue;
                }
                
                tempItemWnd.showWnd();
                tempItemWnd.setInfo(itemData);
                count++;
            }

            for (int i = _m_lItemGroupList.Count; i > count; i--)
            {
                removeItemWnd(_m_lItemGroupList[i - 1]);
                _m_lItemGroupList.RemoveAt(i - 1);
            }
        }

        /// <summary>
        /// 设置选中
        /// </summary>
        /// <param name="_refObj"></param>
        public void setSelect(GConsortSkinRefObj _refObj, bool _needCallBack)
        {
            if (_refObj == null)
                return;

            for (int i = 0; i < _m_lItemGroupList.Count; i++)
            {
                if (_m_lItemGroupList[i] != null && _m_lItemGroupList[i].skinRef != null &&
                    _m_lItemGroupList[i].skinRef.id == _refObj.id)
                {
                    if(_needCallBack)
                        setSelectItem(_m_lItemGroupList[i]);
                    else
                        setSelectItemWithOutCallBack(_m_lItemGroupList[i]);
                    
                    break;
                }
            }
        }
        
        /// <summary>
        /// 刷新容器布局
        /// </summary>
        public void _refreshContentLayout()
        {
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                if (wnd == null || wnd.itemContainer == null)
                    return;
        
                LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.itemContainer.GetComponent<RectTransform>());
            });
        }
        
        /// <summary>
        /// 妃位皮肤有变化
        /// </summary>
        private void _onConsortSkinChg()
        { 
            foreach (GGUIWndConsortSkinItem skinItem in _m_lItemGroupList)
            {
                skinItem.refreshWnd();
            }
        }

        // private void _onRedTipChg()
        // {
        //     
        //     foreach (GGUIWndConsortSkinItem skinItem in _m_lItemGroupList)
        //     {
        //         skinItem.refreshRedWnd();
        //     }
        // }
        
        
    }
}