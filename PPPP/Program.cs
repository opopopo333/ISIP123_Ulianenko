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

class Program
{
    static char[] vowels = { 'а','е','ё','и','о','у','ы','э','ю','я',
                             'a','e','i','o','u','y' };
    static char[] consonants = { 'б','в','г','д','ж','з','й','к','л','м','н','п','р','с','т','ф','х','ц','ч','ш','щ',
                                 'q','w','r','t','p','s','d','f','g','h','j','k','l','z','x','c','v','b','n','m' };

    static void Main()
    {
        List<TextStatistics> history = new List<TextStatistics>();

        while (true)
        {
            Console.WriteLine("===== МЕНЮ =====");
            Console.WriteLine("1. Ввести новый текст");
            Console.WriteLine("2. Показать всю статистику по прошлым текстам");
            Console.WriteLine("3. Выход");
            Console.Write("Выберите пункт: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.WriteLine("\nВведите текст (минимум 100 символов):");
                string input = Console.ReadLine();

                if (input.Length < 100)
                {
                    Console.WriteLine("Текст слишком короткий!\n");
                    continue;
                }

                TextStatistics stats = AnalyzeText(input);
                history.Add(stats);

                Console.WriteLine("\nРезультат анализа:");
                stats.Print();
            }
            else if (choice == "2")
            {
                if (history.Count == 0)
                {
                    Console.WriteLine("История пуста.\n");
                }
                else
                {
                    Console.WriteLine("\n=== История анализов ===");
                    for (int i = 0; i < history.Count; i++)
                    {
                        Console.WriteLine($"Текст #{i + 1}:");
                        history[i].Print();
                    }
                }
            }
            else if (choice == "3")
            {
                Console.WriteLine("Выход из программы...");
                break;
            }
            else
            {
                Console.WriteLine("Неверный ввод. Попробуйте снова.\n");
            }
        }
    }
    static TextStatistics AnalyzeText(string text)
    {
        TextStatistics stats = new TextStatistics();
        stats.OriginalText = text;
        stats.LetterFrequency = new Dictionary<char, int>();

        string[] words = text.Split(new char[] { ' ', '\t', '\n', ',', '.', '!', '?', ';', ':' },
                                    StringSplitOptions.RemoveEmptyEntries);

        stats.WordCount = words.Length;
        stats.ShortestWord = words[0];
        stats.LongestWord = words[0];

        for (int i = 0; i < words.Length; i++)
        {
            string word = words[i];
            if (word.Length < stats.ShortestWord.Length)
                stats.ShortestWord = word;
            if (word.Length > stats.LongestWord.Length)
                stats.LongestWord = word;
        }

        stats.SentenceCount = 0;
        for (int i = 0; i < text.Length; i++)
        {
            char ch = text[i];
            if (ch == '.' || ch == '!' || ch == '?')
                stats.SentenceCount++;
        }

        stats.VowelCount = 0;
        stats.ConsonantCount = 0;
        for (int i = 0; i < text.Length; i++)
        {
            char ch = char.ToLower(text[i]);

            if (Array.IndexOf(vowels, ch) >= 0)
                stats.VowelCount++;
            else if (Array.IndexOf(consonants, ch) >= 0)
                stats.ConsonantCount++;

            if (char.IsLetter(ch))
            {
                if (!stats.LetterFrequency.ContainsKey(ch))
                    stats.LetterFrequency[ch] = 0;
                stats.LetterFrequency[ch]++;
            }
        }

        return stats;
    }

}