using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星基地民意求助 列表容器
    /// 使用 DiffPrefab 容器基类，支持后续按需要以不同预制实例化项。
    /// </summary>
    public class GGUIWndMarsPopularWillHelpContainer : _ANPGGUIBasicDiffPrefabSubWndContainer<GGUIMonoMarsPopularWillHelpItem, GGUIMonoMarsPopularWillHelpContainer, GGUIWndMarsPopularWillHelpItem>
    {
        public GGUIWndMarsPopularWillHelpContainer(GGUIMonoMarsPopularWillHelpContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_MARS_POPULAR_WILL_HELP_ITEM, _onSimulateClickHelpItem);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_MARS_POPULAR_WILL_HELP_ITEM, _onSimulateClickHelpItem);
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override GGUIWndMarsPopularWillHelpItem _createItemWnd(GGUIMonoMarsPopularWillHelpItem _itemMono)
        {
            GGUIWndMarsPopularWillHelpItem itemWnd = new GGUIWndMarsPopularWillHelpItem(_itemMono);
            return itemWnd;
        }

        protected override void _onAddItemWnd(GGUIWndMarsPopularWillHelpItem _itemWnd)
        {
        }

        public void setData(List<_IMarsPeopleWillHelp> _helpList)
        {
            // 因为不同item可能会有不同的预制，所以这里直接清除所有子窗口
            clearAll();

            if(wnd == null)
                return;
            
            if (_helpList == null || _helpList.Count <= 0)
            {
                ALUGUICommon.setGameObjEnable(wnd.noItemShow, true);
                return;
            }

            ALUGUICommon.setGameObjEnable(wnd.noItemShow, false);
            
            ALProcess process = ALProcess.CreateProcess();
            foreach (var helpItem in _helpList)
            {
                if(helpItem == null || helpItem.refObj == null || helpItem.refObj.item_prefab_asset_path == null || !helpItem.refObj.item_prefab_asset_path.enable)
                    continue;

                process.addDelegateProcess((_complete) =>
                {
                    addItemWndByAssetPathInfo(helpItem.refObj.item_prefab_asset_path, (_itemWnd) =>
                    {
                        if (_itemWnd != null)
                        {
                            _itemWnd.showWnd();
                            _itemWnd.setData(helpItem);
                        }

                        _complete?.Invoke();
                    });
                });
            }
            
            process.deal();
        }
        
        private void _onSimulateClickHelpItem(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1)
                return;

            int clickItemIndex = 0;
            if (_objs[0] is string indexStr)
            {
                int.TryParse(indexStr, out clickItemIndex);
            }
            
            GGUIWndMarsPopularWillHelpItem clickItem = getItemWnd(clickItemIndex);
            clickItem?.simulateClick();
        }
    }
}
