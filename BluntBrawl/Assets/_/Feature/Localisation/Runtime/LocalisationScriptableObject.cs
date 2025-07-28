using System.Collections.Generic;
using UnityEngine;

namespace Localisation.Runtime
{
    [CreateAssetMenu(fileName = "LocalisationScriptableObject", menuName = "Scriptable Objects/LocalisationSO")]
    public class LocalisationScriptableObject : ScriptableObject
    {
        public LocalisationData m_localisationData = new LocalisationData();
    }
}
