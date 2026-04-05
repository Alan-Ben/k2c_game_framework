using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class WCGSfxAnimation : MonoBehaviour {

    public SpriteRenderer animSprite;
    public List<Sprite> spriteList;
    public bool loop;
    public float interval;

    private int index = 0;
    private float nextTime = 0;

    // Use this for initialization
    void OnEnable () {
        index = 0;
        nextTime = Time.time + interval;
        forceRender();
    }

    // Update is called once per frame
    void Update ()
    {
        if (animSprite == null || spriteList.Count <= 1)
            return;

        if (Time.time < nextTime)
            return;

        nextTime += interval;
        ++index;

        if (loop)
            index %= spriteList.Count;

        if (index >= spriteList.Count)
            animSprite.sprite = null;
        else
            animSprite.sprite = spriteList[index];
    }

    private void forceRender()
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
