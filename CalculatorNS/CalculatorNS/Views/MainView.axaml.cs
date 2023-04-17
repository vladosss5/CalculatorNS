using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace CalculatorNS.Views;

public partial class MainView : UserControl
{
    List<int> firstnumber = new List<int>();
    List<int> secondnumber = new List<int>();

    int currentState = 1;
    float numeric1 = 0;
    float numeric2 = 0;
    string dividend = "";
    string divisor = "";
    int notation = 10;
    int maxlist = 0;
    int minlist = 0;
    char operation = '+';
    public MainView()
    {
        InitializeComponent();
        Clear(this, null);
    }

    void Clear(object? sender, RoutedEventArgs e)
    {
        numeric1 = 0;
        numeric2 = 0;
        currentState = 1;
        this.results.Text = "0";
    }

    private void NumberSelection(object? sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        string buttonPressed = button.Content.ToString();
        if (this.results.Text == "0" || currentState < 0)
        {
            this.results.Text = string.Empty;
            if (currentState < 0)
            {
                currentState *= -1;
            }
        }

        this.results.Text += buttonPressed;

        float number;
        if (float.TryParse(this.results.Text,out number))
        {
            this.results.Text = number.ToString();
            if (currentState == 1)
            {
                firstnumber.Clear();
                Сrusher(number, firstnumber);
                numeric1 = number;
                dividend = Convert.ToString(number);
            }
            else
            {
                secondnumber.Clear();
                Сrusher(number, secondnumber);
                numeric2 = number;
                divisor = Convert.ToString(number);
            }
        }
    }

    void OperationSelection(object? sender, RoutedEventArgs e)
    {
        currentState = -2;
        Button button = (Button)sender;
        operation = Convert.ToChar(button.Content);
        this.results.Text = "0";
    }

    private void Calculate(object? sender, RoutedEventArgs e)
    {
        notation = Convert.ToInt32(this.notationIn.SelectedIndex) + 2;

        if (currentState == 2)
        {
            switch (operation)
            {
                case '+':
                    Adder();
                    break;
                case '-':
                    Subtractor();
                    break;
                case 'x' :
                    Multiplier();
                    break;
                case '/':
                    Divide();
                    break;
                case '%':
                    RemOfDevision();
                    break;
            }
        }
        else
        {
            Clear(this, null);
        }
    }
    
    static void Сrusher(float numeric1, List<int> ints) 
    {
        int numeric = Convert.ToInt32(numeric1);
        for (float i = 0; numeric != 0; ++i)
        {
            ints.Add(Convert.ToInt32(numeric % 10));
            numeric /= 10;
        }
    }

    private void Adder()
    {
        if (firstnumber.Count >= secondnumber.Count)
        {
            maxlist = firstnumber.Count;
            minlist = secondnumber.Count;
        }
        else
        {
            maxlist = secondnumber.Count;
            minlist = firstnumber.Count;
        }
        
        int[] preresult = new int [maxlist + 1];

        for (int i = 0; i < maxlist; ++i)
        {
            if (minlist > i)
            {
                if (firstnumber[i] + secondnumber[i] + preresult[i] < notation)
                {
                    preresult[i] += firstnumber[i] + secondnumber[i];
                }
                else
                {
                    preresult[i] = Math.Abs(preresult[i] + secondnumber[i] - (notation - firstnumber[i]));
                    preresult[i + 1]++;
                } 
            }
            else if (firstnumber.Count <= i)
            {
                preresult[i] += secondnumber[i];
            }
            else
            {
                preresult[i] += firstnumber[i];
            }
        }
        
        this.results.Text = preresult.Reverse().Aggregate(string.Empty, (s, i) => s + i).TrimStart('0');
    }

