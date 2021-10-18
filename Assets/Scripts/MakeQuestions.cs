    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Proyecto26;
using RSG;

public class MakeQuestions : MonoBehaviour
{
    public static string preguntaText;
    public static string answerA;
    public static string answerB;
    public static string answerC;
    public static string answerD;
    public static int correctOne;
    public static bool enableDebugMode;

    // Toggles
    [SerializeField] Toggle A;
    [SerializeField] Toggle B;
    [SerializeField] Toggle C;
    [SerializeField] Toggle D;

    // For the Question and Answers
    [SerializeField] TMP_InputField pregunta;
    [SerializeField] TMP_InputField inputA;
    [SerializeField] TMP_InputField inputB;
    [SerializeField] TMP_InputField inputC;
    [SerializeField] TMP_InputField inputD;

    // Para el maximo num de preguntas
    public static int currentMaxQuestions;
    private string whereIsItStored = "MAX";
    MaxQuestions questionsNumber = new MaxQuestions();

    // Para la screen
    [SerializeField] GameObject waitScreen;
    [SerializeField] GameObject inputScreen;
    [SerializeField] GameObject menuScreen;

    void Start()
    {
        RetrieveMaxQuestions();
    }

    
    public void SelectingCorrectAnswer()
    {
        correctOne = 0;
        if(A.isOn)
        {
            correctOne = 1;
        }
        if(B.isOn)
        {
            correctOne = 2;
        }
        if(C.isOn)
        {
            correctOne = 3;
        }
        if(D.isOn)
        {
            correctOne = 4;
        }
        if(enableDebugMode){
            Debug.Log(correctOne);
        }
        
    }

    public void OnSubmit()
    {
        preguntaText = pregunta.text;
        answerA = inputA.text;
        answerB = inputB.text;
        answerC = inputC.text;
        answerD = inputD.text;
        if(enableDebugMode){
            Debug.Log(preguntaText);
            Debug.Log(answerA);
            Debug.Log(answerB);
            Debug.Log(answerC);
            Debug.Log(answerD);
            Debug.Log(correctOne);
        }
        postToDatabase();        
    }

    void postToDatabase()
    {
        waitScreen.SetActive(true);
        // We get the max question number and use it to name the new id
        RetrieveMaxQuestions().Then(() => {    
            InfoPregunta infoPregunta = new InfoPregunta();
            RestClient.Put("https://questions-unity-game-default-rtdb.firebaseio.com/" + currentMaxQuestions + ".json", infoPregunta).Then((response) => {
                // We post the new max Question number
                return postCurrentMaxQuestions();
            }).Then((response)=>{
                //Debug.Log("XD corri");
                waitScreen.SetActive(false);
                menuScreen.SetActive(true);
                inputScreen.SetActive(false);
                     
            });
        });
    }

    IPromise<ResponseHelper> postCurrentMaxQuestions()
    {
        questionsNumber.maxQuestionNumber += 1;
        return RestClient.Put("https://questions-unity-game-default-rtdb.firebaseio.com/" + whereIsItStored + ".json", questionsNumber);
    }

    IPromise RetrieveMaxQuestions()
    {
        return RestClient.Get<MaxQuestions>("https://questions-unity-game-default-rtdb.firebaseio.com/" + whereIsItStored + ".json").Then(response =>
        {
            questionsNumber = response;
            UpdateMQ();
        }).Catch(response => {
			Debug.Log("Error " + response.Message);
            });
    }

    void UpdateMQ()
    {
        currentMaxQuestions = questionsNumber.maxQuestionNumber;
    }

    public void GetMQ()
    {
        RetrieveMaxQuestions();
    }
}
