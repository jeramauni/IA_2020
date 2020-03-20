using UnityEngine;
namespace UCM.IAV.Practica2 {
	public class MazeCell {
		public bool visited = false;
		public GameObject northWall, southWall, eastWall, westWall, floor;
		public bool[] walls;

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