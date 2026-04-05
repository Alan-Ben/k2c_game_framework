using System;
using System.Collections.Generic;
using GOE;
using UnityEngine;

[Serializable]
public class GraveHallShow
{
    public long hallId;
    public List<GameObject> showGos;
}
// 杰出者大厅主界面
public class GTDGraveMainSceneMono : MonoBehaviour
{
    public Canvas canvas;
    public List<GraveHallShow> hallShowItems = new List<GraveHallShow>();
    public List<GTDGraveMainFollowMono> itemList = new List<GTDGraveMainFollowMono>();
    // <AutoGen:MonoDeclaration>
    // </AutoGen:MonoDeclaration>
}
