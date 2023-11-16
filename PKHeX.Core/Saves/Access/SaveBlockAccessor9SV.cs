using System.Collections.Generic;
// ReSharper disable UnusedMember.Local
#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable RCS1213 // Remove unused member declaration.

namespace PKHeX.Core;

/// <summary>
/// Information for Accessing individual blocks within a <see cref="SAV9SV"/>.
/// </summary>
public sealed class SaveBlockAccessor9SV : SCBlockAccessor, ISaveBlock9Main
{
    public override IReadOnlyList<SCBlock> BlockInfo { get; }
    public Box8 BoxInfo { get; }
    public Party9 PartyInfo { get; }
    public MyItem9 Items { get; }
    public MyStatus9 MyStatus { get; }
    public BoxLayout9 BoxLayout { get; }
    public PlayTime9 Played { get; }
    public Zukan9 Zukan { get; }
    public ConfigSave9 Config { get; }
    public ConfigCamera9 ConfigCamera { get; }
    public TeamIndexes9 TeamIndexes { get; }
    public Epoch1970Value LastSaved { get; }
    public PlayerFashion9 PlayerFashion { get; }
    public PlayerAppearance9 PlayerAppearance { get; }
    public RaidSpawnList9 RaidPaldea { get; }
    public RaidSpawnList9 RaidKitakami { get; }
    public RaidSevenStar9 RaidSevenStar { get; }
    public Epoch1900Value EnrollmentDate { get; }

    public SaveBlockAccessor9SV(SAV9SV sav)
    {
        BlockInfo = sav.AllBlocks;
        BoxInfo = new Box8(sav, GetBlock(KBox));
        PartyInfo = new Party9(sav, GetBlock(KParty));
        Items = new MyItem9(sav, GetBlock(KItem));
        BoxLayout = new BoxLayout9(sav, GetBlock(KBoxLayout));
        MyStatus = new MyStatus9(sav, GetBlock(KMyStatus));
        Played = new PlayTime9(sav, GetBlock(KPlayTime));
        Zukan = new Zukan9(sav, GetBlock(KZukan), GetBlockSafe(KZukanT1));
        Config = new ConfigSave9(sav, GetBlock(KConfig));
        ConfigCamera = new ConfigCamera9(sav, GetBlockSafe(KConfigCamera));
        TeamIndexes = new TeamIndexes9(sav, GetBlock(KTeamIndexes));
        LastSaved = new Epoch1970Value(GetBlock(KLastSaved));
        PlayerFashion = new PlayerFashion9(sav, GetBlock(KCurrentClothing));
        PlayerAppearance = new PlayerAppearance9(sav, GetBlock(KCurrentAppearance));
        RaidPaldea = new RaidSpawnList9(sav, GetBlock(KTeraRaidPaldea), RaidSpawnList9.RaidCountLegal_T0, true);
        RaidKitakami = new RaidSpawnList9(sav, GetBlockSafe(KTeraRaidKitakami), RaidSpawnList9.RaidCountLegal_T1, false);
        RaidSevenStar = new RaidSevenStar9(sav, GetBlock(KSevenStarRaidsCapture));
        EnrollmentDate = new Epoch1900Value(GetBlock(KEnrollmentDate));
    }

    // Arrays (Blocks)
    private const uint KTeamIndexes = 0x33F39467; // Team Indexes for competition
    private const uint KBoxLayout = 0x19722c89; // Box Names
    public const uint KBoxWallpapers = 0x2EB1B190; // Box Wallpapers

    // Objects (Blocks)
    private const uint KBox = 0x0d66012c; // Box Data
    private const uint KParty = 0x3AA1A9AD; // Party Data
    public const uint KCurrentBox = 0x017C3CBB; // U32 Box Index
    public const uint KMoney = 0x4F35D0DD; // u32
    public const uint KLeaguePoints = 0xADB4FE17; // u32
    private const uint KMyStatus = 0xE3E89BD1; // Trainer Details
    private const uint KConfig = 0xDF4F1875; // u32 bits
    private const uint KConfigCamera = 0x998844C9; // u32 bits
    private const uint KItem = 0x21C9BD44; // Items
    private const uint KPlayTime = 0xEDAFF794; // Time Played
    private const uint KSessionLength = 0x1522C79C; // Milliseconds(?) elapsed
    private const uint KOverworld = 0x173304D8; // [0x158+7C][20] overworld Pokémon
    private const uint KGimmighoul = 0x53DC955C; // ulong seed x2 (today and tomorrow); Gimmighoul struct (0x20): bool is_active, u64 hash, u64 seed, bool ??, bool first_time
    private const uint KTeraRaidPaldea = 0xCAAC8800;
    private const uint KTeraRaidKitakami = 0x100B93DA;
    public const uint KBoxesUnlocked = 0x71825204;
    public const uint KFusedCalyrex = 0x916BCA9E; // Calyrex
    private const uint KZukan = 0x0DEAAEBD;
    private const uint KZukanT1 = 0xF5D7C0E2;
    private const uint KMysteryGift = 0x99E1625E;
    private const uint KDLCGifts = 0xA4B7A814; // Unix timestamp, 1 byte type of gift (0 = Pokémon, 1 = Item, 2 = Apparel)
    private const uint KLastSaved = 0x7495969E; // u64 time_t
    private const uint KEnrollmentDate = 0xC7409C89;
    private const uint KPlayRecords = 0x549B6033; // 0x18 per entry, first 8 bytes always 01, u64 fnv hash of entry, last 8 bytes value
    private const uint KSandwiches = 0x29B4AED2; // [0xC][151] index, unlocked, times made
    private const uint KCurrentClothing = 0x64235B3D;
    private const uint KCurrentAppearance = 0x812FC3E3;
    private const uint KCurrentRotomPhoneCase = 0x1433CED7;
    private const uint KRentalTeams = 0x19CB0339;
    private const uint KRentalTeamCodes = 0xB476F6D4;
    private const uint KSevenStarRaidsCapture = 0x8B14392F; // prior to 2.0.1, this also stored defeat history
    private const uint KSevenStarRaidsDefeat = 0xA4BA4848; // 2.0.1 onward stores defeat history separately from capture history

    // BCAT (Tera Raid Battles)
    private const uint KBCATRaidFixedRewardItemArray = 0x7D6C2B82; // fixed_reward_item_array
    private const uint KBCATRaidLotteryRewardItemArray = 0xA52B4811; // lottery_reward_item_array
    private const uint KBCATRaidEnemyArray = 0x0520A1B0; // raid_enemy_array
    private const uint KBCATRaidPriorityArray = 0x095451E4; // raid_priority_array
    private const uint KBCATRaidIdentifier = 0x37B99B4D; // VersionNo

    // BCAT (Mass Outbreaks)
    private const uint KBCATOutbreakZonesPaldea = 0x3FDC5DFF; // zone_main_array
    private const uint KBCATOutbreakZonesKitakami = 0xF9F156A3; // zone_su1_array
    private const uint KBCATOutbreakZonesBlueberry = 0x1B45E41C; // zone_su2_array
    private const uint KBCATOutbreakPokeData = 0x6C1A131B; // pokedata_array
    private const uint KBCATOutbreakEnabled = 0x61552076;

    // PlayerSave
    public const uint KCoordinates = 0x708D1511; // PlayerSave_StartPosition
    public const uint KPlayerRotation = 0x31EF132C; // PlayerSave_StartRotation
    private const uint KPlayerIsInField = 0x32645CB7; // PlayerSave_IsInField
    private const uint KPlayerLastSubField = 0x37AF0454; // PlayerSave_LastSubField
    private const uint KPlayerLastRoomMapName = 0x9F1ABF26; // PlayerSave_LastRoomMapName
    private const uint KPlayerLastGreenPosition = 0x5C6F8291; // PlayerSave_LastGreenPos
    private const uint KPlayerCurrentFieldID = 0xF17EB014; // PlayerSave_CurrentFieldId (0 = Paldea, 1 = Kitakami, 2 = Blueberry)

    // Fashion
    public const uint KFashionUnlockedEyewear = 0xCBA20ED5; // 1000-1999
    public const uint KFashionUnlockedGloves = 0x581667B1; // 2000-2999
    public const uint KFashionUnlockedBag = 0x5D6F8110; // 3000-3999
    public const uint KFashionUnlockedFootwear = 0x0221A618; // 4000-4999
    public const uint KFashionUnlockedHeadwear = 0x860CD8FB; // 5000-5999
    public const uint KFashionUnlockedLegwear = 0xD186222E; // 6000-6999
    public const uint KFashionUnlockedClothing = 0x78FF2CB2; // 7000-7999
    public const uint KFashionUnlockedPhoneCase = 0xED0AC675; // 8000-8999

    // Profile Picture
    public const uint KPictureProfileCurrentWidth = 0xFEAA87DA;
    public const uint KPictureProfileCurrentHeight = 0x5361CEB5;
    public const uint KPictureProfileCurrentSize = 0x1E505002;
    public const uint KPictureProfileCurrent = 0x14C5A101;

    public const uint KPictureProfileInitialWidth = 0xC0408301;
    public const uint KPictureProfileInitialHeight = 0xCAABBA50;
    public const uint KPictureProfileInitialSize = 0x902B64DB;
    public const uint KPictureProfileInitial = 0xF8B14C88;

    // Trainer Icon
    public const uint KPictureIconCurrentWidth = 0x8FAB2C4D;
    public const uint KPictureIconCurrentHeight = 0x0B384C24;
    public const uint KPictureIconCurrentSize = 0xEA677637;
    public const uint KPictureIconCurrent = 0xD41F4FC4;

    public const uint KPictureIconInitialWidth = 0x6850A672;
    public const uint KPictureIconInitialHeight = 0x74077ECD;
    public const uint KPictureIconInitialSize = 0x83FC126A;
    public const uint KPictureIconInitial = 0xBCB6F239;

    #region YMAP - Display & Fly Flags -- internal name Fnv1aHash32
    public const uint FSYS_YMAP_FLY_01 = 0xEB597C90;
    public const uint FSYS_YMAP_FLY_02 = 0xEB5981A9;
    public const uint FSYS_YMAP_FLY_03 = 0xEB597FF6;
    public const uint FSYS_YMAP_FLY_04 = 0xEB59850F;
    public const uint FSYS_YMAP_FLY_05 = 0xEB59835C;
    public const uint FSYS_YMAP_FLY_06 = 0xEB598875;
    public const uint FSYS_YMAP_FLY_07 = 0xEB5986C2;
    public const uint FSYS_YMAP_FLY_08 = 0xEB598BDB;
    public const uint FSYS_YMAP_FLY_09 = 0xEB598A28;
    public const uint FSYS_YMAP_FLY_10 = 0xEB569B1A;
    public const uint FSYS_YMAP_FLY_11 = 0xEB569CCD;
    public const uint FSYS_YMAP_FLY_12 = 0xEB5697B4;
    public const uint FSYS_YMAP_FLY_13 = 0xEB569967;
    public const uint FSYS_YMAP_FLY_14 = 0xEB56944E;
    public const uint FSYS_YMAP_FLY_15 = 0xEB569601;
    public const uint FSYS_YMAP_FLY_16 = 0xEB5690E8;
    public const uint FSYS_YMAP_FLY_17 = 0xEB56929B;
    public const uint FSYS_YMAP_FLY_18 = 0xEB568D82;
    public const uint FSYS_YMAP_FLY_19 = 0xEB568F35;
    public const uint FSYS_YMAP_FLY_20 = 0xEB538191;
    public const uint FSYS_YMAP_FLY_21 = 0xEB537FDE;
    public const uint FSYS_YMAP_FLY_22 = 0xEB537E2B;
    public const uint FSYS_YMAP_FLY_23 = 0xEB537C78;
    public const uint FSYS_YMAP_FLY_24 = 0xEB53885D;
    public const uint FSYS_YMAP_FLY_25 = 0xEB5386AA;
    public const uint FSYS_YMAP_FLY_26 = 0xEB5384F7;
    public const uint FSYS_YMAP_FLY_27 = 0xEB538344;
    public const uint FSYS_YMAP_FLY_28 = 0xEB5373F9;
    public const uint FSYS_YMAP_FLY_29 = 0xEB537246;
    public const uint FSYS_YMAP_FLY_30 = 0xEB506808;
    public const uint FSYS_YMAP_FLY_31 = 0xEB5069BB;
    public const uint FSYS_YMAP_FLY_32 = 0xEB506B6E;
    public const uint FSYS_YMAP_FLY_33 = 0xEB506D21;
    public const uint FSYS_YMAP_FLY_34 = 0xEB506ED4;
    public const uint FSYS_YMAP_FLY_35 = 0xEB507087;
    public const uint FSYS_YMAP_FLY_MAGATAMA = 0x1530B53C;
    public const uint FSYS_YMAP_FLY_MOKKAN = 0x4103198E;
    public const uint FSYS_YMAP_FLY_TSURUGI = 0x9DDEC36C;
    public const uint FSYS_YMAP_FLY_UTSUWA = 0x1EB92B72;
    public const uint FSYS_YMAP_POKECEN_02 = 0xB6119AE9;
    public const uint FSYS_YMAP_POKECEN_03 = 0xB6119936;
    public const uint FSYS_YMAP_POKECEN_04 = 0xB6119E4F;
    public const uint FSYS_YMAP_POKECEN_05 = 0xB6119C9C;
    public const uint FSYS_YMAP_POKECEN_06 = 0xB611A1B5;
    public const uint FSYS_YMAP_POKECEN_07 = 0xB611A002;
    public const uint FSYS_YMAP_POKECEN_08 = 0xB611A51B;
    public const uint FSYS_YMAP_POKECEN_09 = 0xB611A368;
    public const uint FSYS_YMAP_POKECEN_10 = 0xB60EB45A;
    public const uint FSYS_YMAP_POKECEN_11 = 0xB60EB60D;
    public const uint FSYS_YMAP_POKECEN_12 = 0xB60EB0F4;
    public const uint FSYS_YMAP_POKECEN_13 = 0xB60EB2A7;
    public const uint FSYS_YMAP_POKECEN_14 = 0xB60EAD8E;
    public const uint FSYS_YMAP_POKECEN_15 = 0xB60EAF41;
    public const uint FSYS_YMAP_POKECEN_16 = 0xB60EAA28;
    public const uint FSYS_YMAP_POKECEN_17 = 0xB60EABDB;
    public const uint FSYS_YMAP_POKECEN_18 = 0xB60EA6C2;
    public const uint FSYS_YMAP_POKECEN_19 = 0xB60EA875;
    public const uint FSYS_YMAP_POKECEN_20 = 0xB60B9AD1;
    public const uint FSYS_YMAP_POKECEN_21 = 0xB60B991E;
    public const uint FSYS_YMAP_POKECEN_22 = 0xB60B976B;
    public const uint FSYS_YMAP_POKECEN_23 = 0xB60B95B8;
    public const uint FSYS_YMAP_POKECEN_24 = 0xB60BA19D;
    public const uint FSYS_YMAP_POKECEN_25 = 0xB60B9FEA;
    public const uint FSYS_YMAP_POKECEN_26 = 0xB60B9E37;
    public const uint FSYS_YMAP_POKECEN_27 = 0xB60B9C84;
    public const uint FSYS_YMAP_POKECEN_28 = 0xB60B8D39;
    public const uint FSYS_YMAP_POKECEN_29 = 0xB60B8B86;
    public const uint FSYS_YMAP_POKECEN_30 = 0xB6088148;
    public const uint FSYS_YMAP_POKECEN_31 = 0xB60882FB;
    public const uint FSYS_YMAP_POKECEN_32 = 0xB60884AE;
    public const uint FSYS_YMAP_POKECEN_33 = 0xB6088661;
    public const uint FSYS_YMAP_POKECEN_34 = 0xB6088814;
    public const uint FSYS_YMAP_POKECEN_35 = 0xB60889C7;
    public const uint FSYS_YMAP_FLY_SU1_AREA10 = 0x7DFB08F3;
    public const uint FSYS_YMAP_FLY_SU1_BUSSTOP = 0xA92B960F;
    public const uint FSYS_YMAP_FLY_SU1_CENTER01 = 0x477F8C2D;
    public const uint FSYS_YMAP_FLY_SU1_PLAZA = 0xF94F91E7;
    public const uint FSYS_YMAP_FLY_SU1_SPOT01 = 0xB59008C4;
    public const uint FSYS_YMAP_FLY_SU1_SPOT02 = 0xB5900DDD;
    public const uint FSYS_YMAP_FLY_SU1_SPOT03 = 0xB5900C2A;
    public const uint FSYS_YMAP_FLY_SU1_SPOT04 = 0xB59003AB;
    public const uint FSYS_YMAP_FLY_SU1_SPOT05 = 0xB59001F8;
    public const uint FSYS_YMAP_FLY_SU1_SPOT06 = 0xB5900711;
    public const uint FSYS_YMAP_SU1MAP_CHANGE = 0x69284BE7;

    private const uint FSYS_YMAP_SCENARIO_DAN_AKU = 0x1EE90D7F;
    private const uint FSYS_YMAP_SCENARIO_DAN_DOKU = 0x6D2CE931;
    private const uint FSYS_YMAP_SCENARIO_DAN_FAIRY = 0xFDC82445;
    private const uint FSYS_YMAP_SCENARIO_DAN_FINAL = 0xAC189200;
    private const uint FSYS_YMAP_SCENARIO_DAN_FINAL_02 = 0xE52F230F;
    private const uint FSYS_YMAP_SCENARIO_DAN_FINAL_03 = 0xE52F215C;
    private const uint FSYS_YMAP_SCENARIO_DAN_HONOO = 0x6C3A6D77;
    private const uint FSYS_YMAP_SCENARIO_DAN_KAKUTOU = 0x1D6BC8F8;
    private const uint FSYS_YMAP_SCENARIO_GYM_DENKI = 0x82305257;
    private const uint FSYS_YMAP_SCENARIO_GYM_DENKI_02 = 0x0D109FBE;
    private const uint FSYS_YMAP_SCENARIO_GYM_DENKI_03 = 0x0D10A171;
    private const uint FSYS_YMAP_SCENARIO_GYM_CLEAR_DENKI = 0x815F4601;
    private const uint FSYS_YMAP_SCENARIO_GYM_ESPER = 0xB80A6CE3;
    private const uint FSYS_YMAP_SCENARIO_GYM_ESPER_02 = 0x53F2E6DA;
    private const uint FSYS_YMAP_SCENARIO_GYM_ESPER_03 = 0x53F2E88D;
    private const uint FSYS_YMAP_SCENARIO_GYM_CLEAR_ESPER = 0xD0249A05;
    private const uint FSYS_YMAP_SCENARIO_GYM_FINAL = 0xCCB617B0;
    private const uint FSYS_YMAP_SCENARIO_GYM_FINAL_02 = 0xBE37FC3F;
    private const uint FSYS_YMAP_SCENARIO_GYM_GHOST = 0xCF20EE67;
    private const uint FSYS_YMAP_SCENARIO_GYM_GHOST_02 = 0xDBB4D1CE;
    private const uint FSYS_YMAP_SCENARIO_GYM_GHOST_03 = 0xDBB4D381;
    private const uint FSYS_YMAP_SCENARIO_GYM_CLEAR_GHOST = 0x1C5C88A5;
    private const uint FSYS_YMAP_SCENARIO_GYM_KOORI = 0x9197F3C2;
    private const uint FSYS_YMAP_SCENARIO_GYM_KOORI_02 = 0x4B6AC965;
    private const uint FSYS_YMAP_SCENARIO_GYM_KOORI_03 = 0x4B6AC7B2;
    private const uint FSYS_YMAP_SCENARIO_GYM_CLEAR_KOORI = 0x0A9299BC;
    private const uint FSYS_YMAP_SCENARIO_GYM_KUSA = 0x0E554BFC;
    private const uint FSYS_YMAP_SCENARIO_GYM_KUSA_02 = 0xFF8FF65B;
    private const uint FSYS_YMAP_SCENARIO_GYM_KUSA_03 = 0xFF8FF4A8;
    private const uint FSYS_YMAP_SCENARIO_GYM_CLEAR_KUSA = 0x26216312;
    private const uint FSYS_YMAP_SCENARIO_GYM_MIZU = 0xCE698EA7;
    private const uint FSYS_YMAP_SCENARIO_GYM_MIZU_02 = 0x7DF6B20E;
    private const uint FSYS_YMAP_SCENARIO_GYM_MIZU_03 = 0x7DF6B3C1;
    private const uint FSYS_YMAP_SCENARIO_GYM_CLEAR_MIZU = 0x16452421;
    private const uint FSYS_YMAP_SCENARIO_GYM_MUSI = 0x9319C51A;
    private const uint FSYS_YMAP_SCENARIO_GYM_MUSI_02 = 0xE8A9553D;
    private const uint FSYS_YMAP_SCENARIO_GYM_MUSI_03 = 0xE8A9538A;
    private const uint FSYS_YMAP_SCENARIO_GYM_CLEAR_MUSHI = 0x8485AC3A;
    private const uint FSYS_YMAP_SCENARIO_GYM_NORMAL = 0xD78448EB;
    private const uint FSYS_YMAP_SCENARIO_GYM_NORMAL_02 = 0xA019AFC2;
    private const uint FSYS_YMAP_SCENARIO_GYM_NORMAL_03 = 0xA019B175;
    private const uint FSYS_YMAP_SCENARIO_GYM_CLEAR_NORMAL = 0x9457390D;
    public const uint FSYS_YMAP_MAGATAMA = 0x159DC19E;
    public const uint FSYS_YMAP_MOKKAN = 0x22587DBC;
    public const uint FSYS_YMAP_TSURUGI = 0x6BA41372;
    public const uint FSYS_YMAP_UTSUWA = 0xDD041500;
    private const uint FSYS_YMAP_SCENARIO_NUSI_DRAGON = 0x181058F9;
    private const uint FSYS_YMAP_SCENARIO_NUSI_DRAGON_02 = 0xE0D86B9C;
    private const uint FSYS_YMAP_SCENARIO_NUSHI_FINAL = 0x9F36BA1E;
    private const uint FSYS_YMAP_SCENARIO_NUSHI_FINAL_02 = 0xBA776E51;
    private const uint FSYS_YMAP_SCENARIO_NUSI_HAGANE = 0x259D1760;
    private const uint FSYS_YMAP_SCENARIO_NUSI_HAGANE_02 = 0xF362822F;
    private const uint FSYS_YMAP_SCENARIO_NUSI_HIKOU = 0xAC3EC55A;
    private const uint FSYS_YMAP_SCENARIO_NUSI_IWA = 0xC593AD1D;
    private const uint FSYS_YMAP_SCENARIO_NUSI_IWA_02 = 0xDDEBE5F0;
    private const uint FSYS_YMAP_SCENARIO_NUSI_JIMEN = 0xD08966DD;
    private const uint FSYS_YMAP_SCENARIO_NUSI_JIMEN_02 = 0x269C23B0;
    private const uint FSYS_YMAP_SCENARIO_00 = 0x4019CED4;
    private const uint FSYS_YMAP_SCENARIO_01 = 0x4019D087;
    private const uint FSYS_YMAP_SCENARIO_02 = 0x4019D23A;
    private const uint FSYS_YMAP_SCENARIO_03 = 0x4019D3ED;
    private const uint FSYS_YMAP_SCENARIO_04 = 0x4019C808;
    private const uint FSYS_YMAP_SCENARIO_05 = 0x4019C9BB;
    private const uint FSYS_YMAP_SCENARIO_06 = 0x4019CB6E;
    private const uint FSYS_YMAP_SCENARIO_07 = 0x4019CD21;
    private const uint FSYS_YMAP_SCENARIO_08 = 0x4019C13C;
    private const uint FSYS_YMAP_SCENARIO_COMMON_0060 = 0x6CCF3E58;
    private const uint FSYS_YMAP_SCENARIO_COMMON_0090 = 0x6CCCFE4F;
    private const uint FSYS_YMAP_SCENARIO_COMMON_0095 = 0x6CCCF5D0;
    private const uint FSYS_YMAP_SCENARIO_COMMON_0100 = 0x71C8FCB9;
    private const uint FSYS_YMAP_SCENARIO_COMMON_0130 = 0x71CC1642;
    private const uint FSYS_YMAP_SCENARIO_COMMON_0170 = 0x71D76C86;
    private const uint FSYS_YMAP_SCENARIO_COMMON_0185 = 0x71DFE4BA;
    private const uint FSYS_YMAP_SCENARIO_COMMON_0190 = 0x71DCC618;
    private const uint FSYS_YMAP_SCENARIO_COMMON_0210 = 0x625B5001;
    private const uint FSYS_YMAP_SCENARIO_COMMON_0220 = 0x625D900A;
    private const uint FSYS_YMAP_SCENARIO_COMMON_0225 = 0x625D8AF1;
    private const uint FSYS_YMAP_SCENARIO_COMMON_0990 = 0x49B67710;
    private const uint FSYS_YMAP_SCENARIO_COMMON_1010 = 0x5FB71156;
    private const uint FSYS_YMAP_SCENARIO_COMMON_2000 = 0x86D1D28C;
    private const uint FSYS_YMAP_SCENARIO_COMMON_2030 = 0x86DB1F27;
    private const uint FSYS_YMAP_SCENARIO_COMMON_2070 = 0x86CF9283;
    private const uint FSYS_YMAP_SCENARIO_COMMON_2080 = 0x86BB2604;
    #endregion

