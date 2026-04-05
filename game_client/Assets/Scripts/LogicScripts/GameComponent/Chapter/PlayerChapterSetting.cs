using System;
using System.Collections.Generic;
using System.Text;
using ALPackage;

namespace GOE
{
    public class PlayerChapterSetting : _AALBasicSettingInfo
    {
        private const char FieldSplitChar = '#';
        private const char CollectionItemSplitChar = ',';
        
        // private List<long> _m_lCheckedUnlockChapterStagePlotIdList;//查看过的已解锁的节剧情id列表
        
        public PlayerChapterSetting(long _cid) : base($"{_cid}_chapter_setting")
        {
            // _m_lCheckedUnlockChapterStagePlotIdList = new List<long>();
            // GRefdataCoreMgr.instance.chapterStagePlotRefCore.dealAllRef((_refObj) =>
            // {
            //     if(_refObj == null)
            //         return;
            //         
            //     if(_refObj.unlock_condition == null || _refObj.unlock_condition.isNoConditionOrEnable(null))
            //         _m_lCheckedUnlockChapterStagePlotIdList.Add(_refObj.plot_id);
            // });
        }

        protected override string _makeSettingStr()
        {
            StringBuilder sb = new StringBuilder();

            // // 序列化 _m_lCheckedUnlockChapterStagePlotIdList
            // if (_m_lCheckedUnlockChapterStagePlotIdList != null)
            // {
            //     foreach (var plotId in _m_lCheckedUnlockChapterStagePlotIdList)
            //     {
            //         sb.Append(plotId);
            //         sb.Append(CollectionItemSplitChar);
            //     }
            // }
            // sb.Append(FieldSplitChar);

            // 

            return sb.ToString();
        }

        protected override void _initSettingStr(string _infoStr)
        {
            if (string.IsNullOrEmpty(_infoStr))
                return;
            
            string[] fieldsArray = _infoStr.Split(FieldSplitChar);
            if(fieldsArray == null)
                return;

            try
            {
                // // 解析_m_lCheckedUnlockChapterStagePlotIdList字段
                // if (fieldsArray.Length >= 1)
                // {
                //     if (_m_lCheckedUnlockChapterStagePlotIdList == null)
                //         _m_lCheckedUnlockChapterStagePlotIdList = new List<long>();
                //     _m_lCheckedUnlockChapterStagePlotIdList.Clear();
                //
                //     if (!string.IsNullOrEmpty(fieldsArray[0]))
                //     {
                //         string[] plotIdArray = fieldsArray[0].Split(CollectionItemSplitChar, StringSplitOptions.RemoveEmptyEntries);
                //         if (plotIdArray != null)
                //         {
                //             foreach (string plotIdStr in plotIdArray)
                //             {
                //                 if (!string.IsNullOrEmpty(plotIdStr) && long.TryParse(plotIdStr, out long plotId))
                //                     _m_lCheckedUnlockChapterStagePlotIdList.Add(plotId);
                //             }
                //         }
                //     }
                // }
            }
            catch (Exception e)
            {
                Debug.LogError($"[PlayerChapterSetting _initSettingStr] Error ! settingStr: {_infoStr}, Exception: {e}");
            }
        }
        
        // /// <summary>
        // /// 添加已查看过的解锁节剧情id
        // /// </summary>
        // /// <param name="_plotId"></param>
        // public void addCheckedUnlockChapterStagePlotId(long _plotId)
        // {
        //     if (_m_lCheckedUnlockChapterStagePlotIdList == null)
        //         _m_lCheckedUnlockChapterStagePlotIdList = new List<long>();
        //     
        //     if (!_m_lCheckedUnlockChapterStagePlotIdList.Contains(_plotId))
        //     {
        //         _m_lCheckedUnlockChapterStagePlotIdList.Add(_plotId);
        //         saveSetting();
        //     }
        // }
        //
        // /// <summary>
        // /// 是否已经查看过解锁节剧情
        // /// </summary>
        // /// <param name="_plotId"></param>
        // /// <returns></returns>
        // public bool isCheckedUnlockChapterStagePlotId(long _plotId)
        // {
        //     if (_m_lCheckedUnlockChapterStagePlotIdList == null)
        //         return false;
        //
        //     return _m_lCheckedUnlockChapterStagePlotIdList.Contains(_plotId);
        // }
    }
}