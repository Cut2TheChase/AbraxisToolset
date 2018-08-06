
using UnityEngine;
using Necro;
using Necro.Data.Stats;
using System.Collections.Generic;

namespace AbraxisToolset.src.Patches
{
    [MonoMod.MonoModPatch("global::Necro.MultiplayerTuningData")]
    class patch_MultiplayerTuningData
    {
        [MonoMod.MonoModIgnore]
        public int ActorAttackAddend;

        [MonoMod.MonoModIgnore]
        public int NonActorAttackAddend;

        [MonoMod.MonoModIgnore]
        public int PvPAttackAddend;

        [MonoMod.MonoModIgnore]
        public int DanceCard;

        [MonoMod.MonoModIgnore]
        public Dictionary<StatDef, float> stats;

        [MonoMod.MonoModIgnore]
        public float GroupCurrencyPickup;

        [MonoMod.MonoModIgnore]
        public float GroupCraftingPickup;

        [MonoMod.MonoModIgnore]
        public float ResurrectHealth;

        [MonoMod.MonoModIgnore]
        public float ResurrectExhaustion;

        [MonoMod.MonoModIgnore]
        public int MaxResurrectionSickness;

        [MonoMod.MonoModIgnore]
        public float ResurrectionSicknessHealthModifier;

        [MonoMod.MonoModIgnore]
        public float ResurrectionSicknessExhaustionModifier;

        [MonoMod.MonoModIgnore]
        public int ActorDefendTraitAddend;

        [MonoMod.MonoModIgnore]
        private extern float GetVariableFloat(string variable);

        [MonoMod.MonoModIgnore]
        private extern float GetCouchScaledVariableFloat(string variable);

        [MonoMod.MonoModIgnore]
        private extern int GetCouchScaledVariableInt(string variable);

        [MonoMod.MonoModIgnore]
        private extern int GetVariableInt(string variable);

        public extern void orig_Set();
        public void Set()
        {
            this.ActorAttackAddend = this.GetCouchScaledVariableInt("Multiplayer.ActorAttackAddend");
            this.NonActorAttackAddend = this.GetCouchScaledVariableInt("Multiplayer.NonActorAttackAddend");
            this.ActorDefendTraitAddend = this.GetCouchScaledVariableInt("Multiplayer.ActorDefendTraitAddend");
            this.PvPAttackAddend = this.GetVariableInt("Multiplayer.PvPAttackAddend");
            this.DanceCard = this.GetCouchScaledVariableInt("Multiplayer.DanceCard");
            this.stats[Stats.HealthRate] = this.GetCouchScaledVariableFloat("Multiplayer.SlapsRoofOfNecropolis"); //This bad boy can fit so many fucking bugs in it
            this.stats[Stats.PoisonRate] = this.GetCouchScaledVariableFloat("Multiplayer.PoisonRate");
            this.GroupCurrencyPickup = this.GetVariableFloat("Multiplayer.GroupCurrencyPickup");
            this.GroupCraftingPickup = this.GetVariableFloat("Multiplayer.GroupCraftingPickup");
            this.ResurrectHealth = this.GetVariableFloat("Multiplayer.ResurrectHealth");
            this.ResurrectExhaustion = this.GetVariableFloat("Multiplayer.ResurrectExhaustion");
            this.MaxResurrectionSickness = this.GetVariableInt("Multiplayer.MaxResurrectionSickness");
            this.ResurrectionSicknessHealthModifier = this.GetVariableFloat("Multiplayer.ResurrectionSicknessHealthModifier");
            this.ResurrectionSicknessExhaustionModifier = this.GetVariableFloat("Multiplayer.ResurrectionSicknessExhaustionModifier");
            this.GroupCraftingPickup = Mathf.Clamp(this.GroupCraftingPickup, 0f, 1f);
            this.GroupCurrencyPickup = Mathf.Clamp(this.GroupCurrencyPickup, 0f, 1f);
            this.ResurrectHealth = Mathf.Clamp(this.ResurrectHealth, 0f, 1f);
            this.ResurrectExhaustion = Mathf.Clamp(this.ResurrectExhaustion, 0f, 1f);
            this.ResurrectionSicknessHealthModifier = Mathf.Clamp(this.ResurrectionSicknessHealthModifier, 0f, 1f);
            this.ResurrectionSicknessExhaustionModifier = Mathf.Clamp(this.ResurrectionSicknessExhaustionModifier, 0f, 1f);
            if (this.ResurrectHealth == 0f)
            {
                this.ResurrectHealth = 0.5f;
            }
        }
    }
}
