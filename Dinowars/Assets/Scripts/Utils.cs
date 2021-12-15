using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Color GenerateRandomColor()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 0.2f);

        return new Color(r, g, b);
    }

    public static string GenerateRandomName(string prefix)
    {
        return  string.Format( "{0} - {1}" ,prefix, Random.Range(100, 999));
    }


}
