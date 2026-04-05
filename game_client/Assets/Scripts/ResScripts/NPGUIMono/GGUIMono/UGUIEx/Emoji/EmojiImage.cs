using CEmoji;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
[AddComponentMenu("UI/EmojiImage", 12)]
public class EmojiImage : MaskableGraphic
{
    [SerializeField]
    private EmojiAtlas m_emojiAtlas;
    
    public EmojiAtlas emojiAtlals
    {
        get
        {
            return m_emojiAtlas;
        }
        set
        {
            m_emojiAtlas = value;
            SetMaterialDirty();
        }
    }

    protected EmojiImage()
    {
        useLegacyMeshGeneration = false;
    }
    private static readonly VertexHelper s_EmojiVertexHelper = new VertexHelper();

    /// <summary>
    /// Returns the texture used to draw this Graphic.
    /// </summary>
    public override Texture mainTexture
    {
        get
        {
            if (m_emojiAtlas == null)
            {
                if (material != null && material.mainTexture != null)
                {
                    return material.mainTexture;
                }
                return s_WhiteTexture;
            }

            return m_emojiAtlas.texture;
        }
    }

    protected override void UpdateGeometry()
    {
        
    }

    public void startMeshGeneration()
    {
        s_EmojiVertexHelper.Clear(); // clear the vertex helper so invalid graphics dont draw.
    }
    
    public void addUIVertexQuad(UIVertex[] verts)
    {
        s_EmojiVertexHelper.AddUIVertexQuad(verts);
    }

    public void endMeshGeneration()
    {
        s_EmojiVertexHelper.FillMesh(workerMesh);
        canvasRenderer.SetMesh(workerMesh);
    }
    
    protected override void OnDidApplyAnimationProperties()
    {
        SetMaterialDirty();
        SetVerticesDirty();
        SetRaycastDirty();
    }
}