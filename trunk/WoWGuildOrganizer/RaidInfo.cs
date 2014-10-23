// -----------------------------------------------------------------------
// <copyright file="RaidInfo.cs" company="Vangent, Inc.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace WoWGuildOrganizer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Contains all the raid loot and the drop results from each of the tiers:
    /// <c>Tier 14 - "Mogu'shan Vaults"
    /// Tier 15 - ?
    /// Tier 16 - "Siege of Orgrimmar"</c>
    /// </summary>
    public class RaidInfo
    {
        #region " Raid Loot - Tier 14 (), Tier 15 (), Tier 16 (Siege of Orgrimmar) "

        /// <summary>
        /// Fill out the raid boss loot data here
        ///   Eventually will need to be able to Add this all in programmatically.
        ///   But for now, I will hard code in what we need, and get the ranking functionality in place to use.        
        /// </summary>
        /// <param name="raidLoot">Dictionary of the loot, keyed by Raid Name</param>
        public void CreateRaidLootData(ref Dictionary<string, Dictionary<string, int[]>> raidLoot)
        {
            Dictionary<string, int[]> tempLoot = new Dictionary<string, int[]>();
            string raidName = string.Empty;
            string raidBoss = string.Empty;
            int[] bossLoot;

            // 1st Tier 14 Raid
            raidName = "Mogu'shan Vaults - 10N";

            raidBoss = "The Stone Guard";
            bossLoot = new int[] { 85922, 85979, 89768, 85924, 85975, 85978, 85925, 89767, 85976, 86134, 85977, 89766, 85926, 85923 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "Feng the Accursed";
            bossLoot = new int[] { 85986, 86082, 85983, 85987, 85985, 89424, 89803, 89802, 85989, 85990, 85988, 85984, 85982, 85980 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "Gara'jal the Spiritbinder";
            bossLoot = new int[] { 86027, 89817, 86038, 85996, 85993, 85994, 86040, 85995, 85997, 86041, 85992, 85991, 86039 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "The Spirit Kings";
            bossLoot = new int[] { 86047, 86081, 86076, 86071, 86075, 89818, 86080, 86127, 86086, 86129, 86084, 89819, 86128, 86083 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "Elegon";
            bossLoot = new int[] { 86133, 86140, 86132, 89822, 86139, 86137, 86136, 86131, 86135, 86141, 86138, 89821, 86130, 89824 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "Will of the Emperor";
            bossLoot = new int[] { 86144, 86145, 86146, 87827, 89823, 86142, 89820, 89825, 86151, 86150, 86147, 86148, 86149, 86152 };
            tempLoot.Add(raidBoss, bossLoot);

            raidLoot.Add(raidName, tempLoot);

            // 2nd Tier 14 Raid - Hearts of Fire            
            raidName = "Heart of Fear - 10N";
            tempLoot = new Dictionary<string, int[]>();

            raidBoss = "Imperial Vizier Zor'lok";
            bossLoot = new int[] { 86156, 89827, 86157, 89829, 86154, 86153, 89826, 87824, 86158, 86159, 86161, 86160, 86203, 86155 };
            tempLoot.Add(raidBoss, bossLoot);

            raidLoot.Add(raidName, tempLoot);

            // Tier 15 Raid
            raidName = "Throne of Thunder - 10N";
            tempLoot = new Dictionary<string, int[]>();

            raidBoss = "Jin'rokh the Breaker";
            bossLoot = new int[] { 94738, 95510, 94512, 94739, 94726, 94723, 94735, 94733, 94724, 94737, 94730, 94725, 94728, 94722, 94736, 94732, 94734, 94729, 94731, 94727, 95064, 97126, 95066, 95503, 95063, 95069, 95065, 95500, 95504, 95498 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "Horridon";
            bossLoot = new int[] { 94754, 95514, 94526, 94514, 94747, 94751, 94742, 94745, 94744, 95063, 95502, 95498, 95505, 95499, 95500, 95068, 95069, 95516 };
            tempLoot.Add(raidBoss, bossLoot);

            raidLoot.Add(raidName, tempLoot);

            // Tier 16 Raid - LFR
            raidName = "Siege of Orgrimmar - LFR";
            tempLoot = new Dictionary<string, int[]>();

            // Wing 1
            raidBoss = "Immerseus";
            bossLoot = new int[] { 104920, 104927, 104917, 104913, 104914, 104923, 104915, 104919, 104929, 104911, 104922, 104921, 104909, 104918, 104912, 104924, 104926, 104925, 104928, 104916, 104910, 104930 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "The Fallen Protectors";
            bossLoot = new int[] { 104936, 104931, 104951, 104939, 104950, 104934, 104944, 104945, 104935, 104946, 104942, 104940, 104948, 104941, 104937, 104949, 104943, 104947, 104932, 104938, 104933 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "Norushen";
            bossLoot = new int[] { 104964, 104969, 104958, 104963, 104971, 104970, 104960, 104961, 104955, 104956, 104968, 104952, 104957, 104959, 104953, 104966, 104954, 104965, 104972, 104967, 104973, 104962 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "Sha of Pride";
            bossLoot = new int[] { 104974, 104982, 104979, 104977, 104981, 104980, 104975, 104976, 104978, 104983 };  // 99678, 99679, 99677 - Tier 16 Chest Tokens
            tempLoot.Add(raidBoss, bossLoot);

            // Wing 2
            raidBoss = "Galakras";
            bossLoot = new int[] { 104991, 104995, 104988, 104984, 104989, 105002, 105001, 104993, 105000, 104997, 104994, 105003, 104987, 104992, 104996, 104999, 104998, 105004, 104985, 104990, 104986, 105005 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "Iron Juggernaut";
            bossLoot = new int[] { 105017, 105027, 105019, 105024, 105026, 105011, 105014, 105020, 105016, 105015, 105023, 105007, 105022, 105018, 105009, 105010, 105008, 105006, 105021, 105013, 105025, 105012 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "Kor'kron Dark Shaman";
            bossLoot = new int[] { 105035, 105041, 105045, 105036, 105034, 105030, 105044, 105037, 105032, 105029, 105040, 105043, 105042, 105028, 105038, 105031, 105047, 105046, 105048, 105039, 105033 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "General Nazgrim";
            bossLoot = new int[] { 105052, 105058, 105056, 105057, 105051, 105049, 105055, 105054, 105050, 105053, 105059 };  // 99681, 99667, 99680 - Tier 16 Hand Tokens
            tempLoot.Add(raidBoss, bossLoot);

            // Wing 3
            raidBoss = "Malkorok";
            bossLoot = new int[] { 105075, 105066, 105078, 105079, 105080, 105074, 105062, 105072, 105061, 105063, 105067, 105065, 105069, 105068, 105071, 105060, 105073, 105076, 105081, 105070, 105077, 105064 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "Spoils of Pandaria";
            bossLoot = new int[] { 105087, 105092, 105086, 105093, 105100, 105099, 105083, 105088, 105096, 105097, 105095, 105085, 105094, 105102, 105090, 105084, 105101, 105091, 105098, 105082, 105089 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "Thok the Bloodthirsty";
            bossLoot = new int[] { 105106, 105112, 105113, 105107, 105104, 105103, 105110, 105105, 105108, 105109, 105111 };  // 99672,99673,99671 - Tier 16 Head Tokens
            tempLoot.Add(raidBoss, bossLoot);

            // Wing 4
            raidBoss = "Siegecrafter Blackfuse";
            bossLoot = new int[] { 105122, 105124, 105118, 105119, 105121, 105117, 105115, 105116, 105120, 105123, 105114 };  // 99669,99670,99668 - Tier 16 Shoulder Tokens
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "Paragons of the Klaxxi";
            bossLoot = new int[] { 105131, 105128, 105132, 105133, 105125, 105130, 105126, 105135, 105127, 105129, 105134 };  // 99675,99676,99674 - Tier 16 Legs Tokens
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "Garrosh Hellscream";
            bossLoot = new int[] { 105148, 105150, 105139, 105156, 105137, 105155, 105147, 105149, 105145, 105154, 105151, 105138, 105136, 105142, 105157, 105140, 105152, 105153, 105141, 105143, 105146 };  // 105860,105861,105862 - Tier 16 All Token
            tempLoot.Add(raidBoss, bossLoot);

            raidLoot.Add(raidName, tempLoot);

            // Tier 16 Raid - Flex
            raidName = "Siege of Orgrimmar - Normal";
            tempLoot = new Dictionary<string, int[]>();

            // Wing 1
            raidBoss = "Immerseus";
            bossLoot = new int[] { 104671, 104678, 104688, 104664, 104665, 104674, 104666, 104670, 104680, 104662, 104673, 104672, 104660, 104669, 104663, 104675, 104677, 104676, 104679, 104667, 104661, 104681 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "The Fallen Protectors";
            bossLoot = new int[] { 104687, 104682, 104702, 104690, 104701, 104685, 104695, 104696, 104686, 104697, 104693, 104691, 104699, 104692, 104688, 104700, 104694, 104698, 104683, 104689, 104684 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "Norushen";
            bossLoot = new int[] { 104715, 104720, 104709, 104714, 104722, 104721, 104711, 104712, 104706, 104707, 104719, 104703, 104708, 104710, 104704, 104717, 104705, 104716, 104723, 104718, 104724, 104713 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "Sha of Pride";
            bossLoot = new int[] { 104725, 99743, 99744, 99742, 104733, 104730, 104728, 104732, 104731, 104726, 104727, 104729, 104734 };
            tempLoot.Add(raidBoss, bossLoot);

            // Wing 2
            raidBoss = "Galakras";
            bossLoot = new int[] { 104742, 104746, 104739, 104735, 104740, 104753, 104752, 104744, 104751, 104748, 104745, 104754, 104738, 104743, 104747, 104750, 104749, 104755, 104736, 104741, 104737, 104756 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "Iron Juggernaut";
            bossLoot = new int[] { 104768, 104778, 104770, 104775, 104777, 104762, 104765, 104771, 104767, 104766, 104774, 104758, 104773, 104769, 104760, 104761, 104759, 104757, 104772, 104764, 104776, 104763 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "Kor'kron Dark Shaman";
            bossLoot = new int[] { 104786, 104792, 104796, 104787, 104785, 104781, 104795, 104788, 104783, 104780, 104791, 104794, 104793, 104779, 104789, 104782, 104798, 104797, 104799, 104790, 104784 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "General Nazgrim";
            bossLoot = new int[] { 104803, 104809, 104807, 104808, 99746, 99747, 99745, 104802, 104800, 104806, 104805, 104801, 104804, 104810 };
            tempLoot.Add(raidBoss, bossLoot);

            // Wing 3
            raidBoss = "Malkorok";
            bossLoot = new int[] { 104826, 104817, 104829, 104830, 104831, 104825, 104813, 104823, 104812, 104814, 104818, 104816, 104820, 104819, 104822, 104811, 104824, 104827, 104832, 104821, 104828, 104815 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "Spoils of Pandaria";
            bossLoot = new int[] { 104838, 104843, 104837, 104844, 104851, 104850, 104834, 104839, 104847, 104848, 104846, 104836, 104845, 104853, 104841, 104835, 104852, 104842, 104849, 104833, 104840 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "Thok the Bloodthirsty";
            bossLoot = new int[] { 104857, 104863, 104864, 104858, 104855, 99749, 99750, 99748, 104854, 104861, 104856, 104859, 104860, 104862 };
            tempLoot.Add(raidBoss, bossLoot);

            // Wing 4
            raidBoss = "Siegecrafter Blackfuse";
            bossLoot = new int[] { 104873, 104875, 104869, 104870, 104872, 104868, 104866, 104867, 104871, 99755, 99756, 99754, 104874, 104865 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "Paragons of the Klaxxi";
            bossLoot = new int[] { 104882, 104879, 104883, 104884, 104876, 104881, 104877, 104886, 104878, 99752, 99753, 99751, 104880, 104885 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "Garrosh Hellscream";
            bossLoot = new int[] { 104899, 104901, 104890, 104907, 104888, 104906, 104898, 104900, 105864, 105863, 105865, 104896, 104905, 104902, 104889, 104887, 104893, 104908, 104891, 104903, 104904, 104892, 104894, 104897, 105674, 105672, 105679, 105678, 105673, 105671, 105680, 105676, 105677, 105670, 105675 };
            tempLoot.Add(raidBoss, bossLoot);

            raidLoot.Add(raidName, tempLoot);

            // Tier 16 Raid - Normal
            raidName = "Siege of Orgrimmar - Heroic";
            tempLoot = new Dictionary<string, int[]>();

            // Wing 1
            raidBoss = "Immerseus";
            bossLoot = new int[] { 103769, 102293, 103728, 103749, 103771, 103736, 103730, 103726, 103738, 103751, 103733, 103755, 103752, 103757, 103741, 103727, 103747, 103760, 103744, 103766, 103763, 103966 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "The Fallen Protectors";
            bossLoot = new int[] { 103783, 103776, 103820, 103799, 103817, 103780, 103809, 103822, 103787, 103801, 103802, 102296, 103812, 103804, 103790, 103815, 103807, 103924, 103775, 103793, 103777 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "Norushen";
            bossLoot = new int[] { 103867, 102295, 103841, 103857, 103852, 103740, 103849, 103830, 103845, 103838, 103847, 103834, 103836, 103942, 103762, 103861, 103858, 103827, 103855, 103864, 103826, 103839 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "Sha of Pride";
            bossLoot = new int[] { 99691, 99696, 99686, 103869, 103873, 102292, 103878, 103881, 102299, 103883, 103870, 103821, 103876 };
            tempLoot.Add(raidBoss, bossLoot);
                        
            // Wing 2: 
            raidBoss = "Galakras";
            bossLoot = new int[] { 103875, 103823, 102298, 103842, 103805, 103889, 103900, 103850, 103887, 103831, 103892, 103778, 103743, 103885, 103905, 103894, 103765, 103748, 103756, 103902, 103907, 103865 };
            tempLoot.Add(raidBoss, bossLoot);
                        
            raidBoss = "Iron Juggernaut";
            bossLoot = new int[] { 103773, 103921, 103898, 103916, 103918, 103731, 103782, 103788, 102297, 103909, 103912, 103735, 103739, 103767, 103811, 103759, 103754, 103908, 103922, 103813, 103863, 103914 };
            tempLoot.Add(raidBoss, bossLoot);
                        
            raidBoss = "Kor'kron Dark Shaman";
            bossLoot = new int[] { 103868, 102300, 103936, 102301, 103934, 103806, 103880, 103798, 103737, 103927, 103930, 103929, 103940, 103926, 103866, 103816, 103943, 103932, 103877, 103938, 103895 };
            tempLoot.Add(raidBoss, bossLoot);
                        
            raidBoss = "General Nazgrim";
            bossLoot = new int[] { 99692, 99682, 99687, 103945, 102294, 103946, 103872, 103829, 103732, 103913, 103808, 103949, 103904, 103947 };
            tempLoot.Add(raidBoss, bossLoot);

            // Wing 3: 
            raidBoss = "Malkorok";
            bossLoot = new int[] { 102303, 103843, 103959, 103772, 103879, 103899, 102306, 103848, 103952, 103835, 103951, 103917, 103761, 103758, 103953, 103923, 103890, 103955, 103939, 103954, 103957, 103742 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "Spoils of Pandaria";
            bossLoot = new int[] { 103962, 103871, 103888, 103851, 103862, 103941, 103961, 103893, 103964, 103860, 103882, 103911, 103803, 103768, 103745, 103779, 103965, 103796, 103967, 102302, 103933 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "Thok the Bloodthirsty";
            bossLoot = new int[] { 99683, 99694, 99689, 103968, 103750, 102305, 104308, 103774, 102304, 103828, 103853, 103919, 103896, 103915 };
            tempLoot.Add(raidBoss, bossLoot);

            // Wing 4: 
            raidBoss = "Siegecrafter Blackfuse";
            bossLoot = new int[] { 99695, 99685, 99690, 102309, 103794, 103884, 103910, 103891, 103970, 103874, 103814, 103969, 102311, 103792 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "Paragons of the Klaxxi";
            bossLoot = new int[] { 99693, 103824, 99684, 103973, 103948, 103810, 103956, 103819, 103886, 103844, 103972, 103971, 99688, 102308 };
            tempLoot.Add(raidBoss, bossLoot);

            raidBoss = "Garrosh Hellscream";
            bossLoot = new int[] { 105857, 105859, 105858, 102310, 103937, 103649, 103901, 103920, 103928, 103729, 103974, 103931, 103906, 103784, 103856, 103963, 103958, 103950, 102307, 103925, 104311, 103837, 103944, 103840, 103785 };
            tempLoot.Add(raidBoss, bossLoot);

            raidLoot.Add(raidName, tempLoot);

            // Tier 16 Raid - Heroic
            raidName = "Siege of Orgrimmar - Mythic";
            tempLoot = new Dictionary<string, int[]>();

            // Wing 1:
            raidBoss = "Immerseus";
            bossLoot = new int[] { 104425, 104414, 104426, 104412, 104416, 104417, 104428, 104429, 104424, 104418, 104421, 104427, 104430, 104422, 104419, 104415, 104431, 104413, 104423, 104411, 104420, 104432 };
            tempLoot.Add(raidBoss, bossLoot);
            
            /* TODO: Example for Future Raids
             * Tier XX Raid - Difficulty - Raid Name
            RaidBoss = "The Fallen Protectors";
            BossLoot = new int[] {  };
            tempLoot.Add(RaidBoss, BossLoot);
             * */

            raidLoot.Add(raidName, tempLoot);
        }

        #endregion

        #region " Raid Loot Results "

        /// <summary>
        /// Checks what raid and boss is selected, and then creates a loot table accordingly
        /// </summary>
        /// <param name="itemIds">array of integers that contains all the possible item ids that could drop as loot</param>
        /// <param name="raidGroup">array list of all the raid members</param>
        /// <returns>data table of the loot drop results</returns>
        public DataTable GetLootResults(int[] itemIds, ArrayList raidGroup)
        {
            // Create data table for all the new data
            DataTable loot = new DataTable();

            // Add Columns
            loot.Columns.Add("Upgrade");
            loot.Columns[0].DataType = typeof(int);
            loot.Columns.Add("CharacterName");
            loot.Columns.Add("ItemId");
            loot.Columns.Add("ItemName");
            loot.Columns.Add("ItemSlot");
            loot.Columns.Add("ItemILevel");
            loot.Columns.Add("OldItemILevel");
            loot.Columns.Add("OldItemId");

            // Now check to make sure it is valid and contains at least 1 item
            if (itemIds != null && itemIds.Length > 0)
            {
                // Go through all item ids
                foreach (int itemId in itemIds)
                {
                    ItemInfo item = null;

                    item = FormMain.Items.GetItem(itemId);

                    if (item != null)
                    {
                        // Now we have the item in the item cache

                        // Check against each member in the Raid Group
                        foreach (GuildMember gm in raidGroup)
                        {
                            /*   Here are the different Classes:
                             * Death Knight
                             * Druid
                             * Hunter
                             * Mage
                             * Monk
                             * Paladin
                             * Priest                                
                             * Rogue
                             * Shaman
                             * Warlock
                             * Warrior 
                             */
                            string charName = string.Empty;
                            int oldItemId = 0;

                            /* TODO:
                            // Change all references to a class to use the Converter.WoWClass.*
                            //  Ex: Converter.WoWClass.Hunter.ToString()
                            // Change all references to a role to use the Converter.WoWRole.*
                            //  Ex: Converter.WoWRole.DPS.ToString()
                            // Change all references to a Spec to use the Converter.WoWSpecs.*
                            //  Ex: Converter.WoWSpecs.Survival.ToString()
                            // **** OR ****
                            // We can make a member function in the GuildMember class like:
                            //  Ex: gm.IsClassHunter()
                            //  Ex: gm.IsRoleHealer()
                            //  Ex: gm.IsSpecSurvival()
                            // **** OR ****
                            //  EX: gm.Class == Converter.WoWClass.Hunter
                             */

                            // can this member use it?
                            if (Converter.ConvertItemClass(item.ItemClass) == "Armor")
                            {
                                // Armor
                                if (Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass) == "Micellaneous")
                                {
                                    // Trinkets
                                    if (Converter.ConvertInventoryType(item.InventoryType) == "trinket")
                                    {
                                        string test = item.Tooltip;

                                        // It's a trinket and not sure who it should go to...
                                        //  So for now, we're going to hard code this by name

                                        // Tier 16
                                        if (item.Name == "Purified Bindings of Immerseus" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesIntellect)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Rook's Unlucky Talisman" && gm.Role == "TANK")
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Fusion-Fire Core" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesStrength)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Assurance of Consequence" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesAgility)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Prismatic Prison of Pride" && gm.Role == "HEALING")
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Evil Eye of Galakras" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesStrength)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Juggernaut's Focusing Crystal" && gm.Role == "TANK")
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Haromm's Talisman" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesAgility)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Kardris' Toxic Totem" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesIntellect)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Nazgrim's Burnished Insignia" && gm.Role == "HEALING")
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Frenzied Crystal of Rage" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesIntellect)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Vial of Living Corruption" && gm.Role == "TANK")
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Sigil of Rampage" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesIntellect)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Thok's Tail Tip" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesStrength)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Thok's Acid-Grooved Tooth" && gm.Role == "HEALING")
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Dysmorphic Samophlange of Discontinuity" && gm.Role == "HEALING")
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Ticking Ebon Detonator" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesAgility)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Skeer's Bloodsoaked Talisman" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesStrength)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Black Blood of Y'Shaarj" && gm.Role == Converter.WoWRole.DPS.ToString() && gm.UsesIntellect)
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.Name == "Curse of Hubris" && gm.Role == "TANK")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasIntellect())
                                    {
                                        // Neck or Rings
                                        if (item.HasSpirit())
                                        {
                                            // Healer Classes
                                            if (gm.Role == "HEALING" || (gm.Role == Converter.WoWRole.DPS.ToString() && (gm.Spec == "Balance" || gm.Spec == "Shadow" || gm.Spec == "Elemental")))
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else
                                        {
                                            if ((!item.HasHit() && gm.Role == "HEALING") ||
                                                (gm.Role == Converter.WoWRole.DPS.ToString() && (gm.Spec == "Balance" || gm.Class == "Mage" || gm.Class == "Shadow" || gm.Spec == "Elemental" || gm.Class == "Warlock")))
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                    }
                                    else if (item.HasTankStats())
                                    {
                                        if (gm.Role == "TANK")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasStrength())
                                    {
                                        if ((gm.Class == "Paladin" && gm.Spec != "Holy") || gm.Class == "Warrior" || gm.Class == "Death Knight")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasAgility())
                                    {
                                        if ((gm.Class == "Monk" && gm.Role != "HEALING") || gm.Class == "Rogue" || gm.Class == "Hunter" || (gm.Class == "Druid" && (gm.Spec == "Feral" || gm.Spec == "Guardian")) || (gm.Class == "Shaman" && gm.Spec == "Enhancement"))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                }
                                else if (Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass) == "Plate")
                                {
                                    if (item.HasIntellect() && gm.Class == "Paladin" && gm.Spec == "Holy")
                                    {
                                        charName = gm.Name;
                                    }
                                    else if (item.HasTankStats())
                                    {
                                        if (gm.Role == "TANK" && (gm.Class == "Paladin" || gm.Class == "Warrior" || gm.Class == "Death Knight"))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (!item.HasIntellect() && ((gm.Class == "Paladin" && gm.Spec != "Holy") || gm.Class == "Warrior" || gm.Class == "Death Knight"))
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass) == "Mail")
                                {
                                    if (item.HasIntellect() && gm.Class == "Shaman" && (gm.Spec == "Elemental" || gm.Spec == "Restoration"))
                                    {
                                        charName = gm.Name;
                                    }
                                    else if (!item.HasIntellect() && ((gm.Class == "Shaman" && gm.Spec == "Enhancement") || gm.Class == "Hunter"))
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass) == "Leather")
                                {
                                    if (item.HasIntellect() && ((gm.Class == "Druid" && (gm.Spec == "Balance" || gm.Spec == "Restoration")) || (gm.Class == "Monk" && gm.Role == "HEALING")))
                                    {
                                        charName = gm.Name;
                                    }
                                    else if (!item.HasIntellect() && (gm.Class == "Rogue" || (gm.Class == "Druid" && (gm.Spec == "Guardian" || gm.Spec == "Feral")) || (gm.Class == "Monk" && gm.Role != "HEALING")))
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass) == "Cloth")
                                {
                                    // check to see if it's a back slot first... very different checks...
                                    if (Converter.ConvertInventoryType(item.InventoryType) == "back")
                                    {
                                        if (item.HasIntellect())
                                        {
                                            if (item.HasSpirit())
                                            {
                                                // Healer Classes
                                                if (gm.Role == "HEALING" || (gm.Role == Converter.WoWRole.DPS.ToString() && (gm.Spec == "Balance" || gm.Spec == "Shadow" || gm.Spec == "Elemental")))
                                                {
                                                    charName = gm.Name;
                                                }
                                            }
                                            else
                                            {
                                                if ((!item.HasHit() && gm.Role == "HEALING") ||
                                                    (gm.Role == Converter.WoWRole.DPS.ToString() && (gm.Spec == "Balance" || gm.Class == "Mage" || gm.Class == "Shadow" || gm.Spec == "Elemental" || gm.Class == "Warlock")))
                                                {
                                                    charName = gm.Name;
                                                }
                                            }
                                        }
                                        else if (item.HasTankStats())
                                        {
                                            if (gm.Role == "TANK")
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else if (item.HasStrength())
                                        {
                                            if ((gm.Class == "Paladin" && gm.Spec != "Holy") || gm.Class == "Warrior" || gm.Class == "Death Knight")
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else if (item.HasAgility())
                                        {
                                            if ((gm.Class == "Monk" && gm.Role != "HEALING") || gm.Class == "Rogue" || gm.Class == "Hunter" || (gm.Class == "Druid" && (gm.Spec == "Feral" || gm.Spec == "Guardian")) || (gm.Class == "Shaman" && gm.Spec == "Enhancement"))
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                    }
                                    else if (item.HasSpirit() && (gm.Role == "HEALING" && gm.Class == "Priest"))
                                    {
                                        // it's a regular non-back, cloth piece
                                        charName = gm.Name;
                                    }
                                    else if (!item.HasSpirit() && (gm.Class == "Mage" || gm.Class == "Warlock" || (gm.Class == "Priest" && gm.Spec == "Shadow")))
                                    {
                                        // it's a regular non-back, cloth piece
                                        charName = gm.Name;
                                    }
                                }
                                else if (Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass) == "Shield")
                                {
                                    if (item.HasIntellect())
                                    {
                                        if ((gm.Class == "Paladin" && gm.Spec == "Holy") || (gm.Class == "Shaman" && (gm.Spec == "Elemental" || gm.Spec == "Restoration")))
                                        {
                                            // Holy Paladin or Elemental Shaman or Restoration Shaman
                                            charName = gm.Name;
                                        }
                                    }
                                    else
                                    {
                                        if (gm.Role == "TANK" && (gm.Class == "Paladin" || gm.Class == "Warrior"))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                }
                                else
                                {
                                    Logging.DisplayError("Armor [" + item.ItemSubClass + "] not found!");
                                }
                            }
                            else if (Converter.ConvertItemClass(item.ItemClass) == "Weapon")
                            {
                                // Weapon
                                string weapon = Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass);

                                if (weapon == "1 Axe")
                                {
                                    // (gm.Class == "Death Knight" || gm.Class == "Monk" || gm.Class == "Paladin" || gm.Class == "Rogue" || gm.Class == "Shaman" || gm.Class == "Warrior")
                                    if (item.HasTankStats())
                                    {
                                        // STR vs AGI
                                        if (gm.Role == "TANK")
                                        {
                                            if (item.HasStrength() && (gm.Class == "Paladin" || gm.Class == "Death Knight" || gm.Class == "Warrior"))
                                            {
                                                charName = gm.Name;
                                            }
                                            else if (item.HasAgility() && gm.Class == "Monk")
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                    }
                                    else if (item.HasStrength())
                                    {
                                        if (gm.Class == "Death Knight" || (gm.Class == "Paladin" && gm.Role != "HEALING") || gm.Class == "Warrior")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasAgility())
                                    {
                                        if ((gm.Class == "Monk" && gm.Role != "HEALING") || gm.Class == "Rogue" || (gm.Class == "Shaman" && gm.Spec == "Enhancement"))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasIntellect())
                                    {
                                        if (item.HasSpirit() && gm.Role == "HEALING" && (gm.Class == "Paladin" || gm.Class == "Shaman" || gm.Class == "Monk"))
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.HasHit() && gm.Role == Converter.WoWRole.DPS.ToString() && gm.Class == "Shaman")
                                        {
                                            charName = gm.Name;
                                        }
                                        else if ((gm.Class == "Monk" && gm.Role == "HEALING") || (gm.Class == "Paladin" && gm.Role == "HEALING") || (gm.Class == "Shaman" && (gm.Spec == "Elemental" || gm.Spec == "Restoration")))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                }
                                else if (weapon == "2 Axe")
                                {
                                    if (item.HasStrength() && (gm.Class == "Death Knight" || (gm.Class == "Paladin" && gm.Role == Converter.WoWRole.DPS.ToString()) || (gm.Class == "Warrior" && gm.Role == Converter.WoWRole.DPS.ToString())))
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (weapon == "1 Mace")
                                {
                                    if (item.HasTankStats())
                                    {
                                        // STR vs AGI
                                        if (gm.Role == "TANK")
                                        {
                                            if (item.HasStrength() && (gm.Class == "Paladin" || gm.Class == "Death Knight" || gm.Class == "Warrior"))
                                            {
                                                charName = gm.Name;
                                            }
                                            else if (item.HasAgility() && (gm.Class == "Monk" || gm.Class == "Druid"))
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                    }
                                    else if (item.HasStrength())
                                    {
                                        if (gm.Class == "Death Knight" || (gm.Class == "Paladin" && gm.Role == "TANK") || (gm.Class == "Warrior" && gm.Spec == "Arms"))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasAgility())
                                    {
                                        if ((gm.Class == "Monk" && gm.Role != "HEALING") || gm.Class == "Rogue" || (gm.Class == "Shaman" && gm.Spec == "Enhancement") || (gm.Class == "Druid" && (gm.Spec == "Feral" || gm.Spec == "Guardian")))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasIntellect())
                                    {
                                        if (item.HasSpirit() && gm.Role == "HEALING" && (gm.Class == "Paladin" || gm.Class == "Shaman" || gm.Class == "Monk" || gm.Class == "Druid" || gm.Class == "Priest"))
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.HasHit() && gm.Role == Converter.WoWRole.DPS.ToString() && (gm.Class == "Druid" || gm.Class == "Priest" || gm.Class == "Shaman"))
                                        {
                                            charName = gm.Name;
                                        }
                                        else if ((gm.Class == "Monk" && gm.Role == "HEALING") || (gm.Class == "Paladin" && gm.Role == "HEALING") || (gm.Class == "Shaman" && (gm.Spec == "Elemental" || gm.Spec == "Restoration")) || gm.Class == "Priest")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                }
                                else if (weapon == "2 Mace")
                                {
                                    if (item.HasStrength())
                                    {
                                        if (item.HasTankStats())
                                        {
                                            if (gm.Role == "TANK" && gm.Class == "Death Knight")
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else if (gm.Role == Converter.WoWRole.DPS.ToString() && (gm.Class == "Death Knight" || gm.Class == "Paladin" || gm.Class == "Warrior"))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasAgility() && gm.Class == "Druid" && gm.Role != "HEALING")
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (weapon == "Polearm")
                                {
                                    if (item.HasStrength() && (gm.Class == "Death Knight" || gm.Class == "Paladin" || gm.Class == "Warrior"))
                                    {
                                        charName = gm.Name;
                                    }
                                    else if (item.HasAgility() && (gm.Class == "Druid" || gm.Class == "Monk") && gm.Role != "HEALING")
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (weapon == "1 Sword")
                                {
                                    if (item.HasTankStats())
                                    {
                                        // STR vs AGI
                                        if (gm.Role == "TANK")
                                        {
                                            if (item.HasStrength() && (gm.Class == "Paladin" || gm.Class == "Death Knight" || gm.Class == "Warrior"))
                                            {
                                                charName = gm.Name;
                                            }
                                            else if (item.HasAgility() && gm.Class == "Monk")
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                    }
                                    else if (item.HasStrength())
                                    {
                                        if ((gm.Class == "Death Knight" && gm.Spec == "Frost") || (gm.Class == "Paladin" && gm.Role == "TANK") || (gm.Class == "Warrior" && gm.Spec != "Arms"))
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasAgility())
                                    {
                                        if ((gm.Class == "Monk" && gm.Role != "HEALING") || gm.Class == "Rogue")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasIntellect())
                                    {
                                        if (item.HasSpirit() && gm.Role == "HEALING" && (gm.Class == "Paladin" || gm.Class == "Shaman" || gm.Class == "Monk"))
                                        {
                                            charName = gm.Name;
                                        }
                                        else if (item.HasHit() && gm.Role == Converter.WoWRole.DPS.ToString() && (gm.Class == "Mage" || gm.Class == "Warlock"))
                                        {
                                            charName = gm.Name;
                                        }
                                        else if ((gm.Class == "Monk" && gm.Role == "HEALING") || (gm.Class == "Paladin" && gm.Role == "HEALING") || gm.Class == "Mage" || gm.Class == "Warlock")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                }
                                else if (weapon == "2 Sword")
                                {
                                    if (item.HasTankStats())
                                    {
                                        if (gm.Role == "TANK" && gm.Class == "Death Knight")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (gm.Role == Converter.WoWRole.DPS.ToString() && ((gm.Class == "Death Knight" && gm.Spec != "Frost") || gm.Class == "Paladin" || gm.Class == "Warrior"))
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (weapon == "Staff")
                                {
                                    if (item.HasIntellect())
                                    {
                                        if (item.HasSpirit())
                                        {
                                            if (gm.Role == "HEALING" && ((gm.Class == "Druid" && gm.Spec == "Restoration") || (gm.Class == "Monk" && gm.Spec == "Mistweaver") || (gm.Class == "Priest" && gm.Spec != "Shadow") || (gm.Class == "Shaman" && gm.Spec == "Restoration")))
                                            {
                                                charName = gm.Name;
                                            }
                                            else if (gm.Role == Converter.WoWRole.DPS.ToString() && ((gm.Class == "Druid" && gm.Spec == "Balance") || (gm.Class == "Priest" && gm.Spec == "Shadow") || (gm.Class == "Shaman" && gm.Spec == "Elemental")))
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else if (item.HasHit())
                                        {
                                            if (gm.Role == Converter.WoWRole.DPS.ToString() && ((gm.Class == "Druid" && gm.Spec == "Balance") || gm.Class == "Mage" || (gm.Class == "Priest" && gm.Spec == "Shadow") || (gm.Class == "Shaman" && gm.Spec == "Elemental") || gm.Class == "Warlock"))
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else if ((gm.Class == "Druid" && (gm.Spec == "Balance" || gm.Spec == "Restoration")) || (gm.Class == "Monk" && gm.Spec == "Mistweaver") || gm.Class == "Mage" || gm.Class == "Priest" || (gm.Class == "Shaman" && (gm.Spec == "Elemental" || gm.Spec == "Restoration")) || gm.Class == "Warlock")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasAgility() && ((gm.Class == "Druid" && (gm.Spec == "Guardian" || gm.Spec == "Feral")) || (gm.Class == "Monk" && (gm.Spec == "Brewmaster" || gm.Spec == "Windwalker"))))
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (weapon == "Fist Weapon")
                                {
                                    if (item.HasAgility() && ((gm.Class == "Druid" && (gm.Spec == "Feral" || gm.Spec == "Guardian")) || (gm.Class == "Monk" && gm.Role != "HEALING") || gm.Class == "Rogue" || (gm.Class == "Shaman" && gm.Spec == "Enhancement")))
                                    {
                                        charName = gm.Name;
                                    }
                                    else if (item.HasStrength() && gm.Class == "Warrior")
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (weapon == "Dagger")
                                {
                                    if (item.HasIntellect())
                                    {
                                        if (item.HasSpirit())
                                        {
                                            if (gm.Role == "HEALING" && ((gm.Class == "Druid" && gm.Spec == "Restoration") || gm.Class == "Priest" || (gm.Class == "Shaman" && gm.Spec == "Restoration")))
                                            {
                                                charName = gm.Name;
                                            }
                                            else if (gm.Role == Converter.WoWRole.DPS.ToString() && ((gm.Class == "Druid" && gm.Spec == "Balance") || gm.Class == "Priest" || (gm.Class == "Shaman" && gm.Spec == "Elemental")))
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else if (item.HasHit())
                                        {
                                            if (gm.Role == Converter.WoWRole.DPS.ToString() && ((gm.Class == "Druid" && gm.Spec == "Balance") || gm.Class == "Mage" || (gm.Class == "Priest" && gm.Spec == "Shadow") || (gm.Class == "Shaman" && gm.Spec == "Elemental") || gm.Class == "Warlock"))
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else if ((gm.Class == "Druid" && (gm.Spec == "Balance" || gm.Spec == "Restoration")) || gm.Class == "Mage" || gm.Class == "Priest" || (gm.Class == "Shaman" && (gm.Spec == "Elemental" || gm.Spec == "Restoration")) || gm.Class == "Warlock")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                    else if (item.HasAgility() && ((gm.Class == "Druid" && (gm.Spec == "Guardian" || gm.Spec == "Feral")) || gm.Class == "Rogue" || (gm.Class == "Shaman" && gm.Spec == "Enhancement")))
                                    {
                                        charName = gm.Name;
                                    }
                                    else if (item.HasStrength() && gm.Class == "Warrior")
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else if (weapon == "Wand")
                                {
                                    if (item.HasIntellect())
                                    {
                                        if (item.HasSpirit())
                                        {
                                            if (gm.Class == "Priest")
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else if (item.HasHit())
                                        {
                                            if (gm.Class == "Mage" || gm.Class == "Warlock" || (gm.Class == "Priest" && gm.Spec == "Shadow"))
                                            {
                                                charName = gm.Name;
                                            }
                                        }
                                        else if (gm.Class == "Mage" || gm.Class == "Warlock" || gm.Class == "Priest")
                                        {
                                            charName = gm.Name;
                                        }
                                    }
                                }
                                else if (weapon == "Bow" || weapon == "Rifle" || weapon == "Thrown" || weapon == "Crossbow")
                                {
                                    if (gm.Class == Converter.WoWClass.Hunter.ToString())
                                    {
                                        charName = gm.Name;
                                    }
                                }
                                else
                                {
                                    Logging.DisplayError("Weapon [" + weapon + "] not found!");
                                }
                            }
                            else if (Converter.ConvertItemClass(item.ItemClass) == "Miscellaneous")
                            {
                                // Check if this is an armor token
                                if (item.Name.StartsWith("Helm"))
                                {
                                    item.InventoryType = 1;
                                }
                                else if (item.Name.StartsWith("Shoulders"))
                                {
                                    item.InventoryType = 3;
                                }
                                else if (item.Name.StartsWith("Chest"))
                                {
                                    item.InventoryType = 5;
                                }
                                else if (item.Name.StartsWith("Gauntlets"))
                                {
                                    item.InventoryType = 10;
                                }
                                else if (item.Name.StartsWith("Leggings"))
                                {
                                    item.InventoryType = 7;
                                }
                                else if (item.Name.StartsWith("Essence"))
                                {
                                    // item.InventoryType = 7;
                                    // todo: need to be able check an item that could be any of the following: head/shoulder/chest/hands/legs tier piece
                                }

                                if (Converter.ConvertInventoryType(item.InventoryType) != "error")
                                {
                                    if (item.Name.EndsWith(" of the Cursed Conqueror") && (gm.Class == "Paladin" || gm.Class == "Priest" || gm.Class == "Warlock"))
                                    {
                                        charName = gm.Name;
                                    }
                                    else if (item.Name.EndsWith(" of the Cursed Protector") && (gm.Class == "Warrior" || gm.Class == "Hunter" || gm.Class == "Shaman" || gm.Class == "Monk"))
                                    {
                                        charName = gm.Name;
                                    }
                                    else if (item.Name.EndsWith(" of the Cursed Vanquisher") && (gm.Class == "Rogue" || gm.Class == "Death Knight" || gm.Class == "Mage" || gm.Class == "Druid"))
                                    {
                                        charName = gm.Name;
                                    }
                                }
                            }

                            // Can the row be added to data table?
                            if (!string.IsNullOrEmpty(charName))
                            {
                                string slot = Converter.ConvertInventoryType(item.InventoryType);
                                bool pass = false;
                                int ilvlOld = 0;
                                int ilvlNew = 0;

                                if (slot == "finger")
                                {
                                    pass |= gm.ItemAudits.ContainsKey("finger1");
                                    pass |= gm.ItemAudits.ContainsKey("finger2");
                                }
                                else if (slot == "trinket")
                                {
                                    pass |= gm.ItemAudits.ContainsKey("trinket1");
                                    pass |= gm.ItemAudits.ContainsKey("trinket2");
                                }
                                else if (slot == "mainHand")
                                {
                                    // Need to check that either a 2 hander is being used
                                    //   or 2 one handers, or 1 hand and off hand is equiped
                                    string weapon = Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass);

                                    if (weapon == "1 Axe" || weapon == "1 Mace" || weapon == "1 Sword" || weapon == "Fist Weapon" || weapon == "Dagger" || weapon == "Wand")
                                    {
                                        // this is NOT a 2 handed weapon, check to make sure both hands are equiped

                                        // Look for 2 x 1 handed weapons
                                        int mainHands = gm.ItemAudits.Keys.Where(kv => kv == "mainHand").Count();

                                        if (mainHands == 2)
                                        {
                                            pass = gm.ItemAudits.ContainsKey(slot);
                                        }
                                        else if (gm.ItemAudits.ContainsKey("Shield") || gm.ItemAudits.ContainsKey("offHand"))
                                        {
                                            pass = gm.ItemAudits.ContainsKey(slot);
                                        }
                                    }
                                    else
                                    {
                                        pass = gm.ItemAudits.ContainsKey(slot);
                                    }
                                }
                                else
                                {
                                    pass = gm.ItemAudits.ContainsKey(slot);
                                }

                                // Did it pass?  Is it an upgrade?
                                if (pass)
                                {
                                    ilvlNew = item.ItemLevel;

                                    if (slot == "finger")
                                    {
                                        // Need to account for any ring duplications
                                        if (item.Name == gm.ItemAudits["finger1"].Name)
                                        {
                                            ilvlOld = gm.ItemAudits["finger1"].ItemLevel;
                                            oldItemId = gm.ItemAudits["finger1"].Id;
                                        }
                                        else if (item.Name == gm.ItemAudits["finger2"].Name)
                                        {
                                            ilvlOld = gm.ItemAudits["finger2"].ItemLevel;
                                            oldItemId = gm.ItemAudits["finger2"].Id;
                                        }
                                        else
                                        {
                                            // upgrade!
                                            ilvlOld = gm.ItemAudits["finger1"].ItemLevel > gm.ItemAudits["finger2"].ItemLevel ? gm.ItemAudits["finger2"].ItemLevel : gm.ItemAudits["finger1"].ItemLevel;
                                            oldItemId = gm.ItemAudits["finger1"].Id > gm.ItemAudits["finger2"].Id ? gm.ItemAudits["finger2"].Id : gm.ItemAudits["finger1"].Id;
                                        }
                                    }
                                    else if (slot == "trinket")
                                    {
                                        // Need to account for any ring duplications
                                        if (item.Name == gm.ItemAudits["trinket1"].Name)
                                        {
                                            ilvlOld = gm.ItemAudits["trinket1"].ItemLevel;
                                            oldItemId = gm.ItemAudits["trinket1"].ItemLevel;
                                        }
                                        else if (item.Name == gm.ItemAudits["trinket2"].Name)
                                        {
                                            ilvlOld = gm.ItemAudits["trinket2"].ItemLevel;
                                            oldItemId = gm.ItemAudits["trinket2"].ItemLevel;
                                        }
                                        else
                                        {
                                            // upgrade!
                                            ilvlOld = gm.ItemAudits["trinket1"].ItemLevel > gm.ItemAudits["trinket2"].ItemLevel ? gm.ItemAudits["trinket2"].ItemLevel : gm.ItemAudits["trinket1"].ItemLevel;
                                            oldItemId = gm.ItemAudits["trinket1"].Id > gm.ItemAudits["trinket2"].Id ? gm.ItemAudits["trinket2"].Id : gm.ItemAudits["trinket1"].Id;
                                        }
                                    }
                                    else if (slot == "mainHand" || slot == "offHand")
                                    {
                                        string weapon = Converter.ConvertItemSubClass(item.ItemClass, item.ItemSubClass);

                                        // Need to check for dual hands vs 2 handers vs one hand + off hand...
                                        if ((gm.Class == "Rogue" || (gm.Class == "Shaman" && gm.Spec == "Enhancement") || (gm.Class == "Warrior" && gm.Spec == "Fury") ||
                                            (gm.Class == "Druid" && (gm.Spec == "Guardian" || gm.Spec == "Feral")) || (gm.Class == "Monk" && (gm.Spec == "Brewmaster" ||
                                            gm.Spec == "Windwalker")) || (gm.Class == "Death Knight" && (gm.Spec == "Unholy" || gm.Spec == "Frost"))) &&
                                            (gm.ItemAudits["offHand"].ItemLevel > 0))
                                        {
                                            ilvlOld = gm.ItemAudits["mainHand"].ItemLevel > gm.ItemAudits["offHand"].ItemLevel ? gm.ItemAudits["offHand"].ItemLevel : gm.ItemAudits["mainHand"].ItemLevel;
                                            oldItemId = gm.ItemAudits["mainHand"].Id > gm.ItemAudits["offHand"].Id ? gm.ItemAudits["offHand"].Id : gm.ItemAudits["mainHand"].Id;
                                        }
                                        else if (slot == "offHand" && gm.ItemAudits["offHand"].ItemLevel == 0)
                                        {
                                            int itemIdMainHand = gm.ItemAudits["mainHand"].Id;
                                            int slotType = 0;
                                            ItemInfo itemMainHand = null;

                                            itemMainHand = FormMain.Items.GetItem(itemIdMainHand);

                                            if (itemMainHand != null)
                                            {
                                                slotType = itemMainHand.InventoryType;

                                                // if slot if offHand and currently doesn't have an offHand equipped
                                                //  and mainhand is a 2 hander... compare iLevel with mainHand
                                                if (slotType == 17 || slotType == 15 || slotType == 26)
                                                {
                                                    ilvlOld = gm.ItemAudits["mainHand"].ItemLevel;
                                                    oldItemId = gm.ItemAudits["mainHand"].Id;
                                                }
                                                else
                                                {
                                                    ilvlOld = gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType)].ItemLevel;
                                                    oldItemId = gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType)].Id;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // upgrade!
                                            ilvlOld = gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType)].ItemLevel;
                                            oldItemId = gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType)].Id;
                                        }
                                    }
                                    else
                                    {
                                        // upgrade!
                                        ilvlOld = gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType)].ItemLevel;
                                        oldItemId = gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType)].Id;
                                    }

                                    if (ilvlNew > ilvlOld)
                                    {
                                        // This is a definite upgrade
                                        DataRow dr = loot.NewRow();
                                        dr["Upgrade"] = ilvlNew - ilvlOld;
                                        dr["ItemId"] = item.Id;
                                        dr["ItemName"] = item.Name;
                                        dr["ItemSlot"] = slot;
                                        dr["CharacterName"] = charName;
                                        dr["ItemILevel"] = ilvlNew;
                                        dr["OldItemILevel"] = ilvlOld;
                                        dr["OldItemId"] = oldItemId;
                                        loot.Rows.Add(dr);
                                    }
                                    else
                                    {
                                        // Check for a upgrade taking upgraded items into account
                                        int ilvlOriginal = ilvlOld;

                                        if (Converter.ConvertInventoryType(item.InventoryType) == "trinket")
                                        {
                                            // Get the original iLevel of both trinkets
                                            int testTrinket1 = FormMain.Items.GetItem(gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType) + "1"].Id).ItemLevel;
                                            int testTrinket2 = FormMain.Items.GetItem(gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType) + "2"].Id).ItemLevel;

                                            // Need to account for any trinket duplicates
                                            if (item.Name == gm.ItemAudits["trinket1"].Name)
                                            {
                                                if (item.ItemLevel > testTrinket1)
                                                {
                                                    ilvlOriginal = testTrinket1;
                                                }
                                            }
                                            else if (item.Name == gm.ItemAudits["trinket2"].Name)
                                            {
                                                if (item.ItemLevel > testTrinket2)
                                                {
                                                    ilvlOriginal = testTrinket2;
                                                }
                                            }
                                            else
                                            {
                                                // upgrade!
                                                ilvlOriginal = testTrinket1 > testTrinket2 ? testTrinket2 : testTrinket1;
                                            }
                                        }
                                        else if (Converter.ConvertInventoryType(item.InventoryType) == "finger")
                                        {
                                            int testRing1 = FormMain.Items.GetItem(gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType) + "1"].Id).ItemLevel;
                                            int testRing2 = FormMain.Items.GetItem(gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType) + "2"].Id).ItemLevel;

                                            // Need to account for any ring duplicates
                                            if (item.Name == gm.ItemAudits["finger1"].Name)
                                            {
                                                if (ilvlOriginal <= testRing1)
                                                {
                                                    ilvlOriginal = testRing1;
                                                }
                                            }
                                            else if (item.Name == gm.ItemAudits["finger2"].Name)
                                            {
                                                if (ilvlOriginal <= testRing2)
                                                {
                                                    ilvlOriginal = testRing2;
                                                }
                                            }
                                            else
                                            {
                                                // upgrade!
                                                ilvlOriginal = testRing1 > testRing2 ? testRing2 : testRing1;
                                            }
                                        }
                                        else
                                        {
                                            // upgrade!
                                            ilvlOriginal = FormMain.Items.GetItem(gm.ItemAudits[Converter.ConvertInventoryType(item.InventoryType)].Id).ItemLevel;
                                        }

                                        // Now that we have both the item's base iLevel and the new item's base iLevel, 
                                        //  we can compare the two
                                        if (ilvlNew > ilvlOriginal)
                                        {
                                            // This is an upgrade according to the pre-upgraded item
                                            DataRow dr = loot.NewRow();
                                            dr["Upgrade"] = ilvlNew - ilvlOriginal;
                                            dr["ItemId"] = item.Id;
                                            dr["ItemName"] = item.Name;
                                            dr["ItemSlot"] = slot;
                                            dr["CharacterName"] = charName;
                                            dr["ItemILevel"] = ilvlNew;
                                            dr["OldItemILevel"] = ilvlOld;
                                            dr["OldItemId"] = oldItemId;
                                            loot.Rows.Add(dr);
                                        }
                                        else
                                        {
                                            // Not an upgrade
                                            DataRow dr = loot.NewRow();
                                            dr["Upgrade"] = -1;
                                            dr["ItemId"] = item.Id;
                                            dr["ItemName"] = item.Name;
                                            dr["ItemSlot"] = slot;
                                            dr["CharacterName"] = "n/a";
                                            dr["ItemILevel"] = ilvlNew;
                                            dr["OldItemILevel"] = 0;
                                            dr["OldItemId"] = 0;
                                            loot.Rows.Add(dr);
                                        }
                                    }
                                }
                                else
                                {
                                    Logging.DisplayError("Slot #" + item.InventoryType + "[" + slot + "] not found!");
                                }
                            }
                        }
                    }
                }
            }

            return loot;
        }

        #endregion
    }
}
