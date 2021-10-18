using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.EventSystems;
using UnityEngine.UI;
using Proyecto26;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject ScreenA;
    [SerializeField] GameObject ScreenReloj;
    [SerializeField] GameObject gameOverScreen;
    
    LoadPregunta questionInfoScript;
    [SerializeField] Color correctColor;
    [SerializeField] Color incorrectColor;
    [SerializeField] AudioSource sounds;
    [SerializeField] AudioSource menuMusicSource;
    [SerializeField] AudioClip correct;
    [SerializeField] AudioClip incorrect;
    [SerializeField] AudioClip menuMusic;
    [SerializeField] GameObject buttonBlocker;

    // Para jugar
    int totalPoints = 0;
    int NofQuestionsBeingAsked = 10;

    // Para la game Over Screen
    [SerializeField] TMP_Text gameOverScoreText;
    [SerializeField] Sprite medallaOro;
    [SerializeField] Sprite medallaPlata;
    [SerializeField] Sprite medallaBronze;
    [SerializeField] Image medalla;


    void Start()
    {
        questionInfoScript = ScreenA.GetComponent<LoadPregunta>();
        //sounds = gameObject.GetComponent<AudioSource>();
        questionInfoScript.NofQuestionsBeingAsked = NofQuestionsBeingAsked;
        PlayMenuMusic(menuMusic, 0.7f, true);
    }

    public void CheckAnswer(Button boton)
    {
        int correctButton = questionInfoScript.GetCorrectAnswer();
        //string buttonName = EventSystem.current.currentSelectedGameObject.name;
        string buttonName = boton.name;
        buttonBlocker.SetActive(true);
        

        if(buttonName == "BotonA" && correctButton == 1)
        {
            ////Debug.Log("Respuesta Correcta !! A");
            StartCoroutine(ChangeButtonColorsTo(boton, true));
            totalPoints += 1;
            PlaySound(correct, 1);            
        }
        else if(buttonName == "BotonB" && correctButton == 2)
        {
            ////Debug.Log("Respuesta Correcta !! B");
            StartCoroutine(ChangeButtonColorsTo(boton, true));
            totalPoints += 1;
            PlaySound(correct, 1);      
        }
        else if(buttonName == "BotonC" && correctButton == 3)
        {
            ////Debug.Log("Respuesta Correcta !! C");
            StartCoroutine(ChangeButtonColorsTo(boton, true));
            totalPoints += 1;
            PlaySound(correct, 1);             
        }
        else if(buttonName == "BotonD" && correctButton == 4)
        {
            ////Debug.Log("Respuesta Correcta !! D");
            StartCoroutine(ChangeButtonColorsTo(boton, true));
            totalPoints += 1;
            PlaySound(correct, 1);      
        } else{
            ////Debug.Log("Respuesta Incorrecta !! , Correcta era: " + correctButton + buttonName);
            StartCoroutine(ChangeButtonColorsTo(boton, false));
            PlaySound(incorrect, 1);      
        }
        // Aumentamos el num de la pregunta
        questionInfoScript.currentQuestionNumber += 1;
        // Despues de la pregunta ruidito y ... mandamos la siguiente
        StartCoroutine(ShowCountdownScreen());
        if(questionInfoScript.currentQuestionNumber <= NofQuestionsBeingAsked)
        {
            StartCoroutine(questionInfoScript.LoadInfoInScreenForX(questionInfoScript.currentQuestionNumber-1, 2));
        } else{
            StartCoroutine(ShowGameOverScreen());
        }
    }

    IEnumerator ChangeButtonColorsTo(Button boton,bool isItCorrect)
    {
        ColorBlock cb;
        cb = boton.colors;

        Color holderNC = cb.normalColor;
        Color holderHC = cb.highlightedColor;
        Color holderSC = cb.selectedColor;

        if(isItCorrect)
        {
            cb.normalColor = correctColor;
            cb.highlightedColor = correctColor;
            cb.selectedColor = correctColor;
        } else{
            cb.normalColor = incorrectColor;
            cb.highlightedColor = incorrectColor;
            cb.selectedColor = incorrectColor;
        }
        boton.colors = cb;

        // Cambiamos de vuelta el color a normal
        yield return new WaitForSeconds(2);
        cb.normalColor = holderNC;
        cb.highlightedColor = holderHC;
        cb.selectedColor = holderSC;

        boton.colors = cb;

    }

    IEnumerator ShowCountdownScreen()
    {
        // Wait 1s before swapping screen for sounds
        yield return new WaitForSeconds(1);
        ScreenReloj.SetActive(true);
        // Wait for the animation
        yield return new WaitForSeconds(4);
        buttonBlocker.SetActive(false);
        ScreenReloj.SetActive(false);
    }

    IEnumerator ShowGameOverScreen()
    {
        updateGOScreen();
        // Wait 1s before swapping screen for sounds
        yield return new WaitForSeconds(1);
        gameOverScreen.SetActive(true);
    }

    void PlaySound( AudioClip clip, float vol, bool doesItLoop = false)
    {
        sounds.clip = clip;
        sounds.PlayOneShot(clip, vol);
        sounds.loop = doesItLoop;
    }

    void PlayMenuMusic( AudioClip clip, float vol, bool doesItLoop = false)
    {
        menuMusicSource.loop = doesItLoop;
        menuMusicSource.clip = clip;
        menuMusicSource.volume = vol;
        menuMusicSource.Play();
    }

    public void StartGame()
    {
        questionInfoScript.Preload10Questions();
    }

    

    private void updateGOScreen()
    {
        // Seteamos el score dependiendo del resultado.
        if(totalPoints <=5 )
        {
            medalla.sprite = medallaBronze;
        }
        else if(totalPoints >= 6 && totalPoints <= 8 )
        {
            medalla.sprite = medallaPlata;
        }
        else if(totalPoints >= 9 )
        {
            medalla.sprite = medallaOro;
        }
        // Seteamos el texto.
        gameOverScoreText.text = totalPoints + "/" + NofQuestionsBeingAsked;
    }
}
