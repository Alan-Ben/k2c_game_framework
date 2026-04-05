using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子主页面列表位置
    /// </summary>
    public class NPTutorialNoticeConsortMainListRectDealer : _AWCGTutorialNoticeRectDealer
    {
        private EConsortMainTargetConsortType _m_targetConsortType;
        private string _m_sCustomParam;

        public override EWCGTutorialNoticeRectType noticeRectType { get { return EWCGTutorialNoticeRectType.CONSORT_MAIN_LIST_P; } }

        public override bool getRect(out Rect _rect)
        {
            RectTransform rectTransform = GGUIWndConsortChatMain.instance?.getTargetConsortRectTransform(_m_targetConsortType, _m_sCustomParam);
            if (null == rectTransform)
            {
                _rect = new Rect();
                return false;
            }

            Vector2 pos = GCommon.getUIRootPos(rectTransform);
            Vector2 minimumCornerPos = new Vector2(pos.x - rectTransform.pivot.x * rectTransform.rect.width,
                                                   pos.y - rectTransform.pivot.y * rectTransform.rect.height);
            _rect = new Rect(minimumCornerPos, rectTransform.rect.size);
            return true;
        }

        /// <summary>
        /// 解析配置字符串
        /// 格式：EConsortMainTargetConsortType:类型自定义参数
        /// 示例：CAN_UPGRADE_BLESS_SKILL  或  SPECIFIED_ID:10001
        /// </summary>
        public static NPTutorialNoticeConsortMainListRectDealer readVariable(string _str)
        {
            NPTutorialNoticeConsortMainListRectDealer effectObj = new NPTutorialNoticeConsortMainListRectDealer();

            if (string.IsNullOrEmpty(_str))
                return effectObj;

            string[] paramArray = _str.Split(':', 2);
            if (!ALCommon.TryEnumParse(typeof(EConsortMainTargetConsortType), paramArray[0], out EConsortMainTargetConsortType targetType))
                return effectObj;

            effectObj._m_targetConsortType = targetType;
            effectObj._m_sCustomParam = paramArray.Length >= 2 ? paramArray[1] : string.Empty;

            return effectObj;
        }
    }
}
