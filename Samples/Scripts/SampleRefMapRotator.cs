using System;
using GameMeanMachine.Unity.WindRose.RefMapChars.Authoring.Behaviours;
using GameMeanMachine.Unity.WindRose.RefMapChars.Authoring.ScriptableObjects;
using GameMeanMachine.Unity.WindRose.RefMapChars.Types;
using GameMeanMachine.Unity.WindRose.RefMapChars.Types.Traits;
using GameMeanMachine.Unity.WindRose.RefMapChars.Types.Traits.Standard;
using GameMeanMachine.Unity.WindRose.Authoring.Behaviours.Entities.Objects;
using GameMeanMachine.Unity.WindRose.Authoring.Behaviours.Entities.Visuals;
using GameMeanMachine.Unity.WindRose.Types;
using UnityEngine;
using Exception = GameMeanMachine.Unity.WindRose.Types.Exception;


[RequireComponent(typeof(MapObject))]
public class SampleRefMapRotator : MonoBehaviour
{
    [SerializeField]
    private bool moves;

    private MapObject mapObject;
    private Visual mainVisual;
    private RefMapStandardModelHolder modelHolder;

    private int sex = 0; // 0-1
    private int bodyColor = 0; // 0-7

    private ushort boots = 0;
    private int bootsColor = 0;
    private ushort pants = 0;
    private int pantsColor = 0;
    private ushort waist = 0;
    private int waistColor = 0;
    private ushort shirt = 0;
    private int shirtColor = 0;
    private ushort hat = 0;
    private int hatColor = 0;
    private ushort hair = 0;
    private int hairColor = 0;
    private ushort longShirt = 0;
    private int longShirtColor = 0;
    private ushort arms = 0;
    private int armsColor = 0;
    private ushort chest = 0;
    private int chestColor = 0;
    private ushort shoulder = 0;
    private int shoulderColor = 0;

    private bool bootsOverPants = false;

    private const int Sexes = 2;
    private const int BodyColors = 8;
    private const int ItemColors = 10;

    private const int MaleBoots = 3;
    private const int FemaleBoots = 1;
    private const int MalePants = 5;
    private const int FemalePants = 10;
    private const int MaleWaists = 4;
    private const int FemaleWaists = 2;
    private const int MaleShirts = 11;
    private const int FemaleShirts = 17;
    private const int MaleChests = 3;
    private const int FemaleChests = 3;
    private const int MaleShoulders = 13;
    private const int FemaleShoulders = 7;
    private const int MaleHats = 7;
    private const int FemaleHats = 8;
    private const int MaleHairs = 15;
    private const int FemaleHairs = 15;
    private const int MaleLongShirts = 9;
    private const int FemaleLongShirts = 9;
    private const int MaleArms = 8;
    private const int FemaleArms = 7;

    private void Awake()
    {
        mapObject = GetComponent<MapObject>();
        mainVisual = mapObject.MainVisual;
        modelHolder = mainVisual.GetComponent<RefMapStandardModelHolder>();
    }

    private void Start()
    {
        FixIndicesAndUpdate();
    }

