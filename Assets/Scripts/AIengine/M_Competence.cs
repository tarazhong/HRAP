﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRAP
{
    public class M_Competences
    {
        private string name;
        private bool isTech;
        private int points;
        private bool isImportant;

        public M_Competences(string name, bool isTech, int points, bool isImportant)
        {
            this.name = name;
            this.isTech = isTech;
            this.points = points;
            this.isImportant = isImportant;
        }

        public string Name { get { return name; } }
        public bool IsTech { get { return isTech; } }
        public int Points { get { return points; } set { points = value; } }
        public bool IsImportant { get { return isImportant; } set { isImportant = value; } }

    }
}