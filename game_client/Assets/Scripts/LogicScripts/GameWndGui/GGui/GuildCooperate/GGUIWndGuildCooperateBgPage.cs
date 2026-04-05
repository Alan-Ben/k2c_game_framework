using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 公会协作主界面背景页面
    /// </summary>
    public class GGUIWndGuildCooperateBgPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoGuildCooperateBgPage>
    {
        //资源id
        private long _m_lUIResId;
        //区域配置
        private GuildCooperateAreaRefObj _m_areaRef;
        //随机数生成器
        private CSSyncRandom _m_random;
        //所有奖励据点父节点组合列表
        [NotNull] private List<List<RectTransform>> _m_lAllParentGroupList = new List<List<RectTransform>>();
        //目标奖励据点父节点列表
        private List<RectTransform> _m_lTargetParentList;

        public GGUIWndGuildCooperateBgPage(long _uiResId, GuildCooperateAreaRefObj _areaRef, Transform _parent) : base(_parent)
        {
            _m_lUIResId = _uiResId;
            _m_areaRef = _areaRef;
        }

        /**************
         * 窗口相关加载配置
         **/
        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_lUIResId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_lUIResId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        /// <summary>
        /// 资源id
        /// </summary>
        public long uiResId { get { return _m_lUIResId; } }
        /// <summary>
        /// 区域id
        /// </summary>
        public long areaId { get { return _m_areaRef != null ? _m_areaRef.area_id : 0; } }

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
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null || _m_areaRef == null)
                return;

            // 初始化随机数生成器
            _m_random = new CSSyncRandom(NPPlayer.instance.guildComp.guildInfo.guildId + _m_areaRef.area_id * 1000000);

            // 获取所有父节点组合
            _m_lAllParentGroupList.Clear();
            _m_lAllParentGroupList = _getAllParentGroup(wnd.goParentList, (int)_m_areaRef.reward_point_num);
            // 检查父节点组合是否有效
            _checkParentGroupIsValid();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo()
        {
            if (_m_areaRef == null || _m_random == null || _m_lAllParentGroupList.Count <= 0)
                return;
            
            if(_m_lAllParentGroupList.Count == 1)
                _m_lTargetParentList = _m_lAllParentGroupList[0];
            else
            {
                // 随机选择一个父节点组合
                int curIndex = _m_random.RandomInt(NPPlayer.instance.guildCooperateComp.resetCount, _m_lAllParentGroupList.Count - 1);
                if (_m_lAllParentGroupList.Count > curIndex)
                    _m_lTargetParentList = _m_lAllParentGroupList[curIndex];
            }
        }

        /// <summary>
        /// 根据下标获取奖励据点父节点
        /// </summary>
        /// <param name="_index"></param>
        /// <returns></returns>
        public RectTransform getRewardParentByIndex(int _index)
        {
            if (_m_lTargetParentList == null || _m_lTargetParentList.Count <= _index)
                return null;

            return _m_lTargetParentList[_index];
        }

        /// <summary>
        /// 获取全部父节点子集列表
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_pointNum"></param>
        /// <returns></returns>
        [NotNull]
        private List<List<RectTransform>> _getAllParentGroup(List<RectTransform> _list, int _pointNum)
        {
            List<List<RectTransform>> result = new List<List<RectTransform>>();
            if (_list == null || _pointNum <= 0 || _pointNum > _list.Count) 
                return result;

            _generateParentSubsets(_list, _pointNum, 0, new List<RectTransform>(), result);
            return result;
        }

        /// <summary>
        /// 设置父节点子集
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_pointNum"></param>
        /// <param name="_start"></param>
        /// <param name="_current"></param>
        /// <param name="_result"></param>
        private void _generateParentSubsets(List<RectTransform> _list, int _pointNum, int _start, List<RectTransform> _current, List<List<RectTransform>> _result)
        {
            if (_current == null || _result == null || _list == null)
                return;

            // 如果当前子集大小等于n，添加到结果
            if (_current.Count == _pointNum)
            {
                _result.Add(new List<RectTransform>(_current));
                return;
            }

            // 从start开始遍历，避免重复组合
            for (int i = _start; i < _list.Count; i++)
            {
                // 选择当前元素
                _current.Add(_list[i]);

                // 递归选择下一个元素
                _generateParentSubsets(_list, _pointNum, i + 1, _current, _result);

                // 回溯，移除当前元素
                _current.RemoveAt(_current.Count - 1);
            }
        }

        /// <summary>
        /// 检查父节点组合是否有效，将距离过近的组合剔除
        /// </summary>
        private void _checkParentGroupIsValid()
        {
            if (_m_lAllParentGroupList.Count <= 0 || _m_areaRef == null || _m_areaRef.reward_point_num <= 1)
                return;

            for (int i = _m_lAllParentGroupList.Count - 1; i >= 0; i--)
            {
                List<RectTransform> group = _m_lAllParentGroupList[i];
                if (group == null || group.Count == 0)
                    continue;

                // 是否有效标志
                bool isValid = true;

                // 检查组合内的每两个父节点距离是否有效
                for (int j = 0; j < group.Count; j++)
                {
                    RectTransform parent1 = group[j];
                    if (parent1 == null)
                        continue;

                    for (int k = j + 1; k < group.Count; k++)
                    {
                        RectTransform parent2 = group[k];
                        if (parent2 == null)
                            continue;

                        // 计算两个父节点的距离
                        float distance = Vector2.Distance(parent1.anchoredPosition, parent2.anchoredPosition);
                        // 如果小于最短距离，标记为无效
                        if (distance < GRefdataCoreMgr.instance.npGeneral.guild_cooperate_reward_point_shortest_distance)
                        {
                            isValid = false;
                            break;
                        }
                    }

                    if (!isValid)
                        break;
                }

                // 如果组合无效，移除数据
                if (!isValid)
                {
                    _m_lAllParentGroupList.RemoveAt(i);
                }
            }
        }
    }
}