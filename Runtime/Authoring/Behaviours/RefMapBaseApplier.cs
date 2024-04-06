using AlephVault.Unity.WindRose.RefMapChars.Authoring.ScriptableObjects;
using AlephVault.Unity.WindRose.RefMapChars.Types;
using AlephVault.Unity.WindRose.RefMapChars.Types.Traits;
using UnityEditor;
using UnityEngine;


namespace AlephVault.Unity.WindRose.RefMapChars
{
    namespace Authoring
    {
        namespace Behaviours
        {
            /// <summary>
            ///   A base RefMAp applier. This applier only makes use of body,
            ///   hair, hat, necklace, and hand tools. Clothes must be defined
            ///   later, in children classes. Also, what to do with the child
            ///   classes must be decided later (e.g. full-moving characters,
            ///   or only-pointing characters).
            /// </summary>
            public abstract class RefMapBaseApplier : MonoBehaviour, IRefMapBaseComposite,
                IApplier<BodyTrait>, IApplier<HairTrait>, IApplier<HatTrait>, IApplier<NecklaceTrait>,
                IApplier<SkilledHandItemTrait>, IApplier<DumbHandItemTrait>
            {
#if UNITY_EDITOR
                public Color gizmoColor = Color.blue;
#endif

                /// <summary>
                ///   The cache to use. This value must be
                ///   set at edit / prefab time.
                /// </summary>
                [SerializeField]
                protected RefMapCache cache;
                
                /// <summary>
                ///   The body image.
                /// </summary>
                public RefMapSource Body => bodyTrait?.Front;

                /// <summary>
                ///   The hair image. It does not include the tail.
                /// </summary>
                public RefMapSource Hair => hairTrait?.Front;

                /// <summary>
                ///   The hair tail image. Not all hairs have tail.
                /// </summary>
                public RefMapSource HairTail => hairTrait?.Back;

                /// <summary>
                ///   The necklace image.
                /// </summary>
                public RefMapSource Necklace => necklaceTrait?.Front;

                /// <summary>
                ///   The hat image.
                /// </summary>
                public RefMapSource Hat => hatTrait?.Front;

                /// <summary>
                ///   The skilled hand item (e.g. weapon) image.
                /// </summary>
                public RefMapSource SkilledHandItem => skilledHandItemTrait?.Front;

                /// <summary>
                ///   The dumb hand item (e.g. shield) image.
                /// </summary>
                public RefMapSource DumbHandItem => dumbHandItemTrait?.Front;

                /// <summary>
                ///   The body trait.
                /// </summary>
                protected BodyTrait bodyTrait;

                /// <summary>
                ///   The necklace trait.
                /// </summary>
                protected NecklaceTrait necklaceTrait;

                /// <summary>
                ///   The hair trait.
                /// </summary>
                protected HairTrait hairTrait;

                /// <summary>
                ///   The hat trait.
                /// </summary>
                protected HatTrait hatTrait;

                /// <summary>
                ///   The skilled hand item trait.
                /// </summary>
                protected SkilledHandItemTrait skilledHandItemTrait;
                
                /// <summary>
                ///   The dumb hand item trait.
                /// </summary>
                protected DumbHandItemTrait dumbHandItemTrait;

                /// <summary>
                ///   A hash function that describes all the assigned
                ///   traits - not just those defined here.
                /// </summary>
                /// <returns>The hash of the current setting</returns>
                public abstract string Hash();

                /// <summary>
                ///   Applies a body trait. When passing null, it clears
                ///   the body trait.
                /// </summary>
                /// <param name="appliance">The appliance to set</param>
                /// <param name="force">Whether to force the update or not</param>
                public void Use(BodyTrait appliance, bool force = true)
                {
                    bodyTrait = appliance;
                    if (force) RefreshTexture();
                }

                /// <summary>
                ///   Applies a hair trait (which may include tail). When
                ///   passing null, it clears the hair trait (which may
                ///   include clearing also the tail, if present).
                /// </summary>
                /// <param name="appliance">The appliance to set</param>
                /// <param name="force">Whether to force the update or not</param>
                public void Use(HairTrait appliance, bool force = true)
                {
                    hairTrait = appliance;
                    if (force) RefreshTexture();
                }

                /// <summary>
                ///   Applies a hat trait. When passing null, it clears
                ///   the hat trait.
                /// </summary>
                /// <param name="appliance">The appliance to set</param>
                /// <param name="force">Whether to force the update or not</param>
                public void Use(HatTrait appliance, bool force = true)
                {
                    hatTrait = appliance;
                    if (force) RefreshTexture();
                }

                /// <summary>
                ///   Applies a necklace trait. When passing null, it clears
                ///   the necklace trait.
                /// </summary>
                /// <param name="appliance">The appliance to set</param>
                /// <param name="force">Whether to force the update or not</param>
                public void Use(NecklaceTrait appliance, bool force = true)
                {
                    necklaceTrait = appliance;
                    if (force) RefreshTexture();
                }

                /// <summary>
                ///   Applies a skilled hand tool trait. When passing null,
                ///   it clears the skilled hand tool trait.
                /// </summary>
                /// <param name="appliance">The appliance to set</param>
                /// <param name="force">Whether to force the update or not</param>
                public void Use(SkilledHandItemTrait appliance, bool force = true)
                {
                    skilledHandItemTrait = appliance;
                    if (force) RefreshTexture();
                }

                /// <summary>
                ///   Applies a dumb hand tool trait. When passing null,
                ///   it clears the dumb hand tool trait.
                /// </summary>
                /// <param name="appliance">The appliance to set</param>
                /// <param name="force">Whether to force the update or not</param>
                public void Use(DumbHandItemTrait appliance, bool force = true)
                {
                    dumbHandItemTrait = appliance;
                    if (force) RefreshTexture();
                }

                public abstract void RefreshTexture();
                
                                
                [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
                public static void DrawContour(RefMapBaseApplier obj, GizmoType gizmoType)
                {
                    Vector3 bottomLeft = obj.transform.position;
                    Vector3 bottomRight = obj.transform.TransformPoint(Vector3.right);
                    Vector3 topLeft = obj.transform.TransformPoint(Vector3.up * 1.5f);
                    Vector3 topRight = obj.transform.TransformPoint(Vector3.up * 1.5f + Vector3.right);
                    
                    Gizmos.color = obj.gizmoColor;
                    Gizmos.DrawLine(bottomLeft, bottomRight);
                    Gizmos.DrawLine(topLeft, topRight);
                    Gizmos.DrawLine(bottomLeft, topLeft);
                    Gizmos.DrawLine(bottomRight, topRight);
                    Gizmos.DrawLine(bottomLeft, topRight);
                    Gizmos.DrawLine(topLeft, bottomRight);
                }
            }
        }
    }
}