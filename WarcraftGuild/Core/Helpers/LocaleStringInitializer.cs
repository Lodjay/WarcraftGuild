using System.Collections.Generic;
using WarcraftGuild.Core.Enums;
using WarcraftGuild.Core.Models;

namespace WarcraftGuild.Core.Helpers
{
    public static class LocaleStringInitializer
    {
        public static List<LocaleString> Generate()
        {
            List<LocaleString> locales = new()
            {
                BAJsonEmpty(), BANotFound(), BAForbidden(), BALimit(), UnknownError()
            };
            return locales;
        }

        private static LocaleString BAJsonEmpty()
        {
            return new LocaleString
            {
                Code = LocaleStringCode.BLIZZ_API_RESPONSE_EMPTY,
                Values = new Dictionary<Locale, string>
                {
                    { Locale.AmericanEnglish , "[Blizzard API] Empty response."},
                    { Locale.BritishEnglish , "[Blizzard API] Empty response."},
                    { Locale.French , "[API Blizzard] Réponse vide."},
                }
            };
        }

        private static LocaleString BANotFound()
        {
            return new LocaleString
            {
                Code = LocaleStringCode.BLIZZ_API_RESPONSE_NOT_FOUND,
                Values = new Dictionary<Locale, string>
                {
                    { Locale.AmericanEnglish , "[Blizzard API] Data not found."},
                    { Locale.BritishEnglish , "[Blizzard API] Data not found."},
                    { Locale.French , "[API Blizzard] Donnée introuvable."},
                }
            };
        }

        private static LocaleString BAForbidden()
        {
            return new LocaleString
            {
                Code = LocaleStringCode.BLIZZ_API_RESPONSE_FORBIDDEN,
                Values = new Dictionary<Locale, string>
                {
                    { Locale.AmericanEnglish , "[Blizzard API] Forbidden data access."},
                    { Locale.BritishEnglish , "[Blizzard API] Forbidden data access."},
                    { Locale.French , "[API Blizzard] Accès aux données interdite."},
                }
            };
        }

        private static LocaleString BALimit()
        {
            return new LocaleString
            {
                Code = LocaleStringCode.BLIZZ_API_LIMIT_REACHED,
                Values = new Dictionary<Locale, string>
                {
                    { Locale.AmericanEnglish , "[Blizzard API] Call Limit reached. Try again later."},
                    { Locale.BritishEnglish , "[Blizzard API] Call Limit reached. Try again later."},
                    { Locale.French , "[API Blizzard] Limite d'appels atteinte. Veuillez réessayer plus tard."},
                }
            };
        }

        private static LocaleString UnknownError()
        {
            return new LocaleString
            {
                Code = LocaleStringCode.UNKNOWN_ERROR,
                Values = new Dictionary<Locale, string>
                {
                    { Locale.AmericanEnglish , "Unknown error."},
                    { Locale.BritishEnglish , "Unknown error."},
                    { Locale.French , "Erreur inconnue."},
                }
            };
        }
    }
}