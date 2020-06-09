using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace EventTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Customer customer = new Customer();
            Waiter waiter = new Waiter();
            customer.Order += waiter.Action;
            customer.Action();
            customer.PayBill();
            System.Timers.Timer time = new System.Timers.Timer();
            time.Interval = 1000;
            Gril gril = new Gril();
            time.Elapsed += Gril.Action;
            time.Start();
            Console.ReadLine();
            Form form = new Form();
            Controller controller = new Controller(form);
            form.ShowDialog();
            form.Dispose();
            MyForm form1 = new MyForm();
            form1.sum(500,7589);
            form1.ShowDialog();

        }


    }
    public class MyForm : Form
    {
       private TextBox textBox;
       private Button button;
        public MyForm()
        {
            textBox = new TextBox();
            button = new Button();
            this.Controls.Add(textBox);
            this.Controls.Add(button);
            button.Click += this.ButtonClicked;
            button.Text = "Say Hello";
            button.Top = 50;
            
        }

       public void ButtonClicked(object sender, EventArgs e)
        {
            textBox.Text = "Hello Wold!!!!!!!!!!!";
        }
    }
    public class Controller
    {
        private Form form;
        public Controller(Form form) 
        {
            if (form != null)
            {
                this.form = form;
                this.form.Click += this.Action;
            }
        }

        public void Action(object sender, EventArgs e)
        {

            this.form.Text = DateTime.Now.ToString();
        }
    }
    internal class Gril
    {
        internal static void Action(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Sing!!!");
        }
    }

    public class OrderEventArgs:EventArgs
    {
        public string DishName { get; set; }
        public string Size { get; set; }
    }

    public class Customer
    {
        //private OrderEventHandler orderEventHandler;

        public event EventHandler Order;
        //{
        //    add { this.orderEventHandler += value; }
        //    remove { this.orderEventHandler -= value; }
        //}
        public double Bill { get; set; }
        public void PayBill()
        {
            Console.WriteLine("I Will Pay ${0}", this.Bill);
        }
        public void WalkIn()
        {
            Console.WriteLine("Walk into the restaurant");
        }
        public void SitDown()
        {
            Console.WriteLine("Sit Down.");
        }
        public void Think()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Let me think.....");
                Thread.Sleep(1000);
            }
            OnOrder("Fire Chicken", "large");
        }
        protected void OnOrder(string DishName,string Size)
        {
            if (Order != null)
            {
                OrderEventArgs e = new OrderEventArgs();
                e.DishName = DishName;
                e.Size = Size;
                Order.Invoke(this, e);
            }
        }
        public void Action()
        {
            Console.ReadLine();
            this.WalkIn();
            this.SitDown();
            this.Think();

        }
    }
    public class Waiter
    {
        public void Action(Object sender, EventArgs e)
        {
            Customer customer = sender as Customer;
            OrderEventArgs orderEventArgs = e as OrderEventArgs;
            Console.WriteLine("I will server you the dish - {0}", orderEventArgs.DishName);
            double price = 100;
            switch (orderEventArgs.Size)
            {
                case"small":
                    price *= 0.8;
                    break;
                case "large":
                    price *= 1.5;
                    break;
                default:
                    break;

            }
            customer.Bill += price;
        }
    }
    public static class ExtendMethod
    {
        
        public static void sum(this Form form, int num1, int num2)
        {
            form.Text = (num1 + num2).ToString();
        }
    }
}
