using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace UniFramework.Localization
{
    public class LocaleIdentifier
    {
        /// <summary>
        /// 文化信息类
        /// </summary>
        public CultureInfo Culture { get; private set; }
        
        /// <summary>
        /// 文化唯一编码
        /// </summary>
        public string CultureCode { get; private set; }

        public LocaleIdentifier(SystemLanguage systemLanguage)
        {
            CultureCode = GetSystemLanguageCultureCode(systemLanguage);
            Culture = CultureInfo.GetCultureInfo(CultureCode);
        }

        public static string GetSystemLanguageCultureCode(SystemLanguage lang)
        {
            switch (lang)
            {
                case SystemLanguage.Afrikaans: return "af";
                case SystemLanguage.Arabic: return "ar";
                case SystemLanguage.Basque: return "eu";
                case SystemLanguage.Belarusian: return "be";
                case SystemLanguage.Bulgarian: return "bg";
                case SystemLanguage.Catalan: return "ca";
                case SystemLanguage.Chinese: return "zh-CN";
                case SystemLanguage.ChineseSimplified: return "zh-hans";
                case SystemLanguage.ChineseTraditional: return "zh-hant";
                case SystemLanguage.SerboCroatian: return "hr";
                case SystemLanguage.Czech: return "cs";
                case SystemLanguage.Danish: return "da";
                case SystemLanguage.Dutch: return "nl";
                case SystemLanguage.English: return "en";
                case SystemLanguage.Estonian: return "et";
                case SystemLanguage.Faroese: return "fo";
                case SystemLanguage.Finnish: return "fi";
                case SystemLanguage.French: return "fr";
                case SystemLanguage.German: return "de";
                case SystemLanguage.Greek: return "el";
                case SystemLanguage.Hebrew: return "he";
                case SystemLanguage.Hungarian: return "hu";
                case SystemLanguage.Icelandic: return "is";
                case SystemLanguage.Indonesian: return "id";
                case SystemLanguage.Italian: return "it";
                case SystemLanguage.Japanese: return "ja";
                case SystemLanguage.Korean: return "ko";
                case SystemLanguage.Latvian: return "lv";
                case SystemLanguage.Lithuanian: return "lt";
                case SystemLanguage.Norwegian: return "no";
                case SystemLanguage.Polish: return "pl";
                case SystemLanguage.Portuguese: return "pt";
                case SystemLanguage.Romanian: return "ro";
                case SystemLanguage.Russian: return "ru";
                case SystemLanguage.Slovak: return "sk";
                case SystemLanguage.Slovenian: return "sl";
                case SystemLanguage.Spanish: return "es";
                case SystemLanguage.Swedish: return "sv";
                case SystemLanguage.Thai: return "th";
                case SystemLanguage.Turkish: return "tr";
                case SystemLanguage.Ukrainian: return "uk";
                case SystemLanguage.Vietnamese: return "vi";
#if UNITY_2022_2_OR_NEWER
                case SystemLanguage.Hindi: return "hi";
#endif
                default: return "";
            }
        }

        public static SystemLanguage GetCultureCodeSystemLanguage(string CultureCode)
        {
            switch (CultureCode)
            {
                case "af": return SystemLanguage.Afrikaans;
                case "ar":return SystemLanguage.Arabic;
                case "eu":return SystemLanguage.Basque;
                case "be":return SystemLanguage.Belarusian;
                case "bg": return SystemLanguage.Bulgarian;
                case "ca": return SystemLanguage.Catalan;
                case "zh-CN": return SystemLanguage.Chinese;
                case "zh-hans": return SystemLanguage.ChineseSimplified;
                case "zh-hant": return SystemLanguage.ChineseTraditional;
                case "hr": return SystemLanguage.SerboCroatian;
                case "cs":return SystemLanguage.Czech;
                case "da":return SystemLanguage.Danish;
                case "nl":return SystemLanguage.Dutch;
                case "en":return SystemLanguage.English;
                case "et":return SystemLanguage.Estonian;
                case "fo":return SystemLanguage.Faroese;
                case "fi":return SystemLanguage.Finnish;
                case "fr":return SystemLanguage.French;
                case "de":return SystemLanguage.German;
                case "el":return SystemLanguage.Greek;
                case "he":return SystemLanguage.Hebrew;
                case "hu":return SystemLanguage.Hungarian;
                case "is":return SystemLanguage.Icelandic;
                case "id":return SystemLanguage.Indonesian;
                case "it":return SystemLanguage.Italian;
                case "ja":return SystemLanguage.Japanese;
                case "ko":return SystemLanguage.Korean;
                case "lv":return SystemLanguage.Latvian;
                case "lt":return SystemLanguage.Lithuanian;
                case "no":return SystemLanguage.Norwegian;
                case "pl":return SystemLanguage.Polish;
                case "pt":return SystemLanguage.Portuguese;
                case "ro":return SystemLanguage.Romanian;
                case "ru":return SystemLanguage.Russian;
                case "sk":return SystemLanguage.Slovak;
                case "sl":return SystemLanguage.Slovenian;
                case "es":return SystemLanguage.Spanish;
                case "sv":return SystemLanguage.Swedish;
                case "th":return SystemLanguage.Thai;
                case "tr":return SystemLanguage.Turkish;
                case "uk":return SystemLanguage.Ukrainian;
                case "vi": return SystemLanguage.Vietnamese;
#if UNITY_2022_2_OR_NEWER
                case "hi": return SystemLanguage.Hindi;
#endif
                default: return SystemLanguage.Unknown;
            }
        }

    }
}