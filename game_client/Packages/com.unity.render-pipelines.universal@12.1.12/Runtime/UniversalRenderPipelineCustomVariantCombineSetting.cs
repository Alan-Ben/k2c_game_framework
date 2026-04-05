using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Serialization;

namespace UnityEngine.Rendering.Universal
{
    //共生  两个宏同时存在才起作用（&&） --优化宏数量：一半
    [Serializable]
    public struct SymbiosisShader
    {
        public Shader shader;
        public List<SymbiosisVariantCombine> symbiosisVariantCombines;
    }
    
    //寄生  依赖另一宏才起作用if(variantName1){variantName2}  --优化宏数量：variantName1未开启数量
    [Serializable]
    public struct ParasitismShader
    {
        public Shader shader;
        public List<ParasitismVariantCombine> parasitismVariantCombines;
    }
    
    //竞争  不会同时存在的宏：例如UIMask和Depth,其中一方依赖保护对象  --优化宏数量：1/4
    [Serializable]
    public struct CompetitionShader
    {
        public Shader shader;
        public List<CompetitionVariantCombine> competitionVariantCombines;
    }

    [Serializable]
    public struct SymbiosisVariantCombine
    {
        public string variantName1;
        public string variantName2;
    }

    [Serializable]
    public struct ParasitismVariantCombine
    {
        public string variantName1;
        public string parasitismVariantName2;
    }
    
    [Serializable]
    public struct CompetitionVariantCombine
    {
        public string variantName1;
        public string variantName2;
        public List<string> variantName2Protects;
    }
    
    [CreateAssetMenu(menuName="Rendering/URP Custom Variant Combine")]
    public class UniversalRenderPipelineCustomVariantCombineSetting : ScriptableObject
    {
        public List<SymbiosisShader> symbiosisShaders;
        public List<ParasitismShader> parasitismShaders;
        public List<CompetitionShader> competitionShaders;
    }
}