using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Library.Models
{
  public class Patron
  {
    private string _patron_name;
    private string _address;
    private string _phone;
    private int _id;

    public Patron(string patron_name, string address, string phone, int id = 0)
    {
      _patron_name = patron_name;
      _address = address;
      _phone = phone;
      _id = id;
    }

    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM patrons;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public string GetPatronName()
    {
      return _patron_name;
    }

    public void SetPatronName(string newPatron_name)
    {
      _patron_name = newPatron_name;
    }

    public string GetPatronAddress()
    {
      return _address;
    }

    public void SetPatronAddress(string newAddress)
    {
      _address = newAddress;
    }

    public string GetPatronPhone()
    {
      return _phone;
    }

    public void SetPatronPhone(string newPhone)
    {
      _phone = newPhone;
    }
    public int GetPatronId()
    {
      return _id;
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO patrons (patron_name, address, phone) VALUES (@patron_name, @address, @phone);";
      MySqlParameter patron_name = new MySqlParameter();
      patron_name.ParameterName = "patron_name";
      patron_name.Value = this._patron_name;
      cmd.Parameters.Add(patron_name);
      MySqlParameter address = new MySqlParameter();
      address.ParameterName = "address";
      address.Value = this._address;
      cmd.Parameters.Add(address);
      MySqlParameter phone = new MySqlParameter();
      phone.ParameterName = "phone";
      phone.Value = this._phone;
      cmd.Parameters.Add(phone);
      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      if ( conn != null )
      {
        conn.Dispose();
      }
    }

    public static List<Patron> GetAll()
    {
      List<Patron> allPatrons = new List<Patron> { };
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM patrons;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        string patron_name = rdr.GetString(0);
        string address = rdr.GetString(1);
        string phone = rdr.GetString(2);
        int id = rdr.GetInt32(3);
        Patron newPatron = new Patron(patron_name, address, phone, id);
        allPatrons.Add(newPatron);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allPatrons;
    }

    public override bool Equals(System.Object otherPatron)
    {
      if (!(otherPatron is Patron))
      {
        return false;
      }
      else
      {
        Patron newPatron = (Patron) otherPatron;
        bool patronNameEquality = (this.GetPatronName() == newPatron.GetPatronName());
        bool patronAddressEquality = (this.GetPatronAddress() == newPatron.GetPatronAddress());
        bool patronPhoneEquality = (this.GetPatronPhone() == newPatron.GetPatronPhone());
        bool PatronIdEquality = (this.GetPatronId() == newPatron.GetPatronId());
        return (patronNameEquality && patronAddressEquality && patronPhoneEquality && PatronIdEquality);
      }
    }


    public static Patron Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM patrons WHERE id = (@searchId);";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int patronId = 0;
      string patron_name = "";
      string address = "";
      string phone = "";
      while(rdr.Read())
      {
        patron_name = rdr.GetString(0);
        address = rdr.GetString(1);
        phone = rdr.GetString(2);
        patronId = rdr.GetInt32(3);
      }
      Patron newPatron = new Patron(patron_name, address, phone, patronId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newPatron;
    }


  }
}


    // public bool Delete()
    // {
    //   MySqlConnection conn = DB.Connection();
    //   conn.Open();
    //   var cmd = conn.CreateCommand() as MySqlCommand;
    //   cmd.CommandText = @"DELETE FROM patrons WHERE id = @patronId;";
    //   MySqlParameter patronIdParameter = new MySqlParameter();
    //   patronIdParameter.ParameterName = "@patronId";
    //   patronIdParameter.Value = this.GetPatronId();
    //   cmd.Parameters.Add(patronIdParameter);
    //   cmd.ExecuteNonQuery();
    //   conn.Close();
    //   if (conn != null)
    //   {
    //     conn.Dispose();
    //   }
    //   return true;
    // }