using System.Collections.Generic;
using System.Linq;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 分组按照优先级显示的groupitem的管理器，
    /// </summary>
    public class GGUICustomShowGroupItemMgr
    {
        protected static GGUICustomShowGroupItemMgr _g_instance = new GGUICustomShowGroupItemMgr();

        public static GGUICustomShowGroupItemMgr instance
        { 
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUICustomShowGroupItemMgr();
                return _g_instance;
            }
        }

        private bool _m_needRefresh = false;
        private List<int> _m_needRefreshGroupIdList = new List<int>();
        private Dictionary<int, List<GGUICustomMonoShowGroupItem>> _m_allGroupDic = new Dictionary<int, List<GGUICustomMonoShowGroupItem>>();

        public GGUICustomShowGroupItemMgr()
        {
        
        }

        private List<GGUICustomMonoShowGroupItem> _getGroupItemList(int _groupId)
        {
            if (null == _m_allGroupDic)
                return null;
            if (_m_allGroupDic.ContainsKey(_groupId))
                return _m_allGroupDic[_groupId];

            return null;
        }

        /// <summary>
        /// 新增脚本item
        /// </summary>
        /// <param name="_groupItem"></param>
        public void addItem(GGUICustomMonoShowGroupItem _groupItem)
        {
            if (null == _groupItem)
                return;
            List<GGUICustomMonoShowGroupItem> groupItemList = _getGroupItemList(_groupItem.groupId);
            if (null == groupItemList)
            {
                groupItemList = new List<GGUICustomMonoShowGroupItem>();
                _m_allGroupDic.Add(_groupItem.groupId, groupItemList);
            }
            groupItemList.Insert(0,_groupItem);
            groupItemList.Sort(_sortGroupItemList);

            if(!_m_needRefreshGroupIdList.Contains(_groupItem.groupId))
                _m_needRefreshGroupIdList.Add(_groupItem.groupId);
            if (_m_needRefresh)
                return;
            _m_needRefresh = true;
            _refreshNextFrame();
        }

        /// <summary>
        /// 对脚本列表排序
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int _sortGroupItemList(GGUICustomMonoShowGroupItem x, GGUICustomMonoShowGroupItem y)
        {
            //按照优先级排序
            if (x.priority < y.priority)
                return -1;
            if (x.priority > y.priority)
                return 1;
            return 0;
        }

        /// <summary>
        /// 移除脚本item
        /// </summary>
        /// <param name="_groupItem"></param>
        public void removeItem(GGUICustomMonoShowGroupItem _groupItem)
        {
            List<GGUICustomMonoShowGroupItem> groupItemList = _getGroupItemList(_groupItem.groupId);
            if (null != groupItemList)
            {
                groupItemList.Remove(_groupItem);
            }

            if(!_m_needRefreshGroupIdList.Contains(_groupItem.groupId))
                _m_needRefreshGroupIdList.Add(_groupItem.groupId);
            if (_m_needRefresh)
                return;
            _m_needRefresh = true;
            _refreshNextFrame();
        }

        /// <summary>
        /// 执行下一帧刷新
        /// </summary>
        private void _refreshNextFrame()
        {
            if (!_m_needRefresh)
                return;
            
            ALCommonTaskController.CommonActionAddNextFrameTask(() =>
            {
                foreach (int groupId in _m_needRefreshGroupIdList)
                {
                    List<GGUICustomMonoShowGroupItem> groupItemList = _getGroupItemList(groupId);
                    if (null != groupItemList)
                    {
                        GGUICustomMonoShowGroupItem item = null;
                        for (int i = 0; i < groupItemList.Count; i++)
                        {
                            item = groupItemList[i];
                            item?.setItemShow(i == 0);
                        }
                    }
                }
                    
                _m_needRefreshGroupIdList.Clear();
                _m_needRefresh = false;
            });
            
        }
    }
}