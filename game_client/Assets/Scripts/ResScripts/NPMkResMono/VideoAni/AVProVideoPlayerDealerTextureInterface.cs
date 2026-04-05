using ALPackage;
#if AL_AVPRO_V2
using RenderHeads.Media.AVProVideo;
#endif
using UnityEngine;

namespace GOE
{
    
    public class AVProVideoPlayerDealerTextureInterface : _AALVideoPlayerDealerTextureInterface
    {
#if AL_AVPRO_V2
        protected internal override bool _setAVProTargetMat(ApplyToMaterial _mat, Material _renderMat)
        {
            if (_mat != null)
            {
                _mat.TexturePropertyId = ShaderPropertyMgr.g_IVideoTextureId;
                _mat.Material = _renderMat;
            }
            return true;
        }
#endif
        protected internal override bool _setUnityPlayerTargetMat(Material _renderMat, Texture _texture)
        {
            if (_renderMat != null) _renderMat.SetTexture(ShaderPropertyMgr.g_IVideoTextureId, _texture);
            return true;
        }
    }
}