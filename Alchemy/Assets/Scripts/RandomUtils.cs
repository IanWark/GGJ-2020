using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomUtils 
{
    // https://forum.unity.com/threads/random-number-chosen-except-one.139317/
    public static int RandomIntExceptOne(int min, int max, int except)
    {
        int result = Random.Range(min, max - 1);
        if (result >= except) result += 1;
        return result;
    }
}
