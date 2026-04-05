using System;
using System.Collections.Generic;
using System.Reflection;
using ALPackage;
using GOE;
using Hotfix.TileMatchEnum;
using UnityEngine;

namespace Hotfix
{
    public class TileMatchGamePlayUIConfig
    {
        [HotfixMono("错误交换限制次数触发提示")]
        public int errorSwitchShowTipCount = 5;

        [HotfixMono("未操作时, 多久显示提示")]
        public float idleToTipTime = 10f;
        
        [HotfixMono("错误交换后重置的延迟时间")]
        public float errorSwitchResetDelayTime = 0.1f;

        [HotfixMono("交换格子音效id")]
        public long exchangeBlockAudioId;
        
        //这边是用反射机制给每个字段赋值的，性能不好的话可以每个子类每个字段单独getProperty<T>初始化赋值
        public virtual void init(MonoSkin _monoSkin)
        {
            if(null == _monoSkin)
                return;
            
            FieldInfo[] fieldInfos = this.GetType().GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                HotfixMonoAttribute customAttribute = fieldInfo.GetCustomAttribute<HotfixMonoAttribute>();
                if(null == customAttribute)
                    continue;
					
                //不序列化不生成
                if(!customAttribute.isSerialize)
                    continue;
                
                fieldInfo.SetValue(this, _monoSkin.getProperty(fieldInfo.FieldType, fieldInfo.Name));
            }
        }
    }

    public class TileMatchGameModelConfig : _AHotfixBaseMono
    {
        [HotfixMono("游戏模式")]
        public string gameModelTypeStr;

        [HotfixMono("切换到该模式时播放动画名")]
        public string changeToModelShowAniName;
    }
    
    /// <summary>
    /// 游戏玩法子窗口
    /// </summary>
    public class GGUIMonoTileMatchGamePlay : _AHotfixBaseMono
    {
        [HotfixMono("棋盘区域")]
        public RectTransform checkerboardArea;

        [HotfixMono("格子cache父节点")]
        public GameObject checkerCacheParent;
        
        [HotfixMono("方块预制体宽度")]
        public float cubeWidth;
        [HotfixMono("方块预制体高度")]
        public float cubeHeight; 
        [HotfixMono("方块预制x间隔")]
        public float cubeSpacingX;
        [HotfixMono("方块预制y间隔")]
        public float cubeSpacingY;

        [HotfixMono("屏蔽操作的遮罩")]
        public GameObject opMask;

        [HotfixMono("操作提示的物体(不需要做显隐, 客户端会设置这个位置)")]
        public GameObject opTipGameObject;
        [HotfixMono("操作提示动画")]
        public Animation opTipAnimation;
        [HotfixMono("向左移动的操作提示动画")]
        public string moveLeftOpTipAniName;
        [HotfixMono("向右移动的操作提示动画")]
        public string moveRightOpTipAniName;
        [HotfixMono("向下移动的操作提示动画")]
        public string moveDownOpTipAniName;
        [HotfixMono("向上移动的操作提示动画")]
        public string moveUpOpTipAniName;

        [HotfixMono("UI上的游戏配置")]
        public MonoSkin gameUiConfig;
        
        [HotfixMono("棋盘特效父节点")]
        public Transform checkerboardSfxParent;
        
        [HotfixMono("高级模式tab")]
        public NPGGUIMonoCommonTab advancedModeTab;
        [HotfixMono("极限模式tab")]
        public NPGGUIMonoCommonTab extremeModeTab;

        [HotfixMono("切换模式动画")]
        public Animation chgModelAnimation;
        [HotfixMono("游戏模式配置列表")]
        public List<MonoSkin> gameModelConfigList;
        
        [HotfixMono("三消任务子窗口")]
        public GGUIHotfixCommonMono monoTileMatchTask;
    }
}