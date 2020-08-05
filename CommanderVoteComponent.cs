using Exiled.API.Features;
using System.Collections.Generic;
using UnityEngine;

namespace BetterThanFalseTrail
{
    internal class CommanderVoteComponent : MonoBehaviour
    {
        private float Timer = 0.0f;
        private readonly float TimeIsUp = 1.0f;
        private float TimerToEnd = 0.0f;
        public Dictionary<int, int> PlayersVotes = new Dictionary<int, int>();
        
        public List<int> PlayersAlreadyVotes = new List<int>();
        Player Commander;

        private bool CommanderChosen = false;

        public void Start()
        {
            foreach (Player player in Player.List)
            {
                if (PlayersVotes.ContainsKey(player.Id))
                {
                    player.ClearBroadcasts();
                    player.Broadcast(10, "Выберите командира МОГ. (Откройте консоль и используйте команду .vote)", Broadcast.BroadcastFlags.Normal);
                    player.SendConsoleMessage(GetVoteMessage(), "yellow");
                }
            }
        }

        private string GetVoteMessage()
        {
            string result = "Список игроков, доступных для голосования:";
            foreach (KeyValuePair<int, int> kvp in PlayersVotes)
            {
                result = result + "\n" + kvp.Key + ") " + (Player.Get(kvp.Key) != null ? Player.Get(kvp.Key).Nickname : "Игрок не найден");
            }
            return result;
        }

        public void Update()
        {
            Timer += Time.deltaTime;

            if (Timer > TimeIsUp)
            {
                Timer = 0.0f;
                if (CommanderChosen)
                {
                    Destroy(this);
                }
                else
                {
                    TimerToEnd += TimeIsUp;

                    if (TimerToEnd > Global.TimeToVoteCommander)
                    {
                        int playerId = 0;
                        int votes = 0;
                        foreach (KeyValuePair<int, int> kvp in PlayersVotes)
                        {
                            if (votes < kvp.Value)
                            {
                                playerId = kvp.Key;
                            }
                        }
                        Commander = Player.Get(playerId);
                        if (Commander != null)
                        {
                            CommanderChosen = true;
                            Global.WhitelistCommander.Add(Commander.UserId.Replace("@steam", string.Empty));
                            Commander.SetRole(RoleType.NtfCommander);
                            Commander.ClearBroadcasts();
                            Commander.Broadcast(10, "Вы были выбраны командиром МОГ", Broadcast.BroadcastFlags.Normal);
                        }
                        Destroy(this);
                    }
                }
            }
        }

        public void OnDestroy()
        {
            Global.PlayersVotes = new Dictionary<int, int>();
            if (Global.WhitelistCommander.Contains(Commander.UserId.Replace("@steam", string.Empty)))
            {
                Global.WhitelistCommander.Remove(Commander.UserId.Replace("@steam", string.Empty));
            }
        }
    }
}