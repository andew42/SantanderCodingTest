namespace SantanderCodingTest;

public class HackerNewsStories
{
    // Downstream endpoints
    const string BestStoriesEndpoint = "https://hacker-news.firebaseio.com/v0/beststories.json";
    const string StoryEndpoint = "https://hacker-news.firebaseio.com/v0/item/{0}.json";

    IReadOnlyList<int> _bestStories = Array.Empty<int>();
    readonly Dictionary<int, HackerNewsStory> _storyCache = [];

    // Return a list of the best Hacker News stories
    public async Task<IList<HackerNewsStory>> GetBestStoriesAsync(int maxStoryCount)
    {
        // Retrieve list of the best stories if not cached
        if (!_bestStories.Any())
        {
            using var httpClient = new HttpClient();
            _bestStories = await httpClient.GetFromJsonAsync<List<int>>(BestStoriesEndpoint) ?? [];
        }

        // Populate a stories collection
        var bestStories = _bestStories;
        var numberOfStoriesToRetrieve = Math.Min(maxStoryCount, bestStories.Count);
        var stories = new HackerNewsStory[numberOfStoriesToRetrieve];
        // Performance could be substantially improved by concurrent requests in batches
        for (int i = 0; i < numberOfStoriesToRetrieve; i++)
            stories[i] = await GetStoryAsync(bestStories[i]);
        return stories;
    }

    // Return a story given a story ID, use cache if possible
    async Task<HackerNewsStory> GetStoryAsync(int id)
    {
        // Return Cached value if available
        lock (_storyCache)
            if (_storyCache.TryGetValue(id, out var s))
                return s;

        // Otherwise go to hacker news for the story
        using var httpClient = new HttpClient();
        var rs = await httpClient.GetFromJsonAsync<RawStory>(string.Format(StoryEndpoint, id));

        // Depending on requirements we could just ignore this story
        if (rs == null)
            throw new ApplicationException($"Failed to retrieve story ID {id}");

        // Add story to story cache
        var ns = rs.AsHackerNewsStory();
        lock (_storyCache)
            _storyCache[id] = ns;
        return ns;
    }

    // Story returned by Hacker News
    class RawStory
    {
        public string By { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public int Time { get; set; }
        public int Score { get; set; }
        public IList<int> Kids { get; set; } = [];

        public HackerNewsStory AsHackerNewsStory() =>
            new(Title, Url, By, DateTimeOffset.FromUnixTimeSeconds(Time), Score, Kids.Count);
    }
}
