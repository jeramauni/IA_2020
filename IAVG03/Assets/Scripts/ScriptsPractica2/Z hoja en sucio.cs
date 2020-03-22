// // Metodo que encuentra el elemento mas "cercano"
// MazeCell getNearestCell(List<MazeCell> open, MazeCell[,] maze, MazeCell celdaActual) {
//     MazeCell celdaMasCercana = null;
//     MazeCell celda;
//     float costeMasBarato = 0;
//     // Comprobar manualmente si puede ir a cada una de las 8 direcciones (diagonales incluidas)
//     // Celda arriba
//     if (celdaActual.walls[2]) {
//         celda = maze[celdaActual.x, celdaActual.z + 1];
//         celda.setG(10);
//         celda.setH(heuristica(celda.x, celda.z));
//         // Asignar provisionalmente la celda izquierda como la más cercana
//         // costeMasBarato = getF();
//         costeMasBarato = 10 + heuristica(celda.x, celda.z);
//         celdaMasCercana = celda;

//     }
//     // Celda abajo
//     if (celdaActual.walls[3]) {
//         celda = maze[celdaActual.x, celdaActual.z - 1];
//         celda.setG(10);
//         celda.setH(heuristica(celda.x, celda.z));
//         // Asignar provisionalmente la celda izquierda como la más cercana
//         // float coste = getF();
//         float coste = 10 + heuristica(celda.x, celda.z);
//         if (costeMasBarato == 0 || costeMasBarato > coste) {
//             costeMasBarato = coste;
//             celdaMasCercana = celda;
//         }
//     }
//     // Celda derecha
//     if (celdaActual.walls[0]) {
//         celda = maze[celdaActual.x + 1, celdaActual.z];
//         celda.setG(10);
//         celda.setH(heuristica(celda.x, celda.z));
//         // Asignar provisionalmente la celda izquierda como la más cercana
//         // float coste = getF();
//         float coste = 10 + heuristica(celda.x, celda.z);
//         if (costeMasBarato == 0 || costeMasBarato > coste) {
//             costeMasBarato = coste;
//             celdaMasCercana = celda;
//         }
//     }
//     // Celda izquierda
//     if (celdaActual.walls[1]) {
//         celda = maze[celdaActual.x - 1, celdaActual.z];
//         celda.setG(10);
//         celda.setH(heuristica(celda.x, celda.z));
//         // Asignar provisionalmente la celda izquierda como la más cercana
//         // float coste = getF();
//         float coste = 10 + heuristica(celdaMasCercana.x, celdaMasCercana.z);
//         if (costeMasBarato == 0 || costeMasBarato > coste) {
//             costeMasBarato = coste;
//             celdaMasCercana = celda;
//         }
//     }
//     // Celda arriba derecha
//     if (celdaActual.walls[0] && maze[celdaActual.x + 1, celdaActual.z].walls[2] 
//         && celdaActual.walls[2] && maze[celdaActual.x, celdaActual.z + 1].walls[0]) {
//         celda = maze[celdaActual.x + 1, celdaActual.z - 1];
//         celda.setG(Mathf.Sqrt(10 * 10 + 10 * 10));
//         celda.setH(heuristica(celda.x, celda.z));
//         // Asignar provisionalmente la celda izquierda como la más cercana
//         // float coste = getF();
//         float coste = Mathf.Sqrt(10 * 10 + 10 * 10) + heuristica(celda.x, celda.z);
//         if (costeMasBarato == 0 || costeMasBarato > coste) {
//             costeMasBarato = coste;
//             celdaMasCercana = celda;
//         }
//     }
//     // Celda arriba izquierda
//     if (celdaActual.walls[1] && maze[celdaActual.x - 1, celdaActual.z].walls[2] 
//         && celdaActual.walls[2] && maze[celdaActual.x, celdaActual.z + 1].walls[1]) {
//         celda = maze[celdaActual.x + 1, celdaActual.z - 1];
//         celda.setG(Mathf.Sqrt(10 * 10 + 10 * 10));
//         celda.setH(heuristica(celda.x, celda.z));
//         // Asignar provisionalmente la celda izquierda como la más cercana
//         // float coste = getF();
//         float coste = Mathf.Sqrt(10 * 10 + 10 * 10) + heuristica(celda.x, celda.z);
//         if (costeMasBarato == 0 || costeMasBarato > coste) {
//             costeMasBarato = coste;
//             celdaMasCercana = celda;
//         }
//     }
//     // Celda abajo derecha
//     if (celdaActual.walls[0] && maze[celdaActual.x + 1, celdaActual.z].walls[3] 
//         && celdaActual.walls[3] && maze[celdaActual.x, celdaActual.z - 1].walls[0]) {
//         celda = maze[celdaActual.x + 1, celdaActual.z - 1];
//         celda.setG(Mathf.Sqrt(10 * 10 + 10 * 10));
//         celda.setH(heuristica(celda.x, celda.z));
//         // Asignar provisionalmente la celda izquierda como la más cercana
//         // float coste = getF();
//         float coste = Mathf.Sqrt(10 * 10 + 10 * 10) + heuristica(celda.x, celda.z);
//         if (costeMasBarato == 0 || costeMasBarato > coste) {
//             costeMasBarato = coste;
//             celdaMasCercana = celda;
//         }
//     }
//     // Celda abajo izquierda
//     if (celdaActual.walls[1] && maze[celdaActual.x - 1, celdaActual.z].walls[3] 
//         && celdaActual.walls[3] && maze[celdaActual.x, celdaActual.z - 1].walls[1]) {
//         celda = maze[celdaActual.x - 1, celdaActual.z - 1];
//         celda.setG(10);
//         celda.setH(heuristica(celda.x, celda.z));
//         // Asignar provisionalmente la celda izquierda como la más cercana
//         // float coste = getF();
//         float coste = Mathf.Sqrt(10 * 10 + 10 * 10) + heuristica(celda.x, celda.z);
//         if (costeMasBarato == 0 || costeMasBarato > coste) {
//             costeMasBarato = coste;
//             celdaMasCercana = celda;
//         }
//     }
//     return celdaMasCercana;
// }

