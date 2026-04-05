using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 获取主城入口entry_point高亮位置
    /// </summary>
    public class TutorialHighlightLocationDealer_HOME_ENTRY_POINT : _ATutorialHighlightLocationDealer
    {
        private long _m_entryPointId;

        public override ETutorialHighlightLocationType highlightLocationType { get { return ETutorialHighlightLocationType.HOME_ENTRY_POINT; } }

        public override bool getCenterUIRootPosition(out Vector2 centerUIRootPosition)
        {
            centerUIRootPosition = Vector2.zero;

            if (GGUIWndHomeEntryFollow.instance == null || !GGUIWndHomeEntryFollow.instance.isLoaded || !GGUIWndHomeEntryFollow.instance.isShow)
            {
                Debug.LogError("[TutorialHighlightLocationDealer_HOME_ENTRY_POINT] 主城入口跟随界面未加载或未显示");
                return false;
            }

            HomeEntryPointFollowInstance followInstance = GGUIWndHomeEntryFollow.instance.getInstance(_m_entryPointId);
            if (followInstance == null)
            {
                Debug.LogError($"[TutorialHighlightLocationDealer_HOME_ENTRY_POINT] 找不到entry_point跟随实例, entryPointId: {_m_entryPointId}");
                return false;
            }

            centerUIRootPosition = followInstance.itemUICenterPos;
            return true;
        }

        public static TutorialHighlightLocationDealer_HOME_ENTRY_POINT readVariable(string _str)
        {
            TutorialHighlightLocationDealer_HOME_ENTRY_POINT dealer = new TutorialHighlightLocationDealer_HOME_ENTRY_POINT();

            if (!string.IsNullOrEmpty(_str))
            {
                dealer._m_entryPointId = ALCommon.GetLong(_str);
            }

            return dealer;
        }
    }
}
