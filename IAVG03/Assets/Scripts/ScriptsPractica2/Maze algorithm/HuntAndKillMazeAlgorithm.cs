using UnityEngine;
namespace UCM.IAV.Practica2 {
	public class HuntAndKillMazeAlgorithm : MazeAlgorithm {

		//Iteradores de la posición en el mapa de baldosas
		private int currentRow = 0;
		private int currentColumn = 0;
		//Flag de finalización
		private bool courseComplete = false;

		//imperfections
		//Parámetro para cálculo de numero de paredes (un número elevado borrará menos paredes)
		float pp = 14.0f;
		//Mínimo tamaño necesario para aplicar Imperfections() -> (NUNCA deberá ser mint < 3)
		int minT = 3;

		//Constructora
		public HuntAndKillMazeAlgorithm(MazeCell[,] mazeCells) : base(mazeCells) {
		}
		//Crea el laberinto final utilizando el algoritmo de Hunt and Kill
		public override void CreateMaze (bool perfectMaze) {
			HuntAndKill ();
			if (!perfectMaze)
			{
				Imperfections();
			}
		}

		private void HuntAndKill() {
			mazeCells [currentRow, currentColumn].visited = true;

			while (! courseComplete) {
				Kill(); // Se ejecutará hasta que encuentre un callejón sin salida
				Hunt(); // Busca la siguiente baldosa sin visitar adyacente a una visitada. Si no lo consigue, pone courseComplete a true.
			}
		}

		//Va moviendose por el mapa aleatoriamente destruyendo paredes  mientras haya una ruta disponible
		private void Kill() {
			while (RouteStillAvailable (currentRow, currentColumn)) {
				int direction = Random.Range (1, 5);

				if (direction == 1 && CellIsAvailable (currentRow - 1, currentColumn)) {
					// North
					DestroyWallIfItExists (mazeCells [currentRow, currentColumn].northWall);
					DestroyWallIfItExists (mazeCells [currentRow - 1, currentColumn].southWall);
					mazeCells[currentRow, currentColumn].walls[0] = true;
					if (currentRow - 1 >= 0)
						mazeCells[currentRow - 1, currentColumn].walls[1] = true;
					currentRow--;
				} else if (direction == 2 && CellIsAvailable (currentRow + 1, currentColumn)) {
					// South
					DestroyWallIfItExists (mazeCells [currentRow, currentColumn].southWall);
					DestroyWallIfItExists (mazeCells [currentRow + 1, currentColumn].northWall);
					mazeCells[currentRow, currentColumn].walls[1] = true;
					if (currentRow + 1 < mazeCells.GetLength(0))
						mazeCells[currentRow + 1, currentColumn].walls[0] = true;
					currentRow++;
				} else if (direction == 3 && CellIsAvailable (currentRow, currentColumn + 1)) {
					// east
					DestroyWallIfItExists (mazeCells [currentRow, currentColumn].eastWall);
					DestroyWallIfItExists (mazeCells [currentRow, currentColumn + 1].westWall);
					mazeCells[currentRow, currentColumn].walls[2] = true;
					if (currentColumn + 1 < mazeCells.GetLength(1))
						mazeCells[currentRow, currentColumn + 1].walls[3] = true;
					currentColumn++;
				} else if (direction == 4 && CellIsAvailable (currentRow, currentColumn - 1)) {
					// west
					DestroyWallIfItExists (mazeCells [currentRow, currentColumn].westWall);
					DestroyWallIfItExists (mazeCells [currentRow, currentColumn - 1].eastWall);
					mazeCells[currentRow, currentColumn].walls[3] = true;
					if (currentColumn - 1 >= 0)
						mazeCells[currentRow, currentColumn - 1].walls[2] = true;
					currentColumn--;
				}

				mazeCells [currentRow, currentColumn].visited = true;
			}
		}

		private void Hunt() {
			courseComplete = true; // Suponemos que hemos finalizado y comprobamos lo opuesto debajo

			for (int r = 0; r < mazeRows; r++) {
				for (int c = 0; c < mazeColumns; c++) {
					if (!mazeCells [r, c].visited && CellHasAnAdjacentVisitedCell(r,c)) {
						courseComplete = false; // Encontramos una baldosa asi que debemos Kill() de nuevo
						currentRow = r;
						currentColumn = c;
						DestroyAdjacentWall (currentRow, currentColumn);
						mazeCells [currentRow, currentColumn].visited = true;
						return;
					}
				}
			}
		}

		//Devuelve verdadero si hay alguna ruta disponible adyacente a la baldosa 
		private bool RouteStillAvailable(int row, int column) {
			int availableRoutes = 0;

			if (row > 0 && !mazeCells[row-1,column].visited) {
				availableRoutes++;
			}

			if (row < mazeRows - 1 && !mazeCells [row + 1, column].visited) {
				availableRoutes++;
			}

			if (column > 0 && !mazeCells[row,column-1].visited) {
				availableRoutes++;
			}

			if (column < mazeColumns-1 && !mazeCells[row,column+1].visited) {
				availableRoutes++;
			}

			return availableRoutes > 0;
		}

		//Devuelve verdadero si la posición seleccionada contiene una baldosa válida y sin visitar
		private bool CellIsAvailable(int row, int column) {
			if (row >= 0 && row < mazeRows && column >= 0 && column < mazeColumns && !mazeCells [row, column].visited) {
				return true;
			} else {
				return false;
			}
		}

		//Destruye la pared seleccionada
		private void DestroyWallIfItExists(GameObject wall) {
			if (wall != null) {
				GameObject.Destroy (wall);
			}
		}

