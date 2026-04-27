namespace SquidGame.LandScape.Core
{
    public static class Define
    {
        public const int WIN_SEASON_COIN = 1000;
        public const int WIN_MINIGAME_COIN = 100;
        public const int HAIR_PRICE = 1200;

        public class PlayerConfig
        {
            public const float SPEED = 4.5f;
            public const float JUMP_HEIGHT = 1.6f;
            public const float GRAVITY = -60f;
        }

        public class SceneName
        {
            public const string CORE = "Core";
            public const string HOME = "Home";
            public const string LOBBY = "Lobby";
        }

        public class UIName
        {
            // Menu
            public const string HOME_MENU = "Menu/HomeMenuUI";

            public const string CHALLENGE_MENU = "Menu/ChallengeMenuUI";
            public const string MINIGAMES_MENU = "Menu/MinigamesMenuUI";
            public const string MINIGAME05_MENU = "Menu/Minigame05MenuUI";
            public const string LOBBY_MENU = "Menu/LobbyUI";
            public const string SHOP_MENU = "Menu/ShopMenuUI";

            // Popup
            public const string SETTINGS_POPUP = "Popup/SettingsPopupUI";
            public const string WIN_CHALLENGE_POPUP = "Popup/WinChallengePopupUI";
            public const string LOSE_CHALLENGE_POPUP = "Popup/LoseChallengePopupUI";
            public const string WIN_MINIGAME_POPUP = "Popup/WinMinigamePopupUI";
            public const string LOSE_MINIGAME_POPUP = "Popup/LoseMinigamePopupUI";
            public const string NO_INTERNET_POPUP = "Popup/NoInternetPopupUI";
            public const string RATE_US_POPUP = "Popup/RateUsPopupUI";

            // Misc
            public const string LOADING_UI = "LoadingUI";

            //Minigame 
            public const string MINIGAME_01_MENU = "Menu/Minigame01MenuUI";
            public const string MINIGAME_02_MENU = "Menu/Minigame02MenuUI";
            public const string MINIGAME_03_MENU = "Menu/Minigame03MenuUI";
            public const string MINIGAME_04_MENU = "Menu/Minigame04MenuUI";
            public const string MINIGAME_05_MENU = "Menu/Minigame05MenuUI";
            public const string MINIGAME_06_MENU = "Menu/Minigame06MenuUI";
            public const string MINIGAME_MINGLE_MENU = "Menu/MinigameMingleMenuUI";
            public const string MINIGAME_SURVIVAL_MENU = "Menu/MinigameSurvivalUI";
            public const string MINIGAME_BALANCE_BRIDGE_MENU = "Menu/MinigameBalanceBridgeUI";
            public const string MINIGAME_SQUID_GAME_MENU = "Menu/MinigameSquidGameUI";
            public const string MINIGAME_JUMPING_MENU = "Menu/MinigameJumpingUI";
            public const string MINIGAME_FIND_MARBLES_MENU = "Menu/MinigameFindMarblesMenuUI";
            public const string MINIGAME_MARBLESTWO_MENU = "Menu/MinigameMarblesTwoMenuUI";
            public const string MINIGAME_16_MENU = "Menu/Minigame16MenuUI";
            public const string MINIGAME_ROCKPAPERSCISSORS_MENU = "Menu/MinigameRockPaperScissorsMenuUI";

        }

        public class SoundPath
        {
            // BGM
            private const string ROOT_BGM = "BGM/";

            public const string BGM_MAIN = ROOT_BGM + "BGM_Main";
            public const string BGM_LOBBY = ROOT_BGM + "BGM_Lobby";
            public const string BGM_MINIGAME_01 = ROOT_BGM + "BGM_Game_1";
            public const string BGM_MINIGAME_02 = ROOT_BGM + "BGM_Game_2";
            public const string BGM_MINIGAME_03 = ROOT_BGM + "BGM_Game_3";

            // Mingiame06
            public const string BGM_MINIGAME_06 = ROOT_BGM + "Minigame06/BGM_Game_6";

            // Minigame 21
            public const string BGM_MG21_INGAME = ROOT_BGM + "Minigame21/BGM_Ingame";
            public const string BGM_MG21_MINGLE = ROOT_BGM + "Minigame21/BGM_Mingle_Game";
            // SFX
            private const string ROOT_SFX = "SFX/";

            // Common
            private const string ROOT_COMMON = "SFX/Common/";
            public const string SFX_CORRECT_CHOICE = ROOT_COMMON + "SFX_Correct_Choice";
            public const string SFX_WRONG_CHOICE = ROOT_COMMON + "SFX_Wrong_Choice";
            public const string SFX_SWAP = ROOT_COMMON + "SFX_Swap";

            public const string SFX_BUTTON_CLICK = ROOT_SFX + "SFX_Click";
            public const string SFX_LOSING = ROOT_SFX + "SFX_Losing";
            public const string SFX_WIN_DANCE = ROOT_SFX + "SFX_Win_Dance";
            public const string SFX_WIN_SCREEN = ROOT_SFX + "SFX_Win_Screen";
            public const string SFX_REVIVE = ROOT_SFX + "SFX_Revive";
            public const string SFX_TING = ROOT_SFX + "SFX_Ting";
            public const string SFX_BUY_ITEM = ROOT_SFX + "SFX_Buy_Item";
            public const string SFX_EQUIP_ITEM = ROOT_SFX + "SFX_Equip_Item";
            public const string SFX_CLAIM_REWARD = ROOT_SFX + "SFX_Claim_Daily_Reward";
            public const string SFX_FOOT_STEP = ROOT_SFX + "SFX_Foot_Step";
            public const string SFX_ADD_COIN = ROOT_SFX + "SFX_AddCoin";

            //Lobby
            public const string SFX_LOBBY_SMASH = ROOT_SFX + "SFX_Lobby_Smash";
            public const string SFX_LOBBY_COIN_DROP = ROOT_SFX + "SFX_Coin_Drop";
            public const string SFX_LOBBY_COIN_DROP_2 = ROOT_SFX + "SFX_Coin_Drop_2";


            // Minigame01
            public const string SFX_MG01_GUNSHOT = ROOT_SFX + "Minigame01/SFX_GunShot";
            public const string SFX_MG01_M4_SHOOT = ROOT_SFX + "Minigame01/SFX_M4_shoot";
            public const string SFX_MG01_HOSTAGE_F_HIT_01 = ROOT_SFX + "Minigame01/SFX_Hostage_F_Hit_01";
            public const string SFX_MG01_HOSTAGE_M_HIT_01 = ROOT_SFX + "Minigame01/SFX_Hostage_M_Hit_01";
            public const string SFX_MG01_SEARCHING = ROOT_SFX + "Minigame01/SFX_Searching";
            public const string SFX_MG01_SING = ROOT_SFX + "Minigame01/SFX_Sing";
            public const string SFX_MG01_TICK = ROOT_SFX + "Minigame01/SFX_Time_Tick";
            public const string SFX_MG01_WHISTLE = ROOT_SFX + "Minigame01/SFX_whistle";

            // Minigame02
            public const string SFX_MG02_GLASS_BREAK = ROOT_SFX + "Minigame02/SFX_Glass_Break";
            public const string SFX_MG02_JUMP = ROOT_SFX + "Minigame02/SFX_Jump";
            public const string SFX_MG02_SCREAM = ROOT_SFX + "Minigame02/SFX_Scream";
            public const string SFX_MG02_JUMP_ON_GLASS = ROOT_SFX + "Minigame02/SFX_Jump_On_Glass";

            // Minigame03
            public const string SFX_MG03_CHOOSE_CASE = ROOT_SFX + "Minigame03/SFX_Choose_Case";
            public const string SFX_MG03_BREAK_PART_IN = ROOT_SFX + "Minigame03/SFX_Break_Part_In";
            public const string SFX_MG03_BREAK_PART_OUT = ROOT_SFX + "Minigame03/SFX_Break_Part_Out";
            public const string SFX_MG03_DRAW = ROOT_SFX + "Minigame03/SFX_Draw";
            public const string SFX_MG03_BROKEN = ROOT_SFX + "Minigame03/SFX_Broken";
            public const string SFX_MG03_RANDOM = ROOT_SFX + "Minigame03/SFX_Random";

            // Minigame04
            public const string SFX_MG04_THROW_MARBLE = ROOT_SFX + "Minigame04/SFX_Throw_Marble";
            public const string SFX_MG04_INTO_HOLE = ROOT_SFX + "Minigame04/SFX_Into_Hole";
            public const string SFX_MG04_FALL = ROOT_SFX + "Minigame04/SFX_MarbleFall";
            public const string SFX_MG04_MARBLE_COLLIDE = ROOT_SFX + "Minigame04/SFX_MarbleCollide";
            public const string SFX_MG04_MARBLE_DROP = ROOT_SFX + "Minigame04/SFX_MarbleDrop";
            public const string SFX_MG04_BALL_IN_HOLE = ROOT_SFX + "Minigame04/SFX_BallInHole";
            public const string SFX_CONGRA = ROOT_SFX + "Minigame04/SFX_Congra";

            // Minigame05
            public const string SFX_MG05_CHEER = ROOT_SFX + "Minigame05/SFX_Cheer";
            public const string SFX_MG05_WIN = ROOT_SFX + "Minigame05/SFX_Win_TugOfWar";
            public const string SFX_MG05_COUNT_DOWN = ROOT_SFX + "Minigame05/SFX_Countdown";
            public const string SFX_CLOCK = ROOT_SFX + "Minigame05/SFX_Clock";

            // Survival
            public const string SFX_AXE_SLASH = ROOT_SFX + "MinigameSurvival/SFX_Axe_Slash";
            public const string SFX_BASEBALL_BAT_SWING = ROOT_SFX + "MinigameSurvival/SFX_Baseball_Bat_Swing";
            public const string SFX_KATANA_SLASH = ROOT_SFX + "MinigameSurvival/SFX_Katana_Slash";
            public const string SFX_SWORD_SLASH = ROOT_SFX + "MinigameSurvival/SFX_Sword_Slash";
            public const string SFX_PICKUP_WEAPON = ROOT_SFX + "MinigameSurvival/SFX_Pickup_Weapon";
            public const string SFX_WEAPON_DAMAGE = ROOT_SFX + "MinigameSurvival/SFX_Weapon_Damage";

            public const string SFX_COUNT_DOWN_8s = ROOT_SFX + "Countdown_Time";

            // Minigame06
            public const string SFX_MG06_CHEER = ROOT_SFX + "Minigame06/SFX_Cheer";
            public const string SFX_MG06_COUNTDOWN_TIME = ROOT_SFX + "Minigame06/SFX_Countdown_Time";
            public const string SFX_MG06_DESTINATION = ROOT_SFX + "Minigame06/SFX_Destination";
            public const string SFX_MG06_PAPER_PUNCH = ROOT_SFX + "Minigame06/SFX_Paper_punch";
            public const string SFX_MG06_STONE_DROP = ROOT_SFX + "Minigame06/SFX_Stone_drop";
            public const string SFX_MG06_CORRECT_CARD = ROOT_SFX + "Minigame06/SFX_Correct_Card";
            public const string SFX_MG06_FLIP_CARD = ROOT_SFX + "Minigame06/SFX_Flip_Card";
            public const string SFX_MG06_TARGET_FALL = ROOT_SFX + "Minigame06/SFX_Target_Fall";

            //MinigameMingle
            public const string ROOT_MINGLE = "MinigameMingle/";
            public const string SFX_MINGLE_RORATE = ROOT_SFX + ROOT_MINGLE + "BG_Mingle";
            public const string SFX_MINGLE_OPEN_DOOR = ROOT_SFX + ROOT_MINGLE + "SFX_Door_Open";
            public const string SFX_MINGLE_CLOSE_DOOR = ROOT_SFX + ROOT_MINGLE + "SFX_Door_Close";
            public const string SFX_MINGLE_TENSION = ROOT_SFX + ROOT_MINGLE + "SFX_Tension";
            public const string SFX_MINGLE_RANDOM = ROOT_SFX + ROOT_MINGLE + "SFX_Random";
            public const string SFX_MINGLE_TICK = ROOT_SFX + ROOT_MINGLE + "Time_Tick";

            //Minigame Balance Bridge
            public const string SFX_BALANCE_BRIDGE_FALL = ROOT_SFX + "MinigameBalanceBridge/SFX_Fall";
            //Minigame Marbles 2
            public const string ROOT_MARBLES2 = "MinigameMarbles2/";
            public const string SFX_MG_MARBLES_HIT_WALL = ROOT_SFX + ROOT_MARBLES2 + "SFX_Random";

            public const string ROOT_ROCKPAPERSCISSOR = "MinigameRockPaperScissor/";
            public const string SFX_ROCKPAPERSCISSOR_CORRECT = ROOT_SFX + ROOT_ROCKPAPERSCISSOR + "SFX_Correct_Choice";
            public const string SFX_ROCKPAPERSCISSOR_WRONG = ROOT_SFX + ROOT_ROCKPAPERSCISSOR + "SFX_Wrong_Choice";
            public const string SFX_ROCKPAPERSCISSOR_SHOOT_EMPTY = ROOT_SFX + ROOT_ROCKPAPERSCISSOR + "SFX_Empty_Gun";
            public const string SFX_ROCKPAPERSCISSOR_SPIN_GUN = ROOT_SFX + ROOT_ROCKPAPERSCISSOR + "SFX_Spin_gun";
            public const string SFX_ROCKPAPERSCISSOR_RESULT = ROOT_SFX + ROOT_ROCKPAPERSCISSOR + "SFX_Game_Draw";
        }

        public class SaveKey
        {
            public const string USER_DATA = "UserData";
        }

        public class TagName
        {
            public const string STONE = "Stone";
            public const string WALL = "Wall";
            public const string CHECK_POINT = "Check_Point";
            public const string HOLE = "Hole";
            public const string PLAYER = "Player";
            public const string BOT = "Bot";
        }
        public class MaterialPropertyName
        {
            public const string SATURATION = "_Saturation";
            public const string BRIGHTNESS = "_Brightness";
        }
    }
}
