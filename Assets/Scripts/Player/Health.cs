using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health;
    public int numberOfHearts = 5;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite EmptyHeart;
    private void Update()
    {
        if (health > numberOfHearts) //security
        {
            health = numberOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = EmptyHeart;
            if (i < numberOfHearts)
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;
        }
    }
}
