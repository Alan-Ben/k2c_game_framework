using ALPackage;
using NPEnum;
using SQLite4Unity3d;
using System.Collections.Generic;


/**************
 * 创角配表
 **/

[System.Serializable]
public class NPPlayerPrefabRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;

    public ENPGenderType gender_type;//性别字符串
    public List<long> avatar_id_list;//形象列表
    public NPGTextureIndex icon;
    public NPGGoIndex go_index;//裸模
    public NPGMaterialIndex skin_mat_index;//身体皮肤材质索引
    public NPGMaterialIndex face_mat_index;//面部材质索引
    public NPGSpriteIndex skin_icon;
    public long skin_color_id;
}

public class NPSOPlayerPrefabRefSet : _TALSOBasicRefSet<NPPlayerPrefabRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/player_refdata.unity3d"; } }
    public static string objName { get { return "player_prefab"; } }
}
