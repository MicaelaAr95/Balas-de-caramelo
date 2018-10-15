using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager Instancia;

	//VARIABLES
	public int enemigosDerrotados;
	public float tiempoDeLaPartida;
	public string nivelActual;
	public string nivelSiguiente;

	public bool jefeDerrotado = false;
	public bool jugadorPerdio = false;

	//variable para ver los objetivos del nivel
	ObjetivosPorNivel objetivos;

	// para que solo haya un game manager por escena (singletón)
	void Awake () {
		if (Instancia == null) {
			Instancia = this;
		} else 
			Destroy (gameObject);

	}

	void Start (){
		//Resetea sus variables
		enemigosDerrotados = 0;

		//busca el script "objetivos" por el nivel
		objetivos = FindObjectOfType<ObjetivosPorNivel> ();

		//Cambia el nombre del nivel
		nivelActual = objetivos.nombreDelNivel;
		nivelSiguiente = objetivos.nombreSiguienteNivel;
	}

	void Update(){


		//Aca detectamos que clase de cosa tiene que hacer el jugador para superar el nivel

		if (objetivos.jefe == true){

			//el jugador gana si elimina al jefe
			if (jefeDerrotado == true){
				SceneManager.LoadScene ("Ganaste");
			}
		}//Si no hay jefe
		else if (objetivos.jefe == false){
			//el jugador gana si elimina a la cantidad de enemigos exigida en el nivel
			if (enemigosDerrotados >= objetivos.enemigosADerrotar){
				SceneManager.LoadScene ("Ganaste");
			}

		} 


		//Si se marca que el jugador perdió se lo envía a la escena de perder
		if (jugadorPerdio == true){
			SceneManager.LoadScene ("Perdiste");
		}
		//Si el jugador aprieta la tecla esc sale del juego
		if (Input.GetKey (KeyCode.Escape)){
			Application.Quit ();
		}

		//Si el jugador quiere puede volver al selector de niveles
		if (Input.GetKeyDown (KeyCode.Keypad0)) {
			SceneManager.LoadScene ("Selector de Niveles");
		}

	}

}
