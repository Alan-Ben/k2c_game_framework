using System;
using System.Collections.Generic;

namespace ALPackage
{
    public class ALResourceDataCore
    {
        private static ALResourceDataCore g_instance;

        public static ALResourceDataCore Instance
        {
            get
            {
                if (null == g_instance)
                    g_instance = new ALResourceDataCore();

                return g_instance;
            }
        }

        public ALTexture2DResourceMgr textureMgr;
        public ALGameObjectResourceMgr gameObjectMgr;

        protected ALResourceDataCore()
        {
            textureMgr = new ALTexture2DResourceMgr();

            gameObjectMgr = new ALGameObjectResourceMgr();
        }
    }
}
