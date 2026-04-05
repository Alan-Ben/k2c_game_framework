
using ChatPackage;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    //宴会分享
    public class NPGGUIMonoChatMsgItemDinnerInvite : _ANPGGUIMonoPlayerChatMsgItem_JumpTo
    {
        [ALHeader("标题")]
        public TextEx txtTitle;
        [ALHeader("宴请描述")]
        public TextEx txtDesc;
        [ALHeader("席位数量")]
        public TextEx txtSeatCount;
        [ALHeader("已结束置灰对象")]
        public List<MaskableGraphic> goGrayList;
        [ALHeader("已结束显示对象")]
        public List<GameObject> goEndShowList;
        [ALHeader("已结束隐藏对象")]
        public List<GameObject> goEndHideList;


        public static string myAssetPath { get { return UIResPathAssistant.getAssetPath(1350); } }
        public static string myObjName { get { return UIResPathAssistant.getObjName(1350);} }
        
        public static string othersAssetPath { get { return UIResPathAssistant.getAssetPath(1351); } }
        public static string othersObjName { get { return UIResPathAssistant.getObjName(1351);} }
    }
}