namespace StringSimilarity.Groups;

public interface IGroups
{
    Task<IEnumerable<string>> GetGroupsNamesAsync();
}
