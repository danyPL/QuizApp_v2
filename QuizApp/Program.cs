using System;
using System.Collections.Generic;
using System.IO;

namespace QuizApp
{
    class Question
    {
        public int id { get; set; }
        public string question { get; set; }
        public int trueQ { get; set; }
        public List<string> questions { get; set; }

        public Question(int id, string question, int trueQ, List<string> questions)
        {
            this.id = id;
            this.question = question;
            this.trueQ = trueQ;
            this.questions = questions;
        }

        public void ShowQuestion()
        {
            Console.WriteLine(question + ":");
            for (int i = 0; i < questions.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {questions[i]}");
            }
        }
    }

    class Program
    {
        public static int maxQ = 10;
        public static int highestScore = 0;
        public static bool repeat = true;
        public static List<int> scores = new List<int>();

        static void StartQuiz()
        {
            Console.Clear();
            string path = "Questions.txt";
            int selectedQuestion;
            List<Question> questions = new List<Question>();
            if (!File.Exists(path))
            {
                Console.WriteLine("Nie znaleziono pliku");
                return;
            }
            StreamReader reader = new StreamReader(path);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] words = line.Split(';');
                if (words.Length >= 3 && int.TryParse(words[0], out int id) && int.TryParse(words[2], out int trueQ))
                {
                    List<string> qList = new List<string> { words[3], words[4], words[5], words[6] };
                    Question newQuestion = new Question(id, words[1], trueQ, qList);
                    questions.Add(newQuestion);
                }
                else
                {
                    Console.WriteLine($"Nieprawidłowy format pytania w wierszu: {line}");
                }
            }
            reader.Close();

            int points = 0;
            for (int i = 0; i < maxQ; i++)
            {
                Random randomQ = new Random();
                int q = randomQ.Next(questions.Count);

                Question question = questions[q];
                int odp = question.trueQ;

                Console.Clear();
                question.ShowQuestion();
                Console.WriteLine("");
                Console.WriteLine("Twoja odpowiedź:");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int test))
                {
                    int choice2 = test;
                    if (choice2 == odp)
                    {
                        Console.WriteLine("Dobra odpowiedź!");
                        points++;
                    }else if(choice2 > 4 && i > 1)
                    {
                        Console.WriteLine("Naprawdę chciało panu sie to sprawdzać? Za to nie dostaje pan punktu :D");
                    }
                    else
                    {
                        Console.WriteLine("Zła odpowiedź!");
                    }
                }
                Console.ReadKey();
            }
            scores.Add(points);
            if (highestScore < points)
            {
                highestScore = points;
            }

            Console.Clear();
            Console.WriteLine($"Twój wynik to: {points}/{maxQ}");
            Console.Write("Podaj imię: ");
            string playerName = Console.ReadLine();
            File.AppendAllText("rekord.txt", $"{playerName}: {points}/{maxQ}\n");
            Console.WriteLine("Wynik zapisany.");
            Console.ReadKey();
        }

        static void ShowScores()
        {
            Console.Clear();
            Console.WriteLine("Lista wyników:");

            if (File.Exists("rekord.txt"))
            {
                string[] lines = File.ReadAllLines("rekord.txt");
                foreach (string line in lines)
                {
                    Console.WriteLine(line);
                }
            }
            else
            {
                Console.WriteLine("Brak wyników.");
            }

            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            while (repeat)
            {
                Console.Clear();
                Console.WriteLine($"Witaj w Quizzie Informatycznym! Obecny najwyższy wynik to {highestScore}");
                Console.WriteLine("Wybierz daną opcję i wczyśnij Enter.");
                Console.WriteLine("1. Rozpocznij Quiz!\n2. Pokaż listę wyników\n3. Wyjście: ");
                int choice = Convert.ToInt16(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        StartQuiz();
                        break;
                    case 2:
                        ShowScores();
                        break;
                    case 3:
                        repeat = false;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}