    #region BGM
    private const uint KUnlockedMultiplayerBGM01 = 0x7276003E; // FSYS_BGM_VS_SELECT_01
    private const uint KUnlockedMultiplayerBGM02 = 0x7275FE8B; // FSYS_BGM_VS_SELECT_02
    private const uint KUnlockedMultiplayerBGM03 = 0x7275FCD8; // FSYS_BGM_VS_SELECT_03
    private const uint KUnlockedMultiplayerBGM04 = 0x727608BD; // FSYS_BGM_VS_SELECT_04
    private const uint KUnlockedMultiplayerBGM05 = 0x7276070A; // FSYS_BGM_VS_SELECT_05
    private const uint KUnlockedMultiplayerBGM06 = 0x72760557; // FSYS_BGM_VS_SELECT_06
    private const uint KUnlockedMultiplayerBGM07 = 0x727603A4; // FSYS_BGM_VS_SELECT_07
    private const uint KUnlockedMultiplayerBGM08 = 0x7275F459; // FSYS_BGM_VS_SELECT_08
    private const uint KUnlockedMultiplayerBGM09 = 0x7275F2A6; // FSYS_BGM_VS_SELECT_09
    private const uint KUnlockedMultiplayerBGM10 = 0x7273C1E8; // FSYS_BGM_VS_SELECT_10
    private const uint KUnlockedMultiplayerBGM11 = 0x7273C39B; // FSYS_BGM_VS_SELECT_11
    private const uint KUnlockedMultiplayerBGM12 = 0x7273C54E; // FSYS_BGM_VS_SELECT_12
    private const uint KUnlockedMultiplayerBGM13 = 0x7273C701; // FSYS_BGM_VS_SELECT_13
    private const uint KUnlockedMultiplayerBGM14 = 0x7273C8B4; // FSYS_BGM_VS_SELECT_14
    private const uint KUnlockedMultiplayerBGM15 = 0x7273CA67; // FSYS_BGM_VS_SELECT_15
    private const uint KUnlockedMultiplayerBGM16 = 0x7273CC1A; // FSYS_BGM_VS_SELECT_16
    private const uint KUnlockedMultiplayerBGM17 = 0x7273CDCD; // FSYS_BGM_VS_SELECT_17
    private const uint KUnlockedMultiplayerBGM18 = 0x7273B450; // FSYS_BGM_VS_SELECT_18
    private const uint KUnlockedMultiplayerBGM19 = 0x7273B603; // FSYS_BGM_VS_SELECT_19
    private const uint KUnlockedMultiplayerBGM20 = 0x727BFEA3; // FSYS_BGM_VS_SELECT_20
    private const uint KUnlockedMultiplayerBGM21 = 0x727BFCF0; // FSYS_BGM_VS_SELECT_21
    private const uint KUnlockedMultiplayerBGM22 = 0x727C0209; // FSYS_BGM_VS_SELECT_22
    private const uint KUnlockedMultiplayerBGM23 = 0x727C0056; // FSYS_BGM_VS_SELECT_23
    private const uint KUnlockedMultiplayerBGM24 = 0x727C056F; // FSYS_BGM_VS_SELECT_24
    private const uint KUnlockedMultiplayerBGM25 = 0x727C03BC; // FSYS_BGM_VS_SELECT_25
    private const uint KUnlockedMultiplayerBGM26 = 0x727C08D5; // FSYS_BGM_VS_SELECT_26
    #endregion

    #region Tips
    private const uint FSYS_TIPS_NEW_HOWTO_01 = 0x8D0B961A;
    private const uint FSYS_TIPS_NEW_YMAP_01 = 0xB6B855D0;
    private const uint FSYS_TIPS_NEW_POKEDEX_01 = 0x1CF8C0C5;
    private const uint FSYS_TIPS_NEW_POKEDEX_02 = 0x1CF8BBAC;
    private const uint FSYS_TIPS_NEW_BOX_01 = 0xC075A4D0;
    private const uint FSYS_TIPS_NEW_POKEGETTUTORIAL_01 = 0x06F21620;
    private const uint FSYS_TIPS_NEW_POKEBATTLE_01 = 0xC255FC84;
    private const uint FSYS_TIPS_NEW_LETSGO_01 = 0x1B768D79;
    private const uint FSYS_TIPS_NEW_TREASUREHUNT_01 = 0x6E40A4E1;
    private const uint FSYS_TIPS_NEW_DESTINATION_01 = 0xD23E6A57;
    private const uint FSYS_TIPS_NEW_CROSSKEY_01 = 0x93E7F4A0;
    private const uint FSYS_TIPS_NEW_LOCKON_01 = 0x81281903;
    private const uint FSYS_TIPS_NEW_QUICKRECOVERY_01 = 0x7F53596B;
    private const uint FSYS_TIPS_NEW_HIDEWALK_01 = 0x07F7526E;
    private const uint FSYS_TIPS_NEW_WAZAREMEMBER_01 = 0xA74B89AB;
    private const uint FSYS_TIPS_NEW_POKERUSH_01 = 0x950B300C;
    private const uint FSYS_TIPS_NEW_UNIONCIRCLE_01 = 0xD087BBD6;
    private const uint FSYS_TIPS_NEW_TTYPE_01 = 0x1D3E0B23;
    private const uint FSYS_TIPS_NEW_PHOTOMODE_01 = 0x1756CFD2;
    private const uint FSYS_TIPS_NEW_SHOPWAZAMACHINE_01 = 0x13EE2DB5;
    private const uint FSYS_TIPS_NEW_POKEPICNIC_01 = 0x2C0929E0;
    private const uint FSYS_TIPS_NEW_COOKING_01 = 0x56AF4A17;
    private const uint FSYS_TIPS_NEW_POKEWASH_01 = 0xA14D9063;
    private const uint FSYS_TIPS_NEW_LEAGUCARD_01 = 0x2BC75897;
    private const uint FSYS_TIPS_NEW_RAIDSPOT_01 = 0x1803F741;
    private const uint FSYS_TIPS_NEW_RAIDBATTLE_01 = 0xD864A863;
    private const uint FSYS_TIPS_NEW_DANBATTLE_01 = 0x5839AFD8;
    private const uint FSYS_TIPS_NEW_BATTLESTADIUM_02 = 0x36C706A1;
    private const uint FSYS_TIPS_NEW_BATTLESTADIUM_03 = 0x36C704EE;
    private const uint FSYS_TIPS_NEW_BATTLESTADIUM_04 = 0x36C70A07;
    private const uint FSYS_TIPS_NEW_MYSTERY_01 = 0x709264C4;
    private const uint FSYS_TIPS_NEW_RIDEON_01 = 0x43543908;
    private const uint FSYS_TIPS_NEW_RIDEON_02 = 0x43543E21;
    private const uint FSYS_TIPS_NEW_RIDEDASH_01 = 0x714A76A3;
    private const uint FSYS_TIPS_NEW_RIDEJUMP_01 = 0xEEDD8AA3;
    private const uint FSYS_TIPS_NEW_RIDECLIMB_01 = 0x4690A9A6;
    private const uint FSYS_TIPS_NEW_RIDEGLID_01 = 0x2C808979;
    private const uint FSYS_TIPS_NEW_RIDEFLOAT_01 = 0xDC51E1CB;
    private const uint FSYS_TIPS_NEW_RIDEDASH_02 = 0x714A7856;
    private const uint FSYS_TIPS_NEW_RIDEJUMP_02 = 0xEEDD8C56;
    private const uint FSYS_TIPS_NEW_RIDECLIMB_02 = 0x4690A7F3;
    private const uint FSYS_TIPS_NEW_RIDEGLID_02 = 0x2C808460;
    private const uint FSYS_TIPS_NEW_RIDEFLOAT_02 = 0xDC51E37E;
    private const uint FSYS_TIPS_NEW_RIDEFORM_01 = 0x54689EBD;
    private const uint FSYS_TIPS_NEW_RIDEFORM_02 = 0x546899A4;
    private const uint FSYS_TIPS_NEW_RENTALTEAM_01 = 0x4CDE35CC;
    private const uint FSYS_TIPS_NEW_TSYMBOL_01 = 0x316CB3C3;
    private const uint FSYS_TIPS_NEW_GSRULE_01 = 0xD89BDB81;
    private const uint FSYS_TIPS_NEW_POKETRADE_01 = 0x0B85E5F8;
    private const uint FSYS_TIPS_NEW_JUDGE_01 = 0x6E0C61B2;
    private const uint FSYS_TIPS_NEW_RAIDBATTLE_02 = 0xD864AA16;
    private const uint FSYS_TIPS_DISP_GO_01 = 0xCFC538C9;
    private const uint FSYS_TIPS_NEWS_GO_01 = 0xE2096F66;
    #endregion

    #region TM
    private const uint KCanCraftTM001 = 0xF28D370B; // FSYS_UI_WAZA_MACHINE_RELEASE_001
    private const uint KCanCraftTM002 = 0xF28D38BE; // FSYS_UI_WAZA_MACHINE_RELEASE_002
    private const uint KCanCraftTM003 = 0xF28D3A71; // FSYS_UI_WAZA_MACHINE_RELEASE_003
    private const uint KCanCraftTM004 = 0xF28D3C24; // FSYS_UI_WAZA_MACHINE_RELEASE_004
    private const uint KCanCraftTM005 = 0xF28D3DD7; // FSYS_UI_WAZA_MACHINE_RELEASE_005
    private const uint KCanCraftTM006 = 0xF28D3F8A; // FSYS_UI_WAZA_MACHINE_RELEASE_006
    private const uint KCanCraftTM007 = 0xF28D413D; // FSYS_UI_WAZA_MACHINE_RELEASE_007
    private const uint KCanCraftTM008 = 0xF28D27C0; // FSYS_UI_WAZA_MACHINE_RELEASE_008
    private const uint KCanCraftTM009 = 0xF28D2973; // FSYS_UI_WAZA_MACHINE_RELEASE_009
    private const uint KCanCraftTM010 = 0xF2904EE1; // FSYS_UI_WAZA_MACHINE_RELEASE_010
    private const uint KCanCraftTM011 = 0xF2904D2E; // FSYS_UI_WAZA_MACHINE_RELEASE_011
    private const uint KCanCraftTM012 = 0xF2904B7B; // FSYS_UI_WAZA_MACHINE_RELEASE_012
    private const uint KCanCraftTM013 = 0xF29049C8; // FSYS_UI_WAZA_MACHINE_RELEASE_013
    private const uint KCanCraftTM014 = 0xF29055AD; // FSYS_UI_WAZA_MACHINE_RELEASE_014
    private const uint KCanCraftTM015 = 0xF29053FA; // FSYS_UI_WAZA_MACHINE_RELEASE_015
    private const uint KCanCraftTM016 = 0xF2905247; // FSYS_UI_WAZA_MACHINE_RELEASE_016
    private const uint KCanCraftTM017 = 0xF2905094; // FSYS_UI_WAZA_MACHINE_RELEASE_017
    private const uint KCanCraftTM018 = 0xF2904149; // FSYS_UI_WAZA_MACHINE_RELEASE_018
    private const uint KCanCraftTM019 = 0xF2903F96; // FSYS_UI_WAZA_MACHINE_RELEASE_019
    private const uint KCanCraftTM020 = 0xF293686A; // FSYS_UI_WAZA_MACHINE_RELEASE_020
    private const uint KCanCraftTM021 = 0xF2936A1D; // FSYS_UI_WAZA_MACHINE_RELEASE_021
    private const uint KCanCraftTM022 = 0xF2936504; // FSYS_UI_WAZA_MACHINE_RELEASE_022
    private const uint KCanCraftTM023 = 0xF29366B7; // FSYS_UI_WAZA_MACHINE_RELEASE_023
    private const uint KCanCraftTM024 = 0xF293619E; // FSYS_UI_WAZA_MACHINE_RELEASE_024
    private const uint KCanCraftTM025 = 0xF2936351; // FSYS_UI_WAZA_MACHINE_RELEASE_025
    private const uint KCanCraftTM026 = 0xF2935E38; // FSYS_UI_WAZA_MACHINE_RELEASE_026
    private const uint KCanCraftTM027 = 0xF2935FEB; // FSYS_UI_WAZA_MACHINE_RELEASE_027
    private const uint KCanCraftTM028 = 0xF2935AD2; // FSYS_UI_WAZA_MACHINE_RELEASE_028
    private const uint KCanCraftTM029 = 0xF2935C85; // FSYS_UI_WAZA_MACHINE_RELEASE_029
    private const uint KCanCraftTM030 = 0xF2964B93; // FSYS_UI_WAZA_MACHINE_RELEASE_030
    private const uint KCanCraftTM031 = 0xF29649E0; // FSYS_UI_WAZA_MACHINE_RELEASE_031
    private const uint KCanCraftTM032 = 0xF2964EF9; // FSYS_UI_WAZA_MACHINE_RELEASE_032
    private const uint KCanCraftTM033 = 0xF2964D46; // FSYS_UI_WAZA_MACHINE_RELEASE_033
    private const uint KCanCraftTM034 = 0xF296525F; // FSYS_UI_WAZA_MACHINE_RELEASE_034
    private const uint KCanCraftTM035 = 0xF29650AC; // FSYS_UI_WAZA_MACHINE_RELEASE_035
    private const uint KCanCraftTM036 = 0xF29655C5; // FSYS_UI_WAZA_MACHINE_RELEASE_036
    private const uint KCanCraftTM037 = 0xF2965412; // FSYS_UI_WAZA_MACHINE_RELEASE_037
    private const uint KCanCraftTM038 = 0xF296592B; // FSYS_UI_WAZA_MACHINE_RELEASE_038
    private const uint KCanCraftTM039 = 0xF2965778; // FSYS_UI_WAZA_MACHINE_RELEASE_039
    private const uint KCanCraftTM040 = 0xF299651C; // FSYS_UI_WAZA_MACHINE_RELEASE_040
    private const uint KCanCraftTM041 = 0xF29966CF; // FSYS_UI_WAZA_MACHINE_RELEASE_041
    private const uint KCanCraftTM042 = 0xF2996882; // FSYS_UI_WAZA_MACHINE_RELEASE_042
    private const uint KCanCraftTM043 = 0xF2996A35; // FSYS_UI_WAZA_MACHINE_RELEASE_043
    private const uint KCanCraftTM044 = 0xF2995E50; // FSYS_UI_WAZA_MACHINE_RELEASE_044
    private const uint KCanCraftTM045 = 0xF2996003; // FSYS_UI_WAZA_MACHINE_RELEASE_045
    private const uint KCanCraftTM046 = 0xF29961B6; // FSYS_UI_WAZA_MACHINE_RELEASE_046
    private const uint KCanCraftTM047 = 0xF2996369; // FSYS_UI_WAZA_MACHINE_RELEASE_047
    private const uint KCanCraftTM048 = 0xF29972B4; // FSYS_UI_WAZA_MACHINE_RELEASE_048
    private const uint KCanCraftTM049 = 0xF2997467; // FSYS_UI_WAZA_MACHINE_RELEASE_049
    private const uint KCanCraftTM050 = 0xF29BA525; // FSYS_UI_WAZA_MACHINE_RELEASE_050
    private const uint KCanCraftTM051 = 0xF29BA372; // FSYS_UI_WAZA_MACHINE_RELEASE_051
    private const uint KCanCraftTM052 = 0xF29BA1BF; // FSYS_UI_WAZA_MACHINE_RELEASE_052
    private const uint KCanCraftTM053 = 0xF29BA00C; // FSYS_UI_WAZA_MACHINE_RELEASE_053
    private const uint KCanCraftTM054 = 0xF29B9E59; // FSYS_UI_WAZA_MACHINE_RELEASE_054
    private const uint KCanCraftTM055 = 0xF29B9CA6; // FSYS_UI_WAZA_MACHINE_RELEASE_055
    private const uint KCanCraftTM056 = 0xF29B9AF3; // FSYS_UI_WAZA_MACHINE_RELEASE_056
    private const uint KCanCraftTM057 = 0xF29B9940; // FSYS_UI_WAZA_MACHINE_RELEASE_057
    private const uint KCanCraftTM058 = 0xF29BB2BD; // FSYS_UI_WAZA_MACHINE_RELEASE_058
    private const uint KCanCraftTM059 = 0xF29BB10A; // FSYS_UI_WAZA_MACHINE_RELEASE_059
    private const uint KCanCraftTM060 = 0xF29EBEAE; // FSYS_UI_WAZA_MACHINE_RELEASE_060
    private const uint KCanCraftTM061 = 0xF29EC061; // FSYS_UI_WAZA_MACHINE_RELEASE_061
    private const uint KCanCraftTM062 = 0xF29EBB48; // FSYS_UI_WAZA_MACHINE_RELEASE_062
    private const uint KCanCraftTM063 = 0xF29EBCFB; // FSYS_UI_WAZA_MACHINE_RELEASE_063
    private const uint KCanCraftTM064 = 0xF29EC57A; // FSYS_UI_WAZA_MACHINE_RELEASE_064
    private const uint KCanCraftTM065 = 0xF29EC72D; // FSYS_UI_WAZA_MACHINE_RELEASE_065
    private const uint KCanCraftTM066 = 0xF29EC214; // FSYS_UI_WAZA_MACHINE_RELEASE_066
    private const uint KCanCraftTM067 = 0xF29EC3C7; // FSYS_UI_WAZA_MACHINE_RELEASE_067
    private const uint KCanCraftTM068 = 0xF29EB116; // FSYS_UI_WAZA_MACHINE_RELEASE_068
    private const uint KCanCraftTM069 = 0xF29EB2C9; // FSYS_UI_WAZA_MACHINE_RELEASE_069
    private const uint KCanCraftTM070 = 0xF2A1D837; // FSYS_UI_WAZA_MACHINE_RELEASE_070
    private const uint KCanCraftTM071 = 0xF2A1D684; // FSYS_UI_WAZA_MACHINE_RELEASE_071
    private const uint KCanCraftTM072 = 0xF2A1DB9D; // FSYS_UI_WAZA_MACHINE_RELEASE_072
    private const uint KCanCraftTM073 = 0xF2A1D9EA; // FSYS_UI_WAZA_MACHINE_RELEASE_073
    private const uint KCanCraftTM074 = 0xF2A1D16B; // FSYS_UI_WAZA_MACHINE_RELEASE_074
    private const uint KCanCraftTM075 = 0xF2A1CFB8; // FSYS_UI_WAZA_MACHINE_RELEASE_075
    private const uint KCanCraftTM076 = 0xF2A1D4D1; // FSYS_UI_WAZA_MACHINE_RELEASE_076
    private const uint KCanCraftTM077 = 0xF2A1D31E; // FSYS_UI_WAZA_MACHINE_RELEASE_077
    private const uint KCanCraftTM078 = 0xF2A1CA9F; // FSYS_UI_WAZA_MACHINE_RELEASE_078
    private const uint KCanCraftTM079 = 0xF2A1C8EC; // FSYS_UI_WAZA_MACHINE_RELEASE_079
    private const uint KCanCraftTM080 = 0xF2765270; // FSYS_UI_WAZA_MACHINE_RELEASE_080
    private const uint KCanCraftTM081 = 0xF2765423; // FSYS_UI_WAZA_MACHINE_RELEASE_081
    private const uint KCanCraftTM082 = 0xF27655D6; // FSYS_UI_WAZA_MACHINE_RELEASE_082
    private const uint KCanCraftTM083 = 0xF2765789; // FSYS_UI_WAZA_MACHINE_RELEASE_083
    private const uint KCanCraftTM084 = 0xF276593C; // FSYS_UI_WAZA_MACHINE_RELEASE_084
    private const uint KCanCraftTM085 = 0xF2765AEF; // FSYS_UI_WAZA_MACHINE_RELEASE_085
    private const uint KCanCraftTM086 = 0xF2765CA2; // FSYS_UI_WAZA_MACHINE_RELEASE_086
    private const uint KCanCraftTM087 = 0xF2765E55; // FSYS_UI_WAZA_MACHINE_RELEASE_087
    private const uint KCanCraftTM088 = 0xF2766008; // FSYS_UI_WAZA_MACHINE_RELEASE_088
    private const uint KCanCraftTM089 = 0xF27661BB; // FSYS_UI_WAZA_MACHINE_RELEASE_089
    private const uint KCanCraftTM090 = 0xF2796BF9; // FSYS_UI_WAZA_MACHINE_RELEASE_090
    private const uint KCanCraftTM091 = 0xF2796A46; // FSYS_UI_WAZA_MACHINE_RELEASE_091
    private const uint KCanCraftTM092 = 0xF2796893; // FSYS_UI_WAZA_MACHINE_RELEASE_092
    private const uint KCanCraftTM093 = 0xF27966E0; // FSYS_UI_WAZA_MACHINE_RELEASE_093
    private const uint KCanCraftTM094 = 0xF27972C5; // FSYS_UI_WAZA_MACHINE_RELEASE_094
    private const uint KCanCraftTM095 = 0xF2797112; // FSYS_UI_WAZA_MACHINE_RELEASE_095
    private const uint KCanCraftTM096 = 0xF2796F5F; // FSYS_UI_WAZA_MACHINE_RELEASE_096
    private const uint KCanCraftTM097 = 0xF2796DAC; // FSYS_UI_WAZA_MACHINE_RELEASE_097
    private const uint KCanCraftTM098 = 0xF2797991; // FSYS_UI_WAZA_MACHINE_RELEASE_098
    private const uint KCanCraftTM099 = 0xF27977DE; // FSYS_UI_WAZA_MACHINE_RELEASE_099
    private const uint KCanCraftTM100 = 0xF7D23C43; // FSYS_UI_WAZA_MACHINE_RELEASE_100
    private const uint KCanCraftTM101 = 0xF7D23A90; // FSYS_UI_WAZA_MACHINE_RELEASE_101
    private const uint KCanCraftTM102 = 0xF7D23FA9; // FSYS_UI_WAZA_MACHINE_RELEASE_102
    private const uint KCanCraftTM103 = 0xF7D23DF6; // FSYS_UI_WAZA_MACHINE_RELEASE_103
    private const uint KCanCraftTM104 = 0xF7D2430F; // FSYS_UI_WAZA_MACHINE_RELEASE_104
    private const uint KCanCraftTM105 = 0xF7D2415C; // FSYS_UI_WAZA_MACHINE_RELEASE_105
    private const uint KCanCraftTM106 = 0xF7D24675; // FSYS_UI_WAZA_MACHINE_RELEASE_106
    private const uint KCanCraftTM107 = 0xF7D244C2; // FSYS_UI_WAZA_MACHINE_RELEASE_107
    private const uint KCanCraftTM108 = 0xF7D249DB; // FSYS_UI_WAZA_MACHINE_RELEASE_108
    private const uint KCanCraftTM109 = 0xF7D24828; // FSYS_UI_WAZA_MACHINE_RELEASE_109
    private const uint KCanCraftTM110 = 0xF7CF591A; // FSYS_UI_WAZA_MACHINE_RELEASE_110
    private const uint KCanCraftTM111 = 0xF7CF5ACD; // FSYS_UI_WAZA_MACHINE_RELEASE_111
    private const uint KCanCraftTM112 = 0xF7CF55B4; // FSYS_UI_WAZA_MACHINE_RELEASE_112
    private const uint KCanCraftTM113 = 0xF7CF5767; // FSYS_UI_WAZA_MACHINE_RELEASE_113
    private const uint KCanCraftTM114 = 0xF7CF524E; // FSYS_UI_WAZA_MACHINE_RELEASE_114
    private const uint KCanCraftTM115 = 0xF7CF5401; // FSYS_UI_WAZA_MACHINE_RELEASE_115
    private const uint KCanCraftTM116 = 0xF7CF4EE8; // FSYS_UI_WAZA_MACHINE_RELEASE_116
    private const uint KCanCraftTM117 = 0xF7CF509B; // FSYS_UI_WAZA_MACHINE_RELEASE_117
    private const uint KCanCraftTM118 = 0xF7CF4B82; // FSYS_UI_WAZA_MACHINE_RELEASE_118
    private const uint KCanCraftTM119 = 0xF7CF4D35; // FSYS_UI_WAZA_MACHINE_RELEASE_119
    private const uint KCanCraftTM120 = 0xF7CC3F91; // FSYS_UI_WAZA_MACHINE_RELEASE_120
    private const uint KCanCraftTM121 = 0xF7CC3DDE; // FSYS_UI_WAZA_MACHINE_RELEASE_121
    private const uint KCanCraftTM122 = 0xF7CC3C2B; // FSYS_UI_WAZA_MACHINE_RELEASE_122
    private const uint KCanCraftTM123 = 0xF7CC3A78; // FSYS_UI_WAZA_MACHINE_RELEASE_123
    private const uint KCanCraftTM124 = 0xF7CC465D; // FSYS_UI_WAZA_MACHINE_RELEASE_124
    private const uint KCanCraftTM125 = 0xF7CC44AA; // FSYS_UI_WAZA_MACHINE_RELEASE_125
    private const uint KCanCraftTM126 = 0xF7CC42F7; // FSYS_UI_WAZA_MACHINE_RELEASE_126
    private const uint KCanCraftTM127 = 0xF7CC4144; // FSYS_UI_WAZA_MACHINE_RELEASE_127
    private const uint KCanCraftTM128 = 0xF7CC31F9; // FSYS_UI_WAZA_MACHINE_RELEASE_128
    private const uint KCanCraftTM129 = 0xF7CC3046; // FSYS_UI_WAZA_MACHINE_RELEASE_129
    private const uint KCanCraftTM130 = 0xF7C92608; // FSYS_UI_WAZA_MACHINE_RELEASE_130
    private const uint KCanCraftTM131 = 0xF7C927BB; // FSYS_UI_WAZA_MACHINE_RELEASE_131
    private const uint KCanCraftTM132 = 0xF7C9296E; // FSYS_UI_WAZA_MACHINE_RELEASE_132
    private const uint KCanCraftTM133 = 0xF7C92B21; // FSYS_UI_WAZA_MACHINE_RELEASE_133
    private const uint KCanCraftTM134 = 0xF7C92CD4; // FSYS_UI_WAZA_MACHINE_RELEASE_134
    private const uint KCanCraftTM135 = 0xF7C92E87; // FSYS_UI_WAZA_MACHINE_RELEASE_135
    private const uint KCanCraftTM136 = 0xF7C9303A; // FSYS_UI_WAZA_MACHINE_RELEASE_136
    private const uint KCanCraftTM137 = 0xF7C931ED; // FSYS_UI_WAZA_MACHINE_RELEASE_137
    private const uint KCanCraftTM138 = 0xF7C91870; // FSYS_UI_WAZA_MACHINE_RELEASE_138
    private const uint KCanCraftTM139 = 0xF7C91A23; // FSYS_UI_WAZA_MACHINE_RELEASE_139
    private const uint KCanCraftTM140 = 0xF7DDC8E7; // FSYS_UI_WAZA_MACHINE_RELEASE_140
    private const uint KCanCraftTM141 = 0xF7DDC734; // FSYS_UI_WAZA_MACHINE_RELEASE_141
    private const uint KCanCraftTM142 = 0xF7DDCC4D; // FSYS_UI_WAZA_MACHINE_RELEASE_142
    private const uint KCanCraftTM143 = 0xF7DDCA9A; // FSYS_UI_WAZA_MACHINE_RELEASE_143
    private const uint KCanCraftTM144 = 0xF7DDC21B; // FSYS_UI_WAZA_MACHINE_RELEASE_144
    private const uint KCanCraftTM145 = 0xF7DDC068; // FSYS_UI_WAZA_MACHINE_RELEASE_145
    private const uint KCanCraftTM146 = 0xF7DDC581; // FSYS_UI_WAZA_MACHINE_RELEASE_146
    private const uint KCanCraftTM147 = 0xF7DDC3CE; // FSYS_UI_WAZA_MACHINE_RELEASE_147
    private const uint KCanCraftTM148 = 0xF7DDBB4F; // FSYS_UI_WAZA_MACHINE_RELEASE_148
    private const uint KCanCraftTM149 = 0xF7DDB99C; // FSYS_UI_WAZA_MACHINE_RELEASE_149
    private const uint KCanCraftTM150 = 0xF7DAAF5E; // FSYS_UI_WAZA_MACHINE_RELEASE_150
    private const uint KCanCraftTM151 = 0xF7DAB111; // FSYS_UI_WAZA_MACHINE_RELEASE_151
    private const uint KCanCraftTM152 = 0xF7DAABF8; // FSYS_UI_WAZA_MACHINE_RELEASE_152
    private const uint KCanCraftTM153 = 0xF7DAADAB; // FSYS_UI_WAZA_MACHINE_RELEASE_153
    private const uint KCanCraftTM154 = 0xF7DAB62A; // FSYS_UI_WAZA_MACHINE_RELEASE_154
    private const uint KCanCraftTM155 = 0xF7DAB7DD; // FSYS_UI_WAZA_MACHINE_RELEASE_155
    private const uint KCanCraftTM156 = 0xF7DAB2C4; // FSYS_UI_WAZA_MACHINE_RELEASE_156
    private const uint KCanCraftTM157 = 0xF7DAB477; // FSYS_UI_WAZA_MACHINE_RELEASE_157
    private const uint KCanCraftTM158 = 0xF7DAA1C6; // FSYS_UI_WAZA_MACHINE_RELEASE_158
    private const uint KCanCraftTM159 = 0xF7DAA379; // FSYS_UI_WAZA_MACHINE_RELEASE_159
    private const uint KCanCraftTM160 = 0xF7D795D5; // FSYS_UI_WAZA_MACHINE_RELEASE_160
    private const uint KCanCraftTM161 = 0xF7D79422; // FSYS_UI_WAZA_MACHINE_RELEASE_161
    private const uint KCanCraftTM162 = 0xF7D7926F; // FSYS_UI_WAZA_MACHINE_RELEASE_162
    private const uint KCanCraftTM163 = 0xF7D790BC; // FSYS_UI_WAZA_MACHINE_RELEASE_163
    private const uint KCanCraftTM164 = 0xF7D78F09; // FSYS_UI_WAZA_MACHINE_RELEASE_164
    private const uint KCanCraftTM165 = 0xF7D78D56; // FSYS_UI_WAZA_MACHINE_RELEASE_165
    private const uint KCanCraftTM166 = 0xF7D78BA3; // FSYS_UI_WAZA_MACHINE_RELEASE_166
    private const uint KCanCraftTM167 = 0xF7D789F0; // FSYS_UI_WAZA_MACHINE_RELEASE_167
    private const uint KCanCraftTM168 = 0xF7D7A36D; // FSYS_UI_WAZA_MACHINE_RELEASE_168
    private const uint KCanCraftTM169 = 0xF7D7A1BA; // FSYS_UI_WAZA_MACHINE_RELEASE_169
    private const uint KCanCraftTM170 = 0xF7D47C4C; // FSYS_UI_WAZA_MACHINE_RELEASE_170
    private const uint KCanCraftTM171 = 0xF7D47DFF; // FSYS_UI_WAZA_MACHINE_RELEASE_171
    private const uint KCanCraftTM172 = 0xF7D47FB2; // FSYS_UI_WAZA_MACHINE_RELEASE_172
    private const uint KCanCraftTM173 = 0xF7D48165; // FSYS_UI_WAZA_MACHINE_RELEASE_173
    private const uint KCanCraftTM174 = 0xF7D47580; // FSYS_UI_WAZA_MACHINE_RELEASE_174
    private const uint KCanCraftTM175 = 0xF7D47733; // FSYS_UI_WAZA_MACHINE_RELEASE_175
    private const uint KCanCraftTM176 = 0xF7D478E6; // FSYS_UI_WAZA_MACHINE_RELEASE_176
    private const uint KCanCraftTM177 = 0xF7D47A99; // FSYS_UI_WAZA_MACHINE_RELEASE_177
    private const uint KCanCraftTM178 = 0xF7D489E4; // FSYS_UI_WAZA_MACHINE_RELEASE_178
    private const uint KCanCraftTM179 = 0xF7D48B97; // FSYS_UI_WAZA_MACHINE_RELEASE_179
    private const uint KCanCraftTM180 = 0xF7BAB63B; // FSYS_UI_WAZA_MACHINE_RELEASE_180
    private const uint KCanCraftTM181 = 0xF7BAB488; // FSYS_UI_WAZA_MACHINE_RELEASE_181
    private const uint KCanCraftTM182 = 0xF7BAB9A1; // FSYS_UI_WAZA_MACHINE_RELEASE_182
    private const uint KCanCraftTM183 = 0xF7BAB7EE; // FSYS_UI_WAZA_MACHINE_RELEASE_183
    private const uint KCanCraftTM184 = 0xF7BABD07; // FSYS_UI_WAZA_MACHINE_RELEASE_184
    private const uint KCanCraftTM185 = 0xF7BABB54; // FSYS_UI_WAZA_MACHINE_RELEASE_185
    private const uint KCanCraftTM186 = 0xF7BAC06D; // FSYS_UI_WAZA_MACHINE_RELEASE_186
    private const uint KCanCraftTM187 = 0xF7BABEBA; // FSYS_UI_WAZA_MACHINE_RELEASE_187
    private const uint KCanCraftTM188 = 0xF7BAA8A3; // FSYS_UI_WAZA_MACHINE_RELEASE_188
    private const uint KCanCraftTM189 = 0xF7BAA6F0; // FSYS_UI_WAZA_MACHINE_RELEASE_189
    private const uint KCanCraftTM190 = 0xF7B79CB2; // FSYS_UI_WAZA_MACHINE_RELEASE_190
    private const uint KCanCraftTM191 = 0xF7B79E65; // FSYS_UI_WAZA_MACHINE_RELEASE_191
    private const uint KCanCraftTM192 = 0xF7B7994C; // FSYS_UI_WAZA_MACHINE_RELEASE_192
    private const uint KCanCraftTM193 = 0xF7B79AFF; // FSYS_UI_WAZA_MACHINE_RELEASE_193
    private const uint KCanCraftTM194 = 0xF7B795E6; // FSYS_UI_WAZA_MACHINE_RELEASE_194
    private const uint KCanCraftTM195 = 0xF7B79799; // FSYS_UI_WAZA_MACHINE_RELEASE_195
    private const uint KCanCraftTM196 = 0xF7B79280; // FSYS_UI_WAZA_MACHINE_RELEASE_196
    private const uint KCanCraftTM197 = 0xF7B79433; // FSYS_UI_WAZA_MACHINE_RELEASE_197
    private const uint KCanCraftTM198 = 0xF7B7AA4A; // FSYS_UI_WAZA_MACHINE_RELEASE_198
    private const uint KCanCraftTM199 = 0xF7B7ABFD; // FSYS_UI_WAZA_MACHINE_RELEASE_199
    private const uint KCanCraftTM200 = 0xFBA50B8E; // FSYS_UI_WAZA_MACHINE_RELEASE_200
    private const uint KCanCraftTM201 = 0xFBA50D41; // FSYS_UI_WAZA_MACHINE_RELEASE_201
    private const uint KCanCraftTM202 = 0xFBA50828; // FSYS_UI_WAZA_MACHINE_RELEASE_202
    private const uint KCanCraftTM203 = 0xFBA509DB; // FSYS_UI_WAZA_MACHINE_RELEASE_203
    private const uint KCanCraftTM204 = 0xFBA5125A; // FSYS_UI_WAZA_MACHINE_RELEASE_204
    private const uint KCanCraftTM205 = 0xFBA5140D; // FSYS_UI_WAZA_MACHINE_RELEASE_205
    private const uint KCanCraftTM206 = 0xFBA50EF4; // FSYS_UI_WAZA_MACHINE_RELEASE_206
    private const uint KCanCraftTM207 = 0xFBA510A7; // FSYS_UI_WAZA_MACHINE_RELEASE_207
    private const uint KCanCraftTM208 = 0xFBA4FDF6; // FSYS_UI_WAZA_MACHINE_RELEASE_208
    private const uint KCanCraftTM209 = 0xFBA4FFA9; // FSYS_UI_WAZA_MACHINE_RELEASE_209
    private const uint KCanCraftTM210 = 0xFBA74B97; // FSYS_UI_WAZA_MACHINE_RELEASE_210
    private const uint KCanCraftTM211 = 0xFBA749E4; // FSYS_UI_WAZA_MACHINE_RELEASE_211
    private const uint KCanCraftTM212 = 0xFBA74EFD; // FSYS_UI_WAZA_MACHINE_RELEASE_212
    private const uint KCanCraftTM213 = 0xFBA74D4A; // FSYS_UI_WAZA_MACHINE_RELEASE_213
    private const uint KCanCraftTM214 = 0xFBA744CB; // FSYS_UI_WAZA_MACHINE_RELEASE_214
    private const uint KCanCraftTM215 = 0xFBA74318; // FSYS_UI_WAZA_MACHINE_RELEASE_215
    private const uint KCanCraftTM216 = 0xFBA74831; // FSYS_UI_WAZA_MACHINE_RELEASE_216
    private const uint KCanCraftTM217 = 0xFBA7467E; // FSYS_UI_WAZA_MACHINE_RELEASE_217
    private const uint KCanCraftTM218 = 0xFBA73DFF; // FSYS_UI_WAZA_MACHINE_RELEASE_218
    private const uint KCanCraftTM219 = 0xFBA73C4C; // FSYS_UI_WAZA_MACHINE_RELEASE_219
    private const uint KCanCraftTM220 = 0xFB9ED87C; // FSYS_UI_WAZA_MACHINE_RELEASE_220
    private const uint KCanCraftTM221 = 0xFB9EDA2F; // FSYS_UI_WAZA_MACHINE_RELEASE_221
    private const uint KCanCraftTM222 = 0xFB9EDBE2; // FSYS_UI_WAZA_MACHINE_RELEASE_222
    private const uint KCanCraftTM223 = 0xFB9EDD95; // FSYS_UI_WAZA_MACHINE_RELEASE_223
    private const uint KCanCraftTM224 = 0xFB9ED1B0; // FSYS_UI_WAZA_MACHINE_RELEASE_224
    private const uint KCanCraftTM225 = 0xFB9ED363; // FSYS_UI_WAZA_MACHINE_RELEASE_225
    private const uint KCanCraftTM226 = 0xFB9ED516; // FSYS_UI_WAZA_MACHINE_RELEASE_226
    private const uint KCanCraftTM227 = 0xFB9ED6C9; // FSYS_UI_WAZA_MACHINE_RELEASE_227
    private const uint KCanCraftTM228 = 0xFB9EE614; // FSYS_UI_WAZA_MACHINE_RELEASE_228
    private const uint KCanCraftTM229 = 0xFB9EE7C7; // FSYS_UI_WAZA_MACHINE_RELEASE_229
    #endregion

