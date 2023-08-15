namespace Ellie;

public static class Filter
{
    public static bool IsSafe(string val)
    {
        string[] sections = val.Split(',');

        foreach(string section in sections)
        {
            foreach(string str in section.Split(' '))
            {
                foreach(string word in Globals.cfg.blacklist)
                {
                    if(str == word)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }
}