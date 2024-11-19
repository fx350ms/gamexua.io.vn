using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace GameXuaVN.Localization
{
    public static class GameXuaVNLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(GameXuaVNConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(GameXuaVNLocalizationConfigurer).GetAssembly(),
                        "GameXuaVN.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
