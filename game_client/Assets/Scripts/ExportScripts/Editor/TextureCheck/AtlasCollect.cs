using System.Collections.Generic;
using UnityEditor;
using UnityEngine.U2D;


public class AtlasCollect
{
    private static AtlasCollect _g_instance = new AtlasCollect();
    public static AtlasCollect instance
    {
        get
        {
            if(null == _g_instance)
                _g_instance = new AtlasCollect();

            return _g_instance;
        }
    }
    private Dictionary<string, SpriteAtlas> _m_atlasMap = new Dictionary<string, SpriteAtlas>();
    protected AtlasCollect()
    {
        init();
    }

    public void init()
    {
        var atlasguids = AssetDatabase.FindAssets("t:SpriteAtlas");
        _m_atlasMap.Clear();
        foreach (var guid in atlasguids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            SpriteAtlas atlas = AssetDatabase.LoadMainAssetAtPath(path) as SpriteAtlas;
            var depences = AssetDatabase.GetDependencies(path);
            foreach (var depen in depences)
            {
                if (_m_atlasMap.ContainsKey(depen))
                {
                    UnityEngine.Debug.LogError($"存在打包在不同图集的图片{_m_atlasMap[depen].name}, {atlas.name}");
                }
                else
                {
                    _m_atlasMap.Add(depen, atlas);
                }
            }
            
        }
    }


    public string getAtlas(string _texPath)
    {
        SpriteAtlas atlas;
        if (_m_atlasMap.TryGetValue(_texPath, out atlas))
        {
            return atlas.name;
        }

        return "None";
    }
}