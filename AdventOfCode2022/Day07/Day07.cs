namespace AdventOfCode2022.Day07;

internal class Day07
{
    const string inputPath = @"Day07/Input.txt";
    private static List<int> dirSize = new List<int>();

    public static void Task1and2()
    {
        List<String> lines = File.ReadAllLines(inputPath).Skip(1).ToList();
        FileSystem fileSystem = new FileSystem("/", 0, true);
        FileSystem currentParent = fileSystem;

        foreach (string line in lines)
        {
            if (line.StartsWith("$ cd"))
            {
                string dir = line.Split(' ')[2];
                if (dir == "..")
                    currentParent = currentParent!.Parent!;
                else
                    currentParent = currentParent!.Childs!.Find(f => f.Name == dir)!;
            }
            else if (line.StartsWith("$ ls")) continue;
            else if (line.StartsWith("dir"))
            {
                String[] dirInfo = line.Split(' ');
                FileSystem tmpDir = new FileSystem(dirInfo[1], 0, true, currentParent);
                currentParent.Childs.Add(tmpDir);
            }
            else
            {
                String[] fileInfo = line.Split(' ');
                FileSystem tmpFile = new FileSystem(fileInfo[1], int.Parse(fileInfo[0]), false, currentParent);
                currentParent.Childs.Add(tmpFile);
            }
        }

        CalculateSize(fileSystem);
        Console.WriteLine($"Task 1: {CalculateTotSize(fileSystem)}");

        int deleteMinSize = 30000000 - (70000000 - fileSystem.Size);
        DeleteDir(fileSystem, deleteMinSize);
        Console.WriteLine($"Task 2: {dirSize.Min()}");
    }

    private static int CalculateSize(FileSystem currentDir)
    {
        if (currentDir.Size != 0) 
            return currentDir.Size;

        int size = 0;

        if (currentDir.Childs.Count == 0) 
            size = currentDir.Size;
        else 
            size += currentDir.Childs.Sum(c => CalculateSize(c));

        currentDir.Size = size;

        return size;
    }

    private static int CalculateTotSize(FileSystem currentDir)
    {
        int size = 0;
        if (currentDir.IsDir && currentDir.Size <= 100000) 
            size = currentDir.Size;
        if (currentDir.Childs.Count > 0) 
            size += currentDir.Childs.Sum(c => CalculateTotSize(c)); 

        return size;
    }

    private static void DeleteDir(FileSystem currentDir, int deleteMinSize)
    {
        if (!currentDir.IsDir || currentDir.Size < deleteMinSize) 
            return;

        dirSize.Add(currentDir.Size);

        foreach (FileSystem child in currentDir.Childs) 
            DeleteDir(child, deleteMinSize); 
    }

    private class FileSystem
    {
        public String Name { get; set; }
        public Int32 Size { get; set; }
        public FileSystem? Parent { get; set; }
        public List<FileSystem> Childs { get; set; }
        public bool IsDir { get; set; } 

        public FileSystem(string name, int size, bool isDir, FileSystem? parent = null) 
        {
            Name = name;
            Size = size;
            Childs = new List<FileSystem>();
            IsDir = isDir;
            Parent = parent;
        }
    }
}
