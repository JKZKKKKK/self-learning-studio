using project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace student
{
    class Student : Person
    {
        public string School;

        public Student(string Name, double Height, int Weight, int Age, string School)
            : base(Name, Height, Weight, Age) // 呼叫父類別的建構子
        {
            Console.WriteLine("Person 創建成功 " + Name);
            this.School = School;
        }
    }

}
