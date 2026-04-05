using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using CEmoji;
using UnityEditor.U2D;
using UnityEngine.U2D;
using UnityEngine.UI;

[CustomEditor(typeof(EmojiAtlas))]
public class EmojiAtlasEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var mono = target as EmojiAtlas;
        if (GUILayout.Button("更新emoji unicode和特定int id对应关系"))
        {
            // mono.emoji2codeDic = new SerializableDictionary<string, int>();
            updateEmojiName2CodeDic(mono);
        }
        
        if (GUILayout.Button("自动更新"))
        {
            updateSprites(mono);
            updateEmojiName2CodeDic(mono);
        }
    }

    private List<Sprite> sprites = new List<Sprite>();
    private void updateSprites(EmojiAtlas mono)
    {
        if (mono != null)
        {
            sprites = new List<Sprite>();
            var guids = AssetDatabase.FindAssets("t:Sprite", new[] {mono.spritesPath});
            foreach (var guid in guids)
            {
                var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(guid));
                if(sprite != null)
                {
                    sprites.Add(sprite);
                }
            }
        }
    }

    private void updateEmojiName2CodeDic(EmojiAtlas mono)
    {
        if (mono != null && sprites != null)
        {
            int emojiSeq = EmojiAtlas.EMOJI_SEQ_START;

                
            List<int> usedCodes = new List<int>();
            int maxUsedCodes = EmojiAtlas.EMOJI_SEQ_START - 1;
            foreach (var emoji2Code in mono.m_EmojiSpriteName2Code)
            {
                usedCodes.Add(emoji2Code.Value);
                if (emoji2Code.Value > maxUsedCodes)
                {
                    maxUsedCodes = emoji2Code.Value;
                }
            }
            List<bool> isCodeUsed = new List<bool>();

            for (int i = EmojiAtlas.EMOJI_SEQ_START; i < maxUsedCodes; i++)
            {
                isCodeUsed.Add(usedCodes.Contains(i));
            }
                

            foreach (var sprite in sprites)
            {
                if (sprite != null)
                {
                    string strEmojiKey = sprite.name;
						
                    if(mono.m_EmojiSpriteName2Code.ContainsKey(strEmojiKey))
                    {
                        continue;
                    }

                    bool outMaxUsedCodes = true;
                    int getCode = 0;
                    for (var i = 0; i < isCodeUsed.Count; i++)
                    {
                        var isUse = isCodeUsed[i];
                        if (!isUse)
                        {
                            outMaxUsedCodes = false;
                            isCodeUsed[i] = true;
                            getCode = EmojiAtlas.EMOJI_SEQ_START + i;
                            break;
                        }
                    }

                    if (outMaxUsedCodes)
                    {
                        maxUsedCodes++;
                        getCode = maxUsedCodes;
                    }
                    mono.m_EmojiSpriteName2Code[strEmojiKey] = getCode;
                }
            }
            mono.m_EmojiSpriteName2Code.Sort();
            EditorUtility.SetDirty(mono);
        }
    }
    
}