using SQLite4Unity3d;
using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 对话句子表
    /// </summary>
    public class NPDialogueSentenceRefObj
    {
        public long id;//唯一id
        public long comic_id;//漫画id，有配就不管那些效果直接展示漫画
        public string next_id;//下一句id字符串
        private List<long> _m_nextId;//下一句id
        public string force_auto_skip_condition;//强制自动跳过条件(最高判断优先级,优先级高于is_must,满足条件就算不点跳过也一定会自动跳过这句对话,进入下一句)
        private _NPPlayerConditionSerializeInfo _m_forceAutoSkipCondition;//强制自动跳过条件(最高判断优先级,优先级高于is_must,满足条件就算不点跳过也一定会自动跳过这句对话,进入下一句)
        public string sender_name;//发言者名称
        public string sender_name_args;//发言者参数
        private List<string> _m_senderNameArgs;//发言者参数
        public string sender_icon;//发言者头像图标
        public string content;//对话内容
        public string content_args;//对话内容参数字符串
        private List<string> _m_contentArgs;//对话内容参数
        public string response_option_list;//回应选项字符串
        private List<NPDialogueResponseOptionRefObj> _m_response_option_list;//回应选项
        public string skip_effect;//跳过执行效果字符串
        private _NPPlayerEffectSerializeInfo _m_skipEffect;//跳过需要的执行效果
        public string pre_effect;//预执行效果字符串
        private _NPPlayerEffectSerializeInfo _m_preEffect;//预执行效果
        public string begin_effect;//开始效果字符串
        private _NPPlayerEffectSerializeInfo _m_beginEffect;//开始效果
        public string end_effect;//结束效果字符串
        private _NPPlayerEffectSerializeInfo _m_endEffect;//结束效果
        public long dialog_res_path_id;//对话框资源路径id（0表示不处理文本）
        public float start_delay;//开始延迟（秒）
        public float end_delay;//结束延迟（秒）
        public long chat_history_ui_path_id;//聊天记录展示样式id
        public long option_chat_history_ui_path_id;//聊天记录选项展示样式id
        public long plot_dialog_ui_path_id;//剧情对话展示样式id
        public string is_must;//是否必须查看
        public long skip_time_ms;//多少毫秒后可以跳过
        public long auto_next_time_ms;//多少毫秒后自动到下一句
        public string is_hide_skip_btn;//是否隐藏跳过按钮
        public string is_hide_auto_play_btn;//是否隐藏自动播放按钮

        public long Id { get { return id; } set { id = value; } }
        public long ComicId { get { return comic_id; } set { comic_id = value; } }
        public string NextId { get { return next_id; } set { next_id = value; } }
        public string ForceAutoSkipCondition { get { return force_auto_skip_condition; } set { force_auto_skip_condition = value; } }
        public string SenderName { get { return sender_name; } set { sender_name = value; } }
        public string SenderNameArgs { get { return sender_name_args; } set { sender_name_args = value; } }
        public string SenderIcon { get { return sender_icon; } set { sender_icon = value; } }
        public string Content { get { return content; } set { content = value; } }
        public string ContentArgs { get { return content_args; } set { content_args = value; } }
        public string ResponseOptions { get { return response_option_list; } set { response_option_list = value; } }
        public string SkipEffect { get { return skip_effect; } set { skip_effect = value; } }
        public string PreEffect { get { return pre_effect; } set { pre_effect = value; } }
        public string BeginEffect { get { return begin_effect; } set { begin_effect = value; } }
        public string EndEffect { get { return end_effect; } set { end_effect = value; } }
        public long DialogResPathId { get { return dialog_res_path_id; } set { dialog_res_path_id = value; } }
        public float StartDelay { get { return start_delay; } set { start_delay = value; } }
        public float EndDelay { get { return end_delay; } set { end_delay = value; } }
        public long ChatHistoryUIPathId { get { return chat_history_ui_path_id; } set { chat_history_ui_path_id = value; } }
        public long OptionChatHistoryUIPathId { get { return option_chat_history_ui_path_id; } set { option_chat_history_ui_path_id = value; } }
        public long PlotDialogUIPathId { get { return plot_dialog_ui_path_id; } set { plot_dialog_ui_path_id = value; } }
        public string IsMust { get { return is_must; } set { is_must = value; } }
        public long skipTimeMs { get { return skip_time_ms; } set { skip_time_ms = value; } }
        public long autoNextTimeMs { get { return auto_next_time_ms; } set { auto_next_time_ms = value; } }
        public string IsHideSkipBtn { get { return is_hide_skip_btn; } set { is_hide_skip_btn = value; } }
        public string IsHideAutoPlayBtn { get { return is_hide_auto_play_btn; } set { is_hide_auto_play_btn = value; } }
        
        [Ignore]
        public List<long> nextId //下一句id
        {
            get
            {
                if (_m_nextId == null)
                {
                    _m_nextId = new List<long>();
                    string[] strs = next_id.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (long.TryParse(strs[i], out long nextId))
                        {
                            _m_nextId.Add(nextId);
                        }
                        else
                        {
                            #if UNITY_EDITOR
                            ALLog.Error($"------对话句子表：{id}的字段：next_id配置错误，请检查");
                            #endif
                        }
                    }
                }
                return _m_nextId;
            }
        }
        [Ignore]
        public _NPPlayerConditionSerializeInfo forceAutoSkipCondition //强制自动跳过条件(最高判断优先级,优先级高于is_must,满足条件就算不点跳过也一定会自动跳过这句对话,进入下一句)
        {
            get
            {
                if (_m_forceAutoSkipCondition == null)
                    _m_forceAutoSkipCondition = _NPPlayerConditionSerializeInfo.ReadFromString(force_auto_skip_condition);
                return _m_forceAutoSkipCondition;
            }
        }
        [Ignore]
        public List<string> contentArgs //对话内容参数
        {
            get
            {
                if (_m_contentArgs == null)
                {
                    _m_contentArgs = new List<string>();
                    if (null != content_args)
                    {
                        string[] strs = content_args.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < strs.Length; i++)
                        {
                            _m_contentArgs.Add(strs[i]);
                        }
                    }
                }
                return _m_contentArgs;
            }
        }
        
        [Ignore]
        public List<string> senderNameArgs //发言者参数
        {
            get
            {
                if (_m_senderNameArgs == null)
                {
                    _m_senderNameArgs = new List<string>();
                    if (null != SenderNameArgs)
                    {
                        string[] strs = SenderNameArgs.Split(new string[] {";"}, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < strs.Length; i++)
                        {
                            _m_senderNameArgs.Add(strs[i]);
                        }
                    }
                }
                return _m_senderNameArgs;
            }
        }

