
using UnityEngine;

public class EmojiTest : MonoBehaviour
{
    public string emojiStr = "As😁😀Ba😁😀你好😁😀";
    public TextEmoji text;
    
    // Use this for initialization
    void OnEnable () {
        if (text != null)
        {
            text.text = emojiStr;
            // text.text = "As😁😀Ba😁😀你好😁😀";
        }
    }

}