    #region Other System Flags (Humanized, Hash, Internal Name)
    private const uint KUnlockedIVJudge = 0x7C6E7444; // FSYS_BOX_JUDGE
    private const uint KUnlockedUpgradeHighJump = 0x54B8E8E1; // FSYS_RIDE_HIJUMP_ENABLE
    private const uint KUnlockedUpgradeDash = 0x0D707154; // FSYS_RIDE_DASH_ENABLE
    private const uint KUnlockedUpgradeClimb = 0x6ABB01F7; // FSYS_RIDE_CLIMB_ENABLE
    private const uint KUnlockedUpgradeGlide = 0x9DDF953F; // FSYS_RIDE_GLIDE_ENABLE
    private const uint KUnlockedUpgradeSwim = 0x76FF4AF0; // FSYS_RIDE_SWIM_ENABLE
    private const uint KUnlockedRaidDifficulty3 = 0xEC95D8EF; // FSYS_RAID_DIFFICTLTY3_RELEASE
    private const uint KUnlockedRaidDifficulty4 = 0xA9428DFE; // FSYS_RAID_DIFFICTLTY4_RELEASE
    private const uint KUnlockedRaidDifficulty5 = 0x9535F471; // FSYS_RAID_DIFFICTLTY5_RELEASE
    private const uint KUnlockedRaidDifficulty6 = 0x6E7F8220; // FSYS_RAID_DIFFICTLTY6_RELEASE
    private const uint KUnlockedTeraRaidBattles = 0x27025EBF; // FSYS_RAID_SCENARIO_00
    private const uint KUnlockedLetsGo = 0xF4670B74; // FSYS_LETSGO_UNLOCK
    private const uint KUnlockedMap = 0xC7B5D24A; // FSYS_YMAP_ENABLE
    private const uint KUnlockedMassOutbreaks = 0x94D0D4DB; // FSYS_ENCOUNT_OUTBREAK
    private const uint KUnlockedTeraTypeChanging = 0x13DB4C8F; // FSYS_GEM_CHANGE_ENABLE
    private const uint KUnlockedYMapSubMenu = 0xE9F1D866; // FSYS_YMAP_TOPMENU_UNLOCK_01
    private const uint KSelectedBirthday = 0xB1AF24E4; // FSYS_IS_INPUT_BIRTHDAY
    private const uint KUnlockedEmotes = 0xCB13D905; // FSYS_EMOTE_UNLOCK_EVENT

    private const uint KGameClearPaldea = 0x92CE2CF6; // FSYS_SCENARIO_GAME_CLEAR
    private const uint KCanRideLegendary = 0xB9B1220D; // FSYS_RIDE_ENABLE
    private const uint KFixedSpawnsOnly = 0x8BA58864; // FSYS_ENCOUNT_FIXEDSPAWN_ONLY
    private const uint KWildSpawnsEnabled = 0xC812EDC7; // FSYS_ENCOUNT_ENABLE_SPAWN
    private const uint KIsBirthdayToday = 0xB223D309; // FSYS_BIRTHDAY_EVENT
    private const uint KCanClaimPokedexDiplomaPaldea = 0xF0D246CC; // FSYS_POKEDEX_SYOUJOU_ENABLE
    private const uint KClaimedPokedexDiplomaPaldea = 0xF7900D11; // FSYS_POKEDEX_SYOUJOU_EVENT
    private const uint KReceivedMasterRankRibbon = 0x44CA754B; // FSYS_GET_RIBBON_MASTERRANK
    private const uint KIsFlyDisabled = 0xC1555927; // FSYS_FLY_DISABLE

    // League Representatives
    private const uint KLeagueRepsAvailable = 0x2EF38578; // FSYS_MISTER_TRAINER_START
    private const uint KClearedLeagueRep01 = 0x7739D2B1; // FSYS_MISTER_TRAINER_AREA_01
    private const uint KClearedLeagueRep04 = 0x7739D7CA; // FSYS_MISTER_TRAINER_AREA_04
    private const uint KClearedLeagueRep05 = 0x7739D97D; // FSYS_MISTER_TRAINER_AREA_05
    private const uint KClearedLeagueRep06 = 0x7739D464; // FSYS_MISTER_TRAINER_AREA_06
    private const uint KClearedLeagueRep07 = 0x7739D617; // FSYS_MISTER_TRAINER_AREA_07
    private const uint KClearedLeagueRep08 = 0x7739C366; // FSYS_MISTER_TRAINER_AREA_08
    private const uint KClearedLeagueRep09 = 0x7739C519; // FSYS_MISTER_TRAINER_AREA_09
    private const uint KClearedLeagueRep10 = 0x773CEA87; // FSYS_MISTER_TRAINER_AREA_10
    private const uint KClearedLeagueRep11 = 0x773CE8D4; // FSYS_MISTER_TRAINER_AREA_11
    private const uint KClearedLeagueRep12 = 0x773CEDED; // FSYS_MISTER_TRAINER_AREA_12
    private const uint KClearedLeagueRep14 = 0x773CE3BB; // FSYS_MISTER_TRAINER_AREA_14
    private const uint KClearedLeagueRep15 = 0x773CE208; // FSYS_MISTER_TRAINER_AREA_15
    private const uint KClearedLeagueRep16 = 0x773CE721; // FSYS_MISTER_TRAINER_AREA_16
    private const uint KClearedLeagueRep17 = 0x773CE56E; // FSYS_MISTER_TRAINER_AREA_17
    private const uint KClearedLeagueRep18 = 0x773CDCEF; // FSYS_MISTER_TRAINER_AREA_18
    private const uint KClearedLeagueRep19 = 0x773CDB3C; // FSYS_MISTER_TRAINER_AREA_19
    private const uint KClearedLeagueRep20 = 0x7734776C; // FSYS_MISTER_TRAINER_AREA_20
    private const uint KClearedLeagueRep21 = 0x7734791F; // FSYS_MISTER_TRAINER_AREA_21
    private const uint KClearedLeagueRep22 = 0x77347AD2; // FSYS_MISTER_TRAINER_AREA_22

    private const uint FSYS_REPORT_DISABLE = 0xFD2B3BCA;
    private const uint FSYS_XMENU_POKELIST_RIDE = 0x091382AD;
    private const uint FSYS_XMENU_FULLOPEN = 0xFE482E4A;
    private const uint FSYS_CLIFF_RETURN_DISABLE = 0x77A4B321;
    private const uint FSYS_YMAP_TOPMENU_UNLOCK_00 = 0xE9F1DA19;
    private const uint FSYS_YMAP_TOPMENU_UNLOCK_EVENT_00 = 0x35B97ADC;
    private const uint FSYS_YMAP_TOPMENU_UNLOCK_EVENT_01 = 0x35B97C8F;
    private const uint FSYS_UI_BLUR_ENABLE = 0x68B12197;
    private const uint FSYS_RIDEPOKEBATTLE = 0xC31D56D6;
    private const uint FSYS_BATTLE_STUDIUM_BATTLE_TERMS = 0x788E0AC8;
    private const uint FSYS_YMAP_MASS_TEST_01 = 0x8EE54640;
    private const uint FSYS_YMAP_MASS_TEST_02 = 0x8EE54B59;
    private const uint FSYS_YMAP_MASS_TEST_03 = 0x8EE549A6;
    private const uint FSYS_YMAP_MASS_TEST_04 = 0x8EE54EBF;
    private const uint FSYS_YMAP_MASS_TEST_05 = 0x8EE54D0C;
    private const uint FSYS_YMAP_POK_02 = 0x30C57070;
    private const uint FSYS_YMAP_RAID_TEST_01 = 0xE93B3430;
    private const uint FSYS_YMAP_RAID_TEST_02 = 0xE93B3949;
    private const uint FSYS_YMAP_RAID_TEST_03 = 0xE93B3796;
    private const uint FSYS_YMAP_RAID_TEST_04 = 0xE93B3CAF;
    private const uint FSYS_YMAP_RAID_TEST_05 = 0xE93B3AFC;
    private const uint FSYS_RAID_DIFFICTLTY4_SURVEY = 0x2985A8E7;
    private const uint FSYS_RAID_DIFFICTLTY5_SURVEY = 0x48A247BA;
    private const uint FSYS_RAID_DIFFICTLTY6_SURVEY = 0xAD1DC231;
    private const uint FSYS_NETCONTENTS_OFF = 0x9AB093F2;

    // Pokémon GO Connectivity (Saturday)
    private const uint KGOAccountPaired = 0x3ABC21E3;
    private const uint KGOConnectionHistory = 0x7EE0A576; // Form ID, number of items/coins, extra item ID, Unix timestamp, GO username
    private const uint KGOVivillonForm = 0x22F70BCF; // BibiyonFormNoSave_formNo
    private const uint KGOVivillonFormEnabled = 0x0C125D5C; // BibiyonFormNoSave_isValid
    private const uint KGOVivillonPostcardSent = 0x867F0240; // BibiyonFormNoSave_accessedRealTime (Unix timestamp for when a postcard was sent from GO)
    #endregion

    #region FEVT
    // Daily events
    private const uint KChallengedTodayGiacomo = 0x2DAF633E; // FEVT_AJITO_AKU_BOSS_CHALLENGED_TODAY
    private const uint KChallengedTodayAtticus = 0xCE233DCE; // FEVT_AJITO_DOKU_BOSS_CHALLENGED_TODAY
    private const uint KChallengedTodayOrtega = 0x08EB6E34; // FEVT_AJITO_FAIRY_BOSS_CHALLENGED_TODAY
    private const uint KChallengedTodayMela = 0x57A4FAC2; // FEVT_AJITO_HONOO_BOSS_CHALLENGED_TODAY
    private const uint KChallengedTodayEri = 0xC0D96AD5; // FEVT_AJITO_KAKUTOU_BOSS_CHALLENGED_TODAY

    #region Treasures of Ruin
    private const uint KChallengedTodayTingLu = 0x7E1AB823; // FEVT_SUB_014_CHALLENGED_TODAY
    private const uint KChallengedTodayChienPao = 0xDC5B835A; // FEVT_SUB_015_CHALLENGED_TODAY
    private const uint KChallengedTodayWoChien = 0x9C620E19; // FEVT_SUB_016_CHALLENGED_TODAY
    private const uint KChallengedTodayChiYu = 0x33D68D98; // FEVT_SUB_017_CHALLENGED_TODAY

