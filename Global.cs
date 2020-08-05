using System.Collections.Generic;
using System.IO;

namespace BetterThanFalseTrail
{
    public static class Global
    {
        public static bool CanUseConsoleCommands = false;

        public static readonly string WhitelistCommanderFullFileName = Path.Combine("/etc/scpsl/Plugin/WhitelistCommander.txt");
        
        public static readonly string WhitelistManagerFullFileName = Path.Combine("/etc/scpsl/Plugin/WhitelistManager.txt");

        public static List<string> WhitelistCommander = new List<string>();

        public static List<string> WhitelistManager = new List<string>();

        public static readonly float TimeToPickupMicroHID = 5.0f;

        public static int CurrentManagerPlayerId = 0;

        public static int CurrentSecurityChiefPlayerId = 0;

        public static List<int> AvailableCommanderPlayerId = new List<int>();

        public static readonly float TimeToVoteCommander = 30.0f;

        public static Dictionary<int, int> PlayersVotes = new Dictionary<int, int>();

        public static bool InWhitelistCommander(string steamId)
        {
            return WhitelistCommander.Contains(steamId.Replace("@steam", string.Empty));
        }

        public static bool InWhitelistManager(string steamId)
        {
            return WhitelistManager.Contains(steamId.Replace("@steam", string.Empty));
        }
    }
}