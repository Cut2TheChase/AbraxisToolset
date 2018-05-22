using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using AbraxisToolset.Multiplayer;
using HBS;
using Necro;
using HBS.Text;
using HBS.DebugConsole;

namespace AbraxisToolset.DiscordRPC {
    public class DiscordManager: MonoBehaviour {

        public static DiscordManager instance;

        public const string ID = "388842201104252929";
        private DiscordRpc.EventHandlers handlers;
        private DiscordRpc.RichPresence presence;

        private bool shouldUpdate = false;

        //Rich Presence stats
        private string currentParty = "";
        private ulong lobbyID = 0;
        private int partyMembers = 0;
        private int partyMax = 4;

        //Rich Presence description values
        private string presenceState = "Loading";
        private int currentLevel = int.MinValue;

        private string largePictureID;
        private string largePictureText;

        private Rect outRect = new Rect( 0, 0, 0, 0 );
        private Rect inRect = new Rect( 0, 0, 0, 0 );
        private float fadeTime = 35;
        private float fadeProgress = 0;

        private string inviteUserID;
        public string inviteUserName;
        private bool isInvite = false;

        float updateTimer = 0;
        private Dictionary<string, string> actorIDtoPictureID = new Dictionary<string, string>() {
            {"BlackguardMale","blackguard_female"},
{"BlackguardFemale","blackguard_female"},
{"BlackguardColorTest","blackguard_female"},
{"BruteMale","blackguard_female"},
{"BruteFemale","blackguard_female"},
{"ArcanistMale","blackguard_female"},
{"ArcanistFemale","blackguard_female"},
{"Screamer","blackguard_female"},
{"Screamer2","blackguard_female"},
{"Screamer3","blackguard_female"},
{"Screamer4","blackguard_female"},
{"RatKingNormal","blackguard_female"},
{"ScroungeNormal","blackguard_female"},
{"ScroungeFull","blackguard_female"},
{"ScroungeBomb","blackguard_female"},
{"FadeNormal","blackguard_female"},
{"FadeLate","blackguard_female"},
{"DebugFire","blackguard_female"},
{"DebugIce","blackguard_female"},
{"DebugPierce","blackguard_female"},
{"DebugArcane","blackguard_female"},
{"DebugAcid","blackguard_female"},
{"DebugGrine","blackguard_female"},
{"DebugGrineResist","blackguard_female"},
{"DebugBash","blackguard_female"},
{"AutomatonNormal","blackguard_female"},
{"RuinedAutomatonBombardier","blackguard_female"},
{"AutomatonKnight","blackguard_female"},
{"AutomatonBombadier","blackguard_female"},
{"AutomatonPuppet","blackguard_female"},
{"GrineScout","blackguard_female"},
{"GrineNormal","blackguard_female"},
{"GrineBowman","blackguard_female"},
{"GrineCaptain","blackguard_female"},
{"GrineShieldCaptain","blackguard_female"},
{"GrineHealer","blackguard_female"},
{"InugamiGrine","blackguard_female"},
{"HoardmanGrine","blackguard_female"},
{"LordOfKnivesGrine","blackguard_female"},
{"FatherOfKnivesGrine","blackguard_female"},
{"GemeaterGrine","blackguard_female"},
{"Digger","blackguard_female"},
{"Digger2","blackguard_female"},
{"Digger3","blackguard_female"},
{"Digger4","blackguard_female"},
{"Delver","blackguard_female"},
{"Delver2","blackguard_female"},
{"Delver3","blackguard_female"},
{"Delver4","blackguard_female"},
{"HoardmanPariah","blackguard_female"},
{"HoardmanPariah2","blackguard_female"},
{"HoardmanPariah3","blackguard_female"},
{"HoardmanPariah4","blackguard_female"},
{"HoardmanAscetic","blackguard_female"},
{"HoardmanAscetic2","blackguard_female"},
{"HoardmanAscetic3","blackguard_female"},
{"HoardmanAscetic4","blackguard_female"},
{"HoardmanImmolator","blackguard_female"},
{"HoardmanImmolator2","blackguard_female"},
{"HoardmanImmolator3","blackguard_female"},
{"HoardmanImmolator4","blackguard_female"},
{"HoardmanGreybeard","blackguard_female"},
{"HoardmanGreybeard2","blackguard_female"},
{"HoardmanGreybeard3","blackguard_female"},
{"HoardmanGreybeard4","blackguard_female"},
{"HoardmanLordOfKnives","blackguard_female"},
{"HoardmanLordOfKnives2","blackguard_female"},
{"HoardmanLordOfKnives3","blackguard_female"},
{"HoardmanLordOfKnives4","blackguard_female"},
{"HoardmanFatherOfKnives","blackguard_female"},
{"HoardmanBerserker","blackguard_female"},
{"HoardmanBerserker2","blackguard_female"},
{"HoardmanBerserker3","blackguard_female"},
{"HoardmanBerserker4","blackguard_female"},
{"HoardmanBerserkerPAX","blackguard_female"},
{"HoardmanGreybeardPAX","blackguard_female"},
{"ClockworkKnightSoldierPAX","blackguard_female"},
{"GemeaterBull","blackguard_female"},
{"GemeaterAlbino","blackguard_female"},
{"GemeaterPup","blackguard_female"},
{"GemeaterWelp","blackguard_female"},
{"GemeaterWelpMinion","blackguard_female"},
{"GemeaterAquaticWarrior","blackguard_female"},
{"GemeaterAquatic","blackguard_female"},
{"GemeaterAquaticDark","blackguard_female"},
{"GemeaterAquaticAlpha","blackguard_female"},
{"GemeaterDarkstoneMolting","blackguard_female"},
{"GemeaterDarkstoneKing","blackguard_female"},
{"GemeaterDarkstoneBrawler","blackguard_female"},
{"GemeaterDarkstone","blackguard_female"},
{"HollowManScout_Shieldless","blackguard_female"},
{"FrozenHollowMan_Shieldless","blackguard_female"},
{"HollowManScout","blackguard_female"},
{"FrozenHollowManScout","blackguard_female"},
{"HollowManKnight","blackguard_female"},
{"FrozenHollowManKnight","blackguard_female"},
{"HollowManHellKnight","blackguard_female"},
{"HollowManBrute","blackguard_female"},
{"HollowManScoutPAX","blackguard_female"},
{"RuinedClockworkKnightSoldier","blackguard_female"},
{"ClockworkKnightSoldier","blackguard_female"},
{"DeathEyeClockworkKnight","blackguard_female"},
{"EmeraldClockworkKnight","blackguard_female"},
{"ClockworkLord","blackguard_female"},
{"BarghestNormal","blackguard_female"},
{"BarghestArmor","blackguard_female"},
{"ChangelingFade","blackguard_female"},
{"ChangelingNormal","blackguard_female"},
{"SkeletonMook","blackguard_female"},
{"SkeletonMook1","blackguard_female"},
{"SkeletonMook2","blackguard_female"},
{"SkeletonScout","blackguard_female"},
{"SkeletonGuard","blackguard_female"},
{"SkeletonGuardTwo","blackguard_female"},
{"SkeletonGuardThree","blackguard_female"},
{"SkeletonGuardFour","blackguard_female"},
{"SkeletonGuardFive","blackguard_female"},
{"SkeletonBomber","blackguard_female"},
{"SkeletonGladiator","blackguard_female"},
{"SkeletonMage","blackguard_female"},
{"SkeletonWarlord","blackguard_female"},
{"SkeletonFireGuard","blackguard_female"},
{"SkeletonFireGuardTwo","blackguard_female"},
{"SkeletonFireGuardThree","blackguard_female"},
{"SkeletonFireGuardFour","blackguard_female"},
{"SkeletonFireGuardFive","blackguard_female"},
{"SkeletonGemeater","blackguard_female"},
{"DropSpiderling","blackguard_female"},
{"DropSpider","blackguard_female"},
{"DropSpiderQueen","blackguard_female"},
{"CryptTick","blackguard_female"},
{"BoneEffigy","blackguard_female"},
{"InugamiNormal","blackguard_female"},
{"InugamiNormal2","blackguard_female"},
{"InugamiNormal3","blackguard_female"},
{"InugamiNormal4","blackguard_female"},
{"InugamiStag","blackguard_female"},
{"MolderNormal","blackguard_female"},
{"NagualNormal","blackguard_female"},
{"NagualSniper","blackguard_female"},
{"NagualSpinner","blackguard_female"},
{"NagualHunter","blackguard_female"},
{"NagualWailer","blackguard_female"},
{"ShadowBornJuvenile","blackguard_female"},
{"ShadowBornBuilder","blackguard_female"},
{"ShadowBornJumper","blackguard_female"},
{"ShadowBornCrusher","blackguard_female"},
{"ShellMageNormal","blackguard_female"},
{"ShellMageTeleporter","blackguard_female"},
{"ShellMageBlaster","blackguard_female"},
{"ShellMageSummoner","blackguard_female"},
{"InfectedThiefNormal","blackguard_female"},
{"InfectedExploder","blackguard_female"},
{"InfectedExploder2","blackguard_female"},
{"InfectedHeavy","blackguard_female"},
{"InfectedBrute","blackguard_female"},
{"DevarNormal","blackguard_female"},
{"ChemistNormal","blackguard_female"},
{"OldManNormal","blackguard_female"},
{"HurtyGertieNormal","blackguard_female"},
{"CrystalMantis","blackguard_female"},
{"FireDjinn","blackguard_female"},
{"Bandit","blackguard_female"}
        };
        private Dictionary<string, string> actorIDtoText = new Dictionary<string, string>() {
            {"BlackguardMale","Playing Blackguard Male"},
{"BlackguardFemale","Playing Blackguard Female"},
{"BlackguardColorTest","Playing Blackguard ColorTest"},
{"BruteMale","Playing Brute Male"},
{"BruteFemale","Playing Brute Female"},
{"ArcanistMale","Playing Arcanist Male"},
{"ArcanistFemale","Playing Arcanist Female"},
{"Screamer","Playing Some Hideous Creature"},
{"Screamer2","Playing Some Hideous Creature"},
{"Screamer3","Playing Some Hideous Creature"},
{"Screamer4","Playing Some Hideous Creature"},
{"RatKingNormal","Playing Some Hideous Creature"},
{"ScroungeNormal","Playing Some Hideous Creature"},
{"ScroungeFull","Playing Some Hideous Creature"},
{"ScroungeBomb","Playing Some Hideous Creature"},
{"FadeNormal","Playing Some Hideous Creature"},
{"FadeLate","Playing Some Hideous Creature"},
{"DebugFire","Playing Some Hideous Creature"},
{"DebugIce","Playing Some Hideous Creature"},
{"DebugPierce","Playing Some Hideous Creature"},
{"DebugArcane","Playing Some Hideous Creature"},
{"DebugAcid","Playing Some Hideous Creature"},
{"DebugGrine","Playing Some Hideous Creature"},
{"DebugGrineResist","Playing Some Hideous Creature"},
{"DebugBash","Playing Some Hideous Creature"},
{"AutomatonNormal","Playing Some Hideous Creature"},
{"RuinedAutomatonBombardier","Playing Some Hideous Creature"},
{"AutomatonKnight","Playing Some Hideous Creature"},
{"AutomatonBombadier","Playing Some Hideous Creature"},
{"AutomatonPuppet","Playing Some Hideous Creature"},
{"GrineScout","Playing Some Hideous Creature"},
{"GrineNormal","Playing Some Hideous Creature"},
{"GrineBowman","Playing Some Hideous Creature"},
{"GrineCaptain","Playing Some Hideous Creature"},
{"GrineShieldCaptain","Playing Some Hideous Creature"},
{"GrineHealer","Playing Some Hideous Creature"},
{"InugamiGrine","Playing Some Hideous Creature"},
{"HoardmanGrine","Playing Some Hideous Creature"},
{"LordOfKnivesGrine","Playing Some Hideous Creature"},
{"FatherOfKnivesGrine","Playing Some Hideous Creature"},
{"GemeaterGrine","Playing Some Hideous Creature"},
{"Digger","Playing Some Hideous Creature"},
{"Digger2","Playing Some Hideous Creature"},
{"Digger3","Playing Some Hideous Creature"},
{"Digger4","Playing Some Hideous Creature"},
{"Delver","Playing Some Hideous Creature"},
{"Delver2","Playing Some Hideous Creature"},
{"Delver3","Playing Some Hideous Creature"},
{"Delver4","Playing Some Hideous Creature"},
{"HoardmanPariah","Playing Some Hideous Creature"},
{"HoardmanPariah2","Playing Some Hideous Creature"},
{"HoardmanPariah3","Playing Some Hideous Creature"},
{"HoardmanPariah4","Playing Some Hideous Creature"},
{"HoardmanAscetic","Playing Some Hideous Creature"},
{"HoardmanAscetic2","Playing Some Hideous Creature"},
{"HoardmanAscetic3","Playing Some Hideous Creature"},
{"HoardmanAscetic4","Playing Some Hideous Creature"},
{"HoardmanImmolator","Playing Some Hideous Creature"},
{"HoardmanImmolator2","Playing Some Hideous Creature"},
{"HoardmanImmolator3","Playing Some Hideous Creature"},
{"HoardmanImmolator4","Playing Some Hideous Creature"},
{"HoardmanGreybeard","Playing Some Hideous Creature"},
{"HoardmanGreybeard2","Playing Some Hideous Creature"},
{"HoardmanGreybeard3","Playing Some Hideous Creature"},
{"HoardmanGreybeard4","Playing Some Hideous Creature"},
{"HoardmanLordOfKnives","Playing Some Hideous Creature"},
{"HoardmanLordOfKnives2","Playing Some Hideous Creature"},
{"HoardmanLordOfKnives3","Playing Some Hideous Creature"},
{"HoardmanLordOfKnives4","Playing Some Hideous Creature"},
{"HoardmanFatherOfKnives","Playing Some Hideous Creature"},
{"HoardmanBerserker","Playing Some Hideous Creature"},
{"HoardmanBerserker2","Playing Some Hideous Creature"},
{"HoardmanBerserker3","Playing Some Hideous Creature"},
{"HoardmanBerserker4","Playing Some Hideous Creature"},
{"HoardmanBerserkerPAX","Playing Some Hideous Creature"},
{"HoardmanGreybeardPAX","Playing Some Hideous Creature"},
{"ClockworkKnightSoldierPAX","Playing Some Hideous Creature"},
{"GemeaterBull","Playing Some Hideous Creature"},
{"GemeaterAlbino","Playing Some Hideous Creature"},
{"GemeaterPup","Playing Some Hideous Creature"},
{"GemeaterWelp","Playing Some Hideous Creature"},
{"GemeaterWelpMinion","Playing Some Hideous Creature"},
{"GemeaterAquaticWarrior","Playing Some Hideous Creature"},
{"GemeaterAquatic","Playing Some Hideous Creature"},
{"GemeaterAquaticDark","Playing Some Hideous Creature"},
{"GemeaterAquaticAlpha","Playing Some Hideous Creature"},
{"GemeaterDarkstoneMolting","Playing Some Hideous Creature"},
{"GemeaterDarkstoneKing","Playing Some Hideous Creature"},
{"GemeaterDarkstoneBrawler","Playing Some Hideous Creature"},
{"GemeaterDarkstone","Playing Some Hideous Creature"},
{"HollowManScout_Shieldless","Playing Some Hideous Creature"},
{"FrozenHollowMan_Shieldless","Playing Some Hideous Creature"},
{"HollowManScout","Playing Some Hideous Creature"},
{"FrozenHollowManScout","Playing Some Hideous Creature"},
{"HollowManKnight","Playing Some Hideous Creature"},
{"FrozenHollowManKnight","Playing Some Hideous Creature"},
{"HollowManHellKnight","Playing Some Hideous Creature"},
{"HollowManBrute","Playing Some Hideous Creature"},
{"HollowManScoutPAX","Playing Some Hideous Creature"},
{"RuinedClockworkKnightSoldier","Playing Some Hideous Creature"},
{"ClockworkKnightSoldier","Playing Some Hideous Creature"},
{"DeathEyeClockworkKnight","Playing Some Hideous Creature"},
{"EmeraldClockworkKnight","Playing Some Hideous Creature"},
{"ClockworkLord","Playing Some Hideous Creature"},
{"BarghestNormal","Playing Some Hideous Creature"},
{"BarghestArmor","Playing Some Hideous Creature"},
{"ChangelingFade","Playing Some Hideous Creature"},
{"ChangelingNormal","Playing Some Hideous Creature"},
{"SkeletonMook","Playing Some Hideous Creature"},
{"SkeletonMook1","Playing Some Hideous Creature"},
{"SkeletonMook2","Playing Some Hideous Creature"},
{"SkeletonScout","Playing Some Hideous Creature"},
{"SkeletonGuard","Playing Some Hideous Creature"},
{"SkeletonGuardTwo","Playing Some Hideous Creature"},
{"SkeletonGuardThree","Playing Some Hideous Creature"},
{"SkeletonGuardFour","Playing Some Hideous Creature"},
{"SkeletonGuardFive","Playing Some Hideous Creature"},
{"SkeletonBomber","Playing Some Hideous Creature"},
{"SkeletonGladiator","Playing Some Hideous Creature"},
{"SkeletonMage","Playing Some Hideous Creature"},
{"SkeletonWarlord","Playing Some Hideous Creature"},
{"SkeletonFireGuard","Playing Some Hideous Creature"},
{"SkeletonFireGuardTwo","Playing Some Hideous Creature"},
{"SkeletonFireGuardThree","Playing Some Hideous Creature"},
{"SkeletonFireGuardFour","Playing Some Hideous Creature"},
{"SkeletonFireGuardFive","Playing Some Hideous Creature"},
{"SkeletonGemeater","Playing Some Hideous Creature"},
{"DropSpiderling","Playing Some Hideous Creature"},
{"DropSpider","Playing Some Hideous Creature"},
{"DropSpiderQueen","Playing Some Hideous Creature"},
{"CryptTick","Playing Some Hideous Creature"},
{"BoneEffigy","Playing Some Hideous Creature"},
{"InugamiNormal","Playing Some Hideous Creature"},
{"InugamiNormal2","Playing Some Hideous Creature"},
{"InugamiNormal3","Playing Some Hideous Creature"},
{"InugamiNormal4","Playing Some Hideous Creature"},
{"InugamiStag","Playing Some Hideous Creature"},
{"MolderNormal","Playing Some Hideous Creature"},
{"NagualNormal","Playing Some Hideous Creature"},
{"NagualSniper","Playing Some Hideous Creature"},
{"NagualSpinner","Playing Some Hideous Creature"},
{"NagualHunter","Playing Some Hideous Creature"},
{"NagualWailer","Playing Some Hideous Creature"},
{"ShadowBornJuvenile","Playing Some Hideous Creature"},
{"ShadowBornBuilder","Playing Some Hideous Creature"},
{"ShadowBornJumper","Playing Some Hideous Creature"},
{"ShadowBornCrusher","Playing Some Hideous Creature"},
{"ShellMageNormal","Playing Some Hideous Creature"},
{"ShellMageTeleporter","Playing Some Hideous Creature"},
{"ShellMageBlaster","Playing Some Hideous Creature"},
{"ShellMageSummoner","Playing Some Hideous Creature"},
{"InfectedThiefNormal","Playing Some Hideous Creature"},
{"InfectedExploder","Playing Some Hideous Creature"},
{"InfectedExploder2","Playing Some Hideous Creature"},
{"InfectedHeavy","Playing Some Hideous Creature"},
{"InfectedBrute","Playing Some Hideous Creature"},
{"DevarNormal","Playing Some Hideous Creature"},
{"ChemistNormal","Playing Some Hideous Creature"},
{"OldManNormal","Playing Some Hideous Creature"},
{"HurtyGertieNormal","Playing Some Hideous Creature"},
{"CrystalMantis","Playing Some Hideous Creature"},
{"FireDjinn","Playing Some Hideous Creature"},
{"Bandit","Playing Some Hideous Creature"}

        };

