using System;

namespace GOE
{
    /// <summary>
    /// NPC-Actor类型子表
    /// </summary>
    [Serializable]
    public class NPNPCActorRefObj
    {
        public long id;//唯一id
        public long actor_id;//单位id
        public long skin_property_id;//皮肤属性id

        public long Id { get { return id; } set { id = value; } }
        public long ActorId { get { return actor_id; } set { actor_id = value; } }
        public long SkinPropertyId { get { return skin_property_id; } set { skin_property_id = value; } }

        public static string assetPath { get { return "refdata_db/npc.unity3d"; } }
        public static string objName { get { return "refdata_db/npc_actor.txt"; } }
        public static string tableName { get { return "npc_actor"; } }
    }
}