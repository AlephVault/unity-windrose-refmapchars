using System;
using AlephVault.Unity.WindRose.RefMapChars.Authoring.Behaviours;
using AlephVault.Unity.WindRose.RefMapChars.Authoring.ScriptableObjects;


namespace AlephVault.Unity.WindRose.RefMapChars
{
    namespace Types
    {
        /// <summary>
        ///   This stands for a data record to populate an
        ///   instance of <see cref="RefMapBaseModelHolder"/>.
        /// </summary>
        public interface IRefMapBaseModel
        {
            /// <summary>
            ///   Data of the body color to use.
            /// </summary>
            public RefMapBody.ColorCode? BodyColor { get; }

            /// <summary>
            ///   Data of the sex to have.
            /// </summary>
            public RefMapBundle.SexCode? Sex { get; }

            /// <summary>
            ///   Data of the hair to use. It can be null.
            /// </summary>
            public Tuple<ushort, RefMapAddOn.ColorCode> Hair { get; }

            /// <summary>
            ///   Data of the hat to use. It can be null.
            /// </summary>
            public Tuple<ushort, RefMapAddOn.ColorCode> Hat { get; }

            /// <summary>
            ///   Data of the necklace item to use.
            /// </summary>
            public ushort? Necklace { get; }

            /// <summary>
            ///   Data of the skilled hand item to use.
            /// </summary>
            public ushort? SkilledHandItem { get; }

            /// <summary>
            ///   Data of the dumb hand item to use.
            /// </summary>
            public ushort? DumbHandItem { get; }
        }
    }
}