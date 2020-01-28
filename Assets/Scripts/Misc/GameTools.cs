using System;
using System.Collections.Generic;
using System.Linq;

public class OnsetInfo
{
    public float time;
    public float amplitude;
}

public class GameTools
{   
    public static List<OnsetInfo> GetOnsets(string data)
    {
        var lines = data.Split('\n');
        return lines.Where(s => s.Contains(',')).Select(s =>
        {
            var vals = s.Split(',');
            return new OnsetInfo()
            {
                time = float.Parse(vals[0]),
                amplitude = float.Parse(vals[1])
            };
        }).ToList();
    }
}
