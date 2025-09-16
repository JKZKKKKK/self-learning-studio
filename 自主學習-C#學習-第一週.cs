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

//第六節C# 基本計算機
System.Console.Write("請輸入第一個數字:");
string num1Text = System.Console.ReadLine();
double num1 = double.Parse(num1Text);// 法二 int num1 = System.Convert.ToInt32(num1Text);
System.Console.Write("請輸入第二個數字:");
string num2Text = System.Console.ReadLine();
double num2 = double.Parse(num2Text);// 法二 double num1 = System.Convert.ToDouble(num1Text);
System.Console.WriteLine(num1 + num2);

// 第七節C# ARRARY 陣列
int[] scores = { 50, 60, 70, 80, 90 };
string[] names = new string[3]; // 宣告一個可放3個字串的陣列
names[0] = "科科";
names[1] = "王大明";
names[2] = "來華";

System.Console.WriteLine("陣列元素個數:" + scores.Length);
System.Console.WriteLine("陣列第一個元素:" + scores[0]);
System.Console.WriteLine("陣列第三個元素:" + scores[2]);
scores[4] = 100; // 修改陣列元素
System.Console.WriteLine("陣列第五個元素:" + scores[4]);
System.Console.WriteLine("陣列第二個人的名字:" + names[1]);

//第七節C# if 判斷式

//如果我是學生
//    我會就讀華中

bool isStudent = true;
if (isStudent) { 
    System.Console.WriteLine("我會就讀華中");
}

//如果今天校車友準時
//    我會準時到學校
//否則
//    我會遲到

bool isBusOnTime = false;
if (isBusOnTime) 
{ 
    System.Console.WriteLine("我會準時到學校");
}
else {
    System.Console.WriteLine("我會遲到");
}

//如果你考100分
//    你會得到一台Iphone 15
//否則如果你考80分以上
//    你會得到一台ipad
//否則
//    你會得到一台筆記型電腦

int score = 75;
if (score == 100) 
{ 
    System.Console.WriteLine("你會得到一台Iphone 15");
}
else if (score >= 80) 
{
    System.Console.WriteLine("你會得到一台ipad");
}
else 
{
    System.Console.WriteLine("你會得到一台筆記型電腦");
}

//如果你考100分 且 你是學生
//    你會得到一台Iphone 15
//反則
//    你會得到一台筆記型電腦

int score2 = 100;
bool isStudent2 = true;
if (score2 == 100 && isStudent2) 
{
    System.Console.WriteLine("你會得到一台Iphone 15");
}
else 
{
    System.Console.WriteLine("你會得到一台筆記型電腦");
}

//如果你考100分 或 你是學生
//    你會得到一台Iphone 15
//反則
//    你會得到一台筆記型電腦
int score3 = 90;
bool isStudent3 = false;
if (score3 == 100 || !isStudent3)
{
    System.Console.WriteLine("你會得到一台Iphone 15");
}
else
{
    System.Console.WriteLine("你會得到一台筆記型電腦");
}


//第八節 C# 進階計算機
System.Console.Write("請輸入第一個數字:");
double num1 = System.Convert.ToDouble(System.Console.ReadLine());

System.Console.Write("請輸入要做的運算");
string oper = System.Console.ReadLine();

System.Console.Write("請輸入第二個數字:");
double num2 = System.Convert.ToDouble(System.Console.ReadLine());

if (oper == "+") {
    System.Console.WriteLine(num1 + num2);
} else if (oper == "-") {
    System.Console.WriteLine(num1 - num2);
} else if (oper == "*") {
    System.Console.WriteLine(num1 * num2);
} else if (oper == "/") {
    if (num2 != 0) {
        System.Console.WriteLine(num1 / num2);
    } else {
        System.Console.WriteLine("除數不可為0");
    }
} else {
    System.Console.WriteLine("不支援此運算");
}


// 第九節 C# while 迴圈

int count = 1;
while (count <= 5) { 
    System.Console.WriteLine("顆顆" + count);
    count = count + 1; // count += 1; count++;
}

System.Console.WriteLine("迴圈結束後count=" + count);

do { 
    System.Console.WriteLine("顆顆" + count);
    count++;
} while (count <= 5);

System.Console.WriteLine("迴圈結束後count=" + count);



// 第十節 C# 猜數字

int answer = new System.Random().Next(1, 101); // 1~100隨機數
int guess ; // 使用者猜的數字
int counta = 3; // 猜的次數

do { 
    System.Console.Write("請輸入1~100的數字:");
    guess = System.Convert.ToInt32(System.Console.ReadLine());
    if (guess < answer && counta > 0)
    {
        System.Console.WriteLine("太小囉 剩餘次數" + counta );
        counta-=1;
    }
    else if (guess > answer && counta > 0)
    {
        System.Console.WriteLine("太大囉 剩餘次數" + counta);
        counta-=1;
    }
    else if (answer == guess && counta > 0)
    {
        System.Console.WriteLine("恭喜你猜對了");
    }
    else {
        System.Console.WriteLine("很遺憾,你沒有猜中,正確答案是" + answer);
        break;
    }
} while (guess != answer);
System.Console.WriteLine("遊戲結束");

// 第十一節 C# for 迴圈
for (int i = 1; i <= 5; i++) { 
    System.Console.WriteLine("顆顆" + i);
}
for (int i = 1; i <= 100; i++) 
{
    if (i % 2 == 0) 
    {
        System.Console.WriteLine(i);
    }
}