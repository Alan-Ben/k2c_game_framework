
using System.Collections.Generic;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    // 烹饪模块管理类
    public partial class NPPlayerIconBgkComponent
    {
        private class RedTipDealer
        {
            private NPPlayerIconBgkComponent _m_comp;
            [NotNull] private Dictionary<string, CommonForceRedTipNode> _m_dIconBgkNode = new Dictionary<string, CommonForceRedTipNode>(); 
            public RedTipDealer(NPPlayerIconBgkComponent _comp)
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
                List<PlayerIconBgkItem> iconList = new List<PlayerIconBgkItem>();
                _m_comp.getIconBgkList(iconList);
                PlayerIconBgkItem temp = null;
                for (int i = 0; i < iconList.Count; i++)
                {
                    temp = iconList[i];
                    if (null == temp)
                        continue;
                
                    string key = _createRedKey(RedTipConst.RED_PLAYER_ICON_BGK, temp.refId);
                    CommonForceRedTipNode newIconBgkNode = new CommonForceRedTipNode(key);
                    newIconBgkNode.setCount(0);
                    RedTipMgr.instance.addRedTipNodeWithParent(newIconBgkNode, RedTipConst.RED_PLAYER_ICON_BGK);
                    _m_dIconBgkNode[key] = newIconBgkNode;
                }
            }

            /// <summary>
            /// 清空
            /// </summary>
            public void clear()
            {
                _m_dIconBgkNode.Clear();
            }

            /// <summary>
            /// 头像是否需要展示红点
            /// </summary>
            /// <param name="_id"></param>
            public bool iconBgkNeedShow(long _id)
            {
                string key = _createRedKey(RedTipConst.RED_PLAYER_ICON_BGK, _id);
                if (_m_dIconBgkNode.TryGetValue(key, out CommonForceRedTipNode node))
                    return node.needShow();
                return false;
            }

            /// <summary>
            /// 新增头像
            /// </summary>
            /// <param name="_id"></param>
            public void onAddIconBgk(long _id)
            {
                string key = _createRedKey(RedTipConst.RED_PLAYER_ICON_BGK, _id);
                CommonForceRedTipNode newIconBgkNode = new CommonForceRedTipNode(key);
                RedTipMgr.instance.addRedTipNodeWithParent(newIconBgkNode, RedTipConst.RED_PLAYER_ICON_BGK);
                newIconBgkNode.setCount(1);
                _m_dIconBgkNode[key] = newIconBgkNode;
            }

            /// <summary>
            /// 设置头像已读
            /// </summary>
            /// <param name="_id"></param>
            public void readIconBgk(long _id)
            {
                string key = _createRedKey(RedTipConst.RED_PLAYER_ICON_BGK, _id);
                if (_m_dIconBgkNode.TryGetValue(key, out CommonForceRedTipNode node))
                    node.setCount(0);
            }

            /// <summary>
            /// 设置所有头像已读
            /// </summary>
            public void readAllIconBgk()
            {
                foreach (KeyValuePair<string, CommonForceRedTipNode> node in _m_dIconBgkNode)
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
