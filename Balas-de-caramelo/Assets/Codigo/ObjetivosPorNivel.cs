using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetivosPorNivel : MonoBehaviour {

	//Variables
	public int enemigosADerrotar = 4;
	public string nombreDelNivel;
	public string nombreSiguienteNivel;

	//variables para hacer un spawn de enemigos
	public Transform[] fabricaDeEnemigos;
	public GameObject[] enemigos;
	public Transform[] posicionesAPatrullar;
	public int cantidad = 5;
	public float delay =1f;
	public float delayComun =5f; 

	//bool para niveles con jefe
	public bool jefe = false;



	void Start () {
		//Iniciamos la corutina en el start
		StartCoroutine (SpawnEnemigos ());

	}


	IEnumerator SpawnEnemigos(){

		//Este es el delay inicial previo a spawnear (por si quieren poner waves de enemigos que salgan cada X tiempo
		yield return new WaitForSeconds (delay);

		//Ahora vamos a spawnear enemigos utilizando un For, que se va a repetir siguiendo la variable cantidad
		for (int i = 0; i < cantidad; i++){
			//Para randomear una posicion dentro del array,  utilizamos el valor minimo (0) y el maximo (largo del array, Lenght)
			int posicionRandom = Random.Range (0, fabricaDeEnemigos.Length);
			//Ahora con los enemigos
			int enemigoRandom = Random.Range (0, enemigos.Length);
			//Instanciamos el enemigo al azar en esa posicion random que salio
			Instantiate (enemigos [enemigoRandom], 
				fabricaDeEnemigos [posicionRandom].position, 
				fabricaDeEnemigos [posicionRandom].rotation);

			//En vez de esperar un tiempo especifico, ahora el tiempo de espera va a ser aleatorio
			yield return new WaitForSeconds (delayComun);

		}


	}
}
