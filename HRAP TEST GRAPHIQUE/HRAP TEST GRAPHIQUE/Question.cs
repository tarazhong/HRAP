﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAP_TEST_GRAPHIQUE
{
   
        class Question
        {
            private int questionId;
            private string questionStr;
            private int numAnswers;

            public Question(int id, string str, int answers)
            {
                this.questionId = id;
                this.questionStr = str;
                this.numAnswers = answers;
            }

            public int Id { get { return questionId; } }
            public string String { get { return questionStr; } }
            public int NumAnswers { get { return numAnswers; } }

        }
    }
