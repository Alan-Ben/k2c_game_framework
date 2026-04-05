using ALPackage;
using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    // 通用下拉框容器
    public class NPGGUIWndCommonDropDownBoxContainer : _ATNPGGUIWndDropDownBoxContainer<NPGGUIMonoDropDownBoxItem, NPGGUIMonoDropDownBoxContainer, NPGGUIWndCommonDropDownBoxItem>
    {
        public NPGGUIWndCommonDropDownBoxContainer(NPGGUIMonoDropDownBoxContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override NPGGUIWndCommonDropDownBoxItem _createItemWndExt(NPGGUIMonoDropDownBoxItem _itemMono)
        {
            return new NPGGUIWndCommonDropDownBoxItem(_itemMono);
        }
        
        /// <summary>
        /// 设置item是否选中
        /// </summary>
        public void setItemIsSelect(long _id, bool _isSelect)
        {
            if(null == _m_lItemList)
                return;

            NPGGUIWndCommonDropDownBoxItem itemWnd = getItem(_id);
            if (null != itemWnd)
            {
                itemWnd.setIsItemSelected(_isSelect);
            }
        }
    }
}
