using UnityEngine;

public enum IngredientType 
{ 
    MatchaPowder, 
    Milk, 
    Sweetener 
}

public class IngredientPacket : MonoBehaviour
{
    public IngredientType type;
    public float currentAmount = 100f; // Hidden percentage (Starts at 100%)

    // Machine se transfer amount mangne par ye function calculate karega
    public float TransferIngredient(float neededAmount)
    {
        if (currentAmount >= neededAmount)
        {
            currentAmount -= neededAmount;
            CheckIfEmpty();
            return neededAmount;
        }
        else
        {
            float transferred = currentAmount;
            currentAmount = 0f;
            CheckIfEmpty();
            return transferred;
        }
    }

    private void CheckIfEmpty()
    {
        if (currentAmount <= 0f)
        {
            Destroy(gameObject); // Packet khali hote hi gayab ho jayega
        }
    }
}