using Spine.Unity;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// showcase resObj通用渲染表现接口_Spine资源的
    /// </summary>
    public class ShowCaseCommonResObjRenderEffect_Spine : _AShowCaseCommonResObjRenderEffect
    {
        [ALHeader("spine数据")]
        public SkeletonMecanim skeletonMecanim;
        
        // //spine的Material不能直接替换，官方推荐是使用CustomMaterialOverride来替换
        // [NotNull]private Dictionary<Material, Material> _m_matDic = new Dictionary<Material, Material>();

        // private void Awake()
        // {
        //     if(null == skeletonMecanim || null == skeletonMecanim.skeletonDataAsset)
        //         return;
        //     
        //     foreach (AtlasAssetBase atlas in skeletonMecanim.skeletonDataAsset.atlasAssets)
        //     {
        //         if(atlas == null || atlas.Materials == null)
        //             continue;
        //         foreach (Material mat in atlas.Materials)
        //         {
        //             if(mat == null) 
        //                 continue;
        //             
        //             _m_matDic.Add(mat, new Material(mat));
        //         }
        //     }
        // }

        public override void setAlpha(float _alpha)
        {
            if(null == skeletonMecanim || null == skeletonMecanim.skeletonDataAsset || null == skeletonMecanim.skeletonDataAsset.atlasAssets)
                return;

            Color color = skeletonMecanim.skeleton.GetColor();
            color.a = _alpha;
            skeletonMecanim.skeleton.SetColor(color);
            
            //
            // foreach (AtlasAssetBase atlas in skeletonMecanim.skeletonDataAsset.atlasAssets)
            // {
            //     if(atlas == null || atlas.Materials == null)
            //         continue;
            //     foreach (Material mat in atlas.Materials)
            //     {
            //         if(mat == null) 
            //             continue;
            //         //新材质
            //         Material newMat = _m_matDic[mat];
            //         newMat.color = new Color(newMat.color.r, newMat.color.g, newMat.color.b, _alpha);
            //         
            //         skeletonMecanim.CustomMaterialOverride[mat] = newMat;
            //     }
            // }
        }

        public override void setZSpacing(float _value)
        {
            if (skeletonMecanim == null)
                return;

            skeletonMecanim.zSpacing = _value;
        }
    }
}