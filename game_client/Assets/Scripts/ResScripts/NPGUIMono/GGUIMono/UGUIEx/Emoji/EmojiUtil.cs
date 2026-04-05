using System.Collections.Generic;
using System.Text;
using UnityEngine.Events;

namespace CEmoji
{
    
    public class ObjectPool<T> where T : new()
    {
        private readonly Stack<T> m_Stack = new Stack<T>();
        private readonly UnityAction<T> m_ActionOnGet;
        private readonly UnityAction<T> m_ActionOnRelease;

        public int countAll { get; private set; }
        public int countActive { get { return countAll - countInactive; } }
        public int countInactive { get { return m_Stack.Count; } }

        public ObjectPool(UnityAction<T> actionOnGet=null, UnityAction<T> actionOnRelease=null)
        {
            m_ActionOnGet = actionOnGet;
            m_ActionOnRelease = actionOnRelease;
        }

        public T Get()
        {
            T element;
            if (m_Stack.Count == 0)
            {
                element = new T();
                countAll++;
            }
            else
            {
                element = m_Stack.Pop();
            }
            if (m_ActionOnGet != null)
                m_ActionOnGet(element);
            return element;
        }

        public void Release(T element, bool checkRepeat = true)
        {
            // if (m_Stack.Count > 0 && checkRepeat && ReferenceEquals(m_Stack.Peek(), element))
            // 	Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
            if (m_ActionOnRelease != null)
                m_ActionOnRelease(element);
            m_Stack.Push(element);
        }

    }

    
    public static class StringBuilderPool
    {
        static readonly ObjectPool<StringBuilder> s_strBuilder = new ObjectPool<StringBuilder>(null, s => s.Length = 0);

        public static StringBuilder Get(int length = 0)
        {
            StringBuilder sb = s_strBuilder.Get();
            if (sb.Capacity < length)
                sb.Capacity = length;
            return sb;
        }

        public static void Release(StringBuilder toRelease)
        {
            s_strBuilder.Release(toRelease);
        }
    }
}