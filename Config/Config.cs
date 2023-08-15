namespace Ellie;

public record Config(string token, List<string> blacklist, bool useblacklist, string defaultmodel);