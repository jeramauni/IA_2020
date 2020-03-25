using UnityEngine;
namespace UCM.IAV.Practica2 {
	//Clase baldosa
	public class MazeCell {
		// Flag usado en el algoritmo de generación del laberinto
		public bool visited = false;
		// Variables para A*
		// F = G + H
		private float finalCost;
		private float realCost;
		private float heuristicCost;
		// Position (x, z)
		public int x;
		public int z;
		// Rederencia a la celda de la que viene
		private MazeCell padre;
		// Paredes alrededor de la baldosa
		public GameObject northWall, southWall, eastWall, westWall, floor;
		// Vector de booleanos de las paredes que la rodean. (True = no hay pared, False = Hay pared -> No se puede pasar)
		public bool[] walls;
		// Constructora -> Inicializa el vector de paredes a false
		public MazeCell() {
			walls = new bool[4]; // N, S, E, O
			for (int i = 0; i < walls.Length; i++) {
				walls[i] = false;
			}
		}
		// Getters
		public float getG() { return realCost; }
		public float getH() { return heuristicCost; }
		public float getF() { return finalCost; }
		public MazeCell getPadre() { return padre; }
		// Setters
		public void setG(float g) { realCost = g; }
		public void setH(float h) { heuristicCost = h; }
		public void setF(float g, float h) { finalCost = g + h; }
		public void setPadre(MazeCell p) { padre = p; }
	}
}