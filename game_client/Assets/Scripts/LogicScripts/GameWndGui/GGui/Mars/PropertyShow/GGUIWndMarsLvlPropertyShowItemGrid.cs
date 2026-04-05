using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星科技等级属性展示列表
    /// </summary>
    public class GGUIWndMarsLvlPropertyShowItemGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoMarsLvlPropertyShowItem, GGUIMonoMarsLvlPropertyShowItemGrid, GGUIWndMarsLvlPropertyShowItem>
    {
        private List<MarsLvlPropertyShowInfo> _m_lLevelPropertyShowList;
        private List<_IPropertyShow> _m_lAllPropertyList;
        private Func<long, string> _m_fIntegerToStrFunc;
        private Func<float, string> _m_fDecimalsToStrFunc;
        private int _m_iCurLvl;//当前等级

        [NotNull][ItemNotNull] private List<NPGGUIWndCommonTextItem> _m_valueItemWndList = new List<NPGGUIWndCommonTextItem>();

        public GGUIWndMarsLvlPropertyShowItemGrid(GGUIMonoMarsLvlPropertyShowItemGrid _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _dealAllValueItemWnd((_itemWnd) =>
            {
                _itemWnd?.hideWnd();
            });
        }

        protected override void _onReset()
        {
            _dealAllValueItemWnd((_itemWnd) =>
            {
                _itemWnd?.resetWnd();
            });
        }

        protected override void _onDiscard()
        {
            _m_lLevelPropertyShowList = null;
            _m_lAllPropertyList = null;
            _m_fIntegerToStrFunc = null;
            _m_fDecimalsToStrFunc = null;

            _destoryAllItem();
        }

        protected override GGUIWndMarsLvlPropertyShowItem _createItemWnd(GGUIMonoMarsLvlPropertyShowItem _itemMono)
        {
            if (_itemMono == null)
                return null;
            
            return new GGUIWndMarsLvlPropertyShowItem(_itemMono);
        }

        protected override void _onRefreshItemWnd(GGUIWndMarsLvlPropertyShowItem _itemWnd, int _itemIdx)
        {
            if (_itemWnd == null || _itemIdx < 0 || _m_lLevelPropertyShowList == null || _itemIdx >= _m_lLevelPropertyShowList.Count)
                return;

            MarsLvlPropertyShowInfo showInfo = _m_lLevelPropertyShowList[_itemIdx];
            
            _itemWnd.setData(showInfo, _m_lAllPropertyList, showInfo.lvl == _m_iCurLvl, _itemIdx, _m_fIntegerToStrFunc, _m_fDecimalsToStrFunc);
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        /// <param name="_levelPropertyShowInfoList">火星科技等级属性信息列表</param>
        /// <param name="_allPropertyList">要显示的属性列表</param>
        /// <param name="_curLvl">当前等级</param>
        /// <param name="_integerToStrFunc">整数转字符串函数</param>
        /// <param name="_decimalsToStrFunc">浮点数转字符串函数</param>
        public void setData(List<MarsLvlPropertyShowInfo> _levelPropertyShowInfoList, List<_IPropertyShow> _allPropertyList, int _curLvl, Func<long, string> _integerToStrFunc = null, Func<float, string> _decimalsToStrFunc = null)
        {
            _m_lLevelPropertyShowList = _levelPropertyShowInfoList;
            _m_lAllPropertyList = _allPropertyList;
            _m_iCurLvl = _curLvl;
            _m_fIntegerToStrFunc = _integerToStrFunc;
            _m_fDecimalsToStrFunc = _decimalsToStrFunc;

            _refreshPropertyNameShow();
            
            setItemCount(_m_lLevelPropertyShowList?.Count ?? 0);
            forceRefreshAllItem();

            // 移动到当前等级位置
            int curLevelIndex = _m_lLevelPropertyShowList?.FindIndex((_shoInfo) =>
            {
                return _shoInfo.lvl == _curLvl;
            }) ?? -1;
            ALCommonTaskController.CommonActionAddNextFrameLaterTask(() =>
            {
                if(!isShow)
                    return;

                moveItemToCenter(curLevelIndex);
            });
        }


        /// <summary>
        /// 将指定索引的item移动到Grid的ViewPort中心
        /// </summary>
        /// <param name="_itemIdx">item索引</param>
        public void moveItemToCenter(int _itemIdx)
        {
            if (wnd == null || wnd.scrollRect == null || wnd.scrollRect.content == null || wnd.scrollRect.viewport == null
                || wnd.gridAreaMaskObj == null)
                return;

            if (_m_lLevelPropertyShowList == null || _itemIdx < 0 || _itemIdx >= _m_lLevelPropertyShowList.Count)
                return;

            // 获取item的位置（相对于content）
            Vector2 itemPos = getItemPos(_itemIdx);
            
            // 计算滚动位置，使item位于可视区域中心
            if (wnd.scrollRect.horizontal)
            {
                // 水平滚动
                // float contentWidth = wnd.scrollRect.content.rect.width;
                // float viewportWidth = wnd.scrollRect.viewport.rect.width;
                //
                // if (contentWidth > viewportWidth)
                // {
                //     // item中心位置x坐标
                //     float itemCenterX = -itemPos.x + wnd.itemTemplate.width * 0.5f;
                //     // 计算让item位于中心的normalized位置
                //     float normalizedX = (itemCenterX - viewportWidth * 0.5f) / (contentWidth - viewportWidth);
                //     normalizedX = Mathf.Clamp01(normalizedX);
                //     
                //     wnd.scrollRect.horizontalNormalizedPosition = normalizedX;
                // }
                
                float canMoveWidth = allHeight - wnd.gridAreaMaskObj.rect.width;
                float tmpWidth = canMoveWidth + wnd.gridAreaMaskObj.rect.width * 0.5f - itemPos.y;
                float horizontalRate = Mathf.Clamp(tmpWidth / canMoveWidth, 0f, 1f);
            
                wnd.scrollRect.horizontalNormalizedPosition = horizontalRate;
            }
            
            if (wnd.scrollRect.vertical)
            {
                // 垂直滚动
                // float contentHeight = wnd.scrollRect.content.rect.height;
                // float viewportHeight = wnd.scrollRect.viewport.rect.height;
                //
                // if (contentHeight > viewportHeight)
                // {
                //     // item中心位置y坐标（注意：Unity UI中y轴是向下的，所以是负值）
                //     float itemCenterY = itemPos.y - wnd.itemTemplate.height * 0.5f;
                //     // 计算让item位于中心的normalized位置
                //     // 注意：垂直滚动的normalizedPosition是从下到上的，需要转换
                //     float normalizedY = (contentHeight + itemCenterY - viewportHeight * 0.5f) / (contentHeight - viewportHeight);
                //     normalizedY = Mathf.Clamp01(normalizedY);
                //     
                //     wnd.scrollRect.verticalNormalizedPosition = normalizedY;
                // }
                
                float canMoveHeight = allHeight - wnd.gridAreaMaskObj.rect.height;
                float tmpHeight = -wnd.gridAreaMaskObj.rect.height * 0.5f - itemPos.y;
                float verticalRate = Mathf.Clamp(tmpHeight / canMoveHeight, 0f, 1f);
            
                wnd.scrollRect.verticalNormalizedPosition = 1 - verticalRate;
            }
        }
        
        private void _refreshPropertyNameShow()
        {
            if (_m_lAllPropertyList == null || _m_lAllPropertyList.Count <= 0)
            {
                _dealAllValueItemWnd((_itemWnd) =>
                {
                    _itemWnd?.hideWnd();
                });
                
                return;
            }

            _IPropertyShow propertyShow = null;
            int showValueItemCount = 0;
            NPGGUIWndCommonTextItem valueItemWnd = null;
            for (int i = 0, count = _m_lAllPropertyList.Count; i < count; i++)
            {
                propertyShow = _m_lAllPropertyList[i];
                if(propertyShow == null)
                    continue;
                
                if (showValueItemCount < _m_valueItemWndList.Count)
                    valueItemWnd = _m_valueItemWndList[showValueItemCount];
                else
                    valueItemWnd = _createPropertyTextItem();
                showValueItemCount++;
                
                if(valueItemWnd == null)
                    continue;
                
                valueItemWnd.showWnd();
                valueItemWnd.setTxt(TextTranslate.instance.getLanguage(propertyShow.simpleName), TextTranslate.instance.getLanguage(propertyShow.simpleName), null, false);
            }
            
            for(int i = showValueItemCount; i < _m_valueItemWndList.Count; i++)
            {
                valueItemWnd = _m_valueItemWndList[i];
                if(valueItemWnd == null)
                    continue;
                
                valueItemWnd.hideWnd();
            }
        }
        
        #region propertyNameItem
        
        /// <summary>
        /// 创建属性文本项
        /// </summary>
        private NPGGUIWndCommonTextItem _createPropertyTextItem()
        {
            if (wnd == null || wnd.monoPropertyNameItemPrefab == null || wnd.propertyNameItemParent == null)
                return null;

            //实例化一个子窗口对象
            NPGGUIMonoCommonTextItem itemWndObj = _instantiateItemMono(wnd.monoPropertyNameItemPrefab);
            //创建一个子窗口管理对象
            NPGGUIWndCommonTextItem itemWnd = new NPGGUIWndCommonTextItem(itemWndObj);
            if(null == itemWnd.wnd)
            {
                //创建对象无效，删除创建对象资源并退出
                _releaseItemMono(itemWndObj);
                return null;
            }

            //将子窗口添加到容器中
            itemWnd.wnd.transform.SetParent(wnd.propertyNameItemParent);
            itemWnd.wnd.transform.localPosition = Vector3.zero;
            itemWnd.wnd.transform.localScale = Vector3.one;
            itemWnd.wnd.transform.localRotation = Quaternion.identity;

            //调用子窗口的初始化函数
            itemWnd.initWnd();

            //添加到数据集合
            _m_valueItemWndList.Add(itemWnd);

            return itemWnd;
        }
        
        private void _destoryItemWnd(NPGGUIWndCommonTextItem _itemWnd)
        {
            NPGGUIWndCommonTextItem tmpWnd = null;
            //遍历查找，匹配成功则跳出循环
            for (int i = 0; i < _m_valueItemWndList.Count; i++)
            {
                tmpWnd = _m_valueItemWndList[i];
                if (null == tmpWnd)
                    continue;

                if (tmpWnd == _itemWnd)
                {
                    //移除对应位置节点
                    _m_valueItemWndList.RemoveAt(i);
                    break;
                }
            }

            //判断数据是否有效
            if (_itemWnd != tmpWnd || null == tmpWnd)
                return;

            NPGGUIMonoCommonTextItem wndMono = tmpWnd.wnd;
            //使子窗口的位置可以正确显示
            tmpWnd.wnd.transform.SetParent(null);
            //释放窗口对象
            tmpWnd.discard();

            //删除对应的对象
            _releaseItemMono(wndMono);
        }
        
        private void _destoryAllItem()
        {
            //释放物品格的对象
            foreach (NPGGUIWndCommonTextItem itemWnd in _m_valueItemWndList)
            {
                if (itemWnd == null)
                    continue;
                //使子窗口的位置可以正确显示
                if (null != itemWnd.wnd)
                    itemWnd.wnd.transform.SetParent(null);

                NPGGUIMonoCommonTextItem wndMono = itemWnd.wnd;
                itemWnd.discard();
                
                //删除对应的对象
                _releaseItemMono(wndMono);
            }
            //清空数据集
            _m_valueItemWndList.Clear();
        }
        
        /***************
         * 创建本对象的脚本对象实例
         **/
        private NPGGUIMonoCommonTextItem _instantiateItemMono (NPGGUIMonoCommonTextItem _template) 
        {
            if(null == _template)
                return null;

            //实例化一个子窗口对象
            return GameObject.Instantiate(_template) as NPGGUIMonoCommonTextItem;
        }

        /***************
         * 创建本对象的脚本对象实例
         **/
        private void _releaseItemMono (NPGGUIMonoCommonTextItem _mono) 
        {
            if(null == _mono)
                return ;

            //删除对应的对象
            ALUnityCommon.releaseGameObj(_mono);
        }

        private void _dealAllValueItemWnd(Action<NPGGUIWndCommonTextItem> _action)
        {
            if(_action == null)
                return;

            _m_valueItemWndList.ForEach(_action);
        }
        
        #endregion
    }
}
