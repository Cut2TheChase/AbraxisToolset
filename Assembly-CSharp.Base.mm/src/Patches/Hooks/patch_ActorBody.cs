using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using AbraxisToolset;
using HBS.Animation;
using HBS.Nav;
using Necro;
using Partiality.Modloader;
using UnityEngine;

namespace Patches {
    [MonoMod.MonoModPatch( "global::Necro.ActorBody" )]
    class patch_ActorBody: PartialityMod {

        /* FROM WHEN THIS USED TO BE PATCHING ACTOR I JUST LIKE TO LEAVE THIS HERE TO SEE EXAMPLES
        public extern void orig_Init(ActorDef actorDef, int actorLevel, ActorBody body, bool initViaSpawn, SessionDataActor sda = null);
        public void Init(ActorDef actorDef, int actorLevel, ActorBody body, bool initViaSpawn, SessionDataActor sda = null) {
            NecroHooks.OnActorSpawned(this);
            orig_Init(actorDef, actorLevel, body, initViaSpawn, sda);
        }

        public extern void orig_Kill(bool loot = true, bool force = false, Actor killer = null, bool deathSequence = true, bool fromNetwork = false);
        public void Killbool (bool loot = true, bool force = false, Actor killer = null, bool deathSequence = true, bool fromNetwork = false) {
            NecroHooks.OnActorKilled(this);
            orig_Kill(loot, force, killer, deathSequence, fromNetwork);
        }

        public extern void orig_Despawn();
        public void Despawn() {
            NecroHooks.OnActorPreDespawn(this);
            orig_Despawn();
        }
        */

        [MonoMod.MonoModIgnore]
        public IAnimator thisAnimator;

        [MonoMod.MonoModIgnore]
        private int layerIdBase;

        [MonoMod.MonoModIgnore]
        private Actor _thisActor;

        [MonoMod.MonoModIgnore]
        public NavAgent thisAgent;

        [MonoMod.MonoModIgnore]
        private extern void NotifyActorAssigned(Actor newActor);

        [MonoMod.MonoModIgnore]
        public Actor thisActor
        {
            get
            {
                return this._thisActor;
            }
            set
            {
                this._thisActor = value;
                if (this.thisAgent != null)
                {
                    this.thisAgent.userData = this._thisActor;
                }
                if (this._thisActor != null)
                {
                    this.NotifyActorAssigned(this._thisActor);
                }
            }
        }

        private extern void orig_AE_TriggerGameEffect();
        private void AE_TriggerGameEffect()
        {
            UnityEngine.Debug.Log("HEY ITS TIME TO SEE IF IM GONNA TRIGGER THIS GAME BOI FOR " + this.thisActor.actorDefId);
            if (this._thisActor == null)
            {
                UnityEngine.Debug.Log("Why? Literally....just why");
                return;
            }
            if (!this.thisAnimator.CurrentClipHasEvent(this.layerIdBase, "AE_TriggerGameEffect"))
            {
                UnityEngine.Debug.Log("Really? You gonna do this? Yah come into MY house on the day of daughter's wedding BADDAHBINGBADDAHBOOM");
                return;
            }
            UnityEngine.Debug.Log("Hey yah fuckin idiot trigger this game effect for " + this.thisActor.actorDefId);
            this.thisActor._OnTriggerGameEffect();

        }


       

    }
}
