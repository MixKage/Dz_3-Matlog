using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DZ_3
{
    class Program
    {
        //\u2229-Пересечение
        //\u22C3-Объединение
        public static string Text = "A={b,e,f,k,t};B={f,i,j,p,y};C={j,k,l,y};D={i,j,s,t,u,y,z};X={(A\u2229C)\u22C3(-B\u2229C)}";
        public static List<char> A = new List<char>();
        public static List<char> B = new List<char>();
        public static List<char> C = new List<char>();
        public static List<char> D = new List<char>();
        public static List<char> U = new List<char>();
        public static List<char> X = new List<char>();
        static readonly int TextLength = Text.Length;


        static void Main(string[] args)
        {
            CreateArrayList();


            Console.WriteLine("This is A:");
            foreach (var c in A)
            {
                Console.WriteLine(c);
            }
            Console.WriteLine("This is B:");
            foreach (var c in B)
            {
                Console.WriteLine(c);
            }
            Console.WriteLine("This is C:");
            foreach (var c in C)
            {
                Console.WriteLine(c);
            }
            Console.WriteLine("This is D:");
            foreach (var c in D)
            {
                Console.WriteLine(c);
            }
        }

        public static List<char> Union(List<char> list1, List<char> list2)
        {
            List<char> Temp = new List<char>(list1);
            Temp.AddRange(list2);
            Temp = Temp.Distinct().ToList();
            return Temp;
        }

        public static List<char> Negation(List<char> list)//Не чистит за собой Temp
        {
            List<char> Temp = new List<char>(U);
            foreach (var t in list)
            {
                Temp.Remove(t);
            }

            return Temp;
        }

        public static void UR1(int vrm)
        {
            bool TF = false;
            for (int L1 = vrm; L1 < TextLength; L1++)
            {
                if (!TF)
                {
                    if ((Text[L1] == 'A') || (Text[L1] == 'B') || (Text[L1] == 'C') || (Text[L1] == 'D'))
                    {
                        List<char> A1 = new List<char>();
                        switch (Text[L1])
                        {
                            case 'A':
                                A1 = A;
                                break;
                            case 'B':
                                A1 = B;
                                break;
                            case 'C':
                                A1 = C;
                                break;
                            case 'D':
                                A1 = D;
                                break;
                        }
                        TF = true;
                        if (Text[L1 - 1] == '-')
                        {
                            Negation(A1);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {

                }
            }
        }

        public static void CreateArrayList()
        {
            int vrm = 0;
            bool vUR = false;

            bool exitFlag;
            for (int i = 0; i < TextLength; i++)
            {
                List<char> temp = new List<char>();
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
                for (int j = i + 1; j < TextLength; j++)
                {
                    if (!exitFlag)
                    {
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
                    }
                    else
                    {
                        break;
                    }
                }
            }

            U = Union(Union(A, B), Union(C, D));

            Console.WriteLine("This is U:");
            foreach (var c in U)
            {
                Console.WriteLine(c);
            }
            Console.WriteLine("This is C:");
            foreach (var c in C)
            {
                Console.WriteLine(c);
            }
            var negativeC = Negation(C);
            Console.WriteLine("This is -C:");
            foreach (var t in negativeC)
            {
                Console.WriteLine(t);
            }

            string subStr = "";
            bool read = false;
            for (int i = vrm; i < TextLength; i++)
            {
                if (Text[i] == '{')
                {
                    read = true;
                    continue;
                }
                else if (Text[i] == '}')
                {
                    read = false;
                    break;
                }

                if (read)
                {
                    subStr += Text[i];
                }
            }

            Console.WriteLine("This is D\u22C3C:");
            var res = Union(D,C);
            foreach (var c in res)
            {
                Console.WriteLine(c);
            }
            Console.WriteLine("This is D\u22C3C:");
            res = Calc("(D\u22C3C)");
            foreach (var c in res)
            {
                Console.WriteLine(c);
            }
            Calc(subStr);

            exitFlag = false;
            for (int i = vrm; i < TextLength; i++)
            {
                if (!exitFlag)
                {
                    if (Text[i] == 'X')
                    {
                        exitFlag = true;
                    }
                }
                else
                {
                    switch (Text[i])
                    {
                        case '=':
                        case '{':
                            continue;
                        case '(' when vUR == false:
                            vUR = true;
                            vrm = i;
                            UR1(vrm);
                            break;
                        case '(':
                            //UR_2(vrm);
                            break;
                    }
                }
            }

        }

        private static List<char> Calc(string expression)
        {
            List<char> temp = new List<char>();

            for (var i = 0; i < expression.Length; i++)
            {
                var c = expression[i];
                if (c == '-')
                {
                    if (expression[i + 1] == '(')
                    {
                        string tempExpression = "";
                        i++;
                        while (expression[i] != ')')
                        {
                            tempExpression += expression[i];
                            i++;
                        }
                        temp = Negation(Calc(tempExpression));

                    }
                    else
                    {
                        temp = Negation(Calc(expression[i + 1]));
                    }
                }
                else if (c == '(')
                {
                    string tempExpression = "";
                    i++;
                    while (expression[i] != ')')
                    {
                        tempExpression += expression[i];
                        i++;
                    }
                    temp = Calc(tempExpression);
                }
                else if (c == '\u22C3')
                {
                    temp = Union(temp, Calc(expression[i + 1]));
                    i++;
                }
                else if (c == 'A' || c == 'B' || c == 'C' || c == 'D')
                {
                    temp = Calc(c);
                }

            }

            return temp;
        }

        private static List<char> Calc(char expression)
        {
            List<char> temp = new List<char>();
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