		// Devuelve verdadero si hay alguna baldosa visitada adyacente a esta.
		private bool CellHasAnAdjacentVisitedCell(int row, int column) {
			int visitedCells = 0;

			// Mira una fila encima (N) si estamos en la fila 1 o superior
			if (row > 0 && mazeCells [row - 1, column].visited) {
				visitedCells++;
			}

			// Mira una fila debajo (S) si estamos en la penúltima fila o menos
			if (row < (mazeRows-2) && mazeCells [row + 1, column].visited) {
				visitedCells++;
			}

			//Mira una columna a la izquierda (W) si estamos en la columna 1 o superior
			if (column > 0 && mazeCells [row, column - 1].visited) {
				visitedCells++;
			}

			// Mira una columna a la derecha (E) si estamos en la penúltima columna o menos
			if (column < (mazeColumns-2) && mazeCells [row, column + 1].visited) {
				visitedCells++;
			}

			return visitedCells > 0;
		}
		//Destruye una pared aleatoria adyacente a la posición seleccionada y por tanto abre un posible camino -> mazeCells[row, column].walls[x] = true;
		private void DestroyAdjacentWall(int row, int column) {
			bool wallDestroyed = false;

			while (!wallDestroyed) {
				 int direction = Random.Range (1, 5);

				if (direction == 1 && row > 0 && mazeCells [row - 1, column].visited) {
					DestroyWallIfItExists (mazeCells [row, column].northWall);
					DestroyWallIfItExists (mazeCells [row - 1, column].southWall);
					mazeCells[row, column].walls[0] = true;
					if (row - 1 >= 0)
						mazeCells[row - 1, column].walls[1] = true;
					wallDestroyed = true;
				} else if (direction == 2 && row < (mazeRows-2) && mazeCells [row + 1, column].visited) {
					DestroyWallIfItExists (mazeCells [row, column].southWall);
					DestroyWallIfItExists (mazeCells [row + 1, column].northWall);
					mazeCells[row, column].walls[1] = true;
					if (row + 1 < mazeCells.GetLength(0))
						mazeCells[row + 1, column].walls[0] = true;
					wallDestroyed = true;
				} else if (direction == 3 && column > 0 && mazeCells [row, column-1].visited) {
					DestroyWallIfItExists (mazeCells [row, column].westWall);
					DestroyWallIfItExists (mazeCells [row, column-1].eastWall);
					mazeCells[row, column].walls[2] = true;
					if (column + 1 < mazeCells.GetLength(1))				
						mazeCells[row, column + 1].walls[3] = true;			
					wallDestroyed = true;
				} else if (direction == 4 && column < (mazeColumns-2) && mazeCells [row, column+1].visited) {
					DestroyWallIfItExists (mazeCells [row, column].eastWall);
					DestroyWallIfItExists (mazeCells [row, column+1].westWall);
					mazeCells[row, column].walls[3] = true;
					if (column-1 >= 0)				
						mazeCells[row, column - 1].walls[2] = true;				
					wallDestroyed = true;
				}
			}
		}

		//Destruye paredes pseudo-aleatorias del interior del laberinto para crear más de un camino posible para llegar al final
		private void Imperfections()
		{
			
			if (mazeRows < minT || mazeColumns < minT)
			{
				Debug.Log("No se puede crear un laberinto imperfecto de " + mazeRows + " x " + mazeColumns);
			}
			else
			{

				//Fórmula que calcula el número de paredes a borrar en función del tamaño del laberinto y el parámetro 'pp'
				int nparedes = Mathf.Abs(Mathf.RoundToInt(mazeRows * mazeColumns / pp));

				int row = 1;
				int column = 1;
				int j = 0; 
				bool gotwall = false;

				//Hasta haber borrado 'nparedes'...
				for (int i = 0; i < nparedes; i++)
				{
					//Mientras no encuentre una dirección con pared o no encuentre ninguna...
					while (!gotwall && j < 4)
					{
						if (mazeCells[row, column].walls[j] == false)
						{
							switch (j)
							{
								case 0:
									DestroyWallIfItExists(mazeCells[row, column].northWall);
									mazeCells[row, column].walls[0] = true;

									if (row - 1 >= 0)
									{
										DestroyWallIfItExists(mazeCells[row - 1, column].southWall);
										mazeCells[row - 1, column].walls[1] = true;

									}

									break;

								case 1:
									DestroyWallIfItExists(mazeCells[row, column].southWall);
									mazeCells[row, column].walls[1] = true;

									if (row + 1 < mazeRows)
									{
										DestroyWallIfItExists(mazeCells[row + 1, column].northWall);
										mazeCells[row + 1, column].walls[0] = true; 

									}

									break;

								case 2:
									DestroyWallIfItExists(mazeCells[row, column].eastWall);
									mazeCells[row, column].walls[2] = true;

									if(column + 1 < mazeColumns)
									{
										DestroyWallIfItExists(mazeCells[row, column + 1].westWall);
										mazeCells[row, column + 1].walls[3] = true;
									}

									break;

								case 3:
									DestroyWallIfItExists(mazeCells[row, column].westWall);
									mazeCells[row, column].walls[3] = true;

									if (column - 1 >= 0)
									{
										DestroyWallIfItExists(mazeCells[row, column - 1].eastWall);
										mazeCells[row, column - 1].walls[2] = true;
									}

									break;

								default:
									break;
							}
							gotwall = true;
						}
						else
						{
							j++;
						}
					}
					//Resets
					gotwall = false;
					j = 0;
					//Actualización de la siguiente baldosa
					row = Random.Range(row + 1, mazeRows - 2);
					if ( row >= mazeRows - 1)
						row = 1;

					column = Random.Range(1, mazeColumns - 1);			
				}
			}
		}
	}
}