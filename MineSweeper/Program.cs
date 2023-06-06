using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    internal class Program
    {
        // hàm Main để nhận đầu vào và kiểm tra kết thúc trò chơi
        static void Main()
        {
            int rows = 10;
            int columns = 10;
            int numMines = 10;
            bool[,] mines = GenerateMines(rows, columns, numMines);
            char[,] grid = new char[rows, columns];
            bool[,] visited = new bool[rows, columns];

            InitializeGrid(grid);

            bool gameOver = false;

            while (!gameOver)
            {
                // xóa di array cũ và tạo mới một trò chơi mới
                Console.Clear();
                DrawGrid(grid);

                Console.Write("Nhap toa do ban muon chon (VD: 1 2): ");
                string[] input = Console.ReadLine().Split(' ');
                int row = int.Parse(input[0]);
                int col = int.Parse(input[1]);

                if (mines[row, col])
                {
                    Console.WriteLine("Game Over! You hit a mine.");
                    gameOver = true;
                }
                else
                {
                    //cap nhat lai tro choi
                    UpdateGrid(mines, grid, visited, row, col);

                    if (IsGameWon(grid, numMines))
                    {
                        Console.WriteLine("Congratulations! You won the game.");
                        gameOver = true;
                    }
                }
            }

            Console.ReadLine();
        }
        //tao vi tri boom trong array tao mot so ngau nhien de xac dinh vi tri boom
        static bool[,] GenerateMines(int rows, int columns, int numMines)
        {
            bool[,] mines = new bool[rows, columns];
            Random random = new Random();
            //ramdom boom va hien thi so ngay ben gn qua boom
            while (numMines > 0)
            {
                int row = random.Next(0, rows);
                int col = random.Next(0, columns);

                if (!mines[row, col])
                {
                    mines[row, col] = true;
                    numMines--;
                }
            }

            return mines;
        }
        // ve ra ki hieu mang an di boom va cac so
        static void InitializeGrid(char[,] grid)
        {
            int rows = grid.GetLength(0);
            int columns = grid.GetLength(1);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    grid[row, col] = '-';
                }
            }
        }
        // ve ra le va tao so de xac dinh vi tri de cho nguoi choi xac dinh duoc toa do cua tung nut can an
        static void DrawGrid(char[,] grid)
        {
            int rows = grid.GetLength(0);
            int columns = grid.GetLength(1);

            Console.WriteLine("  0 1 2 3 4 5 6 7 8 9");
            Console.WriteLine("---------------------");

            for (int row = 0; row < rows; row++)
            {
                //hien thi tro choi dau ngan cach so voi le trai
                Console.Write(row + "|");

                for (int col = 0; col < columns; col++)
                {
                    if (grid[row, col] == '0')
                    {
                        Console.Write("  "); // Hiển thị khoảng trắng cho ô trống
                    }
                    else
                    {
                        Console.Write(grid[row, col] + " ");
                    }
                }

                Console.WriteLine();
            }
            //dong ket thuc gioi han game
            Console.WriteLine("---------------------");
        }
        // Dem so luong boom ben canh cho mot o trong array
        static int CountAdjacentMines(bool[,] mines, int row, int col)
        {
            int count = 0;
            int rows = mines.GetLength(0);
            int columns = mines.GetLength(1);

            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (i >= 0 && i < rows && j >= 0 && j < columns)
                    {
                        if (mines[i, j])
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }

        static void UncoverEmptyCells(bool[,] mines, char[,] grid, bool[,] visited, int row, int col)
        {
            if (visited[row, col])
            {
                return;
            }

            visited[row, col] = true;
            grid[row, col] = ' ';

            int rows = mines.GetLength(0);
            int columns = mines.GetLength(1);

            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (i >= 0 && i < rows && j >= 0 && j < columns && !mines[i, j] && grid[i, j] == '-')
                    {
                        int adjacentMines = CountAdjacentMines(mines, i, j);
                        grid[i, j] = adjacentMines.ToString()[0];

                        if (adjacentMines == 0)
                        {
                            UncoverEmptyCells(mines, grid, visited, i, j);
                        }
                    }
                }
            }
        }

        static bool IsGameWon(char[,] grid, int numMines)
        {
            int rows = grid.GetLength(0);
            int columns = grid.GetLength(1);
            int uncoveredCount = 0;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    if (grid[row, col] != '-' && grid[row, col] != 'F')
                    {
                        uncoveredCount++;
                    }
                }
            }

            int totalCells = rows * columns;
            int safeCells = totalCells - numMines;

            return uncoveredCount == safeCells;
        }
        //hàm cập nhật hiển thị trò 
        static void UpdateGrid(bool[,] mines, char[,] grid, bool[,] visited, int row, int col)
        {
            if (visited[row, col])
            {
                return;
            }

            visited[row, col] = true;

            if (mines[row, col])
            {
                grid[row, col] = 'X'; // Hiển thị mìn đã đánh trúng
                return;
            }

            int adjacentMines = CountAdjacentMines(mines, row, col);
            grid[row, col] = adjacentMines.ToString()[0];

            if (adjacentMines == 0)
            {
                for (int i = row - 1; i <= row + 1; i++)
                {
                    for (int j = col - 1; j <= col + 1; j++)
                    {
                        if (i >= 0 && i < grid.GetLength(0) && j >= 0 && j < grid.GetLength(1))
                        {
                            if (!mines[i, j] && !visited[i, j])
                            {
                                UpdateGrid(mines, grid, visited, i, j);
                            }
                        }
                    }
                }
            }
        }
    }
}
