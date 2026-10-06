namespace Interface;

class Program
{
    static void Main(string[] args)
    {

        var countries = LoadCountry();
        foreach (var entry in countries)
        {
            Console.Write(entry.ToString() + "\n");

        }



    }

    public static Dictionary<string, Country> LoadCountry()
    {

        string line = null;
        Dictionary<string, Country> ListOfCountries = new Dictionary<string, Country>();

        string path = "./data.csv";

        System.IO.StreamReader reader;
        reader = new System.IO.StreamReader(path);

        while ((line = reader.ReadLine()) != null)
        {

            string[] Part = line.Split(",");
            string _name = Part[0];

            Country NewCountry = new Country(Part[0], Part[1], Part[2], Part[3], Part[4], Part[5]);

            ListOfCountries[_name] = NewCountry;

        }
        return ListOfCountries;


    }
}





public class Mechanics
{
    public bool LoadDataFile()
    {

        string path = "./data.csv";

        System.IO.StreamReader reader;
        reader = new System.IO.StreamReader(path);

        return false;
    }

    public bool ReadData()
    {

        return false;

    }

    public bool LoadInToDatabase() { return false; }

    public string AddNew()
    {


        return null;
    }

    public string NewCountryInput()
    {
        string[] TemporaryArray = new string[6];
        for (int i = 0; i < 6; i++)
        {
            //  string text = Console.Read();
            //TemporaryArray[i] = text;
        }

        for (int i = 0; i < TemporaryArray.Length; i++)
        {



        }
        string result = "unknown";
        return result;
    }
    public string ViewCountry() { return null; }
    public bool DeleteCountry() { return false; }
    public bool DelteCountryInfo() { return false; }

}


public class Country : ICountries
{
    private static int _nextID = 1;
    private int? _idNumber;
    private string? _country;
    private string? _countrycode;
    private string? _religion;
    private string? _language;
    private string? _continent;
    private string? _population;




    public Country(string country, string religion, string continent, string language, string countrycode, string population)
    {

        this._idNumber = _nextID;
        _nextID++;
        this._country = country;
        this._countrycode = countrycode;
        this._religion = religion;
        this._continent = continent;
        this._language = language;
        this._population = population;


    }



    public int? CountryCount()
    {
        return _idNumber;

    }


    public void SetCountry(string data)
    {
        _country = data;
    }







    public string GetCountry()
    {
        return _country;
    }

    public string GetReligion()
    {

        return _religion;

    }

    public string GetContinent()
    {

        return _continent;


    }

    public string GetLanguage()
    {
        return _language;
    }

    public string GetCountryCode()
    {

        return _countrycode;

    }

    public string GetPopulation()
    {

        return _population;
    }

    //Helper methods
    public override string ToString()
    {
        // This defines what the "Police Report" looks like for this object
        return $"{_country}, Language: {_language}, Country Code({_countrycode}), Religion {_religion}, Continent {_continent} Population: {_population}";
    }




}
