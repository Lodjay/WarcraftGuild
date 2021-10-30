using System;
using WarcraftGuild.Core.Enums;

namespace WarcraftGuild.Core.Extensions
{
    public static class RegionExtensions
    {
        public static Locale GetDefaultLocale(this Region region)
        {
            return region switch
            {
                Region.Europe => Locale.BritishEnglish,
                Region.Korea => Locale.Korean,
                Region.Taiwan => Locale.TraditionalChinese,
                Region.SoutheastAsia or Region.UnitedStates => Locale.AmericanEnglish,
                Region.China => Locale.SimplifiedChinese,
                _ => throw new NotImplementedException($"The {nameof(Region)} [{region}] does not have an associated {nameof(Locale)}"),
            };
        }

        public static bool IsLocaleAvailable(this Region region, Locale locale)
        {
            return region switch
            {
                Region.Europe => (Locale.BritishEnglish == locale) ||
                                    (Locale.SpainSpanish == locale) ||
                                    (Locale.French == locale) ||
                                    (Locale.Russian == locale) ||
                                    (Locale.German == locale) ||
                                    (Locale.PortugalPortuguese == locale) ||
                                    (Locale.Italian == locale),
                Region.Korea => Locale.Korean == locale,
                Region.Taiwan => Locale.TraditionalChinese == locale,
                Region.SoutheastAsia or Region.UnitedStates => (Locale.AmericanEnglish == locale) ||
                    (Locale.MexicanSpanish == locale) ||
                    (Locale.BrazilianPortuguese == locale),
                Region.China => Locale.SimplifiedChinese == locale,
                _ => throw new NotImplementedException($"The {nameof(Region)} [{region}] does not have an associated {nameof(Locale)}"),
            };
        }
    }
}