#if NP_GAME
        [Ignore]
        public List<NPDialogueResponseOptionRefObj> responseOptions //回应选项
        {
            get
            {
                if (_m_response_option_list == null)
                {
                    _m_response_option_list = new List<NPDialogueResponseOptionRefObj>();
                    NPDialogueResponseOptionRefObj responseOptionRefObj = null;
                    string[] strs = response_option_list.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    long optionRefId;
                    for (int i = 0; i < strs.Length; i++)
                    {
                        if(!long.TryParse(strs[i],out optionRefId))
                            continue;

                        responseOptionRefObj = GRefdataCoreMgr.instance.dialogueResponseOptionRefCore.getRef(optionRefId);
                        if (null == responseOptionRefObj)
                            continue;
                        _m_response_option_list.Add(responseOptionRefObj);
                    }
                }
                return _m_response_option_list;
            }
        }
#endif
        
        [Ignore]
        public NPGTextureIndex senderIcon //发言者头像图标
        {
            get
            {
                if (!string.IsNullOrEmpty(sender_icon))
                    return NPGTextureIndex.readIndexInfo(sender_icon);
                return null;
            }
        }
        [Ignore]
        public _NPPlayerEffectSerializeInfo skipEffect //跳过需要执行效果
        {
            get
            {
                if (_m_skipEffect == null)
                    _m_skipEffect = _NPPlayerEffectSerializeInfo.ReadFromString(skip_effect);
                return _m_skipEffect;
            }
        }
        [Ignore]
        public _NPPlayerEffectSerializeInfo preEffect //预执行效果
        {
            get
            {
                if (_m_preEffect == null)
                    _m_preEffect = _NPPlayerEffectSerializeInfo.ReadFromString(pre_effect);
                return _m_preEffect;
            }
        }
        [Ignore]
        public _NPPlayerEffectSerializeInfo beginEffect //开始效果
        {
            get
            {
                if (_m_beginEffect == null)
                    _m_beginEffect = _NPPlayerEffectSerializeInfo.ReadFromString(begin_effect);
                return _m_beginEffect;
            }
        }
        [Ignore]
        public _NPPlayerEffectSerializeInfo endEffect //结束效果
        {
            get
            {
                if (_m_endEffect == null)
                    _m_endEffect = _NPPlayerEffectSerializeInfo.ReadFromString(end_effect);
                return _m_endEffect;
            }
        }
        [Ignore]
        public bool isSelf
        {
            get
            {
                return sender_name.Equals("self", StringComparison.OrdinalIgnoreCase);
            }
        }
        
        [Ignore]
        public bool isMust
        {
            get
            {
                return null != is_must && is_must.Equals("true", StringComparison.OrdinalIgnoreCase);
            }
        }
        
        [Ignore]
        public bool isHideSkipBtn
        {
            get
            {
                return null != is_hide_skip_btn && is_hide_skip_btn.Equals("true", StringComparison.OrdinalIgnoreCase);
            }
        }
        
        [Ignore]
        public bool isHideAutoPlayBtn
        {
            get
            {
                return null != is_hide_auto_play_btn && is_hide_auto_play_btn.Equals("true", StringComparison.OrdinalIgnoreCase);
            }
        }
        

        public string getContentStr()
        {
            return TextTranslate.instance.getLanguage(content, contentArgs);
        }

        /// <summary>
        /// 获取下一句的配置id
        /// </summary>
        /// <param name="_index">列表索引</param>
        /// <returns></returns>
        public long getNextId(int _index)
        {
            if (_index < 0 || _index >= nextId.Count)
                return -1;

            return nextId[_index];
        }

        /// <summary>
        /// 是否强制自动跳过
        /// </summary>
        /// <returns></returns>
        public bool isForceAutoSkip()
        {
            return forceAutoSkipCondition != null && forceAutoSkipCondition.hasCondition && forceAutoSkipCondition.IsEnable(null);
        }
        
        public static string assetPath { get { return "refdata_db/dialogue.unity3d"; } }
        public static string objName { get { return "refdata_db/dialogue_sentence.txt"; } }
        public static string tableName { get { return "dialogue_sentence"; } }
    }
}