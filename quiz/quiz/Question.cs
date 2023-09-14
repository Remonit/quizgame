using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz
{
    internal class Question
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string AnswerA {  get; set; }
        public string AnswerB { get; set; }
        public string AnswerC { get; set; }
        public string CorrectAnswer {  get; set; }

        public Question(string id, string description, string answerA, string answerB, string answerC, string correctAnswer)
        {
            Id = id;
            Description = description;
            AnswerA = answerA;
            AnswerB = answerB;
            AnswerC = answerC;
            CorrectAnswer = correctAnswer;
        }
    }
}
