
using System.Collections.Generic;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    // 烹饪模块管理类
    public partial class NPPlayerIconComponent
    {
        private class RedTipDealer
        {
            private NPPlayerIconComponent _m_comp;
            [NotNull] private Dictionary<string, CommonForceRedTipNode> _m_dIconNode = new Dictionary<string, CommonForceRedTipNode>(); //是否头像红点
            public RedTipDealer(NPPlayerIconComponent _comp)
            {
                _m_comp = _comp;
            }

            /// <summary>
            /// 初始化
            /// </summary>
            public void init()
            {
                if (null == _m_comp)
                    return;

                //初始化avatar字典
                List<NPPlayerIconItem> iconList = new List<NPPlayerIconItem>();
                _m_comp.getIconList(iconList);
                NPPlayerIconItem temp = null;
                for (int i = 0; i < iconList.Count; i++)
                {
                    temp = iconList[i];
                    if (null == temp)
                        continue;
                
                    string key = _createRedKey(RedTipConst.RED_PLAYER_ICON, temp.refId);
                    CommonForceRedTipNode newIconNode = new CommonForceRedTipNode(key);
                    newIconNode.setCount(0);
                    RedTipMgr.instance.addRedTipNodeWithParent(newIconNode, RedTipConst.RED_PLAYER_ICON);
                    _m_dIconNode[key] = newIconNode;
                }
            }

            /// <summary>
            /// 清空
            /// </summary>
            public void clear()
            {
                _m_dIconNode.Clear();
            }

            /// <summary>
            /// 头像是否需要展示红点
            /// </summary>
            /// <param name="_id"></param>
            public bool iconNeedShow(long _id)
            {
                string key = _createRedKey(RedTipConst.RED_PLAYER_ICON, _id);
                if (_m_dIconNode.TryGetValue(key, out CommonForceRedTipNode node))
                    return node.needShow();
                return false;
            }

            /// <summary>
            /// 新增头像
            /// </summary>
            /// <param name="_id"></param>
            public void onAddIcon(long _id)
            {
                string key = _createRedKey(RedTipConst.RED_PLAYER_ICON, _id);
                CommonForceRedTipNode newIconNode = new CommonForceRedTipNode(key);
                RedTipMgr.instance.addRedTipNodeWithParent(newIconNode, RedTipConst.RED_PLAYER_ICON);
                newIconNode.setCount(1);
                _m_dIconNode[key] = newIconNode;
            }

            /// <summary>
            /// 设置头像已读
            /// </summary>
            /// <param name="_id"></param>
            public void readIcon(long _id)
            {
                string key = _createRedKey(RedTipConst.RED_PLAYER_ICON, _id);
                if (_m_dIconNode.TryGetValue(key, out CommonForceRedTipNode node))
                    node.setCount(0);
            }

            /// <summary>
            /// 设置所有头像已读
            /// </summary>
            public void readAllIcon()
            {
                foreach (KeyValuePair<string, CommonForceRedTipNode> node in _m_dIconNode)
                {
                    node.Value.setCount(0);
                }
            }

            private string _createRedKey(long _key, long _subId)
            {
                return $"{_key}_{_subId}";
            }

        }
    }
}
