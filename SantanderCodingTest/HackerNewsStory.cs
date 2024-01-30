namespace SantanderCodingTest;

// Story returned by our API
public record HackerNewsStory(string Title, string Uri, string PostedBy, DateTimeOffset Time, int Score, int CommendCount);