        public void Awake() {
            instance = this;
        }

        public void Update() {
            updateTimer -= Time.deltaTime;

            if( isInvite ) {

                fadeProgress -= Time.deltaTime;
                if( fadeProgress < 5 )
                    fadeProgress -= Time.deltaTime * 14;
                if( fadeProgress <= 0 ) {
                    DiscordRpc.Respond( inviteUserID, DiscordRpc.Reply.Ignore );
                    isInvite = false;
                    fadeProgress = 0;
                    Debug.Log( "Ignored invite" );
                }

                if( fadeProgress > 0 ) {
                    if( Necro.Platform.GetKeyDown( KeyCode.LeftArrow ) ) {
                        DiscordRpc.Respond( inviteUserID, DiscordRpc.Reply.No );
                        isInvite = false;
                        fadeProgress = 0;
                        Debug.Log("Denied invite");
                    } else if( Necro.Platform.GetKeyDown( KeyCode.RightArrow ) ) {
                        DiscordRpc.Respond( inviteUserID, DiscordRpc.Reply.Yes );
                        isInvite = false;
                        fadeProgress = 0;
                        Debug.Log( "Accepted invite" );
                    }
                }
            }


            if( updateTimer <= 0 ) {
                //Character updater
                {
                    if( CharacterSelectorCamera.HasInstance ) {
                        if( Patches.patch_CharacterSelectorCamera.Instance.GetLocalActor() != null ) {
                           SetLargePictureID( actorIDtoPictureID[Patches.patch_CharacterSelectorCamera.Instance.GetLocalActor().actorDefId] );
                            SetLargePictureText( actorIDtoText[Patches.patch_CharacterSelectorCamera.Instance.GetLocalActor().actorDefId] );
                        }
                    }

                    if( ThirdPersonCameraControl.HasInstance ) {
                        if( ThirdPersonCameraControl.Instance.CharacterActor != null ) {
                           SetLargePictureID( actorIDtoPictureID[ThirdPersonCameraControl.Instance.CharacterActor.actorDefId] );
                            SetLargePictureText( actorIDtoText[ThirdPersonCameraControl.Instance.CharacterActor.actorDefId] );
                        }
                    }
                }

                //Floor updater
                if( NecropoloidSingleton.HasInstance && ThirdPersonCameraControl.HasInstance ) {
                    if( ThirdPersonCameraControl.Instance.CharacterActor != null ) {
                        Necropoloid.Cell currentCell = NecropoloidSingleton.Instance.Necropoloid.GetCellFromPosition( ThirdPersonCameraControl.Instance.CharacterFocusPoint );
                        if( currentCell != null ) {
                            SetRichPresenceState( "In A Game" );
                            SetLevel( ( -currentCell.gridPos.y ) );
                        }
                    }
                } else if( Necro.UI.Menu_MainMenu.HasInstance ) {
                    SetRichPresenceState( "On Main Menu" );
                } else {
                    SetRichPresenceState( "Loading" );
                }
                updateTimer = 5;
            }

            if( shouldUpdate ) {
                DebugConsole.Log("Updating Discord Presence");
                shouldUpdate = false;
                UpdateRichPresenceValues();
                DiscordRpc.UpdatePresence( ref presence );
            }

            DiscordRpc.RunCallbacks();
        }

