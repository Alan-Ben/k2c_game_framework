using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 游戏设置切换语言列表
    /// </summary>
    public class NPGGUIWndGameSettingSwitchLanguageContainer : _ATNPGGUIWndShowAnimContainer<NPGGUIMonoGameSettingSwitchLanguageContainerItem, NPGGUIMonoGameSettingSwitchLanguageContainer, NPGGUIWndGameSettingSwitchLanguageContainerItem>
    {
        private List<NPGGUIWndGameSettingSwitchLanguageContainerItem> _m_lItemList;//item列表

        public NPGGUIWndGameSettingSwitchLanguageContainer(NPGGUIMonoGameSettingSwitchLanguageContainer _mono) : base(_mono)
        {
            _m_lItemList = new List<NPGGUIWndGameSettingSwitchLanguageContainerItem>();
            initWnd();
        }

        protected override NPGGUIWndGameSettingSwitchLanguageContainerItem _createItemWnd(NPGGUIMonoGameSettingSwitchLanguageContainerItem _itemMono)
        {
            return new NPGGUIWndGameSettingSwitchLanguageContainerItem(_itemMono);
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
            if(_m_lItemList != null)
                _m_lItemList.Clear();
            _m_lItemList = null;
        }

        protected override void _onWndInitDone()
        {
        }


        public void showItemList(List<ENPLanguage> _showLanguageList, ENPLanguage _selectLanguage, Action<ENPLanguage> _onClickItem)
        {
            if (wnd == null || _m_lItemList == null)
                return;

            NPGGUIWndGameSettingSwitchLanguageContainerItem itemWnd = null;
            int itemIdx = 0;
            if (_showLanguageList != null)
            {
                //遍历玩家数据
                for (int i = 0; i < _showLanguageList.Count; i++)
                {
                    ENPLanguage language = _showLanguageList[i];
                    
                    //如果容器内部个数不足则新增视图
                    if (itemIdx >= _m_lItemList.Count)
                    {
                        itemWnd = addItemWnd();
                        if (null == itemWnd)
                            continue;
                        _m_lItemList.Add(itemWnd);
                    }
                    //如果容器个数足够，则取出
                    else
                        itemWnd = _m_lItemList[itemIdx];

                    if (itemWnd != null)
                    {
                        itemWnd.showWnd();
                        itemWnd.setInfo(language, language == _selectLanguage, _onClickItem);
                    }

                    itemIdx++;
                }
            }

            //隐藏容器中多余的视图
            for (int j = _m_lItemList.Count - 1; j >= itemIdx; j--)
            {
                //移除窗口
                removeItemWnd(_m_lItemList[j]);
                //从队列删除
                _m_lItemList.RemoveAt(j);
            }
        }

    }
}
