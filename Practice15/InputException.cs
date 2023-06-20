using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Practice13
{
    public class InputException:Exception
    {
        public InputException(string Msg, int Code) : base(Msg)
        {
            ErrorCode = Code;
        }
        public int ErrorCode { get; set; }
    }

    public class ErrorMethod
    {
        public static event Action<string> journalLog;
        public static void Method(string msg)
        {
            string Title = "Ошибка";
            try
            {
                long result;
                if (msg == "Введите сумму....") throw new InputException("Обратите внимание на поле <Введите сумму...> для пополнения счета, снятия, перевода",1);
                if (String.IsNullOrEmpty(msg)) throw new InputException("Поле не должно оставаться пустым или отмените ввод",2);
                if (!long.TryParse(msg, out result)) throw new InputException("Введите число",3);
            }
            catch (InputException e) when (e.ErrorCode == 1)
            {
                return;

            }
            catch (InputException e) when (e.ErrorCode == 2)
            {
                return;
            }
            catch (InputException e) when (e.ErrorCode == 3)
            {
                MessageBox.Show(
                    $"{e.Message}",
                    Title,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                    );
            }
        }
    }
}
