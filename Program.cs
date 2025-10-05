using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        int width = 150, height = 50;
        char cell = System.Text.Encoding.GetEncoding(437).GetChars(new byte[] { 219 })[0]; //the cell character that gets printed to console
        StreamWriter stdout = new StreamWriter(Console.OpenStandardOutput(), System.Text.Encoding.GetEncoding(437)); //using this is faster than using Console.Write()
        Console.CursorVisible = false; //disable blinking cursor

        for (List<bool[,]> grid = new List<bool[,]>() { InitializeRandom(width, height), new bool[width, height] }; //grid[0] contains current grid, grid[1] contains future grid
            !Console.KeyAvailable; //break execution with any key stroke
            grid = new List<bool[,]>() { grid[1], new bool[width, height] }) //make the future grid become the current grid
        {
            Console.CursorTop = Console.CursorLeft = 0; //reset cursor position
            for (int gy = 0; gy < height; gy++, stdout.WriteLine()) //this and the following loop is used to iterate over current grid, both for printing current grid and calculating future grid
                for (int gx = 0; gx < width; gx++)
                {
                    stdout.Write(grid[0][gx, gy] ? cell : ' '); //print current grid
                    int neighbours = grid[0][gx, gy] ? -1 : 0; //initialize neighbours. do not count self.
                    for (int nx = gx - 1; nx < gx + 2; nx++) //this and the following loop is used iterate over neighbours of cell[gx,gy]
                        for (int ny = gy - 1; ny < gy + 2; ny++)
                            if (grid[0][nx < 0 ? nx + width : nx >= width ? nx - width : nx, ny < 0 ? ny + height : ny >= height ? ny - height : ny]) //check if cell[nx,ny] is alive. wrap the edges if cell is at border.
                                //if (a >= 0 && b >= 0 && a < width && b < height && grids[0][a, b]) //use this instead of the line above if you do not want to wrap the edges
                                neighbours++;
                    grid[1][gx, gy] = grid[0][gx, gy] ? neighbours == 2 || neighbours == 3 : neighbours == 3; //fill future grid: a currently living cell survives if it has 2 or 3 neighbours. a currently dead cell gets reborn if it has 3 neighbours.
                }
            stdout.Flush();
            System.Threading.Thread.Sleep(50);
        }
    }

    static bool[,] InitializeRandom(int width, int height)
    {
        Random rand = new Random();
        bool[,] grid = new bool[width, height];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                grid[x, y] = rand.NextDouble() >= 0.5;

        return grid;
    }

    static bool[,] InitializeGliderGun(int width, int height)
    {
        bool[,] grid= new bool[width, height];
        byte[,] gun = new byte[,] { //copied this from: http://www.codehosting.net/blog/BlogEngine/post/Conways-Game-of-Life-as-a-C-console-app.aspx
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,1 },
            { 0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,1 },
            { 1,1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 1,1,0,0,0,0,0,0,0,0,1,0,0,0,1,0,1,1,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }};

        for (int y = 0; y < gun.GetLength(0); y++)
            for (int x = 0; x < gun.GetLength(1); x++)
                grid[x, y] = gun[y, x] == 1;

        return grid;
    }
}
