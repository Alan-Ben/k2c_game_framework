using System;
using ALPackage;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 分享item列表
    /// </summary>
    public class NPGGUIWndCommonShareContainer : _ATNPGGUIWndShowAnimContainer<NPGGUIMonoCommonShareItem, NPGGUIMonoCommonShareContainer, NPGGUIWndCommonShareItem>
    {
        private ENPShareType _m_shareType;

        public NPGGUIWndCommonShareContainer(NPGGUIMonoCommonShareContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            clearAll();
        }

        protected override void _onWndInitDone()
        {
        }

        protected override NPGGUIWndCommonShareItem _createItemWnd(NPGGUIMonoCommonShareItem _itemMono)
        {
            switch (_m_shareType)
            {
                default:
                    return new NPGGUIWndCommonShareItem(_itemMono);
            }
        }

        public void showItemList(ENPShareType _type, List<long> _list,Action<long,Action> _shareAction,Func<long,bool> _isOnCDFunc)
        {
            clearAll();
            if (_list == null)
                return;
            _m_shareType = _type;
            int count = 0;
            //遍历数据
            for (int i = 0; i < _list.Count; i++)
            {
                long cid = _list[i];
                NPGGUIWndCommonShareItem itemWnd =  addItemWnd();
                if (itemWnd == null)
                    continue;
                itemWnd.showWnd();
                itemWnd.setInfo(cid, _shareAction,_isOnCDFunc);
                count++;
            }

            //空数据显示
            ALUGUICommon.setGameObjEnable(wnd.goListShowOnEmpty, count <= 0);
        }
    }
}