// // Si es el nodo bueno, acabar la busqueda
// if (currentCell == end) {
//     foreach (MazeCell c in close) {
//         camino.Add(c);
//     }
// }
// // Si no, coger sus conexiones
// bool [] conections = mazeLoader.mazeCells[currentCell.x, currentCell.z].walls;
// // Recorrer todas las conexiones de la casilla
// for (int i = 0; i < conections.Length; ++i){
//     // Conseguir el coste hasta esa casilla entre el acumulado y lo que costaria llegar hasta alli
//     MazeCell endCell = null;
//     if (conections[i]) {
//         switch (i) {
//             case 2: endCell = mazeLoader.mazeCells[currentCell.x, currentCell.z + 1]; break;
//             case 3: endCell = mazeLoader.mazeCells[currentCell.x, currentCell.z - 1]; break;
//             case 1: endCell = mazeLoader.mazeCells[currentCell.x + 1, currentCell.z]; break;
//             case 0: endCell = mazeLoader.mazeCells[currentCell.x - 1, currentCell.z];break;
//         }
//     }
//     float endCellCost = costeActual + endCell.finalCost;

//     // Si la celda esta en la lista de close, tal vez hay que saltarlo o quitarlo de la lista 'close'
//     if (close.Contains(endCell)) {
//         // Aqui encontramos en la lista 'close' el nodo que ha sido registrado
//         MazeCell endNodeRecord = close.Find(x => x.finalCost == endCellCost);

//         // Si nuestra ruta no es mejor, entonces no seguir
//         float endNodeRecordCost = costeActual + endNodeRecord.finalCost;
//         if (endNodeRecordCost <= endCellCost) {
//             continue;
//         }
//         // Quitarlo de la lista 'close'
//         close.Remove(endNodeRecord);
//         // Podemos usar el coste de los valores viejos para calcular su heuristica sin llamar a la funcion que calcula la heuristica
//         //float endCellHeuristic =
//     }
// }