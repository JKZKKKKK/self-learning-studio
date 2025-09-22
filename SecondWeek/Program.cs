//第二週
//第一節 C# 二維陣列



int [ , ] nums =
{
    { 1 , 2 , 3 },
    { 4 , 5 , 6 },
    { 7 , 8 , 9 }
};

System.Console.WriteLine(nums[0 , 0]);

int[,] num = new int[4, 8];
num[0, 0] = 123;

int[ ,, ] num3 = new int[3, 4, 5];

//第二節 C# 類別與物件 class object
Person person1 = new Person(); // person1 是 Person 類別的物件
person1.Age = 20;
person1.Height = 180;
person1.Weight = 70;
person1.Name = "Zhongze";

System.Console.WriteLine(person1.Name);



//第二節 C# using 與命名空間 namespace
using Animal.hii;
using System;

Person person2 = new Person();
person2.Age = 30;
person2.Height = 170;
Console.WriteLine(person2.Name);
person2.Name = "Lili";
Console.WriteLine(person2.Name);

Person person3 = new Person();
person3.Name = "John";

person3.Age = 40;
Console.WriteLine(person3.Age); 
Console.WriteLine(person3.Name);



//第三節 C# 方法 method

using Animal.hii;
using System;

Person person1 = new Person();
person1.Name = "Zhongze";
person1.SsyHi();
person1.Age = 15;
Console.WriteLine(person1.IsAdult());
person1.Add(3, 70);

Person person2 = new Person();
person2.Name = "Lili";
person2.Age = 20;
Console.WriteLine(person2.IsAdult());


//第四節 C# main 方法

using Animal.hii;
using System;


class Program
{
    static void Main()
    {
        Person person1 = new Person();
        person1.Name = "Zhongze";
        person1.SsyHi();
        person1.Age = 15;
        Console.WriteLine(person1.IsAdult());
        person1.Add(3, 70);

        Person person2 = new Person();
        person2.Name = "Lili";
        person2.Age = 20;
        Console.WriteLine(person2.IsAdult());
    }
}


//第五節 C# constructor 建構子
using project;
using System;
class Program
{
    static void Main()
    {
        Person person1 = new Person("Zhongze",180,70,23);
        Person person2 = new Person("xcen",120,25,60);
    }

}



//第六節 C# getter setter 屬性
using Video;
using System;

video video1 = new video("C# 教學","Zhongze","程式設計");
Console.WriteLine(video1.title);
Console.WriteLine(video1.author);
Console.WriteLine(video1.Type);

video video2 = new video("Java 教學","Lili","哈哈哈哈");
Console.WriteLine(video2.title);
Console.WriteLine(video2.author);
Console.WriteLine(video2.Type);


//第七節 C# static attribute 靜態屬性
using System;
using Video;

video video1 = new video("C# 教學", "Zhongze", "程式設計");
Console.WriteLine(video1.GetVideoCount());
video video2 = new video("Java 教學", "Lili", "哈哈哈哈");
Console.WriteLine(video2.GetVideoCount());


//第八節 C# static method 靜態方法
using System;
using secondweek;

//Math math = new Math();
//tool tool = new tool(); // 靜態類別無法建立物件
tool.SayHello();



//第九節 C# inheritance 繼承

using System;
using secondweek;
using project;
using student;

Student student1 = new Student("Zhongze", 180, 70, 23, "台灣大學");
student1.PrintName();
student1.PrintaAge();
student1.School = "台灣大學";
Console.WriteLine(student1.School);
Student student2 = new Student("Lili", 160, 50, 20, "政治大學");
student2.PrintName();
student2.PrintaAge();
student2.School = "政治大學";
Console.WriteLine(student2.School);