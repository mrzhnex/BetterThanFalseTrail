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

        public static float time_to_contain_096 = 15.0f;
        public static string _outofscp096 = "SCP 096 нет рядом";
        public static string _successstartcontain096 = "Вы надеваете мешок на SCP 096. Ждите: ";
        public static string _noticescp096 = "На вас надевают мешок";
        public static string announcecontainscp096 = "SCP 0 9 6 ContainedSuccessfully . ";
        public static string _alreadycontainproccess096 = "Другой гуманоид уже надевает мешок";
        public static string _failedcontain096and173 = "Вы отошли слишком далеко. Процесс прерван";

        public static float time_to_contain_173 = 40.0f;
        public static string _outofscp173 = "SCP 173 нет рядом";
        public static string _successstartcontain173 = "Вы собираете клетку 173. Ждите: ";
        public static string _noticescp173 = "Вас помещают в клетку";
        public static string announcecontainscp173 = "SCP 1 7 3 ContainedSuccessfully . ";
        public static string _alreadycontainproccess173 = "Другой гуманоид уже собирает клетку";

        public static float distanceForContain096And173 = 2.0f;
    }
}