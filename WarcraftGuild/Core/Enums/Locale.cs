using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarcraftGuild.Core.Extensions;

namespace WarcraftGuild.Core.Enums
{
    public enum Locale
    {
        [EnumCode("en_GB")]
        BritishEnglish,
        [EnumCode("ko_KR")]
        Korean,
        [EnumCode("zh_TW")]
        TraditionalChinese,
        [EnumCode("en_US")]
        AmericanEnglish,
        [EnumCode("es_MX")]
        MexicanSpanish,
        [EnumCode("pt_BR")]
        BrazilianPortuguese,
        [EnumCode("de_de")]
        German,
        [EnumCode("es_ES")]
        SpainSpanish,
        [EnumCode("fr_FR")]
        French,
        [EnumCode("it_IT")]
        Italian,
        [EnumCode("pt_PT")]
        PortugalPortuguese,
        [EnumCode("ru_RU")]
        Russian,
        [EnumCode("zh_CN")]
        SimplifiedChinese,
    }
}
