using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using ALPackage;
using GOE;

public class NPGUIMonoRawImage : RawImage
{
#if UNITY_EDITOR
    //
    // Summary:
    //     ///
    //     The RawImage's texture. (ReadOnly).
    //     ///
    public override Texture mainTexture {
        get
        {
            Texture resTexture = null;

            NPAGUIMonoCustomTextureMono index = GetComponent<NPAGUIMonoCustomTextureMono>();
            if (null != index && null == resTexture)
                resTexture = (Texture)Resources.Load<Texture>("GUI/ATextures/atex_" + index.textureIndex.mainId + "/atex_" + index.textureIndex.mainId + "_" + index.textureIndex.subId);

            NPGGUIMonoCustomTextureMono custTex = GetComponent<NPGGUIMonoCustomTextureMono>();
            if (null != custTex && null == resTexture)
                resTexture = (Texture)Resources.Load<Texture>("GUI/Textures/tex_" + custTex.textureIndex.mainId + "/tex_" + custTex.textureIndex.mainId + "_" + custTex.textureIndex.subId);

            //如果为空还需要处理
            if(null == resTexture)
                resTexture = base.mainTexture;

            return resTexture;
        }
    }
#endif
}
