using auth_and_other;
class Program
{
    static User CurrentUser { get; set; }
    static void Main()
    {
        bool flag = true;
        while (flag)
        {
            Console.WriteLine("Войти(Q)\nЗарегистрироваться(W)\nИзменить пароль(E)\n" +
                "Вывести секретное сообщение(A)\nУдалить пользователя(R)\nВыйти(S)");
            var inputValue = Console.ReadKey();
            switch (inputValue.Key)
            {
                case ConsoleKey.Q:
                    Console.Clear();
                    Autorization();
                    break;
                case ConsoleKey.W:
                    Console.Clear();
                    Registration();
                    break;
                case ConsoleKey.E:
                    Console.Clear();
                    UpdPassword();
                    break;
                case ConsoleKey.A:
                    Console.Clear();
                    OutputMessage();
                    break;
                case ConsoleKey.R:
                    Console.Clear();
                    RemoveUser();
                    break;
                case ConsoleKey.S:
                    Console.Clear();
                    LogOut();
                    break;
            }
        }
    }

    static User Input(bool check)
    {
        //check == true - проверять на длину
        var flag = true;
        string password = "";
        Console.WriteLine("Введите логин...");
        var username = Console.ReadLine();
        while (flag)
        {
            Console.WriteLine("Введите пароль...");
            password = Console.ReadLine();
            if (check && password.Length < 6)
            {
                Console.WriteLine("Пароль должен быть не меньше 6 символов!");
                continue;
            }
            flag = false;
        }
        return new User()
        {
            Username = username,
            Password = password,
        };
    }
    static bool OutputMessage()
    {
        if (!IsLogIn())
        {
            Console.WriteLine("Чтобы увидеть секретное сообщение нужно авторизоваться");
            return false;
        }
        Console.WriteLine("Hello world!");
        return true;
    }

    static void CreateUser(User user)
    {
        using var db = new AppDBContext();
        db.Users.Add(user);
        CurrentUser = user;
        db.SaveChanges();
    }

    static bool IsLogIn()
    {
        return CurrentUser != null;
    }
    static void Registration()
    {
        var user = new User();
        while (true)
        {
            user = Input(true);
            using var db = new AppDBContext();
            var entity = db.Users.FirstOrDefault(u => u.Username == user.Username);
            if (entity == null)
            {
                CreateUser(user);
                Console.WriteLine("Регистрация прошла успешно!");
                break;
            }
            Console.WriteLine("Такой пользователь уже существует!");
            Console.WriteLine();
        }
    }

    static User? Autorization()
    {
        if (IsLogIn())
        {
            Console.WriteLine("Вы уже вошли в систему!");
            return null;
        }
        while (true)
        {
            var user = Input(false);
            CurrentUser = user;
            using var db = new AppDBContext();
            var entity = db.Users.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);
            if (entity != null)
            {
                Console.WriteLine("Вы вошли в систему");
                return entity;
            }
            Console.WriteLine("Пароль или логин введен неправильно");
        }
    }

    static void LogOut()
    {
        if (!IsLogIn())
        {
            Console.WriteLine("Вы еще не вошли в учетную запись!");
            return;
        }
        CurrentUser = null;
        Console.WriteLine("Вы вышли из учетной записи");
    }
    static void UpdPassword()
    {
        var user = Autorization();
        Console.WriteLine("Введите новый пароль...");
        var passwrd = Console.ReadLine();
        Console.WriteLine("Повторите пароль...");
        var repeatedPass = Console.ReadLine();
        if (passwrd != repeatedPass)
        {
            throw new Exception("Пароли не совпадают");
        }
        using var db = new AppDBContext();
        user.Password = passwrd;
        db.Users.Update(user);
        db.SaveChanges();
    }

    static void RemoveUser()
    {
        var user = Autorization();
        using var db = new AppDBContext();
        db.Users.Remove(user);
        db.SaveChanges();
        Console.WriteLine("Вы удалили свою учетную запись");
    }
}
