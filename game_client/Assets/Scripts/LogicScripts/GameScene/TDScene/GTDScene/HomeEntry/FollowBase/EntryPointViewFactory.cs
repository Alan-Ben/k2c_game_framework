using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 入口对象的表现类工厂
    /// </summary>
    public class EntryPointViewFactory
    {
        private static EntryPointViewFactory _g_instance;
        [NotNull] public static EntryPointViewFactory instance { get { return _g_instance ??= new EntryPointViewFactory(); } }
        
        [NotNull]private Dictionary<Type, Func<_AGTDHomeEntryPointMono_Base, _IGTDHoneEntryPointView>> _m_entryPointViewDict = new Dictionary<Type, Func<_AGTDHomeEntryPointMono_Base, _IGTDHoneEntryPointView>>();

        public EntryPointViewFactory()
        {
            _m_entryPointViewDict.Add(typeof(GTDHomeEntryPointMono_Common), (_mono) => new GTDHomeEntryPointView_Common(_mono as GTDHomeEntryPointMono_Common));
            _m_entryPointViewDict.Add(typeof(GTDHomeEntryPointMono_AvatarGacha), (_mono) => new GTDHomeEntryPointView_AvatarGacha(_mono as GTDHomeEntryPointMono_AvatarGacha));
            _m_entryPointViewDict.Add(typeof(GTDHomeEntryPointMono_SelfDinner), (_mono) => new GTDHomeEntryPointView_SelfDinner(_mono as GTDHomeEntryPointMono_SelfDinner));
            _m_entryPointViewDict.Add(typeof(GTDHomeEntryPointMono_DinnerList), (_mono) => new GTDHomeEntryPointView_DinnerList(_mono as GTDHomeEntryPointMono_DinnerList));
            _m_entryPointViewDict.Add(typeof(GTDHomeEntryPointMono_Recruit), (_mono) => new GTDHomeEntryPointView_Recruit(_mono as GTDHomeEntryPointMono_Recruit));
        }

        /// <summary>
        /// 获取对应入口表现类
        /// </summary>
        /// <param name="_mono"></param>
        /// <returns></returns>
        public _IGTDHoneEntryPointView createEntryPointView(_AGTDHomeEntryPointMono_Base _mono)
        {
            if (null == _mono)
                return null;
            
            if (_m_entryPointViewDict.TryGetValue(_mono.GetType(), out var _func))
            {
                if (_func != null) 
                    return _func(_mono);
            }
            return null;
        } 
    }
}