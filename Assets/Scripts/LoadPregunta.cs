using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Proyecto26;
using RSG;
using System.Linq;
using System.Threading.Tasks;

public class LoadPregunta : MonoBehaviour
{

    [SerializeField] TMP_Text pregunta;
    [SerializeField] TMP_Text respA;
    [SerializeField] TMP_Text respB;
    [SerializeField] TMP_Text respC;
    [SerializeField] TMP_Text respD;
    private string whereIsItStored = "MAX";
    [SerializeField] GameObject waitScreen;

    InfoPregunta dataPregunta = new InfoPregunta();
    MaxQuestions numOfQuestions = new MaxQuestions();
    int maxRange;

    private List <int> randomList = new List<int>();
    private List<InfoPregunta> preguntaList = new List<InfoPregunta>();
    public int currentQuestionNumber = 1;
    public int NofQuestionsBeingAsked = 0; // NO TOCAR SE CAMBIA DESDE MAIN MENU, SOLUCIOn FEA PERO RAPIDA.



    IPromise GetMQs()
    {
        return RestClient.Get<MaxQuestions>("https://questions-unity-game-default-rtdb.firebaseio.com/" + whereIsItStored + ".json").Then(response =>
        {
            numOfQuestions = response;
            maxRange = numOfQuestions.maxQuestionNumber;
            ////Debug.Log("Max Range is: " + maxRange);
        });
        /*return*/ 
    }

    void ReturnRandomQuestion()
    {
        int chosen = Random.Range(0, maxRange);
        ////Debug.Log("Number Chosen: " + chosen);
        RestClient.Get<InfoPregunta>("https://questions-unity-game-default-rtdb.firebaseio.com/" + chosen + ".json").Then(response =>
        {
            dataPregunta = response;
        }).Then(() => {
            LoadInfoInScreen();
            waitScreen.SetActive(false);
        });
    }

    public void Preload10Questions()
    {
        
        waitScreen.SetActive(true);
        //Get Max questions
        GetMQs().Then(() => {
        //Choose 10 numbers from that
        
            for(int x = 0; x< NofQuestionsBeingAsked; x++)  // SI X es mayor al numero de entradas en la base de datos se rompe todo.
            {
                int chosen = Random.Range(0, maxRange);
                while(randomList.Contains(chosen))
                {
                    chosen = Random.Range(0, maxRange);
                }
                randomList.Add(chosen);
                //Debug.Log("Se agrego N " + chosen + " a la lista");
            }
            // Add them to the list                      
        }).Then(() => {
            Promise<InfoPregunta>.All(randomList.Select(index => 
                RestClient.Get<InfoPregunta>("https://questions-unity-game-default-rtdb.firebaseio.com/" + index + ".json")
            )).Done(list => {
                preguntaList = list.ToList();
                // Mostramos la primera pregunta;
                StartCoroutine(LoadInfoInScreenForX(0, 0.1f));
                waitScreen.SetActive(false);
                //preguntaList.ForEach(e => Debug.Log(e.preguntaText));
                
            });
        });        
    }

    void LoadInfoInScreen()
    {
        pregunta.text = dataPregunta.preguntaText;
        respA.text = dataPregunta.answerA;
        respB.text = dataPregunta.answerB;
        respC.text = dataPregunta.answerC;
        respD.text = dataPregunta.answerD;
        Debug.Log("Done Loading " + dataPregunta.preguntaText);
    }

    public IEnumerator LoadInfoInScreenForX(int index, float time)
    {
        yield return new WaitForSeconds(time);
        pregunta.text = preguntaList[index].preguntaText;
        respA.text = preguntaList[index].answerA;
        respB.text = preguntaList[index].answerB;
        respC.text = preguntaList[index].answerC;
        respD.text = preguntaList[index].answerD;
        ////Debug.Log("Done Loading " + preguntaList[index].preguntaText);
    }

    void IniciateQuestion()
    {
        waitScreen.SetActive(true);
        ReturnRandomQuestion();
    }

    public int GetCorrectAnswer()
    {
        return System.Convert.ToInt32(preguntaList[currentQuestionNumber-1].correctOne);        
    }
}
