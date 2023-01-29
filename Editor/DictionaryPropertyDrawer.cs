using AlephVault.Unity.Support.Generic.Authoring.Types;
using UnityEditor;


namespace GameMeanMachine.Unity.WindRose.RefMapChars
{
    namespace Authoring
    {
        namespace ScriptableObjects
        {
            namespace Standard
            {
                [CustomPropertyDrawer(typeof(RefMapBody.RefMapVariationsDictionary))]
                [CustomPropertyDrawer(typeof(RefMapItem.RefMapVariationsDictionary))]
                [CustomPropertyDrawer(typeof(RefMapItemType.RefMapItemsDictionary))]
                [CustomPropertyDrawer(typeof(RefMapSex.RefMapItemTypesDictionary))]
                [CustomPropertyDrawer(typeof(RefMapBundle.RefMapSexDictionary))]
                [CustomPropertyDrawer(typeof(RefMapBundle.RefMapExtraDictionary))]
                [CustomPropertyDrawer(typeof(RefMapExtra.RefMapExtraItemsDictionary))]
                public class RefMapDictionaryPropertyDrawer : DictionaryPropertyDrawer {}
            }
        }
    }
}

