using System;
using GameMeanMachine.Unity.WindRose.RefMapChars.Authoring.Behaviours;
using GameMeanMachine.Unity.WindRose.RefMapChars.Authoring.ScriptableObjects;
using JetBrains.Annotations;

namespace GameMeanMachine.Unity.WindRose.RefMapChars
{
    namespace Types
    {
        /// <summary>
        ///   This stands for a data record to populate an
        ///   instance of <see cref="RefMapSimpleModelHolder"/>.
        /// </summary>
        public interface IRefMapSimpleModel : IRefMapBaseModel
        {
            /// <summary>
            ///   Data of the cloth to use. It can be null.
            /// </summary>
            [CanBeNull]
            public Tuple<ushort, RefMapItem.ColorCode> Cloth { get; }
        }
    }
}