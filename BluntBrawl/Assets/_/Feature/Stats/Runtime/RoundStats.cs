using UnityEngine;

[CreateAssetMenu(fileName = "RoundStats", menuName = "Stats/Round Stats")]
public class RoundStats : ScriptableObject
{
   public int m_maxRounds;
   public float m_preStartRoundTimer;
   public float m_maxRoundTime;
   public int m_requiredPlayers;


}
