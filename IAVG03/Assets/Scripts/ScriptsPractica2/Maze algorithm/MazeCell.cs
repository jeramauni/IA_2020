using UnityEngine;
namespace UCM.IAV.Practica2 {
	//Clase baldosa
	public class MazeCell {
		public bool visited = false;										//Flag usado en el algoritmo de generación del laberinto
		public int finalCost;
		public int currentCost;
		public int heuristicCost;
		public int x;
		public int z;
		public GameObject northWall, southWall, eastWall, westWall, floor;	//Paredes alrededor de la baldosa
		public bool[] walls;												//Vector de booleanos de las paredes que la rodean. (True = no hay pared, False = Hay pared -> No se puede pasar)

		//Constructora -> Inicializa el vector de paredes a false
		public MazeCell()
		{
			walls = new bool[4]; //N,S,E,O
			for (int i = 0; i < walls.Length; i++)
			{
				walls[i] = false;
			}
		}
	}
}