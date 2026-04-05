using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    public partial class PlayerTitleComponent
    {
        /// <summary>
        /// 称号红点管理
        /// </summary>
        private class RedTipDealer
        {
            private PlayerTitleComponent _m_comp;
            [NotNull] private Dictionary<string, CommonForceRedTipNode> _m_dTitleNode = new Dictionary<string, CommonForceRedTipNode>(); 
            public RedTipDealer(PlayerTitleComponent _comp)
            {
                _m_comp = _comp;
            }

            /// <summary>
            /// 清空
            /// </summary>
            public void clear()
            {
                _m_dTitleNode.Clear();
            }

            #region 普通称号

            /// <summary>
            /// 普通称号是否需要展示红点
            /// </summary>
            /// <param name="_id"></param>
            public bool titleNeedShow(long _id)
            {
                PlayerTitleRefObj titleRef = GRefdataCoreMgr.instance.playerTitleRefCore.getRef(_id);
                if (titleRef == null)
                    return false;

                long redTipId = 0;
                if (titleRef.show_type == EPlayerTitleTabType.FIXED)
                    redTipId = RedTipConst.RED_PLAYER_TITLE_FIXED;
                else
                    redTipId = RedTipConst.RED_PLAYER_TITLE_LIMIT;

                string key= _createTitleRedKey(redTipId, _id);
                if (_m_dTitleNode.TryGetValue(key, out CommonForceRedTipNode node))
                    return node.needShow();
                return false;
            }

            /// <summary>
            /// 新增普通称号
            /// </summary>
            /// <param name="_id"></param>
            public void onAddTitle(long _id)
            {
                PlayerTitleRefObj titleRef = GRefdataCoreMgr.instance.playerTitleRefCore.getRef(_id);
                if (titleRef == null)
                    return;

                long redTipId = 0;
                if (titleRef.show_type == EPlayerTitleTabType.FIXED)
                    redTipId = RedTipConst.RED_PLAYER_TITLE_FIXED;
                else
                    redTipId = RedTipConst.RED_PLAYER_TITLE_LIMIT;

                string key = _createTitleRedKey(redTipId, _id);
                CommonForceRedTipNode newTitleNode = new CommonForceRedTipNode(key);
                RedTipMgr.instance.addRedTipNodeWithParent(newTitleNode, redTipId);
                newTitleNode.setCount(1);
                _m_dTitleNode[key] = newTitleNode;
            }

            /// <summary>
            /// 设置普通称号已读
            /// </summary>
            /// <param name="_id"></param>
            public void readTitle(long _id)
            {
                PlayerTitleRefObj titleRef = GRefdataCoreMgr.instance.playerTitleRefCore.getRef(_id);
                if (titleRef == null)
                    return;

                long redTipId = 0;
                if (titleRef.show_type == EPlayerTitleTabType.FIXED)
                    redTipId = RedTipConst.RED_PLAYER_TITLE_FIXED;
                else
                    redTipId = RedTipConst.RED_PLAYER_TITLE_LIMIT;

                string key = _createTitleRedKey(redTipId, _id);
                if (_m_dTitleNode.TryGetValue(key, out CommonForceRedTipNode node))
                    node.setCount(0);
            }

            //创建称号key
            [NotNull]
            private string _createTitleRedKey(long _key, long _subId)
            {
                return $"{_key}_{_subId}_title";
            }

            #endregion

            #region 称号前缀

            /// <summary>
            /// 称号前缀是否需要展示红点
            /// </summary>
            /// <param name="_id"></param>
            public bool prefixNeedShow(long _id)
            {
                string key = _createPrefixRedKey(RedTipConst.RED_PLAYER_TITLE_PREFIX, _id);
                if (_m_dTitleNode.TryGetValue(key, out CommonForceRedTipNode node))
                    return node.needShow();
                return false;
            }

            /// <summary>
            /// 新增称号前缀
            /// </summary>
            /// <param name="_id"></param>
            public void onAddPrefix(long _id)
            {
                string key = _createPrefixRedKey(RedTipConst.RED_PLAYER_TITLE_PREFIX, _id);
                CommonForceRedTipNode newTitleNode = new CommonForceRedTipNode(key);
                RedTipMgr.instance.addRedTipNodeWithParent(newTitleNode, RedTipConst.RED_PLAYER_TITLE_PREFIX);
                newTitleNode.setCount(1);
                _m_dTitleNode[key] = newTitleNode;
            }

            /// <summary>
            /// 设置称号前缀已读
            /// </summary>
            /// <param name="_id"></param>
            public void readPrefix(long _id)
            {
                string key = _createPrefixRedKey(RedTipConst.RED_PLAYER_TITLE_PREFIX, _id);
                if (_m_dTitleNode.TryGetValue(key, out CommonForceRedTipNode node))
                    node.setCount(0);
            }

            //创建称号前缀key
            [NotNull]
            private string _createPrefixRedKey(long _key, long _subId)
            {
                return $"{_key}_{_subId}_prefix";
            }

            #endregion

            #region 称号后缀

            /// <summary>
            /// 称号后缀是否需要展示红点
            /// </summary>
            /// <param name="_id"></param>
            public bool suffixNeedShow(long _id)
            {
                string key = _createSuffixRedKey(RedTipConst.RED_PLAYER_TITLE_SUFFIX, _id);
                if (_m_dTitleNode.TryGetValue(key, out CommonForceRedTipNode node))
                    return node.needShow();
                return false;
            }

            /// <summary>
            /// 新增称号后缀
            /// </summary>
            /// <param name="_id"></param>
            public void onAddSuffix(long _id)
            {
                string key = _createSuffixRedKey(RedTipConst.RED_PLAYER_TITLE_SUFFIX, _id);
                CommonForceRedTipNode newTitleNode = new CommonForceRedTipNode(key);
                RedTipMgr.instance.addRedTipNodeWithParent(newTitleNode, RedTipConst.RED_PLAYER_TITLE_SUFFIX);
                newTitleNode.setCount(1);
                _m_dTitleNode[key] = newTitleNode;
            }

            /// <summary>
            /// 设置称号后缀已读
            /// </summary>
            /// <param name="_id"></param>
            public void readSuffix(long _id)
            {
                string key = _createSuffixRedKey(RedTipConst.RED_PLAYER_TITLE_SUFFIX, _id);
                if (_m_dTitleNode.TryGetValue(key, out CommonForceRedTipNode node))
                    node.setCount(0);
            }

            //创建称号后缀key
            [NotNull]
            private string _createSuffixRedKey(long _key, long _subId)
            {
                return $"{_key}_{_subId}_suffix";
            }

            #endregion

            #region 称号底图

            /// <summary>
            /// 称号后缀是否需要展示红点
            /// </summary>
            /// <param name="_id"></param>
            public bool bgNeedShow(long _id)
            {
                string key = _createBgRedKey(RedTipConst.RED_PLAYER_TITLE_BG, _id);
                if (_m_dTitleNode.TryGetValue(key, out CommonForceRedTipNode node))
                    return node.needShow();
                return false;
            }

            /// <summary>
            /// 新增称号后缀
            /// </summary>
            /// <param name="_id"></param>
            public void onAddBg(long _id)
            {
                string key = _createBgRedKey(RedTipConst.RED_PLAYER_TITLE_BG, _id);
                CommonForceRedTipNode newTitleNode = new CommonForceRedTipNode(key);
                RedTipMgr.instance.addRedTipNodeWithParent(newTitleNode, RedTipConst.RED_PLAYER_TITLE_BG);
                newTitleNode.setCount(1);
                _m_dTitleNode[key] = newTitleNode;
            }

            /// <summary>
            /// 设置称号后缀已读
            /// </summary>
            /// <param name="_id"></param>
            public void readBg(long _id)
            {
                string key = _createBgRedKey(RedTipConst.RED_PLAYER_TITLE_BG, _id);
                if (_m_dTitleNode.TryGetValue(key, out CommonForceRedTipNode node))
                    node.setCount(0);
            }

            //创建称号底图key
            [NotNull]
            private string _createBgRedKey(long _key, long _subId)
            {
                return $"{_key}_{_subId}_bg";
            }

            #endregion
        }
    }
}
