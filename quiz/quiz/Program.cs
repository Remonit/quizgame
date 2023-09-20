using System.Formats.Asn1;
using System.Globalization;
using System;
using CsvHelper;
using System.Numerics;
using CsvHelper.Configuration;
using System.Security.Cryptography.X509Certificates;

namespace quiz
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] chosenAnswers;
            Console.WriteLine("Hey welkom in de quiz app!\nKun je even de path van het .csv bestand hieronder typen?");
            string filePath = Console.ReadLine();

            using (var reader = new StreamReader("D:/school/JAAR3/PROJECT/quizgame/quiz/quiz/questions.csv"))
            {

                List<Question> questionList = new List<Question>();

                while (!reader.EndOfStream)
                {

                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    Question question = new Question(values[0], values[1], values[2], values[3], values[4], values[5]);
                    questionList.Add(question);
                }
                questionList.Remove(questionList[0]);
                foreach (var question in questionList)
                {
                    Console.WriteLine(question.Description);
                    if(question.AnswerA == "" || question.AnswerB == "" || question.AnswerC == "")
                    {
                        Console.WriteLine("dit is een open vraag, vul je antwoord hieronder in:");
                        string chosenAnswer = Console.ReadLine();
                        
                    }
                    else
                    {
                        Console.WriteLine(
                            $"\nA. {question.AnswerA}" +
                            $"\nB. {question.AnswerB}" +
                            $"\nC. {question.AnswerC}\n");
                        string chosenAnswer = Console.ReadLine();
                        if (CheckAnswer(question, chosenAnswer) == true)
                        {
                            Console.WriteLine("\nCORRECT!!\nje bent een fucking topper ofnie dan");
                        }
                        else
                        {
                            Console.WriteLine($"Helaas... het correcte antwoord was {question.CorrectAnswer.ToUpper()}");
                        }
                    }
                    Console.WriteLine("\n\nklik op enter om verder te gaan");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
            
        }
        public static bool CheckAnswer(Question question, string chosenAnswer)
        {
            if(question.CorrectAnswer.ToUpper() ==  chosenAnswer.ToUpper())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}