    // Groundblight Shrine
    private const uint KRemovedStakeTingLu1 = 0x12AC859B; // FEVT_SUB_014_KUI_01_RELEASE
    private const uint KRemovedStakeTingLu2 = 0x8DEDB4EE; // FEVT_SUB_014_KUI_02_RELEASE
    private const uint KRemovedStakeTingLu3 = 0x8AD96AE1; // FEVT_SUB_014_KUI_03_RELEASE
    private const uint KRemovedStakeTingLu4 = 0xDE563974; // FEVT_SUB_014_KUI_04_RELEASE
    private const uint KRemovedStakeTingLu5 = 0xCD9991DF; // FEVT_SUB_014_KUI_05_RELEASE
    private const uint KRemovedStakeTingLu6 = 0xAE72B792; // FEVT_SUB_014_KUI_06_RELEASE
    private const uint KRemovedStakeTingLu7 = 0xAB74A785; // FEVT_SUB_014_KUI_07_RELEASE
    private const uint KRemovedStakeTingLu8 = 0x32E24C38; // FEVT_SUB_014_KUI_08_RELEASE
    public const uint KStakesRemovedTingLu = 0xB836D2B5;

    // Icerend Shrine
    private const uint KRemovedStakeChienPao1 = 0xF1CFFA8E; // FEVT_SUB_015_KUI_01_RELEASE
    private const uint KRemovedStakeChienPao2 = 0x4C7DC4BB; // FEVT_SUB_015_KUI_02_RELEASE
    private const uint KRemovedStakeChienPao3 = 0xAE0F19B0; // FEVT_SUB_015_KUI_03_RELEASE
    private const uint KRemovedStakeChienPao4 = 0x95414EA5; // FEVT_SUB_015_KUI_04_RELEASE
    private const uint KRemovedStakeChienPao5 = 0x52F60D32; // FEVT_SUB_015_KUI_05_RELEASE
    private const uint KRemovedStakeChienPao6 = 0x5C55EE7F; // FEVT_SUB_015_KUI_06_RELEASE
    private const uint KRemovedStakeChienPao7 = 0xFFEB3B14; // FEVT_SUB_015_KUI_07_RELEASE
    private const uint KRemovedStakeChienPao8 = 0xDAD07F09; // FEVT_SUB_015_KUI_08_RELEASE
    public const uint KStakesRemovedChienPao = 0x25A71ADC;

    // Grasswither Shrine
    private const uint KRemovedStakeWoChien1 = 0xA3B68F79; // FEVT_SUB_016_KUI_01_RELEASE
    private const uint KRemovedStakeWoChien2 = 0x4CB308C8; // FEVT_SUB_016_KUI_02_RELEASE
    private const uint KRemovedStakeWoChien3 = 0xF6FC8073; // FEVT_SUB_016_KUI_03_RELEASE
    private const uint KRemovedStakeWoChien4 = 0x7E28078A; // FEVT_SUB_016_KUI_04_RELEASE
    private const uint KRemovedStakeWoChien5 = 0xCD304EDD; // FEVT_SUB_016_KUI_05_RELEASE
    private const uint KRemovedStakeWoChien6 = 0x27CB724C; // FEVT_SUB_016_KUI_06_RELEASE
    private const uint KRemovedStakeWoChien7 = 0x0470BDF7; // FEVT_SUB_016_KUI_07_RELEASE
    private const uint KRemovedStakeWoChien8 = 0x59A600BE; // FEVT_SUB_016_KUI_08_RELEASE
    public const uint KStakesRemovedWoChien = 0x9C7D6B7B;

    // Firescourge Shrine
    private const uint KRemovedStakeChiYu1 = 0x6F341C9C; // FEVT_SUB_017_KUI_01_RELEASE
    private const uint KRemovedStakeChiYu2 = 0x7077C7AD; // FEVT_SUB_017_KUI_02_RELEASE
    private const uint KRemovedStakeChiYu3 = 0xD9072D1A; // FEVT_SUB_017_KUI_03_RELEASE
    private const uint KRemovedStakeChiYu4 = 0x97E33903; // FEVT_SUB_017_KUI_04_RELEASE
    private const uint KRemovedStakeChiYu5 = 0xFC221F98; // FEVT_SUB_017_KUI_05_RELEASE
    private const uint KRemovedStakeChiYu6 = 0x6714A749; // FEVT_SUB_017_KUI_06_RELEASE
    private const uint KRemovedStakeChiYu7 = 0xC83DEFF6; // FEVT_SUB_017_KUI_07_RELEASE
    private const uint KRemovedStakeChiYu8 = 0xCA415ABF; // FEVT_SUB_017_KUI_08_RELEASE
    public const uint KStakesRemovedChiYu = 0xAE752C6A;
    #endregion

    private const uint KCapturedBoxLegendary = 0x987B8A24; // FEVT_SUB_018_CAPTURED
    private const uint KHasSaveDataLGPE = 0x2F9CC37D; // FEVT_SUB_028_HAVE_BELUGA_SAVE_DATA
    private const uint KHasSaveDataBDSP = 0xC4B1323E; // FEVT_SUB_028_HAVE_DELPHIS_SAVE_DATA
    private const uint KHasSaveDataLA = 0x592092B7; // FEVT_SUB_028_HAVE_HAYABUSA_SAVE_DATA
    private const uint KHasSaveDataSWSH = 0xE454155A; // FEVT_SUB_028_HAVE_ORION_SAVE_DATA
    private const uint KCompletedTradeSnom = 0xCEF6C179; // FEVT_SUB_044_TRADE_A
    private const uint KCompletedTradeWooper = 0xCEF6BC60; // FEVT_SUB_044_TRADE_B
    private const uint KCompletedTradeHaunter = 0xCEF6BE13; // FEVT_SUB_044_TRADE_C
    private const uint KClearedCourseBlizzard = 0xFD126E2B; // FEVT_SUB_GYM_KOORI_HARD_CLER
    private const uint KClearedCoursePowderSnow = 0x8BEB3731; // FEVT_SUB_GYM_KOORI_NORMAL_CLER
    private const uint KClearedCourseSheerCold = 0xC11CEE3A; // FEVT_SUB_GYM_KOORI_VERY_HARD_CLER
    private const uint KUnlockedRecipesMotherNormal = 0x0629031F; // FEVT_MOTHER_NORMAL_RECIPE_ADD_END
    private const uint KUnlockedRecipesMotherMaster = 0xA796AC7C; // FEVT_MOTHER_MASTER_RECIPE_ADD_END
    private const uint KUnlockedRecipesHerbaMystica1 = 0x6680756C; // FEVT_SUB_047_RACIPE_AMA_RELEASE
    private const uint KUnlockedRecipesHerbaMystica2 = 0x1DE39DD2; // FEVT_SUB_047_RACIPE_KARA_RELEASE
    private const uint KUnlockedRecipesHerbaMystica3 = 0xD4424476; // FEVT_SUB_047_RACIPE_NIGA_RELEASE
    private const uint KUnlockedRecipesHerbaMystica4 = 0x48598E18; // FEVT_SUB_047_RACIPE_SIO_RELEASE
    private const uint KUnlockedRecipesHerbaMystica5 = 0x483DB133; // FEVT_SUB_047_RACIPE_SPA_RELEASE

    private const uint FEVT_1ST_BATTLE_TOURNAMENT_LOST = 0x820A884D;
    private const uint FEVT_ATLANTIS_BASE_A1_EVENT_END = 0xED39BBAD;
    private const uint FEVT_ATLANTIS_BASE_A2_EVENT_END = 0x3C0C05F0;
    private const uint FEVT_ATLANTIS_BASE_A3_EVENT_END = 0x7F9FD74F;
    private const uint FEVT_CHAMP_HAVE_FAILED_INTERVIEW = 0x10E3E483;
    private const uint FEVT_CHAMP_PASS_1ST_INTERVIEW = 0x5F25E6E9;
    private const uint FEVT_COMMON_0065_FORCE_WALK = 0x8F0BC4E2;
    private const uint FEVT_COMMON_0140_NO_ENCOUNT = 0xF06488B7;
    private const uint FEVT_GYM_ESPER_TEST_BATTLE_WIN = 0x1C661813;
    private const uint FEVT_GYM_ESPER_TEST_TALKED = 0x883253B3;
    private const uint FEVT_GYM_KUSA_SUB_EVENT_AGREE = 0x851FB75A;
    private const uint FEVT_GYM_KUSA_TEST_AGREE = 0x92EE4167;
    private const uint FEVT_GYM_KUSA_TEST_BATTLE01_WIN = 0x6FEB5F41;
    private const uint FEVT_GYM_KUSA_TEST_BATTLE02_WIN = 0xE81273B4;
    private const uint FEVT_GYM_KUSA_TEST_BATTLE03_WIN = 0x1245C70B;
    private const uint FEVT_GYM_KUSA_TEST_BATTLE04_WIN = 0x358E0EBE;
    private const uint FEVT_GYM_KUSA_TEST_BATTLE05_WIN = 0x063CE0F5;
    private const uint FEVT_GYM_MUSHI_TEST_BATTLE01_WIN = 0x6C9FBBBB;
    private const uint FEVT_GYM_MUSHI_TEST_BATTLE02_WIN = 0x4FB3E2AA;
    private const uint FEVT_GYM_MUSHI_TEST_TALKED = 0xDF33CFF8;
    private const uint FEVT_NUSHI_DRAGON_EVENT_END = 0x3B815FEB;
    private const uint FEVT_NUSHI_DRAGON_FOOD_GET = 0x813881DC;
    private const uint FEVT_NUSHI_HAGANE_EVENT_END = 0x2F9F93C2;
    private const uint FEVT_NUSHI_HAGANE_FOOD_GET = 0x373D95BB;
    private const uint FEVT_NUSHI_HIKOU_EVENT_END = 0x375F553A;
    private const uint FEVT_NUSHI_HIKOU_FOOD_GET = 0x0403FC03;
    private const uint FEVT_NUSHI_IWA_EVENT_END = 0x571D5C39;
    private const uint FEVT_NUSHI_IWA_FOOD_GET = 0xDE305802;
    private const uint FEVT_NUSHI_JIMEN_EVENT_END = 0x6377C805;
    private const uint FEVT_NUSHI_JIMEN_FOOD_GET = 0xAD1BA90E;
    private const uint FEVT_SEAMLESS_TALK_BATTLE_LOSE = 0x924E346A;
    private const uint FEVT_SUB_020_END = 0x711B3174;
    private const uint FEVT_SUB_026_TALKED = 0x367A9A06;
    private const uint FEVT_SUB_043_TALKED_01 = 0xF5A9841D;
    private const uint FEVT_SUB_043_TALKED_02 = 0xF5A97F04;
    private const uint FEVT_SUB_043_TALKED_03 = 0xF5A980B7;
    private const uint FEVT_SUB_043_TALKED_04 = 0xF5A97B9E;
    private const uint FEVT_SUB_043_TALKED_05 = 0xF5A97D51;
    private const uint FEVT_SUB_043_TALKED_06 = 0xF5A97838;
    private const uint FEVT_SUB_043_TALKED_07 = 0xF5A979EB;
    private const uint FEVT_SUB_047_GAMECLEAR_TALKED = 0xF3711FD9;
    private const uint FEVT_SUB_047_TALKED = 0x8E7E08F5;
    private const uint FEVT_SUB_GYM_ESPER_ENDLESS_TRIED = 0xC00B5C3F;
    private const uint FEVT_SUB_GYM_ESPER_TALKED = 0x517DC48F;
    private const uint FEVT_SUB_GYM_MUSHI_REWARD_01 = 0x5EAAA944;
    private const uint FEVT_SUB_GYM_MUSHI_REWARD_02 = 0x5EAAAE5D;
    private const uint FEVT_SUB_GYM_MUSHI_REWARD_03 = 0x5EAAACAA;
    private const uint FEVT_SUB_GYM_MUSHI_REWARD_04 = 0x5EAAA42B;
    private const uint FEVT_TAKARA_CHANGE_TUTORIAL_CLEAR = 0x96124E87;
    #endregion

    #region Porto Marinada Auctions
    private const uint KAuctionRaffle = 0xDC31EB08; // WEVT_GYM_MIZU_SERI_RAFFLE
    private const uint KAuctionVenue1PurchasedItem = 0x50531CE6; // WEVT_GYM_MIZU_SERI_VENUE1_PROGRESS
    private const uint KAuctionVenue1Item = 0x3D837AB6; // WEVT_GYM_MIZU_SERI_VENUE1_ITEM_ID
    private const uint KAuctionVenue1Quantity = 0x93DB9439; // WEVT_GYM_MIZU_SERI_VENUE1_ITEM_NUM
    private const uint KAuctionVenue1NPC1 = 0x9A78757B; // WEVT_GYM_MIZU_SERI_VENUE1_NPC_ID1
    private const uint KAuctionVenue1NPC2 = 0x9A78772E; // WEVT_GYM_MIZU_SERI_VENUE1_NPC_ID2
    private const uint KAuctionVenue2PurchasedItem = 0x549187F5; // WEVT_GYM_MIZU_SERI_VENUE2_PROGRESS
    private const uint KAuctionVenue2Item = 0xC65EB057; // WEVT_GYM_MIZU_SERI_VENUE2_ITEM_ID
    private const uint KAuctionVenue2Quantity = 0x178AC492; // WEVT_GYM_MIZU_SERI_VENUE2_ITEM_NUM
    private const uint KAuctionVenue2NPC1 = 0x13AF53B6; // WEVT_GYM_MIZU_SERI_VENUE2_NPC_ID1
    private const uint KAuctionVenue2NPC2 = 0x13AF5203; // WEVT_GYM_MIZU_SERI_VENUE2_NPC_ID2
    private const uint KAuctionVenue3PurchasedItem = 0x702C68EC; // WEVT_GYM_MIZU_SERI_VENUE3_PROGRESS
    private const uint KAuctionVenue3Item = 0x26F510C4; // WEVT_GYM_MIZU_SERI_VENUE3_ITEM_ID
    private const uint KAuctionVenue3Quantity = 0x54D737FF; // WEVT_GYM_MIZU_SERI_VENUE3_ITEM_NUM
    private const uint KAuctionVenue3NPC1 = 0x96FD9641; // WEVT_GYM_MIZU_SERI_VENUE3_NPC_ID1
    private const uint KAuctionVenue3NPC2 = 0x96FD9128; // WEVT_GYM_MIZU_SERI_VENUE3_NPC_ID2
    private const uint KAuctionVenue4PurchasedItem = 0x6D418A8B; // WEVT_GYM_MIZU_SERI_VENUE4_PROGRESS
    private const uint KAuctionVenue4Item = 0x8EF22D95; // WEVT_GYM_MIZU_SERI_VENUE4_ITEM_ID
    private const uint KAuctionVenue4Quantity = 0xFDDFB678; // WEVT_GYM_MIZU_SERI_VENUE4_ITEM_NUM
    private const uint KAuctionVenue4NPC1 = 0x3275F474; // WEVT_GYM_MIZU_SERI_VENUE4_NPC_ID1
    private const uint KAuctionVenue4NPC2 = 0x3275F98D; // WEVT_GYM_MIZU_SERI_VENUE4_NPC_ID2

    private const uint FEVT_SERI_LEGEND_0112 = 0x8048F243; // unused Griseous Orb
    private const uint KPurchasedGriseousOrb = 0xF917055C; // FEVT_SERI_LEGEND_0112_v2
    private const uint KPurchasedAdamantOrb = 0x804E5454; // FEVT_SERI_LEGEND_0135
    private const uint KPurchasedLustrousOrb = 0x804E596D; // FEVT_SERI_LEGEND_0136
    private const uint KPurchasedFlamePlate = 0x8E874DC8; // FEVT_SERI_LEGEND_0298
    private const uint KPurchasedSplashPlate = 0x8E874F7B; // FEVT_SERI_LEGEND_0299
    private const uint KPurchasedZapPlate = 0x895CD8D6; // FEVT_SERI_LEGEND_0300
    private const uint KPurchasedMeadowPlate = 0x895CDA89; // FEVT_SERI_LEGEND_0301
    private const uint KPurchasedIciclePlate = 0x895CD570; // FEVT_SERI_LEGEND_0302
    private const uint KPurchasedFistPlate = 0x895CD723; // FEVT_SERI_LEGEND_0303
    private const uint KPurchasedToxicPlate = 0x895CDFA2; // FEVT_SERI_LEGEND_0304
    private const uint KPurchasedEarthPlate = 0x895CE155; // FEVT_SERI_LEGEND_0305
    private const uint KPurchasedSkyPlate = 0x895CDC3C; // FEVT_SERI_LEGEND_0306
    private const uint KPurchasedMindPlate = 0x895CDDEF; // FEVT_SERI_LEGEND_0307
    private const uint KPurchasedInsectPlate = 0x895CE66E; // FEVT_SERI_LEGEND_0308
    private const uint KPurchasedStonePlate = 0x895CE821; // FEVT_SERI_LEGEND_0309
    private const uint KPurchasedSpookyPlate = 0x895FF25F; // FEVT_SERI_LEGEND_0310
    private const uint KPurchasedDracoPlate = 0x895FF0AC; // FEVT_SERI_LEGEND_0311
    private const uint KPurchasedDreadPlate = 0x895FF5C5; // FEVT_SERI_LEGEND_0312
    private const uint KPurchasedIronPlate = 0x895FF412; // FEVT_SERI_LEGEND_0313
    private const uint KPurchasedGracidea = 0x97A78CE7; // FEVT_SERI_LEGEND_0466
    private const uint KPurchasedRevealGlass = 0xA1E7C2EE; // FEVT_SERI_LEGEND_0638
    private const uint KPurchasedPixiePlate = 0xA1D94C55; // FEVT_SERI_LEGEND_0644
    private const uint KPurchasedPrisonBottle = 0x9CA69C5B; // FEVT_SERI_LEGEND_0765
    private const uint KPurchasedRustedSword = 0x32CAE86C; // FEVT_SERI_LEGEND_1103
    private const uint KPurchasedRustedShield = 0x32CAE6B9; // FEVT_SERI_LEGEND_1104
    private const uint KPurchasedRotomCatalog = 0x246C9C43; // FEVT_SERI_LEGEND_1278
    private const uint KPurchasedReinsOfUnity = 0x1F3F28F8; // FEVT_SERI_LEGEND_1590
    private const uint KPurchasedAdamantCrystal = 0x16729999; // FEVT_SERI_LEGEND_1777
    private const uint KPurchasedLustrousGlobe = 0x1672A8E4; // FEVT_SERI_LEGEND_1778
    private const uint KPurchasedGriseousCore = 0xFE092048; // FEVT_SERI_LEGEND_1779_v2
    private const uint KPurchasedScrollOfDarkness = 0x06B12DD6; // FEVT_SERI_LEGEND_1857
    private const uint KPurchasedScrollOfWaters = 0x06B14087; // FEVT_SERI_LEGEND_1858
    #endregion

    #region Former Titans
    private const uint KBattledExTitanTatsugiri = 0xA3AD7A61; // NUSHI_DRAGON_AFTER
    private const uint KBattledExTitanOrthworm = 0xA9B66964; // NUSHI_HAGANE_AFTER
    private const uint KBattledExTitanBombirdier = 0xE5751F6B; // NUSHI_HIKO_AFTER
    private const uint KBattledExTitanKlawf = 0x8753B93F; // NUSHI_IWA_AFTER
    private const uint KBattledExTitanGreatTuskIronTreads = 0x9BDA0E57; // NUSHI_JIMEN_AFTER
    private const uint KCapturedExTitanTatsugiri = 0x2D7B5238; // NUSHI_DRAGON_AFTER_PERMANENT
    private const uint KCapturedExTitanOrthworm = 0x740065E9; // NUSHI_HAGANE_AFTER_PERMANENT
    private const uint KCapturedExTitanBombirdier = 0x7C6F7BD2; // NUSHI_HIKO_AFTER_PERMANENT
    private const uint KCapturedExTitanKlawf = 0x8CFF73AE; // NUSHI_IWA_AFTER_PERMANENT
    private const uint KCapturedExTitanGreatTuskIronTreads = 0xDCFC1846; // NUSHI_JIMEN_AFTER_PERMANENT
    private const uint SUSHI_DAMMY_00 = 0x1282B067;
    private const uint SUSHI_DAMMY_01 = 0x1282AEB4;
    private const uint SUSHI_DAMMY_02 = 0x1282B3CD;
    private const uint SUSHI_DAMMY_03 = 0x1282B21A;
    private const uint SUSHI_DAMMY_04 = 0x1282A99B;
    private const uint SUSHI_DAMMY_05 = 0x1282A7E8;
    private const uint SUSHI_DAMMY_06 = 0x1282AD01;
    private const uint SUSHI_DAMMY_07 = 0x1282AB4E;
    private const uint SUSHI_DAMMY_08 = 0x1282A2CF;
    private const uint SUSHI_DAMMY_09 = 0x1282A11C;
    private const uint SUSHI_DAMMY_10 = 0x127F96DE;
    private const uint SUSHI_DAMMY_11 = 0x127F9891;
    private const uint SUSHI_DAMMY_12 = 0x127F9378;
    private const uint SUSHI_DAMMY_13 = 0x127F952B;
    private const uint SUSHI_DAMMY_14 = 0x127F9DAA;
    private const uint SUSHI_DAMMY_15 = 0x127F9F5D;
    private const uint SUSHI_DAMMY_16 = 0x127F9A44;
    private const uint SUSHI_DAMMY_17 = 0x127F9BF7;
    private const uint SUSHI_DAMMY_18 = 0x127F8946;
    private const uint SUSHI_DAMMY_19 = 0x127F8AF9;
    #endregion

    #region WEVT
    // Badge Received Indexes (0-17)
    private const uint KIndexReceivedBadgeDark = 0x6C29ACC5; // WEVT_DAN_AKU_CLEAR
    private const uint KIndexReceivedBadgePoison = 0x71DB2CEB; // WEVT_DAN_DOKU_CLEAR
    private const uint KIndexReceivedBadgeFairy = 0xE1271327; // WEVT_DAN_FAIRY_CLEAR
    private const uint KIndexReceivedBadgeFire = 0x9C6FF7DD; // WEVT_DAN_HONOO_CLEAR
    private const uint KIndexReceivedBadgeFighting = 0x2A3AC89A; // WEVT_DAN_KAKUTOU_CLEAR
    private const uint KIndexReceivedBadgeElectric = 0x8205ECAD; // WEVT_GYM_DENKI_CLEAR
    private const uint KIndexReceivedBadgePsychic = 0x3B819021; // WEVT_GYM_ESPER_CLEAR
    private const uint KIndexReceivedBadgeGhost = 0xCDA61DED; // WEVT_GYM_GHOST_CLEAR
    private const uint KIndexReceivedBadgeIce = 0x46B6CB30; // WEVT_GYM_KOORI_CLEAR
    private const uint KIndexReceivedBadgeGrass = 0xB4C3AFE6; // WEVT_GYM_KUSA_CLEAR
    private const uint KIndexReceivedBadgeWater = 0xA803FAAD; // WEVT_GYM_MIZU_CLEAR
    private const uint KIndexReceivedBadgeBug = 0x89306FE6; // WEVT_GYM_MUSHI_CLEAR
    private const uint KIndexReceivedBadgeNormal = 0xF90EFD79; // WEVT_GYM_NORMAL_CLEAR
    private const uint KIndexReceivedBadgeDragon = 0xEC7361B7; // WEVT_NUSHI_DRAGON_CLEAR
    private const uint KIndexReceivedBadgeSteel = 0x0D0602DE; // WEVT_NUSHI_HAGANE_CLEAR
    private const uint KIndexReceivedBadgeFlying = 0x9C16DA94; // WEVT_NUSHI_HIKOU_CLEAR
    private const uint KIndexReceivedBadgeRock = 0xA6CDE603; // WEVT_NUSHI_IWA_CLEAR
    private const uint KIndexReceivedBadgeGround = 0xBDAC74B3; // WEVT_NUSHI_JIMEN_CLEAR

    // Gym Challenges
    private const uint KBestTimeOliveRoll = 0x120A3884; // WEVT_SUB_GYM_MUSHI_BEST_RECORD_TIME
    private const uint KESPExerciseKnockoutHighScore = 0x287ED11F; // WEVT_SUB_GYM_ESPER_ENDLESS_HIGH_SCORE
    private const uint KESPExerciseModesUnlocked = 0x4874E130; // WEVT_SUB_GYM_ESPER_EXERCISE_LEVEL
    private const uint KSnowSlopeRunBestTime1 = 0x594C72A5; // WEVT_GYM_KOORI_BEST_RECORD_TIME_01
    private const uint KSnowSlopeRunBestTime2 = 0x594C6D8C; // WEVT_GYM_KOORI_BEST_RECORD_TIME_02
    private const uint KSnowSlopeRunBestTime3 = 0x594C6F3F; // WEVT_GYM_KOORI_BEST_RECORD_TIME_03

