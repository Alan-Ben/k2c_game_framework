using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 权益卡属性列表
    /// </summary>
    public class GGUIWndPrivilegeCardDescContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoPrivilegeCardDescContainerItem, GGUIMonoPrivilegeCardDescContainer, GGUIWndPrivilegeCardDescContainerItem>
    {
        //item列表
        protected List<GGUIWndPrivilegeCardDescContainerItem> _m_lItemList;
        public GGUIWndPrivilegeCardDescContainer(GGUIMonoPrivilegeCardDescContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndPrivilegeCardDescContainerItem _createItemWnd(GGUIMonoPrivilegeCardDescContainerItem _itemMono)
        {
            GGUIWndPrivilegeCardDescContainerItem item = new GGUIWndPrivilegeCardDescContainerItem(_itemMono);
            return item;
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
            _m_lItemList?.Clear();
        }

        protected override void _onDiscard()
        {
            _m_lItemList?.Clear();
            _m_lItemList = null;
        }

        protected override void _onWndInitDone()
        {
            _m_lItemList = new List<GGUIWndPrivilegeCardDescContainerItem>();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(List<string> _titleList, List<DescWithParamPair> _contentList)
        {
            if (wnd == null || _titleList == null || _contentList == null || _m_lItemList == null)
                return;

            if (_titleList.Count != _contentList.Count)
            {
                Debug.LogError("【PrivilegeCard】权益描述标题和内容数量不匹配");
                return;
            }

            GGUIWndPrivilegeCardDescContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < _titleList.Count; i++)
            {
                //如果容器内部个数不足则新增视图
                if (i >= _m_lItemList.Count)
                {
                    itemWnd = addItemWnd();
                    if (null == itemWnd)
                        continue;
                    _m_lItemList.Add(itemWnd);
                }
                //如果容器个数足够，则取出
                else
                    itemWnd = _m_lItemList[i];

                itemWnd.showWnd();
                itemWnd.setInfo(TextTranslate.instance.getLanguage(_titleList[i]), TextTranslate.instance.getLanguage(_contentList[i].first(), _contentList[i].second()));
                count++;
            }

            //隐藏容器中多余的视图
            for (int j = _m_lItemList.Count - 1; j >= count; j--)
            {
                //移除窗口
                removeItemWnd(_m_lItemList[j]);
                //从队列删除
                _m_lItemList.RemoveAt(j);
            }
        }
    }
}
