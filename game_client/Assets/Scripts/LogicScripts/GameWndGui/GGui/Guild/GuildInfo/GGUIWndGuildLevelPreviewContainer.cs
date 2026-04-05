using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 联盟等级预览信息列表
    /// </summary>
    public class GGUIWndGuildLevelPreviewContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoGuildLevelPreviewContainerItem, GGUIMonoGuildLevelPreviewContainer, GGUIWndGuildLevelPreviewContainerItem>
    {
        //窗口容器
        protected List<GGUIWndGuildLevelPreviewContainerItem> _m_lItemList;

        public GGUIWndGuildLevelPreviewContainer(GGUIMonoGuildLevelPreviewContainer _containerMono) : base(_containerMono)
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
            _m_lItemList?.Clear();
        }

        protected override void _onDiscard()
        {
            _m_lItemList?.Clear();
            _m_lItemList = null;
        }

        protected override void _onWndInitDone()
        {
            _m_lItemList = new List<GGUIWndGuildLevelPreviewContainerItem>();
        }

        protected override GGUIWndGuildLevelPreviewContainerItem _createItemWnd(GGUIMonoGuildLevelPreviewContainerItem _itemMono)
        {
            // 创建对象
            GGUIWndGuildLevelPreviewContainerItem item = new GGUIWndGuildLevelPreviewContainerItem(_itemMono);
            return item;
        }

        /// <summary>
        /// 设置展示信息
        /// </summary>
        /// <param name="_levelRef"></param>
        public void showItemList(GuildLevelRefObj _levelRef)
        {
            if (_levelRef == null)
                return;

            List<string> descList = new List<string>();
            List<string> valueList = new List<string>();
            if(_levelRef.preview_desc_list != null)
                descList.AddRange(_levelRef.preview_desc_list);
            if(_levelRef.preview_desc_value_list != null)
                valueList.AddRange(_levelRef.preview_desc_value_list);

            if (descList.Count != valueList.Count)
            {
                Debug.LogError($"【联盟等级预览】等级效果描述参数和值数量不一致，level:{_levelRef.level}");
                return;
            }

            GGUIWndGuildLevelPreviewContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < descList.Count; i++)
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
                itemWnd.setInfo(descList[i], valueList[i]);
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
