using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine.UI;

[System.Serializable]
public class elementItem
{
    public TutorialMoveMaskElementType type;
    public LayoutElement element;
}

public class NPGGUIMonoMovementMask : _AALBasicUIWndMono
{
    public elementItem[] elementList;
    
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(UIResPathConst.WIN_TUTORIAL_MOVE_MASK); } }
    public static string objName { get { return UIResPathAssistant.getObjName(UIResPathConst.WIN_TUTORIAL_MOVE_MASK); } }
}
