namespace Interface;

public interface ICountries
{

    int? CountryCount();
    string GetCountry();
    string GetCountryCode();
    string GetContinent();
    string GetReligion();
    string GetLanguage();
    string GetPopulation();

    void SetCountry(string data);
    //void SetCountryCode(string data);
    //void SetContinent(string data);
    //void SetReligion(string data);
    //void SetLanguage(string data);
    //void SetPopulation(string data);

}

public interface IMechanics
{
    bool LoadDataFile();
    bool ReadData();
    bool LoadInToDatabase();
    string AddNew();
    string ViewCountry();
    bool DeleteCountry();
    bool DeleteCountryInfo();

}



