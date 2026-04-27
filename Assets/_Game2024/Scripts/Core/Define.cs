using System.Collections.Generic;

namespace SquidGame.Core
{
    public static class Define
    {
        public const float DELAY_WIN = 3f;
        public const int CLAIM_ADS_MULTIPLY = 3;
        public const int DAY_REWARD = 1000;
        public const float EXTRA_TIME_BOOSTER_PERCENTAGE = 0.2f;

        public static readonly int[] CHALLENGE_MAX_DAY_CONFIG = new int[3] { 6, 8, 10 };
        public static readonly List<int> DEFAULT_MINIGAME_LIST = new() { 1, 2, 4, 3 };

        public class SceneName
        {
            public const string LOADING = "Loading";
            public const string CORE = "Core";
        }

        public class UIName
        {
            public const string MINIGAME_01_MENU = "Menu/Minigame01MenuUI";
            public const string MINIGAME_02_MENU = "Menu/Minigame02MenuUI";
            public const string MINIGAME_03_MENU = "Menu/Minigame03MenuUI";
            public const string MINIGAME_04_MENU = "Menu/Minigame04MenuUI";
            public const string MINIGAME_05_MENU = "Menu/Minigame05MenuUI";
            public const string MINIGAME_06_MENU = "Menu/Minigame06MenuUI";
            public const string MINIGAME_07_MENU = "Menu/Minigame07MenuUI";
            public const string MINIGAME_08_MENU = "Menu/Minigame08MenuUI";
            public const string MINIGAME_09_MENU = "Menu/Minigame09MenuUI";
            public const string MINIGAME_10_MENU = "Menu/Minigame10MenuUI";
            public const string MINIGAME_11_MENU = "Menu/Minigame11MenuUI";
            public const string MINIGAME_15_MENU = "Menu/Minigame15MenuUI";
            public const string MINIGAME_16_MENU = "Menu/Minigame16MenuUI";
            public const string MINIGAME_18_MENU = "Menu/Minigame18MenuUI";
            public const string MINIGAME_19_MENU = "Menu/Minigame19MenuUI";
            public const string MINIGAME_21_MENU = "Menu/Minigame21MenuUI";

            // Menu
            public const string HOME_MENU = "Menu/HomeMenuUI";
            public const string TRAINING_MODE_MENU = "Menu/TrainingModeMenuUI";

            // Popup
            public const string GAMEPLAY_POPUP = "Popup/GameplayPopupUI";
            public const string REVIVE_POPUP = "Popup/RevivePopupUI";
            public const string SHOP_POPUP = "Popup/ShopPopupUI";
            public const string SELECT_LANGUAGE_POPUP = "Popup/SelectLanguagePopupUI";
            public const string SETTINGS_POPUP = "Popup/SettingsPopupUI";
            public const string RATE_US_POPUP = "Popup/RateUsPopupUI";
            public const string NO_INTERNET_POPUP = "Popup/NoInternetPopupUI";
            public const string DAILY_REWARD_POPUP = "Popup/DailyRewardPopupUI";
            public const string REWARD_POPUP = "Popup/RewardPopupUI";
            public const string WARNING_POPUP = "Popup/WarningPopupUI";

            // ResultPopup
            public const string WIN_CHALLENGE_MODE_POPUP = "Popup/WinChallengeModePopupUI";
            public const string WIN_TRAINING_MODE_POPUP = "Popup/WinTrainingModePopupUI";
            public const string LOSE_CHALLENGE_MODE_POPUP = "Popup/LoseChallengeModePopupUI";
            public const string LOSE_TRAINING_MODE_POPUP = "Popup/LoseTrainingModePopupUI";
        }

        public class SoundPath
        {
            // BGM
            private const string ROOT_BGM = "BGM/";

            public const string BGM_MAIN = ROOT_BGM + "BGM_Main";
            public const string BGM_MINIGAME_01 = ROOT_BGM + "BGM_Game_1";
            public const string BGM_MINIGAME_02 = ROOT_BGM + "BGM_Game_2";
            public const string BGM_MINIGAME_03 = ROOT_BGM + "BGM_Game_3";

            // Minigame 21
            public const string BGM_MG21_INGAME = ROOT_BGM + "Minigame21/BGM_Ingame";
            public const string BGM_MG21_MINGLE = ROOT_BGM + "Minigame21/BGM_Mingle_Game";


            // SFX
            private const string ROOT_SFX = "SFX/";
            public const string SFX_BUTTON_CLICK = ROOT_SFX + "SFX_Click";
            public const string SFX_LOSING = ROOT_SFX + "SFX_Losing";
            public const string SFX_WIN_DANCE = ROOT_SFX + "SFX_Win_Dance";
            public const string SFX_WIN_SCREEN = ROOT_SFX + "SFX_Win_Screen";
            public const string SFX_REVIVE = ROOT_SFX + "SFX_Revive";
            public const string SFX_TING = ROOT_SFX + "SFX_Ting";
            public const string SFX_BUY_ITEM = ROOT_SFX + "SFX_Buy_Item";
            public const string SFX_EQUIP_ITEM = ROOT_SFX + "SFX_Equip_Item";
            public const string SFX_CLAIM_REWARD = ROOT_SFX + "SFX_Claim_Daily_Reward";

