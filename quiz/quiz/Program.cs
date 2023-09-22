using System.Formats.Asn1;
using System.Globalization;
using System;
using CsvHelper;
using System.Numerics;
using CsvHelper.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace quiz
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            List<string> chosenAnswers = new List<string>();
            int score = 0;

            Console.WriteLine("Hey welkom in de quiz app!\nKun je even de path van de map met het .csv bestand hieronder typen?");
            string filePath = Console.ReadLine();
            filePath = "D:/school/JAAR3/PROJECT/quizgame/quiz/quiz";

            using (var reader = new StreamReader(filePath + "/questions.csv"))
            {
                Console.Clear();
                List<Question> questionList = new List<Question>();

                while (!reader.EndOfStream)
                {

                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    Question question = new Question(values[0], values[1], values[2], values[3], values[4], values[5]);
                    questionList.Add(question);
                }
                questionList.Remove(questionList[0]);
                int i = 1;
                string userType = "";
                while (userType == "")
                {
                    Console.Clear();
                    Console.WriteLine("Wat ben je?\n1. student\n2. docent");
                    userType = Console.ReadLine();
                }
                if (userType == "1")
                {
                    foreach (var question in questionList)
                    {
                        Console.WriteLine($"{i}/{questionList.Count} {question.Description}");
                        if (question.AnswerA == "" || question.AnswerB == "" || question.AnswerC == "")
                        {
                            Console.WriteLine("dit is een open vraag, vul je antwoord hieronder in:");
                            string chosenAnswer = Console.ReadLine();
                            if (chosenAnswer == "") chosenAnswer = "X";
                            chosenAnswers.Add(chosenAnswer);
                            if (CheckAnswer(question, chosenAnswer) == true)
                            {
                                Console.WriteLine("\nCORRECT!!\nje bent een fucking topper ofnie dan");
                                score++;
                            }
                            else
                            {
                                Console.WriteLine($"Helaas... het correcte antwoord was {question.CorrectAnswer.ToUpper()}");
                            }
                        }
                        else
                        {
                            Console.WriteLine(
                                $"\nA. {question.AnswerA}" +
                                $"\nB. {question.AnswerB}" +
                                $"\nC. {question.AnswerC}\n");
                            string chosenAnswer = Console.ReadLine();
                            if (chosenAnswer == "") chosenAnswer = "X";
                            chosenAnswers.Add(chosenAnswer);
                            if (CheckAnswer(question, chosenAnswer) == true)
                            {
                                Console.WriteLine("\nCORRECT!!\nje bent een fucking topper ofnie dan");
                                score++;
                            }
                            else
                            {
                                Console.WriteLine($"Helaas... het correcte antwoord was {question.CorrectAnswer.ToUpper()}");
                            }
                        }
                        Console.WriteLine("\n\nklik op enter om verder te gaan");
                        Console.ReadLine();
                        Console.Clear();
                        i++;
                    }
                    Console.WriteLine($"Je bent klaar met de toets, de hebt een score van {score} van de {questionList.Count} gehaald");
                    Console.WriteLine("Wil je je score opslaan? ja/nee");
                    string answer = Console.ReadLine();
                    if (answer.ToUpper() == "JA")
                    {
                        Console.Clear();
                        Console.WriteLine("Onder welke naam wil je opslaan?");
                        string name = Console.ReadLine();
                        string fileName = filePath + "/" + name + ".txt";

                        if (File.Exists(fileName))
                        {
                            File.Delete(fileName);
                        }

                        using (FileStream fs = File.Create(fileName))
                        {
                            Byte[] title = new UTF8Encoding(true).GetBytes($"{name} heeft een score van {score} van de {questionList.Count}\n");
                            fs.Write(title, 0, title.Length);
                            for (int j = 0; j < questionList.Count; j++)
                            {
                                Byte[] answers = new UTF8Encoding(true).GetBytes($"gekozen antwoord:  {chosenAnswers[j]}\n" +
                                    $"correcte antwoord: {questionList[j].CorrectAnswer}\n\n");
                                fs.Write(answers, 0, answers.Length);
                            }

                        }
                    }
                }
                else if (userType == "2")
                {
                    Console.Clear();
                    Console.WriteLine("Hier is een lijst van alle vragen:\n");
                    foreach(var question in questionList )
                    {
                        Console.WriteLine($"{i}/{questionList.Count} - {question.Description}");
                        i++;
                    }
                    
                }
                
            }
            string action = "";
            StringBuilder output = new StringBuilder();
            while(action == "" )
            {
                Console.WriteLine("\nMaak een keuze:\n1. Voeg een vraag toe\n2. Verwijder een vraag");
                action = Console.ReadLine();
            }
            if (action == "1")
            {
                Console.Clear();
                Console.WriteLine("Vul hieronder de vraag die je wilt toevoegen in:");
                string questionToBeAdded = Console.ReadLine();
                Console.WriteLine("Vul het antwoord voor keuze A in:");
                string questionToBeAddedAnswerA = Console.ReadLine();
                Console.WriteLine("Vul het antwoord voor keuze B in:");
                string questionToBeAddedAnswerB = Console.ReadLine();
                Console.WriteLine("Vul het antwoord voor keuze C in:");
                string questionToBeAddedAnswerC = Console.ReadLine();
                Console.WriteLine("Wat is het correcte antwoord? (vul in 'A', 'B' of 'C'");
                string questionToBeAddedCorrectAnswer = Console.ReadLine();

                string newLine = string.Format("{0};{1};{2};{3};{4};{5}", "d89", questionToBeAdded, questionToBeAddedAnswerA, questionToBeAddedAnswerB, questionToBeAddedAnswerC, questionToBeAddedCorrectAnswer);
                output.AppendLine(string.Join(";", newLine));

                try
                {
                    File.AppendAllText(filePath + "/questions.csv", output.ToString());
                    Console.WriteLine("SUCCES");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("Data could not be written to the CSV file.");
                    return;
                }
            }
            else if (action == "2")
            {
                Console.WriteLine("Welke vraag wil je verwijderen? (vul het nummer van de vraag in)");
                int questionToBeDeleted = int.Parse(Console.ReadLine()) - 1;

                List<string> linesList = File.ReadAllLines(filePath + "/questions.csv").ToList();
                linesList.RemoveAt(questionToBeDeleted);
                File.WriteAllLines(filePath + "/questions.csv", linesList.ToArray());
                Console.WriteLine("Vraag succesvol verwijderd");
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