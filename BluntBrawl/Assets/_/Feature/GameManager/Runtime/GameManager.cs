using System;
using System.Collections.Generic;
using System.IO;
using FactSystem.Runtime;
using Localisation.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace GameManager.Runtime
{
    public class GameManager : MonoBehaviour
    {
        #region Publics


        public static FactDictionary m_factDictionary= new FactDictionary();

        public static GameManager m_instance;
        
        public static LocalisationScriptableObject m_locaSO => m_instance.m_localisationScriptable;

        public LocalisationScriptableObject m_localisationScriptable;
        
        public static UnityEvent m_OnLanguageChanged = new UnityEvent();
        
        
        #endregion
        
        #region Unity API

        private void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (m_instance != this)
                Destroy(this.gameObject);

            if (!m_factDictionary.FacExist("localisation", out string language))
            {
                string defaultLanguage = m_locaSO.m_localisationData.m_languages[0].m_languageKey;
                ChangeLanguage(defaultLanguage);
            }
        }

        #endregion
        
        #region Serializable Class
        
        
        [Serializable]
        public class SerializebleFact
        {
            public string key;
            public string value;
            public string assemblyTypeName;
        }

        [Serializable]
        public class ToJson
        {
            public List<SerializebleFact> facts;
        }
        
        
        #endregion
        
        #region Main Methods

        public static void SavePersistentFacts()
        {
            List<SerializebleFact> facts = new List<SerializebleFact>();
            foreach (var fact in m_factDictionary.AllFacts)
            {
                if (!fact.Value.IsPersistent) continue;

                SerializebleFact serializebleFact = new SerializebleFact();
                
                serializebleFact.key = fact.Key;
                
                Type factType = fact.Value.GetObjectValue().GetType();
                if (factType.IsPrimitive || factType == typeof(string))
                {
                    var value = fact.Value.GetObjectValue();
                    serializebleFact.value = value.ToString();
                }
                else serializebleFact.value = JsonUtility.ToJson(fact.Value.GetObjectValue());

                serializebleFact.assemblyTypeName = fact.Value.GetObjectValue().GetType().AssemblyQualifiedName;
                
                facts.Add(serializebleFact);
            }
            
            ToJson toJson = new ToJson();
            toJson.facts = facts;
            string data = JsonUtility.ToJson(toJson);
            File.WriteAllText(Application.persistentDataPath + "/save.json", data);
        }

        public static void SetFactsOnLoad()
        {
            string path = Application.persistentDataPath + "/save.json";

            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            ToJson fromJson = JsonUtility.FromJson<ToJson>(json);
            
            List<SerializebleFact> facts = new List<SerializebleFact>();
            facts = fromJson.facts;

            foreach (var fact in facts)
            {
                Type type = Type.GetType(fact.assemblyTypeName);
                
                if (type == typeof(string))
                {
                    m_factDictionary.SetFact(fact.key, fact.value, FactDictionary.FactPersistence.persistent);
                }
                else if (type.IsPrimitive)
                {
                    var value = Convert.ChangeType(fact.value,type);
                    CreateGenericSetFact(type, fact, value);
                }
                else
                {
                    var value = JsonUtility.FromJson(fact.value, type);
                    CreateGenericSetFact(type, fact, value);
                }
            }
        }
        
        //Language
        public static List<string> GetAllLanguages()
        {
            List<string> languages = new List<string>();
            foreach (LocalisationData.Language lang in m_locaSO.m_localisationData.m_languages)
            {
                languages.Add(lang.m_languageKey);
            }
            return languages;
        }
        
        public static bool ChangeLanguage(string language)
        {
            foreach (LocalisationData.Language lang in m_locaSO.m_localisationData.m_languages)
            {
                if (lang.m_languageKey == language)
                {
                    m_factDictionary.SetFact("localisation", language, FactDictionary.FactPersistence.persistent);
                    m_OnLanguageChanged.Invoke();
                    return true;
                }
            }
            throw new InvalidDataException("No language found");
        }

        public static string GetTextLocalised(string key)
        {
            string language = m_factDictionary.GetFact<string>("localisation");

            for (int i = 0; i < m_locaSO.m_localisationData.m_languages.Count; i++)
            {
                if (m_locaSO.m_localisationData.m_languages[i].m_languageKey == language)
                {
                    foreach (LocalisationData.TextLoca textLoca in m_locaSO.m_localisationData.m_languages[i].m_textLoca)
                    {
                        if (textLoca.m_textKey == key) return textLoca.m_textValue;
                    }
                }
            }
            return null;
        }
        

        #endregion
        
        #region Utils
        
        
        private static void CreateGenericSetFact(Type type, SerializebleFact fact, object value)
        {
            var setFactMethod = m_factDictionary.GetType().GetMethod("SetFact")
                .MakeGenericMethod(type);
            setFactMethod.Invoke(m_factDictionary, new object[] { fact.key, value, FactDictionary.FactPersistence.persistent });
        }
        
        
        #endregion

        #region Private and Protected


        

        #endregion

    }
}
