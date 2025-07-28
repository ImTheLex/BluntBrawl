using System.Collections.Generic;
using System.IO;
using Localisation.Runtime;
using UnityEditor;
using UnityEngine;

namespace Localisation.Editor
{
    public class CreateLocalisationSO : MonoBehaviour
    {
    [MenuItem("Assets/Create/Mytools/Localisation")]
    public static void CreateOnSelection()
    {
        Object[] selectedObjects = Selection.objects;
    
        foreach (Object obj in selectedObjects)
        {
            if (obj is TextAsset textAsset)
            {
                string path = AssetDatabase.GetAssetPath(textAsset);
                if (path.EndsWith(".csv")) CreateSO(path);
                else throw new InvalidDataException("TextAsset must have .CSV extension");
            }
            else throw new InvalidDataException("Only TextAsset files are supported");
        }
    }
    
    
    public static void CreateSO(string path)
    {
        LocalisationScriptableObject localisationScriptableObject = ScriptableObject.CreateInstance<LocalisationScriptableObject>();
        string assetName = Path.GetFileNameWithoutExtension(path);
        localisationScriptableObject.name = assetName;
        
        TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
        string[] lines = textAsset.text.Split('\n');
        
        //create language
        string[] languages = lines[0].Split(','); 
        LocalisationData localisationData = new LocalisationData();
        for (int i = 1; i < languages.Length; i++)
        {
            LocalisationData.Language language = new LocalisationData.Language();
            language.m_languageKey = languages[i];
            localisationData.m_languages.Add(language);
        }

        for (int i = 1; i < lines.Length; i++)
        {
            string[] fields = lines[i].Split(',');
            for (int lang = 0; lang < languages.Length-1; lang++)
            {
                if (fields[0].StartsWith('%')) continue;
                LocalisationData.TextLoca textLoca = new LocalisationData.TextLoca();
                textLoca.m_textKey = fields[0];
                textLoca.m_textValue = fields[lang+1];
                localisationData.m_languages[lang].m_textLoca.Add(textLoca);
            }
        }
        localisationScriptableObject.m_localisationData = localisationData;
        AssetDatabase.CreateAsset(localisationScriptableObject, "Assets/_/DataBase/LocalisationSO/" + localisationScriptableObject.name + ".asset");
    }
    }
}
