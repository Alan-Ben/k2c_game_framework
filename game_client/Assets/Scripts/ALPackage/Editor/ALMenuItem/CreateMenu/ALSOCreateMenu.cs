using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

#if AL_CREATURE_SYS
namespace ALPackage
{
    public class ALSOCreateMenu
    {
        //生成空对象
        [MenuItem("ALSOCreate/ALSOActionAddObjEvent")]
        public static void makeALSOActionAddObjEvent()
        {
            ALCreateCommonFunc.createSOObj<ALSOActionAddObjEvent>();
        }
        [MenuItem("ALSOCreate/ALSOActionCanBlendEvent")]
        public static void makeALSOActionCanBlendEvent()
        {
            ALCreateCommonFunc.createSOObj<ALSOActionCanBlendEvent>();
        }
        [MenuItem("ALSOCreate/ALSOActionChgTimeScaleEvent")]
        public static void makeALSOActionChgTimeScaleEvent()
        {
            ALCreateCommonFunc.createSOObj<ALSOActionChgTimeScaleEvent>();
        }
        [MenuItem("ALSOCreate/ALSOActionFinishEvent")]
        public static void makeALSOActionFinishEvent()
        {
            ALCreateCommonFunc.createSOObj<ALSOActionFinishEvent>();
        }
        [MenuItem("ALSOCreate/ALSOActionInstigatorMovementEvent")]
        public static void makeALSOActionInstigatorMovementEvent()
        {
            ALCreateCommonFunc.createSOObj<ALSOActionInstigatorMovementEvent>();
        }
        [MenuItem("ALSOCreate/ALSOActionRemoveObjEvent")]
        public static void makeALSOActionRemoveObjEvent()
        {
            ALCreateCommonFunc.createSOObj<ALSOActionRemoveObjEvent>();
        }
        [MenuItem("ALSOCreate/ALSOAdditionLinkBonesObjInfo")]
        public static void makeALSOAdditionLinkBonesObjInfo()
        {
            ALCreateCommonFunc.createSOObj<ALSOAdditionLinkBonesObjInfo>();
        }

        [MenuItem("ALSOCreate/ALSOAdditionMeshObjInfo")]
        public static void makeALSOAdditionMeshObjInfo()
        {
            ALCreateCommonFunc.createSOObj<ALSOAdditionMeshObjInfo>();
        }
        [MenuItem("ALSOCreate/ALSOAdditionObjInfo")]
        public static void makeALSOAdditionObjInfo()
        {
            ALCreateCommonFunc.createSOObj<ALSOAdditionObjInfo>();
        }
        [MenuItem("ALSOCreate/ALSOAnimationAdditionAddEvent")]
        public static void makeALSOAnimationAdditionAddEvent()
        {
            ALCreateCommonFunc.createSOObj<ALSOAnimationAdditionAddEvent>();
        }
        [MenuItem("ALSOCreate/ALSOAnimationAdditionRemoveEvent")]
        public static void makeALSOAnimationAdditionRemoveEvent()
        {
            ALCreateCommonFunc.createSOObj<ALSOAnimationAdditionRemoveEvent>();
        }

        [MenuItem("ALSOCreate/ALSOAnimationChgTimeScaleEvent")]
        public static void makeALSOAnimationChgTimeScaleEvent()
        {
            ALCreateCommonFunc.createSOObj<ALSOAnimationChgTimeScaleEvent>();
        }
        [MenuItem("ALSOCreate/ALSOAnimationFinishEvent")]
        public static void makeALSOAnimationFinishEvent()
        {
            ALCreateCommonFunc.createSOObj<ALSOAnimationFinishEvent>();
        }
        [MenuItem("ALSOCreate/ALSOAnimationSetLayerEvent")]
        public static void makeALSOAnimationSetLayerEvent()
        {
            ALCreateCommonFunc.createSOObj<ALSOAnimationSetLayerEvent>();
        }
        [MenuItem("ALSOCreate/ALSOBaseAnimationInfo")]
        public static void makeALSOBaseAnimationInfo()
        {
            ALCreateCommonFunc.createSOObj<ALSOBaseAnimationInfo>();
        }

        [MenuItem("ALSOCreate/ALSOBaseAnimationInfoObj")]
        public static void makeALSOBaseAnimationInfoObj()
        {
            ALCreateCommonFunc.createSOObj<ALSOBaseAnimationInfoObj>();
        }
        [MenuItem("ALSOCreate/ALSOCreatureActionInfo")]
        public static void makeALSOCreatureActionInfo()
        {
            ALCreateCommonFunc.createSOObj<ALSOCreatureActionInfo>();
        }
        [MenuItem("ALSOCreate/ALSOCreatureBoneObjInfo")]
        public static void makeALSOCreatureBoneObjInfo()
        {
            ALCreateCommonFunc.createSOObj<ALSOCreatureBoneObjInfo>();
        }
        [MenuItem("ALSOCreate/ALSOCreatureRootObjInfo")]
        public static void makeALSOCreatureRootObjInfo()
        {
            ALCreateCommonFunc.createSOObj<ALSOCreatureRootObjInfo>();
        }

        [MenuItem("ALSOCreate/ALSOMoveAnimationInfo")]
        public static void makeALSOMoveAnimationInfo()
        {
            ALCreateCommonFunc.createSOObj<ALSOMoveAnimationInfo>();
        }
        [MenuItem("ALSOCreate/ALSOMoveAnimationInfoObj")]
        public static void makeALSOMoveAnimationInfoObj()
        {
            ALCreateCommonFunc.createSOObj<ALSOMoveAnimationInfoObj>();
        }
        [MenuItem("ALSOCreate/ALSOMoveAnimationModeObj")]
        public static void makeALSOMoveAnimationModeObj()
        {
            ALCreateCommonFunc.createSOObj<ALSOMoveAnimationModeObj>();
        }

        [MenuItem("ALSOCreate/ALSOMoveStateCanBlendEvent")]
        public static void makeALSOMoveStateCanBlendEvent()
        {
            ALCreateCommonFunc.createSOObj<ALSOMoveStateCanBlendEvent>();
        }
    }
}
#endif
