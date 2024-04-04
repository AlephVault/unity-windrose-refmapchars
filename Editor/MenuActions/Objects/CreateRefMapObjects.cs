using System.Collections.Generic;
using AlephVault.Unity.Layout.Utils;
using AlephVault.Unity.MenuActions.Types;
using AlephVault.Unity.MenuActions.Utils;
using AlephVault.Unity.Support.Utils;
using AlephVault.Unity.WindRose.Authoring.Behaviours.Entities.Common;
using AlephVault.Unity.WindRose.Authoring.Behaviours.Entities.Objects;
using AlephVault.Unity.WindRose.Authoring.Behaviours.Entities.Objects.CommandExchange;
using AlephVault.Unity.WindRose.Authoring.Behaviours.Entities.Objects.CommandExchange.Talk;
using AlephVault.Unity.WindRose.Authoring.Behaviours.Entities.Objects.Strategies;
using AlephVault.Unity.WindRose.Authoring.Behaviours.Entities.Objects.Strategies.Base;
using AlephVault.Unity.WindRose.Authoring.Behaviours.Entities.Objects.Strategies.Simple;
using AlephVault.Unity.WindRose.Authoring.Behaviours.Entities.Objects.Strategies.Solidness;
using AlephVault.Unity.WindRose.Authoring.Behaviours.Entities.Visuals;
using AlephVault.Unity.WindRose.Authoring.Behaviours.Entities.Visuals.StateBundles.Moving;
using AlephVault.Unity.WindRose.Authoring.Behaviours.World.Layers.Objects;
using AlephVault.Unity.WindRose.RefMapChars.Authoring.Behaviours;
using UnityEditor;
using UnityEngine;

namespace AlephVault.Unity.WindRose.RefMapChars
{
    namespace Editor
    {
        namespace Objects
        {
            public static class CreateRefMapObjects
            {
                private class CreateObjectWindow : SmartEditorWindow
                {
                    private static int[] addStrategyOptions = { 0, 1, 2 };
                    private static string[] addStrategyLabels = { "Simple (includes Solidness and Layout)", "Layout", "Nothing (will be added manually later)" };
                    private static int[] addTriggerOptions = { 0, 1 };
                    private static string[] addTriggerLabels = { "No trigger", "Live trigger" };
                    private static int[] objectTypeOptions = { 0, 1 };
                    private static string[] objectTypeLabels = { "Statue (not a moving character)", "Moving character" };
                    private static int[] clothTypeOptions = { 0, 1 };
                    private static string[] clothTypeLabels = { "Simple (one single .Cloth setting)", "Standard (cloth specified in many settings)" };

                    public Transform selectedTransform;
                    // Basic properties.
                    private string objectName = "New Object";
                    // Optional behaviours to send commands.
                    private bool addCommandSender = false;
                    private bool addTalkSender = false; // depends on addCommandSender.
                    // Optional behaviours for trigger type.
                    private int addTrigger = 0;
                    // Optional behaviours for when trigger type = activator.
                    private bool addCommandReceiver = false; // depends on addTrigger == 1.
                    private bool addTalkReceiver = false; // depends on addCommandReceiver.
                    // Object strategy setup.
                    private int addStrategy = 0;
                    // Type of object: Status (static) or Character.
                    private int objectType = 0;
                    // Type of cloth: Simple (one setting) or Standard (many settings).
                    private int clothType = 0;
                    // The level of the visual.
                    private ushort visualLevel = 1 << 14;

                    protected override float GetSmartWidth()
                    {
                        return 500;
                    }
                    
