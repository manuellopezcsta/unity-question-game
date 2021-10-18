using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using System;

[Serializable]
public class InfoPregunta
{
    public string preguntaText;
    public string answerA;
    public string answerB;
    public string answerC;
    public string answerD;
    public int correctOne;

    // Constructor
    public InfoPregunta()
    {
        preguntaText = MakeQuestions.preguntaText;
        answerA = MakeQuestions.answerA;
        answerB = MakeQuestions.answerB;
        answerC = MakeQuestions.answerC;
        answerD = MakeQuestions.answerD;
        correctOne = MakeQuestions.correctOne;
    }
}
