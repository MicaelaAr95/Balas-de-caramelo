using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubiertaEnemigo : MonoBehaviour {

	//El daño que hace la cubierta por segundo
	public float danioXsegundo = 5f;
	//multiplicador que reduce el daño
	public float reductorDeDanio = 0.5f;
	//lo que aguanta la cubierta
	public float resistencia = 10f;

	void Update (){

		//si la resistencia llega a 0 se destruye
		if (resistencia <= 0){
			Destroy (gameObject);
		}
	}

	//Detecta si algo está dentro de la cubierta
	void OnTriggerStay(Collider otro){
		Jugador jugador;
		//Detectamos si el objeto que estamos detectando es efectivamente el jugador
		if (otro.gameObject.tag == "Player") {
			//busca su script
			jugador = otro.gameObject.GetComponentInParent <Jugador> ();
			//le resta salud por segundo
			jugador.vidaActual -= danioXsegundo * Time.deltaTime;

			//si la vida del jugador llega a 0 lo destruye
			if (jugador.vidaActual <= 0){
				Destroy (otro.gameObject);
			}
		}
	}

}
