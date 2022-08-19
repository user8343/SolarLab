namespace birthDates.Person;
public class Person
{
    private String FirstName = "";
    private String LastName = "";
    private DateOnly BirthDay;
    private int daysBeforeBirthday;
    private int yearsOld;

    public void UpdateInfo(String FirstName,String LastName, DateOnly BirthDay){
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.BirthDay = BirthDay;
        CalculateDaysBeforeBirthday();
    }

    public String GetFirstName(){
        return FirstName;
    }
    public int GetDaysBeforeBirthday(){
        return daysBeforeBirthday;
    }
    public String GetLastName(){
        return LastName;
    }
    public Person(String FirstName,String LastName, DateOnly BirthDay){
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.BirthDay = BirthDay;
        CalculateDaysBeforeBirthday();
    }

    public void CalculateDaysBeforeBirthday(){
        DateTime nowDate = DateTime.Now.Date;

        if((nowDate.Month > BirthDay.Month) || ((nowDate.Month == BirthDay.Month)&&(nowDate.Day > BirthDay.Day))){
            TimeSpan delta = new DateTime(nowDate.Year+1,BirthDay.Month,BirthDay.Day) - nowDate;
            daysBeforeBirthday = delta.Days;
            yearsOld = nowDate.Year+1 - BirthDay.Year;
        }else{
            TimeSpan delta = new DateTime(nowDate.Year,BirthDay.Month,BirthDay.Day) - nowDate;
            daysBeforeBirthday = delta.Days;
            yearsOld = nowDate.Year - BirthDay.Year;
        }
    }

    public void FullPrint(){
        Console.WriteLine("|{0,20}|{1,20}|{2:d2}.{3:d2}.{4:d4}|{5,10}|{6,11}|",FirstName,LastName,BirthDay.Day,BirthDay.Month,BirthDay.Year,(daysBeforeBirthday!=0)?daysBeforeBirthday.ToString():"Сегодня",yearsOld.ToString());
    }

    public void ShortPrint(){
        Console.WriteLine("|{0,20}|{1,20}|{2,10}|{3,11}|",FirstName,LastName,(daysBeforeBirthday!=0)?daysBeforeBirthday.ToString():"Сегодня",yearsOld.ToString());
    }

    public String GetInfo(){
        return string.Format("'{0}', '{1}', '{2:d4}-{3:d2}-{4:d2}'",FirstName,LastName,BirthDay.Year,BirthDay.Month,BirthDay.Day);
    }
    
}
