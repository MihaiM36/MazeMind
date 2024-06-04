/*Tutorial: https://www.youtube.com/watch?v=_aeYq5BmDMg*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private MazeCell _mazeCellPrefab; // Prefab pentru celulele labirintului

    [SerializeField]
    private GameObject _obstaclePrefab; // Prefab pentru obstacole

    [SerializeField]
    private GameObject _playerPrefab; // Prefab pentru jucator

    [SerializeField]
    private GameObject _endPointPrefab; // Prefab pentru punctul final

    [SerializeField]
    private int _mazeWidth; // Latimea labirintului

    [SerializeField]
    private int _mazeDepth; // Adancimea labirintului

    [SerializeField]
    private int _numberOfObstacles; // Numarul de obstacole de generat

    private MazeCell[,] _mazeGrid; // Matricea celulelor labirintului
    private GameObject playerInstance; // Referinta la instanta jucatorului

    void Start()
    {
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth]; // Initializarea matricei de celule

        // Generarea celulelor labirintului
        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity);
            }
        }

        // Generarea drumurilor prin labirint
        GenerateMaze(null, _mazeGrid[0, 0]);

        // Plasarea obstacolelor
        PlaceObstacles();

        // Plasarea jucatorului si a punctului final
        Vector3 startPosition = PlacePlayer();
        PlaceEndPoint(startPosition);

        // Asigurarea urmaririi camerei pentru jucator
        SetupCamera();
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit(); // Marcarea celulei curente ca vizitata
        ClearWalls(previousCell, currentCell); // Curatarea peretilor dintre celule

        new WaitForSeconds(0.05f); // Pauza de 0.05 secunde

        MazeCell nextCell;

        // Algoritm de backtracking pentru generarea drumurilor
        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell); // Recursiv pentru urmatoarea celula
            }
        } while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell); // Obtinerea celulelor nevizitate adiacente

        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault(); // Returnarea unei celule aleatorii
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

        // Verificarea celulei din dreapta
        if (x + 1 < _mazeWidth)
        {
            var cellToRight = _mazeGrid[x + 1, z];

            if (cellToRight.IsVisited == false)
            {
                yield return cellToRight;
            }
        }

        // Verificarea celulei din stanga
        if (x - 1 >= 0)
        {
            var cellToLeft = _mazeGrid[x - 1, z];

            if (cellToLeft.IsVisited == false)
            {
                yield return cellToLeft;
            }
        }

        // Verificarea celulei din fata
        if (z + 1 < _mazeDepth)
        {
            var cellToFront = _mazeGrid[x, z + 1];

            if (cellToFront.IsVisited == false)
            {
                yield return cellToFront;
            }
        }

        // Verificarea celulei din spate
        if (z - 1 >= 0)
        {
            var cellToBack = _mazeGrid[x, z - 1];

            if (cellToBack.IsVisited == false)
            {
                yield return cellToBack;
            }
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null)
        {
            return;
        }

        // Curatarea peretelui din dreapta
        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        // Curatarea peretelui din stanga
        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        // Curatarea peretelui din fata
        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        // Curatarea peretelui din spate
        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }

    private void PlaceObstacles()
    {
        List<Vector2> availablePositions = new List<Vector2>();

        // Adaugarea pozitiilor disponibile pentru obstacole
        for (int x = 1; x < _mazeWidth - 1; x++)
        {
            for (int z = 1; z < _mazeDepth - 1; z++)
            {
                availablePositions.Add(new Vector2(x, z));
            }
        }

        int obstaclesPlaced = 0;

        // Plasarea obstacolelor
        while (obstaclesPlaced < _numberOfObstacles && availablePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, availablePositions.Count);
            Vector2 position = availablePositions[randomIndex];
            availablePositions.RemoveAt(randomIndex);

            Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0); // Rotire aleatorie la 0, 90, 180 sau 270 grade
            GameObject obstacle = Instantiate(_obstaclePrefab, new Vector3(position.x, 0.45f, position.y), randomRotation); // Ajustarea pozitiei Y
            obstacle.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // Ajustarea scalei pentru a face obstacolul mai mic
            obstaclesPlaced++;
        }
    }

    private Vector3 PlacePlayer()
    {
        // Gasirea unei pozitii de start aleatorie departe de punctul final
        Vector2 startCell = new Vector2(Random.Range(0, _mazeWidth), Random.Range(0, _mazeDepth));
        Vector3 startPosition = new Vector3(startCell.x, 0.5f, startCell.y);

        playerInstance = Instantiate(_playerPrefab, startPosition, Quaternion.identity);
        return startPosition;
    }

    private void PlaceEndPoint(Vector3 startPosition)
    {
        List<Vector2> availablePositions = new List<Vector2>();

        // Adaugarea pozitiilor disponibile pentru punctul final
        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                Vector3 position = new Vector3(x, 0, z);
                if (Vector3.Distance(position, startPosition) > (_mazeWidth + _mazeDepth) / 4) // Criteriu de distanta ajustat
                {
                    availablePositions.Add(new Vector2(x, z));
                }
            }
        }

        if (availablePositions.Count == 0)
        {
            Debug.LogError("No available positions found for the end point.");
            // Logica fallback: plasarea punctului final in coltul cel mai indepartat
            availablePositions.Add(new Vector2(_mazeWidth - 1, _mazeDepth - 1));
        }

        int randomIndex = Random.Range(0, availablePositions.Count);
        Vector2 endCell = availablePositions[randomIndex];
        Vector3 endPosition = new Vector3(endCell.x, 0.5f, endCell.y);

        GameObject endPoint = Instantiate(_endPointPrefab, endPosition, Quaternion.identity);
        endPoint.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // Ajustarea scalei pentru a face punctul final mai mic
    }

    private void SetupCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null && playerInstance != null)
        {
            CameraFollow cameraFollow = mainCamera.gameObject.AddComponent<CameraFollow>();
            cameraFollow.player = playerInstance.transform;
            cameraFollow.offset = new Vector3(0, 5, -10); // Ajustarea offset-ului dupa nevoie
            cameraFollow.smoothSpeed = 0.125f; // Ajustarea vitezei de smooth dupa nevoie
        }
    }
}