        public void OnEnable() {

            try {
                handlers = new DiscordRpc.EventHandlers();
                handlers.readyCallback = ReadyCallback;
                handlers.disconnectedCallback += DisconnectedCallback;
                handlers.errorCallback += ErrorCallback;
                handlers.joinCallback += JoinCallback;
                handlers.spectateCallback += SpectateCallback;
                handlers.requestCallback += RequestCallback;

                Debug.Log( "Initializing Discord" );

                presence = new DiscordRpc.RichPresence();

                DiscordRpc.Initialize( ID, ref handlers, true, "384490" );
            } catch( System.Exception e ) {
                Debug.LogError( e );
            }
        }

        public void OnGUI() {
            outRect = new Rect( Screen.width / 2 - 250, -75, 500, 55 );
            inRect = new Rect( Screen.width / 2 - 250, 25, 500, 55 );
            Rect currentRect = new Rect( Vector2.Lerp( outRect.position, inRect.position, fadeProgress / fadeTime ), Vector2.Lerp( outRect.size, inRect.size, fadeProgress / fadeTime ) );

            GUI.Box( currentRect, "" );
            GUILayout.BeginArea( currentRect );
            GUILayout.Label( string.Format( "<size=20>User {0} is asking to join your game</size>", inviteUserName ) );
            GUILayout.Label( "Press [Right Arrow] to accept, or [Left Arrow] to deny" );
            GUILayout.EndArea();
        }

