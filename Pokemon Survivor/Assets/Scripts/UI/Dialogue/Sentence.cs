using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Emotion
{
    neutral,
    sad,
    angry,
    happy,
    veryHappy,
    emotional
}
[System.Serializable]
public class Sentence
{
    public bool isQuestion;
    public bool shoping;
    public Emotion emotion = Emotion.neutral;

    [TextArea(3, 9)]
    public string sentence;
}