                    protected override void OnAdjustedGUI()
                    {
                        GUIStyle longLabelStyle = MenuActionUtils.GetSingleLabelStyle();
                        GUIStyle indentedStyle = MenuActionUtils.GetIndentedStyle();
                        
                        // General settings start here.

                        titleContent = new GUIContent("WindRose - Creating a new object");
                        EditorGUILayout.LabelField("This wizard will create an object in the hierarchy of the current scene, under the selected objects layer in the hierarchy.", longLabelStyle);

                        // Object properties.

                        EditorGUILayout.LabelField("This is the name the game object will have when added to the hierarchy.", longLabelStyle);
                        objectName = MenuActionUtils.EnsureNonEmpty(EditorGUILayout.TextField("Object name", objectName), "New Object");

                        EditorGUILayout.LabelField("These are the object properties in the editor. Can be changed later.", longLabelStyle);

                        addCommandSender = EditorGUILayout.ToggleLeft("Close Command Sender (Provides feature to send a custom command to close objects)", addCommandSender);
                        if (addCommandSender)
                        {
                            EditorGUILayout.BeginVertical(indentedStyle);
                            addTalkSender = EditorGUILayout.ToggleLeft("Talk Sender (A particular close command sender that dispatches a talk command to NPCs)", addTalkSender);
                            EditorGUILayout.EndVertical();
                        }
                        addTrigger = EditorGUILayout.IntPopup("Trigger Type", addTrigger, addTriggerLabels, addTriggerOptions);
                        if (addTrigger == 1)
                        {
                            EditorGUILayout.BeginVertical(indentedStyle);
                            addCommandReceiver = EditorGUILayout.ToggleLeft("Command Receiver (Provides feature to NPCs to receive a custom command)", addCommandReceiver);
                            if (addCommandReceiver)
                            {
                                EditorGUILayout.BeginVertical(indentedStyle);
                                addTalkReceiver = EditorGUILayout.ToggleLeft("Talk Receiver (A particular command receiver that understands a talk command)", addTalkReceiver);
                                EditorGUILayout.EndVertical();
                            }
                            EditorGUILayout.EndVertical();
                        }
                        addStrategy = EditorGUILayout.IntPopup("Object Strategy", addStrategy, addStrategyLabels, addStrategyOptions);
                        EditorGUILayout.LabelField(
                            "This object will be created with RefMep visuals and behaviours. Choose whether it " +
                            "will be a moving character or a static statue"
                        );
                        objectType = EditorGUILayout.IntPopup("Object Type", objectType, objectTypeLabels, objectTypeOptions);
                        EditorGUILayout.LabelField(
                            "Also, choose whether the cloth system will be simple (involve only one cloth, e.g. " +
                            "Argentum Online) or standard (involve clothes by parts, e.g. Habbo Hotel)"
                        );
                        clothType = EditorGUILayout.IntPopup("Cloth Type", clothType, clothTypeLabels, clothTypeOptions);
                        EditorGUILayout.LabelField("Finally, set the visual level");
                        visualLevel = (ushort)Values.Clamp(0, EditorGUILayout.IntField("Level [1 to 32767]", visualLevel), (1 << 15) - 1);
                        SmartButton("Create Object", Execute);
                    }