            // Minigame01
            public const string SFX_MG01_GUNSHOT = ROOT_SFX + "Minigame01/SFX_GunShot";
            public const string SFX_MG01_M4_SHOOT = ROOT_SFX + "Minigame01/SFX_M4_shoot";
            public const string SFX_MG01_HOSTAGE_F_HIT_01 = ROOT_SFX + "Minigame01/SFX_Hostage_F_Hit_01";
            public const string SFX_MG01_HOSTAGE_M_HIT_01 = ROOT_SFX + "Minigame01/SFX_Hostage_M_Hit_01";
            public const string SFX_MG01_SEARCHING = ROOT_SFX + "Minigame01/SFX_Searching";
            public const string SFX_MG01_SING = ROOT_SFX + "Minigame01/SFX_Sing";

            // Minigame02
            public const string SFX_MG02_GLASS_BREAK = ROOT_SFX + "Minigame02/SFX_Glass_Break";
            public const string SFX_MG02_JUMP = ROOT_SFX + "Minigame02/SFX_Jump";
            public const string SFX_MG02_SCREAM = ROOT_SFX + "Minigame02/SFX_Scream";
            public const string SFX_MG02_JUMP_ON_GLASS = ROOT_SFX + "Minigame02/SFX_Jump_On_Glass";

            // Minigame03
            public const string SFX_MG03_CHOOSE_CASE = ROOT_SFX + "Minigame03/SFX_Choose_Case";
            public const string SFX_MG03_BREAK_PART_IN = ROOT_SFX + "Minigame03/SFX_Break_Part_In";
            public const string SFX_MG03_BREAK_PART_OUT = ROOT_SFX + "Minigame03/SFX_Break_Part_Out";

            // Minigame04
            public const string SFX_MG04_THROW_MARBLE = ROOT_SFX + "Minigame04/SFX_Throw_Marble";
            public const string SFX_MG04_INTO_HOLE = ROOT_SFX + "Minigame04/SFX_Into_Hole";
            public const string SFX_MG04_FALL = ROOT_SFX + "Minigame04/SFX_MarbleFall";
            public const string SFX_MG04_MARBLE_COLLIDE = ROOT_SFX + "Minigame04/SFX_MarbleCollide";
            public const string SFX_MG04_MARBLE_DROP = ROOT_SFX + "Minigame04/SFX_MarbleDrop";

            // Minigame04
            public const string SFX_MG05_HEY = ROOT_SFX + "Minigame05/SFX_Hey";

            // Minigame06
            public const string SFX_MG06_HIT = ROOT_SFX + "Minigame06/SFX_Hit";
            public const string SFX_MG06_PUNCH = ROOT_SFX + "Minigame06/SFX_Punch";
            public const string SFX_MG06_PICKUP = ROOT_SFX + "Minigame06/SFX_Pickup_Item";

            // Minigame07
            public const string SFX_MG07_SMASH = ROOT_SFX + "Minigame07/SFX_Smash";

            // Minigame09
            public const string SFX_MG09_KNIFE_SLASH = ROOT_SFX + "Minigame09/SFX_Knife_Slash";

            // Minigame10
            public const string SFX_MG10_FALL = ROOT_SFX + "Minigame10/SFX_Fall";

            // Minigame11
            public const string SFX_MG11_JUMP = ROOT_SFX + "Minigame11/SFX_Jump";
            public const string SFX_MG11_CLING = ROOT_SFX + "Minigame11/SFX_Cling";
            public const string SFX_MG11_PICK_HITTING_ROCK = ROOT_SFX + "Minigame11/SFX_Pick_Hitting_Rock";

            // Minigame13
            public const string SFX_MG13_GAME_DRAW = ROOT_SFX + "Minigame13/SFX_Game_Draw";

            // Minigame14
            public const string SFX_MG14_SWAP = ROOT_SFX + "Minigame14/SFX_Swap";
            public const string SFX_MG14_CORRECT = ROOT_SFX + "Minigame14/SFX_Correct_Choice";
            public const string SFX_MG14_WRONG = ROOT_SFX + "Minigame14/SFX_Wrong_Choice";

            // Minigame15
            public const string SFX_MG15_MARBLES_HIT_WALL = ROOT_SFX + "Minigame15/SFX_Marbles_Hit_Wall";

            // Minigame16
            public const string SFX_MG16_LIGHT_ON = ROOT_SFX + "Minigame16/SFX_Light_On";

            // Minigame21
            public const string SFX_MG21_DOOR_CLOSE = ROOT_SFX + "Minigame21/SFX_Door_Close";
            public const string SFX_MG21_TIME_COUNTDOWN = ROOT_SFX + "Minigame21/SFX_Time_Countdown";
        }

        public class TagName
        {
            public const string PLAYER = "Player";
            public const string BOT = "Bot";
        }

        public class MaterialPropertyName
        {
            public const string SATURATION = "_Saturation";
        }
    }
}