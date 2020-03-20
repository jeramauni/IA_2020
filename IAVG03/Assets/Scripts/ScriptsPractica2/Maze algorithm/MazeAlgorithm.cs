namespace UCM.IAV.Practica2 {

	//Estructura usada en el algoritmo generador del laberinto 
	public abstract class MazeAlgorithm {
		//Mapa de baldosas
		protected MazeCell[,] mazeCells;
		//Filas y columnas del laberinto
		protected int mazeRows, mazeColumns;

		//Constructora
		protected MazeAlgorithm(MazeCell[,] mazeCells) : base() {
			this.mazeCells = mazeCells;
			mazeRows = mazeCells.GetLength(0);
			mazeColumns = mazeCells.GetLength(1);
		}

		//Método que creará el laberinto definitivo
		public abstract void CreateMaze ();
	}
}