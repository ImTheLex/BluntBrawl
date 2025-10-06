using UnityEngine;

[CreateAssetMenu(fileName = "RoundStats", menuName = "Stats/Round Stats")]
public class RoundStats : ScriptableObject
{
   public int m_maxRounds;
   public float m_maxRoundTime;
   public float m_waitForPlayerTimer;
   public float m_preStartRoundTimer;
   public float m_preRoundBreakTimer;
   public float m_preEndRoundTimer;
   public float m_preEndMatchTimer;
   public float m_preQuitMatchTimer;
   
   public int m_requiredPlayers;


}
