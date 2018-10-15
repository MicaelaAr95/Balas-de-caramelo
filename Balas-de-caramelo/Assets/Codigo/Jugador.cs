using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour {

	[Header ("Vida")]
	public float vida = 40f;

	[Header ("Velocidades")]
	public float velocidadNormal;
	public float multiplicadorInterno;
	public float fuerzaSalto = 2f;

	[Header("Armas")]

	//La cantidad inicial de cargadores de Caramelos
	public int cantidadCargadoresC = 2;
	//La cantidad maxima de cargadores de Caramelos que puede llevar el jugador
	public int cargadoresMaximosC = 4;

	[Header ("Formas")]
	//Forma humanoide
	public GameObject persona;
	//Forma esférica
	public GameObject bola;

	[Header ("Ataque bola")]
	public float danio = 40f;
	public float danioPropio = 5f;


	[Header ("Variables Temporales")]
	//La cantidad de vida actual que tiene el jugador
	public float vidaActual;
	//La cantidad de cargadores de Caramelos que tiene el jugador
	public int cargadoresActualesC;


	//Variables privadas
	PersonajeBolita variablesBola;

	//Para la transformación
	[HideInInspector]public bool esUnaBola;

	//Para el salto
	bool tocaElPiso = true;

	//Rigidbody al que se le aplica la fuerza del salto
	Rigidbody ribody;

	//Reproductor de sonido
	AudioSource reproductor;

	//Animator para controlar las animaciones
	Animator animador;

	void Awake (){
		ribody = GetComponent <Rigidbody>();

		//busca el Animator
		animador = GetComponentInChildren <Animator>();

	}
	void Start () {
		//se cargan las variables temporales
		vidaActual = vida;
		cargadoresActualesC = cantidadCargadoresC;

		//busca al reproducctor de sonido
		reproductor = Camera.main.GetComponent <AudioSource> ();
	}



	void Update () {

		//Transformación
		if (Input.GetKeyDown (KeyCode.F) && esUnaBola == false){
			persona.SetActive (false);
			bola.SetActive (true);
			esUnaBola = true;
		} else if (Input.GetKeyDown (KeyCode.F) && esUnaBola == true){
			bola.SetActive (false);
			persona.SetActive (true);
			esUnaBola = false;
		}


		//Movimiento WASD
		if (Input.GetKey (KeyCode.W)) {
			transform.Translate (0, 0, velocidadNormal * multiplicadorInterno * Time.deltaTime);
		} 

		if (Input.GetKey (KeyCode.S)) {
			transform.Translate (0, 0, - velocidadNormal * multiplicadorInterno * Time.deltaTime);
		} 

		if (Input.GetKey (KeyCode.D)) {
			transform.Translate (velocidadNormal * multiplicadorInterno * Time.deltaTime, 0, 0);
		} 

		if (Input.GetKey (KeyCode.A)) {
			transform.Translate (- velocidadNormal * multiplicadorInterno * Time.deltaTime, 0, 0);
		}

		//Reestablecer salud
		if 	(Input.GetKeyDown (KeyCode.E) && cargadoresActualesC > 0 && vidaActual != vida){
			//Partículas salud a tope...
			//Audio plim...
			vidaActual = vida;
			cargadoresActualesC --;
		}


		//Salto

		if (Input.GetKeyDown (KeyCode.Space) && tocaElPiso == true){
			ribody.AddForce (Vector3.up * fuerzaSalto, ForceMode.Impulse);
		}

	}
	void OnCollisionEnter (Collision pum){
		//ataque bola
		if (pum.gameObject.tag == "Enemigo" && esUnaBola == true) {
			print ("chocó bola con" + pum.gameObject);

			CubiertaEnemigo resistenciaCubierta;
			Enemigo vidaEnemigo;
			Animator animacionParpadeo;

			//carga sus variables
			resistenciaCubierta = pum.gameObject.GetComponentInChildren <CubiertaEnemigo> ();
			vidaEnemigo = pum.gameObject.GetComponent <Enemigo> ();
			animacionParpadeo = pum.gameObject.GetComponentInChildren <Animator> ();

			//si hay cubierta
			if (resistenciaCubierta != null){
				vidaEnemigo.vida -= danio * resistenciaCubierta.reductorDeDanio;
				resistenciaCubierta.resistencia -= danio;
				print ("Vida Enemigo" + vidaEnemigo.vida);
				animacionParpadeo.Play ("Parpadeo");

				//si la resistencia de la cubierta llega a 0 la destruye
				if (resistenciaCubierta.resistencia <= 0){
					Destroy (resistenciaCubierta.gameObject);
				}

				//si no hay cubierta
			} else if (resistenciaCubierta == null){
				vidaEnemigo.vida -= danio;
				print ("Vida Enemigo" + vidaEnemigo.vida);
				animacionParpadeo.Play ("Parpadeo");
			}

			vidaActual -= danioPropio;
			print ("Bola dañó a" + pum.gameObject);

		}


		//salto
		if (pum.gameObject.layer == LayerMask.NameToLayer ("Piso")){
			tocaElPiso = true;
			animador.SetBool ("enElAire",false);
			//Audio aterriza
		}

	}
	void OnCollisionExit (Collision pum){
		
		if (pum.gameObject.layer == LayerMask.NameToLayer ("Piso")){
			tocaElPiso = false;
			animador.SetBool ("enElAire", true);
		}
	}

	public bool AgregarCargadorC(){
		//Primero pregunto si la cantidad de cargadores actuales es menor a la cantidad máxima
		if (cargadoresActualesC < cargadoresMaximosC) {
			//Si esto es verdadero, le sumo uno al cargador
			cargadoresActualesC++;

			//La palabra return indica el valor que vamos a devolver, en este caso true
			return true;
		} 
		else{
			//Si el player tiene los cargadores llenos, devolvemos falso.
			print ("Cargadores de Caramelos Llenos");
			return false;
		}

	}
}
