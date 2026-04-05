using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 子嗣列表位置
    /// </summary>
    public class NPTutorialNoticeChildMainListRectDealer : _AWCGTutorialNoticeRectDealer
    {
        private EChildMainTargetChildType _m_targetChildType;

        public override EWCGTutorialNoticeRectType noticeRectType { get { return EWCGTutorialNoticeRectType.CHILD_MAIN_LIST_P; } }

        public override bool getRect(out Rect _rect)
        {
            RectTransform rectTransform = GGUIWndChildMain.instance?.getTargetHeroRectTransform(_m_targetChildType);
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

        public static NPTutorialNoticeChildMainListRectDealer readVariable(string _str)
        {
            NPTutorialNoticeChildMainListRectDealer effectObj = new NPTutorialNoticeChildMainListRectDealer();

            // 解析枚举类型
            if (!string.IsNullOrEmpty(_str))
            {
                if (ALCommon.TryEnumParse(typeof(EChildMainTargetChildType), _str, out EChildMainTargetChildType _targetType))
                {
                    effectObj._m_targetChildType = _targetType;
                }
            }

            return effectObj;
        }
    }
}

