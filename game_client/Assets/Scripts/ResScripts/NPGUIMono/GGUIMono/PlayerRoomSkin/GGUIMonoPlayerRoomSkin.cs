using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoPlayerRoomSkin : _AALBasicUIWndMono
    {
        [ALHeader("选中卧室皮肤背景")]
        public RawImage selectedRoomSkinBg;

        [ALHeader("选中卧室皮肤来源描述")]
        public TextEx txtSelectedRoomSkinSource;
        
        [ALHeader("选中卧室皮肤不同状态配置列表")]
        public List<NPCommonEnumStatMutexShowInfo<EPlayerRoomSkinState>> selectedRoomSkinStateConfigList;

        [ALHeader("卧室皮肤列表")]
        public GGUIMonoPlayerRoomSkinItemGrid monoRoomSkinItemGrid;
        
        [ALHeader("关闭按钮")]
        public GameObject btnReturn;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1432); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1432);} }
    }
}