    private void FixIndicesAndUpdate()
    {
        // First, rotating the colors is easy.
        if (bodyColor == 8) bodyColor = 0;
        if (bootsColor == 10) bootsColor = 0;
        if (pantsColor == 10) pantsColor = 0;
        if (waistColor == 10) waistColor = 0;
        if (shirtColor == 10) shirtColor = 0;
        if (chestColor == 10) chestColor = 0;
        if (longShirtColor == 10) longShirtColor = 0;
        if (shoulderColor == 10) shoulderColor = 0;
        if (armsColor == 10) armsColor = 0;
        if (hairColor == 10) hairColor = 0;
        if (hatColor == 10) hatColor = 0;
        
        // Rotating the models is more difficult.
        // If the value is now >= the cap, it will
        // become (-1) and the character will not
        // have a cloth of that type dressed.

        int maxBoots = sex == 0 ? MaleBoots : FemaleBoots;
        if (boots > maxBoots) boots = 0;
        int maxPants = sex == 0 ? MalePants : FemalePants;
        if (pants > maxPants) pants = 0;
        int maxWaits = sex == 0 ? MaleWaists : FemaleWaists;
        if (waist > maxWaits) waist = 0;
        int maxShirts = sex == 0 ? MaleShirts : FemaleShirts;
        if (shirt > maxShirts) shirt = 0;
        int maxChest = sex == 0 ? MaleChests : FemaleChests;
        if (chest > maxChest) chest = 0;
        int maxLongShirt = sex == 0 ? MaleLongShirts : FemaleLongShirts;
        if (longShirt > maxLongShirt) longShirt = 0;
        int maxShoulder = sex == 0 ? MaleShoulders : FemaleShoulders;
        if (shoulder > maxShoulder) shoulder = 0;
        int maxArms = sex == 0 ? MaleArms : FemaleArms;
        if (arms > maxArms) arms = 0;
        int maxHat = sex == 0 ? MaleHats : FemaleHats;
        if (hat > maxHat) hat = 0;
        int maxHair = sex == 0 ? MaleHairs : FemaleHairs;
        if (hair > maxHair) hair = 0;
        
        // Now, applying everything will take place.
        
        modelHolder.BodyColor = (RefMapBody.ColorCode) bodyColor;
        modelHolder.Boots = new Tuple<ushort, RefMapAddOn.ColorCode>(boots, (RefMapAddOn.ColorCode)bootsColor);
        modelHolder.Pants = new Tuple<ushort, RefMapAddOn.ColorCode>(pants, (RefMapAddOn.ColorCode)pantsColor);
        modelHolder.Waist = new Tuple<ushort, RefMapAddOn.ColorCode>(waist, (RefMapAddOn.ColorCode)waistColor);
        modelHolder.Shirt = new Tuple<ushort, RefMapAddOn.ColorCode>(shirt, (RefMapAddOn.ColorCode)shirtColor);
        modelHolder.Chest = new Tuple<ushort, RefMapAddOn.ColorCode>(chest, (RefMapAddOn.ColorCode)chestColor);
        modelHolder.LongShirt = new Tuple<ushort, RefMapAddOn.ColorCode>(longShirt, (RefMapAddOn.ColorCode)longShirtColor);
        modelHolder.Shoulder = new Tuple<ushort, RefMapAddOn.ColorCode>(shoulder, (RefMapAddOn.ColorCode)shoulderColor);
        modelHolder.Arms = new Tuple<ushort, RefMapAddOn.ColorCode>(arms, (RefMapAddOn.ColorCode)armsColor);
        modelHolder.Hat = new Tuple<ushort, RefMapAddOn.ColorCode>(hat, (RefMapAddOn.ColorCode)hatColor);
        // This example does not have Cloak.
        modelHolder.Hair = new Tuple<ushort, RefMapAddOn.ColorCode>(hair, (RefMapAddOn.ColorCode)hairColor);
        modelHolder.RefreshTexture();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Rotate Boots
            boots++;
            FixIndicesAndUpdate();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Rotate Pants
            pants++;
            FixIndicesAndUpdate();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Rotate Waist
            waist++;
            FixIndicesAndUpdate();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // Rotate Shirt
            shirt++;
            FixIndicesAndUpdate();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            // Rotate Chest
            chest++;
            FixIndicesAndUpdate();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            // Rotate LongShirt
            longShirt++;
            FixIndicesAndUpdate();
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            // Rotate Shoulder
            shoulder++;
            FixIndicesAndUpdate();
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            // Rotate Arms
            arms++;
            FixIndicesAndUpdate();
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            // Rotate Hair
            hair++;
            FixIndicesAndUpdate();
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            // Rotate Hat
            hat++;
            FixIndicesAndUpdate();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Rotate Boots Color
            bootsColor++;
            FixIndicesAndUpdate();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            // Rotate Pants Color
            pantsColor++;
            FixIndicesAndUpdate();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Rotate Waist Color
            waistColor++;
            FixIndicesAndUpdate();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Rotate Shirt Color
            shirtColor++;
            FixIndicesAndUpdate();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            // Rotate Chest Color
            chestColor++;
            FixIndicesAndUpdate();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            // Rotate LongShirt Color
            longShirtColor++;
            FixIndicesAndUpdate();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            // Rotate Shoulder Color
            shoulderColor++;
            FixIndicesAndUpdate();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            // Rotate Arms Color
            armsColor++;
            FixIndicesAndUpdate();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            // Rotate Hair Color
            hairColor++;
            FixIndicesAndUpdate();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Rotate Hat Color
            hatColor++;
            FixIndicesAndUpdate();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            // Toggle Boots over Pants
            bootsOverPants = !bootsOverPants;
            FixIndicesAndUpdate();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            // Toggle Sex
            sex = 1 - sex;
            modelHolder.Sex = (RefMapBundle.SexCode)sex;
            FixIndicesAndUpdate();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            // Rotate Body
            bodyColor++;
            FixIndicesAndUpdate();
        }

        // Movement starts here.
        
        if (Input.GetKey(KeyCode.DownArrow))
        {
            mapObject.Orientation = Direction.DOWN;
            if (moves) mapObject.StartMovement(Direction.DOWN);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            mapObject.Orientation = Direction.UP;
            if (moves) mapObject.StartMovement(Direction.UP);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            mapObject.Orientation = Direction.LEFT;
            if (moves) mapObject.StartMovement(Direction.LEFT);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            mapObject.Orientation = Direction.RIGHT;
            if (moves) mapObject.StartMovement(Direction.RIGHT);
        }
    }
}
