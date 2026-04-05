namespace GOE
{
    public interface _ICameraCustomMovement
    {
        /// <summary>
        /// 是否要打断上一个相机移动操作
        /// </summary>
        bool interruptPreviousMovement { get; }

        /// <summary>
        /// 当这个移动生效时的处理
        /// </summary>
        void enableMovement();
        /// <summary>
        /// 当这个移动失效时的处理
        /// </summary>
        void disableMovement();
    }
}