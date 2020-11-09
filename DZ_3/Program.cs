using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using NStack;
using Terminal.Gui;
//Программа должна создать файл и выводить туда информацию (чему ровняется числовые ряды и раскрыть скобки (показать чему они будут равны) что-то типа по действиям)
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
            Application.Init();
            var top = Application.Top;
            // Creates the top-level window to show
            var win = new Window("DZ_3")
            {
                X = 0,
                Y = 0, // Leave one row for the toplevel menu

                // By using Dim.Fill(), it will automatically resize without manual intervention
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            top.Add(win);

            var frameView = new FrameView("How to")
            {
                X = 1,
                Y = 1,
                Width = Dim.Fill() - 1,
                Height = 6

            };

            frameView.Add(
                new Label(1, 0, "'v' - Объединение"),
                new Label(1, 1, "'^' - Пересечение"),
                new Label(1, 2, "'-' - Отрицание"),
                new Label(1, 3, "'\\' - Вычитание")
                );

            var aLabel = new Label("A:")
            {
                X = 1,
                Y = Pos.Bottom(frameView) + 1
            };
            var aText = new TextField("b,e,f,k,t")
            {
                X = Pos.Right(aLabel),
                Y = Pos.Top(aLabel),
                Width = 19
            };

            var bLabel = new Label("B:")
            {
                X = 1,
                Y = Pos.Bottom(aLabel) + 1
            };
            var bText = new TextField("f,i,j,p,y")
            {
                X = Pos.Right(bLabel),
                Y = Pos.Top(bLabel),
                Width = Dim.Width(aText)
            };

            var cLabel = new Label("С:")
            {
                X = 1,
                Y = Pos.Bottom(bLabel) + 1
            };
            var cText = new TextField("j,k,l,y")
            {
                X = Pos.Right(cLabel),
                Y = Pos.Top(cLabel),
                Width = Dim.Width(aText)
            };

            var dLabel = new Label("D:")
            {
                X = 1,
                Y = Pos.Bottom(cLabel) + 1
            };
            var dText = new TextField("i,j,s,t,u,y,z")
            {
                X = Pos.Right(dLabel),
                Y = Pos.Top(dLabel),
                Width = Dim.Width(aText)
            };

            // Add some controls, 
            win.Add(
                // The ones with my favorite layout system, Computed
                frameView,
                aLabel,
                aText,
                bLabel,
                bText,
                cLabel,
                cText,
                dLabel,
                dText
            );

            var xLabel = new Label("X:")
            {
                X = 1,
                Y = Pos.Bottom(dLabel) + 2
            };
            var xText = new TextField("(A^C)v(B^C)")
            {
                X = Pos.Right(xLabel),
                Y = Pos.Top(xLabel),
                Width = Dim.Width(aText)
            };

            var yLabel = new Label("Y:")
            {
                X = 1,
                Y = Pos.Bottom(xLabel) + 1
            };
            var yText = new TextField("(A^-B)v(D\\C)")
            {
                X = Pos.Right(yLabel),
                Y = Pos.Top(yLabel),
                Width = Dim.Width(aText)
            };
            var button = new Button("Calc!")
            {
                HotKey = Key.AltMask | Key.ControlC,
                X = 1,
                Y = Pos.Bottom(yLabel) + 1
            };
            button.Clicked += () =>
            {
                var text = $"A={aText.Text};B={bText.Text};C={cText.Text};D={dText.Text};X={xText.Text.ToUpper()};Y={yText.Text.ToUpper()}";

                text = text.Replace(" ", "").Replace("\n", "");

                try
                {
                    FillArrayList(text);

                    var strX = "";
                    foreach (var c in X) strX += $"{c}, ";
                    if (!string.IsNullOrEmpty(strX))
                    {
                        strX = strX.Remove(strX.Length - 2, 2);
                    }
                    var strY = "";
                    foreach (var c in Y) strY += $"{c}, ";
                    if (!string.IsNullOrEmpty(strY))
                    {
                        strY = strY.Remove(strY.Length - 2, 2);
                    }
                    MessageBox.Query(60, 6, "Result", $"X = ({strX})\nY = ({strY})", "Ok");
                }
                catch (Exception e)
                {
                    MessageBox.Query(60, 10, "Error", $"{e.Message}\n{e.Source}", "Ok");
                    return;
                }

            };
            var button2 = new Button("Save!")
            {
                HotKey = Key.AltMask | Key.ControlC,
                X = Pos.Right(button) + 1,
                Y = Pos.Bottom(yLabel) + 1
            };

            button2.Clicked += () =>
            {
                FileMeneger fileMeneger = new FileMeneger();

                var text = $"A={aText.Text};B={bText.Text};C={cText.Text};D={dText.Text};X={xText.Text.ToUpper()};Y={yText.Text.ToUpper()}";
                text = text.Replace(" ", "").Replace("\n", "");
                try
                {
                    FillArrayList(text);
                }
                catch (Exception e)
                {
                    MessageBox.Query(60, 10, "Error", $"{e.Message}\n{e.Source}", "Ok");
                    return;
                }
                string[] row = new string[4];
                row[0] = aText.Text.ToString();
                row[1] = aText.Text.ToString();
                row[2] = aText.Text.ToString();
                row[3] = aText.Text.ToString();
                for (int i = 0; i < 4; i++)
                {
                    row[i] = string.Join<char>(", ", row[i]) + ",";
                    row[i] = row[i].Remove(row[i].Length - 1);
                }
                fileMeneger.StringSave($"A = {{{row[0]}}}; B = {{{row[1]}}}; C = {{{row[2]}}}; D = {{{row[3]}}};");
                fileMeneger.StringSave($"X = { xText.Text.ToUpper().ToString().Replace('^', '\u2229').Replace('V', '\u222A')};");
                string tempX = xText.Text.ToUpper().ToString();
                string resX = "";
                while (tempX.IndexOf('(') != -1)
                {
                    int indexStart = tempX.IndexOf('(');
                    int indexStop = tempX.IndexOf(')') + 1;
                    string scope = tempX.Substring(indexStart, indexStop);
                    var strX = "";
                    foreach (var c in Calc(scope)) strX += $"{c}, ";
                    if (!string.IsNullOrEmpty(strX))
                    {
                        strX = strX.Remove(strX.Length - 2, 2);
                    }
                    if (strX.Length == 0)
                        resX += $"{scope} = \u2205;\n";
                    else
                        resX += $"{scope} = {{{strX}}};\n";
                    if (indexStop != tempX.Length)
                        tempX = tempX.Substring(indexStop + 1);
                    else
                    {
                        tempX = "";
                    }
                }
                //
                string strX2 = "";
                foreach (var c in X) strX2 += $"{c}, ";
                if (!string.IsNullOrEmpty(strX2))
                {
                    strX2 = strX2.Remove(strX2.Length - 2, 2);
                }
                string strY2 = "";
                foreach (var c in Y) strY2 += $"{c}, ";
                if (!string.IsNullOrEmpty(strY2))
                {
                    strY2 = strY2.Remove(strY2.Length - 2, 2);
                }
                //
                if(strX2.Length==0)
                    resX += $"X = { xText.Text.ToUpper()} = \u2205;";
                else
                    resX += $"X = { xText.Text.ToUpper()} = {{{strX2}}};";
                resX = resX.Replace('^', '\u2229').Replace('V', '\u2228');
                fileMeneger.StringSave(resX);

                fileMeneger.StringSave($"\nY = { yText.Text.ToUpper().ToString().Replace('^', '\u2229').Replace('V', '\u222A')};");
                string tempY = yText.Text.ToUpper().ToString();
                string resY = "";
                while (tempY.IndexOf('(') != -1)
                {
                    int indexStart = tempY.IndexOf('(');
                    int indexStop = tempY.IndexOf(')') + 1;
                    string scope = tempY.Substring(indexStart, indexStop);
                    var strY = "";
                    foreach (var c in Calc(scope)) strY += $"{c}, ";
                    if (!string.IsNullOrEmpty(strY))
                    {
                        strY = strY.Remove(strY.Length - 2, 2);
                    }
                    if (strY.Length == 0)
                        resY += $"{scope} = \u2205;\n";
                    else
                        resY += $"{scope} = {{{strY}}};\n";
                    if (indexStop != tempY.Length)
                        tempY = tempY.Substring(indexStop + 1);
                    else
                    {
                        tempY = "";
                    }
                }
                if(strY2.Length==0)
                    resY += $"Y = { yText.Text.ToUpper()} = \u2205;";
                else
                    resY += $"Y = { yText.Text.ToUpper()} = {{{strY2}}};";
                resY = resY.Replace('^', '\u2229').Replace('V', '\u222A');
                fileMeneger.StringSave(resY);

                var Mes = MessageBox.Query(60, 6, "Success", $"Data add to File: \"{Directory.GetCurrentDirectory()}\\Answer.txt\"!", "Ok!");
                fileMeneger.FileClose();
            };
            win.Add(
                xLabel,
                xText,
                yLabel,
                yText,
                button,
                button2
            );
            Application.Run();
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
            for (var i = 0; i < 4; i++)
            {
                var tempStrArr = textArr[i].Split('=');
                var arr = tempStrArr[1].Replace(",", "").ToCharArray();

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

            X = Calc(textArr[4].Split('=')[1]);
            X.Sort();

            Y = Calc(textArr[5].Split('=')[1]);
            Y.Sort();
        }

        private static List<char> Calc(string expression)
        {
            var temp = new List<char>();

            for (var i = 0; i < expression.Length; i++)
            {
                var c = expression[i];
                switch (c)
                {
                    case '-' when expression[i + 1] == '(':
                        {
                            var tempExpression = GetExpression(expression, ref i);

                            temp = Negation(Calc(tempExpression));
                            break;
                        }
                    case '-':
                        temp = Negation(Calc(expression[i + 1]));
                        i++;
                        break;
                    case '(':
                        {
                            var tempExpression = GetExpression(expression, ref i);
                            temp = Calc(tempExpression);
                            break;
                        }
                    case '^':
                        switch (expression[i + 1])
                        {
                            case '(':
                                {
                                    i++;
                                    var tempExpression = GetExpression(expression, ref i);
                                    temp = Crossing(temp, Calc(tempExpression));
                                    break;
                                }
                            case '-':
                                temp = Crossing(temp, Calc(expression.Substring(i + 1, 2)));
                                i += 2;
                                break;
                            default:
                                temp = Crossing(temp, Calc(expression[i + 1]));
                                i++;
                                break;
                        }

                        break;
                    case 'V':
                        switch (expression[i + 1])
                        {
                            case '(':
                                {
                                    i++;
                                    var tempExpression = GetExpression(expression, ref i);
                                    temp = Union(temp, Calc(tempExpression));
                                    break;
                                }
                            case '-':
                                temp = Union(temp, Calc(expression.Substring(i + 1, 2)));
                                i += 2;
                                break;
                            default:
                                temp = Union(temp, Calc(expression[i + 1]));
                                i++;
                                break;
                        }

                        break;
                    case '\\':
                        switch (expression[i + 1])
                        {
                            case '(':
                                {
                                    i++;
                                    var tempExpression = GetExpression(expression, ref i);
                                    temp = Subtraction(temp, Calc(tempExpression));
                                    break;
                                }
                            case '-':
                                temp = Subtraction(temp, Calc(expression.Substring(i + 1, 2)));
                                i += 2;
                                break;
                            default:
                                temp = Subtraction(temp, Calc(expression[i + 1]));
                                i++;
                                break;
                        }

                        break;
                    case 'A':
                    case 'B':
                    case 'C':
                    case 'D':
                        temp = Calc(c);
                        break;
                }
            }

            return temp;
        }

        private static string GetExpression(string expression, ref int i)
        {
            var tempExpression = "";
            i++;
            try
            {
                while (expression[i] != ')')
                {
                    tempExpression += expression[i];
                    i++;
                }
            }
            catch
            {
                throw new Exception("Missing scope!");
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