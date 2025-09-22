using System;

namespace Animal
{
    namespace hii { 
        class Person {
            public double Height;
            public double Weight;
            public int Age;
            public string Name;

            public void SsyHi() {
                Console.WriteLine("hiiiii" + Name);
            }

            public bool IsAdult() {
                if (Age >= 18) {
                    Console.WriteLine("You are an adult.");
                    return true;
                } else {
                    Console.WriteLine("You are not an adult.");
                    return false;
                }
            }

            public int Add(int a, int b) {
                return a + b;
            }
        }
    }


}

namespace project{
    class Person
    {
        public double Height;
        public double Weight;
        public int Age;
        public string Name;

        public void PrintaAge() {
            Console.WriteLine(this.Age);
        }

        public void PrintName()
        {
            Console.WriteLine(this.Name);
        }


        public Person ( string Name , double Height  ,int Weight ,int Age ) {
            Console.WriteLine("Person 創建成功"+ Name);
            this.Name = Name;
            this.Height = Height;
            this.Weight = Weight;
            this.Age = Age;
        }
    }



}
