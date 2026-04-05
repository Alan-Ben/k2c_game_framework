
using System.Collections.Generic;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    // 烹饪模块管理类
    public partial class NPPlayerBubbleComponent
    {
        private class RedTipDealer
        {
            private NPPlayerBubbleComponent _m_comp;
            [NotNull] private Dictionary<string, CommonForceRedTipNode> _m_dBubbleNode = new Dictionary<string, CommonForceRedTipNode>(); 
            public RedTipDealer(NPPlayerBubbleComponent _comp)
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
                List<NPPlayerBubbleItem> iconList = new List<NPPlayerBubbleItem>();
                _m_comp.getBubbleList(iconList);
                NPPlayerBubbleItem temp = null;
                for (int i = 0; i < iconList.Count; i++)
                {
                    temp = iconList[i];
                    if (null == temp)
                        continue;
                
                    string key = _createRedKey(RedTipConst.RED_PLAYER_BUBBLE, temp.refId);
                    CommonForceRedTipNode newBubbleNode = new CommonForceRedTipNode(key);
                    newBubbleNode.setCount(0);
                    RedTipMgr.instance.addRedTipNodeWithParent(newBubbleNode, RedTipConst.RED_PLAYER_BUBBLE);
                    _m_dBubbleNode[key] = newBubbleNode;
                }
            }

            /// <summary>
            /// 清空
            /// </summary>
            public void clear()
            {
                _m_dBubbleNode.Clear();
            }

            /// <summary>
            /// 头像是否需要展示红点
            /// </summary>
            /// <param name="_id"></param>
            public bool bubbleNeedShow(long _id)
            {
                string key = _createRedKey(RedTipConst.RED_PLAYER_BUBBLE, _id);
                if (_m_dBubbleNode.TryGetValue(key, out CommonForceRedTipNode node))
                    return node.needShow();
                return false;
            }

            /// <summary>
            /// 新增头像
            /// </summary>
            /// <param name="_id"></param>
            public void onAddBubble(long _id)
            {
                string key = _createRedKey(RedTipConst.RED_PLAYER_BUBBLE, _id);
                CommonForceRedTipNode newBubbleNode = new CommonForceRedTipNode(key);
                RedTipMgr.instance.addRedTipNodeWithParent(newBubbleNode, RedTipConst.RED_PLAYER_BUBBLE);
                newBubbleNode.setCount(1);
                _m_dBubbleNode[key] = newBubbleNode;
            }

            /// <summary>
            /// 设置头像已读
            /// </summary>
            /// <param name="_id"></param>
            public void readBubble(long _id)
            {
                string key = _createRedKey(RedTipConst.RED_PLAYER_BUBBLE, _id);
                if (_m_dBubbleNode.TryGetValue(key, out CommonForceRedTipNode node))
                    node.setCount(0);
            }

            /// <summary>
            /// 设置所有头像已读
            /// </summary>
            public void readAllBubble()
            {
                foreach (KeyValuePair<string, CommonForceRedTipNode> node in _m_dBubbleNode)
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
