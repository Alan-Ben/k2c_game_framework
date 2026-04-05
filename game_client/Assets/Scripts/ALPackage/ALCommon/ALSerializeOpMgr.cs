namespace ALPackage
{
    public static class ALSerializeOpMgr
    {
        private static int _g_serializeOp = 1;

        public static int next()
        {
            _g_serializeOp++;
            return _g_serializeOp;
        }
    }
}