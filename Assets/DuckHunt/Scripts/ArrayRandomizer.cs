using System.Collections;
using System.Collections.Generic;
using System;


public class ArrayRandomizer
{
    public static void Randomize<T>( T[] items )
    {
        Random rand = new Random();

        // For each spot in the array, pick
        // a random item to swap into that spot.
        for (int i = 0; i < items.Length - 1; i++)
        {
            int j = rand.Next ( i, items.Length );
            T temp = items[i];
            items[i] = items[j];
            items[j] = temp;
        }
    }

    public static T[] RandomizeCopy<T>( T[] items )
    {
        Random rand = new Random();
        T[] copy = new T[items.Length];
        items.CopyTo( copy, 0 );

        Randomize( copy );

        return copy;
    }

}
