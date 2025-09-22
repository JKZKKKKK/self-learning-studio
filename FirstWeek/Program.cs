/*
// 第一小節C# Console 練習 & 安裝壞竟
System.Console.WriteLine("顆顆");
//tiangle
System.Console.WriteLine(" /|");
System.Console.WriteLine(" / |");
System.Console.WriteLine("/__|");

//第二節C# 資料型態 & 變數
// 2-1 字串 string "華盛頓中學"
System.Console.WriteLine("華盛頓中學");
// 2-2 字元 char 'a'
System.Console.WriteLine("a");
// 2-3 整數 int 123   -123
System.Console.WriteLine(123124123453456435);
// 2-4 浮點數 double 3.14 180.5
System.Console.WriteLine(234234.23423423);
// 2-5 布林值 bool true/false
System.Console.WriteLine(true);

// 2-6 變數宣告
string name = "科科"; // 宣告一個字串變數
char sex = 'M';
int age = 16;
double height = 180.5;
bool isStudent = true;

System.Console.WriteLine("有一個人叫" + name);
name = "小明"; // 變更變數內容
System.Console.WriteLine( name + "今年" + age + "歲");
System.Console.WriteLine( name + "身高" + height + "公分");
System.Console.WriteLine( name +"是不是學生?" + isStudent);

// 第三節C# string 字串用法
System.Console.WriteLine("Hello Mr.Lan");
                       // 0123456789.......
System.Console.WriteLine("Hello" + "Mr.Lan");
string phrase = "Hello Mr.Lan";
System.Console.WriteLine(phrase.Length);
System.Console.WriteLine(phrase.ToUpper());
System.Console.WriteLine(phrase.ToLower());
System.Console.WriteLine(phrase.Contains("Mr"));
System.Console.WriteLine(phrase.Contains("Ms"));
System.Console.WriteLine(phrase[0]);
System.Console.WriteLine(phrase.IndexOf("Lan"));// 找Lan字串,回傳起始位置
System.Console.WriteLine(phrase.Substring(6,3));// 從第6個字元開始,擷取3個字元

// 第四節C# 數字運算(int , double)
System.Console.WriteLine(100-999);
System.Console.WriteLine(999+100);
System.Console.WriteLine(99*88);
System.Console.WriteLine(999/3);// 整數除法
System.Console.WriteLine(10/3.0);// 浮點數除法
int n1 = 100;
System.Console.WriteLine(n1*9999*n1);
System.Console.WriteLine(System.Math.Abs(-199999)); // 絕對值
System.Console.WriteLine(System.Math.Pow(3,9)); // 3的4次方
System.Console.WriteLine(System.Math.Sqrt(999999)); // 開根號
System.Console.WriteLine(System.Math.Round(3.1415926)); // 四捨五入
System.Console.WriteLine(System.Math.Ceiling(3.1415926)); // 無條件進位
System.Console.WriteLine(System.Math.Floor(3.1415926)); // 無條件捨去
System.Console.WriteLine(System.Math.Max(99,88)); // 取最大值
System.Console.WriteLine(System.Math.Min(99,88)); // 取最小值

// 第五節C# 讀取使用者輸入
System.Console.Write("請輸入你的名字:");
string userName = System.Console.ReadLine();
System.Console.WriteLine("Hello " + userName);
System.Console.Write("請輸入你的年齡:");
string ageText = System.Console.ReadLine();
int userAge = int.Parse(ageText);
System.Console.WriteLine("你今年" + userAge + "歲");
System.Console.Write("請輸入你的身高:");
string heightText = System.Console.ReadLine();
*/
//第六節C# 基本計算機
System.Console.Write("請輸入第一個數字:");
string num1Text = System.Console.ReadLine();
double num1 = double.Parse(num1Text);// 法二 int num1 = System.Convert.ToInt32(num1Text);
System.Console.Write("請輸入第二個數字:");
string num2Text = System.Console.ReadLine();
double num2 = double.Parse(num2Text);// 法二 double num1 = System.Convert.ToDouble(num1Text);
System.console.WriteLine({"相加結果" + num1 + num2)
