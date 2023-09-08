namespace Ellie;

public record Config(string token, List<string> denylist, bool usedenylist, string defaultmodel, List<ulong> adminids);