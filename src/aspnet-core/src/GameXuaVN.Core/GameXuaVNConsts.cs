using GameXuaVN.Debugging;

namespace GameXuaVN
{
    public class GameXuaVNConsts
    {
        public const string LocalizationSourceName = "GameXuaVN";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = false;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "c219563b5462456bbfd59d83d9739aa8";
    }
}
