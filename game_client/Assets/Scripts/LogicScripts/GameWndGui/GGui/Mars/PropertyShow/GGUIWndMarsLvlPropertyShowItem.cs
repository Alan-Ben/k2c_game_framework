using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星科技等级属性展示信息
    /// </summary>
    public struct MarsLvlPropertyShowInfo
    {
        public int lvl;
        
        public Dictionary<_IPropertyShow, long> propertyValueDic;
    }
    
    /// <summary>
    /// 火星科技等级属性展示Item
    /// </summary>
    public class GGUIWndMarsLvlPropertyShowItem : _ANPGGUIBasicGridItemWnd<GGUIMonoMarsLvlPropertyShowItem>
    {
        private List<_IPropertyShow> _m_lShowPropertyList;
        private MarsLvlPropertyShowInfo _m_currentShowInfo;
        private bool _m_bIsCurLvl = false;//是否当前等级显示项
        private int _m_iInListIndex = -1;//在列表中的索引
        private Func<long, string> _m_fIntegerToStrFunc;//整数转字符串的函数
        private Func<float, string> _m_fDecimalsToStrFunc;//浮点数转字符串的函数
        
        [NotNull][ItemNotNull] private List<NPGGUIWndCommonTextItem> _m_valueItemWndList = new List<NPGGUIWndCommonTextItem>();

        public GGUIWndMarsLvlPropertyShowItem(GGUIMonoMarsLvlPropertyShowItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

        }

        protected override void _onDiscard()
        {
            _destoryAllItem();
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

        protected override void _resetGridItem()
        {
        }

        public void setData(MarsLvlPropertyShowInfo _showInfo, List<_IPropertyShow> _propertyList, bool _isCurLvl, int _inListIndex, Func<long, string> _integerToStrFunc = null, Func<float, string> _decimalsToStrFunc = null)
        {
            _m_currentShowInfo = _showInfo;
            _m_lShowPropertyList = _propertyList;
            _m_bIsCurLvl = _isCurLvl;
            _m_iInListIndex = _inListIndex;
            _m_fIntegerToStrFunc = _integerToStrFunc;
            _m_fDecimalsToStrFunc = _decimalsToStrFunc;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null)
                return;
            
            string lvlStr = String.Empty;
            if(string.IsNullOrEmpty(wnd.txtLvlKey))
                lvlStr = _m_currentShowInfo.lvl.ToString();
            else
                lvlStr = TextTranslate.instance.getLanguage(wnd.txtLvlKey, _m_currentShowInfo.lvl);
            if(wnd.needChgLvlColor)
                lvlStr = GCommon.addColorForRichText(lvlStr, _m_bIsCurLvl ? wnd.curLvlTxtColor : wnd.otherLvlTxtColor);
            ALUGUICommon.setLabelTxt(wnd.txtLvl, lvlStr);

            if (wnd.inListCyclicIndexShowGoList != null && wnd.inListCyclicIndexShowGoList.Count > 0)
            {
                int index = _m_iInListIndex % wnd.inListCyclicIndexShowGoList.Count;
                for (int i = 0; i < wnd.inListCyclicIndexShowGoList.Count; i++)
                {
                    ALUGUICommon.setGameObjEnable(wnd.inListCyclicIndexShowGoList[i], i == index);
                }
            }
            
            _refreshPropertyShow();
            
            ALUGUICommon.setGameObjEnable(wnd.curLvlShow, _m_bIsCurLvl);
            ALUGUICommon.setGameObjEnable(wnd.curLvlHide, !_m_bIsCurLvl);
        }

        private void _refreshPropertyShow()
        {
            if (_m_lShowPropertyList == null || _m_lShowPropertyList.Count <= 0)
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
            for (int i = 0, count = _m_lShowPropertyList.Count; i < count; i++)
            {
                propertyShow = _m_lShowPropertyList[i];
                if(propertyShow == null)
                    continue;
                
                long propertyValue = 0;
                _m_currentShowInfo.propertyValueDic?.TryGetValue(propertyShow, out propertyValue);

                if (showValueItemCount < _m_valueItemWndList.Count)
                    valueItemWnd = _m_valueItemWndList[showValueItemCount];
                else
                    valueItemWnd = _createPropertyTextItem();
                showValueItemCount++;
                
                if(valueItemWnd == null)
                    continue;
                
                _refreshPropertyShowItem(propertyShow, propertyValue, valueItemWnd);
            }
            
            for(int i = showValueItemCount; i < _m_valueItemWndList.Count; i++)
            {
                valueItemWnd = _m_valueItemWndList[i];
                if(valueItemWnd == null)
                    continue;
                
                valueItemWnd.hideWnd();
            }
        }

        private void _refreshPropertyShowItem(_IPropertyShow _property, long _value, NPGGUIWndCommonTextItem _itemWnd)
        {
            if (_itemWnd == null)
                return;

            string _valueStr = string.Empty;
            if (_property?.isAddPer ?? false)
            {
                float perValue = _value / 100f;
                if (_m_fDecimalsToStrFunc != null)
                {
                    _valueStr = _m_fDecimalsToStrFunc(perValue);
                }
                else//默认显示方式
                {
                    _valueStr = perValue.ToString("F2");
                    if (_value >= 0)//若大于0, 前面添加+号
                    {
                        _valueStr = TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _valueStr);
                    }
                    _valueStr = TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, _valueStr);
                }
            }
            else
            {
                if (_m_fIntegerToStrFunc != null)
                {
                    _valueStr = _m_fIntegerToStrFunc(_value);
                }
                else
                {
                    _valueStr = _value.ToString();
                    if (_value >= 0)//若大于0, 前面添加+号
                    {
                        _valueStr = TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _valueStr);
                    }
                }
            }

            if (wnd != null && wnd.needChgLvlColor)
                _valueStr = GCommon.addColorForRichText(_valueStr, _m_bIsCurLvl ? wnd.curLvlTxtColor : wnd.otherLvlTxtColor);
            
            _itemWnd.showWnd();
            _itemWnd.setTxt(_valueStr, _valueStr, null, false);
        }
        
        #region propertyValueItem
        
        /// <summary>
        /// 创建属性文本项
        /// </summary>
        private NPGGUIWndCommonTextItem _createPropertyTextItem()
        {
            if (wnd == null || wnd.monoPropertyValueItemPrefab == null || wnd.propertyValueItemParent == null)
                return null;

            //实例化一个子窗口对象
            NPGGUIMonoCommonTextItem itemWndObj = _instantiateItemMono(wnd.monoPropertyValueItemPrefab);
            //创建一个子窗口管理对象
            NPGGUIWndCommonTextItem itemWnd = new NPGGUIWndCommonTextItem(itemWndObj);
            if(null == itemWnd.wnd)
            {
                //创建对象无效，删除创建对象资源并退出
                _releaseItemMono(itemWndObj);
                return null;
            }

            //将子窗口添加到容器中
            itemWnd.wnd.transform.SetParent(wnd.propertyValueItemParent);
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