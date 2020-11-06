using System;
using System.Collections.Generic;
using System.Linq;

namespace DZ_3
{
    internal class Program
    {
        //^-Пересечение
        //v-Объединение
        public static string Text =
            "A={b,e,f,k,t};B={f,i,j,p,y};C={j,k,l,y};D={i,j,s,t,u,y,z};X={(A^C)v(B^C)};Y={(A^-B)v(D\\C)}";

        public static List<char> A = new List<char>();
        public static List<char> B = new List<char>();
        public static List<char> C = new List<char>();
        public static List<char> D = new List<char>();
        public static List<char> U = new List<char>();
        public static List<char> X = new List<char>();
        public static List<char> Y = new List<char>();

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
            Console.WriteLine("This is U:");
            foreach (var c in U) Console.WriteLine(c);
            Console.WriteLine("\nThis is X:");
            foreach (var c in X) Console.WriteLine(c);
            Console.WriteLine("\nThis is Y:");
            foreach (var c in Y) Console.WriteLine(c);
        }

        public static List<char> Union(List<char> list1, List<char> list2)
        {
            var temp = new List<char>(list1);
            temp.AddRange(list2);
            temp = temp.Distinct().ToList();
            return temp;
        }
        public static List<char> Crossing(List<char> list1, List<char> list2)
        {
            return list1.Where(list2.Contains).ToList();
        }

        public static List<char> Negation(List<char> list) //Не чистит за собой Temp
        {
            var temp = new List<char>(U);
            foreach (var t in list) temp.Remove(t);

            return temp;
        }

        public static List<char> Subtraction(List<char> list1, List<char> list2)
        {
            var temp = new List<char>(list1);
            foreach (var t in list2) temp.Remove(t);
            return temp;
        }


        public static void FillArrayList(string text)
        {
            var textArr = text.Split(';');
            int startIndex;
            for (var i = 0; i < 4; i++)
            {
                var tempStrArr = textArr[i].Split('=');
                startIndex = tempStrArr[1].IndexOf('{');
                var values = tempStrArr[1].Substring(startIndex + 1, tempStrArr[1].Length - startIndex - 2);
                var arr = values.Replace(",", "").ToCharArray();

                switch (tempStrArr[0][0])
                {
                    case 'A':
                        A = new List<char>(arr);
                        A.Sort();
                        break;
                    case 'B':
                        B = new List<char>(arr);
                        B.Sort();
                        break;
                    case 'C':
                        C = new List<char>(arr);
                        C.Sort();
                        break;
                    case 'D':
                        D = new List<char>(arr);
                        D.Sort();
                        break;
                }
            }

            U = Union(Union(A, B), Union(C, D));
            U.Sort();

            startIndex = textArr[4].IndexOf('{');
            X = Calc(textArr[4].Substring(startIndex + 1, textArr[4].Length - startIndex - 2));
            X.Sort();

            startIndex = textArr[5].IndexOf('{');
            Y = Calc(textArr[5].Substring(startIndex + 1, textArr[5].Length - startIndex - 2));
            Y.Sort();
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
                        i++;
                    }
                }
                else if (c == '(')
                {
                    var tempExpression = GetExpression(expression, ref i);
                    temp = Calc(tempExpression);
                }
                else if (c == '^')
                {
                    if (expression[i + 1] == '(')
                    {
                        i++;
                        var tempExpression = GetExpression(expression, ref i);
                        temp = Crossing(temp, Calc(tempExpression));
                    }
                    else if (expression[i + 1] == '-')
                    {
                        temp = Crossing(temp, Calc(expression.Substring(i + 1, 2)));
                        i += 2;
                    }
                    else
                    {
                        temp = Crossing(temp, Calc(expression[i + 1]));
                        i++;
                    }
                }
                else if (c == 'v')
                {
                    if (expression[i + 1] == '(')
                    {
                        i++;
                        var tempExpression = GetExpression(expression, ref i);
                        temp = Union(temp, Calc(tempExpression));
                    }
                    else if (expression[i + 1] == '-')
                    {
                        temp = Union(temp, Calc(expression.Substring(i+1,2)));
                        i+=2;
                    }
                    else
                    {
                        temp = Union(temp, Calc(expression[i + 1]));
                        i++;
                    }
                }
                else if (c == '\\')
                {
                    if (expression[i + 1] == '(')
                    {
                        i++;
                        var tempExpression = GetExpression(expression, ref i);
                        temp = Subtraction(temp, Calc(tempExpression));
                    }
                    else
                    {
                        temp = Subtraction(temp, Calc(expression[i + 1]));
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