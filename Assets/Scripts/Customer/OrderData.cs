using UnityEngine;

[CreateAssetMenu(fileName = "NewOrder", menuName = "Cafe/Order Data")]
public class OrderData : ScriptableObject
{
    public string itemName = "Masala Chai";
    public Sprite icon;
    public float patienceTime = 30f;   // customer kitni der wait karega
    public int coinReward = 10;
}
