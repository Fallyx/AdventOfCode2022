using System.Text.Json.Nodes;
namespace AdventOfCode2022.Day13;

internal class Day13
{
    const string inputPath = @"Day13/Input.txt";
    public static void Task1and2()
    {
        List<String> lines = File.ReadAllLines(inputPath).ToList();
        int sumIdx = 0;

        for (int i = 0; i < lines.Count; i += 3)
        {
            JsonNode leftNode = JsonNode.Parse(lines[i])!;
            JsonNode rightNode = JsonNode.Parse(lines[i+1])!;
            bool? isRightOrder = ComparePairs(leftNode!, rightNode!);

            if (isRightOrder == true)
                sumIdx += i / 3 + 1;
        }

        Console.WriteLine($"Task 1: {sumIdx}");

        lines.RemoveAll(s => string.IsNullOrEmpty(s));

        List<JsonNode> nodeList = lines.Select(l => JsonNode.Parse(l)!).ToList();
        JsonNode dividerTwo = JsonNode.Parse("[[2]]")!;
        JsonNode dividerSix = JsonNode.Parse("[[6]]")!;
        nodeList.Add(dividerTwo);
        nodeList.Add(dividerSix);

        nodeList.Sort((l, r) => (ComparePairs(l, r) == true ? -1 : 1));
        int idxTwo = nodeList.IndexOf(dividerTwo) + 1;
        int idxSix = nodeList.IndexOf(dividerSix) + 1;

        Console.WriteLine($"Task 2: {idxTwo * idxSix}");
    }

    private static bool? ComparePairs(JsonNode left, JsonNode right)
    {
        if (left is JsonValue && right is JsonValue)
            return CompareValues(left, right);

        // One or both are arrays
        JsonArray leftArr = GetJsonArray(left);
        JsonArray rightArr = GetJsonArray(right);        

        int minLength = Math.Min(leftArr.Count, rightArr.Count);
        for (int i = 0; i < minLength; i++)
        {
            bool? isRightOrder = ComparePairs(leftArr![i]!, rightArr![i]!);
            if (isRightOrder != null) 
                return isRightOrder;
        }

        if (leftArr.Count < rightArr.Count)
            return true;
        else if (leftArr.Count > rightArr.Count)
            return false;

        return null;
    }

    private static bool? CompareValues(JsonNode left, JsonNode right)
    {
        int leftInt = left.GetValue<int>();
        int rightInt = right.GetValue<int>();

        if (leftInt == rightInt) 
            return null;
        return leftInt < rightInt;
    }

    private static JsonArray GetJsonArray(JsonNode node)
    {
        JsonArray arr;
        if (node is JsonArray) 
            arr = node.AsArray();
        else 
            arr = new JsonArray(node.GetValue<int>());
        return arr;
    }
}