    private const uint WEVT_6TH_GYM_BATTLE_FIRST_POKEMON = 0xBF269C72;
    private const uint WEVT_AJITO_AKU_AFTER_EVENT_LP_COUNT = 0x2639E91E;
    private const uint WEVT_AJITO_DOKU_AFTER_EVENT_LP_COUNT = 0xC20DC520;
    private const uint WEVT_AJITO_FAIRY_AFTER_EVENT_LP_COUNT = 0x65B48D54;
    private const uint WEVT_AJITO_HONOO_AFTER_EVENT_LP_COUNT = 0x6377D0E6;
    private const uint WEVT_AJITO_KAKUTOU_AFTER_EVENT_LP_COUNT = 0x1E55392F;
    private const uint WEVT_COMMON_0080_FIRST_POKEMON = 0x908A0119;
    private const uint WEVT_COMMON_0240_REPLY_TO_RIVAL = 0x7367C02E;
    private const uint WEVT_COMMON_2050_1ST_REPLY = 0x492E5D15;
    private const uint WEVT_COMMON_2050_2ND_REPLY = 0x9A721F29;
    private const uint WEVT_COMMON_2050_3RD_REPLY = 0x0C089D22;
    private const uint WEVT_COMMON_2050_4TH_REPLY = 0x2E790F65;
    private const uint WEVT_DAN_HONOO_BATTLE_FIRST_POKEMON = 0x9F8DB33D;
    private const uint WEVT_GYM_DENKI_ACTION_END = 0xA8BAB54C;
    private const uint WEVT_GYM_DENKI_EVOLUTION_SELECT_ITEM_ID = 0xCE509B58;
    private const uint WEVT_GYM_DENKI_LOST_PROPERTY_PROGRESS = 0xF4AFE9C8;
    private const uint WEVT_GYM_DENKI_TEST_NPC_01 = 0x044F4DCE;
    private const uint WEVT_GYM_DENKI_TEST_NPC_02 = 0x044F4C1B;
    private const uint WEVT_GYM_GHOST_TALKED_TRAINER = 0x706D7FB5;
    private const uint WEVT_GYM_KOORI_TALKED = 0x664A20D4;
    private const uint WEVT_SUB_GYM_KOORI_COURSE_LEVEL = 0xDF3E258C;
    private const uint WEVT_SUB_GYM_KOORI_TALKED = 0xB04D088F;

    #region Artazon Gym Challenge Sunflora (Four blocks each: State, X coordinate, Y coordinate, Z coordinate)
    private const uint WEVT_GYM_KUSA_FIND_POKE_001 = 0xFF1D78E2;
    private const uint WEVT_GYM_KUSA_FIND_POKE_001_X = 0xEF4282CD;
    private const uint WEVT_GYM_KUSA_FIND_POKE_001_Y = 0xEF42811A;
    private const uint WEVT_GYM_KUSA_FIND_POKE_001_Z = 0xEF427F67;
    private const uint WEVT_GYM_KUSA_FIND_POKE_002 = 0xFF1D772F;
    private const uint WEVT_GYM_KUSA_FIND_POKE_002_X = 0xEB7FF698;
    private const uint WEVT_GYM_KUSA_FIND_POKE_002_Y = 0xEB7FF84B;
    private const uint WEVT_GYM_KUSA_FIND_POKE_002_Z = 0xEB7FF9FE;
    private const uint WEVT_GYM_KUSA_FIND_POKE_003 = 0xFF1D757C;
    private const uint WEVT_GYM_KUSA_FIND_POKE_003_X = 0xE4DB3613;
    private const uint WEVT_GYM_KUSA_FIND_POKE_003_Y = 0xE4DB3460;
    private const uint WEVT_GYM_KUSA_FIND_POKE_003_Z = 0xE4DB3979;
    private const uint WEVT_GYM_KUSA_FIND_POKE_004 = 0xFF1D73C9;
    private const uint WEVT_GYM_KUSA_FIND_POKE_004_X = 0xE061410E;
    private const uint WEVT_GYM_KUSA_FIND_POKE_004_Y = 0xE06142C1;
    private const uint WEVT_GYM_KUSA_FIND_POKE_004_Z = 0xE0613DA8;
    private const uint WEVT_GYM_KUSA_FIND_POKE_005 = 0xFF1D7216;
    private const uint WEVT_GYM_KUSA_FIND_POKE_005_X = 0xDCA08309;
    private const uint WEVT_GYM_KUSA_FIND_POKE_005_Y = 0xDCA08156;
    private const uint WEVT_GYM_KUSA_FIND_POKE_005_Z = 0xDCA07FA3;
    private const uint WEVT_GYM_KUSA_FIND_POKE_006 = 0xFF1D7063;
    private const uint WEVT_GYM_KUSA_FIND_POKE_006_X = 0xD6B3B344;
    private const uint WEVT_GYM_KUSA_FIND_POKE_006_Y = 0xD6B3B4F7;
    private const uint WEVT_GYM_KUSA_FIND_POKE_006_Z = 0xD6B3B6AA;
    private const uint WEVT_GYM_KUSA_FIND_POKE_007 = 0xFF1D6EB0;
    private const uint WEVT_GYM_KUSA_FIND_POKE_007_X = 0xD2F2F53F;
    private const uint WEVT_GYM_KUSA_FIND_POKE_007_Y = 0xD2F2F38C;
    private const uint WEVT_GYM_KUSA_FIND_POKE_007_Z = 0xD2F2F8A5;
    private const uint WEVT_GYM_KUSA_FIND_POKE_008 = 0xFF1D882D;
    private const uint WEVT_GYM_KUSA_FIND_POKE_008_X = 0x1C9C4F6A;
    private const uint WEVT_GYM_KUSA_FIND_POKE_008_Y = 0x1C9C511D;
    private const uint WEVT_GYM_KUSA_FIND_POKE_008_Z = 0x1C9C4C04;
    private const uint WEVT_GYM_KUSA_FIND_POKE_009 = 0xFF1D867A;
    private const uint WEVT_GYM_KUSA_FIND_POKE_009_X = 0x15F6B565;
    private const uint WEVT_GYM_KUSA_FIND_POKE_009_Y = 0x15F6B3B2;
    private const uint WEVT_GYM_KUSA_FIND_POKE_009_Z = 0x15F6B1FF;
    private const uint WEVT_GYM_KUSA_FIND_POKE_010 = 0xFF1A610C;
    private const uint WEVT_GYM_KUSA_FIND_POKE_010_X = 0x022CA9A3;
    private const uint WEVT_GYM_KUSA_FIND_POKE_010_Y = 0x022CA7F0;
    private const uint WEVT_GYM_KUSA_FIND_POKE_010_Z = 0x022CAD09;
    private const uint WEVT_GYM_KUSA_FIND_POKE_011 = 0xFF1A62BF;
    private const uint WEVT_GYM_KUSA_FIND_POKE_011_X = 0x06A70B68;
    private const uint WEVT_GYM_KUSA_FIND_POKE_011_Y = 0x06A70D1B;
    private const uint WEVT_GYM_KUSA_FIND_POKE_011_Z = 0x06A70ECE;
    private const uint WEVT_GYM_KUSA_FIND_POKE_012 = 0xFF1A6472;
    private const uint WEVT_GYM_KUSA_FIND_POKE_012_X = 0x0A6777DD;
    private const uint WEVT_GYM_KUSA_FIND_POKE_012_Y = 0x0A67762A;
    private const uint WEVT_GYM_KUSA_FIND_POKE_012_Z = 0x0A677477;
    private const uint WEVT_GYM_KUSA_FIND_POKE_013 = 0xFF1A6625;
    private const uint WEVT_GYM_KUSA_FIND_POKE_013_X = 0x110C3862;
    private const uint WEVT_GYM_KUSA_FIND_POKE_013_Y = 0x110C3A15;
    private const uint WEVT_GYM_KUSA_FIND_POKE_013_Z = 0x110C34FC;
    private const uint WEVT_GYM_KUSA_FIND_POKE_014 = 0xFF1A5A40;
    private const uint WEVT_GYM_KUSA_FIND_POKE_014_X = 0xED60664F;
    private const uint WEVT_GYM_KUSA_FIND_POKE_014_Y = 0xED60649C;
    private const uint WEVT_GYM_KUSA_FIND_POKE_014_Z = 0xED6069B5;
    private const uint WEVT_GYM_KUSA_FIND_POKE_015 = 0xFF1A5BF3;
    private const uint WEVT_GYM_KUSA_FIND_POKE_015_X = 0xF1D9EE94;
    private const uint WEVT_GYM_KUSA_FIND_POKE_015_Y = 0xF1D9F047;
    private const uint WEVT_GYM_KUSA_FIND_POKE_015_Z = 0xF1D9F1FA;
    private const uint WEVT_GYM_KUSA_FIND_POKE_016 = 0xFF1A5DA6;
    private const uint WEVT_GYM_KUSA_FIND_POKE_016_X = 0xF87F1BD9;
    private const uint WEVT_GYM_KUSA_FIND_POKE_016_Y = 0xF87F1A26;
    private const uint WEVT_GYM_KUSA_FIND_POKE_016_Z = 0xF87F1873;
    private const uint WEVT_GYM_KUSA_FIND_POKE_017 = 0xFF1A5F59;
    private const uint WEVT_GYM_KUSA_FIND_POKE_017_X = 0xFB87E91E;
    private const uint WEVT_GYM_KUSA_FIND_POKE_017_Y = 0xFB87EAD1;
    private const uint WEVT_GYM_KUSA_FIND_POKE_017_Z = 0xFB87E5B8;
    private const uint WEVT_GYM_KUSA_FIND_POKE_018 = 0xFF1A6EA4;
    private const uint WEVT_GYM_KUSA_FIND_POKE_018_X = 0x299AECBB;
    private const uint WEVT_GYM_KUSA_FIND_POKE_018_Y = 0x299AEB08;
    private const uint WEVT_GYM_KUSA_FIND_POKE_018_Z = 0x299AF021;
    private const uint WEVT_GYM_KUSA_FIND_POKE_019 = 0xFF1A7057;
    private const uint WEVT_GYM_KUSA_FIND_POKE_019_X = 0x2CA34D40;
    private const uint WEVT_GYM_KUSA_FIND_POKE_019_Y = 0x2CA34EF3;
    private const uint WEVT_GYM_KUSA_FIND_POKE_019_Z = 0x2CA350A6;
    private const uint WEVT_GYM_KUSA_FIND_POKE_020 = 0xFF23ADA7;
    private const uint WEVT_GYM_KUSA_FIND_POKE_020_X = 0xDBE96590;
    private const uint WEVT_GYM_KUSA_FIND_POKE_020_Y = 0xDBE96743;
    private const uint WEVT_GYM_KUSA_FIND_POKE_020_Z = 0xDBE968F6;
    private const uint WEVT_GYM_KUSA_FIND_POKE_021 = 0xFF23ABF4;
    private const uint WEVT_GYM_KUSA_FIND_POKE_021_X = 0xD5457E8B;
    private const uint WEVT_GYM_KUSA_FIND_POKE_021_Y = 0xD5457CD8;
    private const uint WEVT_GYM_KUSA_FIND_POKE_021_Z = 0xD54581F1;
    private const uint WEVT_GYM_KUSA_FIND_POKE_022 = 0xFF23B10D;
    private const uint WEVT_GYM_KUSA_FIND_POKE_022_X = 0xE596A1CA;
    private const uint WEVT_GYM_KUSA_FIND_POKE_022_Y = 0xE596A37D;
    private const uint WEVT_GYM_KUSA_FIND_POKE_022_Z = 0xE5969E64;
    private const uint WEVT_GYM_KUSA_FIND_POKE_023 = 0xFF23AF5A;
    private const uint WEVT_GYM_KUSA_FIND_POKE_023_X = 0xDEF1E145;
    private const uint WEVT_GYM_KUSA_FIND_POKE_023_Y = 0xDEF1DF92;
    private const uint WEVT_GYM_KUSA_FIND_POKE_023_Z = 0xDEF1DDDF;
    private const uint WEVT_GYM_KUSA_FIND_POKE_024 = 0xFF23A6DB;
    private const uint WEVT_GYM_KUSA_FIND_POKE_024_X = 0xC66457FC;
    private const uint WEVT_GYM_KUSA_FIND_POKE_024_Y = 0xC66459AF;
    private const uint WEVT_GYM_KUSA_FIND_POKE_024_Z = 0xC6645B62;
    private const uint WEVT_GYM_KUSA_FIND_POKE_025 = 0xFF23A528;
    private const uint WEVT_GYM_KUSA_FIND_POKE_025_X = 0xC35C6437;
    private const uint WEVT_GYM_KUSA_FIND_POKE_025_Y = 0xC35C6284;
    private const uint WEVT_GYM_KUSA_FIND_POKE_025_Z = 0xC35C679D;
    private const uint WEVT_GYM_KUSA_FIND_POKE_026 = 0xFF23AA41;
    private const uint WEVT_GYM_KUSA_FIND_POKE_026_X = 0xD0CA4346;
    private const uint WEVT_GYM_KUSA_FIND_POKE_026_Y = 0xD0CA44F9;
    private const uint WEVT_GYM_KUSA_FIND_POKE_026_Z = 0xD0CA3FE0;
    private const uint WEVT_GYM_KUSA_FIND_POKE_027 = 0xFF23A88E;
    private const uint WEVT_GYM_KUSA_FIND_POKE_027_X = 0xCD09F201;
    private const uint WEVT_GYM_KUSA_FIND_POKE_027_Y = 0xCD09F04E;
    private const uint WEVT_GYM_KUSA_FIND_POKE_027_Z = 0xCD09EE9B;
    private const uint WEVT_GYM_KUSA_FIND_POKE_028 = 0xFF23A00F;
    private const uint WEVT_GYM_KUSA_FIND_POKE_028_X = 0xB47B2278;
    private const uint WEVT_GYM_KUSA_FIND_POKE_028_Y = 0xB47B242B;
    private const uint WEVT_GYM_KUSA_FIND_POKE_028_Z = 0xB47B25DE;
    private const uint WEVT_GYM_KUSA_FIND_POKE_029 = 0xFF239E5C;
    private const uint WEVT_GYM_KUSA_FIND_POKE_029_X = 0xADD73B73;
    private const uint WEVT_GYM_KUSA_FIND_POKE_029_Y = 0xADD739C0;
    private const uint WEVT_GYM_KUSA_FIND_POKE_029_Z = 0xADD73ED9;
    private const uint WEVT_GYM_KUSA_FIND_POKE_030 = 0xFF20941E;
    private const uint WEVT_GYM_KUSA_FIND_POKE_030_X = 0xE82EE711;
    private const uint WEVT_GYM_KUSA_FIND_POKE_030_Y = 0xE82EE55E;
    private const uint WEVT_GYM_KUSA_FIND_POKE_030_Z = 0xE82EE3AB;
    #endregion

    private const uint WEVT_GYM_KUSA_SUB_EVENT_REWARD_COUNT = 0x71FB8B10;
    private const uint WEVT_GYM_KUSA_TALKED = 0x8E3B5E0A;
    private const uint WEVT_GYM_MUSHI_COURSE_TYPE = 0x3D5FE4C1;
    private const uint WEVT_RIDE_HAPPINESS = 0xD9DF9254;
    private const uint WEVT_SUB_047_RACIPE_BADGE_RELEASE = 0x8A3575A5;

    // 0 = sealed, 1 = seal lifted, 2 = captured
    private const uint KShrineStateTingLu = 0xA3B2E1E8; // WEVT_SUB_014_EVENT_STATE_UTHUWA
    private const uint KShrineStateChienPao = 0xB6D28884; // WEVT_SUB_015_EVENT_STATE_TSURUGI
    private const uint KShrineStateWoChien = 0x8FC1AFF5; // WEVT_SUB_016_EVENT_STATE_MOKKAN
    private const uint KShrineStateChiYu = 0x0FD2F9E2; // WEVT_SUB_017_EVENT_STATE_MAGATAMA
    #endregion

    #region WSYS
    public const uint KGameLanguage = 0x25839373; // WSYS_LANGUAGE_SELECT
    private const uint KBagSortingOption = 0xEEAEB167; // WSYS_BAG_SORT
    private const uint KGimmighoulCoinOverflow = 0x0265ECEB; // WSYS_COIN_OVERFLOW_NUM
    private const uint KSkinTone = 0xEDBB0C80; // WSYS_PLAYER_SKIN_SELECT
    private const uint KLastPokedexVolumeRewardThresholdPaldea = 0x359436C5; // WSYS_POKEDX_REWARD_CHIHOUA_VALUE
    private const uint KRaidsWonDifficulty4 = 0x6A0F66E2; // WSYS_RAID_DIFFICTLTY4_WIN_COUNT
    private const uint KRaidsWonDifficulty6 = 0xF97AC8A4; // WSYS_RAID_DIFFICTLTY6_WIN_COUNT

    private const uint WSYS_AIBOUNOAKASI_WALK_COUNT = 0x9C677EC6;
    private const uint WSYS_BIRTHDAY_EVENT_TIME = 0x77B3D1BE;
    private const uint WSYS_EGG_WALK_COUNT = 0xC1946E9B;
    private const uint WSYS_FRIEND_SHIP_WALK_COUNT = 0xC7CC9EBD;
    private const uint WSYS_GEM_CHANGE_POINT = 0xE1B947F5;
    private const uint WSYS_GEM_CHANGE_POINT_MAX = 0x6C1169C0;
    private const uint WSYS_GOZONJI_HINT_COUNT = 0x42579CF4;
    private const uint WSYS_LEAGUE_CARD_SELECT_CARD = 0xB5284E7C;
    private const uint WSYS_LEAGUE_CARD_SELECT_ICON = 0x45AEB78B;
    private const uint WSYS_NET_BATTLE_BGM_ID = 0x603562B5;
    private const uint WSYS_PARTNER_WALK_COUNT = 0x0CB2BF62;
    private const uint WSYS_PARTNER_WALK_COUNT_TEMP = 0x2523D6E1;
    private const uint WSYS_RAID_PENALTY_COUNT = 0xD9D557BE;
    private const uint WSYS_SCHOOL_MAP_DESTINATION_FLAG_VALUE = 0x612E2777;
    private const uint WSYS_SCHOOL_MAP_NEW_FLAG_VALUE = 0xEF63CAB5;
    private const uint WSYS_XMENU_RELEASE = 0x5452A1B1;
    private const uint WSYS_YMAP_CLEAR_EFFECT_DONE = 0x2F532744;
    #endregion

    #region EncountOutbreakSave (Paldea)
    private const uint KOutbreakMainNumActive        = 0x6C375C8A; // EncountOutbreakSave_enablecount

    private const uint KOutbreak01MainCenterPos      = 0x2ED42F4D; // EncountOutbreakSave_centerPos[0]
    private const uint KOutbreak01MainDummyPos       = 0x4A13BE7C; // EncountOutbreakSave_dummyPos[0]
    private const uint KOutbreak01MainSpecies        = 0x76A2F996; // EncountOutbreakSave_monsno[0]
    private const uint KOutbreak01MainForm           = 0x29B4615D; // EncountOutbreakSave_formno[0]
    private const uint KOutbreak01MainFound          = 0x7E203623; // EncountOutbreakSave_isFind[0]
    private const uint KOutbreak01MainNumKOed        = 0x4B16FBC2; // EncountOutbreakSave_subjugationCount[0]
    private const uint KOutbreak01MainTotalSpawns    = 0xB7DC495A; // EncountOutbreakSave_subjugationLimit[0]
    private const uint KOutbreak01MainMaterials      = 0x598C9F67; // EncountOutbreakSave_dropMaterialCount[0]
    private const uint KOutbreak01MainDeliveryID     = 0xF7598F85; // EncountOutbreakSave_deliveryId[0]
    private const uint KOutbreak01MainDeliveryZoneID = 0xE32CFC01; // EncountOutbreakSave_deliveryZoneIdx[0]
    private const uint KOutbreak01MainDeliveryPokeID = 0xF13432AE; // EncountOutbreakSave_deliveryPokeIdx[0]

    private const uint KOutbreak02MainCenterPos      = 0x2ED5F198; // EncountOutbreakSave_centerPos[1]
    private const uint KOutbreak02MainDummyPos       = 0x4A118F71; // EncountOutbreakSave_dummyPos[1]
    private const uint KOutbreak02MainSpecies        = 0x76A0BCF3; // EncountOutbreakSave_monsno[1]
    private const uint KOutbreak02MainForm           = 0x29B84368; // EncountOutbreakSave_formno[1]
    private const uint KOutbreak02MainFound          = 0x7E22DF86; // EncountOutbreakSave_isFind[1]
    private const uint KOutbreak02MainNumKOed        = 0x4B14BF1F; // EncountOutbreakSave_subjugationCount[1]
    private const uint KOutbreak02MainTotalSpawns    = 0xB7DA0CB7; // EncountOutbreakSave_subjugationLimit[1]
    private const uint KOutbreak02MainMaterials      = 0x598E6F4A; // EncountOutbreakSave_dropMaterialCount[1]
    private const uint KOutbreak02MainDeliveryID     = 0xF75BBE90; // EncountOutbreakSave_deliveryId[1]
    private const uint KOutbreak02MainDeliveryZoneID = 0xE32EBE4C; // EncountOutbreakSave_deliveryZoneIdx[1]
    private const uint KOutbreak02MainDeliveryPokeID = 0xF131F60B; // EncountOutbreakSave_deliveryPokeIdx[1]

    private const uint KOutbreak03MainCenterPos      = 0x2ECE09D3; // EncountOutbreakSave_centerPos[2]
    private const uint KOutbreak03MainDummyPos       = 0x4A0E135A; // EncountOutbreakSave_dummyPos[2]
    private const uint KOutbreak03MainSpecies        = 0x76A97E38; // EncountOutbreakSave_monsno[2]
    private const uint KOutbreak03MainForm           = 0x29AF8223; // EncountOutbreakSave_formno[2]
    private const uint KOutbreak03MainFound          = 0x7E25155D; // EncountOutbreakSave_isFind[2]
    private const uint KOutbreak03MainNumKOed        = 0x4B1CA6E4; // EncountOutbreakSave_subjugationCount[2]
    private const uint KOutbreak03MainTotalSpawns    = 0xB7E1F47C; // EncountOutbreakSave_subjugationLimit[2]
    private const uint KOutbreak03MainMaterials      = 0x59925821; // EncountOutbreakSave_dropMaterialCount[2]
    private const uint KOutbreak03MainDeliveryID     = 0xF752FD4B; // EncountOutbreakSave_deliveryId[2]
    private const uint KOutbreak03MainDeliveryZoneID = 0xE32669C7; // EncountOutbreakSave_deliveryZoneIdx[2]
    private const uint KOutbreak03MainDeliveryPokeID = 0xF13AB750; // EncountOutbreakSave_deliveryPokeIdx[2]

    private const uint KOutbreak04MainCenterPos      = 0x2ED04676; // EncountOutbreakSave_centerPos[3]
    private const uint KOutbreak04MainDummyPos       = 0x4A0BD6B7; // EncountOutbreakSave_dummyPos[3]
    private const uint KOutbreak04MainSpecies        = 0x76A6E26D; // EncountOutbreakSave_monsno[3]
    private const uint KOutbreak04MainForm           = 0x29B22B86; // EncountOutbreakSave_formno[3]
    private const uint KOutbreak04MainFound          = 0x7E28F768; // EncountOutbreakSave_isFind[3]
    private const uint KOutbreak04MainNumKOed        = 0x4B1A77D9; // EncountOutbreakSave_subjugationCount[3]
    private const uint KOutbreak04MainTotalSpawns    = 0xB7DFC571; // EncountOutbreakSave_subjugationLimit[3]
    private const uint KOutbreak04MainMaterials      = 0x5994F3EC; // EncountOutbreakSave_dropMaterialCount[3]
    private const uint KOutbreak04MainDeliveryID     = 0xF756ECEE; // EncountOutbreakSave_deliveryId[3]
    private const uint KOutbreak04MainDeliveryZoneID = 0xE329132A; // EncountOutbreakSave_deliveryZoneIdx[3]
    private const uint KOutbreak04MainDeliveryPokeID = 0xF136D545; // EncountOutbreakSave_deliveryPokeIdx[3]

    private const uint KOutbreak05MainCenterPos      = 0x2EC78531; // EncountOutbreakSave_centerPos[4]
    private const uint KOutbreak05MainDummyPos       = 0x4A1FFBD8; // EncountOutbreakSave_dummyPos[4]
    private const uint KOutbreak05MainSpecies        = 0x76986F3A; // EncountOutbreakSave_monsno[4]
    private const uint KOutbreak05MainForm           = 0x29A9D701; // EncountOutbreakSave_formno[4]
    private const uint KOutbreak05MainFound          = 0x7E13F8C7; // EncountOutbreakSave_isFind[4]
    private const uint KOutbreak05MainNumKOed        = 0x4B23391E; // EncountOutbreakSave_subjugationCount[4]
    private const uint KOutbreak05MainTotalSpawns    = 0xB7E886B6; // EncountOutbreakSave_subjugationLimit[4]
    private const uint KOutbreak05MainMaterials      = 0x599729C3; // EncountOutbreakSave_dropMaterialCount[4]
    private const uint KOutbreak05MainDeliveryID     = 0xF74D5229; // EncountOutbreakSave_deliveryId[4]
    private const uint KOutbreak05MainDeliveryZoneID = 0xE337865D; // EncountOutbreakSave_deliveryZoneIdx[4]
    private const uint KOutbreak05MainDeliveryPokeID = 0xF1286212; // EncountOutbreakSave_deliveryPokeIdx[4]

