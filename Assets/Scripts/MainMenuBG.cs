using UnityEngine;
using UnityEngine.UI;

public class MainMenuBG : MonoBehaviour
{
    Image backgroundImage;
    float ratio;

    void Start()
    {
        backgroundImage = GetComponent<Image>();
        ratio = backgroundImage.sprite.bounds.size.x / backgroundImage.sprite.bounds.size.y;
    }

    void Update()
    {
        if (!backgroundImage.rectTransform)
            return;

        if (Screen.height * ratio >= Screen.width)
        {
            backgroundImage.rectTransform.sizeDelta = new Vector2(Screen.height * ratio, Screen.height);
        }
        else
        {
            backgroundImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.width / ratio);
        }
    }
}
