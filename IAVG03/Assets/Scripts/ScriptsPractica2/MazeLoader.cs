using UnityEngine;
using System.IO;

namespace UCM.IAV.Practica2 {
	public class MazeLoader : MonoBehaviour {
		public int mazeRows, mazeColumns;
		public GameObject wallGo;
		public GameObject floorGo;

		public float size = 2f;

		public MazeCell[,] mazeCells;

		// Use this for initialization
		void Start () {
			InitializeMaze ();

			MazeAlgorithm ma = new HuntAndKillMazeAlgorithm (mazeCells);
			ma.CreateMaze ();
			DebugMaze();
		}
		
		private void InitializeMaze() {

			mazeCells = new MazeCell[mazeRows,mazeColumns];

			for (int r = 0; r < mazeRows; r++) {
				for (int c = 0; c < mazeColumns; c++) {
					mazeCells [r, c] = new MazeCell ();
					
					mazeCells[r, c].floor = Instantiate(floorGo, new Vector3(r * size, -(size / 2f), c * size), Quaternion.identity) as GameObject;
					mazeCells[r, c].floor.name = "Floor " + r + "," + c;

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

		private void DebugMaze ()
		{
			StreamWriter writer = new StreamWriter("Assets/test.txt", true);
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