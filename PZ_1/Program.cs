namespace PZ_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Результат работы метода Where с доп. методом:");
            LinqWithArrayOfString();

            Console.WriteLine("\nРезультат работы метода Where с лямбдой:");
            LinqWithArrayOfStringWithLambda();

            Console.WriteLine("\nФильтрация по типу");
            LinqWithArrayOfExceptions();

            Console.WriteLine("\nВыбранные методы из таблицы");
            LinqWithArrayOfStringWithTheSelectedMethod();
            LinqWithArrayOfStringWithDistinct();
        }

        //Метод Where с дополнительным методом
        static void LinqWithArrayOfString()
        {
            var names = new string[] { "Иван", "Сергей", "Михаил", "Георгий", "Даниил"};

            var query = names.Where(NameLongerThanFour);

            foreach(string item in query)
            {
                Console.WriteLine(item);
            }
        }

        static bool NameLongerThanFour(string name)
        {
            return name.Length > 4;
        }

        //Метод Where с лямбдой + OrderBy + ThenBy
        static void LinqWithArrayOfStringWithLambda()
        {
            var names = new string[] { "Иван", "Сергей", "Михаил", "Георгий", "Даниил" };

            var query = names
                .Where(n => n.Length > 4)
                .OrderByDescending(n => n.Length)
                .ThenBy(n => n);

            foreach (string item in query)
            {
                Console.WriteLine(item);
            }
        }

        //Разбор методов из таблицы
        static void LinqWithArrayOfStringWithTheSelectedMethod()
        {
            var names1 = new string[] { "Иван", "Сергей", "Михаил", "Георгий", "Даниил" };

            var names2 = new string[] { "Дмитрий", "Сергей", "Андрей", "Георгий", "Даниил" };

            var result1 = names1.Except(names2);    //В данном случае из массива name 1 убираются все элементы, которые есть в массиве names2
            var result2 = names2.Intersect(names1); //В данном случае выводятся только те элементы, которые есть и в name1 и в name2
            var result3 = names1.Union(names2);     //Здесь результат - новый набор, в котором имеются элементы, как из первого, так и из второго массива.
                                                    //Повторяющиеся элементы добавляются в результат только один раз

            Console.WriteLine("\nМетод Except:");
            foreach (string item in result1)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("\nМетод Intersect:");

            foreach(string s in result2)
            {
                Console.WriteLine(s);
            }

            Console.WriteLine("\nМетод Union:");

            foreach (string n in result3)
            {
                Console.WriteLine(n);
            }
        }

        static void LinqWithArrayOfStringWithDistinct()
        {
            var names = new string[] { "Иван", "Сергей", "Михаил", "Георгий", "Даниил", "Иван", "Михаил" };

            var query = names.Distinct();//Данный метод убирает из массива повторяющиеся элементы

            Console.WriteLine("\nМетод Distinct:");
            foreach (string item in query)
            {
                Console.WriteLine(item);
            }
        }

        //Фильтрация по типу
        static void LinqWithArrayOfExceptions()
        {
            var errors = new Exception[]
            {
                new ArgumentException(),
                new SystemException(),
                new InvalidOperationException(),
                new NullReferenceException(),
                new InvalidCastException(),
                new OverflowException(),
                new DivideByZeroException(),
                new ApplicationException()
            };

            var numbersErrors = errors.OfType<ArgumentException>();

            foreach(var error in numbersErrors)
            {
                Console.WriteLine(error);
            }
        }
    }
}