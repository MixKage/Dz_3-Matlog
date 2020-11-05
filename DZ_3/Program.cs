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
        public static string TEXT = "A={b,e,f,k,t};B={f,i,j,p,y};C={j,k,l,y};D={i,j,s,t,u,y,z};X={(A\u2229C)\u22C3(-B\u2229C)}";
        public static List<char> A = new List<char>();
        public static List<char> B = new List<char>();
        public static List<char> C = new List<char>();
        public static List<char> D = new List<char>();
        public static List<char> U = new List<char>();
        public static List<char> Temp = new List<char>();
        public static List<char> UR_1 = new List<char>();
        public static List<char> UR_2 = new List<char>();
        static int TEXTLength = TEXT.Length;


        static void Main(string[] args)
        {
            CreateArrayList();
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
            char A2;
            for (int L1 = vrm; L1 < TEXTLength; L1++)
            {
                if (!TF)
                {
                    if ((TEXT[L1] == 'A') || (TEXT[L1] == 'B') || (TEXT[L1] == 'C') || (TEXT[L1] == 'D'))
                    {
                        List<char> A1 = new List<char>();
                        switch (TEXT[L1])
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
                        if (TEXT[L1 - 1] == '-')
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
            bool TF = false;
            int vrm = 0;
            bool vUR = false;

            for (int L1 = 0; L1 < TEXTLength; L1++)
            {
                if (TEXT[L1] == 'A')
                {
                    TF = false;
                    for (int count_1 = L1 + 1; count_1 < TEXTLength; count_1++)
                    {
                        if (TF == false)
                        {
                            if (TEXT[count_1] == '=')
                            {
                                continue;
                            }
                            else if (TEXT[count_1] == ',')
                            {
                                continue;
                            }
                            else if (TEXT[count_1] == '{')
                            {
                                continue;
                            }
                            else if (TEXT[count_1] == '}')
                            {
                                TF = true;
                                L1 = count_1;
                            }
                            else
                            {
                                A.Add(TEXT[count_1]);
                            }
                        }
                        else
                        {
                            TF = false;
                            break;
                        }
                    }
                }
                else if (TEXT[L1] == 'B')
                {
                    TF = false;
                    for (int count_1 = L1 + 1; count_1 < TEXTLength; count_1++)
                    {
                        if (TF == false)
                        {
                            if (TEXT[count_1] == '=')
                            {
                                continue;
                            }
                            else if (TEXT[count_1] == ',')
                            {
                                continue;
                            }
                            else if (TEXT[count_1] == '{')
                            {
                                continue;
                            }
                            else if (TEXT[count_1] == '}')
                            {
                                TF = true;
                                L1 = count_1;
                            }
                            else
                            {
                                B.Add(TEXT[count_1]);
                            }
                        }
                        else
                        {
                            TF = false;
                            break;
                        }
                    }
                }
                else if (TEXT[L1] == 'C')
                {
                    TF = false;
                    for (int count_1 = L1 + 1; count_1 < TEXTLength; count_1++)
                    {
                        if (TF == false)
                        {
                            if (TEXT[count_1] == '=')
                            {
                                continue;
                            }
                            else if (TEXT[count_1] == ',')
                            {
                                continue;
                            }
                            else if (TEXT[count_1] == '{')
                            {
                                continue;
                            }
                            else if (TEXT[count_1] == '}')
                            {
                                TF = true;
                                L1 = count_1;
                            }
                            else
                            {
                                C.Add(TEXT[count_1]);
                            }
                        }
                        else
                        {
                            TF = false;
                            break;
                        }
                    }
                }
                else if (TEXT[L1] == 'D')
                {
                    TF = false;
                    for (int count_1 = L1 + 1; count_1 < TEXTLength; count_1++)
                    {
                        if (TF == false)
                        {
                            if (TEXT[count_1] == '=')
                            {
                                continue;
                            }
                            else if (TEXT[count_1] == ',')
                            {
                                continue;
                            }
                            else if (TEXT[count_1] == '{')
                            {
                                continue;
                            }
                            else if (TEXT[count_1] == '}')
                            {
                                TF = true;
                                L1 = count_1;
                            }
                            else
                            {
                                D.Add(TEXT[count_1]);
                            }
                        }
                        else
                        {
                            TF = false;
                            break;
                        }
                    }
                }

                else if (TEXT[L1] == 'X')
                {
                    vrm = L1;
                    break;
                }
            }

            U = Union(Union(A, B), Union(C, D));

            UR_1.Clear();
            UR_2.Clear();
            Console.WriteLine("This is U:");
            for (int i = 0; i < U.Count; i++)
            {
                Console.WriteLine(U[i]);
            }
            Console.WriteLine("This is C:");
            for (int i = 0; i < C.Count; i++)
            {
                Console.WriteLine(C[i]);
            }
            var NegativeC = Negation(C);
            Console.WriteLine("This is -C:");
            foreach (var t in NegativeC)
            {
                Console.WriteLine(t);
            }
            //string ur1;
            //string ur2;
            TF = false;


            for (int L2 = vrm; L2 < TEXTLength; L2++)
            {
                if (TF == false)
                {
                    if (TEXT[L2] == 'X')
                    {
                        TF = true;
                    }
                }
                else
                {
                    if (TEXT[L2] == '=')
                    {
                        continue;
                    }
                    else if (TEXT[L2] == '{')
                    {
                        continue;
                    }
                    else if (TEXT[L2] == '(')
                    {
                        if (vUR == false)
                        {
                            vUR = true;
                            vrm = L2;
                            UR1(vrm);
                        }
                        else
                        {
                            //UR_2(vrm);
                        }
                    }
                }
            }

            Console.WriteLine("This is A:");
            for (int i = 0; i < A.Count; i++)
            {
                Console.WriteLine(A[i]);
            }
            Console.WriteLine("This is B:");
            for (int i = 0; i < B.Count; i++)
            {
                Console.WriteLine(B[i]);
            }
            Console.WriteLine("This is C:");
            for (int i = 0; i < C.Count; i++)
            {
                Console.WriteLine(C[i]);
            }
            Console.WriteLine("This is D:");
            for (int i = 0; i < D.Count; i++)
            {
                Console.WriteLine(D[i]);
            }
        }
    }
}
