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
                [CustomPropertyDrawer(typeof(RefMapAddOn.RefMapVariationsDictionary))]
                [CustomPropertyDrawer(typeof(RefMapAddOnType.RefMapAddOnsDictionary))]
                [CustomPropertyDrawer(typeof(RefMapItemType.RefMapItemsDictionary))]
                [CustomPropertyDrawer(typeof(RefMapSex.RefMapAddOnTypesDictionary))]
                [CustomPropertyDrawer(typeof(RefMapSex.RefMapItemTypesDictionary))]
                [CustomPropertyDrawer(typeof(RefMapBundle.RefMapSexDictionary))]
                public class RefMapDictionaryPropertyDrawer : DictionaryPropertyDrawer {}
            }
        }
    }
}

