using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class WCGSfxSpriteAnimation : MonoBehaviour {

    public Image animSprite;
    public List<Sprite> spriteList;
    public bool loop;
    public float interval;

    private int index = 0;
    private float time = 0;

    // Use this for initialization
    void OnEnable () {
        index = 0;
        time = Time.time;
        Render();
    }

    // Update is called once per frame
    void Update () {
        if (Time.time - time < interval)

            return;

        time += interval;
        ++index;
        Render();
    }

    private void Render()
    {
        int count = spriteList.Count;
        if (animSprite == null || count == 0)
            return;

        if (loop)
            index %= count;

        if (index >= count)
            animSprite.sprite = null;
        else
            animSprite.sprite = spriteList[index];
    }
}
