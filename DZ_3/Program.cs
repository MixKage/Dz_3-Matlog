using System;
using System.Collections.Generic;
using System.Linq;

namespace DZ_3
{
    internal class Program
    {
        //\u2229-Пересечение
        //\u22C3-Объединение
        public static string Text =
            "A={b,e,f,k,t};B={f,i,j,p,y};C={j,k,l,y};D={i,j,s,t,u,y,z};X={(A\u2229C)\u22C3(-B\u2229C)}";

        public static List<char> A = new List<char>();
        public static List<char> B = new List<char>();
        public static List<char> C = new List<char>();
        public static List<char> D = new List<char>();
        public static List<char> U = new List<char>();
        public static List<char> X = new List<char>();
        private static readonly int TextLength = Text.Length;


        private static void Main(string[] args)
        {
            FillArrayList(Text);

            Console.WriteLine("This is A:");
            foreach (var c in A) Console.WriteLine(c);
            Console.WriteLine("This is B:");
            foreach (var c in B) Console.WriteLine(c);
            Console.WriteLine("This is C:");
            foreach (var c in C) Console.WriteLine(c);
            Console.WriteLine("This is D:");
            foreach (var c in D) Console.WriteLine(c);
        }

        public static List<char> Union(List<char> list1, List<char> list2)
        {
            var Temp = new List<char>(list1);
            Temp.AddRange(list2);
            Temp = Temp.Distinct().ToList();
            return Temp;
        }

        public static List<char> Crossing(List<char> list1, List<char> list2)
        {
            return list1.Where(list2.Contains).ToList();
        }

        public static List<char> Negation(List<char> list) //Не чистит за собой Temp
        {
            var Temp = new List<char>(U);
            foreach (var t in list) Temp.Remove(t);

            return Temp;
        }

        public static void FillArrayList(string text)
        {
            var textArr = text.Split(';');
            int startIndex;
            for (int i = 0; i < 4; i++)
            {
                var tempStrArr = textArr[i].Split('=');
                startIndex = tempStrArr[1].IndexOf('{');
                var values = tempStrArr[1].Substring(startIndex + 1, tempStrArr[1].Length - startIndex - 2);
                var arr = values.Replace(",","").ToCharArray();

                switch (tempStrArr[0][0])
                {
                    case 'A':
                        A = new List<char>(arr);
                        break;
                    case 'B':
                        B = new List<char>(arr);
                        break;
                    case 'C':
                        C = new List<char>(arr);
                        break;
                    case 'D':
                        D = new List<char>(arr);
                        break;
                }
            }
            startIndex = textArr[4].IndexOf('{');
            X = Calc(textArr[4].Substring(startIndex + 1, textArr[4].Length - startIndex - 2));
        }
        public static void CreateArrayList()
        {
            var vrm = 0;

            bool exitFlag;
            for (var i = 0; i < TextLength; i++)
            {
                var temp = new List<char>();
                switch (Text[i])
                {
                    case 'A':
                        temp = A;
                        break;
                    case 'B':
                        temp = B;
                        break;
                    case 'C':
                        temp = C;
                        break;
                    case 'D':
                        temp = D;
                        break;
                    case 'X':
                        vrm = i;
                        i = TextLength;
                        break;
                }

                exitFlag = false;
                for (var j = i + 1; j < TextLength; j++)
                    if (!exitFlag)
                        switch (Text[j])
                        {
                            case '=':
                            case ',':
                            case '{':
                            case '}':
                                continue;
                            case ';':
                                i = j;
                                exitFlag = true;
                                break;
                            default:
                                temp.Add(Text[j]);
                                break;
                        }
                    else
                        break;
            }

            U = Union(Union(A, B), Union(C, D));

            Console.WriteLine("This is U:");
            foreach (var c in U) Console.WriteLine(c);

            var subStr = "";
            var read = false;
            for (var i = vrm; i < TextLength; i++)
            {
                if (Text[i] == '{')
                {
                    read = true;
                    continue;
                }

                if (Text[i] == '}')
                {
                    read = false;
                    break;
                }

                if (read) subStr += Text[i];
            }

            Console.WriteLine("This is X:");
            var res = Calc(subStr);
            foreach (var c in res) Console.WriteLine(c);
        }

        private static List<char> Calc(string expression)
        {
            var temp = new List<char>();

            for (var i = 0; i < expression.Length; i++)
            {
                var c = expression[i];
                if (c == '-')
                {
                    if (expression[i + 1] == '(')
                    {
                        var tempExpression = GetExpression(expression, ref i);

                        temp = Negation(Calc(tempExpression));
                    }
                    else
                    {
                        temp = Negation(Calc(expression[i + 1]));
                    }
                }
                else if (c == '(')
                {
                    var tempExpression = GetExpression(expression, ref i);
                    temp = Calc(tempExpression);
                }
                else if (c == '\u22C3')
                {
                    if (expression[i + 1] == '(')
                    {
                        i++;
                        var tempExpression = GetExpression(expression, ref i);
                        temp = Union(temp, Calc(tempExpression));
                    }
                    else
                    {
                        temp = Union(temp, Calc(expression[i + 1]));
                        i++;
                    }
                }
                else if (c == '\u2229')
                {
                    if (expression[i + 1] == '(')
                    {
                        i++;
                        var tempExpression = GetExpression(expression, ref i);
                        temp = Crossing(temp, Calc(tempExpression));
                    }
                    else
                    {
                        temp = Crossing(temp, Calc(expression[i + 1]));
                        i++;
                    }
                }
                else if (c == 'A' || c == 'B' || c == 'C' || c == 'D')
                {
                    temp = Calc(c);
                }
            }

            return temp;
        }

        private static string GetExpression(string expression, ref int i)
        {
            var tempExpression = "";
            i++;
            while (expression[i] != ')')
            {
                tempExpression += expression[i];
                i++;
            }

            return tempExpression;
        }

        private static List<char> Calc(char expression)
        {
            var temp = new List<char>();
            switch (expression)
            {
                case 'A':
                    temp = A;
                    break;
                case 'B':
                    temp = B;
                    break;
                case 'C':
                    temp = C;
                    break;
                case 'D':
                    temp = D;
                    break;
                case 'X':
                    temp = X;
                    break;
            }

            return temp;
        }
    }
}