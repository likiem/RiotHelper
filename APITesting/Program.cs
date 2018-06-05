using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace APITesting
{
    class Program
    {
        static void Main(string[] args)
        {
            //string ApiKey = "RGAPI-9d3d2e42-a655-4c9b-b63c-7afe7a957f44"; // Replace this with your API key, of course.

            List<string> tags = new List<string>();
            string[] newtag = { "stats" };
            tags.Add("stats");           
            string baseURL = @"https://euw1.api.riotgames.com/";
            string endTagStats = @"&locale=en_US&dataById=true&tags=all";
            string apiCallChampionByID = @"lol/static-data/v3/champions/1";
            string apiKey = "?api_key=RGAPI-012d4171-9db4-4c5f-b8c8-d0877602287d";

            ChampionListStatic championList = GetAllChampions(apiKey);
            string summonerName = "ganklikegaston";
            Console.WriteLine("Welcome, The match history for summoner: {0}", summonerName);


            string apiCallSummonerByName = @"/lol/summoner/v3/summoners/by-name/" + summonerName;

            //string apiCallSummonerByName = @"/lol/summoner/v3/summoners/by-name/feedzlikegaston";
            //long accountID = ReturnItemOfJson<Summmoner>(baseURL + "/lol/summoner/v3/summoners/by-name/likiem" + apiKey).AccountId;
            long accountID = ReturnAccountByID(baseURL + apiCallSummonerByName + apiKey);
            string apiCallMatchByAccountID = @"/lol/match/v3/matchlists/by-account/" + accountID;
            
            string apiCallMatchByGameId = @"/lol/match/v3/matches/";
            string endIndexTag = "&endIndex=100";

            string testURL = baseURL + apiCallChampionByID + apiKey + endTagStats;

            Champion chump = ReturnItemOfJson<Champion>(testURL);
            MatchList summonerMatches = ReturnItemOfJson<MatchList>(baseURL + apiCallMatchByAccountID + apiKey + endIndexTag);
            List<long> listOfGameId = new List<long>();
            List<Match> PlayerMatches = new List<Match>();
            int adamcount = 0;
            int georgecount = 0;
            int rachcount = 0;
            int jamiecount = 0;
            int howmanyfirstblood = 0;

            

            foreach (var item in summonerMatches.matches)
            {
                
                Console.WriteLine("Champion: " + ReturnChampionName(item.champion, championList) + " Role: " + item.lane + " queue: " + item.queue);
                listOfGameId.Add(item.gameId);
            }

            foreach (var gameId in listOfGameId)
            {
               PlayerMatches.Add(ReturnItemOfJson<Match>(baseURL + apiCallMatchByGameId + gameId + apiKey + endIndexTag));
               Thread.Sleep(1000);
            }

            Console.WriteLine("~~~~~~~~~~~~~~~~~~");

            foreach (var match in PlayerMatches)
            {
                Console.WriteLine("Match:" );
                foreach (var participant in match.ParticipantIdentities)
                {
                    Console.WriteLine("participants: " + participant.Player.SummonerName);
                    if(participant.Player.SummonerName == "PushLikeGaston")
                    {
                        adamcount++;
                    } else if (participant.Player.SummonerName == "likiem")
                    {
                        rachcount++;
                    } else if (participant.Player.SummonerName == "GankLikeGaston")
                    {
                        georgecount++;
                    } else if (participant.Player.SummonerName == "ProjectNull")
                    {
                        jamiecount++;
                    }


                    if (participant.Player.SummonerName == "Likiem")
                    {
                        int participantuniqueid = participant.ParticipantId;
                        foreach (var item in match.Participants)
                        {
                            if(participantuniqueid == item.ParticipantId)
                            {
                                if (item.Stats.FirstBloodKill)
                                {
                                    howmanyfirstblood++;
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("END");
            }

            Console.WriteLine("Total number of matches compared: " + summonerMatches.matches.Count);
            Console.WriteLine("Times played with adam: " + adamcount);
            Console.WriteLine("Times played with nath: " + rachcount);
            Console.WriteLine("Times played with george: " + georgecount);
            Console.WriteLine("Times played with jamie: " + jamiecount);
            Console.WriteLine("Total number of firstblood by Likiem: " + howmanyfirstblood);

             

            Console.ReadLine();
            //https://euw1.api.riotgames.com/lol/summoner/v3/summoners/by-name/Likiem?api_key=RGAPI-84af421c-31d0-4d98-aebd-d8e04536acad
            //key = RGAPI-0ec7b317-7370-4890-92fd-035de0dea8e1
            //Matches:
            //https://euw1.api.riotgames.com/lol/match/v3/matchlists/by-account/232921/recent?api_key=RGAPI-84af421c-31d0-4d98-aebd-d8e04536acad
            //Specific match
            //https://euw1.api.riotgames.com/lol/match/v3/matches/3496281038?api_key=RGAPI-0ec7b317-7370-4890-92fd-035de0dea8e1

            //CustomClasses.RootObject items = ReturnItemOfJson<CustomClasses.RootObject>("https://euw1.api.riotgames.com/lol/match/v3/matches/3496281038?api_key=RGAPI-0ec7b317-7370-4890-92fd-035de0dea8e1");
            //    int champion1 = items.participants[0].championId;
            //    Champion champion = ReturnItemOfJson<Champion>("https://euw1.api.riotgames.com//lol/platform/v3/champions/" + champion1 + "?api_key=RGAPI-0ec7b317-7370-4890-92fd-035de0dea8e1");
            //    string annie = ReturnJsonString("https://na1.api.riotgames.com/lol/static-data/v3/champions/1?api_key=RGAPI-0ec7b317-7370-4890-92fd-035de0dea8e1&locale=en_US&dataById=true&tags=stats");
            //    string champ = ReturnJsonString("https://euw1.api.riotgames.com/lol/static-data/v3/champions/10?api_key=RGAPI-0ec7b317-7370-4890-92fd-035de0dea8e1&locale=en_US&dataById=true&tags=stats");
        }
        //public static Task GetStaticChampionsAsyncTest()
        //{
        //    IRiotClient client = new RiotClient();
        //    var championList = client.GetStaticChampionsAsync(tags: new[] { "all" });
        //    return championList;
        //}

        public static void ReturnDamageDone(Participant participant)
        {
             
        }
        public static long ReturnAccountByID (string jsonURL)
        {
            using (WebClient wc = new WebClient())
            {
                Console.WriteLine("request sent");
                wc.Proxy = WebRequest.DefaultWebProxy;
                wc.Credentials = System.Net.CredentialCache.DefaultCredentials; ;
                wc.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                var json = wc.DownloadString(jsonURL);
                Summmoner item = JsonConvert.DeserializeObject<Summmoner>(json);
                return item.AccountId;
            }
        }
        public static T ReturnItemOfJson<T>(string jsonURL)
        {
            using (WebClient wc = new WebClient())
            {
                Console.WriteLine("request sent");
                wc.Proxy = WebRequest.DefaultWebProxy;
                wc.Credentials = System.Net.CredentialCache.DefaultCredentials; ;
                wc.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                var json = wc.DownloadString(jsonURL);
                T item = JsonConvert.DeserializeObject<T>(json);
                return item;
            }
        }
        public static ChampionListStatic GetAllChampions (string apiKey)
        {           
                //var json = File.ReadAllText(@"C:\Users\20151174\Documents\Visual Studio 2015\Projects\APITesting\APITesting\champions.json");
                var json = File.ReadAllText(@"C:\Users\mazu\Downloads\rito\APITesting\APITesting\champions.json");

            ChampionListStatic champList = JsonConvert.DeserializeObject<ChampionListStatic>(json);
                return champList;
        }
        public static void ParticipantIdentifierFunction(ParticipantIdentity PI)
        {

        }
        public static string ReturnChampionName(int champId, ChampionListStatic championList)
        {
            string champName = "id: " + champId;
            foreach (var champion in championList.Champions)
            {
                if (champion.Value.Id == champId)
                {
                    champName = champion.Value.Name;
                }
            }
            return champName;
        }

        public static string ReturnJsonString(string jsonURL)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Proxy = WebRequest.DefaultWebProxy;
                wc.Credentials = System.Net.CredentialCache.DefaultCredentials; ;
                wc.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                var json = wc.DownloadString(jsonURL);
                return json;
            }
        }
    }
    public class Champion
    {

        public int id { get; set; }
        public bool active { get; set; }
        public bool botEnabled { get; set; }
        public bool freeToPlay { get; set; }
        public bool botMmEnabled { get; set; }
        public bool rankedPlayEnabled { get; set; }

    }
    public class Summmoner
    {
        //
        // Summary:
        //     Gets or sets the summoner's account ID.
        public long AccountId { get; set; }
        //
        // Summary:
        //     Gets or sets the summoner ID.
        public long Id { get; set; }
        //
        // Summary:
        //     Gets or sets the summoner's name.
        public string Name { get; set; }
        //
        // Summary:
        //     Gets or sets the ID of the summoner's profile icon.
        public int ProfileIconId { get; set; }
        //
        // Summary:
        //     Gets or sets the date and time (in UTC) when the summoner was last modified.
        //     The summoner is modified by the following events: changing summoner icon, playing
        //     a tutorial, finishing a game, or changing summoner name.
        public long RevisionDate { get; set; }
        //
        // Summary:
        //     Gets or sets the summoner's level.
        public long SummonerLevel { get; set; }
    }
    public class MatchList
    {

        public List<MatchReference> matches { get; set; }
        public int totalGames { get; set; }
        public int startIndex { get; set; }
        public int endIndex { get; set; }
    }
    public class MatchReference
    {
        public string lane { get; set; }
        public long gameId { get; set; }
        public int champion { get; set; }
        public string platformId { get; set; }
        public int season { get; set; }
        public int queue { get; set; }
        public string role { get; set; }
        public long timestamp { get; set; }
    }
    public class ChampionStatic
    {
        internal ChampionStatic() { }

        [JsonProperty("allytips")]
        public List<string> AllyTips { get; set; }

        [JsonProperty("blurb")]
        public string Blurb { get; set; }


        [JsonProperty("enemytips")]
        public List<string> EnemyTips { get; set; }


        [JsonProperty("id")]
        public int Id { get; set; }


        [JsonProperty("image")]
        public ImageStatic Image { get; set; }


        [JsonProperty("info")]
        public InfoStatic Info { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }


        [JsonProperty("lore")]
        public string Lore { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("partype")]
        public string Partype { get; set; }

        [JsonProperty("passive")]
        public PassiveStatic Passive { get; set; }

        [JsonProperty("recommended")]
        public List<RecommendedStatic> RecommendedItems { get; set; }

        [JsonProperty("skins")]
        public List<SkinStatic> Skins { get; set; }

        [JsonProperty("spells")]
        public List<ChampionSpellStatic> Spells { get; set; }


        [JsonProperty("stats")]
        public ChampionStatsStatic Stats { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
    public class InfoStatic
    {
        internal InfoStatic() { }

        /// <summary>
        /// Number between 1 and 10 representing the attack power of a champion.
        /// </summary>
        [JsonProperty("attack")]
        public int Attack { get; set; }

        /// <summary>
        /// Number between 1 and 10 representing the defense power of a champion.
        /// </summary>
        [JsonProperty("defense")]
        public int Defense { get; set; }

        /// <summary>
        /// Number between 1 and 10 representing the difficulty of a champion.
        /// </summary>
        [JsonProperty("difficulty")]
        public int Difficulty { get; set; }

        /// <summary>
        /// Number between 1 and 10 representing the magic power of a champion.
        /// </summary>
        [JsonProperty("magic")]
        public int Magic { get; set; }
    }
    public class ChampionStatsStatic
    {
        internal ChampionStatsStatic() { }

        /// <summary>
        /// Base armor.
        /// </summary>
        [JsonProperty("armor")]
        public double Armor { get; set; }

        /// <summary>
        /// Armor won per level.
        /// </summary>
        [JsonProperty("armorperlevel")]
        public double ArmorPerLevel { get; set; }

        /// <summary>
        /// Base attack damage.
        /// </summary>
        [JsonProperty("attackdamage")]
        public double AttackDamage { get; set; }

        /// <summary>
        /// Attack damage won per level.
        /// </summary>
        [JsonProperty("attackdamageperlevel")]
        public double AttackDamagePerLevel { get; set; }

        /// <summary>
        /// Base attack range.
        /// </summary>
        [JsonProperty("attackrange")]
        public double AttackRange { get; set; }

        /// <summary>
        /// Base attack speed.
        /// </summary>
        [JsonProperty("attackspeedoffset")]
        public double AttackSpeedOffset { get; set; }

        /// <summary>
        /// Attack speed won per level.
        /// </summary>
        [JsonProperty("attackspeedperlevel")]
        public double AttackSpeedPerLevel { get; set; }

        /// <summary>
        /// Base crit percentage.
        /// </summary>
        [JsonProperty("crit")]
        public double Crit { get; set; }

        /// <summary>
        /// Crit percentage won per level.
        /// </summary>
        [JsonProperty("critperlevel")]
        public double CritPerLevel { get; set; }

        /// <summary>
        /// Base hit points.
        /// </summary>
        [JsonProperty("hp")]
        public double Hp { get; set; }

        /// <summary>
        /// Hit points won per level.
        /// </summary>
        [JsonProperty("hpperlevel")]
        public double HpPerLevel { get; set; }

        /// <summary>
        /// Base hit point regeneration.
        /// </summary>
        [JsonProperty("hpregen")]
        public double HpRegen { get; set; }

        /// <summary>
        /// Hit points regeneration per level.
        /// </summary>
        [JsonProperty("hpregenperlevel")]
        public double HpRegenPerLevel { get; set; }

        /// <summary>
        /// Base move speed.
        /// </summary>
        [JsonProperty("movespeed")]
        public double MoveSpeed { get; set; }

        /// <summary>
        /// Base mana points.
        /// </summary>
        [JsonProperty("mp")]
        public double Mp { get; set; }

        /// <summary>
        /// Mana points won per level.
        /// </summary>
        [JsonProperty("mpperlevel")]
        public double MpPerLevel { get; set; }

        /// <summary>
        /// Base mana point regeneration.
        /// </summary>
        [JsonProperty("mpregen")]
        public double MpRegen { get; set; }

        /// <summary>
        /// Mana point regeneration won per level.
        /// </summary>
        [JsonProperty("mpregenperlevel")]
        public double MpRegenPerLevel { get; set; }

        /// <summary>
        /// Base spell block.
        /// </summary>
        [JsonProperty("spellblock")]
        public double SpellBlock { get; set; }

        /// <summary>
        /// Spell block won per level.
        /// </summary>
        [JsonProperty("spellblockperlevel")]
        public double SpellBlockPerLevel { get; set; }
    }
    public class PassiveStatic
    {
        internal PassiveStatic() { }

        /// <summary>
        /// String descripting the passive.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Image of the passive.
        /// </summary>
        [JsonProperty("image")]
        public ImageStatic Image { get; set; }

        /// <summary>
        /// Name of the passive.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Sanitized (HTML stripped) description of the passive.
        /// </summary>
        [JsonProperty("sanitizedDescription")]
        public string SanitizedDescription { get; set; }
    }
    public class RecommendedStatic
    {
        internal RecommendedStatic() { }

        /// <summary>
        /// List of recommended items ordered by block.
        /// </summary>
        [JsonProperty("blocks")]
        public List<BlockStatic> Blocks { get; set; }

        /// <summary>
        /// Name of the champion for which those items are recommended.
        /// </summary>
        [JsonProperty("champion")]
        public string Champion { get; set; }

        /// <summary>
        /// Map id for which those items are recommended.
        /// <list type="table">
        /// <listheader><description>Possible values:</description></listheader>
        /// <item><term>1</term><description>Summoner's Rift: Summer Variant</description></item>
        /// <item><term>2</term><description>Summoner's Rift: Autumn Variant</description></item>
        /// <item><term>3</term><description>The Proving Grounds: Tutorial Map</description></item>
        /// <item><term>4</term><description>Twisted Treeline: Original Version</description></item>
        /// <item><term>8</term><description>The Crystal Scar: Dominion Map</description></item>
        /// <item><term>10</term><description>Twisted Treeline: Current Version</description></item>
        /// <item><term>12</term><description>Howling Abyss: ARAM Map</description></item>
        /// </list>
        /// </summary>
        [JsonProperty("map")]
        public string Map { get; set; }

        /// <summary>
        /// Mode for which those items are recommended.
        /// </summary>
        [JsonProperty("mode")]
        public string Mode { get; set; }

        /// <summary>
        /// Priority of the recommended items list.
        /// <para>This is default false for each Riot page.
        /// This means players' lists will normally display when a game starts instead of Riots' pages.</para>
        /// </summary>
        [JsonProperty("priority")]
        public bool Priority { get; set; }

        /// <summary>
        /// Title of the items list.
        /// <para>(eg: Beginner / riot-beginner / VladimirHA / VladimirDM / ...)</para>
        /// <para>Later if costum sets are server side, we might be able to request these too.</para>
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Type of list.
        /// <para>(eg: riot-beginner / riot)</para>
        /// <para>This is probably to find out who's list it is. (Riot's' or a players')</para>
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
    }
    public class SkinStatic
    {
        internal SkinStatic() { }

        /// <summary>
        /// Id of the skin.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Name of the skin.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Ordered number of the skin.
        /// </summary>
        [JsonProperty("num")]
        public int Num { get; set; }
    }
    public class LevelTipStatic
    {
        internal LevelTipStatic() { }

        /// <summary>
        /// List of string representing the effects of leveling up this spell (going from a percentage
        /// to another for example.
        /// </summary>
        [JsonProperty("effect")]
        public List<string> Effects { get; set; }

        /// <summary>
        /// List of string representing which stats will be affected by leveling up this spell.
        /// </summary>
        [JsonProperty("label")]
        public List<string> Labels { get; set; }
    }
    public class BlockItemStatic
    {
        internal BlockItemStatic() { }

        /// <summary>
        /// Recommended count.
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }

        /// <summary>
        /// Id of the recommended item.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
    }
    public class ChampionSpellStatic
    {
        internal ChampionSpellStatic() { }

        /// <summary>
        /// List of alternative images.
        /// </summary>
        [JsonProperty("altimages")]
        public List<ImageStatic> Altimages { get; set; }

        /// <summary>
        /// List of the cooldowns for each level of the spell.
        /// </summary>
        [JsonProperty("cooldown")]
        public List<float> Cooldowns { get; set; }

        /// <summary>
        /// String representing the cooldowns for each level of the spell.
        /// </summary>
        [JsonProperty("cooldownBurn")]
        public string CooldownBurn { get; set; }

        /// <summary>
        /// List of the costs for each level of the spell.
        /// </summary>
        [JsonProperty("cost")]
        public List<int> Costs { get; set; }

        /// <summary>
        /// String representing the costs for each level of the spell.
        /// </summary>
        [JsonProperty("costBurn")]
        public string CostBurn { get; set; }

        /// <summary>
        /// Type of cost (mana, energy, percentage of current health, etc).
        /// </summary>
        [JsonProperty("costType")]
        public string CostType { get; set; }

        /// <summary>
        /// Description of the spell.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Effects of the spell (damage, etc). This field is a List of List of Integer.
        /// </summary>
        [JsonProperty("effect")]
        public List<List<double>> Effects { get; set; }

        /// <summary>
        /// String representing the effects of the spell.
        /// </summary>
        [JsonProperty("effectBurn")]
        public List<string> EffectBurns { get; set; }

        /// <summary>
        /// Image of the spell.
        /// </summary>
        [JsonProperty("image")]
        public ImageStatic Image { get; set; }

        /// <summary>
        ///  String identifying a spell (champion's name + key to activate the spell, example: "AatroxQ".
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// Tooltip when leveling up this spell.
        /// </summary>
        [JsonProperty("levelTip")]
        public LevelTipStatic LevelTip { get; set; }

        /// <summary>
        /// Maximum rank of this spell.
        /// </summary>
        [JsonProperty("maxRank")]
        public int MaxRank { get; set; }

        /// <summary>
        /// Name of this spell.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// This field is either a List of Integer or the String 'self' for spells that target one's own champion.
        /// </summary>
        [JsonProperty("range")]
        public object Range { get; set; }

        /// <summary>
        /// String representing the range for each level of the spell.
        /// </summary>
        [JsonProperty("rangeBurn")]
        public string RangeBurn { get; set; }

        /// <summary>
        /// String representing the cost for the champion when using this spell (example: "{{ e3 }}% of Current
        /// Health".
        /// </summary>
        [JsonProperty("resource")]
        public string Resource { get; set; }

        /// <summary>
        /// Sanitized (HTML stripped) description of the spell.
        /// </summary>
        [JsonProperty("sanitizedDescription")]
        public string SanitizedDescription { get; set; }

        /// <summary>
        /// Sanitized (HTML stripped) tooltip of the spell.
        /// </summary>
        [JsonProperty("sanitizedTooltip")]
        public string SanitizedTooltip { get; set; }

        /// <summary>
        /// Tooltip for this spell.
        /// </summary>
        [JsonProperty("tooltip")]
        public string Tooltip { get; set; }

        /// <summary>
        /// Various effects of this spell.
        /// </summary>
        [JsonProperty("vars")]
        public List<SpellVarsStatic> Vars { get; set; }
    }
    public class BlockStatic
    {
        internal BlockStatic() { }

        /// <summary>
        /// List of recommended items.
        /// </summary>
        [JsonProperty("items")]
        public List<BlockItemStatic> Items { get; set; }

        /// <summary>
        /// Rec math.
        /// </summary>
        [JsonProperty("recMath")]
        public bool RecMath { get; set; }

        /// <summary>
        /// Type of items (starting, essential, offensive, etc).
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
    }
    public class ImageStatic
    {
        internal ImageStatic() { }

        /// <summary>
        /// Full name for this image.
        /// </summary>
        [JsonProperty("full")]
        public string Full { get; set; }

        /// <summary>
        /// Image's group (spell, champion, item, etc).
        /// </summary>
        [JsonProperty("group")]
        public string Group { get; set; }

        /// <summary>
        /// Image's height.
        /// </summary>
        [JsonProperty("h")]
        public int Height { get; set; }

        /// <summary>
        /// Image's sprite.
        /// </summary>
        [JsonProperty("sprite")]
        public string Sprite { get; set; }

        /// <summary>
        /// Image's width.
        /// </summary>
        [JsonProperty("w")]
        public int Width { get; set; }

        /// <summary>
        /// X starting point for this image.
        /// </summary>
        [JsonProperty("x")]
        public int X { get; set; }

        /// <summary>
        /// Y starting point for this image.
        /// </summary>
        [JsonProperty("y")]
        public int Y { get; set; }
    }
    public class SpellVarsStatic
    {
        internal SpellVarsStatic() { }

        /// <summary>
        /// Coeff for this summoner spell for the summoner's level.
        /// </summary>
        [JsonProperty("coeff")]
        public object Coeff { get; set; }

        /// <summary>
        /// Seems to always be equal to + when it is present.
        /// </summary>
        [JsonProperty("dyn")]
        public string Dyn { get; set; }

        /// <summary>
        /// Key.
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// Link.
        /// </summary>
        [JsonProperty("link")]
        public string Link { get; set; }

        /// <summary>
        /// Ranks with.
        /// </summary>
        [JsonProperty("ranksWith")]
        public string RanksWith { get; set; }
    }
    public class ChampionListStatic
    {
        internal ChampionListStatic() { }

        /// <summary>
        /// Map of champions indexed by their name.
        /// </summary>
        [JsonProperty("data")]
        public Dictionary<string, ChampionStatic> Champions { get; set; }

        /// <summary>
        /// Format of the data retrieved (always null afaik).
        /// </summary>
        [JsonProperty("format")]
        public string Format { get; set; }

        /// <summary>
        /// Map of the champions names indexed by their id.
        /// </summary>
        [JsonProperty("keys")]
        public Dictionary<int, string> Keys { get; set; }

        /// <summary>
        /// TAPI type (item).
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Version of the API.
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }
    }
    public class BannedChampion
    {
        internal BannedChampion() { }

        /// <summary>
        /// Banned champion ID.
        /// </summary>
        [JsonProperty("championId")]
        public int ChampionId { get; set; }

        /// <summary>
        /// Turn during which the champion was banned.
        /// </summary>
        [JsonProperty("pickTurn")]
        public int PickTurn { get; set; }
    }
    public class Event
    {
        internal Event() { }

        /// <summary>
        /// The ascended type of the event. Only present if relevant.
        /// Note that CLEAR_ASCENDED refers to when a participants kills the ascended player.
        /// </summary>
        [JsonProperty("ascendedType")]
        public string AscendedType { get; set; }

        /// <summary>
        /// The assisting participant IDs of the event. Only present if relevant.
        /// </summary>
        [JsonProperty("assistingParticipantIds")]
        public List<int> AssistingParticipantIds { get; set; }

        /// <summary>
        /// The building type of the event (tower or inhibitor). Only present if relevant.
        /// </summary>
        [JsonProperty("buildingType")]
        public string BuildingType { get; set; }

        /// <summary>
        /// The creator ID of the event. Only present if relevant.
        /// </summary>
        [JsonProperty("creatorId")]
        public int CreatorId { get; set; }

        /// <summary>
        /// Event type (building kills, champion kills, ward placements, items purchases, etc).
        /// </summary>
        [JsonProperty("eventType")]
        public string EventType { get; set; }

        /// <summary>
        /// The ending item ID of the event. Only present if relevant.
        /// </summary>
        [JsonProperty("itemAfter")]
        public int ItemAfter { get; set; }

        /// <summary>
        /// The starting item ID of the event. Only present if relevant.
        /// </summary>
        [JsonProperty("itemBefore")]
        public int ItemBefore { get; set; }

        /// <summary>
        /// The item ID of the event. Only present if relevant.
        /// </summary>
        [JsonProperty("itemId")]
        public int ItemId { get; set; }

        /// <summary>
        /// The killer ID of the event. Only present if relevant.
        /// </summary>
        [JsonProperty("killerId")]
        public int KillerId { get; set; }

        /// <summary>
        /// The lane type of the event. Only present if relevant.
        /// </summary>
        [JsonProperty("laneType")]
        public string LaneType { get; set; }

        /// <summary>
        /// The level up type of the event. Only present if relevant.
        /// </summary>
        [JsonProperty("levelUpType")]
        public string LevelUpType { get; set; }

        /// <summary>
        /// The monster type of the event. Only present if relevant.
        /// </summary>
        [JsonProperty("monsterType")]
        public string MonsterType { get; set; }

        /// <summary>
        /// The monster type of the event. Only present if relevant.
        /// </summary>
        [JsonProperty("monsterSubType")]
        public string MonsterSubType { get; set; }

        /// <summary>
        /// The participant ID of the event. Only present if relevant.
        /// </summary>
        [JsonProperty("participantId")]
        public int ParticipantId { get; set; }

        /// <summary>
        /// The point captured in the event. Only present if relevant.
        /// </summary>
        [JsonProperty("pointCaptured")]
        public string CapturedPoint { get; set; }

        /// <summary>
        /// The position of the event. Only present if relevant.
        /// </summary>
        [JsonProperty("position")]
        public Position Position { get; set; }

        /// <summary>
        /// The skill slot of the event. Only present if relevant.
        /// </summary>
        [JsonProperty("skillSlot")]
        public int SkillSlot { get; set; }

        /// <summary>
        /// The team ID of the event. Only present if relevant.
        /// </summary>
        [JsonProperty("teamId")]
        public int TeamId { get; set; }

        /// <summary>
        /// Represents how much time into the game the event occurred.
        /// </summary>
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        /// <summary>
        /// The tower type of the event. Only present if relevant.
        /// </summary>
        [JsonProperty("towerType")]
        public string TowerType { get; set; }

        /// <summary>
        /// The victim ID of the event. Only present if relevant.
        /// </summary>
        [JsonProperty("victimId")]
        public int VictimId { get; set; }

        /// <summary>
        /// The ward type of the event. Only present if relevant.
        /// </summary>
        [JsonProperty("wardType")]
        public string WardType { get; set; }
    }
    public class Frame
    {
        internal Frame() { }

        /// <summary>
        /// List of events for this frame.
        /// </summary>
        [JsonProperty("events")]
        public List<Event> Events { get; set; }

        /// <summary>
        /// Map of each participant ID to the participant's information for the frame.
        /// </summary>
        [JsonProperty("participantFrames")]
        public Dictionary<string, ParticipantFrame> ParticipantFrames { get; set; }

        /// <summary>
        /// Represents how much time into the game the frame occurred.
        /// </summary>
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }
    public class Mastery
    {
        internal Mastery() { }

        /// <summary>
        /// Mastery ID.
        /// </summary>
        [JsonProperty("masteryId")]
        public int MasteryId { get; set; }

        /// <summary>
        /// Mastery rank.
        /// </summary>
        [JsonProperty("rank")]
        public int Rank { get; set; }
    }
    public class Match
    {
        /// <summary>
        /// The season ID.
        /// </summary>
        [JsonProperty("seasonId")]
        public int SeasonId { get; set; }

        [JsonProperty("queueId")]
        public int QueueId { get; set; }

        /// <summary>
        /// Equivalent to match id
        /// </summary>
        [JsonProperty("gameId")]
        public long GameId { get; set; }

        /// <summary>
        /// The participants identities.
        /// </summary>
        [JsonProperty("participantIdentities")]
        public List<ParticipantIdentity> ParticipantIdentities { get; set; }

        /// <summary>
        /// The game version.
        /// </summary>
        [JsonProperty("gameVersion")]
        public string GameVersion { get; set; }

        /// <summary>
        /// The game mode.
        /// </summary>
        [JsonProperty("gameMode")]
        public string GameMode { get; set; }

        /// <summary>
        /// The map ID.
        /// </summary>
        [JsonProperty("MapId")]
        public int MapId { get; set; }

        /// <summary>
        /// The game type.
        /// </summary>
        [JsonProperty("gameType")]
        public string GameType { get; set; }

        /// <summary>
        /// The teams.
        /// </summary>
        [JsonProperty("teams")]
        public List<TeamStats> Teams { get; set; }

        /// <summary>
        /// The participants.
        /// </summary>
        [JsonProperty("participants")]
        public List<Participant> Participants { get; set; }

        /// <summary>
        /// The game duration.
        /// </summary>
        [JsonProperty("gameDuration")]
        public long GameDuration { get; set; }

        /// <summary>
        /// The date time of the game creation.
        /// </summary>
        [JsonProperty("gameCreation")]
        public long GameCreation { get; set; }
    }
    public class MatchDetail : MatchSummary
    {
        internal MatchDetail() { }

        /// <summary>
        /// Team information.
        /// </summary>
        [JsonProperty("teams")]
        public List<TeamStats> Teams { get; set; }

        /// <summary>
        /// Match timeline data. Not included by default.
        /// </summary>
        [JsonProperty("timeline")]
        public Timeline Timeline { get; set; }
    }
    public class MatchSummary
    {
        internal MatchSummary() { }

        /// <summary>
        /// Map type.
        /// </summary>
        [JsonProperty("mapId")]
        public string MapType { get; set; }

        /// <summary>
        /// Match creation time. Designates when the team select lobby is created and/or the match is made through
        /// match making, not when the game actually starts.
        /// </summary>
        [JsonProperty("matchCreation")]
        public long MatchCreation { get; set; }

        /// <summary>
        /// Match duration.
        /// </summary>
        [JsonProperty("matchDuration")]
        public long MatchDuration { get; set; }

        /// <summary>
        /// Match ID.
        /// </summary>
        [JsonProperty("matchId")]
        public long MatchId { get; set; }

        /// <summary>
        /// Match mode.
        /// </summary>
        [JsonProperty("matchMode")]
        public string MatchMode { get; set; }

        [JsonProperty("matchType")]
        public string MatchType { get; set; }

        /// <summary>
        /// Match version.
        /// </summary>
        [JsonProperty("matchVersion")]
        public string MatchVersion { get; set; }

        /// <summary>
        /// Participants identity information.
        /// </summary>
        [JsonProperty("participantIdentities")]
        public List<ParticipantIdentity> ParticipantIdentities { get; set; }

        /// <summary>
        /// Participants information
        /// </summary>
        [JsonProperty("participants")]
        public List<Participant> Participants { get; set; }

        /// <summary>
        /// Match queue type.
        /// </summary>
        [JsonProperty("queueType")]
        public string QueueType { get; set; }

        /// <summary>
        /// Region where the match was played.
        /// </summary>
        [JsonProperty("region")]
        public string Region { get; set; }

        /// <summary>
        /// Season match was played.
        /// </summary>
        [JsonProperty("season")]
        public string Season { get; set; }
    }
    public class Participant
    {
        internal Participant() { }

        /// <summary>
        /// Champion ID.
        /// </summary>
        [JsonProperty("championId")]
        public int ChampionId { get; set; }

        /// <summary>
        /// List of mastery information.
        /// </summary>
        [JsonProperty("masteries")]
        public List<Mastery> Masteries { get; set; }

        /// <summary>
        /// Participant ID.
        /// </summary>
        [JsonProperty("participantId")]
        public int ParticipantId { get; set; }

        /// <summary>
        /// List of rune information.
        /// </summary>
        [JsonProperty("runes")]
        public List<Rune> Runes { get; set; }

        /// <summary>
        /// First summoner spell ID.
        /// </summary>
        [JsonProperty("spell1Id")]
        public int Spell1Id { get; set; }

        /// <summary>
        /// Second summoner spell ID.
        /// </summary>
        [JsonProperty("spell2Id")]
        public int Spell2Id { get; set; }

        /// <summary>
        /// Participant statistics.
        /// </summary>
        [JsonProperty("stats")]
        public ParticipantStats Stats { get; set; }

        /// <summary>
        /// Team ID.
        /// </summary>
        [JsonProperty("teamId")]
        public int TeamId { get; set; }

        /// <summary>
        /// Timeline data.
        /// </summary>
        [JsonProperty("timeline")]
        public ParticipantTimeline Timeline { get; set; }

        /// <summary>
        /// Highest achieved season tier.
        /// </summary>
        [JsonProperty("highestAchievedSeasonTier")]
        public string HighestAchievedSeasonTier { get; set; }
    }
    public class ParticipantFrame
    {
        internal ParticipantFrame() { }

        /// <summary>
        /// Participant's current gold.
        /// </summary>
        [JsonProperty("currentGold")]
        public int CurrentGold { get; set; }

        /// <summary>
        /// Number of jungle minions killed by participant.
        /// </summary>
        [JsonProperty("jungleMinionsKilled")]
        public int JungleMinionsKilled { get; set; }

        /// <summary>
        /// Participant's current level.
        /// </summary>
        [JsonProperty("level")]
        public int Level { get; set; }

        /// <summary>
        /// Number of minions killed by participant.
        /// </summary>
        [JsonProperty("minionsKilled")]
        public int MinionsKilled { get; set; }

        /// <summary>
        /// Participant ID.
        /// </summary>
        [JsonProperty("participantId")]
        public int ParticipantId { get; set; }

        /// <summary>
        /// Participant's position.
        /// </summary>
        [JsonProperty("position")]
        public Position Position { get; set; }

        /// <summary>
        /// Participant's total gold.
        /// </summary>
        [JsonProperty("totalGold")]
        public int TotalGold { get; set; }

        /// <summary>
        /// Experience earned by participant.
        /// </summary>
        [JsonProperty("xp")]
        public int XP { get; set; }
    }
    public class ParticipantIdentity
    {
        internal ParticipantIdentity() { }

        /// <summary>
        /// Participant ID.
        /// </summary>
        [JsonProperty("participantId")]
        public int ParticipantId { get; set; }

        /// <summary>
        /// Player information.
        /// </summary>
        [JsonProperty("player")]
        public Player Player { get; set; }
    }
    public class Player
    {
        internal Player() { }

        /// <summary>
        /// Current platform ID.
        /// </summary>
        [JsonProperty("currentPlatformId")]
        public string CurrentPlatformId { get; set; }

        /// <summary>
        /// Platform ID.
        /// </summary>
        [JsonProperty("platformId")]
        public string PlatformId { get; set; }

        /// <summary>
        /// Match history URI.
        /// </summary>
        [JsonProperty("matchHistoryUri")]
        public string MatchHistoryUri { get; set; }

        /// <summary>
        /// Profile icon ID.
        /// </summary>
        [JsonProperty("profileIcon")]
        public int ProfileIcon { get; set; }

        /// <summary>
        /// Current account ID.
        /// </summary>
        [JsonProperty("currentAccountId")]
        public long CurrentAccountId { get; set; }

        /// <summary>
        /// Account ID.
        /// </summary>
        [JsonProperty("accountId")]
        public long AccountId { get; set; }

        /// <summary>
        /// Summoner ID.
        /// </summary>
        [JsonProperty("summonerId")]
        public long SummonerId { get; set; }

        /// <summary>
        /// Summoner name.
        /// </summary>
        [JsonProperty("summonerName")]
        public string SummonerName { get; set; }
    }
    public class ParticipantTimelineData
    {
        internal ParticipantTimelineData() { }

        /// <summary>
        /// Value per minute from 10 min to 20 min.
        /// </summary>
        [JsonProperty("tenToTwenty")]
        public double TenToTwenty { get; set; }

        /// <summary>
        /// Value per minute from 30 min to the end of the game.
        /// </summary>
        [JsonProperty("thirtyToEnd")]
        public double ThirtyToEnd { get; set; }

        /// <summary>
        /// Value per minute from 20 min to 30 min.
        /// </summary>
        [JsonProperty("twentyToThirty")]
        public double TwentyToThirty { get; set; }

        /// <summary>
        /// Value per minute from the beginning of the game to 10 min.
        /// </summary>
        [JsonProperty("zeroToTen")]
        public double ZeroToTen { get; set; }
    }
    public class ParticipantTimeline
    {
        /// <summary>
        /// The lane of the participant.
        /// </summary>
        [JsonProperty("lane")]
        public string Lane { get; set; }

        /// <summary>
        /// The role of the participant.
        /// </summary>
        [JsonProperty("role")]
        public string Role { get; set; }

        /// <summary>
        /// The participant ID.
        /// </summary>
        [JsonProperty("participantId")]
        public int ParticipantId { get; set; }

        [JsonProperty("goldPerMinDeltas")]
        public Dictionary<string, double> GoldPerMinDeltas { get; set; }

        [JsonProperty("xpDiffPerMinDeltas")]
        public Dictionary<string, double> XpDiffPerMinDeltas { get; set; }

        [JsonProperty("xpPerMinDeltas")]
        public Dictionary<string, double> XpPerMinDeltas { get; set; }

        [JsonProperty("csDiffPerMinDeltas")]
        public Dictionary<string, double> CsDiffPerMinDeltas { get; set; }

        [JsonProperty("creepsPerMinDeltas")]
        public Dictionary<string, double> CreepsPerMinDeltas { get; set; }

        [JsonProperty("damageTakenDiffPerMinDeltas")]
        public Dictionary<string, double> DamageTakenDiffPerMinDeltas { get; set; }

        [JsonProperty("damageTakenPerMinDeltas")]
        public Dictionary<string, double> DamageTakenPerMinDeltas { get; set; }
    }
    public class ParticipantStats
    {
        internal ParticipantStats() { }

        /// <summary>
        /// Number of assists.
        /// </summary>
        [JsonProperty("assists")]
        public long Assists { get; set; }

        /// <summary>
        /// Champion level achieved.
        /// </summary>
        [JsonProperty("champLevel")]
        public long ChampLevel { get; set; }

        /// <summary>
        /// If game was a dominion game, player's combat score, otherwise 0.
        /// </summary>
        [JsonProperty("combatPlayerScore")]
        public long CombatPlayerScore { get; set; }

        /// <summary>
        /// Number of deaths.
        /// </summary>
        [JsonProperty("deaths")]
        public long Deaths { get; set; }

        /// <summary>
        /// Number of double kills.
        /// </summary>
        [JsonProperty("doubleKills")]
        public long DoubleKills { get; set; }

        /// <summary>
        /// Flag indicating if participant got an assist on first blood.
        /// </summary>
        [JsonProperty("firstBloodAssist")]
        public bool FirstBloodAssist { get; set; }

        /// <summary>
        /// Flag indicating if participant got first blood.
        /// </summary>
        [JsonProperty("firstBloodKill")]
        public bool FirstBloodKill { get; set; }

        /// <summary>
        /// Flag indicating if participant got an assist on the first inhibitor.
        /// </summary>
        [JsonProperty("firstInhibitorAssist")]
        public bool FirstInhibitorAssist { get; set; }

        /// <summary>
        /// Flag indicating if participant destroyed the first inhibitor.
        /// </summary>
        [JsonProperty("firstInhibitorKill")]
        public bool FirstInhibitorKill { get; set; }

        /// <summary>
        /// Flag indicating if participant got an assist on the first tower.
        /// </summary>
        [JsonProperty("firstTowerAssist")]
        public bool FirstTowerAssist { get; set; }

        /// <summary>
        /// Flag indicating if participant destroyed the first tower.
        /// </summary>
        [JsonProperty("firstTowerKill")]
        public bool FirstTowerKill { get; set; }

        /// <summary>
        /// Gold earned.
        /// </summary>
        [JsonProperty("goldEarned")]
        public long GoldEarned { get; set; }

        /// <summary>
        /// Gold spent.
        /// </summary>
        [JsonProperty("goldSpent")]
        public long GoldSpent { get; set; }

        /// <summary>
        /// Numer of inhibitor kills.
        /// </summary>
        [JsonProperty("inhibitorKills")]
        public long InhibitorKills { get; set; }

        /// <summary>
        /// First item ID.
        /// </summary>
        [JsonProperty("item0")]
        public long Item0 { get; set; }

        /// <summary>
        /// Second item ID.
        /// </summary>
        [JsonProperty("item1")]
        public long Item1 { get; set; }

        /// <summary>
        /// Third item ID.
        /// </summary>
        [JsonProperty("item2")]
        public long Item2 { get; set; }

        /// <summary>
        /// Fourth item ID.
        /// </summary>
        [JsonProperty("item3")]
        public long Item3 { get; set; }

        /// <summary>
        /// Fifth item ID.
        /// </summary>
        [JsonProperty("item4")]
        public long Item4 { get; set; }

        /// <summary>
        /// Sixth item ID.
        /// </summary>
        [JsonProperty("item5")]
        public long Item5 { get; set; }

        /// <summary>
        /// Seventh item ID.
        /// </summary>
        [JsonProperty("item6")]
        public long Item6 { get; set; }

        /// <summary>
        /// Number of killing sprees.
        /// </summary>
        [JsonProperty("killingSprees")]
        public long KillingSprees { get; set; }

        /// <summary>
        /// Number of kills.
        /// </summary>
        [JsonProperty("kills")]
        public long Kills { get; set; }

        /// <summary>
        /// Largest critical strike.
        /// </summary>
        [JsonProperty("largestCriticalStrike")]
        public long LargestCriticalStrike { get; set; }

        /// <summary>
        /// Largest killing spree.
        /// </summary>
        [JsonProperty("largestKillingSpree")]
        public long LargestKillingSpree { get; set; }

        /// <summary>
        /// Largest multi kill.
        /// </summary>
        [JsonProperty("largestMultiKill")]
        public long LargestMultiKill { get; set; }

        /// <summary>
        /// Magic damage dealt.
        /// </summary>
        [JsonProperty("magicDamageDealt")]
        public long MagicDamageDealt { get; set; }

        /// <summary>
        /// Magic damage dealt to champions.
        /// </summary>
        [JsonProperty("magicDamageDealtToChampions")]
        public long MagicDamageDealtToChampions { get; set; }

        /// <summary>
        /// Magic damage taken.
        /// </summary>
        [JsonProperty("magicDamageTaken")]
        public long MagicDamageTaken { get; set; }

        /// <summary>
        /// Minions kiled.
        /// </summary>
        [JsonProperty("minionsKilled")]
        public long MinionsKilled { get; set; }

        /// <summary>
        /// Neutral minions killed.
        /// </summary>
        [JsonProperty("neutralMinionsKilled")]
        public long NeutralMinionsKilled { get; set; }

        /// <summary>
        /// Neutral jungle minions killed in the enemy team's jungle.
        /// </summary>
        [JsonProperty("neutralMinionsKilledEnemyJungle")]
        public long NeutralMinionsKilledEnemyJungle { get; set; }

        /// <summary>
        /// Neutral jungle minions killed in your team's jungle.
        /// </summary>
        [JsonProperty("neutralMinionsKilledTeamJungle")]
        public long NeutralMinionsKilledJungle { get; set; }

        /// <summary>
        /// If game was a dominion game, number of node captures.
        /// </summary>
        [JsonProperty("nodeCapture")]
        public long NodeCapture { get; set; }

        /// <summary>
        /// If game was a dominion game, number of node capture assists.
        /// </summary>
        [JsonProperty("nodeCaptureAssist")]
        public long NodeCaptureAssist { get; set; }

        /// <summary>
        /// If game was a dominion game, number of node neutralizations.
        /// </summary>
        [JsonProperty("nodeNeutralize")]
        public long NodeNeutralize { get; set; }

        /// <summary>
        /// If game was a dominion game, number of node neutralization assists.
        /// </summary>
        [JsonProperty("nodeNeutralizeAssist")]
        public long NodeNeutralizeAssist { get; set; }

        /// <summary>
        /// If game was a dominion game, player's objectives score, otherwise 0.
        /// </summary>
        [JsonProperty("objectivePlayerScore")]
        public long ObjectivePlayerScore { get; set; }

        /// <summary>
        /// Number of penta kills.
        /// </summary>
        [JsonProperty("pentaKills")]
        public long PentaKills { get; set; }

        /// <summary>
        /// Physical damage dealt.
        /// </summary>
        [JsonProperty("physicalDamageDealt")]
        public long PhysicalDamageDealt { get; set; }

        /// <summary>
        /// Physical damage dealt to champions.
        /// </summary>
        [JsonProperty("physicalDamageDealtToChampions")]
        public long PhysicalDamageDealtToChampions { get; set; }

        /// <summary>
        /// Physical damage taken.
        /// </summary>
        [JsonProperty("physicalDamageTaken")]
        public long PhysicalDamageTaken { get; set; }

        /// <summary>
        /// Number of quadra kills.
        /// </summary>
        [JsonProperty("quadraKills")]
        public long QuadraKills { get; set; }

        /// <summary>
        /// Number of sight wards purchased.
        /// </summary>
        [JsonProperty("sightWardsBoughtInGame")]
        public long SightWardsBoughtInGame { get; set; }

        /// <summary>
        /// If game was a dominion game, number of completed team objectives (i.e., quests).
        /// </summary>
        [JsonProperty("teamObjective")]
        public long TeamObjective { get; set; }

        /// <summary>
        /// Total damage dealt.
        /// </summary>
        [JsonProperty("totalDamageDealt")]
        public long TotalDamageDealt { get; set; }

        /// <summary>
        /// Total damage dealt to champions.
        /// </summary>
        [JsonProperty("totalDamageDealtToChampions")]
        public long TotalDamageDealtToChampions { get; set; }

        /// <summary>
        /// Total damage taken.
        /// </summary>
        [JsonProperty("totalDamageTaken")]
        public long TotalDamageTaken { get; set; }

        /// <summary>
        /// Total heal.
        /// </summary>
        [JsonProperty("totalHeal")]
        public long TotalHeal { get; set; }

        /// <summary>
        /// If game was a dominion game, player's total score, otherwise 0.
        /// </summary>
        [JsonProperty("totalPlayerScore")]
        public long TotalPlayerScore { get; set; }

        /// <summary>
        /// If game was a dominion game, team rank of the player's total score (e.g., 1-5).
        /// </summary>
        [JsonProperty("totalScoreRank")]
        public long TotalScoreRank { get; set; }

        /// <summary>
        /// Total time crowd control dealt.
        /// </summary>
        [JsonProperty("totalTimeCrowdControlDealt")]
        public long TotalTimeCrowdControlDealt { get; set; }

        /// <summary>
        /// Total units healed.
        /// </summary>
        [JsonProperty("totalUnitsHealed")]
        public long TotalUnitsHealed { get; set; }

        /// <summary>
        /// Number of tower kills.
        /// </summary>
        [JsonProperty("towerKills")]
        public long TowerKills { get; set; }

        /// <summary>
        /// Number of triple kills.
        /// </summary>
        [JsonProperty("tripleKills")]
        public long TripleKills { get; set; }

        /// <summary>
        /// True damage dealt.
        /// </summary>
        [JsonProperty("trueDamageDealt")]
        public long TrueDamageDealt { get; set; }

        /// <summary>
        /// True damage dealt to champions.
        /// </summary>
        [JsonProperty("trueDamageDealtToChampions")]
        public long TrueDamageDealtToChampions { get; set; }

        /// <summary>
        /// True damage taken.
        /// </summary>
        [JsonProperty("trueDamageTaken")]
        public long TrueDamageTaken { get; set; }

        /// <summary>
        /// Number of unreal kills.
        /// </summary>
        [JsonProperty("unrealKills")]
        public long UnrealKills { get; set; }

        /// <summary>
        /// Number of vision wards purchased.
        /// </summary>
        [JsonProperty("visionWardsBoughtInGame")]
        public long VisionWardsBoughtInGame { get; set; }

        /// <summary>
        /// Number of wards killed.
        /// </summary>
        [JsonProperty("wardsKilled")]
        public long WardsKilled { get; set; }

        /// <summary>
        /// Number of wards placed.
        /// </summary>
        [JsonProperty("wardsPlaced")]
        public long WardsPlaced { get; set; }

        /// <summary>
        /// Flag indicating whether or not the participant won.
        /// </summary>
        [JsonProperty("winner")]
        public bool Winner { get; set; }
    }
    public class Position
    {
        internal Position() { }

        /// <summary>
        /// Participant's X coordinate.
        /// </summary>
        [JsonProperty("x")]
        public int X { get; set; }

        /// <summary>
        /// Participant's Y coordinate.
        /// </summary>
        [JsonProperty("y")]
        public int Y { get; set; }
    }
    public class Rune
    {
        internal Rune() { }

        /// <summary>
        /// Rune rank.
        /// </summary>
        [JsonProperty("rank")]
        public int Rank { get; set; }

        /// <summary>
        /// Rune ID.
        /// </summary>
        [JsonProperty("runeId")]
        public int RuneId { get; set; }
    }
    public class TeamBan
    {
        internal TeamBan() { }

        /// <summary>
        /// The pick turn where the champion has been banned.
        /// </summary>
        [JsonProperty("pickTurn")]
        public int PickTurn { get; set; }
        /// <summary>
        /// ID of the banned champion.
        /// </summary>
        [JsonProperty("championId")]
        public int ChampionId { get; set; }
    }
    public class TeamStats
    {
        internal TeamStats() { }

        /// <summary>
        /// If game was draft mode, contains banned champion data, otherwise null.
        /// </summary>
        [JsonProperty("bans")]
        public List<TeamBan> Bans { get; set; }

        /// <summary>
        /// Number of times the team killed baron.
        /// </summary>
        [JsonProperty("baronKills")]
        public int BaronKills { get; set; }

        /// <summary>
        /// If game was a dominion game, specifies the points the team had at game end, otherwise null.
        /// </summary>
        [JsonProperty("dominionVictoryScore")]
        public long DominionVictoryScore { get; set; }

        /// <summary>
        /// Number of times the team killed dragon.
        /// </summary>
        [JsonProperty("dragonKills")]
        public int DragonKills { get; set; }

        /// <summary>
        /// Flag indicating whether or not the team got the first baron kill.
        /// </summary>
        [JsonProperty("firstBaron")]
        public bool FirstBaron { get; set; }

        /// <summary>
        /// Flag indicating whether or not the team got first blood.
        /// </summary>
        [JsonProperty("firstBlood")]
        public bool FirstBlood { get; set; }

        /// <summary>
        /// Flag indicating whether or not the team got the first dragon kill.
        /// </summary>
        [JsonProperty("firstDragon")]
        public bool FirstDragon { get; set; }

        /// <summary>
        /// Flag indicating whether or not the team destroyed the first inhibitor.
        /// </summary>
        [JsonProperty("firstInhibitor")]
        public bool FirstInhibitor { get; set; }

        /// <summary>
        /// Flag indicating whether or not the team killed the first rift herald.
        /// </summary>
        [JsonProperty("firstRiftHerald")]
        public bool FirstRiftHerald { get; set; }

        /// <summary>
        /// Flag indicating whether or not the team destroyed the first tower.
        /// </summary>
        [JsonProperty("firstTower")]
        public bool FirstTower { get; set; }

        /// <summary>
        /// Number of inhibitors the team destroyed.
        /// </summary>
        [JsonProperty("inhibitorKills")]
        public int InhibitorKills { get; set; }

        /// <summary>
        /// Number of rift heralds killed.
        /// </summary>
        [JsonProperty("riftHeraldKills")]
        public int RiftHeraldKills { get; set; }

        /// <summary>
        /// Team ID.
        /// </summary>
        [JsonProperty("teamId")]
        public int TeamId { get; set; }

        /// <summary>
        /// Number of towers the team destroyed.
        /// </summary>
        [JsonProperty("towerKills")]
        public int TowerKills { get; set; }

        /// <summary>
        /// Number of times the team killed vilemaw (Twisted Treeline epic monster).
        /// </summary>
        [JsonProperty("vilemawKills")]
        public int VilemawKills { get; set; }

        /// <summary>
        /// A string indicating whether or not the team won.
        /// </summary>
        [JsonProperty("win")]
        public string Win { get; set; }
    }
    public class Timeline
    {
        internal Timeline() { }

        /// <summary>
        /// Time between each returned frame.
        /// </summary>
        [JsonProperty("frameInterval")]
        public long FrameInterval { get; set; }

        /// <summary>
        /// List of timeline frames for the game.
        /// </summary>
        [JsonProperty("frames")]
        public List<Frame> Frames { get; set; }
    }
}