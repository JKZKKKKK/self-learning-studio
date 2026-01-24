using UnityEngine;

[CreateAssetMenu(menuName = "Card/CardData")]
public class CardData : ScriptableObject
{
    public int id;
    public string cardName;
    public string description;

    public Sprite cardSprite;

    public int basePower;
    public int addPowerPercent;
    public int learning;
    public int skillCooldown;
    public int HPRecover;
    public int recoverEffect;
    public int criticalHitRate;
    public int gainExperiencePoints;
    public int wisdom;
    public int homeworkCompletionSpeed;
    public int focus;
    public int forgettingSpeed;
    public int readingSpeed;
    public int understanding;
}
