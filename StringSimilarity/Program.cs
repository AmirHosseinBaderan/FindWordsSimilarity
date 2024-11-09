using StringSimilarity;
using StringSimilarity.Groups;
using static System.Console;

//? Dependencies
ISqlConnectionFactory sqlConnectionFactory = new SqlConnectionFactory(Constants.ConnectionString);
IGroups groups = new Groups(sqlConnectionFactory);

IEnumerable<string> words = await groups.GetGroupsNamesAsync();
WriteLine(words.Count());

string? input;
while (true)
{
    Write("Input your text : ");
    input = ReadLine();
    if (input == null)
        continue;
    else if (input == "exit")
        break;

    string? closestWord = FindClosestWord(input, words);
    WriteLine($"Closest word to '{input}' is '{closestWord}'");

    IEnumerable<(string Word, double Similarity)> closestWordsWithSimilarity = FindClosestWordsWithSimilarity(input, words);
    foreach (var item in closestWordsWithSimilarity)
        WriteLine($"Word: {item.Word}, Similarity: {item.Similarity:F2}%");

}

string? FindClosestWord(string input, IEnumerable<string> words)
{
    string? closestWord = null;
    double highestSimilarity = 0;

    foreach (var word in words)
    {
        double similarity = CalculateSimilarity(input, word);
        if (similarity > highestSimilarity)
        {
            highestSimilarity = similarity;
            closestWord = word;
        }
    }

    return closestWord;
}

IEnumerable<(string Word, double Similarity)> FindClosestWordsWithSimilarity(string input, IEnumerable<string> words)
{
    List<(string Word, double Similarity)> result = [];
    foreach (var word in words)
    {
        double similarity = CalculateSimilarity(input, word);
        result.Add((word, similarity));
    }

    double max = result.Max(x => x.Similarity);
    double average = result.Average(x => x.Similarity);
    double min = max - average;
    return result
            .Where(x => x.Similarity <= max && x.Similarity >= min)
            .OrderByDescending(x => x.Similarity);
}


double CalculateSimilarity(string source, string target)
{
    int distance = LevenshteinDistance(source, target);
    int maxLength = Math.Max(source.Length, target.Length);
    return (1.0 - ((double)distance / maxLength)) * 100;
}

int LevenshteinDistance(string source, string target)
{
    if (string.IsNullOrEmpty(source))
        return target?.Length ?? 0;

    if (string.IsNullOrEmpty(target))
        return source.Length;

    int[,] matrix = new int[source.Length + 1, target.Length + 1];

    for (int i = 0; i <= source.Length; i++)
        matrix[i, 0] = i;

    for (int j = 0; j <= target.Length; j++)
        matrix[0, j] = j;

    for (int i = 1; i <= source.Length; i++)
        for (int j = 1; j <= target.Length; j++)
        {
            int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

            matrix[i, j] = Math.Min(Math.Min(
                matrix[i - 1, j] + 1,
                matrix[i, j - 1] + 1),
                matrix[i - 1, j - 1] + cost);
        }

    return matrix[source.Length, target.Length];
}
