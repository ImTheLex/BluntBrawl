using System;
using System.Collections.Generic;

namespace Localisation.Runtime
{
    [Serializable]
    public class LocalisationData
    {
        [Serializable]
        public class TextLoca
        {
            public string m_textKey;
            public string m_textValue;
        }
        
        
        [Serializable]
        public class Language
        {
            public string m_languageKey;
            public List<TextLoca> m_textLoca = new List<TextLoca>();
        }
        
        public List<Language> m_languages = new List<Language>();
    }
}
