using UnityEngine;
using System.IO;

namespace UCM.IAV.Practica2 {
	//Cargador del laberinto al iniciar la partida
	public class MazeLoader : MonoBehaviour {
		//Numero de filas y columnas
		public int mazeRows, mazeColumns;
		//Prefab de paredes y suelo
		public GameObject wallGo;
		public GameObject floorGo;
		//Camara
		private Camera camera;
		//Tamaño de los objetos del laberinto (Usado para reescalar el laberinto)
		public float size = 1.25f;
		//Mapa de baldosas y elementos del laberinto
		public MazeCell[,] mazeCells;
		//Indicador del tipo de laberinto deseado (true = un solo camino posible de inicio a fin / false = posibilidad de que haya más de un solo camino posible)
		public bool perfectMaze = true;
		void Start () {
			//Coloca la cámara en una posición acorde al tamaño del laberinto
			camera = Camera.main;
			PlaceCamera();
			//Instancia los objetos del laberinto en la escena
			InitializeMaze ();
			//Construye el algoritmo generador del laberinto pasandole 'mazeCells' como el mapa de baldosas a utilizar 
			MazeAlgorithm ma = new HuntAndKillMazeAlgorithm (mazeCells);
			//Crea el laberinto definitivo usando el algoritmo de Hunt And Kill
			ma.CreateMaze (perfectMaze);
			//(OPCIONAL) Imprime el mapa de baldosas en "Assets/debug.txt"
			DebugMaze();
		}
		
		private void PlaceCamera()
		{
			camera.transform.position = new Vector3((mazeRows - 1) * 1.25f / 2.0f, camera.transform.position.y, camera.transform.position.z); //(mazeColumns - 1) * 1.25f / 2.0f
		}

		//Instancia todos los posibles objetos del laberinto y les asigna un nombre rellenando el mapa de baldosas
		private void InitializeMaze() {

			mazeCells = new MazeCell[mazeRows,mazeColumns];

			//Para cada posición posible del mapa de baldosas...
			for (int r = 0; r < mazeRows; r++) {
				for (int c = 0; c < mazeColumns; c++) {
					mazeCells [r, c] = new MazeCell ();
					//Crea una baldosa...
					mazeCells[r, c].floor = Instantiate(floorGo, new Vector3(r * size, -(size / 2f), c * size), Quaternion.identity) as GameObject;
					//Le asigna un nombre único...
					mazeCells[r, c].floor.name = "Floor " + r + "," + c;

					//Y crea las paredes correspondientes 
					if (c == 0)
					{
						mazeCells[r, c].westWall = Instantiate(wallGo, new Vector3(r * size, 0, (c * size) - (size / 2f)), Quaternion.identity) as GameObject;
						mazeCells[r, c].westWall.name = "West Wall " + r + "," + c;
					}

					mazeCells[r, c].eastWall = Instantiate(wallGo, new Vector3(r * size, 0, (c * size) + (size / 2f)), Quaternion.identity) as GameObject;
					mazeCells[r, c].eastWall.name = "East Wall " + r + "," + c;

					if (r == 0)
					{
						mazeCells[r, c].northWall = Instantiate(wallGo, new Vector3((r * size) - (size / 2f), 0, c * size), Quaternion.identity) as GameObject;
						mazeCells[r, c].northWall.name = "North Wall " + r + "," + c;
						mazeCells[r, c].northWall.transform.Rotate(Vector3.up * 90f);

					}

					mazeCells[r, c].southWall = Instantiate(wallGo, new Vector3((r * size) + (size / 2f), 0, c * size), Quaternion.identity) as GameObject;
					mazeCells[r, c].southWall.name = "South Wall " + r + "," + c;
					mazeCells[r, c].southWall.transform.Rotate(Vector3.up * 90f);
				}
			}
		}

		//Genera un archivo debug.txt en "Assets/" conteniendo una representación del mapa de baldosas
		private void DebugMaze ()
		{
			StreamWriter writer = new StreamWriter("Assets/debug.txt", true);
			writer.WriteLine("Debug: Maze " + mazeRows + "x" + mazeColumns);
			for (int i = 0; i < mazeCells.GetLength(0); i++)
			{
				for (int j = 0; j < mazeCells.GetLength(1); j++)
				{
					for (int k = 0; k < 4; k++)
					{
						writer.Write(mazeCells[i, j].walls[k] + "-");
					}
					writer.Write(" | ");

				}
				writer.Write('\n');
			}
			writer.WriteLine();
			writer.Close();
		}
	}
}