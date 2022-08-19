using System;
using System.Data.SQLite;

namespace birthDates.birthDates{
    class Program{
        private static SQLiteConnection db_Conn;
        private static SQLiteCommand sql;
        private static List<Person.Person> Persons = new List<Person.Person>();
        private static bool ConnectDB(String dbFileName){
            if (!File.Exists(dbFileName))
                SQLiteConnection.CreateFile(dbFileName);

            try{
                db_Conn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
                db_Conn.Open();

                String sql_text = "CREATE TABLE IF NOT EXISTS Employees (id INTEGER PRIMARY KEY AUTOINCREMENT, FirstName VARCHAR(20), LastName VARCHAR(20), BirthDay DATE)";
                sql = new SQLiteCommand(sql_text, db_Conn);
                sql.ExecuteNonQuery();

                return true;
            }catch(Exception es){
                Console.WriteLine(es.Message);
                return false;
            }
        }

        static void sortPerson(){
            Persons.Sort(delegate(Person.Person p1, Person.Person p2){
                if(p1.GetDaysBeforeBirthday() < p2.GetDaysBeforeBirthday()) return -1;
                else if(p1.GetDaysBeforeBirthday() > p2.GetDaysBeforeBirthday()) return 1;
                else return 0;
            });
        }

        static void AddPerson(){
            Console.Clear();
            Console.WriteLine("==========Добавление сотрудника в список ДР=========");
            
            Console.WriteLine("    Фамилия:");          String LN = Console.ReadLine();
            Console.WriteLine("    Имя:");              String FN = Console.ReadLine();
            Console.WriteLine("    Дата Рождения(ДД.ММ.ГГГГ):");    
            try{
                DateOnly BirthDay = DateOnly.Parse(Console.ReadLine());
                Persons.Add(new Person.Person(FN,LN,BirthDay));
            }catch(Exception es){
                Console.WriteLine(es);
                Console.WriteLine("Нажмите ENTER для продолжения...");
                Console.ReadLine();
            }
        }

        static void SaveDB(){
            Console.Clear();
            sql.CommandText = "DELETE FROM Employees";
            sql.ExecuteNonQuery();
            foreach(Person.Person p in Persons){
                sql.CommandText = "INSERT INTO Employees (FirstName, LastName, BirthDay) VALUES ("+p.GetInfo()+")";
                sql.ExecuteNonQuery();
            }
        }

        static void LoadDB(){
            Console.Clear();
            sql.CommandText = "SELECT * FROM Employees";
            SQLiteDataReader read = sql.ExecuteReader();
            while(read.Read()){
                String FN = read.GetValue(1).ToString();
                String LN = read.GetValue(2).ToString();
                DateOnly BD = DateOnly.Parse(read.GetValue(3).ToString().Substring(0,10));
                Persons.Add(new Person.Person(FN,LN,BD));
            }
            read.Close();
        }

        static void UpdatePerson(){
            Console.Clear();
            Console.WriteLine("==========Обновление информации=========");
            Console.WriteLine("Поиск сотрудника:");
            Console.WriteLine("    Фамилия:");          String LN = Console.ReadLine();
            Console.WriteLine("    Имя:");              String FN = Console.ReadLine();
            bool correct = false;
            for(int i = 0; i<Persons.Count; i++){
                if ((Persons[i].GetFirstName() == FN)&&(Persons[i].GetLastName() == LN)){
                    Console.WriteLine("Обновление информации:");
                    Console.WriteLine("    Фамилия:");                      LN = Console.ReadLine();
                    Console.WriteLine("    Имя:");                          FN = Console.ReadLine();
                    Console.WriteLine("    Дата Рождения(ДД.ММ.ГГГГ):");    

                    try{
                        DateOnly BirthDay = DateOnly.Parse(Console.ReadLine());
                        Persons[i].UpdateInfo(FN,LN,BirthDay);
                    }catch(Exception es){
                        Console.WriteLine(es);
                        Console.WriteLine("Нажмите ENTER для продолжения...");
                        Console.ReadLine();
                    }
                    correct = true;
                    break;
                }
            }
            if (!correct){
                Console.WriteLine("Сотрудник не найден");
                Console.WriteLine("Нажмите ENTER для продолжения...");
                Console.ReadLine();
            }
        }

