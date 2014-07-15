// -----------------------------------------------------------------------
// <copyright file="RaidInfo.cs" company="Vangent, Inc.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace WoWGuildOrganizer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RaidInfo
    {
        #region " Raid Loot - Tier 14 (), Tier 15 (), Tier 16 (Siege of Orgrimmar) "

        /// <summary>
        /// Fill out the raid boss loot data here
        ///   Evenually will need to be able to Add this all in programmatically.
        ///   But for now, I will hard code in what we need, and get the ranking functionality in place to use.
        /// </summary>
        public void CreateRaidLootData(ref Dictionary<string, Dictionary<string, int[]>> RaidLoot)
        {
            Dictionary<string, int[]> tempLoot = new Dictionary<string, int[]>();
            string RaidName = string.Empty;
            string RaidBoss = string.Empty;
            int[] BossLoot;

            // 1st Tier 14 Raid
            RaidName = "Mogu'shan Vaults - 10N";

            RaidBoss = "The Stone Guard";
            BossLoot = new int[] { 85922, 85979, 89768, 85924, 85975, 85978, 85925, 89767, 85976, 86134, 85977, 89766, 85926, 85923 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Feng the Accursed";
            BossLoot = new int[] { 85986, 86082, 85983, 85987, 85985, 89424, 89803, 89802, 85989, 85990, 85988, 85984, 85982, 85980 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Gara'jal the Spiritbinder";
            BossLoot = new int[] { 86027, 89817, 86038, 85996, 85993, 85994, 86040, 85995, 85997, 86041, 85992, 85991, 86039 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "The Spirit Kings";
            BossLoot = new int[] { 86047, 86081, 86076, 86071, 86075, 89818, 86080, 86127, 86086, 86129, 86084, 89819, 86128, 86083 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Elegon";
            BossLoot = new int[] { 86133, 86140, 86132, 89822, 86139, 86137, 86136, 86131, 86135, 86141, 86138, 89821, 86130, 89824 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Will of the Emperor";
            BossLoot = new int[] { 86144, 86145, 86146, 87827, 89823, 86142, 89820, 89825, 86151, 86150, 86147, 86148, 86149, 86152 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidLoot.Add(RaidName, tempLoot);

            // 2nd Tier 14 Raid - Hearts of Fire            
            RaidName = "Heart of Fear - 10N";
            tempLoot = new Dictionary<string, int[]>();

            RaidBoss = "Imperial Vizier Zor'lok";
            BossLoot = new int[] { 86156, 89827, 86157, 89829, 86154, 86153, 89826, 87824, 86158, 86159, 86161, 86160, 86203, 86155 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidLoot.Add(RaidName, tempLoot);

            // Tier 15 Raid
            RaidName = "Throne of Thunder - 10N";
            tempLoot = new Dictionary<string, int[]>();

            RaidBoss = "Jin'rokh the Breaker";
            BossLoot = new int[] { 94738, 95510, 94512, 94739, 94726, 94723, 94735, 94733, 94724, 94737, 94730, 94725, 94728, 94722, 94736, 94732, 94734, 94729, 94731, 94727, 95064, 97126, 95066, 95503, 95063, 95069, 95065, 95500, 95504, 95498 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Horridon";
            BossLoot = new int[] { 94754, 95514, 94526, 94514, 94747, 94751, 94742, 94745, 94744, 95063, 95502, 95498, 95505, 95499, 95500, 95068, 95069, 95516 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidLoot.Add(RaidName, tempLoot);

            // Tier 16 Raid - LFR
            RaidName = "Siege of Orgrimmar - LFR";
            tempLoot = new Dictionary<string, int[]>();

            // Wing 1
            RaidBoss = "Immerseus";
            BossLoot = new int[] { 104920, 104927, 104917, 104913, 104914, 104923, 104915, 104919, 104929, 104911, 104922, 104921, 104909, 104918, 104912, 104924, 104926, 104925, 104928, 104916, 104910, 104930 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "The Fallen Protectors";
            BossLoot = new int[] { 104936, 104931, 104951, 104939, 104950, 104934, 104944, 104945, 104935, 104946, 104942, 104940, 104948, 104941, 104937, 104949, 104943, 104947, 104932, 104938, 104933 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Norushen";
            BossLoot = new int[] { 104964, 104969, 104958, 104963, 104971, 104970, 104960, 104961, 104955, 104956, 104968, 104952, 104957, 104959, 104953, 104966, 104954, 104965, 104972, 104967, 104973, 104962 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Sha of Pride";
            BossLoot = new int[] { 104974, 104982, 104979, 104977, 104981, 104980, 104975, 104976, 104978, 104983 };  // 99678, 99679, 99677 - Tier 16 Chest Tokens
            tempLoot.Add(RaidBoss, BossLoot);

            // Wing 2
            RaidBoss = "Galakras";
            BossLoot = new int[] { 104991, 104995, 104988, 104984, 104989, 105002, 105001, 104993, 105000, 104997, 104994, 105003, 104987, 104992, 104996, 104999, 104998, 105004, 104985, 104990, 104986, 105005 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Iron Juggernaut";
            BossLoot = new int[] { 105017, 105027, 105019, 105024, 105026, 105011, 105014, 105020, 105016, 105015, 105023, 105007, 105022, 105018, 105009, 105010, 105008, 105006, 105021, 105013, 105025, 105012 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Kor'kron Dark Shaman";
            BossLoot = new int[] { 105035, 105041, 105045, 105036, 105034, 105030, 105044, 105037, 105032, 105029, 105040, 105043, 105042, 105028, 105038, 105031, 105047, 105046, 105048, 105039, 105033 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "General Nazgrim";
            BossLoot = new int[] { 105052, 105058, 105056, 105057, 105051, 105049, 105055, 105054, 105050, 105053, 105059 };  // 99681, 99667, 99680 - Tier 16 Hand Tokens
            tempLoot.Add(RaidBoss, BossLoot);

            // Wing 3
            RaidBoss = "Malkorok";
            BossLoot = new int[] { 105075, 105066, 105078, 105079, 105080, 105074, 105062, 105072, 105061, 105063, 105067, 105065, 105069, 105068, 105071, 105060, 105073, 105076, 105081, 105070, 105077, 105064 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Spoils of Pandaria";
            BossLoot = new int[] { 105087, 105092, 105086, 105093, 105100, 105099, 105083, 105088, 105096, 105097, 105095, 105085, 105094, 105102, 105090, 105084, 105101, 105091, 105098, 105082, 105089 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Thok the Bloodthirsty";
            BossLoot = new int[] { 105106, 105112, 105113, 105107, 105104, 105103, 105110, 105105, 105108, 105109, 105111 };  // 99672,99673,99671 - Tier 16 Head Tokens
            tempLoot.Add(RaidBoss, BossLoot);

            // Wing 4
            RaidBoss = "Siegecrafter Blackfuse";
            BossLoot = new int[] { 105122, 105124, 105118, 105119, 105121, 105117, 105115, 105116, 105120, 105123, 105114 };  // 99669,99670,99668 - Tier 16 Shoulder Tokens
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Paragons of the Klaxxi";
            BossLoot = new int[] { 105131, 105128, 105132, 105133, 105125, 105130, 105126, 105135, 105127, 105129, 105134 };  // 99675,99676,99674 - Tier 16 Legs Tokens
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Garrosh Hellscream";
            BossLoot = new int[] { 105148, 105150, 105139, 105156, 105137, 105155, 105147, 105149, 105145, 105154, 105151, 105138, 105136, 105142, 105157, 105140, 105152, 105153, 105141, 105143, 105146 };  // 105860,105861,105862 - Tier 16 All Token
            tempLoot.Add(RaidBoss, BossLoot);

            RaidLoot.Add(RaidName, tempLoot);

            // Tier 16 Raid - Flex
            RaidName = "Siege of Orgrimmar - Flex";
            tempLoot = new Dictionary<string, int[]>();

            // Wing 1
            RaidBoss = "Immerseus";
            BossLoot = new int[] { 104671, 104678, 104688, 104664, 104665, 104674, 104666, 104670, 104680, 104662, 104673, 104672, 104660, 104669, 104663, 104675, 104677, 104676, 104679, 104667, 104661, 104681 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "The Fallen Protectors";
            BossLoot = new int[] { 104687, 104682, 104702, 104690, 104701, 104685, 104695, 104696, 104686, 104697, 104693, 104691, 104699, 104692, 104688, 104700, 104694, 104698, 104683, 104689, 104684 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Norushen";
            BossLoot = new int[] { 104715, 104720, 104709, 104714, 104722, 104721, 104711, 104712, 104706, 104707, 104719, 104703, 104708, 104710, 104704, 104717, 104705, 104716, 104723, 104718, 104724, 104713 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Sha of Pride";
            BossLoot = new int[] { 104725, 99743, 99744, 99742, 104733, 104730, 104728, 104732, 104731, 104726, 104727, 104729, 104734 };
            tempLoot.Add(RaidBoss, BossLoot);

            // Wing 2
            RaidBoss = "Galakras";
            BossLoot = new int[] { 104742, 104746, 104739, 104735, 104740, 104753, 104752, 104744, 104751, 104748, 104745, 104754, 104738, 104743, 104747, 104750, 104749, 104755, 104736, 104741, 104737, 104756 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Iron Juggernaut";
            BossLoot = new int[] { 104768, 104778, 104770, 104775, 104777, 104762, 104765, 104771, 104767, 104766, 104774, 104758, 104773, 104769, 104760, 104761, 104759, 104757, 104772, 104764, 104776, 104763 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Kor'kron Dark Shaman";
            BossLoot = new int[] { 104786, 104792, 104796, 104787, 104785, 104781, 104795, 104788, 104783, 104780, 104791, 104794, 104793, 104779, 104789, 104782, 104798, 104797, 104799, 104790, 104784 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "General Nazgrim";
            BossLoot = new int[] { 104803, 104809, 104807, 104808, 99746, 99747, 99745, 104802, 104800, 104806, 104805, 104801, 104804, 104810 };
            tempLoot.Add(RaidBoss, BossLoot);

            // Wing 3
            RaidBoss = "Malkorok";
            BossLoot = new int[] { 104826, 104817, 104829, 104830, 104831, 104825, 104813, 104823, 104812, 104814, 104818, 104816, 104820, 104819, 104822, 104811, 104824, 104827, 104832, 104821, 104828, 104815 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Spoils of Pandaria";
            BossLoot = new int[] { 104838, 104843, 104837, 104844, 104851, 104850, 104834, 104839, 104847, 104848, 104846, 104836, 104845, 104853, 104841, 104835, 104852, 104842, 104849, 104833, 104840 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Thok the Bloodthirsty";
            BossLoot = new int[] { 104857, 104863, 104864, 104858, 104855, 99749, 99750, 99748, 104854, 104861, 104856, 104859, 104860, 104862 };
            tempLoot.Add(RaidBoss, BossLoot);

            // Wing 4
            RaidBoss = "Siegecrafter Blackfuse";
            BossLoot = new int[] { 104873, 104875, 104869, 104870, 104872, 104868, 104866, 104867, 104871, 99755, 99756, 99754, 104874, 104865 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Paragons of the Klaxxi";
            BossLoot = new int[] { 104882, 104879, 104883, 104884, 104876, 104881, 104877, 104886, 104878, 99752, 99753, 99751, 104880, 104885 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Garrosh Hellscream";
            BossLoot = new int[] { 104899, 104901, 104890, 104907, 104888, 104906, 104898, 104900, 105864, 105863, 105865, 104896, 104905, 104902, 104889, 104887, 104893, 104908, 104891, 104903, 104904, 104892, 104894, 104897, 105674, 105672, 105679, 105678, 105673, 105671, 105680, 105676, 105677, 105670, 105675 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidLoot.Add(RaidName, tempLoot);

            // Tier 16 Raid - Flex
            RaidName = "Siege of Orgrimmar - Normal";
            tempLoot = new Dictionary<string, int[]>();

            // Wing 1
            RaidBoss = "Immerseus";
            BossLoot = new int[] { 103769, 102293, 103728, 103749, 103771, 103736, 103730, 103726, 103738, 103751, 103733, 103755, 103752, 103757, 103741, 103727, 103747, 103760, 103744, 103766, 103763, 103966 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "The Fallen Protectors";
            BossLoot = new int[] { 103783, 103776, 103820, 103799, 103817, 103780, 103809, 103822, 103787, 103801, 103802, 102296, 103812, 103804, 103790, 103815, 103807, 103924, 103775, 103793, 103777 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Norushen";
            BossLoot = new int[] { 103867, 102295, 103841, 103857, 103852, 103740, 103849, 103830, 103845, 103838, 103847, 103834, 103836, 103942, 103762, 103861, 103858, 103827, 103855, 103864, 103826, 103839 };
            tempLoot.Add(RaidBoss, BossLoot);

            RaidBoss = "Sha of Pride";
            BossLoot = new int[] { 99691, 99696, 99686, 103869, 103873, 102292, 103878, 103881, 102299, 103883, 103870, 103821, 103876 };
            tempLoot.Add(RaidBoss, BossLoot);

            // todo:
            // Wing 2 - todo: 
            // Wing 3 - todo: 
            // Wing 4 - todo: 

            RaidLoot.Add(RaidName, tempLoot);
        }

        #endregion
    }
}
