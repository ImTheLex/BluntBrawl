using FactSystem.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Foundation.Runtime
{
    public class FBehaviour : MonoBehaviour
    {
        #region Unity API

         
        
        

        #endregion
        
        
        #region Utils
        
        
        //Facts
        protected bool HasFact<T>(string key, out T value)
        {
            return GameManager.Runtime.GameManager.m_factDictionary.FacExist(key, out value);
        }

        protected T GetFact<T>(string key)
        {
            return GameManager.Runtime.GameManager.m_factDictionary.GetFact<T>(key);
        }

        protected void SetFact<T>(string key, T value,bool isSaved = false)
        {
            FactDictionary.FactPersistence state = (FactDictionary.FactPersistence)(isSaved ? 1 : 0);
            GameManager.Runtime.GameManager.m_factDictionary.SetFact<T>(key, value,state);
        }

        protected void RemoveFact(string key)
        {
            GameManager.Runtime.GameManager.m_factDictionary.RemoveFact(key);
        }
        
        //Scene Loader
        protected void ChangeScene(string sceneName) => SceneManager.LoadScene(sceneName);
        
        protected void ChangeScene(int buildIndex) => SceneManager.LoadScene(buildIndex);
        
        //Save and Load System
        protected void SaveFact() => GameManager.Runtime.GameManager.SavePersistentFacts(); 

        protected void LoadFact() => GameManager.Runtime.GameManager.SetFactsOnLoad();
        
        //Choose Languages
        protected void SetLanguage(string language)=> GameManager.Runtime.GameManager.ChangeLanguage(language);

        protected string GetText(string key) => GameManager.Runtime.GameManager.GetTextLocalised(key);
        
        //Math Tools
        protected float Remap (float from, float fromMin, float fromMax, float toMin, float toMax) =>
            (from - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
        
        #endregion
        
        
    }
}
