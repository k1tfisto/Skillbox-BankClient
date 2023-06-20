using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BankLibrary;

namespace Practice13
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random r;
        private readonly string path = "Bank.txt";
        private readonly string pathAcc = "Accounts.txt";
        ObservableCollection<Employee> employees = new ObservableCollection<Employee>();
        ObservableCollection<Accounts> accounts = new ObservableCollection<Accounts>();

        public event Action<string> journalLog;
        Logs logs = new Logs();


        private string defaultText = "Введите сумму....";

        public MainWindow()
        {
            InitializeComponent();
            closeAcc.IsEnabled = false;
            addAmount.IsEnabled = false;
            delAmount.IsEnabled = false;
            journalLog += MainWindow_journalLog;
            journalBox.ItemsSource = logs.logFile;
            employees.CollectionChanged += Employees_CollectionChanged;
        }

        public void Employees_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace)
            {
                Employee oldChange = e.OldItems as Employee;
                Employee newChange = e.NewItems as Employee;
                journalLog?.Invoke($"Клиенту {oldChange.ToString()} изменили данные на {newChange.ToString()}");
            }
        }

        private void MainWindow_journalLog(string obj)
        {
            logs.AddToLog(obj);
        }

        private void MngDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] args = sr.ReadLine().Split('#');

                        employees.Add(new Employee(args[0], args[1], args[2], args[3], args[4]));

                        mngDataGrid.ItemsSource = employees;
                    }
                }
            }
        }


        private void Save_Click(object sender, RoutedEventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(path))
                foreach (var personal in employees)
                {
                    string temp = String.Format("{0}#{1}#{2}#{3}#{4}",
                                            personal.LastName,
                                            personal.FirstName,
                                            personal.MiddleName,
                                            personal.NumberPhone,
                                            personal.PassportEmpl);

                    sw.WriteLine($"{temp}");
                }

            using (StreamWriter sw = new StreamWriter(pathAcc))
                foreach (var account in accounts)
                {
                    string tempAcc = String.Format("{0}#{1}#{2}#{3}#{4}",
                                            account.Passport,
                                            account.AccountNumber,
                                            account.Deposit,
                                            account.AccountAmount,
                                            account.AccountStatus);

                    sw.WriteLine($"{tempAcc}");
                }

            MessageBox.Show(
                    "Запись сохранена",
                    this.Title,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                    );

            journalLog?.Invoke("Записи сохранены");
        }

        private void MngAdd_Click(object sender, RoutedEventArgs e)
        {
            employees.Add(new Employee("", "", "", "", ""));
            mngDataGrid.ItemsSource = employees;
            journalLog?.Invoke("Добавлен новый клиент");
        }

        private void MngDel_Click(object sender, RoutedEventArgs e)
        {
            employees.RemoveAt(mngDataGrid.SelectedIndex);
        }

        private void MngDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FindAcc();
            closeAcc.IsEnabled = false;
            addAmount.IsEnabled = false;
            delAmount.IsEnabled = false;
        }

        private bool FindTypeNonDep(Accounts arg)
        {
            return (arg.Deposit == "False" && arg.AccountStatus == "True" && arg.Passport == (mngDataGrid.SelectedItem as Employee).PassportEmpl);
        }

        private bool FindTypeDep(Accounts arg)
        {
            return (arg.Deposit == "True" && arg.AccountStatus == "True" && arg.Passport == (mngDataGrid.SelectedItem as Employee).PassportEmpl);
        }

        private bool Find(Accounts arg)
        {
            return arg.Passport == (mngDataGrid.SelectedItem as Employee).PassportEmpl;
        }
        private void FindAcc()
        {
            var findAccounts = accounts.Where(Find);
            accountsManagement.ItemsSource = findAccounts;
            toAmount.ItemsSource = accounts;
            fromAmount.ItemsSource = accounts;

        }

        private void OpenAcc_Click(object sender, RoutedEventArgs e)
        {
            if (accounts.Any(FindTypeNonDep))
            {
                MessageBox.Show(
                    "У данного клиента уже есть недепозитный счет",
                    this.Title,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                    );
            }
            else
            {
                r = new Random();
                long rand = 10 * r.Next(10_000_000, 100_000_000);
                accounts.Add(new Accounts((mngDataGrid.SelectedItem as Employee).PassportEmpl, rand, false, 0, true));
                FindAcc();
                journalLog?.Invoke($"Добавлен новый счет: {rand}, статус депозита: {false}, баланс на счете:{0}, статус счета: {true}");
            }
            
        }

        private void AccountsManagement_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(pathAcc))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] args = sr.ReadLine().Split('#');

                        accounts.Add(new Accounts(args[0], Convert.ToDouble(args[1]), Convert.ToBoolean(args[2]), Convert.ToDouble(args[3]), Convert.ToBoolean(args[4])));

                        accountsManagement.ItemsSource = accounts;
                    }
                }
            }
        }

        private void CloseAcc_Click(object sender, RoutedEventArgs e)
        {
            foreach (var value in accounts)
            {
                if (value.AccountNumber == (accountsManagement.SelectedItem as Accounts).AccountNumber)
                {
                    closeAcc.IsEnabled = true;
                    if (value.Deposit == Convert.ToString(true))
                    {
                        MessageBox.Show(
                    "Депозитный счет",
                    this.Title,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                    );
                        break;
                    }
                    else
                    {
                        journalLog?.Invoke($"Счет номер {value.AccountNumber} закрыт");

                        value.AccountStatus = Convert.ToString(false);
                        value.AccountAmount = "0";
                        
                        valAcc.Text = defaultText;
                    }
                }
            }
            FindAcc();
        }

        private void AddAmount_Click(object sender, RoutedEventArgs e)
        {
            foreach(var value in accounts)
            {
                if(value.AccountNumber == (accountsManagement.SelectedItem as Accounts).AccountNumber)
                {
                    journalLog?.Invoke($"На счет номер {value.AccountNumber} положили {valAcc.Text}$");

                    value.AccountAmount = BankMath.SumValue(value.AccountAmount, valAcc.Text);

                    FindAcc();
                    valAcc.Text = defaultText;
                }
                
            }
        }

        private void ValAcc_GotFocus(object sender, RoutedEventArgs e)
        {
            valAcc.Text = valAcc.Text == defaultText ? string.Empty : valAcc.Text;
        }

        private void ValAcc_LostFocus(object sender, RoutedEventArgs e)
        {
            valAcc.Text = valAcc.Text == string.Empty ? defaultText : valAcc.Text;
        }

        private void DelAmount_Click(object sender, RoutedEventArgs e)
        {
            foreach (var value in accounts)
            {
                if (value.AccountNumber == (accountsManagement.SelectedItem as Accounts).AccountNumber)
                {
                    if(Convert.ToDouble(value.AccountAmount) <= 0)
                    {
                        MessageBox.Show(
                    "Нулевой баланс",
                    this.Title,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                    );
                        break;
                    }
                    else if(value.Deposit == Convert.ToString(true))
                    {
                        MessageBox.Show(
                    "Депозитный счет",
                    this.Title,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                    );
                        break;
                    }
                    else
                    {
                        journalLog?.Invoke($"Со счета номер {value.AccountNumber} сняли {valAcc.Text}$");

                        value.AccountAmount = BankMath.DelValue(value.AccountAmount, valAcc.Text);
                        FindAcc();
                        valAcc.Text = defaultText;
                    }
                }
            }
        }

        private void FromAmount_Loaded(object sender, RoutedEventArgs e)
        {
            fromAmount.ItemsSource = accounts;
        }

        private void ToAmount_Loaded(object sender, RoutedEventArgs e)
        {
            toAmount.ItemsSource = accounts;
        }

        private void MoneyTransfer_Click(object sender, RoutedEventArgs e)
        {
            foreach (var value in accounts)
            {
                if (value.AccountNumber == (fromAmount.SelectedItem as Accounts).AccountNumber)
                {
                    foreach (var valueTo in accounts)
                    {
                        if (valueTo.AccountNumber == (toAmount.SelectedItem as Accounts).AccountNumber)
                        {
                            if (value.Deposit == Convert.ToString(true))
                            {
                                MessageBox.Show(
                            "Депозитный счет",
                            this.Title,
                            MessageBoxButton.OK,
                            MessageBoxImage.Information
                            );
                                break;
                            }
                            else if (value.AccountAmount == "0")
                            {
                                MessageBox.Show(
                    "Нулевой баланс",
                    this.Title,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                    );
                                break;
                            }
                            else
                            {
                                journalLog?.Invoke($"Со счета номер {value.AccountNumber} перевели на счет {valueTo.AccountNumber}  {valAcc.Text}$");

                                value.AccountAmount = BankMath.DelValue(value.AccountAmount, valAcc.Text);
                                valueTo.AccountAmount = BankMath.SumValue(valueTo.AccountAmount, valAcc.Text);
                                FindAcc();

                            }
                        }
                    }
                }
            }
            valAcc.Text = defaultText;
            fromAmount.SelectedIndex = -1;
            toAmount.SelectedIndex = -1;
        }

        private void Deposit_Click(object sender, RoutedEventArgs e)
        {
            if (accounts.Any(FindTypeDep))
            {
                MessageBox.Show(
                "У данного клиента уже есть депозитный счет",
                this.Title,
                MessageBoxButton.OK,
                MessageBoxImage.Information
                );

            }
            else
            {
                r = new Random();
                long rand = 10 * r.Next(10_000_000, 100_000_000);
                accounts.Add(new Accounts((mngDataGrid.SelectedItem as Employee).PassportEmpl, rand, true, 10000, true));
                journalLog?.Invoke($"Добавлен новый счет: {rand}, статус депозита: {true}, баланс на счете:{10000}, статус счета: {true}");

                FindAcc();
            }
        }

        private void AccountsManagement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            closeAcc.IsEnabled = true;
            addAmount.IsEnabled = true;
            delAmount.IsEnabled = true;
        }

        private void MngDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Employee change = e.Row.Item as Employee;
                journalLog?.Invoke($"Внесены измененения - {change.LastName} {change.FirstName} {change.MiddleName} {change.NumberPhone} {change.PassportEmpl}");
            }
        }

        private void ValAcc_TextChanged(object sender, TextChangedEventArgs e)
        {
            ErrorMethod.Method(valAcc.Text);
        }
    }
}
