using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Runtime.CompilerServices;

namespace Kalkulator
{

    enum EOperacje
    {
        None, Clear, Clear1, ClearE, Neg, Sqrt, Pow2, Proc, Sum, Sub, Prod, Div, Equal, Opp
    };
    public class KalulatorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool clear;  // jeśli clear = true to czyścimy pamięć - dzięki temu można wpisać ułamki mniejsze niż 1 np. 0,2

        private EOperacje ostatnia_operacja; //operacja dwuargumentowa do wykonania
        private double y; // zapamiętuje 1. argument do operacji
        public void UpdateControls([CallerMemberName] String PropertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        private EOperacje Encode(object p)
        {
            EOperacje op;
            switch(p.ToString())
            {
                case "C":
                    op = EOperacje.Clear;
                    break;
                case "CE":
                    op = EOperacje.ClearE;
                    break;
                case "<=":
                    op = EOperacje.Clear1;
                    break;
                case "+/-":
                    op = EOperacje.Neg;
                    break;
                case "%":
                    op = EOperacje.Proc;
                    break;
                case "sqrt":
                    op = EOperacje.Sqrt;
                    break;
                case "x^2":
                    op = EOperacje.Pow2;
                    break;
                case "+":
                    op = EOperacje.Sum;
                    break;
                case "-":
                    op = EOperacje.Sub;
                    break;
                case "*":
                    op = EOperacje.Prod;
                    break;
                case "/":
                    op = EOperacje.Div;
                    break;
                case "=":
                    op = EOperacje.Equal;
                    break;
                case "1/x":
                    op = EOperacje.Opp;
                    break;

                default:
                    op = EOperacje.None;
                    break;
            }
            return op;
        }
        public KalulatorViewModel()
        {
            clear = true;
            Display = "0";
            AddNumber = new CommandClick(p =>
            {
                if (clear && p.ToString() != ",")
                {
                    Display = p.ToString();
                    
                }
                else
                    Display += p;
                clear = false;
            }); 

            CalcOp1 = new CommandClick(p =>
            {
                EOperacje op = Encode(p);
                switch(op)
                {
                    case EOperacje.ClearE:
                        if (y > 0)
                        {
                            Display = y.ToString();
                            x = 0;
                        }
                            
                        else
                        {
                            Display = "0";
                            clear = true;
                        }                            
                        break;
                    case EOperacje.Clear1:
                        if (Display.Length > 1)
                            Display = Display.Remove(Display.Length - 1);
                        break;
                    case EOperacje.Clear:
                        Display = "0";
                        clear = true;
                        break;
                    case EOperacje.Sqrt:
                        if (x > 0)
                            Display = Math.Sqrt(x).ToString();
                        clear = true;
                        break;
                    case EOperacje.Pow2:
                        if (x != 0)
                            Display = Math.Pow(x,2).ToString();
                        clear = true;
                        break;
                    case EOperacje.Neg:
                        if (x != 0)
                            x = x*-1;
                        Display = x.ToString();
                        clear = true;
                        break;
                    case EOperacje.Opp:
                        if (x != 0)
                            x = 1/x;
                        Display = x.ToString();
                        clear = true;
                        break;
                    case EOperacje.Proc:
                        if (x != 0)
                            x = x / 100;
                        Display = x.ToString();
                        clear = true;
                        break;

                }
                
            });

            CalcOp2 = new CommandClick(p =>
            {
                CalcOperacja(ostatnia_operacja); 
                ostatnia_operacja = Encode(p);
                y = x; //zapamiętuje 1. argument
                clear = true;

            }
            );
        }

        private void CalcOperacja(EOperacje op)
        {
            switch(op)
            {
                case EOperacje.Sum:
                    Display = (y + x).ToString();
                    break;
                case EOperacje.Sub:
                    Display = (y - x).ToString();
                    break;
                case EOperacje.Prod:
                    Display = (y * x).ToString();
                    break;
                case EOperacje.Div:
                    Display = (y / x).ToString();
                    break;
            }
            ostatnia_operacja = EOperacje.None;
        }

        private String _display;
        private double x;

        public String Display
        {
            get { return _display; }
            set
            {
                if (value != _display)
                {
                    try
                    {
                        x = Double.Parse(value);
                        _display = value;
                        UpdateControls();
                    }
                    catch(Exception ee)
                    { }

                    
                }
            }
        }

        
        public ICommand AddNumber{get;}
        public ICommand CalcOp1 { get; }
        public ICommand CalcOp2 { get; }

    }
}