    private void Subtractor()
    {
        int[] preresult = new int [firstnumber.Count];
    
        for (int i = 0; i < firstnumber.Count; ++i)
        {
            if (secondnumber.Count > i)
            {
                if (firstnumber[i] >= secondnumber[i])
                {
                    preresult[i] = firstnumber[i] - secondnumber[i];
                }
                else
                {
                    preresult[i] = notation - secondnumber[i] + firstnumber[i];
                    firstnumber[i + 1]--;
                }
            }
            else
            {
                preresult[i] = firstnumber[i];
            }
        }
        this.results.Text = preresult.Reverse().Aggregate(string.Empty, (s, i) => s + i);
    }
    

    private void Multiplier()
    {
        int[] number1 = new int[firstnumber.Count];
        int[] number2 = new int[secondnumber.Count];
        int[] preresult = new int[number1.Length + number2.Length];
        Aggregate(firstnumber, number1);
        Aggregate(secondnumber, number2);

        for (int i = number1.Length - 1; i >= 0; --i)
        {
            for (int j = number2.Length - 1; j >= 0; --j)
            {
                int multiplier = number1[i] * number2[j];
                int pos1 = i + j;
                int pos2 = i + j + 1;
                int sum = multiplier + preresult[pos2];
                preresult[pos1] += sum / notation;
                preresult[pos2] = sum % notation;
            }
        }
        this.results.Text = preresult.Reverse().Aggregate(string.Empty, (s, i) => s + i).TrimStart('0');
    }
    
    static void Aggregate(List<int> list, int[] number)
    {
        for (int i = 0; i < list.Count; ++i)
        {
            number[i] = list[i];
        }
    }

    private void RemOfDevision()
    {
        string dividend = Convert.ToString(numeric1);
        string divisor = Convert.ToString(numeric2);
        int divisorValue = 0;
        int remainder = 0;
                
        for (int i = 0; i < divisor.Length; i++)
        {
            divisorValue += (int)(Char.GetNumericValue(divisor[i]) * Math.Pow(notation, divisor.Length - i - 1));
        }
                
        for (int i = 0; i < dividend.Length; i++)
        {
            int digit = (int)Char.GetNumericValue(dividend[i]);
            remainder = (remainder * notation + digit) % divisorValue;
        }

        this.results.Text = remainder.ToString();
    }

    private void Division()
    {
        this.results.Text = Convert.ToString(numeric1 / numeric2);
    }
    
    private void Divide()
        {
            string result = "";
            string remainder = "";
            int len = dividend.Length;
            for (int i = 0; i < len; i++)
            {
                remainder += dividend[i];
                int count = 0;
                while (Compare(remainder, divisor, notation) >= 0)
                {
                    remainder = Subtract(remainder, divisor, notation);
                    count++;
                }
                result += count.ToString();
            }
            this.results.Text = result.TrimStart('0');
        }

        static int Compare(string num1, string num2, int system)
        {
            int len1 = num1.Length;
            int len2 = num2.Length;
            if (len1 < len2)
            {
                return -1;
            }
            else if (len1 > len2)
            {
                return 1;
            }
            else
            {
                for (int i = 0; i < len1; i++)
                {
                    int digit1 = int.Parse(num1[i].ToString());
                    int digit2 = int.Parse(num2[i].ToString());
                    if (digit1 < digit2)
                    {
                        return -1;
                    }
                    else if (digit1 > digit2)
                    {
                        return 1;
                    }
                }
                return 0;
            }
        }

        static string Subtract(string num1, string num2, int system)
        {
            string result = "";
            int borrow = 0;
            int len1 = num1.Length;
            int len2 = num2.Length;
            int maxLen = Math.Max(len1, len2);
            for (int i = 0; i < maxLen; i++)
            {
                int digit1 = (i < len1) ? int.Parse(num1[len1 - 1 - i].ToString()) : 0;
                int digit2 = (i < len2) ? int.Parse(num2[len2 - 1 - i].ToString()) : 0;
                int diff = digit1 - digit2 - borrow;
                if (diff < 0)
                {
                    diff += system;
                    borrow = 1;
                }
                else
                {
                    borrow = 0;
                }
                result = diff.ToString() + result;
            }
            return result.TrimStart('0');
        }
}