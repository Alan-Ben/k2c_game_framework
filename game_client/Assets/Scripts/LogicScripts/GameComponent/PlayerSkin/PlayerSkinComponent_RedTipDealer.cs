using JetBrains.Annotations;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 玩家皮肤管理器
    /// </summary>
    public partial class PlayerSkinComponent
    {
        private class RedTipDealer
        {
            //伙伴组件
            private PlayerSkinComponent _m_heroComponent;
            //红点字典
            [NotNull] private Dictionary<string, CommonForceRedTipNode> _m_dMyNode = new Dictionary<string, CommonForceRedTipNode>(); // 为了根据自定义的索引取到对应节点的字典

            public RedTipDealer(PlayerSkinComponent _comp)
            {
                _m_heroComponent = _comp;
            }

            #region 初始化红点

            /// <summary>
            /// 初始化骑士红点管理器
            /// </summary>
            public void init()
            {
                if (_m_heroComponent == null)
                    return;

                clear();

                GRefdataCoreMgr.instance.playerSkinRefCore.dealAllRef(_skinRef =>
                {
                    _addRedTipNode(_skinRef);
                });
            }

            //初始化添加红点
            private void _addRedTipNode(PlayerSkinRefObj _skinRef)
            {
                if (_skinRef == null)
                    return;

                _addNode(RedTipConst.RED_PLAYER_SKIN, _skinRef);//骑士首次获得
            }

            //添加红点
            private void _addNode(long _parentId, PlayerSkinRefObj _skinRef)
            {
                if (_skinRef == null)
                    return;

                string nodeKey = _createRedKey(_parentId, _skinRef.id);
                if (_m_dMyNode.ContainsKey(nodeKey))
                    return;

                CommonForceRedTipNode node = new CommonForceRedTipNode(nodeKey);
                _m_dMyNode[nodeKey] = node;
                RedTipMgr.instance.addRedTipNodeWithParent(node, _parentId);
            }

            #endregion

            #region 事件变更刷新红点

            /// <summary>
            /// 刷新首次获得红点
            /// </summary>
            /// <param name="_heroInfo"></param>
            public void onGainSkin(PlayerSkinRefObj _skinRef)
            {
                if (_skinRef == null)
                    return;

                _addRedTipNode(_skinRef);

                string nodeKey = _createRedKey(RedTipConst.RED_PLAYER_SKIN, _skinRef.id);
                if (_m_dMyNode.TryGetValue(nodeKey, out CommonForceRedTipNode _node))
                    _node?.setCount(1);
            }

            #endregion

            /// <summary>
            /// 是否需要展示红点
            /// </summary>
            /// <param name="_redTipId"></param>
            /// <param name="_heroId"></param>
            /// <returns></returns>
            public bool needShowRedTip(long _redTipId, long _skinRef)
            {
                string nodeKey = _createRedKey(_redTipId, _skinRef);
                _m_dMyNode.TryGetValue(nodeKey, out CommonForceRedTipNode _node);
                return _node != null && _node.needShow();
            }

            /// <summary>
            /// 设置红点已读
            /// </summary>
            /// <param name="_redTipId"></param>
            /// <param name="_heroId"></param>
            public void setReadRedTip(long _redTipId, long _skinRef)
            {
                string nodeKey = _createRedKey(_redTipId, _skinRef);
                _m_dMyNode.TryGetValue(nodeKey, out CommonForceRedTipNode _node);
                _node?.setCount(0);
            }

            /// <summary>
            /// 清除数据
            /// </summary>
            public void clear()
            {
                _m_dMyNode.Clear();
            }

            //创建唯一key
            [NotNull]
            private string _createRedKey(long _key, long _skinRef)
            {
                return $"{_key}_{_skinRef}";
            }
        }
    }
}