    private const uint KOutbreak06MainCenterPos      = 0x2ECB673C; // EncountOutbreakSave_centerPos[5]
    private const uint KOutbreak06MainDummyPos       = 0x4A1C868D; // EncountOutbreakSave_dummyPos[5]
    private const uint KOutbreak06MainSpecies        = 0x76947F97; // EncountOutbreakSave_monsno[5]
    private const uint KOutbreak06MainForm           = 0x29AB994C; // EncountOutbreakSave_formno[5]
    private const uint KOutbreak06MainFound          = 0x7E16A22A; // EncountOutbreakSave_isFind[5]
    private const uint KOutbreak06MainNumKOed        = 0x4B208FBB; // EncountOutbreakSave_subjugationCount[5]
    private const uint KOutbreak06MainTotalSpawns    = 0xB7E49713; // EncountOutbreakSave_subjugationLimit[5]
    private const uint KOutbreak06MainMaterials      = 0x599AACA6; // EncountOutbreakSave_dropMaterialCount[5]
    private const uint KOutbreak06MainDeliveryID     = 0xF7513434; // EncountOutbreakSave_deliveryId[5]
    private const uint KOutbreak06MainDeliveryZoneID = 0xE33B6868; // EncountOutbreakSave_deliveryZoneIdx[5]
    private const uint KOutbreak06MainDeliveryPokeID = 0xF125B8AF; // EncountOutbreakSave_deliveryPokeIdx[5]

    private const uint KOutbreak07MainCenterPos      = 0x2EC1CC77; // EncountOutbreakSave_centerPos[6]
    private const uint KOutbreak07MainDummyPos       = 0x4A1A50B6; // EncountOutbreakSave_dummyPos[6]
    private const uint KOutbreak07MainSpecies        = 0x769D40DC; // EncountOutbreakSave_monsno[6]
    private const uint KOutbreak07MainForm           = 0x29A344C7; // EncountOutbreakSave_formno[6]
    private const uint KOutbreak07MainFound          = 0x7E1A8B01; // EncountOutbreakSave_isFind[6]
    private const uint KOutbreak07MainNumKOed        = 0x4B28E440; // EncountOutbreakSave_subjugationCount[6]
    private const uint KOutbreak07MainTotalSpawns    = 0xB7EE31D8; // EncountOutbreakSave_subjugationLimit[6]
    private const uint KOutbreak07MainMaterials      = 0x599CE27D; // EncountOutbreakSave_dropMaterialCount[6]
    private const uint KOutbreak07MainDeliveryID     = 0xF74872EF; // EncountOutbreakSave_deliveryId[6]
    private const uint KOutbreak07MainDeliveryZoneID = 0xE332A723; // EncountOutbreakSave_deliveryZoneIdx[6]
    private const uint KOutbreak07MainDeliveryPokeID = 0xF12E79F4; // EncountOutbreakSave_deliveryPokeIdx[6]

    private const uint KOutbreak08MainCenterPos      = 0x2EC5BC1A; // EncountOutbreakSave_centerPos[7]
    private const uint KOutbreak08MainDummyPos       = 0x4A166113; // EncountOutbreakSave_dummyPos[7]
    private const uint KOutbreak08MainSpecies        = 0x769B11D1; // EncountOutbreakSave_monsno[7]
    private const uint KOutbreak08MainForm           = 0x29A5EE2A; // EncountOutbreakSave_formno[7]
    private const uint KOutbreak08MainFound          = 0x7E1C4D4C; // EncountOutbreakSave_isFind[7]
    private const uint KOutbreak08MainNumKOed        = 0x4B256EF5; // EncountOutbreakSave_subjugationCount[7]
    private const uint KOutbreak08MainTotalSpawns    = 0xB7EABC8D; // EncountOutbreakSave_subjugationLimit[7]
    private const uint KOutbreak08MainMaterials      = 0x59A0C488; // EncountOutbreakSave_dropMaterialCount[7]
    private const uint KOutbreak08MainDeliveryID     = 0xF74B1C52; // EncountOutbreakSave_deliveryId[7]
    private const uint KOutbreak08MainDeliveryZoneID = 0xE3355086; // EncountOutbreakSave_deliveryZoneIdx[7]
    private const uint KOutbreak08MainDeliveryPokeID = 0xF12C4AE9; // EncountOutbreakSave_deliveryPokeIdx[7]
    #endregion

    #region EncountOutbreakSave_f1 (Kitakami)
    private const uint KOutbreakDLC1NumActive        = 0xBD7C2A04; // EncountOutbreakSave_f1_enablecount

    private const uint KOutbreak01DLC1CenterPos      = 0x411A0C07; // EncountOutbreakSave_f1_centerPos[0]
    private const uint KOutbreak01DLC1DummyPos       = 0x632EFBFE; // EncountOutbreakSave_f1_dummyPos[0]
    private const uint KOutbreak01DLC1Species        = 0x37E55F64; // EncountOutbreakSave_f1_monsno[0]
    private const uint KOutbreak01DLC1Form           = 0x69A930AB; // EncountOutbreakSave_f1_formno[0]
    private const uint KOutbreak01DLC1Found          = 0x7B688081; // EncountOutbreakSave_f1_isFind[0]
    private const uint KOutbreak01DLC1NumKOed        = 0xB29D7978; // EncountOutbreakSave_f1_subjugationCount[0]
    private const uint KOutbreak01DLC1TotalSpawns    = 0x9E16873C; // EncountOutbreakSave_f1_subjugationLimit[0]
    private const uint KOutbreak01DLC1Materials      = 0x0DEEA32D; // EncountOutbreakSave_f1_dropMaterialCount[0]
    private const uint KOutbreak01DLC1DeliveryID     = 0x6CADFC47; // EncountOutbreakSave_f1_deliveryId[0]
    private const uint KOutbreak01DLC1DeliveryZoneID = 0x9F8C7243; // EncountOutbreakSave_f1_deliveryZoneIdx[0]
    private const uint KOutbreak01DLC1DeliveryPokeID = 0x75BCDC74; // EncountOutbreakSave_f1_deliveryPokeIdx[0]

    private const uint KOutbreak02DLC1CenterPos      = 0x411CB56A; // EncountOutbreakSave_f1_centerPos[1]
    private const uint KOutbreak02DLC1DummyPos       = 0x632D2C1B; // EncountOutbreakSave_f1_dummyPos[1]
    private const uint KOutbreak02DLC1Species        = 0x37E33059; // EncountOutbreakSave_f1_monsno[1]
    private const uint KOutbreak02DLC1Form           = 0x69AD204E; // EncountOutbreakSave_f1_formno[1]
    private const uint KOutbreak02DLC1Found          = 0x7B6A42CC; // EncountOutbreakSave_f1_isFind[1]
    private const uint KOutbreak02DLC1NumKOed        = 0xB29ADDAD; // EncountOutbreakSave_f1_subjugationCount[1]
    private const uint KOutbreak02DLC1TotalSpawns    = 0x9E12A531; // EncountOutbreakSave_f1_subjugationLimit[1]
    private const uint KOutbreak02DLC1Materials      = 0x0DF13EF8; // EncountOutbreakSave_f1_dropMaterialCount[1]
    private const uint KOutbreak02DLC1DeliveryID     = 0x6CB0A5AA; // EncountOutbreakSave_f1_deliveryId[1]
    private const uint KOutbreak02DLC1DeliveryZoneID = 0x9F8FF526; // EncountOutbreakSave_f1_deliveryZoneIdx[1]
    private const uint KOutbreak02DLC1DeliveryPokeID = 0x75BAAD69; // EncountOutbreakSave_f1_deliveryPokeIdx[1]

    private const uint KOutbreak03DLC1CenterPos      = 0x411EEB41; // EncountOutbreakSave_f1_centerPos[2]
    private const uint KOutbreak03DLC1DummyPos       = 0x633580A0; // EncountOutbreakSave_f1_dummyPos[2]
    private const uint KOutbreak03DLC1Species        = 0x37DFB442; // EncountOutbreakSave_f1_monsno[2]
    private const uint KOutbreak03DLC1Form           = 0x69AEE965; // EncountOutbreakSave_f1_formno[2]
    private const uint KOutbreak03DLC1Found          = 0x7B61EE47; // EncountOutbreakSave_f1_isFind[2]
    private const uint KOutbreak03DLC1NumKOed        = 0xB298A7D6; // EncountOutbreakSave_f1_subjugationCount[2]
    private const uint KOutbreak03DLC1TotalSpawns    = 0x9E10DC1A; // EncountOutbreakSave_f1_subjugationLimit[2]
    private const uint KOutbreak03DLC1Materials      = 0x0DE87DB3; // EncountOutbreakSave_f1_dropMaterialCount[2]
    private const uint KOutbreak03DLC1DeliveryID     = 0x6CB48E81; // EncountOutbreakSave_f1_deliveryId[2]
    private const uint KOutbreak03DLC1DeliveryZoneID = 0x9F922AFD; // EncountOutbreakSave_f1_deliveryZoneIdx[2]
    private const uint KOutbreak03DLC1DeliveryPokeID = 0x75B6C492; // EncountOutbreakSave_f1_deliveryPokeIdx[2]

    private const uint KOutbreak04DLC1CenterPos      = 0x4122608C; // EncountOutbreakSave_f1_centerPos[3]
    private const uint KOutbreak04DLC1DummyPos       = 0x6332E4D5; // EncountOutbreakSave_f1_dummyPos[3]
    private const uint KOutbreak04DLC1Species        = 0x37DD779F; // EncountOutbreakSave_f1_monsno[3]
    private const uint KOutbreak04DLC1Form           = 0x69B2CB70; // EncountOutbreakSave_f1_formno[3]
    private const uint KOutbreak04DLC1Found          = 0x7B6497AA; // EncountOutbreakSave_f1_isFind[3]
    private const uint KOutbreak04DLC1NumKOed        = 0xB294B833; // EncountOutbreakSave_f1_subjugationCount[3]
    private const uint KOutbreak04DLC1TotalSpawns    = 0x9E0CEC77; // EncountOutbreakSave_f1_subjugationLimit[3]
    private const uint KOutbreak04DLC1Materials      = 0x0DEC6D56; // EncountOutbreakSave_f1_dropMaterialCount[3]
    private const uint KOutbreak04DLC1DeliveryID     = 0x6CB650CC; // EncountOutbreakSave_f1_deliveryId[3]
    private const uint KOutbreak04DLC1DeliveryZoneID = 0x9F960D08; // EncountOutbreakSave_f1_deliveryZoneIdx[3]
    private const uint KOutbreak04DLC1DeliveryPokeID = 0x75B41B2F; // EncountOutbreakSave_f1_deliveryPokeIdx[3]
    #endregion

    #region EncountOutbreakSave_f2 (Blueberry)
    private const uint KOutbreakDLC2NumActive        = 0x19A98811; // EncountOutbreakSave_f2_enablecount

    private const uint KOutbreak01DLC2CenterPos      = 0xCE463C0C; // EncountOutbreakSave_f2_centerPos[0]
    private const uint KOutbreak01DLC2DummyPos       = 0x0B0C71CB; // EncountOutbreakSave_f2_dummyPos[0]
    private const uint KOutbreak01DLC2Species        = 0xB8E99C8D; // EncountOutbreakSave_f2_monsno[0]
    private const uint KOutbreak01DLC2Form           = 0xEFA6983A; // EncountOutbreakSave_f2_formno[0]
    private const uint KOutbreak01DLC2Found          = 0x32074910; // EncountOutbreakSave_f2_isFind[0]
    private const uint KOutbreak01DLC2NumKOed        = 0x4EF9BC25; // EncountOutbreakSave_f2_subjugationCount[0]
    private const uint KOutbreak01DLC2TotalSpawns    = 0x4385E0AD; // EncountOutbreakSave_f2_subjugationLimit[0]
    private const uint KOutbreak01DLC2Materials      = 0x01B2F482; // EncountOutbreakSave_f2_dropMaterialCount[0]
    private const uint KOutbreak01DLC2DeliveryID     = 0x8273D376; // EncountOutbreakSave_f2_deliveryId[0]
    private const uint KOutbreak01DLC2DeliveryZoneID = 0x1BF676D4; // EncountOutbreakSave_f2_deliveryZoneIdx[0]
    private const uint KOutbreak01DLC2DeliveryPokeID = 0x1786D1CB; // EncountOutbreakSave_f2_deliveryPokeIdx[0]
    #endregion

    #region EncountOutbreakSave_bc (Paldea, BCAT)
    private const uint KOutbreakBCMainNumActive        = 0x7478FD9A; // EncountOutbreakSave_bc_enablecount

    private const uint KOutbreakBC01MainCenterPos      = 0x71DB2C9D; // EncountOutbreakSave_bc_centerPos[0]
    private const uint KOutbreakBC01MainDummyPos       = 0xB5D2D0EC; // EncountOutbreakSave_bc_dummyPos[0]
    private const uint KOutbreakBC01MainSpecies        = 0x84AB44A6; // EncountOutbreakSave_bc_monsno[0]
    private const uint KOutbreakBC01MainForm           = 0xD82BDDAD; // EncountOutbreakSave_bc_formno[0]
    private const uint KOutbreakBC01MainFound          = 0x6F473373; // EncountOutbreakSave_bc_isFind[0]
    private const uint KOutbreakBC01MainNumKOed        = 0x65AC15F2; // EncountOutbreakSave_bc_subjugationCount[0]
    private const uint KOutbreakBC01MainTotalSpawns    = 0x71862A2A; // EncountOutbreakSave_bc_subjugationLimit[0]
    private const uint KOutbreakBC01MainMaterials      = 0xB577AF37; // EncountOutbreakSave_bc_dropMaterialCount[0]
    private const uint KOutbreakBC01MainDeliveryID     = 0xF2248B55; // EncountOutbreakSave_bc_deliveryId[0]
    private const uint KOutbreakBC01MainDeliveryZoneID = 0xC41AEEB1; // EncountOutbreakSave_bc_deliveryZoneIdx[0]
    private const uint KOutbreakBC01MainDeliveryPokeID = 0x53DFC2DE; // EncountOutbreakSave_bc_deliveryPokeIdx[0]

    private const uint KOutbreakBC02MainCenterPos      = 0x71DD5BA8; // EncountOutbreakSave_bc_centerPos[1]
    private const uint KOutbreakBC02MainDummyPos       = 0xB5D03521; // EncountOutbreakSave_bc_dummyPos[1]
    private const uint KOutbreakBC02MainSpecies        = 0x84A7C1C3; // EncountOutbreakSave_bc_monsno[1]
    private const uint KOutbreakBC02MainForm           = 0xD82E7978; // EncountOutbreakSave_bc_formno[1]
    private const uint KOutbreakBC02MainFound          = 0x6F497016; // EncountOutbreakSave_bc_isFind[1]
    private const uint KOutbreakBC02MainNumKOed        = 0x65A8930F; // EncountOutbreakSave_bc_subjugationCount[1]
    private const uint KOutbreakBC02MainTotalSpawns    = 0x718380C7; // EncountOutbreakSave_bc_subjugationLimit[1]
    private const uint KOutbreakBC02MainMaterials      = 0xB579EBDA; // EncountOutbreakSave_bc_dropMaterialCount[1]
    private const uint KOutbreakBC02MainDeliveryID     = 0xF2272720; // EncountOutbreakSave_bc_deliveryId[1]
    private const uint KOutbreakBC02MainDeliveryZoneID = 0xC41ED0BC; // EncountOutbreakSave_bc_deliveryZoneIdx[1]
    private const uint KOutbreakBC02MainDeliveryPokeID = 0x53DD197B; // EncountOutbreakSave_bc_deliveryPokeIdx[1]

    private const uint KOutbreakBC03MainCenterPos      = 0x71D49A63; // EncountOutbreakSave_bc_centerPos[2]
    private const uint KOutbreakBC03MainDummyPos       = 0xB5CC4C4A; // EncountOutbreakSave_bc_dummyPos[2]
    private const uint KOutbreakBC03MainSpecies        = 0x84B15C88; // EncountOutbreakSave_bc_monsno[2]
    private const uint KOutbreakBC03MainForm           = 0xD825B833; // EncountOutbreakSave_bc_formno[2]
    private const uint KOutbreakBC03MainFound          = 0x6F4D58ED; // EncountOutbreakSave_bc_isFind[2]
    private const uint KOutbreakBC03MainNumKOed        = 0x65B22DD4; // EncountOutbreakSave_bc_subjugationCount[2]
    private const uint KOutbreakBC03MainTotalSpawns    = 0x718BD54C; // EncountOutbreakSave_bc_subjugationLimit[2]
    private const uint KOutbreakBC03MainMaterials      = 0xB57D67F1; // EncountOutbreakSave_bc_dropMaterialCount[2]
    private const uint KOutbreakBC03MainDeliveryID     = 0xF21ED29B; // EncountOutbreakSave_bc_deliveryId[2]
    private const uint KOutbreakBC03MainDeliveryZoneID = 0xC41535F7; // EncountOutbreakSave_bc_deliveryZoneIdx[2]
    private const uint KOutbreakBC03MainDeliveryPokeID = 0x53E56E00; // EncountOutbreakSave_bc_deliveryPokeIdx[2]

    private const uint KOutbreakBC04MainCenterPos      = 0x71D743C6; // EncountOutbreakSave_bc_centerPos[3]
    private const uint KOutbreakBC04MainDummyPos       = 0xB5CA7C67; // EncountOutbreakSave_bc_dummyPos[3]
    private const uint KOutbreakBC04MainSpecies        = 0x84AD7A7D; // EncountOutbreakSave_bc_monsno[3]
    private const uint KOutbreakBC04MainForm           = 0xD829A7D6; // EncountOutbreakSave_bc_formno[3]
    private const uint KOutbreakBC04MainFound          = 0x6F4FF4B8; // EncountOutbreakSave_bc_isFind[3]
    private const uint KOutbreakBC04MainNumKOed        = 0x65AE4BC9; // EncountOutbreakSave_bc_subjugationCount[3]
    private const uint KOutbreakBC04MainTotalSpawns    = 0x718A1301; // EncountOutbreakSave_bc_subjugationLimit[3]
    private const uint KOutbreakBC04MainMaterials      = 0xB57F96FC; // EncountOutbreakSave_bc_dropMaterialCount[3]
    private const uint KOutbreakBC04MainDeliveryID     = 0xF220A27E; // EncountOutbreakSave_bc_deliveryId[3]
    private const uint KOutbreakBC04MainDeliveryZoneID = 0xC419259A; // EncountOutbreakSave_bc_deliveryZoneIdx[3]
    private const uint KOutbreakBC04MainDeliveryPokeID = 0x53E3ABB5; // EncountOutbreakSave_bc_deliveryPokeIdx[3]

    private const uint KOutbreakBC05MainCenterPos      = 0x71CEEF41; // EncountOutbreakSave_bc_centerPos[4]
    private const uint KOutbreakBC05MainDummyPos       = 0xB5DEA188; // EncountOutbreakSave_bc_dummyPos[4]
    private const uint KOutbreakBC05MainSpecies        = 0x849F074A; // EncountOutbreakSave_bc_monsno[4]
    private const uint KOutbreakBC05MainForm           = 0xD8200D11; // EncountOutbreakSave_bc_formno[4]
    private const uint KOutbreakBC05MainFound          = 0x6F3AF617; // EncountOutbreakSave_bc_isFind[4]
    private const uint KOutbreakBC05MainNumKOed        = 0x65B70D0E; // EncountOutbreakSave_bc_subjugationCount[4]
    private const uint KOutbreakBC05MainTotalSpawns    = 0x71926786; // EncountOutbreakSave_bc_subjugationLimit[4]
    private const uint KOutbreakBC05MainMaterials      = 0xB5823993; // EncountOutbreakSave_bc_dropMaterialCount[4]
    private const uint KOutbreakBC05MainDeliveryID     = 0xF218BAB9; // EncountOutbreakSave_bc_deliveryId[4]
    private const uint KOutbreakBC05MainDeliveryZoneID = 0xC42798CD; // EncountOutbreakSave_bc_deliveryZoneIdx[4]
    private const uint KOutbreakBC05MainDeliveryPokeID = 0x53D53882; // EncountOutbreakSave_bc_deliveryPokeIdx[4]

    private const uint KOutbreakBC06MainCenterPos      = 0x71D2648C; // EncountOutbreakSave_bc_centerPos[5]
    private const uint KOutbreakBC06MainDummyPos       = 0xB5DABF7D; // EncountOutbreakSave_bc_dummyPos[5]
    private const uint KOutbreakBC06MainSpecies        = 0x849D3767; // EncountOutbreakSave_bc_monsno[5]
    private const uint KOutbreakBC06MainForm           = 0xD823EF1C; // EncountOutbreakSave_bc_formno[5]
    private const uint KOutbreakBC06MainFound          = 0x6F3EE5BA; // EncountOutbreakSave_bc_isFind[5]
    private const uint KOutbreakBC06MainNumKOed        = 0x65B4D06B; // EncountOutbreakSave_bc_subjugationCount[5]
    private const uint KOutbreakBC06MainTotalSpawns    = 0x718FBE23; // EncountOutbreakSave_bc_subjugationLimit[5]
    private const uint KOutbreakBC06MainMaterials      = 0xB5862936; // EncountOutbreakSave_bc_dropMaterialCount[5]
    private const uint KOutbreakBC06MainDeliveryID     = 0xF21AE9C4; // EncountOutbreakSave_bc_deliveryId[5]
    private const uint KOutbreakBC06MainDeliveryZoneID = 0xC4295B18; // EncountOutbreakSave_bc_deliveryZoneIdx[5]
    private const uint KOutbreakBC06MainDeliveryPokeID = 0x53D148DF; // EncountOutbreakSave_bc_deliveryPokeIdx[5]

    private const uint KOutbreakBC07MainCenterPos      = 0x71CA1007; // EncountOutbreakSave_bc_centerPos[6]
    private const uint KOutbreakBC07MainDummyPos       = 0xB5D889A6; // EncountOutbreakSave_bc_dummyPos[6]
    private const uint KOutbreakBC07MainSpecies        = 0x84A58BEC; // EncountOutbreakSave_bc_monsno[6]
    private const uint KOutbreakBC07MainForm           = 0xD81B2DD7; // EncountOutbreakSave_bc_formno[6]
    private const uint KOutbreakBC07MainFound          = 0x6F418851; // EncountOutbreakSave_bc_isFind[6]
    private const uint KOutbreakBC07MainNumKOed        = 0x65BCB830; // EncountOutbreakSave_bc_subjugationCount[6]
    private const uint KOutbreakBC07MainTotalSpawns    = 0x71987F68; // EncountOutbreakSave_bc_subjugationLimit[6]
    private const uint KOutbreakBC07MainMaterials      = 0xB5885F0D; // EncountOutbreakSave_bc_dropMaterialCount[6]
    private const uint KOutbreakBC07MainDeliveryID     = 0xF212287F; // EncountOutbreakSave_bc_deliveryId[6]
    private const uint KOutbreakBC07MainDeliveryZoneID = 0xC4217353; // EncountOutbreakSave_bc_deliveryZoneIdx[6]
    private const uint KOutbreakBC07MainDeliveryPokeID = 0x53DAE3A4; // EncountOutbreakSave_bc_deliveryPokeIdx[6]

    private const uint KOutbreakBC08MainCenterPos      = 0x71CCB96A; // EncountOutbreakSave_bc_centerPos[7]
    private const uint KOutbreakBC08MainDummyPos       = 0xB5D506C3; // EncountOutbreakSave_bc_dummyPos[7]
    private const uint KOutbreakBC08MainSpecies        = 0x84A2F021; // EncountOutbreakSave_bc_monsno[7]
    private const uint KOutbreakBC08MainForm           = 0xD81D6A7A; // EncountOutbreakSave_bc_formno[7]
    private const uint KOutbreakBC08MainFound          = 0x6F43B75C; // EncountOutbreakSave_bc_isFind[7]
    private const uint KOutbreakBC08MainNumKOed        = 0x65BA8925; // EncountOutbreakSave_bc_subjugationCount[7]
    private const uint KOutbreakBC08MainTotalSpawns    = 0x71949D5D; // EncountOutbreakSave_bc_subjugationLimit[7]
    private const uint KOutbreakBC08MainMaterials      = 0xB58BD458; // EncountOutbreakSave_bc_dropMaterialCount[7]
    private const uint KOutbreakBC08MainDeliveryID     = 0xF2161822; // EncountOutbreakSave_bc_deliveryId[7]
    private const uint KOutbreakBC08MainDeliveryZoneID = 0xC423AFF6; // EncountOutbreakSave_bc_deliveryZoneIdx[7]
    private const uint KOutbreakBC08MainDeliveryPokeID = 0x53D70199; // EncountOutbreakSave_bc_deliveryPokeIdx[7]