        public void OnDestroy() {
            Debug.Log( "Discord Shutdown" );
            DiscordRpc.Shutdown();
        }

        public void UpdateRichPresenceValues() {
            presence.partyId = currentParty;
            presence.partyMax = partyMax;
            presence.partySize = partyMembers;

            presence.joinSecret = currentParty + "," + lobbyID.ToString();
            presence.largeImageKey = largePictureID;
            presence.largeImageText = largePictureText;

            presence.state = presenceState;
            if( currentLevel != int.MinValue ) {
                presence.details = string.Format( "On level {0}", currentLevel + 1 );
            }
        }
        public void UpdateRP() {
            shouldUpdate = true;
        }

        public void ReadyCallback() {
            Debug.Log( "Discord is Ready" );
        }
        public void DisconnectedCallback(int errorCode, string message) {
            Debug.Log( string.Format( "Discord: disconnect {0}: {1}", errorCode, message ) );
        }
        public void ErrorCallback(int errorCode, string message) {
            Debug.Log( string.Format( "Discord: error {0}: {1}", errorCode, message ) );
        }
        public void JoinCallback(string secret) {
            if( secret == string.Empty )
                return;

            List<string> parts = new List<string>();
            StringUtil.SplitCSV( secret, parts, true );

            Debug.Log( string.Format( "Discord: join ({0})", secret ) );

            Patches.patch_NetPlatformServicesSteam.QueueJoin( new Steamworks.CSteamID( ulong.Parse( parts[1] ) ) );
            SetPartyID( parts[0] );
        }
        public void SpectateCallback(string secret) {
            Debug.Log( string.Format( "Discord: spectate ({0})", secret ) );
        }
        public void RequestCallback(ref DiscordRpc.JoinRequest request) {
            Debug.Log( string.Format( "Discord: join request {0}#{1}: {2}", request.username, request.discriminator, request.userId ) );
            inviteUserName = request.username;
            inviteUserID = request.userId;
            fadeProgress = fadeTime;
            isInvite = true;
        }

        public void SetRichPresenceState(string state) {
            if( state != presenceState ) {
                presenceState = state;
                UpdateRP();
            }
        }
        public void SetPartyNumber(int numberOfMembers) {
            if( partyMembers != numberOfMembers ) {
                partyMembers = numberOfMembers;
                UpdateRP();
            }
        }
        public void SetPartyMaxNumber(int maxMembers) {
            if( partyMax != maxMembers ) {
                partyMax = maxMembers;
                UpdateRP();
            }
        }
        public void SetPartyID(string partyID) {
            if( currentParty != partyID ) {
                currentParty = partyID;
                UpdateRP();
            }
        }
        public void SetLobbyID(ulong lobbyID) {
            if( this.lobbyID != lobbyID ) {
                this.lobbyID = lobbyID;
                UpdateRP();
            }
        }
        public void SetLevel(int level) {
            if( level != currentLevel ) {
                currentLevel = level;
                UpdateRP();
            }
        }
        public void SetLargePictureID(string id) {
            if( id != largePictureID ) {
                largePictureID = id;
                UpdateRP();
            }
        }
        public void SetLargePictureText(string txt) {
            if( largePictureText != txt ) {
                largePictureText = txt;
                UpdateRP();
            }
        }

    }
}
