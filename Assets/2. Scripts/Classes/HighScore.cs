using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class HighScore
{
    public float Time { get; set; }
    public int Kills { get; set; }
    public string Name { get; set; }
}

