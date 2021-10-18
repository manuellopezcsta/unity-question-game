using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MaxQuestions
{
    public int maxQuestionNumber;

    public MaxQuestions()
    {
        maxQuestionNumber = MakeQuestions.currentMaxQuestions;
    }
}
