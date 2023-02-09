using System;
using GameMeanMachine.Unity.WindRose.RefMapChars.Authoring.Behaviours;
using GameMeanMachine.Unity.WindRose.RefMapChars.Authoring.ScriptableObjects;

namespace GameMeanMachine.Unity.WindRose.RefMapChars
{
    namespace Types
    {
        /// <summary>
        ///   This stands for a data record to populate an
        ///   instance of <see cref="RefMapStandardModelHolder"/>.
        /// </summary>
        public interface IRefMapStandardModel : IRefMapBaseModel
        {
            /// <summary>
            ///   Data of the boots to use. It can be null.
            /// </summary>
            public Tuple<ushort, RefMapAddOn.ColorCode> Boots { get; }
            
            /// <summary>
            ///   Data of the pants to use. It can be null.
            /// </summary>
            public Tuple<ushort, RefMapAddOn.ColorCode> Pants { get; }
            
            /// <summary>
            ///   Data of the shirt to use. It can be null.
            /// </summary>
            public Tuple<ushort, RefMapAddOn.ColorCode> Shirt { get; }
            
            /// <summary>
            ///   Data of the chest to use. It can be null.
            /// </summary>
            public Tuple<ushort, RefMapAddOn.ColorCode> Chest { get; }
            
            /// <summary>
            ///   Data of the waist to use. It can be null.
            /// </summary>
            public Tuple<ushort, RefMapAddOn.ColorCode> Waist { get; }
            
            /// <summary>
            ///   Data of the arms to use. It can be null.
            /// </summary>
            public Tuple<ushort, RefMapAddOn.ColorCode> Arms { get; }
            
            /// <summary>
            ///   Data of the waist to use. It can be null.
            /// </summary>
            public Tuple<ushort, RefMapAddOn.ColorCode> LongShirt { get; }
            
            /// <summary>
            ///   Data of the shoulder to use. It can be null.
            /// </summary>
            public Tuple<ushort, RefMapAddOn.ColorCode> Shoulder { get; }
            
            /// <summary>
            ///   Data of the cloak to use. It can be null.
            /// </summary>
            public Tuple<ushort, RefMapAddOn.ColorCode> Cloak { get; }
        }
    }
}