        static void DeletePerson(){
            Console.Clear();
            Console.WriteLine("==========Удаление информации=========");
            Console.WriteLine("Поиск сотрудника:");
            Console.WriteLine("    Фамилия:");          String LN = Console.ReadLine();
            Console.WriteLine("    Имя:");              String FN = Console.ReadLine();
            bool correct = false;
            for(int i = 0; i<Persons.Count; i++){
                if ((Persons[i].GetFirstName() == FN)&&(Persons[i].GetLastName() == LN)){
                    Persons.Remove(Persons[i]);
                    Console.WriteLine("Удалено");
                    correct = true;
                    break;
                }
            }
            if (!correct){
                Console.WriteLine("Сотрудник не найден");  
            }
            Console.WriteLine("Нажмите ENTER для продолжения...");
            Console.ReadLine();
        }

        static void FullPrintPerson(){
            Console.Clear();
            sortPerson();
            Console.WriteLine(".=[Полный список сотрудников]===============================================.");
            Console.WriteLine("|         ИМЯ        |       ФАМИЛИЯ      |    ДР    |дней до ДР|исполняется|");
            Console.WriteLine("+--------------------+--------------------+----------+----------+-----------|");
            for(int i = 0; i<Persons.Count; i++) Persons[i].FullPrint();
            Console.WriteLine("Нажмите ENTER для продолжения...");
            Console.ReadLine();
        }
        static void ShortPrintPerson(){
            Console.Clear();
            sortPerson();
            Console.WriteLine(".=[Дни рождения в длижайшие 10 дней]=============================.");
            Console.WriteLine("|         ИМЯ        |       ФАМИЛИЯ      |дней до ДР|исполняется|");
            Console.WriteLine("+--------------------+--------------------+----------+-----------|");
            for(int i = 0; i<Persons.Count; i++){
                if (Persons[i].GetDaysBeforeBirthday()<=10) Persons[i].ShortPrint();
                else break;
            }
            Console.WriteLine("Нажмите ENTER для продолжения...");
            Console.ReadLine();
        }

        static void Main(string[] args){

            if (!ConnectDB("Employees.db")){
                Console.WriteLine("Ошибка подключения");
                Console.Read();
            }else{
                LoadDB();
                ShortPrintPerson();
                bool exitProgram = false;
                while (!exitProgram){
                    Console.Clear();
                    Console.WriteLine("====================Поздравлятор====================");
                    Console.WriteLine("   1. Отображение всего списка ДР");
                    Console.WriteLine("   2. Отображение списка сегодняшних и ближайших ДР");
                    Console.WriteLine("   3. Добавление сотрудника в список ДР");
                    Console.WriteLine("   4. Редактирование информации о сотруднике");
                    Console.WriteLine("   5. Удаление сотрудника из списка ДР");
                    Console.WriteLine("   6. Сохранить информацию в БД");
                    Console.WriteLine("   7. Загрузить информацию из БД");
                    Console.WriteLine("   0. Выход");

                    int key = ((int)Console.ReadKey().Key)-48;
                    switch(key){
                        case 0:
                            exitProgram = true;
                            break;
                        case 1:
                            FullPrintPerson();
                            break;
                        case 2:
                            ShortPrintPerson();
                            break;
                        case 3:
                            AddPerson();
                            break;
                        case 4:
                            UpdatePerson();
                            break;
                        case 5:
                            DeletePerson();
                            break;
                        case 6:
                            SaveDB();
                            break;
                        case 7:
                            LoadDB();
                            break;
                    }
                }
            }
        }
    }
}