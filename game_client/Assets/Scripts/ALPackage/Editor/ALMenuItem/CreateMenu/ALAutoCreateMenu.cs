﻿using System.IO;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

#if AL_CREATURE_SYS
namespace ALPackage
{
   public class ALAutoCreateMenu
   {
       //自动根据选中的animation对象在目录内生成关联的action和operation以及animation对象
       [MenuItem("Assets/ALAutoCreateMenu/AutoCreateAction")]
       public static void autoCreateAction()
       {
           Object[] selectedAnis = Selection.GetFiltered(typeof(AnimationClip), SelectionMode.DeepAssets);

           if (selectedAnis.Length <= 0)
               return;

           //检查目录下的ope和ani目录是否创建
           if (!Directory.Exists(Application.dataPath + "/Resources/__tmpActFolder"))
               AssetDatabase.CreateFolder("Assets/Resources", "__tmpActFolder");
           if (!Directory.Exists(Application.dataPath + "/Resources/__tmpActFolder/ani"))
               AssetDatabase.CreateFolder("Assets/Resources/__tmpActFolder", "ani");
           if (!Directory.Exists(Application.dataPath + "/Resources/__tmpActFolder/act"))
               AssetDatabase.CreateFolder("Assets/Resources/__tmpActFolder", "act");

           //根据每个动作对象创建对应的脚本对象
           for (int i = 0; i < selectedAnis.Length; i++)
           {
               AnimationClip aniObj = selectedAnis[i] as AnimationClip;
               if (null == aniObj)
                   continue;

               //创建animation对象
               ALSOBaseAnimationInfoObj animationInfoObj = ScriptableObject.CreateInstance<ALSOBaseAnimationInfoObj>();
               animationInfoObj.animationClipObj = aniObj;
               animationInfoObj.removeBlendTime = 0.1f;
               animationInfoObj.blendTime = 0.1f;
               animationInfoObj.animationMode = WrapMode.ClampForever;
               animationInfoObj.removeType = RemoveAnimationType.BLEND;
               animationInfoObj.weight = 1;
               animationInfoObj.animationLayer = CreatureActionLayer.BASE_ACTION;

               //创建对象
               //导出路径
               string aniObjPath = "Assets/Resources/__tmpActFolder/ani/obj_" + aniObj.name + ".asset";
               AssetDatabase.CreateAsset(animationInfoObj, aniObjPath);

               //combatStageRefSet导出
               ALSOBaseAnimationInfoObj tmpAniObj = AssetDatabase.LoadAssetAtPath(aniObjPath, typeof(ALSOBaseAnimationInfoObj)) as ALSOBaseAnimationInfoObj;

               //创建animationInfo对象
               ALSOBaseAnimationInfo animationInfo = ScriptableObject.CreateInstance<ALSOBaseAnimationInfo>();
               animationInfo.defaultPlaySpeed = 1;
               animationInfo.animationObjList.Add(tmpAniObj);

               //创建对象
               //导出路径
               string aniInfoPath = "Assets/Resources/__tmpActFolder/ani/ani_" + aniObj.name + ".asset";
               AssetDatabase.CreateAsset(animationInfo, aniInfoPath);

               //combatStageRefSet导出
               ALSOBaseAnimationInfo tmpAniInfo = AssetDatabase.LoadAssetAtPath(aniInfoPath, typeof(ALSOBaseAnimationInfo)) as ALSOBaseAnimationInfo;

               //创建action对象
               ALSOCreatureActionInfo actionInfo = ScriptableObject.CreateInstance<ALSOCreatureActionInfo>();
               actionInfo.actionName = "act_" + aniObj.name;
               actionInfo.timeScale = 1;
               actionInfo.initAnimationList.Add(tmpAniInfo);

               //创建对象
               //导出路径
               string actInfoPath = "Assets/Resources/__tmpActFolder/act/act_" + aniObj.name + ".asset";
               AssetDatabase.CreateAsset(actionInfo, actInfoPath);
           }
       }

       [MenuItem("Assets/ALAutoCreateMenu/AutoCreateAction", true)]
       public static bool jAutoCreateAction()
       {
           Object[] selectedAnis = Selection.GetFiltered(typeof(AnimationClip), SelectionMode.DeepAssets);

           if (null == selectedAnis || selectedAnis.Length <= 0)
               return false;

           return true;
        }

        [MenuItem("Assets/ALAutoCreateMenu/Auto Pack Sprites")]
        public static void autoPackSprites()
        {
            ALSOSpriteList spriteList = ALCreateCommonFunc.createSOObj<ALSOSpriteList>();
            spriteList.spriteList = new List<Sprite>();
            Object[] selectedSprites = Selection.GetFiltered(typeof(Sprite), SelectionMode.Unfiltered);

            for(int i = 0; i < selectedSprites.Length; i++)
            {
                spriteList.spriteList.Add(selectedSprites[i] as Sprite);
            }
        }
        [MenuItem("Assets/ALAutoCreateMenu/Auto Pack Sprites", true)]
        public static bool jAutoPackSprites()
        {
            Object[] selectedSprites = Selection.GetFiltered(typeof(Sprite), SelectionMode.Unfiltered);

            if (null == selectedSprites || selectedSprites.Length <= 0)
                return false;

            return true;
        }
    }
}
#endif