    private const uint KOutbreakBC09MainCenterPos      = 0x71F18795; // EncountOutbreakSave_bc_centerPos[8]
    private const uint KOutbreakBC09MainDummyPos       = 0xB5E92BE4; // EncountOutbreakSave_bc_dummyPos[8]
    private const uint KOutbreakBC09MainSpecies        = 0x84C2791E; // EncountOutbreakSave_bc_monsno[8]
    private const uint KOutbreakBC09MainForm           = 0xD842A565; // EncountOutbreakSave_bc_formno[8]
    private const uint KOutbreakBC09MainFound          = 0x6F5E67EB; // EncountOutbreakSave_bc_isFind[8]
    private const uint KOutbreakBC09MainNumKOed        = 0x65954E3A; // EncountOutbreakSave_bc_subjugationCount[8]
    private const uint KOutbreakBC09MainTotalSpawns    = 0x719E3822; // EncountOutbreakSave_bc_subjugationLimit[8]
    private const uint KOutbreakBC09MainMaterials      = 0xB58E0A2F; // EncountOutbreakSave_bc_dropMaterialCount[8]
    private const uint KOutbreakBC09MainDeliveryID     = 0xF20C7D5D; // EncountOutbreakSave_bc_deliveryId[8]
    private const uint KOutbreakBC09MainDeliveryZoneID = 0xC4322329; // EncountOutbreakSave_bc_deliveryZoneIdx[8]
    private const uint KOutbreakBC09MainDeliveryPokeID = 0x53C88E66; // EncountOutbreakSave_bc_deliveryPokeIdx[8]

    private const uint KOutbreakBC10MainCenterPos      = 0x71F42360; // EncountOutbreakSave_bc_centerPos[9]
    private const uint KOutbreakBC10MainDummyPos       = 0xB5E6FCD9; // EncountOutbreakSave_bc_dummyPos[9]
    private const uint KOutbreakBC10MainSpecies        = 0x84BFCFBB; // EncountOutbreakSave_bc_monsno[9]
    private const uint KOutbreakBC10MainForm           = 0xD8468770; // EncountOutbreakSave_bc_formno[9]
    private const uint KOutbreakBC10MainFound          = 0x6F60A48E; // EncountOutbreakSave_bc_isFind[9]
    private const uint KOutbreakBC10MainNumKOed        = 0x65915E97; // EncountOutbreakSave_bc_subjugationCount[9]
    private const uint KOutbreakBC10MainTotalSpawns    = 0x719A487F; // EncountOutbreakSave_bc_subjugationLimit[9]
    private const uint KOutbreakBC10MainMaterials      = 0xB590B392; // EncountOutbreakSave_bc_dropMaterialCount[9]
    private const uint KOutbreakBC10MainDeliveryID     = 0xF2105F68; // EncountOutbreakSave_bc_deliveryId[9]
    private const uint KOutbreakBC10MainDeliveryZoneID = 0xC4360534; // EncountOutbreakSave_bc_deliveryZoneIdx[9]
    private const uint KOutbreakBC10MainDeliveryPokeID = 0x53C6BE83; // EncountOutbreakSave_bc_deliveryPokeIdx[9]
    #endregion

    #region EncountOutbreakSave_f1_bc (Kitakami, BCAT)
    private const uint KOutbreakBCDLC1NumActive        = 0x0D326604; // EncountOutbreakSave_f1_bc_enablecount

    private const uint KOutbreakBC01DLC1CenterPos      = 0xB3C20007; // EncountOutbreakSave_f1_bc_centerPos[0]
    private const uint KOutbreakBC01DLC1DummyPos       = 0xB2E537FE; // EncountOutbreakSave_f1_bc_dummyPos[0]
    private const uint KOutbreakBC01DLC1Species        = 0x0F4D3B64; // EncountOutbreakSave_f1_bc_monsno[0]
    private const uint KOutbreakBC01DLC1Form           = 0x41110CAB; // EncountOutbreakSave_f1_bc_formno[0]
    private const uint KOutbreakBC01DLC1Found          = 0x52D05C81; // EncountOutbreakSave_f1_bc_isFind[0]
    private const uint KOutbreakBC01DLC1NumKOed        = 0xAA733578; // EncountOutbreakSave_f1_bc_subjugationCount[0]
    private const uint KOutbreakBC01DLC1TotalSpawns    = 0x95EC433C; // EncountOutbreakSave_f1_bc_subjugationLimit[0]
    private const uint KOutbreakBC01DLC1Materials      = 0x2E1D172D; // EncountOutbreakSave_f1_bc_dropMaterialCount[0]
    private const uint KOutbreakBC01DLC1DeliveryID     = 0x40119847; // EncountOutbreakSave_f1_bc_deliveryId[0]
    private const uint KOutbreakBC01DLC1DeliveryZoneID = 0xC1A9C643; // EncountOutbreakSave_f1_bc_deliveryZoneIdx[0]
    private const uint KOutbreakBC01DLC1DeliveryPokeID = 0x97DA3074; // EncountOutbreakSave_f1_bc_deliveryPokeIdx[0]

    private const uint KOutbreakBC02DLC1CenterPos      = 0xB3C4A96A; // EncountOutbreakSave_f1_bc_centerPos[1]
    private const uint KOutbreakBC02DLC1DummyPos       = 0xB2E3681B; // EncountOutbreakSave_f1_bc_dummyPos[1]
    private const uint KOutbreakBC02DLC1Species        = 0x0F4B0C59; // EncountOutbreakSave_f1_bc_monsno[1]
    private const uint KOutbreakBC02DLC1Form           = 0x4114FC4E; // EncountOutbreakSave_f1_bc_formno[1]
    private const uint KOutbreakBC02DLC1Found          = 0x52D21ECC; // EncountOutbreakSave_f1_bc_isFind[1]
    private const uint KOutbreakBC02DLC1NumKOed        = 0xAA7099AD; // EncountOutbreakSave_f1_bc_subjugationCount[1]
    private const uint KOutbreakBC02DLC1TotalSpawns    = 0x95E86131; // EncountOutbreakSave_f1_bc_subjugationLimit[1]
    private const uint KOutbreakBC02DLC1Materials      = 0x2E1FB2F8; // EncountOutbreakSave_f1_bc_dropMaterialCount[1]
    private const uint KOutbreakBC02DLC1DeliveryID     = 0x401441AA; // EncountOutbreakSave_f1_bc_deliveryId[1]
    private const uint KOutbreakBC02DLC1DeliveryZoneID = 0xC1AD4926; // EncountOutbreakSave_f1_bc_deliveryZoneIdx[1]
    private const uint KOutbreakBC02DLC1DeliveryPokeID = 0x97D80169; // EncountOutbreakSave_f1_bc_deliveryPokeIdx[1]

    private const uint KOutbreakBC03DLC1CenterPos      = 0xB3C6DF41; // EncountOutbreakSave_f1_bc_centerPos[2]
    private const uint KOutbreakBC03DLC1DummyPos       = 0xB2EBBCA0; // EncountOutbreakSave_f1_bc_dummyPos[2]
    private const uint KOutbreakBC03DLC1Species        = 0x0F479042; // EncountOutbreakSave_f1_bc_monsno[2]
    private const uint KOutbreakBC03DLC1Form           = 0x4116C565; // EncountOutbreakSave_f1_bc_formno[2]
    private const uint KOutbreakBC03DLC1Found          = 0x52C9CA47; // EncountOutbreakSave_f1_bc_isFind[2]
    private const uint KOutbreakBC03DLC1NumKOed        = 0xAA6E63D6; // EncountOutbreakSave_f1_bc_subjugationCount[2]
    private const uint KOutbreakBC03DLC1TotalSpawns    = 0x95E6981A; // EncountOutbreakSave_f1_bc_subjugationLimit[2]
    private const uint KOutbreakBC03DLC1Materials      = 0x2E16F1B3; // EncountOutbreakSave_f1_bc_dropMaterialCount[2]
    private const uint KOutbreakBC03DLC1DeliveryID     = 0x40182A81; // EncountOutbreakSave_f1_bc_deliveryId[2]
    private const uint KOutbreakBC03DLC1DeliveryZoneID = 0xC1AF7EFD; // EncountOutbreakSave_f1_bc_deliveryZoneIdx[2]
    private const uint KOutbreakBC03DLC1DeliveryPokeID = 0x97D41892; // EncountOutbreakSave_f1_bc_deliveryPokeIdx[2]

    private const uint KOutbreakBC04DLC1CenterPos      = 0xB3CA548C; // EncountOutbreakSave_f1_bc_centerPos[3]
    private const uint KOutbreakBC04DLC1DummyPos       = 0xB2E920D5; // EncountOutbreakSave_f1_bc_dummyPos[3]
    private const uint KOutbreakBC04DLC1Species        = 0x0F45539F; // EncountOutbreakSave_f1_bc_monsno[3]
    private const uint KOutbreakBC04DLC1Form           = 0x411AA770; // EncountOutbreakSave_f1_bc_formno[3]
    private const uint KOutbreakBC04DLC1Found          = 0x52CC73AA; // EncountOutbreakSave_f1_bc_isFind[3]
    private const uint KOutbreakBC04DLC1NumKOed        = 0xAA6A7433; // EncountOutbreakSave_f1_bc_subjugationCount[3]
    private const uint KOutbreakBC04DLC1TotalSpawns    = 0x95E2A877; // EncountOutbreakSave_f1_bc_subjugationLimit[3]
    private const uint KOutbreakBC04DLC1Materials      = 0x2E1AE156; // EncountOutbreakSave_f1_bc_dropMaterialCount[3]
    private const uint KOutbreakBC04DLC1DeliveryID     = 0x4019ECCC; // EncountOutbreakSave_f1_bc_deliveryId[3]
    private const uint KOutbreakBC04DLC1DeliveryZoneID = 0xC1B36108; // EncountOutbreakSave_f1_bc_deliveryZoneIdx[3]
    private const uint KOutbreakBC04DLC1DeliveryPokeID = 0x97D16F2F; // EncountOutbreakSave_f1_bc_deliveryPokeIdx[3]

    private const uint KOutbreakBC05DLC1CenterPos      = 0xB3CC8A63; // EncountOutbreakSave_f1_bc_centerPos[4]
    private const uint KOutbreakBC05DLC1DummyPos       = 0xB2DAADA2; // EncountOutbreakSave_f1_bc_dummyPos[4]
    private const uint KOutbreakBC05DLC1Species        = 0x0F5978C0; // EncountOutbreakSave_f1_bc_monsno[4]
    private const uint KOutbreakBC05DLC1Form           = 0x4106824F; // EncountOutbreakSave_f1_bc_formno[4]
    private const uint KOutbreakBC05DLC1Found          = 0x52DAE6DD; // EncountOutbreakSave_f1_bc_isFind[4]
    private const uint KOutbreakBC05DLC1NumKOed        = 0xAA68AB1C; // EncountOutbreakSave_f1_bc_subjugationCount[4]
    private const uint KOutbreakBC05DLC1TotalSpawns    = 0x95F6CD98; // EncountOutbreakSave_f1_bc_subjugationLimit[4]
    private const uint KOutbreakBC05DLC1Materials      = 0x2E114691; // EncountOutbreakSave_f1_bc_dropMaterialCount[4]
    private const uint KOutbreakBC05DLC1DeliveryID     = 0x401DD5A3; // EncountOutbreakSave_f1_bc_deliveryId[4]
    private const uint KOutbreakBC05DLC1DeliveryZoneID = 0xC19F3BE7; // EncountOutbreakSave_f1_bc_deliveryZoneIdx[4]
    private const uint KOutbreakBC05DLC1DeliveryPokeID = 0x97E66DD0; // EncountOutbreakSave_f1_bc_deliveryPokeIdx[4]

    private const uint KOutbreakBC06DLC1CenterPos      = 0xB3CF33C6; // EncountOutbreakSave_f1_bc_centerPos[5]
    private const uint KOutbreakBC06DLC1DummyPos       = 0xB2D6BDFF; // EncountOutbreakSave_f1_bc_dummyPos[5]
    private const uint KOutbreakBC06DLC1Species        = 0x0F560375; // EncountOutbreakSave_f1_bc_monsno[5]
    private const uint KOutbreakBC06DLC1Form           = 0x41085232; // EncountOutbreakSave_f1_bc_formno[5]
    private const uint KOutbreakBC06DLC1Found          = 0x52DEC8E8; // EncountOutbreakSave_f1_bc_isFind[5]
    private const uint KOutbreakBC06DLC1NumKOed        = 0xAA64C911; // EncountOutbreakSave_f1_bc_subjugationCount[5]
    private const uint KOutbreakBC06DLC1TotalSpawns    = 0x95F50B4D; // EncountOutbreakSave_f1_bc_subjugationLimit[5]
    private const uint KOutbreakBC06DLC1Materials      = 0x2E15289C; // EncountOutbreakSave_f1_bc_dropMaterialCount[5]
    private const uint KOutbreakBC06DLC1DeliveryID     = 0x40207F06; // EncountOutbreakSave_f1_bc_deliveryId[5]
    private const uint KOutbreakBC06DLC1DeliveryZoneID = 0xC1A10BCA; // EncountOutbreakSave_f1_bc_deliveryZoneIdx[5]
    private const uint KOutbreakBC06DLC1DeliveryPokeID = 0x97E28BC5; // EncountOutbreakSave_f1_bc_deliveryPokeIdx[5]

    private const uint KOutbreakBC07DLC1CenterPos      = 0xB3D31C9D; // EncountOutbreakSave_f1_bc_centerPos[6]
    private const uint KOutbreakBC07DLC1DummyPos       = 0xB2DF7F44; // EncountOutbreakSave_f1_bc_dummyPos[6]
    private const uint KOutbreakBC07DLC1Species        = 0x0F53CD9E; // EncountOutbreakSave_f1_bc_monsno[6]
    private const uint KOutbreakBC07DLC1Form           = 0x410C3B09; // EncountOutbreakSave_f1_bc_formno[6]
    private const uint KOutbreakBC07DLC1Found          = 0x52D607A3; // EncountOutbreakSave_f1_bc_isFind[6]
    private const uint KOutbreakBC07DLC1NumKOed        = 0xAA62267A; // EncountOutbreakSave_f1_bc_subjugationCount[6]
    private const uint KOutbreakBC07DLC1TotalSpawns    = 0x95F12276; // EncountOutbreakSave_f1_bc_subjugationLimit[6]
    private const uint KOutbreakBC07DLC1Materials      = 0x2E0C6757; // EncountOutbreakSave_f1_bc_dropMaterialCount[6]
    private const uint KOutbreakBC07DLC1DeliveryID     = 0x4022B4DD; // EncountOutbreakSave_f1_bc_deliveryId[6]
    private const uint KOutbreakBC07DLC1DeliveryZoneID = 0xC1A4F4A1; // EncountOutbreakSave_f1_bc_deliveryZoneIdx[6]
    private const uint KOutbreakBC07DLC1DeliveryPokeID = 0x97DFE92E; // EncountOutbreakSave_f1_bc_deliveryPokeIdx[6]

    private const uint KOutbreakBC08DLC1CenterPos      = 0xB3D54BA8; // EncountOutbreakSave_f1_bc_centerPos[7]
    private const uint KOutbreakBC08DLC1DummyPos       = 0xB2DD5039; // EncountOutbreakSave_f1_bc_dummyPos[7]
    private const uint KOutbreakBC08DLC1Species        = 0x0F51243B; // EncountOutbreakSave_f1_bc_monsno[7]
    private const uint KOutbreakBC08DLC1Form           = 0x410E6A14; // EncountOutbreakSave_f1_bc_formno[7]
    private const uint KOutbreakBC08DLC1Found          = 0x52D8B106; // EncountOutbreakSave_f1_bc_isFind[7]
    private const uint KOutbreakBC08DLC1NumKOed        = 0xAA5FE9D7; // EncountOutbreakSave_f1_bc_subjugationCount[7]
    private const uint KOutbreakBC08DLC1TotalSpawns    = 0x95EEE5D3; // EncountOutbreakSave_f1_bc_subjugationLimit[7]
    private const uint KOutbreakBC08DLC1Materials      = 0x2E0EA3FA; // EncountOutbreakSave_f1_bc_dropMaterialCount[7]
    private const uint KOutbreakBC08DLC1DeliveryID     = 0x402696E8; // EncountOutbreakSave_f1_bc_deliveryId[7]
    private const uint KOutbreakBC08DLC1DeliveryZoneID = 0xC1A7906C; // EncountOutbreakSave_f1_bc_deliveryZoneIdx[7]
    private const uint KOutbreakBC08DLC1DeliveryPokeID = 0x97DDAC8B; // EncountOutbreakSave_f1_bc_deliveryPokeIdx[7]

    private const uint KOutbreakBC09DLC1CenterPos      = 0xB3D8C7BF; // EncountOutbreakSave_f1_bc_centerPos[8]
    private const uint KOutbreakBC09DLC1DummyPos       = 0xB2CEDD06; // EncountOutbreakSave_f1_bc_dummyPos[8]
    private const uint KOutbreakBC09DLC1Species        = 0x0F36E06C; // EncountOutbreakSave_f1_bc_monsno[8]
    private const uint KOutbreakBC09DLC1Form           = 0x40F9D833; // EncountOutbreakSave_f1_bc_formno[8]
    private const uint KOutbreakBC09DLC1Found          = 0x52E72439; // EncountOutbreakSave_f1_bc_isFind[8]
    private const uint KOutbreakBC09DLC1NumKOed        = 0xAA8B4370; // EncountOutbreakSave_f1_bc_subjugationCount[8]
    private const uint KOutbreakBC09DLC1TotalSpawns    = 0x960377B4; // EncountOutbreakSave_f1_bc_subjugationLimit[8]
    private const uint KOutbreakBC09DLC1Materials      = 0x2E33DEE5; // EncountOutbreakSave_f1_bc_dropMaterialCount[8]
    private const uint KOutbreakBC09DLC1DeliveryID     = 0x40285FFF; // EncountOutbreakSave_f1_bc_deliveryId[8]
    private const uint KOutbreakBC09DLC1DeliveryZoneID = 0xC1C1D43B; // EncountOutbreakSave_f1_bc_deliveryZoneIdx[8]
    private const uint KOutbreakBC09DLC1DeliveryPokeID = 0x97C2FBFC; // EncountOutbreakSave_f1_bc_deliveryPokeIdx[8]

    private const uint KOutbreakBC10DLC1CenterPos      = 0xB3DB0462; // EncountOutbreakSave_f1_bc_centerPos[9]
    private const uint KOutbreakBC10DLC1DummyPos       = 0xB2CC33A3; // EncountOutbreakSave_f1_bc_dummyPos[9]
    private const uint KOutbreakBC10DLC1Species        = 0x0F3444A1; // EncountOutbreakSave_f1_bc_monsno[9]
    private const uint KOutbreakBC10DLC1Form           = 0x40FDC7D6; // EncountOutbreakSave_f1_bc_formno[9]
    private const uint KOutbreakBC10DLC1Found          = 0x52E95344; // EncountOutbreakSave_f1_bc_isFind[9]
    private const uint KOutbreakBC10DLC1NumKOed        = 0xAA876165; // EncountOutbreakSave_f1_bc_subjugationCount[9]
    private const uint KOutbreakBC10DLC1TotalSpawns    = 0x95FF95A9; // EncountOutbreakSave_f1_bc_subjugationLimit[9]
    private const uint KOutbreakBC10DLC1Materials      = 0x2E37C0F0; // EncountOutbreakSave_f1_bc_dropMaterialCount[9]
    private const uint KOutbreakBC10DLC1DeliveryID     = 0x402C4FA2; // EncountOutbreakSave_f1_bc_deliveryId[9]
    private const uint KOutbreakBC10DLC1DeliveryZoneID = 0xC1C47D9E; // EncountOutbreakSave_f1_bc_deliveryZoneIdx[9]
    private const uint KOutbreakBC10DLC1DeliveryPokeID = 0x97C0CCF1; // EncountOutbreakSave_f1_bc_deliveryPokeIdx[9]
    #endregion

    #region EncountOutbreakSave_f2_bc (Blueberry, BCAT)
    private const uint KOutbreakBCDLC2NumActive        = 0x1B4ECAC3; // EncountOutbreakSave_f2_bc_enablecount

    private const uint KOutbreakBC01DLC2CenterPos      = 0xE623D9F6; // EncountOutbreakSave_f2_bc_centerPos[0]
    private const uint KOutbreakBC01DLC2DummyPos       = 0xB1E70E4D; // EncountOutbreakSave_f2_bc_dummyPos[0]
    private const uint KOutbreakBC01DLC2Species        = 0x03B50A2B; // EncountOutbreakSave_f2_bc_monsno[0]
    private const uint KOutbreakBC01DLC2Form           = 0x9F47C0A8; // EncountOutbreakSave_f2_bc_formno[0]
    private const uint KOutbreakBC01DLC2Found          = 0x57C23026; // EncountOutbreakSave_f2_bc_isFind[0]
    private const uint KOutbreakBC01DLC2NumKOed        = 0x6CB77613; // EncountOutbreakSave_f2_bc_subjugationCount[0]
    private const uint KOutbreakBC01DLC2TotalSpawns    = 0xCDB0C887; // EncountOutbreakSave_f2_bc_subjugationLimit[0]
    private const uint KOutbreakBC01DLC2Materials      = 0x09934588; // EncountOutbreakSave_f2_bc_dropMaterialCount[0]
    private const uint KOutbreakBC01DLC2DeliveryID     = 0x4E4E8528; // EncountOutbreakSave_f2_bc_deliveryId[0]
    private const uint KOutbreakBC01DLC2DeliveryZoneID = 0xD38F831E; // EncountOutbreakSave_f2_bc_deliveryZoneIdx[0]
    private const uint KOutbreakBC01DLC2DeliveryPokeID = 0x19334439; // EncountOutbreakSave_f2_bc_deliveryPokeIdx[0]

    private const uint KOutbreakBC02DLC2CenterPos      = 0xE6219D53; // EncountOutbreakSave_f2_bc_centerPos[1]
    private const uint KOutbreakBC02DLC2DummyPos       = 0xB1E8D098; // EncountOutbreakSave_f2_bc_dummyPos[1]
    private const uint KOutbreakBC02DLC2Species        = 0x03B8F9CE; // EncountOutbreakSave_f2_bc_monsno[1]
    private const uint KOutbreakBC02DLC2Form           = 0x9F45919D; // EncountOutbreakSave_f2_bc_formno[1]
    private const uint KOutbreakBC02DLC2Found          = 0x57BEAD43; // EncountOutbreakSave_f2_bc_isFind[1]
    private const uint KOutbreakBC02DLC2NumKOed        = 0x6CBB65B6; // EncountOutbreakSave_f2_bc_subjugationCount[1]
    private const uint KOutbreakBC02DLC2TotalSpawns    = 0xCDB371EA; // EncountOutbreakSave_f2_bc_subjugationLimit[1]
    private const uint KOutbreakBC02DLC2Materials      = 0x098F637D; // EncountOutbreakSave_f2_bc_dropMaterialCount[1]
    private const uint KOutbreakBC02DLC2DeliveryID     = 0x4E4C561D; // EncountOutbreakSave_f2_bc_deliveryId[1]
    private const uint KOutbreakBC02DLC2DeliveryZoneID = 0xD38CD9BB; // EncountOutbreakSave_f2_bc_deliveryZoneIdx[1]
    private const uint KOutbreakBC02DLC2DeliveryPokeID = 0x19357344; // EncountOutbreakSave_f2_bc_deliveryPokeIdx[1]

    private const uint KOutbreakBC03DLC2CenterPos      = 0xE6298518; // EncountOutbreakSave_f2_bc_centerPos[2]
    private const uint KOutbreakBC03DLC2DummyPos       = 0xB1E0E8D3; // EncountOutbreakSave_f2_bc_dummyPos[2]
    private const uint KOutbreakBC03DLC2Species        = 0x03BAC2E5; // EncountOutbreakSave_f2_bc_monsno[2]
    private const uint KOutbreakBC03DLC2Form           = 0x9F41A8C6; // EncountOutbreakSave_f2_bc_formno[2]
    private const uint KOutbreakBC03DLC2Found          = 0x57C84808; // EncountOutbreakSave_f2_bc_isFind[2]
    private const uint KOutbreakBC03DLC2NumKOed        = 0x6CBD9B8D; // EncountOutbreakSave_f2_bc_subjugationCount[2]
    private const uint KOutbreakBC03DLC2TotalSpawns    = 0xCDB5A7C1; // EncountOutbreakSave_f2_bc_subjugationLimit[2]
    private const uint KOutbreakBC03DLC2Materials      = 0x098D2DA6; // EncountOutbreakSave_f2_bc_dropMaterialCount[2]
    private const uint KOutbreakBC03DLC2DeliveryID     = 0x4E486D46; // EncountOutbreakSave_f2_bc_deliveryId[2]
    private const uint KOutbreakBC03DLC2DeliveryZoneID = 0xD3952E40; // EncountOutbreakSave_f2_bc_deliveryZoneIdx[2]
    private const uint KOutbreakBC03DLC2DeliveryPokeID = 0x192CB1FF; // EncountOutbreakSave_f2_bc_deliveryPokeIdx[2]

