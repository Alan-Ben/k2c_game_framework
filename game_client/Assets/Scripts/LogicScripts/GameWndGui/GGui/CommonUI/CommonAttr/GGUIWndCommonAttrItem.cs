using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 属性item
    /// </summary>
    public class GGUIWndCommonAttrItem : _AGGUIWndCommonAttrItem<GGUIMonoCommonAttrItem>
    {

        public GGUIWndCommonAttrItem(GGUIMonoCommonAttrItem _mono) : base(_mono)
        {
            initWnd();
        }
    }
}