                    private void Execute()
                    {
                        GameObject gameObject = new GameObject(objectName);
                        gameObject.transform.parent = selectedTransform;
                        gameObject.SetActive(false);
                        Behaviours.AddComponent<MapObject>(gameObject, new Dictionary<string, object>() {
                            { "width", (ushort)1 },
                            { "height", (ushort)1 }
                        });
                        gameObject.AddComponent<ObjectStrategyHolder>();
                        if (addCommandSender)
                        {
                            Behaviours.AddComponent<CloseCommandSender>(gameObject);
                            if (addTalkSender)
                            {
                                Behaviours.AddComponent<TalkSender>(gameObject);
                            }
                        }
                        switch (addTrigger)
                        {
                            case 1:
                                Behaviours.AddComponent<BoxCollider>(gameObject);
                                Behaviours.AddComponent<Rigidbody>(gameObject);
                                Behaviours.AddComponent<TriggerLive>(gameObject);
                                if (addCommandReceiver)
                                {
                                    Behaviours.AddComponent<CommandReceiver>(gameObject);
                                    if (addTalkReceiver)
                                    {
                                        Behaviours.AddComponent<TalkReceiver>(gameObject);
                                    }
                                }
                                break;
                        }
                        ObjectStrategy mainStrategy = null;
                        switch(addStrategy)
                        {
                            case 0:
                                Behaviours.AddComponent<LayoutObjectStrategy>(gameObject);
                                Behaviours.AddComponent<SolidnessObjectStrategy>(gameObject);
                                mainStrategy = Behaviours.AddComponent<SimpleObjectStrategy>(gameObject);
                                break;
                            case 1:
                                mainStrategy = Behaviours.AddComponent<LayoutObjectStrategy>(gameObject);
                                break;
                        }
                        ObjectStrategyHolder currentHolder = gameObject.GetComponent<ObjectStrategyHolder>();
                        Behaviours.SetObjectFieldValues(currentHolder, new Dictionary<string, object>()
                        {
                            { "objectStrategy", mainStrategy }
                        });

                        GameObject visualGameObject = new GameObject(objectName + " Visual");
                        visualGameObject.transform.parent = gameObject.transform;
                        visualGameObject.SetActive(false);
                        Behaviours.AddComponent<Pausable>(visualGameObject);
                        Behaviours.AddComponent<SpriteRenderer>(visualGameObject);
                        Behaviours.AddComponent<Visual>(visualGameObject, new Dictionary<string, object>() {
                            { "level", visualLevel }
                        });
                        if (objectType == 0)
                        {
                            // Statue object.
                            Behaviours.AddComponent<RoseSprited>(visualGameObject);
                            if (clothType == 0)
                            {
                                // Simple.
                                Behaviours.AddComponent<RefMapSimpleStatueApplier>(visualGameObject);
                                Behaviours.AddComponent<RefMapSimpleModelHolder>(visualGameObject);
                            }
                            else
                            {
                                // Standard.
                                Behaviours.AddComponent<RefMapStandardStatueApplier>(visualGameObject);
                                Behaviours.AddComponent<RefMapStandardModelHolder>(visualGameObject);
                            }
                        }
                        else
                        {
                            // Character object.
                            Behaviours.AddComponent<Animated>(visualGameObject);
                            Behaviours.AddComponent<RoseAnimated>(visualGameObject);
                            Behaviours.AddComponent<MultiRoseAnimated>(visualGameObject);
                            Behaviours.AddComponent<MovingAnimationRoseBundle>(visualGameObject);
                            if (clothType == 0)
                            {
                                // Simple.
                                Behaviours.AddComponent<RefMapSimpleCharacterApplier>(visualGameObject);
                                Behaviours.AddComponent<RefMapSimpleModelHolder>(visualGameObject);
                            }
                            else
                            {
                                // Standard.
                                Behaviours.AddComponent<RefMapStandardCharacterApplier>(visualGameObject);
                                Behaviours.AddComponent<RefMapStandardModelHolder>(visualGameObject);
                            }
                        }
                        gameObject.SetActive(true);
                        visualGameObject.SetActive(true);
                        Undo.RegisterCreatedObjectUndo(gameObject, "Create Object");
                    }
                }
                
                [MenuItem("GameObject/Aleph Vault/WindRose/RefMap Chars/Objects/Create Object", false, 116)]
                public static void CreateStandardRefMapObject()
                {
                    CreateObjectWindow window = ScriptableObject.CreateInstance<CreateObjectWindow>();
                    window.position = new Rect(60, 180, 700, 468);
                    window.minSize = new Vector2(700, 215);
                    window.maxSize = window.minSize;
                    window.selectedTransform = Selection.activeTransform;
                    // window.position = new Rect(new Vector2(57, 336), new Vector2(689, 138));
                    window.ShowUtility();
                }

                [MenuItem("GameObject/Aleph Vault/WindRose/RefMap Chars/Objects/Create Object", true)]
                public static bool CanCreateStandardRefMapObject()
                {
                    return Selection.activeTransform && Selection.activeTransform.GetComponent<ObjectsLayer>();
                }
            }
        }
    }
}