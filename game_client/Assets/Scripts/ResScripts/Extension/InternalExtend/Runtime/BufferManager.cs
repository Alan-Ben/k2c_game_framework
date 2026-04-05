using Unity.Collections;
using UnityEngine;

namespace GOE.U2D
{
    
    public class NativeByteArray
    {
        public int Length => array.Length;
        public bool IsCreated => array.IsCreated;
        public byte this[int index] => array[index];

        public NativeArray<byte> array
        { get; }

        public NativeByteArray(NativeArray<byte> array)
        {
            this.array = array;
        }

        public void Dispose() => array.Dispose();
    }
    public class VertexBuffer
    {
        /// <summary>
        /// Number of buffers currently allocated.
        /// </summary>        
        public int bufferCount => m_Buffers.Length;
        
        private readonly int m_Id;
        private bool m_IsActive = true;
        private int m_DeactivateFrame = -1;
        
        private NativeByteArray[] m_Buffers;
        private int m_ActiveIndex = 0;

        public VertexBuffer(int id, int size, bool needDoubleBuffering)
        {
            m_Id = id;
            
            var noOfBuffers = needDoubleBuffering ? 2 : 1;
            m_Buffers = new NativeByteArray[noOfBuffers];
            for (var i = 0; i < noOfBuffers; i++)
                m_Buffers[i] = new NativeByteArray(new NativeArray<byte>(size, Allocator.Persistent, NativeArrayOptions.UninitializedMemory));
        }

        public override int GetHashCode() => m_Id;
        private static int GetCurrentFrame() => Time.frameCount;

        public NativeByteArray GetBuffer(int size)
        {
            if (!m_IsActive)
            {
                Debug.LogError($"Cannot request deactivated buffer. ID: {m_Id}");
                return null;
            }

            m_ActiveIndex = (m_ActiveIndex + 1) % m_Buffers.Length;
            if (m_Buffers[m_ActiveIndex].Length != size)
                ResizeBuffer(m_ActiveIndex, size);
                
            return m_Buffers[m_ActiveIndex];
        }

        private void ResizeBuffer(int bufferId, int newSize)
        {
            m_Buffers[bufferId].Dispose();
            m_Buffers[bufferId] = new NativeByteArray(new NativeArray<byte>(newSize, Allocator.Persistent, NativeArrayOptions.UninitializedMemory));
        }

        public void Deactivate()
        {
            if (!m_IsActive)
                return;
            
            m_IsActive = false;
            m_DeactivateFrame = GetCurrentFrame();
        }

        public void Dispose()
        {
            for (var i = 0; i < m_Buffers.Length; i++)
            {
                if (m_Buffers[i].IsCreated)
                    m_Buffers[i].Dispose();
            }
        }
        
        public bool IsSafeToDispose() => !m_IsActive && GetCurrentFrame() > m_DeactivateFrame;
    }
}