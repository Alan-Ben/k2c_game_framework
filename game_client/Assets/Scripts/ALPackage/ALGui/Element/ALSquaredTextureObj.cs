using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_UNITY_GUI
namespace ALPackage
{
    [System.Serializable]
    public class ALSquaredTextureObj
    {
        public Texture2D texture;
        public int leftBoard;
        public int rightBoard;
        public int topBoard;
        public int bottomBoard;
    }
}

#endif