    private const uint KOutbreakBC04DLC2CenterPos      = 0xE627C2CD; // EncountOutbreakSave_f2_bc_centerPos[3]
    private const uint KOutbreakBC04DLC2DummyPos       = 0xB1E32576; // EncountOutbreakSave_f2_bc_dummyPos[3]
    private const uint KOutbreakBC04DLC2Species        = 0x03BEA4F0; // EncountOutbreakSave_f2_bc_monsno[3]
    private const uint KOutbreakBC04DLC2Form           = 0x9F3EFF63; // EncountOutbreakSave_f2_bc_formno[3]
    private const uint KOutbreakBC04DLC2Found          = 0x57C465FD; // EncountOutbreakSave_f2_bc_isFind[3]
    private const uint KOutbreakBC04DLC2NumKOed        = 0x6CC110D8; // EncountOutbreakSave_f2_bc_subjugationCount[3]
    private const uint KOutbreakBC04DLC2TotalSpawns    = 0xCDB91D0C; // EncountOutbreakSave_f2_bc_subjugationLimit[3]
    private const uint KOutbreakBC04DLC2Materials      = 0x0989AAC3; // EncountOutbreakSave_f2_bc_dropMaterialCount[3]
    private const uint KOutbreakBC04DLC2DeliveryID     = 0x4E45C3E3; // EncountOutbreakSave_f2_bc_deliveryId[3]
    private const uint KOutbreakBC04DLC2DeliveryZoneID = 0xD391B8F5; // EncountOutbreakSave_f2_bc_deliveryZoneIdx[3]
    private const uint KOutbreakBC04DLC2DeliveryPokeID = 0x1930A1A2; // EncountOutbreakSave_f2_bc_deliveryPokeIdx[3]

    private const uint KOutbreakBC05DLC2CenterPos      = 0xE6194F9A; // EncountOutbreakSave_f2_bc_centerPos[4]
    private const uint KOutbreakBC05DLC2DummyPos       = 0xB1DA6431; // EncountOutbreakSave_f2_bc_dummyPos[4]
    private const uint KOutbreakBC05DLC2Species        = 0x03AA7FCF; // EncountOutbreakSave_f2_bc_monsno[4]
    private const uint KOutbreakBC05DLC2Form           = 0x9F3CC98C; // EncountOutbreakSave_f2_bc_formno[4]
    private const uint KOutbreakBC05DLC2Found          = 0x57B5F2CA; // EncountOutbreakSave_f2_bc_isFind[4]
    private const uint KOutbreakBC05DLC2NumKOed        = 0x6CACEBB7; // EncountOutbreakSave_f2_bc_subjugationCount[4]
    private const uint KOutbreakBC05DLC2TotalSpawns    = 0xCDBB52E3; // EncountOutbreakSave_f2_bc_subjugationLimit[4]
    private const uint KOutbreakBC05DLC2Materials      = 0x098774EC; // EncountOutbreakSave_f2_bc_dropMaterialCount[4]
    private const uint KOutbreakBC05DLC2DeliveryID     = 0x4E438E0C; // EncountOutbreakSave_f2_bc_deliveryId[4]
    private const uint KOutbreakBC05DLC2DeliveryZoneID = 0xD38345C2; // EncountOutbreakSave_f2_bc_deliveryZoneIdx[4]
    private const uint KOutbreakBC05DLC2DeliveryPokeID = 0x193F14D5; // EncountOutbreakSave_f2_bc_deliveryPokeIdx[4]

    private const uint KOutbreakBC06DLC2CenterPos      = 0xE6155FF7; // EncountOutbreakSave_f2_bc_centerPos[5]
    private const uint KOutbreakBC06DLC2DummyPos       = 0xB1DE463C; // EncountOutbreakSave_f2_bc_dummyPos[5]
    private const uint KOutbreakBC06DLC2Species        = 0x03AC4FB2; // EncountOutbreakSave_f2_bc_monsno[5]
    private const uint KOutbreakBC06DLC2Form           = 0x9F395441; // EncountOutbreakSave_f2_bc_formno[5]
    private const uint KOutbreakBC06DLC2Found          = 0x57B422E7; // EncountOutbreakSave_f2_bc_isFind[5]
    private const uint KOutbreakBC06DLC2NumKOed        = 0x6CAF285A; // EncountOutbreakSave_f2_bc_subjugationCount[5]
    private const uint KOutbreakBC06DLC2TotalSpawns    = 0xCDBDFC46; // EncountOutbreakSave_f2_bc_subjugationLimit[5]
    private const uint KOutbreakBC06DLC2Materials      = 0x0984D921; // EncountOutbreakSave_f2_bc_dropMaterialCount[5]
    private const uint KOutbreakBC06DLC2DeliveryID     = 0x4E4018C1; // EncountOutbreakSave_f2_bc_deliveryId[5]
    private const uint KOutbreakBC06DLC2DeliveryZoneID = 0xD381091F; // EncountOutbreakSave_f2_bc_deliveryZoneIdx[5]
    private const uint KOutbreakBC06DLC2DeliveryPokeID = 0x1941B0A0; // EncountOutbreakSave_f2_bc_deliveryPokeIdx[5]

    private const uint KOutbreakBC07DLC2CenterPos      = 0xE61EFABC; // EncountOutbreakSave_f2_bc_centerPos[6]
    private const uint KOutbreakBC07DLC2DummyPos       = 0xB1D4AB77; // EncountOutbreakSave_f2_bc_dummyPos[6]
    private const uint KOutbreakBC07DLC2Species        = 0x03B03889; // EncountOutbreakSave_f2_bc_monsno[6]
    private const uint KOutbreakBC07DLC2Form           = 0x9F371E6A; // EncountOutbreakSave_f2_bc_formno[6]
    private const uint KOutbreakBC07DLC2Found          = 0x57BC776C; // EncountOutbreakSave_f2_bc_isFind[6]
    private const uint KOutbreakBC07DLC2NumKOed        = 0x6CB2A471; // EncountOutbreakSave_f2_bc_subjugationCount[6]
    private const uint KOutbreakBC07DLC2TotalSpawns    = 0xCDC1E51D; // EncountOutbreakSave_f2_bc_subjugationLimit[6]
    private const uint KOutbreakBC07DLC2Materials      = 0x0980F04A; // EncountOutbreakSave_f2_bc_dropMaterialCount[6]
    private const uint KOutbreakBC07DLC2DeliveryID     = 0x4E3DE2EA; // EncountOutbreakSave_f2_bc_deliveryId[6]
    private const uint KOutbreakBC07DLC2DeliveryZoneID = 0xD388F0E4; // EncountOutbreakSave_f2_bc_deliveryZoneIdx[6]
    private const uint KOutbreakBC07DLC2DeliveryPokeID = 0x19395C1B; // EncountOutbreakSave_f2_bc_deliveryPokeIdx[6]

    private const uint KOutbreakBC08DLC2CenterPos      = 0xE61B18B1; // EncountOutbreakSave_f2_bc_centerPos[7]
    private const uint KOutbreakBC08DLC2DummyPos       = 0xB1D89B1A; // EncountOutbreakSave_f2_bc_dummyPos[7]
    private const uint KOutbreakBC08DLC2Species        = 0x03B26794; // EncountOutbreakSave_f2_bc_monsno[7]
    private const uint KOutbreakBC08DLC2Form           = 0x9F347507; // EncountOutbreakSave_f2_bc_formno[7]
    private const uint KOutbreakBC08DLC2Found          = 0x57B9DBA1; // EncountOutbreakSave_f2_bc_isFind[7]
    private const uint KOutbreakBC08DLC2NumKOed        = 0x6CB4D37C; // EncountOutbreakSave_f2_bc_subjugationCount[7]
    private const uint KOutbreakBC08DLC2TotalSpawns    = 0xCDC41428; // EncountOutbreakSave_f2_bc_subjugationLimit[7]
    private const uint KOutbreakBC08DLC2Materials      = 0x097F2067; // EncountOutbreakSave_f2_bc_dropMaterialCount[7]
    private const uint KOutbreakBC08DLC2DeliveryID     = 0x4E3B3987; // EncountOutbreakSave_f2_bc_deliveryId[7]
    private const uint KOutbreakBC08DLC2DeliveryZoneID = 0xD386C1D9; // EncountOutbreakSave_f2_bc_deliveryZoneIdx[7]
    private const uint KOutbreakBC08DLC2DeliveryPokeID = 0x193B2BFE; // EncountOutbreakSave_f2_bc_deliveryPokeIdx[7]

    private const uint KOutbreakBC09DLC2CenterPos      = 0xE63BE7EE; // EncountOutbreakSave_f2_bc_centerPos[8]
    private const uint KOutbreakBC09DLC2DummyPos       = 0xB1FDD605; // EncountOutbreakSave_f2_bc_dummyPos[8]
    private const uint KOutbreakBC09DLC2Species        = 0x039DD5B3; // EncountOutbreakSave_f2_bc_monsno[8]
    private const uint KOutbreakBC09DLC2Form           = 0x9F5E8860; // EncountOutbreakSave_f2_bc_formno[8]
    private const uint KOutbreakBC09DLC2Found          = 0x57D9649E; // EncountOutbreakSave_f2_bc_isFind[8]
    private const uint KOutbreakBC09DLC2NumKOed        = 0x6CCF840B; // EncountOutbreakSave_f2_bc_subjugationCount[8]
    private const uint KOutbreakBC09DLC2TotalSpawns    = 0xCDC7903F; // EncountOutbreakSave_f2_bc_subjugationLimit[8]
    private const uint KOutbreakBC09DLC2Materials      = 0x09AA0D40; // EncountOutbreakSave_f2_bc_dropMaterialCount[8]
    private const uint KOutbreakBC09DLC2DeliveryID     = 0x4E654CE0; // EncountOutbreakSave_f2_bc_deliveryId[8]
    private const uint KOutbreakBC09DLC2DeliveryZoneID = 0xD3784EA6; // EncountOutbreakSave_f2_bc_deliveryZoneIdx[8]
    private const uint KOutbreakBC09DLC2DeliveryPokeID = 0x191C7C81; // EncountOutbreakSave_f2_bc_deliveryPokeIdx[8]

    private const uint KOutbreakBC10DLC2CenterPos      = 0xE637F84B; // EncountOutbreakSave_f2_bc_centerPos[9]
    private const uint KOutbreakBC10DLC2DummyPos       = 0xB2000510; // EncountOutbreakSave_f2_bc_dummyPos[9]
    private const uint KOutbreakBC10DLC2Species        = 0x03A1C556; // EncountOutbreakSave_f2_bc_monsno[9]
    private const uint KOutbreakBC10DLC2Form           = 0x9F5BEC95; // EncountOutbreakSave_f2_bc_formno[9]
    private const uint KOutbreakBC10DLC2Found          = 0x57D6BB3B; // EncountOutbreakSave_f2_bc_isFind[9]
    private const uint KOutbreakBC10DLC2NumKOed        = 0x6CD1C0AE; // EncountOutbreakSave_f2_bc_subjugationCount[9]
    private const uint KOutbreakBC10DLC2TotalSpawns    = 0xCDC9CCE2; // EncountOutbreakSave_f2_bc_subjugationLimit[9]
    private const uint KOutbreakBC10DLC2Materials      = 0x09A697F5; // EncountOutbreakSave_f2_bc_dropMaterialCount[9]
    private const uint KOutbreakBC10DLC2DeliveryID     = 0x4E62B115; // EncountOutbreakSave_f2_bc_deliveryId[9]
    private const uint KOutbreakBC10DLC2DeliveryZoneID = 0xD374CBC3; // EncountOutbreakSave_f2_bc_deliveryZoneIdx[9]
    private const uint KOutbreakBC10DLC2DeliveryPokeID = 0x191E3ECC; // EncountOutbreakSave_f2_bc_deliveryPokeIdx[9]
    #endregion

    #region GameEnvSave
    private const uint KAreaNameALO00 = 0xE2FBC051; // GameEnvSave_ALO_AreaName[0]
    private const uint KAreaNameALO01 = 0xE2FDEF5C; // GameEnvSave_ALO_AreaName[1]
    private const uint KAreaNameALO02 = 0xE2F52E17; // GameEnvSave_ALO_AreaName[2]
    private const uint KAreaNameALO03 = 0xE2F91DBA; // GameEnvSave_ALO_AreaName[3]
    private const uint KAreaNameALO04 = 0xE30790ED; // GameEnvSave_ALO_AreaName[4]
    private const uint KAreaNameALO05 = 0xE30A2CB8; // GameEnvSave_ALO_AreaName[5]
    private const uint KAreaNameALO06 = 0xE3016B73; // GameEnvSave_ALO_AreaName[6]
    private const uint KAreaNameALO07 = 0xE303A816; // GameEnvSave_ALO_AreaName[7]
    private const uint KAreaNameALO08 = 0xE3121B49; // GameEnvSave_ALO_AreaName[8]
    private const uint KAreaNameALO09 = 0xE315FD54; // GameEnvSave_ALO_AreaName[9]
    private const uint KFixedGameTime = 0x26E69AA9; // GameEnvSave_FixedGameTime
    private const uint KFixedWeather = 0x329C9262; // GameEnvSave_FixedWeather
    #endregion

    #region Fixed Symbol Encounter Storage
    // Each struct is 0x170: u64 hash, bool active, 0x158 bytes pkm, 7 bytes alignment, u32 unknown, 4 bytes alignment
    // Game fills up each block in this order, and checks them in this order when loading up previously-encountered fixed spawns.
    private const uint KFixedSymbolRetainer01 = 0x74ABBD32;
    private const uint KFixedSymbolRetainer02 = 0x74ABBEE5;
    private const uint KFixedSymbolRetainer03 = 0x74ABB9CC;
    private const uint KFixedSymbolRetainer04 = 0x74ABBB7F;
    private const uint KFixedSymbolRetainer05 = 0x74ABB666;
    private const uint KFixedSymbolRetainer06 = 0x74ABB819;
    private const uint KFixedSymbolRetainer07 = 0x74ABB300;
    private const uint KFixedSymbolRetainer08 = 0x74ABB4B3;
    private const uint KFixedSymbolRetainer09 = 0x74ABCACA;
    private const uint KFixedSymbolRetainer10 = 0x74ABCC7D;
    #endregion

    #region Sudachi 1
    private const uint KGameClearTealMask = 0x0DDBBC62; // FSYS_SCENARIO_GAME_CLEAR_SU1
    private const uint KUnlockedPokedexKitakami = 0x4877DB86; // FSYS_DLC1_POKEDEX_ADD
    private const uint KCanClaimPokedexDiplomaKitakami = 0xC4E1A713; // FSYS_DLC1_POKEDEX_SYOUJOU_ENABLE
    private const uint KClaimedPokedexDiplomaKitakami = 0xA066600C; // FSYS_DLC1_POKEDEX_SYOUJOU_EVENT
    private const uint KUnlockedRotoStick = 0x478A8C60; // FSYS_DLC1_SELFIE_STICK_UNLOCK
    private const uint KUnlockedDLCEmote0 = 0x99849BBF; // FSYS_DLC1_EMOTE_00_RELEASE
    private const uint KUnlockedDLCEmote1 = 0xF6A77854; // FSYS_DLC1_EMOTE_01_RELEASE
    private const uint KUnlockedDLCEmote2 = 0xC2ECD3E5; // FSYS_DLC1_EMOTE_02_RELEASE
    private const uint KUnlockedDLCEmote3 = 0x29583572; // FSYS_DLC1_EMOTE_03_RELEASE
    private const uint KUnlockedDLCEmote4 = 0x298A7CFB; // FSYS_DLC1_EMOTE_04_RELEASE
    private const uint KUnlockedDLCEmote5 = 0x3DA607F0; // FSYS_DLC1_EMOTE_05_RELEASE
    private const uint KUnlockedDLCSelfieEmote1 = 0x1170E5C8; // FSYS_DLC1_EMOTE_SELFIE_00_RELEASE
    private const uint KUnlockedDLCSelfieEmote2 = 0xBBBA5D73; // FSYS_DLC1_EMOTE_SELFIE_01_RELEASE
    private const uint KUnlockedDLCSelfieEmote3 = 0xA3EB52A6; // FSYS_DLC1_EMOTE_SELFIE_02_RELEASE
    private const uint KLastPokedexVolumeRewardThresholdKitakami = 0xCD5D70B6; // WSYS_S1_POKEDX_REWARD_CHIHOUA_VALUE
    private const uint KUnlockedTMMachineMoveFiltering = 0x3CC63057; // FSYS_SU1_SHOPWAZAMACHINE_MESSAGE
    private const uint KCompletedBillyONareQuestPart1 = 0xCB190E5E; // FEVT_S1_SUB_003_TALKED
    private const uint KCompletedBillyONareQuestPart2 = 0x6BBE8497; // FEVT_S1_SUB_006_TALKED
    private const uint KCompletedBillyONareQuestPart3 = 0xDB267000; // FEVT_S1_SUB_009_TALKED
    private const uint KLoyaltyPlazaFundraiserStarted = 0x57A38A70; // FEVT_S1_SUB_015_FIRST_TALKED
    private const uint KLoyaltyPlazaFundraiserDonations = 0xBB9076CA; // WEVT_S1_SUB_015_UNITS_HELD
    private const uint KCurrentMenuBorderDesign = 0x56D810B6; // 0 = match location, 1 = force Paldea

    private const uint KIndexDefeatedLoyalThreeOkidogi = 0x919437A0; // WEVT_S1_INU_CLEAR_NUM
    private const uint KIndexDefeatedLoyalThreeMunkidori = 0x657672CD; // WEVT_S1_SARU_CLEAR_NUM
    private const uint KIndexDefeatedLoyalThreeFezandipiti = 0x9EF45981; // WEVT_S1_KIZI_CLEAR_NUM

    private const uint KCapturedOkidogi = 0x7042479E; // FEVT_S1_SUB_011_CAPTURED
    private const uint KCapturedMunkidori = 0x9F5556DD; // FEVT_S1_SUB_012_CAPTURED
    private const uint KCapturedFezandipiti = 0xFF7CAD99; // FEVT_S1_SUB_016_CAPTURED
    private const uint KCanChallengeOgreClanLeader = 0x18ABCD92; // FEVT_S1_SUB_017_BATTLE_ENABLE
    private const uint KOgreClanReward1 = 0x19B5A525; // FEVT_S1_SUB_017_REWARD_01
    private const uint KOgreClanReward2 = 0x19B5A00C; // FEVT_S1_SUB_017_REWARD_02
    private const uint KOgreClanReward3 = 0x19B5A1BF; // FEVT_S1_SUB_017_REWARD_03
    private const uint KOgreClanReward4 = 0x19B59CA6; // FEVT_S1_SUB_017_REWARD_04
    private const uint KOgreClanReward5 = 0x19B59E59; // FEVT_S1_SUB_017_REWARD_05
    private const uint KOgreClanReward6 = 0x19B59940; // FEVT_S1_SUB_017_REWARD_06
    private const uint KOgreClanReward7 = 0x19B59AF3; // FEVT_S1_SUB_017_REWARD_07

    private const uint KOgreOustinClearedNormal = 0xA5596DD0; // OniballoonSave_isCleardNormal
    private const uint KOgreOustinClearedHard = 0x17AD6C82; // OniballoonSave_isCleardHard
    private const uint KOgreOustinUnlockedNormal = 0xFDC0C3C6; // OniballoonSave_isReleaseNormal
    private const uint KOgreOustinUnlockedHard = 0x2F009BE8; // OniballoonSave_isReleaseHard
    private const uint KOgreOustinCanReceiveRewardEasy = 0xCE0977F2; // OniballoonSave_isCanReceiveRewardEasy
    private const uint KOgreOustinCanReceiveRewardNormal = 0x84E725C7; // OniballoonSave_isCanReceiveRewardNormal
    private const uint KOgreOustinCanReceiveRewardHard = 0x176CD6C9; // OniballoonSave_isCanReceiveRewardHard
    private const uint KOgreOustinHighScoreEasy = 0x52F7FB36; // OniballoonSave_bestScoreEasy
    private const uint KOgreOustinHighScoreNormal = 0xC887C363; // OniballoonSave_bestScoreNormal
    private const uint KOgreOustinHighScoreHard = 0x24AD643D; // OniballoonSave_bestScoreHard
    private const uint KOgreOustinPlayedEasy = 0xB5757519; // OniballoonSave_isPlayedEasy
    private const uint KOgreOustinPlayedNormal = 0x299B1DB8; // OniballoonSave_isPlayedNormal
    private const uint KOgreOustinPlayedHard = 0xAB32F81A; // OniballoonSave_isPlayedHard
    private const uint KOgreOustinCompletedMessage = 0x838484DB; // OniballoonSave_isPlayedCompleteMessage

    private const uint KTimelessWoodsSurveyIndexFoundPokemon01 = 0x45C5F800; // WEVT_S1_SIDE02_0040_POKE01
    private const uint KTimelessWoodsSurveyIndexFoundPokemon02 = 0x45C5FD19; // WEVT_S1_SIDE02_0040_POKE02
    private const uint KTimelessWoodsSurveyIndexFoundPokemon03 = 0x45C5FB66; // WEVT_S1_SIDE02_0040_POKE03
    private const uint KTimelessWoodsSurveyIndexFoundPokemon04 = 0x45C6007F; // WEVT_S1_SIDE02_0040_POKE04
    private const uint KTimelessWoodsSurveyIndexFoundPokemon05 = 0x45C5FECC; // WEVT_S1_SIDE02_0040_POKE05
    private const uint KTimelessWoodsSurveyIndexFoundPokemon06 = 0x45C603E5; // WEVT_S1_SIDE02_0040_POKE06
    private const uint KTimelessWoodsSurveyIndexFoundPokemon07 = 0x45C60232; // WEVT_S1_SIDE02_0040_POKE07
    private const uint KTimelessWoodsSurveyIndexFoundPokemon08 = 0x45C6074B; // WEVT_S1_SIDE02_0040_POKE08
    private const uint KTimelessWoodsSurveyIndexFoundPokemon09 = 0x45C60598; // WEVT_S1_SIDE02_0040_POKE09
    private const uint KTimelessWoodsSurveyIndexFoundPokemon10 = 0x45C3168A; // WEVT_S1_SIDE02_0040_POKE10
    private const uint KTimelessWoodsSurveyIndexFoundPokemon11 = 0x45C3183D; // WEVT_S1_SIDE02_0040_POKE11
    private const uint KTimelessWoodsSurveyIndexFoundPokemon12 = 0x45C31324; // WEVT_S1_SIDE02_0040_POKE12
    private const uint KTimelessWoodsSurveyIndexFoundPokemon13 = 0x45C314D7; // WEVT_S1_SIDE02_0040_POKE13
    private const uint KTimelessWoodsSurveyIndexFoundPokemon14 = 0x45C30FBE; // WEVT_S1_SIDE02_0040_POKE14
    private const uint KTimelessWoodsSurveyIndexFoundPokemon15 = 0x45C31171; // WEVT_S1_SIDE02_0040_POKE15

    private const uint FEVT_S1_SIDE02_0037_FIRST_TALKED = 0x0ACD73FD;
    private const uint FEVT_S1_SUB_013_TALKED_CHAIR_NPC = 0xBF14EBB7;
    private const uint FEVT_S1_SUB_013_TALKED_REST_NPC = 0x59F64244;
    private const uint FEVT_S1_SUB_014_CHECK = 0xD9C569E3;
    private const uint WEVT_S1_SIDE02_0040_TIP_NUM = 0x820588A0;
    private const uint WEVT_S1_SUB_017_WIN_NUM = 0x15F0F3D4;
    private const uint WSYS_S1_EMOTE_NEW_FLAG = 0x49475505;
    private const uint WSYS_S1_EMOTE_SELFIE_NEW_FLAG = 0xBE35FE80;
    private const uint WSYS_S1_GOZONJI_HINT_COUNT = 0x58E1035D;
    private const uint WSYS_S1_POKECEN_KAIFUKU_COUNT = 0x5CC43913;
    private const uint WSYS_S1_SYOUTEN_LP_FLAG_COUNT = 0x3ECD61A0;
    #endregion
}
