using EXILED;

namespace BetterThanFalseTrail
{
    public class MainSettings : Plugin
    {
        public override string getName => "BetterThanFalseTrail";
        private SetEvents SetEvents;

        public override void OnEnable()
        {
            SetEvents = new SetEvents();
            Events.GeneratorInsertedEvent += SetEvents.OnGeneratorInserted;
            Events.PickupItemEvent += SetEvents.OnPickupItem;
            Events.ItemChangedEvent += SetEvents.OnItemChanged;
            Events.UseMedicalItemEvent += SetEvents.OnUseMedicalItem;
            Events.UsedMedicalItemEvent += SetEvents.OnUsedMedicalItem;
            Events.CancelMedicalItemEvent += SetEvents.OnCancelMedicalItem;
            Events.PlayerDeathEvent += SetEvents.OnPlayerDeath;
            Events.PlayerSpawnEvent += SetEvents.OnPlayerSpawn;
            Log.Info(getName + " on");
        }

        public override void OnDisable()
        {
            Events.GeneratorInsertedEvent -= SetEvents.OnGeneratorInserted;
            Events.PickupItemEvent -= SetEvents.OnPickupItem;
            Events.ItemChangedEvent -= SetEvents.OnItemChanged;
            Events.UseMedicalItemEvent -= SetEvents.OnUseMedicalItem;
            Events.UsedMedicalItemEvent -= SetEvents.OnUsedMedicalItem;
            Events.CancelMedicalItemEvent -= SetEvents.OnCancelMedicalItem;
            Events.PlayerDeathEvent -= SetEvents.OnPlayerDeath;
            Events.PlayerSpawnEvent -= SetEvents.OnPlayerSpawn;
            Log.Info(getName + " off");
        }

        public override void OnReload() { }
    }
}