using ALPackage;
using SQLite4Unity3d;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// NPC主表
    /// </summary>
    [Serializable]
    public class ChapterRefObj
    {
        public long chapter_id;//唯一id
        public string name;
        public string name_args;//NPC类型字符串
        public string desc;
        public string desc_args;
        public long initial_power;//战力需求
        public string power_add_rate;//战力需求递增比例列表（万分比）
        public long initial_cost;//金币前进消耗(SILVER)
        public string cost_add_rate;//金币前进消耗递增比例列表（万分比）
        public int point_count;//一共有几波(包括boss)
        public string after_point_dialog;//第几波波后剧情对话id[pointId:dialogId:tipUiResID:npcRefId]

        public long boss_start_dialog;//boss战前的对话id
        public long boss_end_dialog;//boss战结束后对话id
        public string comment_random_group_id;//第几波弹幕评论随机组
        public long boss_power;//boss战力
        public string reward_list;//boss战奖励
        public string reward_show;//boss战奖励预览
        public long gold_inspire_base_value;//boss战金币鼓舞基础值
        public string crit_probability;//每一波暴击概率
        public long boss_style_id;//boss样式id
        public int reward_player_exp;//关卡前进奖励经验
        public long forward_simple_unlock_id;//前进条件
        public long forward_tutorial_id;//前进条件的引导id
        public string chapter_img;//切图

        public string node_list;//关卡节点列表
        public string chapter_map_res_index;//关卡地图资源索引
        public long enter_chapter_dialog_id;//进入章节的对话id
        public long chapter_bg_music_id;//章节背景音乐id
        public long chapter_boss_bg_music_id;//章节boss战背景音乐id
        public string video_boss_bg_go_path;//视频背景资源

        
        public long Id { get { return chapter_id; } set { chapter_id = value; } }
        public string Name { get { return name; } set { name = value; } }
        public string NameArgs { get { return name_args; } set { name_args = value; } }
        public string Desc { get { return desc; } set { desc = value; } }
        public string DescArgs { get { return desc_args; } set { desc_args = value; } }
        public long InitialPower { get { return initial_power; } set { initial_power = value; } }
        public string PowerAddRate { get { return power_add_rate; } set { power_add_rate = value; } }
        public long InitialCost { get { return initial_cost; } set { initial_cost = value; } }
        public string CostAddRate { get { return cost_add_rate; } set { cost_add_rate = value; } }
        public int PointCount { get { return point_count; } set { point_count = value; } }
        public string AfterPointDialog { get { return after_point_dialog; } set { after_point_dialog = value; } }
        public string CommentRandomGroupId { get { return comment_random_group_id; } set { comment_random_group_id = value; } }
        public long BossPower { get { return boss_power; } set { boss_power = value; } }
        public string RewardList { get { return reward_list; } set { reward_list = value; } }
        public long GoldInspireBaseValue { get { return gold_inspire_base_value; } set { gold_inspire_base_value = value; } }
        public string CritProbability { get { return crit_probability; } set { crit_probability = value; } }
        public long BossStyleId { get { return boss_style_id; } set { boss_style_id = value; } }
        public string RewardShow { get { return reward_show; } set { reward_show = value; } }
        public int RewardPlayerExp { get { return reward_player_exp; } set { reward_player_exp = value; } }
        public long BossEndDialog { get { return boss_end_dialog; } set { boss_end_dialog = value; } }
        public long BossStartDialog { get { return boss_start_dialog; } set { boss_start_dialog = value; } }
        public long ForwardSimpleUnlockId { get { return forward_simple_unlock_id; } set { forward_simple_unlock_id = value; } }
        public long ForwardTutorialId { get { return forward_tutorial_id; } set { forward_tutorial_id = value; } }
        public string ChapterImg { get { return chapter_img; } set { chapter_img = value; } }
        public string NodeList { get { return node_list; } set { node_list = value; } }
        public string ChapterMapResIndex { get { return chapter_map_res_index; } set { chapter_map_res_index = value; } }
        public long EnterChapterDialogId { get { return enter_chapter_dialog_id; } set { enter_chapter_dialog_id = value; } }
        public long ChapterBgMusicId { get { return chapter_bg_music_id; } set { chapter_bg_music_id = value; } }
        public long ChapterBossBgMusicId { get { return chapter_boss_bg_music_id; } set { chapter_boss_bg_music_id = value; } }
        public string VideoBossBgGoPath { get { return video_boss_bg_go_path; } set { video_boss_bg_go_path = value; } }

        
        private List<string> _m_name_args;
        [Ignore]
        public List<string> nameArgs
        {
            get
            {
                if (_m_name_args == null)
                    _m_name_args = ALCommon.ParseStringList(name_args, "name_args");
                return _m_name_args;
            }
        }
        
        private List<string> _m_desc_args;
        [Ignore]
        public List<string> descArgs
        {
            get
            {
                if (_m_desc_args == null)
                    _m_desc_args = ALCommon.ParseStringList(desc_args, "desc_args");
                return _m_desc_args;
            }
        }
        
        private List<long> _m_comment_random_group_id;
        [Ignore]    
        public List<long> commentRandomGroupId
        {
            get
            {
                if (_m_comment_random_group_id == null)
                    _m_comment_random_group_id = ALCommon.ParseLongList(comment_random_group_id, "comment_random_group_id");
                return _m_comment_random_group_id;
            }
        }
        
        private List<NPCommonListLongInfo> _m_after_point_dialog;
        [Ignore]
        public List<NPCommonListLongInfo> afterPointDialog
        {
            get
            {
                if (_m_after_point_dialog == null)
                    _m_after_point_dialog = NPCommonListLongInfo.readList(after_point_dialog);
                return _m_after_point_dialog;
            }
        }
        
        private List<NPCommonCostItem> _m_reward_list;
        [Ignore]
        public List<NPCommonCostItem> rewardList
        {
            get
            {
                if (_m_reward_list == null)
                    _m_reward_list = NPCommonCostItem.readList(reward_list);
                return _m_reward_list;
            }
        }
        
        private List<int> _m_power_add_rate;
        [Ignore]
        public List<int> powerAddRate
        {
            get
            {
                if (_m_power_add_rate == null)
                    _m_power_add_rate = ALCommon.ParseIntList(power_add_rate, "power_add_rate");
                return _m_power_add_rate;
            }
        }
        
        private List<int> _m_cost_add_rate;
        [Ignore]
        public List<int> costAddRate
        {
            get
            {
                if (_m_cost_add_rate == null)
                    _m_cost_add_rate = ALCommon.ParseIntList(cost_add_rate, "cost_add_rate");
                return _m_cost_add_rate;
            }
        }
        
        private NPCommonCostItem _m_reward_show;
        [Ignore]
        public NPCommonCostItem rewardShow
        {
            get
            {
                if (_m_reward_show == null)
                    _m_reward_show = NPCommonCostItem.readFromStr(reward_show);
                return _m_reward_show;
            }
        }
        
        [Ignore]
        public ChapterStageRefObj chapterStageRefObj { get; set; }

        private NPGTextureIndex _m_chapterImg;
        [Ignore]
        public NPGTextureIndex chapterImg
        {
            get
            {
                if (_m_chapterImg == null)
                {
                    _m_chapterImg = new NPGTextureIndex();
                    _m_chapterImg.readIndex(chapter_img);
                }
                return _m_chapterImg;
            }
        }
        
        private List<CommonIntLongInfo> _m_node_list;
        [Ignore]
        public List<CommonIntLongInfo> nodeList
        {
            get
            {
                if (_m_node_list == null)
                    _m_node_list = CommonIntLongInfo.readList(node_list);
                return _m_node_list;
            }
        }
        
        private NPCommonAssetPathInfo _m_chapter_map_res_index;
        [Ignore]
        public NPCommonAssetPathInfo chapterMapResIndex
        {
            get
            {
                if (_m_chapter_map_res_index == null)
                {
                    _m_chapter_map_res_index = new NPCommonAssetPathInfo();
                    _m_chapter_map_res_index.ParseFromString(chapter_map_res_index);
                }
                return _m_chapter_map_res_index;
            }
        }
        
        private NPGGoIndex _m_video_boss_bg_go_path;
        [Ignore]
        public NPGGoIndex videoBossBgGoPath
        {
            get
            {
                if (_m_video_boss_bg_go_path == null)
                {
                    _m_video_boss_bg_go_path = new NPGGoIndex();
                    _m_video_boss_bg_go_path.ParseFromString(video_boss_bg_go_path);
                }
                return _m_video_boss_bg_go_path;
            }
        }
        
        /**
     * 计算目标格子所需的战力
     * @return
     */
        public long calNeedPower(int _point)
        {
            if(null == powerAddRate)
                return initial_power;
            
            //判断越界
            if(_point < 1 || _point > point_count || _point > powerAddRate.Count)
                return initial_power;

            return (long)math.ceil((double)initial_power * (10000 + powerAddRate[_point - 1]) / 10000);
        }

        /**
         * 计算目标格子所需的金币
         * @return
         */
        public long calNeedCost(int _point)
        {
            if (null == powerAddRate)
                return initial_cost;
            
            //判断越界
            if(_point < 1 || _point > point_count || _point > costAddRate.Count)
                return initial_cost;

            return initial_cost * (10000 + costAddRate[_point - 1]) / 10000;
        }

#if NP_GAME     
        //获取波次队友的对话队友的icon
        public long getAfterPointDialog(int _point, out long _tipResId, out NPNPCRefObj _npcRef)
        {
            _tipResId = 0;
            _npcRef = null;
            
            if(null == afterPointDialog)
                return 0;
            //判断越界
            if(_point < 1 || _point > point_count)
                return 0;

            foreach (NPCommonListLongInfo npCommonKeyValueInfo in afterPointDialog)
            {
                if(null == npCommonKeyValueInfo || null == npCommonKeyValueInfo.value || npCommonKeyValueInfo.value.Count < 4)
                    continue;
                
                if (npCommonKeyValueInfo.value[0] == _point)
                {
                    _tipResId = npCommonKeyValueInfo.value[2];
                    _npcRef = GRefdataCoreMgr.instance.npcRefCore.getRef(npCommonKeyValueInfo.value[3]);
                    return npCommonKeyValueInfo.value[1];
                }
            }

            return 0;
        }

        private List<long> _m_dialogPointList; 
        public List<long> getDialogPointList()
        {
            if(null == afterPointDialog)
                return null;
            
            if (null != _m_dialogPointList)
                return _m_dialogPointList;

            _m_dialogPointList = new List<long>();
            foreach (NPCommonListLongInfo npCommonListLongInfo in afterPointDialog)
            {
                if(null == npCommonListLongInfo || null == npCommonListLongInfo.value)
                    continue;
                
                _m_dialogPointList.Add(npCommonListLongInfo.value[0]);
            }
            
            return _m_dialogPointList;
        }
        
        //根据节点索引获取节点样式，最后一个节点+1是boss节点
        public _IChapterNodeStyle getChapterNodeStyleByIndexId(int _index, out NPGGoIndex _bgGoIndex)
        {
            _bgGoIndex = null;
            if(null == nodeList || nodeList.Count < _index)
                return null;

            //最后一个节点是boss节点
            if (nodeList.Count == _index)
            {
                _bgGoIndex = videoBossBgGoPath;
                return getBossStyleRefObj();
            }
            
            CommonIntLongInfo intLongInfo = nodeList[_index];
            if(null == intLongInfo)
                return null;

            ChapterNodeStyleRefObj chapterNodeStyle = GRefdataCoreMgr.instance.chapterNodeStyleRefCore.getRef(intLongInfo.longValue);
            return chapterNodeStyle;
        }
        
        //根据节点索引获取节点样式，不获取boss节点
        public ChapterNodeStyleRefObj getChapterNormalNodeStyleByIndexId(int _index)
        {
            if(null == nodeList || nodeList.Count < _index)
                return null;

            //最后一个节点是boss节点
            if (nodeList.Count == _index)
            {
                return null;
            }
            
            CommonIntLongInfo intLongInfo = nodeList[_index];
            if(null == intLongInfo)
                return null;

            ChapterNodeStyleRefObj chapterNodeStyle = GRefdataCoreMgr.instance.chapterNodeStyleRefCore.getRef(intLongInfo.longValue);
            return chapterNodeStyle;
        }
       
        public int getNodeIndexByPoint(int _point)
        {
            if (null == nodeList)
                return 0;
            
            for (int i = 0; i < nodeList.Count; i++)
            {
                if (_point >= nodeList[i].intValue)
                {
                    continue;
                }
                return i;
            }
            return nodeList.Count;
        }
        
        public long getNodeIdByIndex(int _index)
        {
            if (null == nodeList)
                return 0;
            
            if(nodeList.Count > _index && null != nodeList[_index])
                return nodeList[_index].longValue;
            return 0;
        }
        
        private ChapterBossStyleRefObj _m_boss_style_ref_obj;
        public ChapterBossStyleRefObj getBossStyleRefObj()
        {
            if(null == _m_boss_style_ref_obj)
                _m_boss_style_ref_obj = GRefdataCoreMgr.instance.chapterBossStyleRefCore.getRef(boss_style_id);
            return _m_boss_style_ref_obj;
        }

        private bool _isSearch = false;
        private ChapterBuildUnlockRefObj _m_forward_simple_unlock_ref_obj;
        public ChapterBuildUnlockRefObj getChapterBuildUnlockRefObj()
        {
            if(null == _m_forward_simple_unlock_ref_obj && !_isSearch)
            {
                _isSearch = true;
                foreach (ChapterBuildUnlockRefObj chapterBuildUnlockRefObj in GRefdataCoreMgr.instance.chapterBuildUnlockRefCore.refList)
                {
                    if (null != chapterBuildUnlockRefObj && chapter_id <= chapterBuildUnlockRefObj.id)
                    {
                        _m_forward_simple_unlock_ref_obj = chapterBuildUnlockRefObj;
                        break;
                    }
                }
            }
            return _m_forward_simple_unlock_ref_obj;
        }
#endif
        
        public static string assetPath { get { return "refdata_db/chapter.unity3d"; } }
        public static string objName { get { return "refdata_db/chapter.txt"; } }
        public static string tableName { get { return "chapter"; } }
    }
}