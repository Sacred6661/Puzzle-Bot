using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleRobotTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Tree t = new Tree();
            t.Insert("персик");
            t.Insert("черника");
            t.Insert("мандарин");
            t.Insert("груша");
            t.Insert("яблоко");
            t.Insert("клубника");

            Console.WriteLine(t.Display(t));
            Tree s = t.Search("мандарин");
            Console.WriteLine(s.Display(s));
            Console.Read();
            Application.Run(new Form1());
        }
    }
}
