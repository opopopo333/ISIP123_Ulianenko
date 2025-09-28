using System;
using System.Collections.Generic;

class TextStatistics
{
    public string OriginalText;
    public int WordCount;
    public string ShortestWord;
    public string LongestWord;
    public int SentenceCount;
    public int VowelCount;
    public int ConsonantCount;
    public Dictionary<char, int> LetterFrequency;

    public void Print()
    {
        Console.WriteLine("---- Статистика ----");
        Console.WriteLine($"Количество слов: {WordCount}");
        Console.WriteLine($"Самое короткое слово: {ShortestWord}");
        Console.WriteLine($"Самое длинное слово: {LongestWord}");
        Console.WriteLine($"Количество предложений: {SentenceCount}");
        Console.WriteLine($"Гласных: {VowelCount}");
        Console.WriteLine($"Согласных: {ConsonantCount}");
        Console.WriteLine("Частота букв:");
        foreach (var kvp in LetterFrequency)
        {
            Console.WriteLine($"{kvp.Key} : {kvp.Value}");
        }
        Console.WriteLine("--------------------\n");
    }
}
