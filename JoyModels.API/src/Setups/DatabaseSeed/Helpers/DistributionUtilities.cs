namespace JoyModels.API.Setups.DatabaseSeed.Helpers;

public static class DistributionUtilities
{
    public static List<int> DistributeModelsAmongCreators(int creatorCount, int totalModels, int minPerCreator,
        int maxPerCreator)
    {
        var assignments = new List<int>();

        for (var i = 0; i < creatorCount; i++)
            assignments.Add(minPerCreator);

        var remaining = totalModels - (creatorCount * minPerCreator);

        while (remaining > 0)
        {
            var creatorIndex = SeedDataConstants.Random.Next(creatorCount);

            if (assignments[creatorIndex] < maxPerCreator)
            {
                assignments[creatorIndex]++;
                remaining--;
            }
        }

        return assignments;
    }
}