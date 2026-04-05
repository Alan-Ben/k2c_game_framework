using ALPackage;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//输入框的扩展，支持选中输入框就隐藏Placeholder（原功能是有输入才隐藏）
[AddComponentMenu("UI/Input Field Emoji Ex", 31)]
public class InputFieldEmojiEx : InputFieldEmoji
{
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        ALUGUICommon.setGameObjEnable(this.placeholder,false);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        ALUGUICommon.setGameObjEnable(this.placeholder,true);
    }
}