using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemigo : MonoBehaviour {

	public float vida = 40f;
	public float velocidad = 10f;

	//Los proyectiles del enemigo
	public GameObject bala;
	//la colección de balas
	public int fondoDeBalas = 10;
	List<GameObject> balas;

	//las partículas del Enemigo

	//Tiempo entre proyectil y proyectil
	public float tiempoDisparo = 1f;

	//mide el tiempo entre disparo y disparo
	float tiempoUltimoTiro;

	//Puntos a patrullar
	//public Transform[] posicionesAPatrullar;
	ObjetivosPorNivel posicionesPatrulla;

	//Estados
	public bool patrullando = true;
	public bool siguiendo = false;

	//Este es el rango en el cual el enemigo va a cambiar de estado
	public float rangoDeteccion = 5f;

	//A esta distancia va a empezar a atacar
	public float rangoAtaque = 1.5f;

	//Recompensas que caen cuando el enemigo es destruido
	public GameObject[] recompensas;

	//Clips de audio

	//En esta variable vamos a guardar cual es la posicion a la cual estamos yendo
	int index = 0;

	//variable para buscar al jugador
	Jugador personaje;

	//variable para buscar el NavMeshAgent
	NavMeshAgent agente;

	//variable para encontrar su collider
	SphereCollider bolaInvisible;

	//variable para controlar las animaciones

	//Reproductor de sonido
	AudioSource reproductor;

	void Start(){
		//actualiza las características del NavAgent
		agente = GetComponent <NavMeshAgent> ();
		agente.speed = velocidad;
		agente.stoppingDistance = rangoAtaque;

		//busca su collider
		bolaInvisible = GetComponentInChildren <SphereCollider>();

		//busca su objetivo en el nivel
		posicionesPatrulla = FindObjectOfType<ObjetivosPorNivel> ();

		//busca al jugador
		personaje = FindObjectOfType<Jugador> ();

		//fondo de balas
		balas = new List<GameObject> ();
		for (int i=0; i <fondoDeBalas; i ++){

			GameObject obj = (GameObject)Instantiate (bala);
			obj.SetActive (false);
			balas.Add (obj);
		}

	}


	void Update () {
		//Esto es para evitar errores en caso de no encontrar al jugador
		if (personaje == null){
			return;
		}

		//calcula la distancia entre el enemigo y el jugador
		float distanciaAlPersonaje = Vector3.Distance (transform.position, personaje.transform.position);

		//comprueba si el personaje entra al rango de detección
		//si está fuera de ese rango el enemigo sigue de patrulla
		if (distanciaAlPersonaje > rangoDeteccion){
			patrullando = true;
			siguiendo = false;
		}
		//si esta dentro del rango el enemigo sigue al jugador
		else{
			siguiendo = true;
			patrullando = false;
		}


		//Y si patrulla...
		if (patrullando == true && posicionesPatrulla.posicionesAPatrullar.Length > 0){
			print ("Patrullando");

			//calcula la distancia al punto de la patrulla
			float distanciaAlPunto = Vector3.Distance (	posicionesPatrulla.posicionesAPatrullar [index].position, 
				transform.position);


			//calcula si ya llegó al punto de patrullaje
			if (distanciaAlPunto > agente.stoppingDistance){

				agente.isStopped = false;
				agente.destination = posicionesPatrulla.posicionesAPatrullar [index].position;
				//animación de movimiento

			}

			else if (index < posicionesPatrulla.posicionesAPatrullar.Length - 1 ) {
				index++;
			}

			else{
				index = 0;
			}
			//Aca termina la lógica de patrullar
		}

		//Y si está siguiendo al jugador...
		if (siguiendo == true){

			//Lo sigue, mientras no supere el rango de ataque
			if (distanciaAlPersonaje > rangoAtaque){

				print ("Siguiendo");
				//va hacia el jugador
				agente.isStopped = false;
				agente.destination = personaje.transform.position;
				//animación

			}
			//Si llega al rango de ataque
			else{
				print ("Atacando");

				//se detiene
				agente.isStopped = true;
				//animación

				//Comprueba que haya pasado el "tiempoDisparo" entre bala y bala
				if (Time.time - tiempoUltimoTiro > tiempoDisparo){

					//Al momento de disparar guarda el tiempo del disparo
					tiempoUltimoTiro = Time.time;
					//Y dispara
					Disparar ();
				}

			}

		}
		//para ver si se desactiva el colllider
		print ("Collider.enabled = " + bolaInvisible.enabled);
		//Si la vida llega a cero...
		if (vida <= 0f){
			//desactiva su collider
			bolaInvisible.enabled = false;
			//se destruye dándole un tiempo a la animación
			Invoke ("Destruirse", 1f);
		}

	}
	//para ver los diferentes rangos en el editor de Unity
	void OnDrawGizmos(){
		//para el rango de detección
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, rangoDeteccion);
		//para el rango de ataque
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (transform.position, rangoAtaque);
	}


	void Disparar (){
		//apunta hacia el jugador
		transform.LookAt (personaje.transform.position);
		//reproduce un sonido

		//Llama a la bala
		for (int i=0; i < balas.Count; i ++){

			if (!balas[i].activeInHierarchy){

				balas [i].transform.position = transform.position;
				balas [i].transform.rotation = transform.rotation;
				balas [i].SetActive (true);
				break;

			}
		}
	}

	void Destruirse (){
		posicionesPatrulla.cantidad++;
		//suma puntos al GameManager
		GameManager.Instancia.enemigosDerrotados ++;
		print ("Llamó al game manager");
		Recompensa ();
		Destroy (gameObject);
	}

	//Función para dejar una recompensa al azar
	void Recompensa (){
		int recompensaRandom = Random.Range (0, recompensas.Length);
		Instantiate (recompensas[recompensaRandom], transform.position, transform.rotation);
	}
}
