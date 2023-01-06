using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject soundGameOver;
    public GameObject gameOverPanel;
    public GameObject endGamePanel;
    public int countMonedas=0;
    public int countPoints=0;
    public Text coinText, pointText, timerText;
    float timer= 600;

    void Start(){
        gameOverPanel.SetActive(false); //Deshabilitamos el mensaje de Game Over.
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
        
        if (timer > 0.0f){
            timer -= Time.deltaTime;
            string minutes = Mathf.Floor(timer / 60).ToString("00");
            string seconds = Mathf.Clamp((timer % 60), 0, 59).ToString("00");
            timerText.text = "Tiempo: " + (string.Format("{0}:{1}", minutes, seconds));
            if (timer <= 30)
                timerText.color = Color.red;
        }else
            GameOver();
    }

    //Función para mostrar el mensaje de Game Over
    public void GameOver() {
        GetComponent<AudioSource>().Stop();
        print("GameOver"); //Aqui escrivimos el mensaje
        Time.timeScale = 0; //paralizamos el tiempo del juego, es decir, congelamos la imagen
        gameOverPanel.SetActive(true); //Activamos el panel donde vamos a escribir el mensaje
        Instantiate(soundGameOver);
    }

    internal void GameWin(){
        GetComponent<AudioSource>().Stop();
        print("EndGame"); //Aqui escrivimos el mensaje
        Time.timeScale = 0; //paralizamos el tiempo del juego, es decir, congelamos la imagen
        endGamePanel.SetActive(true); //Activamos el panel donde vamos a escribir el mensaje
    }


    public void AddCoints(){
        countMonedas++;
        coinText.text = "Monedas: " + countMonedas;
    }

    public void AddPoints(int points){
        countPoints = countPoints + points;
        pointText.text = "Puntos: " + countPoints;
    }

}